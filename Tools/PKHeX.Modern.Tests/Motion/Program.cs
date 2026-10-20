using System;
using System.IO;
using System.Linq;
using Avalonia;
using Avalonia.Headless;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Threading;
using Avalonia.VisualTree;
using PKHeX.Core;
using PKHeX.Modern;
using PKHeX.Modern.Controls;
using PKHeX.Modern.Services;
using PKHeX.Modern.ViewModels;
using PKHeX.Modern.Views;

// Animações curtas: Pokébola ao abrir, pulinho do slot que recebe um Pokémon, brilho ✦ no editor e a opção que desliga.
AppBuilder.Configure<App>().UseSkia().UseHeadless(new AvaloniaHeadlessPlatformOptions { UseHeadlessDrawing = false }).WithInterFont().SetupWithoutStarting();
var work = args[0]; int fails = 0;
BankStorage.Root = Path.Combine(work, "bank"); SaveBackup.Folder = Path.Combine(work, "data", "backups"); ShinyHuntStore.Folder = Path.Combine(work, "data");
void Check(string n, bool ok, string detail = "") { Console.WriteLine($"{(ok ? "OK" : "FAIL")} {n} {detail}"); if (!ok) fails++; }
void Pump() { for (int i = 0; i < 4; i++) { Dispatcher.UIThread.RunJobs(); AvaloniaHeadlessPlatform.ForceRenderTimerTick(); } }
static long Pixels(Avalonia.Layout.Layoutable v, int w, int h)
{
    v.Measure(new Size(w, h)); v.Arrange(new Rect(0, 0, w, h));
    using var rtb = new RenderTargetBitmap(new PixelSize(w, h)); rtb.Render(v);
    using var ms = new MemoryStream(); rtb.Save(ms); return ms.Length;
}

var sav = BlankSaveFile.Get(GameVersion.B); sav.OT = "Demo";
sav.SetBoxSlotAtIndex(new PK5 { Species = 495, Version = GameVersion.B, CurrentLevel = 5 }, 0, 0);
var path = Path.Combine(work, "Black.sav"); File.WriteAllBytes(path, sav.Write().ToArray());
var vm = new MainViewModel(new AppSettings { CheckForUpdates = false, AskLegalizeOnClick = false, LegalMode = false });
var win = new MainWindow { DataContext = vm, Width = 1400, Height = 900 }; win.Show();

vm.Open(path); Pump();
Check("abrir um save toca a Pokébola", vm.OpenToken == 1);
var ball = win.GetVisualDescendants().OfType<PokeballOpen>().Single();
ball.Progress = 0.6; Pump();
long empty = Pixels(new PokeballOpen { Progress = 1 }, 150, 150);
Check("Pokébola desenha no meio da animação", Pixels(new PokeballOpen { Progress = 0.6, Top = Brushes.Red, Bottom = Brushes.White, Line = Brushes.Black }, 150, 150) > empty + 300);
ball.Progress = 1;

// Pulinho: o slot recebe outro Pokémon; trocar de caixa não pula
vm.CurrentPage = vm.Boxes; Pump();
var slot = vm.Boxes.Slots[1];
int pulses = 0; slot.PropertyChanged += (_, e) => { if (e.PropertyName == nameof(SlotViewModel.Pulse)) pulses++; };
slot.Write(vm.ActiveTab!.Sav, new PK5 { Species = 498, Version = GameVersion.B, CurrentLevel = 5 }); Pump();
Check("slot que recebe um Pokémon pula", pulses == 1 && slot.Pulse == 1);
slot.Load(vm.ActiveTab.Sav); Pump();
Check("recarregar o mesmo Pokémon não pula", pulses == 1);
vm.Boxes.CurrentBox = 1; vm.Boxes.CurrentBox = 0; Pump();
Check("trocar de caixa não pula", vm.Boxes.Slots.All(s => s.Pulse == 0));

// Brilho no editor: virar shiny brilha; abrir não brilha
var select = vm.SelectSlotAsync(vm.Boxes.Slots[0]);
for (int i = 0; i < 2000 && !select.IsCompleted; i++) { Pump(); if (vm.Dialog is { } d) d.Complete(false); System.Threading.Thread.Sleep(5); }
var ed = vm.Editor!;
Check("abrir o Pokémon não brilha", ed.SparkleToken == 0);
if (ed.IsShiny) { ed.ShinyClick(false, false, false); Pump(); } // o PK5 sintético (PID 0, TID 0) já nasce shiny
Check("tirar o shiny não brilha", !ed.IsShiny && ed.SparkleToken == 0);
ed.ShinyClick(false, false, false); Pump();
Check("virar shiny brilha", ed.IsShiny && ed.SparkleToken == 1, $"token {ed.SparkleToken} shiny {ed.IsShiny} legal {ed.IsLegal} modo {ed.LegalMode} status {vm.Status}");
Check("brilho desenha no meio da animação", Pixels(new SparkleBurst { Progress = 0.4, Color = Brushes.Gold }, 120, 120) > Pixels(new SparkleBurst { Progress = 1 }, 120, 120) + 200);

// Prévia do arraste: sprite no ponteiro, ação ao soltar, grupo e origem apagada (restaurada no fim)
var preview = vm.DragPreview; var src = vm.Boxes.Slots[0]; var emptySlot = vm.Boxes.Slots[5]; var full = vm.Boxes.Slots[1];
preview.Begin(src, 1, touch: false);
Check("origem apagada durante o arraste", src.IsDimmed && !preview.IsVisible);
preview.Move(new Point(400, 300), DragPreviewViewModel.Describe(DropMode.Move, emptySlot, src, false)); Pump();
Check("prévia segue o ponteiro", preview.IsVisible && preview.X == 414 && preview.Y == 312 && preview.Sprite is not null && preview.Action == "Mover");
Check("slot ocupado = trocar", DragPreviewViewModel.Describe(DropMode.Move, full, src, false) == "⇄ Trocar");
Check("copiar, sobrescrever e seta de caixa", DragPreviewViewModel.Describe(DropMode.Copy, full, src, false) == "+ Copiar"
    && DragPreviewViewModel.Describe(DropMode.Overwrite, emptySlot, src, false) == "Sobrescrever" && DragPreviewViewModel.Describe(DropMode.Move, null, src, true) == "Trocar de caixa…"
    && DragPreviewViewModel.Describe(DropMode.Move, src, src, false) == "");
win.CaptureRenderedFrame()?.Save(Path.Combine(work, "drag-preview.png"));
preview.End(); Pump();
Check("fim do arraste: some e restaura a origem", !preview.IsVisible && !src.IsDimmed);
preview.Begin(src, 3, touch: true); preview.Move(new Point(400, 300), "Mover");
Check("grupo e toque acima do dedo", preview.HasCount && preview.CountText == "×3" && preview.Y < 300);
preview.End();

// Desligar as animações
var main = win.GetVisualDescendants().OfType<MainView>().Single();
vm.Animations = false; Pump();
Check("desligar: classe noMotion e Motion.Enabled", main.Classes.Contains("noMotion") && !PKHeX.Modern.Theme.Motion.Enabled);
vm.Animations = true; Pump();
Check("religar", !main.Classes.Contains("noMotion") && PKHeX.Modern.Theme.Motion.Enabled);

win.Close();
Console.WriteLine(fails == 0 ? "TUDO OK" : $"{fails} FALHAS"); return fails == 0 ? 0 : 1;
