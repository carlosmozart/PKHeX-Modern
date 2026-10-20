using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using PKHeX.Core;

namespace PKHeX.Modern.Services;

/// <summary>Resumo de um save num momento: o que a linha do tempo compara.</summary>
public sealed record SaveSnapshot
{
    public DateTime When { get; init; }
    /// <summary>"opened" (aberto no app: mudanças vieram do jogo), "saved" (gravado pelo app) ou "backup" (ponto de partida tirado do backup).</summary>
    public string Kind { get; init; } = "opened";
    public int PlayMinutes { get; init; }
    public long Money { get; init; }
    public int Caught { get; init; }
    public int Pokemon { get; init; }
    /// <summary>Espécies guardadas (equipe e caixas), sem repetir.</summary>
    public List<int> Species { get; init; } = [];
    /// <summary>Shinies guardados: "espécie:EC" (o EC identifica o Pokémon mesmo se mudar de caixa).</summary>
    public List<string> Shinies { get; init; } = [];
    /// <summary>Insígnias obtidas ("Unova: Bolt").</summary>
    public List<string> Badges { get; init; } = [];
}

/// <summary>Uma linha da linha do tempo: quando e o que mudou desde o resumo anterior.</summary>
public sealed record TimelineEntry(DateTime When, string Kind, IReadOnlyList<string> Changes, IReadOnlyList<ushort> NewShinies);

/// <summary>
/// Linha do tempo do save no Início: guarda um resumo (<see cref="SaveSnapshot"/>) cada vez que o save é aberto ou
/// gravado pelo app e mostra o que mudou entre um e outro (horas de jogo, Pokémon e espécies novas, shinies,
/// insígnias, Pokédex, dinheiro). Sem resumo anterior, o backup mais recente vira o ponto de partida.
/// Os resumos ficam em &lt;dados do app&gt;/timeline, um arquivo por save (pelo caminho), com os 40 mais recentes.
/// </summary>
public static class SaveTimeline
{
    private const int Keep = 40;

    /// <summary>Pasta dos resumos (ao lado dos backups; os testes isolam pelo <see cref="SaveBackup.Folder"/>).</summary>
    public static string Folder => Path.Combine(Path.GetDirectoryName(SaveBackup.Folder) ?? ".", "timeline");

    private sealed record Stored(string Path, List<SaveSnapshot> Snapshots);

    public static SaveSnapshot Capture(SaveFile sav, string kind = "opened", DateTime? when = null)
    {
        int minutes = 0; long money = 0; int caught = 0;
        try { minutes = (sav.PlayedHours * 60) + sav.PlayedMinutes; } catch { /* sem tempo de jogo */ }
        try { money = sav.Money; } catch { /* sem dinheiro */ }
        try { if (sav.HasPokeDex) caught = sav.CaughtCount; } catch { /* Pokédex ilegível */ }
        var species = new SortedSet<int>();
        var shinies = new List<string>();
        int count = 0;
        try
        {
            foreach (var e in EntitySearch.ReadAll(sav))
            {
                if (e.Pkm.IsEgg) continue;
                count++;
                species.Add(e.Pkm.Species);
                if (e.Pkm.IsShiny)
                    shinies.Add($"{e.Pkm.Species}:{e.Pkm.EncryptionConstant:X8}{e.Pkm.PID:X8}");
            }
        }
        catch
        {
            // caixas ilegíveis
        }
        var badges = new List<string>();
        try
        {
            foreach (var g in TrainerExtras.GetBadges(sav))
                for (int i = 0; i < g.Names.Count; i++)
                    if (g.Get(i)) badges.Add($"{g.Region}: {g.Names[i]}");
        }
        catch
        {
            // insígnias ilegíveis
        }
        return new SaveSnapshot
        {
            When = when ?? DateTime.Now, Kind = kind, PlayMinutes = minutes, Money = money, Caught = caught, Pokemon = count,
            Species = [.. species], Shinies = shinies, Badges = badges,
        };
    }

    /// <summary>
    /// Registra o save aberto ou gravado e devolve a linha do tempo (mais nova primeiro). Um resumo igual ao anterior
    /// não entra de novo (abrir duas vezes sem jogar não cria linha).
    /// </summary>
    public static IReadOnlyList<TimelineEntry> Record(string path, SaveFile sav, string kind = "opened")
    {
        var list = Load(path);
        if (list.Count == 0 && SeedFromBackup(path) is { } seed)
            list.Add(seed);
        var now = Capture(sav, kind);
        if (list.Count == 0 || !Same(list[^1], now))
        {
            list.Add(now);
            if (list.Count > Keep)
                list.RemoveRange(0, list.Count - Keep);
            Save(path, list);
        }
        return Build(list);
    }

    /// <summary>Linha do tempo já registrada, sem registrar nada.</summary>
    public static IReadOnlyList<TimelineEntry> Get(string path) => Build(Load(path));

    /// <summary>Quantos resumos este save tem (1 = só o primeiro, ainda sem comparação).</summary>
    public static int Count(string path) => Load(path).Count;

    private static IReadOnlyList<TimelineEntry> Build(List<SaveSnapshot> list)
    {
        var entries = new List<TimelineEntry>();
        for (int i = list.Count - 1; i >= 1; i--)
        {
            var (changes, shinies) = Diff(list[i - 1], list[i]);
            if (changes.Count > 0)
                entries.Add(new TimelineEntry(list[i].When, list[i].Kind, changes, shinies));
        }
        return entries;
    }

    /// <summary>O que mudou de <paramref name="a"/> para <paramref name="b"/>, em frases curtas.</summary>
    public static (List<string> Changes, List<ushort> NewShinies) Diff(SaveSnapshot a, SaveSnapshot b)
    {
        var changes = new List<string>();
        int played = b.PlayMinutes - a.PlayMinutes;
        if (played > 0)
            changes.Add(played >= 60 ? $"+{played / 60}h{played % 60:00} de jogo" : $"+{played} min de jogo");
        var newShinies = b.Shinies.Except(a.Shinies).Select(s => ushort.Parse(s[..s.IndexOf(':')])).ToList();
        foreach (var sp in newShinies)
            changes.Add($"★ Shiny novo: {Name(sp)}");
        var newSpecies = b.Species.Except(a.Species).ToList();
        if (newSpecies.Count > 0)
            changes.Add(newSpecies.Count <= 4
                ? $"Espécies novas: {string.Join(", ", newSpecies.Select(Name))}"
                : $"{newSpecies.Count} espécies novas: {string.Join(", ", newSpecies.Take(3).Select(Name))}…");
        int pokemon = b.Pokemon - a.Pokemon;
        if (pokemon != 0)
            changes.Add(pokemon > 0 ? $"+{pokemon} Pokémon guardados" : $"{pokemon} Pokémon guardados");
        var badges = b.Badges.Except(a.Badges).Select(x => x[(x.IndexOf(": ", StringComparison.Ordinal) + 2)..]).ToList();
        if (badges.Count > 0)
            changes.Add(badges.Count == 1 ? $"Insígnia nova: {badges[0]}" : $"Insígnias novas: {string.Join(", ", badges)}");
        int caught = b.Caught - a.Caught;
        if (caught > 0)
            changes.Add($"Pokédex +{caught} ({b.Caught} capturados)");
        long money = b.Money - a.Money;
        if (money != 0 && Math.Abs(money) >= 1000)
            changes.Add(money > 0 ? $"+${money:N0}" : $"−${-money:N0}");
        return (changes, newShinies);
    }

    private static string Name(int species) => (uint)species < (uint)CoreAdapter.SpeciesNames.Count ? CoreAdapter.SpeciesNames[species] : $"#{species}";

    private static bool Same(SaveSnapshot a, SaveSnapshot b) => a.PlayMinutes == b.PlayMinutes && a.Money == b.Money && a.Caught == b.Caught
        && a.Pokemon == b.Pokemon && a.Species.SequenceEqual(b.Species) && a.Shinies.Order().SequenceEqual(b.Shinies.Order()) && a.Badges.SequenceEqual(b.Badges);

    /// <summary>Primeiro resumo a partir do backup mais recente deste save (assim a primeira abertura já compara).</summary>
    private static SaveSnapshot? SeedFromBackup(string path)
    {
        try
        {
            var file = Path.GetFullPath(ZipSaves.FileOf(path));
            var backup = SaveBackup.List().FirstOrDefault(b => b.Source is { } s && string.Equals(Path.GetFullPath(s), file, StringComparison.OrdinalIgnoreCase));
            if (backup is null || SaveUtil.GetSaveFile(backup.Path) is not { } old)
                return null;
            return Capture(old, "backup", backup.Created);
        }
        catch
        {
            return null;
        }
    }

    private static string FileFor(string path)
    {
        var hash = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(path.ToLowerInvariant())))[..16];
        return Path.Combine(Folder, hash + ".json");
    }

    private static List<SaveSnapshot> Load(string path)
    {
        try
        {
            var file = FileFor(path);
            if (!File.Exists(file)) return [];
            return JsonSerializer.Deserialize<Stored>(File.ReadAllText(file))?.Snapshots ?? [];
        }
        catch
        {
            return [];
        }
    }

    private static void Save(string path, List<SaveSnapshot> list)
    {
        try
        {
            Directory.CreateDirectory(Folder);
            File.WriteAllText(FileFor(path), JsonSerializer.Serialize(new Stored(path, list)));
        }
        catch
        {
            // a linha do tempo não é crítica
        }
    }
}
