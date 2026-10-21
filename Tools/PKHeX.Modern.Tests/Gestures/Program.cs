using System;
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

// Gestos de toque (0.5.0): deslizar entre caixas e puxar para reler os saves. O headless nao gera toque, entao o
// teste confere a classificacao do movimento e a acao de cada area com a tela de verdade (celular e desktop).
AppBuilder.Configure<App>().UseSkia().UseHeadless(new AvaloniaHeadlessPlatformOptions { UseHeadlessDrawing = false }).WithInterFont().SetupWithoutStarting();
var work = args[0]; int fails = 0;
BankStorage.Root = Path.Combine(work, "bank"); SaveBackup.Folder = Path.Combine(work, "backups");
void Check(string n, bool ok, string extra = "") { Console.WriteLine($"{(ok ? "OK" : "FAIL")} {n} {extra}"); if (!ok) fails++; }
void Pump() { for (int i = 0; i < 3; i++) { Dispatcher.UIThread.RunJobs(); AvaloniaHeadlessPlatform.ForceRenderTimerTick(); } }

var fast = TimeSpan.FromMilliseconds(200);
Check("esquerda = próxima", BoxSwipe.Classify(new Vector(-120, 10), fast) == 1);
Check("direita = anterior", BoxSwipe.Classify(new Vector(120, -15), fast) == -1);
Check("curto não troca", BoxSwipe.Classify(new Vector(-40, 0), fast) == 0);
Check("lento (segurar) não troca", BoxSwipe.Classify(new Vector(-150, 0), TimeSpan.FromMilliseconds(700)) == 0);
Check("diagonal/vertical (rolar) não troca", BoxSwipe.Classify(new Vector(-90, 80), fast) == 0);

var sav = BlankSaveFile.Get(GameVersion.B); sav.OT = "DEMO";
sav.SetBoxSlotAtIndex(new PK5 { Species = 504, Version = GameVersion.B, CurrentLevel = 10, OriginalTrainerName = "DEMO" }, 0, 0);
var path = Path.Combine(work, "Black.sav"); File.WriteAllBytes(path, sav.Write().ToArray());
var library = Path.Combine(work, "lib"); Directory.CreateDirectory(library); File.Copy(path, Path.Combine(library, "Black.sav"));
var vm = new MainViewModel(new AppSettings { CheckForUpdates = false, SavesFolder = library }); vm.Open(path); vm.CurrentPage = vm.Boxes;

void SwipeChecks(string where, TopLevel top)
{
    top.UpdateLayout(); Pump();
    var slot = top.GetVisualDescendants().OfType<Button>().First(b => b.DataContext == vm.Boxes.Slots[5]);
    var area = BoxSwipe.AreaAt(slot);
    Check($"{where}: slot fica numa área de deslizar", area is not null);
    vm.Boxes.CurrentBox = 0; Pump();
    Check($"{where}: deslizar para a esquerda vai para a caixa 2", area is not null && BoxSwipe.Swipe(area, 1) && vm.Boxes.CurrentBox == 1);
    Check($"{where}: deslizar para a direita volta", area is not null && BoxSwipe.Swipe(area, -1) && vm.Boxes.CurrentBox == 0);
    var sidebarItem = top.GetVisualDescendants().OfType<ListBoxItem>().FirstOrDefault();
    Check($"{where}: fora da caixa (barra lateral) não desliza", sidebarItem is null || BoxSwipe.AreaAt(sidebarItem) is null);
}

var desktop = new MainWindow { DataContext = vm, Width = 1440, Height = 950 }; desktop.Show(); Pump();
SwipeChecks("desktop", desktop);
// Bank: o painel do save aberto troca a caixa do save; o do bank troca a do bank
vm.CurrentPage = vm.Pages.OfType<BankPageViewModel>().First(); desktop.UpdateLayout(); Pump();
var saveSlot = desktop.GetVisualDescendants().OfType<Button>().FirstOrDefault(b => b.DataContext == vm.Boxes.Slots[5]);
vm.Boxes.CurrentBox = 0; Pump();
var saveArea = saveSlot is null ? null : BoxSwipe.AreaAt(saveSlot);
Check("Bank: painel do save troca a caixa do save", saveArea is not null && BoxSwipe.Swipe(saveArea, 1) && vm.Boxes.CurrentBox == 1);
var bankAreas = desktop.GetVisualDescendants().OfType<Control>().Where(c => c.Classes.Contains("boxSwipe") && c.IsEffectivelyVisible).ToList();
Check("Bank: painéis do bank e do save são áreas separadas", bankAreas.Count >= 2, $"({bankAreas.Count})");
vm.Boxes.CurrentBox = 0; vm.CurrentPage = vm.Boxes; desktop.Close();

App.ShowShortcuts = false; vm.IsTouchUI = true;
var phone = new Window { Width = 892, Height = 412, Content = new MobileShell(vm) }; phone.Show(); Pump();
SwipeChecks("celular", phone);

// Puxar para reler: a lista do Save Manager fica dentro de um RefreshContainer que relê a pasta
vm.CurrentPage = vm.SaveManager; phone.UpdateLayout(); Pump();
var refresh = phone.GetVisualDescendants().OfType<RefreshContainer>().FirstOrDefault(r => r.IsEffectivelyVisible);
Check("Save Manager tem puxar para reler", refresh is not null);
File.Copy(path, Path.Combine(library, "Black2.sav"));
refresh?.RequestRefresh();
var sw = Stopwatch.StartNew();
while (sw.ElapsedMilliseconds < 10000 && !vm.SaveManager.Entries.Any(e => e.Path.EndsWith("Black2.sav"))) { Pump(); System.Threading.Thread.Sleep(10); }
Check("puxar relê a pasta (save novo aparece)", vm.SaveManager.Entries.Any(e => e.Path.EndsWith("Black2.sav")));
phone.Close();
Console.WriteLine(fails == 0 ? "TUDO OK" : $"FALHAS: {fails}");
return fails == 0 ? 0 : 1;
