using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace PKHeX.Modern.Services;

/// <summary>
/// Acha sozinho os saves dos emuladores na memoria do celular (Android, com "acesso a todos os arquivos"): as pastas
/// padrao de cada emulador e, para os que gravam o save ao lado da ROM (mGBA, My Boy!, Pizza Boy, melonDS...), as
/// pastas do usuario, pulando fotos, musica e pastas do sistema. Emuladores sem pasta fixa (o PokeDaisy grava na pasta
/// escolhida na configuracao dele) entram por essa varredura das pastas do usuario. So devolve caminhos; quem confere se e mesmo um
/// save e o <see cref="SaveLibrary"/> (Core). Sem dependencia do Android: testavel no desktop com uma arvore falsa.
/// </summary>
public static class EmulatorSaves
{
    /// <summary>Raiz da memoria do aparelho (Android: /storage/emulated/0); nulo no desktop (a funcao nao aparece).</summary>
    public static string? StorageRoot { get; set; }
    /// <summary>O app tem "acesso a todos os arquivos" (definido pelo Android).</summary>
    public static Func<bool>? HasAccess { get; set; }
    /// <summary>Abre a tela do Android para dar o acesso (o usuario liga e volta ao app).</summary>
    public static Action? RequestAccess { get; set; }
    /// <summary>O app voltou ao primeiro plano (ex.: depois de dar o acesso): a pagina Saves rele.</summary>
    public static Action? Resumed { get; set; }

    public static bool IsAvailable => StorageRoot is not null && HasAccess is not null;
    public static bool CanRead => IsAvailable && HasAccess!();

    /// <summary>Um emulador: nome, pastas padrao (relativas a raiz da memoria) e que arquivos dentro delas sao saves.</summary>
    public sealed record Emulator(string Name, string[] Folders, Func<string, bool> IsSave);

    /// <summary>Saves de cartucho (GB/GBC/GBA/NDS) com as extensoes mais usadas pelos emuladores.</summary>
    private static readonly string[] CartridgeExtensions = [".sav", ".srm", ".dsv", ".sa1", ".sa2", ".sa3", ".sa4", ".sa5", ".sa6", ".sa7", ".sa8", ".sa9"];

    private static bool HasExtension(string path, params string[] extensions)
        => extensions.Contains(Path.GetExtension(path), StringComparer.OrdinalIgnoreCase);

    /// <summary>Saves de 3DS e Switch: arquivo "main" dentro da arvore do emulador.</summary>
    private static bool IsMain(string path) => Path.GetFileName(path).Equals("main", StringComparison.OrdinalIgnoreCase);

    public static IReadOnlyList<Emulator> Known { get; } =
    [
        new("RetroArch", ["RetroArch/saves", "RetroArch/savefiles", "retroarch/saves"], p => HasExtension(p, CartridgeExtensions)),
        new("DraStic", ["DraStic/backup"], p => HasExtension(p, ".dsv", ".sav")),
        new("melonDS", ["melonDS", "melonDS/saves"], p => HasExtension(p, ".sav", ".dsv")),
        new("Dolphin", ["dolphin-emu/GC", "Dolphin/GC", "mmjr2-vbi/GC"], p => HasExtension(p, ".gci", ".raw", ".sav")),
        new("Azahar", ["azahar-emu/sdmc", "Azahar/sdmc"], IsMain),
        new("Citra", ["citra-emu/sdmc", "Citra/sdmc"], IsMain),
        new("Lime3DS", ["lime3ds-emu/sdmc", "Lime3DS/sdmc"], IsMain),
        new("Eden", ["eden/nand/user/save", "Eden/nand/user/save"], IsMain),
        new("Yuzu", ["yuzu/nand/user/save"], IsMain),
        new("Suyu", ["suyu/nand/user/save"], IsMain),
        new("Citron", ["citron/nand/user/save"], IsMain),
        new("Sudachi", ["sudachi/nand/user/save"], IsMain),
        new("mGBA", ["mGBA", "mgba"], p => HasExtension(p, CartridgeExtensions)),
        new("My Boy!", ["MyBoy", "My Boy"], p => HasExtension(p, CartridgeExtensions)),
        // Do mesmo autor do My Boy!, para GB/GBC: grava em MyOldBoy/save.
        new("My OldBoy!", ["MyOldBoy", "My OldBoy"], p => HasExtension(p, CartridgeExtensions)),
        new("Pizza Boy", ["PizzaBoyGBA", "PizzaBoyGBC", "Pizza Boy"], p => HasExtension(p, CartridgeExtensions)),
    ];

    /// <summary>Pastas da raiz que nunca tem save (midia e sistema): a varredura das pastas de ROMs pula.</summary>
    private static readonly HashSet<string> SkipRoots = new(StringComparer.OrdinalIgnoreCase)
    {
        "Android", "DCIM", "Pictures", "Movies", "Music", "Podcasts", "Ringtones", "Alarms", "Notifications",
        "Audiobooks", "Recordings", "Screenshots", "WhatsApp", "Telegram", ".thumbnails", "LOST.DIR",
    };

    /// <summary>Um save achado: caminho e de onde veio ("RetroArch", "Pasta Roms"...).</summary>
    public sealed record Found(string Path, string Source);

    /// <summary>Saves menores que isso nao existem (o menor, Gen 1/2, tem 32 KiB).</summary>
    private const long MinSize = 8 * 1024;
    private const long MaxSize = 64L * 1024 * 1024;

    /// <summary>
    /// Procura em <paramref name="root"/> (ex.: /storage/emulated/0). Primeiro as pastas padrao dos emuladores,
    /// depois saves de cartucho nas outras pastas (ate <paramref name="maxDepth"/> niveis). Para no tempo dado.
    /// </summary>
    public static IReadOnlyList<Found> Find(string root, TimeSpan? budget = null, int maxDepth = 6)
    {
        var result = new List<Found>();
        if (!Directory.Exists(root))
            return result;
        var watch = Stopwatch.StartNew();
        var limit = budget ?? TimeSpan.FromSeconds(15);
        var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        var covered = new List<string>();

        foreach (var emu in Known)
        {
            foreach (var relative in emu.Folders)
            {
                var dir = Path.Combine(root, relative.Replace('/', Path.DirectorySeparatorChar));
                if (!Directory.Exists(dir))
                    continue;
                covered.Add(Path.GetFullPath(dir));
                foreach (var file in Walk(dir, 12, watch, limit))
                    if (emu.IsSave(file) && IsSaveSize(file) && seen.Add(Path.GetFullPath(file)))
                        result.Add(new(file, emu.Name));
            }
        }

        // Saves ao lado das ROMs: qualquer pasta do usuario (menos midia/sistema e as dos emuladores ja lidas).
        IEnumerable<string> tops;
        try { tops = Directory.EnumerateDirectories(root); }
        catch (Exception) { tops = []; }
        foreach (var top in tops)
        {
            if (SkipRoots.Contains(Path.GetFileName(top)) || Path.GetFileName(top).StartsWith('.'))
                continue;
            foreach (var file in Walk(top, maxDepth, watch, limit))
            {
                var full = Path.GetFullPath(file);
                if (covered.Any(c => full.StartsWith(c, StringComparison.OrdinalIgnoreCase)))
                    continue;
                if (HasExtension(file, CartridgeExtensions) && IsSaveSize(file) && seen.Add(full))
                    result.Add(new(file, Path.GetFileName(top)));
            }
        }
        return result;
    }

    private static bool IsSaveSize(string path)
    {
        try
        {
            var length = new FileInfo(path).Length;
            return length is >= MinSize and <= MaxSize;
        }
        catch (Exception)
        {
            return false;
        }
    }

    /// <summary>Arquivos da pasta e subpastas (ate a profundidade); pastas sem permissao sao puladas.</summary>
    private static IEnumerable<string> Walk(string dir, int depth, Stopwatch watch, TimeSpan limit)
    {
        var stack = new Stack<(string Dir, int Depth)>();
        stack.Push((dir, 0));
        while (stack.Count > 0 && watch.Elapsed < limit)
        {
            var (current, level) = stack.Pop();
            string[] files, dirs;
            try
            {
                files = Directory.GetFiles(current);
                dirs = level < depth ? Directory.GetDirectories(current) : [];
            }
            catch (Exception)
            {
                continue;
            }
            foreach (var f in files)
                yield return f;
            foreach (var d in dirs)
                if (!Path.GetFileName(d).StartsWith('.'))
                    stack.Push((d, level + 1));
        }
    }
}
