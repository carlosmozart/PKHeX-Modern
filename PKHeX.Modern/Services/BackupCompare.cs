using System.Collections.Generic;
using System.Linq;
using PKHeX.Core;

namespace PKHeX.Modern.Services;

/// <summary>
/// "Comparar" de um backup no Save Manager: o que o save atual tem a mais (e se perde ao restaurar) e o que só o
/// backup tem (e volta ao restaurar). Usa o resumo da linha do tempo (<see cref="SaveTimeline"/>: horas de jogo,
/// shinies, espécies, Pokémon, insígnias, Pokédex e dinheiro) e lista os Pokémon que só um dos dois tem.
/// </summary>
public static class BackupCompare
{
    /// <summary>Quantos nomes de Pokémon entram em cada lista antes do "e mais N".</summary>
    public const int MaxNames = 12;

    public sealed record Result(IReadOnlyList<string> OnlyCurrent, IReadOnlyList<string> OnlyBackup)
    {
        public bool Same => OnlyCurrent.Count == 0 && OnlyBackup.Count == 0;
    }

    public static Result Compare(SaveFile backup, SaveFile current)
    {
        var a = SaveTimeline.Capture(backup, "backup");
        var b = SaveTimeline.Capture(current);
        // Cada lado so com o que tem a mais: as linhas negativas ("−$8.000") sao o espelho do outro lado. No lado do
        // backup, especies e shinies novos ja aparecem na lista de Pokemon (com ★), que diz melhor o que volta.
        static bool Gain(string line) => !line.StartsWith('−') && !line.StartsWith('-');
        var onlyCurrent = SaveTimeline.Diff(a, b).Changes.Where(Gain).Select(Loc.T).ToList();
        var onlyBackup = SaveTimeline.Diff(b, a).Changes.Where(l => Gain(l) && !l.Contains("spécies novas") && !l.StartsWith('★')).Select(Loc.T).ToList();
        var before = Pokemon(backup); var after = Pokemon(current);
        AddPokemon(onlyCurrent, Missing(after, before), "Pokémon que o backup não tem: {0}");
        AddPokemon(onlyBackup, Missing(before, after), "Pokémon que só o backup tem: {0}");
        return new Result(onlyCurrent, onlyBackup);
    }

    /// <summary>
    /// Identidade de um Pokémon que não muda ao evoluir, subir de nível ou trocar de caixa: EC, PID e treinador
    /// (Gen 3 em diante) ou treinador e DVs (Gen 1 e 2, que não têm EC nem PID).
    /// </summary>
    public static string Key(PKM pk) => pk.Format <= 2
        ? $"{pk.TID16}:{pk.OriginalTrainerName}:{pk.IV_ATK}:{pk.IV_DEF}:{pk.IV_SPE}:{pk.IV_SPA}"
        : $"{pk.EncryptionConstant:X8}:{pk.PID:X8}:{pk.TID16}:{pk.SID16}";

    private static List<PKM> Pokemon(SaveFile sav)
    {
        try { return [.. EntitySearch.ReadAll(sav).Select(e => e.Pkm)]; }
        catch { return []; }
    }

    /// <summary>Os Pokémon de <paramref name="from"/> que faltam em <paramref name="other"/> (cópias iguais contam uma a uma).</summary>
    private static List<PKM> Missing(List<PKM> from, List<PKM> other)
    {
        var left = other.GroupBy(Key).ToDictionary(g => g.Key, g => g.Count());
        var missing = new List<PKM>();
        foreach (var pk in from)
        {
            var key = Key(pk);
            if (left.TryGetValue(key, out var n) && n > 0) left[key] = n - 1;
            else missing.Add(pk);
        }
        return missing;
    }

    private static void AddPokemon(List<string> lines, List<PKM> list, string pattern)
    {
        if (list.Count == 0)
            return;
        var names = list.Take(MaxNames).Select(Describe).ToList();
        var text = string.Join(", ", names) + (list.Count > MaxNames ? " " + Loc.T($"e mais {list.Count - MaxNames}") : "");
        lines.Add(string.Format(Loc.T(pattern), text));
    }

    private static string Describe(PKM pk)
    {
        var name = (uint)pk.Species < (uint)CoreAdapter.SpeciesNames.Count ? CoreAdapter.SpeciesNames[pk.Species] : $"#{pk.Species}";
        if (pk.IsEgg)
            return Loc.T($"Ovo de {name}");
        return (pk.IsShiny ? "★ " : "") + name + " " + Loc.T($"Nv. {pk.CurrentLevel}");
    }
}
