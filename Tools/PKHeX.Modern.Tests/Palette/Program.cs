using System;
using System.IO;
using System.Linq;
using Avalonia;
using Avalonia.Headless;
using Avalonia.Input;
using Avalonia.Threading;
using Avalonia.VisualTree;
using PKHeX.Core;
using PKHeX.Modern;
using PKHeX.Modern.Services;
using PKHeX.Modern.ViewModels;
using PKHeX.Modern.Views;

// Paleta de comandos: Ctrl+K abre, a busca acha acoes, paginas, caixas e Pokemon, Enter executa e Esc fecha.
AppBuilder.Configure<App>().UseSkia().UseHeadless(new AvaloniaHeadlessPlatformOptions { UseHeadlessDrawing = false }).WithInterFont().SetupWithoutStarting();
var work = args[0]; int fails = 0;
BankStorage.Root = Path.Combine(work, "bank"); SaveBackup.Folder = Path.Combine(work, "backups"); ShinyHuntStore.Folder = Path.Combine(work, "data");
void Check(string n, bool ok, string detail = "") { Console.WriteLine($"{(ok ? "OK" : "FAIL")} {n} {detail}"); if (!ok) fails++; }
void Pump() { for (int i = 0; i < 4; i++) { Dispatcher.UIThread.RunJobs(); AvaloniaHeadlessPlatform.ForceRenderTimerTick(); } }

var sav = BlankSaveFile.Get(GameVersion.B); sav.OT = "Demo";
sav.SetPartySlotAtIndex(new PK5 { Species = 25, Version = GameVersion.B, CurrentLevel = 12 }, 0);
var nick = new PK5 { Species = 133, Version = GameVersion.B, CurrentLevel = 30 }; nick.Nickname = "Fofinho"; nick.IsNicknamed = true;
sav.SetBoxSlotAtIndex(nick, 2, 7);
((IBoxDetailName)sav).SetBoxName(4, "Lendários");
var path = Path.Combine(work, "Black.sav"); File.WriteAllBytes(path, sav.Write().ToArray());

var vm = new MainViewModel(new AppSettings { CheckForUpdates = false });
var win = new MainWindow { DataContext = vm, Width = 1500, Height = 1000 }; win.Show();
vm.Open(path); vm.CurrentPage = vm.Boxes; Pump();

win.KeyPressQwerty(PhysicalKey.K, RawInputModifiers.Control); Pump();
Check("Ctrl+K abre a paleta", vm.HasPalette);
var p = vm.Palette!;
Check("sem busca lista ações e páginas", p.Results.Any(r => r.Title == "Abrir save") && p.Results.Any(r => r.Title == "Ir para Mochila"), $"{p.Results.Count} itens");
string Top(string q) { p.Query = q; Pump(); return p.Results.FirstOrDefault()?.Title ?? "-"; }
Check("ação sem acento", Top("verificar legalidade") == "Verificar legalidade do save", Top("verificar legalidade"));
Check("página sem acento", Top("pokedex") == "Ir para Pokédex", Top("pokedex"));
Check("caixa pelo número", Top("caixa 3") == "Ir para a caixa 3", Top("caixa 3"));
Check("caixa pelo nome", p.Results.Count >= 0 && Top("lendarios") == "Ir para a caixa Lendários", Top("lendarios"));
Check("Pokémon pela espécie", Top("pika") == "Pikachu", Top("pika"));
Check("Pokémon pelo apelido", Top("fofi") == "Fofinho (Eevee)", Top("fofi"));
p.Query = "zzzz"; Pump(); Check("nada encontrado", p.IsEmpty);

// Enter na caixa de texto executa o primeiro resultado
p.Query = "fofi"; Pump();
var view = win.GetVisualDescendants().OfType<CommandPaletteView>().Single();
var input = view.GetVisualDescendants().OfType<Avalonia.Controls.TextBox>().Single(t => t.Name == "PaletteInput");
input.Focus(); Pump();
win.KeyPressQwerty(PhysicalKey.Enter, RawInputModifiers.None); Pump();
for (int i = 0; i < 50 && vm.Editor is null; i++) Pump();
Check("Enter vai até o Pokémon", !vm.HasPalette && vm.CurrentPage == vm.Boxes && vm.Boxes.CurrentBox == 2 && vm.Boxes.Slots[7].IsSelected, $"caixa {vm.Boxes.CurrentBox + 1}");

// Setas e Esc
vm.OpenPalette(); Pump(); p = vm.Palette!; p.Query = "ir para"; Pump();
p.Move(+1); Check("seta para baixo", p.SelectedIndex == 1);
p.Move(-2); Check("seta circular", p.SelectedIndex == p.Results.Count - 1);
win.KeyPressQwerty(PhysicalKey.Escape, RawInputModifiers.None); Pump();
Check("Esc fecha", !vm.HasPalette);

// Ir para caixa executa
vm.OpenPalette(); p = vm.Palette!; p.Query = "caixa 5"; p.RunSelected(); Pump();
Check("caixa 5 abre a caixa", vm.CurrentPage == vm.Boxes && vm.Boxes.CurrentBox == 4);

// Interface em inglês: a busca acha pelo texto traduzido
Loc.Load(Loc.English);
vm.OpenPalette(); p = vm.Palette!;
Check("inglês: bag acha a Mochila", Top("bag") == "Ir para Mochila", Top("bag"));
Check("inglês: undo/legalize", p.Results.Count >= 0 && Top("legality") == "Verificar legalidade do save", Top("legality"));
Loc.Load(Loc.Portuguese);
vm.ClosePalette();
win.Close();
Console.WriteLine(fails == 0 ? "TUDO OK" : $"{fails} FALHAS"); return fails == 0 ? 0 : 1;
