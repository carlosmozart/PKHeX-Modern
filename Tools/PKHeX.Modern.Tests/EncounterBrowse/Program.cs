using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Headless;
using Avalonia.Threading;
using Avalonia.VisualTree;
using PKHeX.Core;
using PKHeX.Modern;
using PKHeX.Modern.Services;
using PKHeX.Modern.ViewModels;
using PKHeX.Modern.Views;

// Encontros › "Ver todos": todos os encontros do jogo do save, de todas as especies, agrupados por especie e local,
// com os filtros de local e de nome. Saves sinteticos (FireRed, Black e Brilliant Diamond).
AppBuilder.Configure<App>().UseSkia().UseHeadless(new AvaloniaHeadlessPlatformOptions { UseHeadlessDrawing = false }).WithInterFont().SetupWithoutStarting();
int fails = 0;
void Check(string name, bool ok, string extra = "") { Console.WriteLine($"{(ok ? "OK  " : "FAIL")} {name} {extra}"); if (!ok) fails++; }
var work = args.Length > 0 ? args[0] : Path.Combine(Path.GetTempPath(), "pkhex-encbrowse-" + Guid.NewGuid()); Directory.CreateDirectory(work);
BankStorage.Root = Path.Combine(work, "bank"); SaveBackup.Folder = Path.Combine(work, "backups");
CoreAdapter.SetLanguage("en");

// Servico: so encontros nativos da versao, cada um na propria especie, sem repetir
foreach (var version in new[] { GameVersion.FR, GameVersion.B, GameVersion.BD })
{
    var sav = BlankSaveFile.Get(version); CoreAdapter.Activate(sav);
    var sw = Stopwatch.StartNew();
    var all = EncounterDatabase.SearchAllEncounters(sav);
    var time = sw.Elapsed.TotalSeconds;
    Check($"{version}: encontros de muitas espécies", all.Select(e => e.Species).Distinct().Count() > 150, $"({all.Count} encontros, {time:F1}s)");
    Check($"{version}: em menos de 20 s", time < 20);
    Check($"{version}: sem repetir o mesmo encontro", all.Distinct(ReferenceEqualityComparer.Instance).Count() == all.Count);
    Check($"{version}: sem os eventos (ficam na página Eventos)", !all.Any(e => e is MysteryGift));
    Check($"{version}: em ordem de Pokédex", all.Zip(all.Skip(1)).All(p => p.First.Species <= p.Second.Species));
    Check($"{version}: só da versão do save (nada exclusivo da outra versão do par)",
        all.All(e => e.Version == version || e.Version.Contains(version) || version.Contains(e.Version)));
    var cards = EncounterDbViewModel.GroupCards(all);
    Check($"{version}: cartões agrupados por espécie e local", cards.Count < all.Count && cards.Sum(c => c.Variants.Count) == all.Count, $"({cards.Count} cartões)");
    Check($"{version}: cada cartão junta só a mesma espécie, forma e local",
        cards.All(c => c.Variants.All(v => v.Species == c.Encounter.Species && v.Form == c.Encounter.Form && EncounterCardViewModel.LocationOf(v) == c.Location)));
    Check($"{version}: ovos de criação sem o local onde o PKHeX os choca", cards.Where(c => c.Encounter is IEncounterEgg).All(c => !c.HasLocation)
        && !cards.Where(c => c.Encounter is IEncounterEgg).Select(c => c.Location).Any(l => cards.Any(o => o.Encounter is not IEncounterEgg && o.Location == l && l.Length > 0)));
    var grouped = cards.First(c => c.HasVariants);
    Check($"{version}: nível do cartão cobre todas as variações",
        grouped.Level == (grouped.Variants.Min(v => v.LevelMin) == grouped.Variants.Max(v => v.LevelMax) ? $"Nv. {grouped.Variants.Min(v => v.LevelMin)}" : $"Nv. {grouped.Variants.Min(v => v.LevelMin)}–{grouped.Variants.Max(v => v.LevelMax)}"), grouped.Level);
    // "Usar": o primeiro encontro de cada cartao gera um Pokemon (amostra)
    var sample = cards.Where((_, i) => i % Math.Max(1, cards.Count / 25) == 0).Take(25).ToList();
    var made = sample.Count(c => EncounterDatabase.ToEntity(sav, c.Encounter, out _) is not null);
    Check($"{version}: Usar gera o Pokémon dos cartões", made == sample.Count, $"({made}/{sample.Count})");
}

// Pagina: "Ver todos", filtros de local e nome, detalhes com as variacoes e volta para a busca por especie
var black = BlankSaveFile.Get(GameVersion.B); black.OT = "Demo"; CoreAdapter.Activate(black);
var lib = Path.Combine(work, "saves"); Directory.CreateDirectory(lib);
var path = Path.Combine(lib, "Black.sav"); File.WriteAllBytes(path, black.Write().ToArray());
var vm = new MainViewModel(new AppSettings { SavesFolder = lib, CheckForUpdates = false, Animations = false });
var win = new MainWindow { DataContext = vm, Width = 1440, Height = 900 }; win.Show();
void Pump(int n = 10) { for (int i = 0; i < n; i++) { Dispatcher.UIThread.RunJobs(); AvaloniaHeadlessPlatform.ForceRenderTimerTick(); } }
void Wait(System.Threading.Tasks.Task t) { for (int i = 0; i < 6000 && !t.IsCompleted; i++) { Pump(1); System.Threading.Thread.Sleep(5); } t.GetAwaiter().GetResult(); Pump(); }
vm.Open(path); Pump();
var enc = vm.Encounters;
vm.CurrentPage = enc; Pump();
Check("dica inicial cita Ver todos", enc.Summary.Contains("Ver todos"), enc.Summary);
var showAll = win.GetVisualDescendants().OfType<Button>().FirstOrDefault(b => b.Content as string == "Ver todos" && b.IsEffectivelyVisible);
Check("botão Ver todos na página", showAll is not null);
Wait(enc.ShowAllAsync());
Check("Ver todos lista o jogo inteiro", enc.IsShowingAll && enc.HasFilters && enc.Results.Count == 90 && enc.HasMore, enc.Summary);
Check("resumo com cartões e espécies", enc.Summary.Contains("cartão(ões) de") && enc.Summary.Contains("espécie(s) deste jogo"), enc.Summary);
Check("filtro de local com os locais do jogo", enc.HasLocationOptions && enc.LocationOptions.Contains("Route 1"), $"({enc.LocationOptions.Count - 1} locais)");
enc.LocationFilter = "Route 1"; Pump();
Check("filtro de local: só a Route 1", enc.Results.Count > 0 && enc.Results.All(c => c.Location == "Route 1") && enc.Results.Any(c => c.Title == "Patrat"),
    string.Join(", ", enc.Results.Select(c => c.Title).Distinct()));
enc.NameFilter = "lilli"; Pump();
Check("filtro de nome dentro do local", enc.Results.Count > 0 && enc.Results.All(c => c.Title.Contains("Lillipup", StringComparison.OrdinalIgnoreCase)) && enc.IsFiltered);
var withVariants = enc.Results.FirstOrDefault(c => c.HasVariants);
if (withVariants is not null)
{
    enc.SelectCommand.Execute(withVariants); Pump();
    // Lillipup na Route 1: 6 slots, 4 deles iguais (Grass, Nv. 4), que viram uma linha com a contagem
    Check("detalhes mostram as variações", enc.Detail is { HasVariants: true } d && d.VariantsHint.Contains(withVariants.Variants.Count.ToString())
        && d.Variants.Count < withVariants.Variants.Count && d.Variants.Any(l => l.EndsWith("(×4)")), string.Join(" | ", enc.Detail?.Variants ?? []));
}
var nameBox = win.GetVisualDescendants().OfType<TextBox>().FirstOrDefault(t => t.Watermark == "Filtrar por nome...");
Check("campo Filtrar por nome visível no Ver todos", nameBox is { IsEffectivelyVisible: true });
win.CaptureRenderedFrame()?.Save(Path.Combine(work, "encounters_all.png"));
enc.ClearFiltersCommand.Execute(null); Pump();
Check("limpar filtros volta à lista inteira", !enc.IsFiltered && enc.Results.Count == 90 && enc.LocationFilter == "Todos os locais");
// Escolher uma especie sai do "Ver todos"
enc.Species = "Eevee";
for (int i = 0; i < 2000 && (enc.IsBusy || enc.IsShowingAll); i++) { Pump(1); System.Threading.Thread.Sleep(5); }
Pump();
Check("busca por espécie sai do Ver todos", !enc.IsShowingAll && enc.Results.All(c => c.Title.StartsWith("Eevee")) && enc.Summary.Contains("Eevee"), enc.Summary);
nameBox = win.GetVisualDescendants().OfType<TextBox>().FirstOrDefault(t => t.Watermark == "Filtrar por nome...");
Check("campo de nome some na busca por espécie", nameBox is null || !nameBox.IsEffectivelyVisible);
win.Close();
Console.WriteLine(fails == 0 ? "TUDO OK" : $"{fails} FALHAS"); return fails == 0 ? 0 : 1;
