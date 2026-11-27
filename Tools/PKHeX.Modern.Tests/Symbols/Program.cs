using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Headless;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Threading;
using PKHeX.Core;
using PKHeX.Modern;
using PKHeX.Modern.Services;
using PKHeX.Modern.Theme;
using PKHeX.Modern.ViewModels;
using PKHeX.Modern.Views;

// Simbolos de icone (issue 1: quadradinhos num Mac Intel). Todo simbolo usado no app precisa de desenho numa fonte
// embutida (Inter, PKHeX Modern Symbols, Noto Sans Symbols 2 ou Noto Emoji), e a PKHeX Modern Symbols vem antes
// das fontes do sistema. Save sintetico (Black) para a captura.
AppBuilder.Configure<App>().UseSkia().UseHeadless(new AvaloniaHeadlessPlatformOptions { UseHeadlessDrawing = false })
    .WithInterFont().With(FontFallbacks.Options(android: false)).SetupWithoutStarting();
int fails = 0;
void Check(string name, bool ok, string extra = "") { Console.WriteLine($"{(ok ? "OK" : "FAIL")} {name} {extra}"); if (!ok) fails++; }
var work = args.Length > 0 ? args[0] : Path.Combine(Path.GetTempPath(), "pkhex-symbols-" + Guid.NewGuid()); Directory.CreateDirectory(work);
BankStorage.Root = Path.Combine(work, "bank"); SaveBackup.Folder = Path.Combine(work, "backups");

const string Fonts = "avares://PKHeX.Modern.UI/Assets/Fonts";
const string Ours = Fonts + "#PKHeX Modern Symbols";
string[] embedded = ["fonts:Inter#Inter", Ours, Fonts + "#Noto Sans Symbols 2", Fonts + "#Noto Emoji"];
var faces = new Dictionary<string, IGlyphTypeface>();
foreach (var family in embedded)
    if (FontManager.Current.TryGetGlyphTypeface(new Typeface(new FontFamily(family)), out var face)) faces[family] = face;
bool Has(string family, int cp) => faces.TryGetValue(family, out var f) && f.TryGetGlyph((uint)cp, out var g) && g != 0;
string Show(int cp) => $"{char.ConvertFromUtf32(cp)} U+{cp:X4}";

Check("PKHeX Modern Symbols carrega", faces.TryGetValue(Ours, out var oursFace) && oursFace.FamilyName == "PKHeX Modern Symbols", faces.ContainsKey(Ours) ? faces[Ours].FamilyName : "não achou");
int[] icons = [0x21B6, 0x21B7, 0x21BA, 0x27F3, 0x22EF, 0x21C4, 0x21C5, 0x2302, 0x2315, 0x24D8, 0x2691];
var missingIcons = icons.Where(cp => !Has(Ours, cp)).ToList();
Check("os onze ícones (↶ ↷ ↺ ⟳ ⋯ ⇄ ⇅ ⌂ ⌕ ⓘ ⚑) estão na fonte", missingIcons.Count == 0, string.Join(", ", missingIcons.Select(Show)));

// Todo simbolo dos textos do app (telas, codigo, dicionarios) tem desenho numa fonte embutida
var root = new DirectoryInfo(AppContext.BaseDirectory);
while (root is not null && !File.Exists(Path.Combine(root.FullName, "PKHeX.Modern", "App.axaml"))) root = root.Parent;
Check("achou o código do app", root is not null, AppContext.BaseDirectory);
var used = new SortedDictionary<int, string>();
if (root is not null)
{
    var app = Path.Combine(root.FullName, "PKHeX.Modern");
    var files = Directory.EnumerateFiles(app, "*.*", SearchOption.AllDirectories)
        .Where(f => f.EndsWith(".axaml") || f.EndsWith(".cs") || (f.EndsWith(".json") && f.Contains(Path.Combine("Assets", "lang"))))
        .Where(f => !f.Contains(Path.DirectorySeparatorChar + "obj" + Path.DirectorySeparatorChar) && !f.Contains(Path.DirectorySeparatorChar + "bin" + Path.DirectorySeparatorChar));
    foreach (var file in files)
    {
        var text = File.ReadAllText(file);
        for (int i = 0; i < text.Length; i++)
        {
            int cp = char.ConvertToUtf32(text, i);
            if (cp > 0xFFFF) i++;
            if ((cp is >= 0x2190 and <= 0x2BFF || cp >= 0x1F000) && !used.ContainsKey(cp)) used[cp] = Path.GetFileName(file);
        }
    }
}
Check("leu os símbolos do app", used.Count > 60 && icons.All(used.ContainsKey), $"{used.Count} símbolos");
var uncovered = used.Where(kv => !embedded.Any(f => Has(f, kv.Key))).Select(kv => $"{Show(kv.Key)} ({kv.Value})").ToList();
Check("todo símbolo do app tem desenho numa fonte embutida", uncovered.Count == 0, string.Join(", ", uncovered));

// Ordem das reservas: a nossa primeiro em todo sistema; no macOS e no Linux, Noto Sans Symbols 2 so para setas e formas
string Names(FontManagerOptions o) => string.Join(" > ", o.FontFallbacks!.Select(f => f.FontFamily.Name));
var windows = FontFallbacks.Options(android: false, windows: true);
var unix = FontFallbacks.Options(android: false, windows: false);
var android = FontFallbacks.Options(android: true);
Check("Windows: nossa fonte e o chinês", Names(windows) == "PKHeX Modern Symbols > Noto Sans SC", Names(windows));
Check("macOS/Linux: nossa fonte, Noto Sans Symbols 2 e o chinês", Names(unix) == "PKHeX Modern Symbols > Noto Sans Symbols 2 > Noto Sans SC", Names(unix));
Check("Android: nossa fonte primeiro", Names(android).StartsWith("PKHeX Modern Symbols > Noto Sans Symbols 2 > Noto Emoji"), Names(android));
var range = unix.FontFallbacks![1].UnicodeRange;
Check("macOS/Linux: Noto Sans Symbols 2 cobre ⭳ ⭱ ⌨ ◐ ▾", new[] { 0x2B73, 0x2B71, 0x2328, 0x25D0, 0x25BE }.All(range.IsInRange));
Check("macOS/Linux: ✓ ✕ ★ ⚙ e emoji ficam com o sistema", !new[] { 0x2713, 0x2715, 0x2605, 0x2699, 0x1F4BE }.Any(range.IsInRange));

// No app (Windows aqui), o ↶ vem da nossa fonte, nao da Segoe UI Symbol: o desenho e igual em todo sistema
foreach (var cp in new[] { 0x21B6, 0x22EF, 0x27F3 })
{
    var found = FontManager.Current.TryMatchCharacter(cp, FontStyle.Normal, FontWeight.Normal, FontStretch.Normal, null, CultureInfo.InvariantCulture, out var tf);
    Check($"reserva do {Show(cp)} é a PKHeX Modern Symbols", found && tf.FontFamily.Name == "PKHeX Modern Symbols", found ? tf.FontFamily.Name : "nenhuma");
}

// Capturas para conferir o desenho: amostra em varios tamanhos e a janela com a barra de acoes
var sample = new StackPanel { Margin = new Thickness(24), Spacing = 12 };
foreach (var size in new[] { 12.0, 14, 16, 24, 40 })
    sample.Children.Add(new TextBlock { FontSize = size, Text = "Box ↶ ↷ ↺ ⟳ ⋯ ⇄ ⇅ ⌂ ⌕ ⓘ ⚑ Caixa" });
var buttons = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 8 };
foreach (var text in new[] { "⋯  Caixa", "⋯", "↶", "↷", "⟳  Atualizar", "⇅  Ordenar", "⇄  Sincronizar", "↺  Restaurar", "⚑  Flags de evento", "ⓘ" })
    buttons.Children.Add(new Button { Content = text, Classes = { "small" } });
sample.Children.Add(buttons);
var sampleWin = new Window { Content = sample, Width = 1000, Height = 420 }; sampleWin.Show();
void Pump(int n = 12) { for (int i = 0; i < n; i++) { Dispatcher.UIThread.RunJobs(); AvaloniaHeadlessPlatform.ForceRenderTimerTick(); } }
Pump();
sampleWin.CaptureRenderedFrame()?.Save(Path.Combine(work, "symbols.png"));
sampleWin.Close();

var path = Path.Combine(work, "Black.sav");
File.WriteAllBytes(path, BlankSaveFile.Get(GameVersion.B).Write().ToArray());
var vm = new MainViewModel(new AppSettings { CheckForUpdates = false });
var win = new MainWindow { DataContext = vm, Width = 1440, Height = 900 }; win.Show();
vm.Open(path); vm.CurrentPage = vm.Boxes; Pump(20);
win.CaptureRenderedFrame()?.Save(Path.Combine(work, "boxes.png"));
Check("capturas geradas", File.Exists(Path.Combine(work, "symbols.png")) && File.Exists(Path.Combine(work, "boxes.png")));

Console.WriteLine(fails == 0 ? "TUDO OK" : $"{fails} FALHAS");
return fails == 0 ? 0 : 1;
