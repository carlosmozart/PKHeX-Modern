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

// Vitrine da equipe: abre pela pagina Equipe, mostra os dados de cada Pokemon, troca o layout, gera o PNG e fecha com Esc.
AppBuilder.Configure<App>().UseSkia().UseHeadless(new AvaloniaHeadlessPlatformOptions { UseHeadlessDrawing = false }).WithInterFont().SetupWithoutStarting();
var work = args[0]; int fails = 0;
BankStorage.Root = Path.Combine(work, "bank"); SaveBackup.Folder = Path.Combine(work, "backups"); ShinyHuntStore.Folder = Path.Combine(work, "data");
void Check(string n, bool ok, string detail = "") { Console.WriteLine($"{(ok ? "OK" : "FAIL")} {n} {detail}"); if (!ok) fails++; }
void Pump() { for (int i = 0; i < 4; i++) { Dispatcher.UIThread.RunJobs(); AvaloniaHeadlessPlatform.ForceRenderTimerTick(); } }

var vm = new MainViewModel(new AppSettings { CheckForUpdates = false, AskLegalizeOnClick = false });
var win = new MainWindow { DataContext = vm, Width = 1500, Height = 1000 }; win.Show();

// Save sem equipe: avisa e nao abre
var empty = BlankSaveFile.Get(GameVersion.B); empty.OT = "Demo";
var emptyPath = Path.Combine(work, "empty.sav"); File.WriteAllBytes(emptyPath, empty.Write().ToArray());
vm.Open(emptyPath); Pump();
vm.Party.ShowcaseCommand!.Execute(null); Pump();
Check("equipe vazia não abre a vitrine", !vm.HasShowcase && vm.Status.Contains("vazia"), vm.Status);

// Scarlet (copia): o primeiro da equipe ganha item, golpes e Tera conhecidos
var svPath = Path.Combine(work, "sv.sav");
var sav = SaveUtil.GetSaveFile(svPath)!;
var first = sav.GetPartySlotAtIndex(0);
first.HeldItem = 234; // Leftovers
CoreAdapter.SetMove(first, 0, 89); CoreAdapter.SetMove(first, 1, 0); CoreAdapter.SetMove(first, 2, 0); CoreAdapter.SetMove(first, 3, 0);
((ITeraType)first).TeraTypeOverride = MoveType.Fairy;
first.RefreshChecksum(); sav.SetPartySlotAtIndex(first, 0);
File.WriteAllBytes(svPath, sav.Write().ToArray());
vm.Open(svPath); vm.CurrentPage = vm.Party; Pump();
int partyCount = vm.Party.Slots.Count(s => !s.IsEmpty);
vm.Party.ShowcaseCommand!.Execute(null); Pump();
var show = vm.Showcase;
Check("vitrine abre com a equipe", show is not null && show.Members.Count == partyCount && partyCount > 0, $"{show?.Members.Count}/{partyCount}");
var m = show!.Members[0];
Check("título com o treinador", show.Title == $"Equipe de {sav.OT}" && show.Subtitle.Length > 0, show.Subtitle);
Check("item com nome e ícone", m.Item == "Leftovers" && m.ItemSprite is not null, m.Item);
Check("golpes só os preenchidos", m.Moves.Count == 1 && m.Moves[0].Name == "Earthquake", string.Join(",", m.Moves.Select(x => x.Name)));
Check("tera do Scarlet", m.Tera?.Name == "Fairy", m.Tera?.Name ?? "-");
Check("tipos, habilidade e natureza", m.Types.Count >= 1 && m.HasAbility && m.HasNature && m.Sprite is not null);

var view = win.GetVisualDescendants().OfType<TeamShowcaseView>().Single();
byte[] Png() { Pump(); return view.RenderPng() ?? []; }
static bool IsPng(byte[] b) => b.Length > 1000 && b[0] == 0x89 && b[1] == (byte)'P' && b[2] == (byte)'N' && b[3] == (byte)'G';
var grid = Png();
Check("imagem da grade", IsPng(grid), $"{grid.Length} bytes");
show.Layout = 1;
var columns = Png();
Check("imagem das colunas é outra", IsPng(columns) && !grid.SequenceEqual(columns));
File.WriteAllBytes(Path.Combine(work, "showcase.png"), columns);

win.KeyPressQwerty(PhysicalKey.Escape, RawInputModifiers.None); Pump();
Check("Esc fecha a vitrine", !vm.HasShowcase);
// Ficha do Pokemon no editor: tipos, Tera, nivel/natureza, origem; acompanha a edicao
var select = vm.SelectSlotAsync(vm.Party.Slots[0]);
for (int i = 0; i < 2000 && !select.IsCompleted; i++) { Pump(); if (vm.Dialog is { } d) d.Complete(false); System.Threading.Thread.Sleep(5); }
var ed = vm.Editor!;
Check("ficha: brilho e Tera", ed.CardGlow is not null && ed.CardTera?.Name == "Fairy", ed.CardTera?.Name ?? "-");
Check("ficha: nível e natureza", ed.CardLine.StartsWith("Nv. ") && ed.CardLine.Contains(" · "), ed.CardLine);
Check("ficha: jogo de origem", ed.HasCardOrigin && ed.CardOrigin.Contains("Gen "), ed.CardOrigin);
Check("hexágono: valores, cor do tipo", ed.RadarTexts is { Count: 6 } && ed.RadarBrush is not null && ed.RadarModifiers is { Count: 6 });
ed.Nature = (int)Nature.Adamant; Pump();
Check("hexágono: natureza +Atq −AtE", ed.RadarModifiers![1] == 1 && ed.RadarModifiers[3] == -1, string.Join(",", ed.RadarModifiers));
var evBefore = ed.RadarEVs;
ed.Stats[1].EV = 252; Pump();
Check("hexágono: EVs na segunda camada", ed.RadarEVs is { Count: 6 } evs && evs[1] == 1, evBefore is null ? "antes sem EVs" : "");
ed.Level = 77; Pump();
Check("ficha acompanha o nível", ed.CardLine.StartsWith("Nv. 77"), ed.CardLine);
win.Close();
Console.WriteLine(fails == 0 ? "TUDO OK" : $"{fails} FALHAS"); return fails == 0 ? 0 : 1;
