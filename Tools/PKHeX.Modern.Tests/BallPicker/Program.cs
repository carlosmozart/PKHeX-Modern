using System;
using System.IO;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Headless;
using Avalonia.Interactivity;
using Avalonia.Threading;
using Avalonia.VisualTree;
using PKHeX.Core;
using PKHeX.Modern;
using PKHeX.Modern.Services;
using PKHeX.Modern.ViewModels;
using PKHeX.Modern.Views;

// Bola no editor: sprites na lista de bolas e o clique na bola da ficha levando para a aba Encontro.
AppBuilder.Configure<App>().UseSkia().UseHeadless(new AvaloniaHeadlessPlatformOptions { UseHeadlessDrawing = false }).WithInterFont().SetupWithoutStarting();
var work = args[0]; int fails = 0;
BankStorage.Root = Path.Combine(work, "bank"); SaveBackup.Folder = Path.Combine(work, "backups");
void Check(string n, bool ok, string extra = "") { Console.WriteLine($"{(ok ? "OK" : "FAIL")} {n} {extra}"); if (!ok) fails++; }
void Pump(int n = 6) { for (int i = 0; i < n; i++) { Dispatcher.UIThread.RunJobs(); AvaloniaHeadlessPlatform.ForceRenderTimerTick(); } }

var sav = BlankSaveFile.Get(GameVersion.B); sav.OT = "DEMO"; CoreAdapter.Activate(sav);
var pk = EncounterDatabase.SearchEncounters(sav, 25, true).Select(e => EncounterDatabase.ToEntity(sav, e, out string? _)).First(p => p is not null && new LegalityAnalysis(p).Valid)!;
sav.SetBoxSlotAtIndex(pk, 0, 0);
var path = Path.Combine(work, "Black.sav"); File.WriteAllBytes(path, sav.Write().ToArray());
var vm = new MainViewModel(new AppSettings { CheckForUpdates = false }); vm.Open(path); vm.CurrentPage = vm.Boxes;
var win = new MainWindow { DataContext = vm, Width = 1600, Height = 1000 }; win.Show(); Pump();
vm.Boxes.SelectSlotCommand.Execute(vm.Boxes.Slots[0]); Pump(10);
var ed = vm.Editor!;
Check("editor começa na Visão geral", ed.SelectedTab == 0);

var ballButton = win.GetVisualDescendants().OfType<Button>().FirstOrDefault(b => b.Classes.Contains("cardBall"));
Check("bola da ficha é um botão", ballButton is { IsEffectivelyVisible: true });
ballButton?.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
Pump(10);
Check("clicar na bola abre a aba Encontro", ed.SelectedTab == PokemonEditorView.EncounterTab, $"aba {ed.SelectedTab}");
var combo = win.GetVisualDescendants().OfType<ComboBox>().FirstOrDefault(c => c.Name == "BallCombo");
Check("lista de bolas visível e com foco", combo is { IsEffectivelyVisible: true, IsFocused: true }, $"foco={combo?.IsFocused}");

// Caixa fechada e lista aberta mostram o sprite da bola
Check("caixa fechada mostra o sprite da bola", combo is not null && combo.GetVisualDescendants().OfType<Image>().Any(i => i.Source is not null));
if (combo is not null) { combo.IsDropDownOpen = true; Pump(10); }
var popupImages = win.GetVisualDescendants().Concat(TopLevel.GetTopLevel(win)!.GetVisualDescendants())
    .OfType<ComboBoxItem>().SelectMany(i => i.GetVisualDescendants().OfType<Image>()).Count(i => i.Source is not null);
Check("lista aberta mostra os sprites", popupImages >= Math.Min(3, ed.BallList.Count), $"{popupImages} sprites para {ed.BallList.Count} bolas");
if (combo is not null) combo.IsDropDownOpen = false;
// Sem o modo legal, a lista tem todas as bolas do jogo, cada uma com o seu sprite
vm.LegalMode = false; Pump(10);
if (combo is not null) { combo.IsDropDownOpen = true; Pump(12); }
var all = TopLevel.GetTopLevel(win)!.GetVisualDescendants().OfType<ComboBoxItem>()
    .Count(i => i.GetVisualDescendants().OfType<Image>().Any(img => img.Source is not null));
Check("sem modo legal: todas as bolas visíveis têm sprite", ed.BallList.Count > 10 && all >= 8, $"{all} itens com sprite, {ed.BallList.Count} bolas");
win.CaptureRenderedFrame()?.Save(Path.Combine(work, "balls.png"));
if (combo is not null) combo.IsDropDownOpen = false;
Console.WriteLine(fails == 0 ? "TUDO OK" : $"FALHAS: {fails}");
return fails == 0 ? 0 : 1;
