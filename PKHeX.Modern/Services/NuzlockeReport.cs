using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PKHeX.Core;

namespace PKHeX.Modern.Services;

/// <summary>Situação de um Pokémon no relatório de Nuzlocke.</summary>
public enum NuzlockeStatus { Party, Alive, Dead }

/// <summary>Um Pokémon capturado pelo próprio treinador neste jogo (equipe ou caixas).</summary>
public sealed record NuzlockeEntry(PKM Pkm, int Box, int Slot, string Where, string Route, DateOnly? Date, int Order,
    NuzlockeStatus Status, ushort Family, bool Hatched)
{
    public string Name => Pkm.IsNicknamed ? $"{Pkm.Nickname} ({CoreAdapter.SpeciesNames[Pkm.Species]})" : CoreAdapter.SpeciesNames[Pkm.Species];
}

/// <summary>Uma rota: o primeiro encontro (o que vale na regra) e as capturas a mais no mesmo lugar.</summary>
public sealed record NuzlockeRoute(string Route, NuzlockeEntry First, IReadOnlyList<NuzlockeEntry> Extras, bool IsDupe, string? DupeOf);

public sealed record NuzlockeResult(IReadOnlyList<NuzlockeRoute> Routes, IReadOnlyList<NuzlockeEntry> Outsiders, int Eggs,
    bool HasDates, string? Unsupported)
{
    public int Alive => Routes.Count(r => r.First.Status != NuzlockeStatus.Dead);
    public int InParty => Routes.Count(r => r.First.Status == NuzlockeStatus.Party);
    public int Dead => Routes.Count(r => r.First.Status == NuzlockeStatus.Dead);
    public int Dupes => Routes.Count(r => r.IsDupe);
    public int Extras => Routes.Sum(r => r.Extras.Count);
}

/// <summary>
/// Relatório de Nuzlocke do save aberto: o primeiro encontro de cada rota (pelo local de captura que o jogo guarda),
/// as capturas a mais na mesma rota, os repetidos (família já capturada numa rota anterior, a "dupes clause") e quem
/// está vivo ou morto. Morto = guardado na caixa escolhida como cemitério. Só conta Pokémon do próprio treinador e
/// deste jogo; os vindos de trocas ou de outros jogos ficam numa lista à parte. Só lê o save.
/// </summary>
public static class NuzlockeReport
{
    /// <summary>Palavras que indicam a caixa do cemitério pelo nome (sugestão inicial).</summary>
    private static readonly string[] GraveyardNames = ["cemit", "cement", "grave", "dead", "morto", "morte", "muert", "box of death", "tumba"];

    /// <summary>Caixa cujo nome parece de cemitério, ou -1.</summary>
    public static int GuessGraveyard(SaveFile sav)
    {
        if (sav is not IBoxDetailNameRead names)
            return -1;
        for (int b = 0; b < sav.BoxCount; b++)
        {
            string name;
            try { name = names.GetBoxName(b); } catch { continue; }
            if (GraveyardNames.Any(g => name.Contains(g, StringComparison.OrdinalIgnoreCase))
                || name.Split(' ', '-', '_', '.').Any(w => w.Equals("RIP", StringComparison.OrdinalIgnoreCase))) // "RIP" só como palavra (não "Tripla")
                return b;
        }
        return -1;
    }

    public static NuzlockeResult Build(SaveFile sav, int graveyardBox)
    {
        if (sav.Generation <= 1)
            return new([], [], 0, false, "A Gen 1 não guarda onde cada Pokémon foi capturado, então não dá para montar o relatório.");

        var own = new List<NuzlockeEntry>();
        var outsiders = new List<NuzlockeEntry>();
        int eggs = 0, order = 0;
        EvolutionTree? tree = null;
        try { tree = EvolutionTree.GetEvolutionTree(sav.Context); } catch { /* sem arvore: familia = especie */ }

        void Add(PKM pk, int box, int slot, string where)
        {
            if (CoreAdapter.IsEmpty(pk))
                return;
            if (pk.IsEgg)
            {
                eggs++;
                return;
            }
            var status = box < 0 ? NuzlockeStatus.Party : box == graveyardBox ? NuzlockeStatus.Dead : NuzlockeStatus.Alive;
            ushort family = pk.Species;
            if (tree is not null)
                try { family = tree.GetBaseSpeciesForm(pk.Species, pk.Form).Species; } catch { /* fica a especie */ }
            var entry = new NuzlockeEntry(pk, box, slot, where, RouteName(pk), pk.MetDate, order++, status, family, pk.WasEgg);
            (IsFromThisGame(sav, pk) ? own : outsiders).Add(entry);
        }

        if (sav.HasParty)
            for (int i = 0; i < sav.PartyCount; i++)
                Add(sav.GetPartySlotAtIndex(i), -1, i, $"Equipe · {i + 1}");
        var boxNames = sav as IBoxDetailNameRead;
        for (int b = 0; b < sav.BoxCount; b++)
        {
            string boxName;
            try { boxName = boxNames?.GetBoxName(b) is { Length: > 0 } n ? n : $"Caixa {b + 1}"; } catch { boxName = $"Caixa {b + 1}"; }
            for (int s = 0; s < sav.BoxSlotCount; s++)
                Add(sav.GetBoxSlotAtIndex(b, s), b, s, $"{boxName} · {s + 1}");
        }

        bool hasDates = own.Count > 0 && own.All(e => e.Date is not null);
        if (sav.Generation == 2 && own.All(e => e.Pkm.MetLocation == 0))
            return new([], outsiders, eggs, false, "Este save da Gen 2 não guarda o local de captura (só o Crystal guarda), então não dá para montar o relatório.");

        // Ordem de captura: pela data quando o jogo guarda (Gen 4 em diante); sem data, a equipe e depois as caixas
        var chronological = own.OrderBy(e => e.Date ?? DateOnly.MaxValue).ThenBy(e => e.Order).ToList();
        var routes = new List<NuzlockeRoute>();
        var families = new Dictionary<ushort, string>();
        foreach (var group in chronological.GroupBy(e => e.Route))
        {
            var first = group.First();
            bool dupe = families.TryGetValue(first.Family, out var dupeOf);
            routes.Add(new NuzlockeRoute(group.Key, first, [.. group.Skip(1)], dupe, dupe ? dupeOf : null));
            if (!dupe)
                families[first.Family] = group.Key;
        }
        return new(routes, outsiders, eggs, hasDates, null);
    }

    /// <summary>Capturado pelo próprio treinador neste jogo (ou no par dele, na Gen 1-2 que não guarda a versão).</summary>
    private static bool IsFromThisGame(SaveFile sav, PKM pk)
    {
        try { return sav.IsFromTrainer(pk); }
        catch { return false; }
    }

    private static string RouteName(PKM pk)
    {
        try
        {
            var name = GameInfo.GetLocationName(false, pk.MetLocation, pk.Format, pk.Generation, pk.Version);
            return string.IsNullOrWhiteSpace(name) ? $"Local {pk.MetLocation}" : name;
        }
        catch
        {
            return $"Local {pk.MetLocation}";
        }
    }

    public static string StatusText(NuzlockeStatus status) => status switch
    {
        NuzlockeStatus.Party => "Na equipe",
        NuzlockeStatus.Dead => "Morto",
        _ => "Vivo",
    };

    /// <summary>Texto para colar em fórum ou rede social (uma linha por rota).</summary>
    public static string ToText(NuzlockeResult result, string game)
    {
        var sb = new StringBuilder();
        sb.AppendLine($"Nuzlocke · {game}");
        sb.AppendLine(Loc.T($"{result.Routes.Count} rotas · {result.Alive} vivos ({result.InParty} na equipe) · {result.Dead} mortos · {result.Dupes} repetidos"));
        sb.AppendLine();
        foreach (var r in result.Routes)
        {
            var e = r.First;
            sb.Append($"{r.Route}: {e.Name} {Loc.T($"Nv. {e.Pkm.CurrentLevel}")} · {Loc.T(StatusText(e.Status))}");
            if (r.IsDupe)
                sb.Append(" · " + Loc.T($"repetido ({r.DupeOf})"));
            if (r.Extras.Count > 0)
                sb.Append(" · " + Loc.T($"+{r.Extras.Count} a mais") + ": " + string.Join(", ", r.Extras.Select(x => x.Name)));
            sb.AppendLine();
        }
        return sb.ToString();
    }
}
