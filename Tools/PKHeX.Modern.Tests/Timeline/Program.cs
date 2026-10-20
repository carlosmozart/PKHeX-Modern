using System;
using System.IO;
using System.Linq;
using Avalonia;
using Avalonia.Headless;
using Avalonia.Threading;
using PKHeX.Core;
using PKHeX.Modern;
using PKHeX.Modern.Services;
using PKHeX.Modern.ViewModels;
using PKHeX.Modern.Views;

// Linha do tempo do save: primeira abertura sem linha, abrir de novo sem mudar nao cria linha, jogar cria "No jogo",
// salvar no app cria "Salvo no app", backup vira o ponto de partida. Save sintetico (Black em branco).
AppBuilder.Configure<App>().UseSkia().UseHeadless(new AvaloniaHeadlessPlatformOptions { UseHeadlessDrawing = false }).WithInterFont().SetupWithoutStarting();
var work = args[0]; int fails = 0;
BankStorage.Root = Path.Combine(work, "bank"); SaveBackup.Folder = Path.Combine(work, "data", "backups"); ShinyHuntStore.Folder = Path.Combine(work, "data");
void Check(string n, bool ok, string detail = "") { Console.WriteLine($"{(ok ? "OK" : "FAIL")} {n} {detail}"); if (!ok) fails++; }
void Pump() { for (int i = 0; i < 4; i++) { Dispatcher.UIThread.RunJobs(); AvaloniaHeadlessPlatform.ForceRenderTimerTick(); } }
Check("resumos isolados na pasta de teste", SaveTimeline.Folder.StartsWith(work, StringComparison.OrdinalIgnoreCase), SaveTimeline.Folder);

var sav = BlankSaveFile.Get(GameVersion.B); sav.OT = "Demo";
sav.SetBoxSlotAtIndex(new PK5 { Species = 495, Version = GameVersion.B, CurrentLevel = 5 }, 0, 0);
var path = Path.Combine(work, "Black.sav"); File.WriteAllBytes(path, sav.Write().ToArray());

MainViewModel OpenNew() { var vm = new MainViewModel(new AppSettings { CheckForUpdates = false, AskLegalizeOnClick = false }); vm.Open(path); Pump(); return vm; }

var first = OpenNew();
Check("primeira abertura: um resumo, sem linha", SaveTimeline.Count(path) == 1 && first.Home.HasNoTimeline);
OpenNew();
Check("abrir de novo sem jogar não cria linha", SaveTimeline.Count(path) == 1);

// "Jogou": horas, shiny novo, espécie nova, insígnias e dinheiro
var game = SaveUtil.GetSaveFile(path)!;
game.PlayedHours += 2; game.PlayedMinutes = 30; game.Money += 5000; ((SAV5)game).Misc.Badges |= 0b11;
var shiny = new PK5 { Species = 571, Version = GameVersion.B, CurrentLevel = 30 }; shiny.SetShiny(); shiny.RefreshChecksum();
game.SetBoxSlotAtIndex(shiny, 0, 1);
File.WriteAllBytes(path, game.Write().ToArray());
var played = OpenNew();
var entry = SaveTimeline.Get(path).FirstOrDefault();
Check("jogar cria linha \"No jogo\"", entry is { Kind: "opened" } && played.Home.HasTimeline && played.Home.TimelineEntries[0].Source == "No jogo");
var text = string.Join(" | ", entry?.Changes ?? []);
Check("horas de jogo", text.Contains("+2h30 de jogo"), text);
Check("shiny novo com sprite", text.Contains("Shiny novo: Zoroark") && entry!.NewShinies.SequenceEqual(new ushort[] { 571 }) && played.Home.TimelineEntries[0].HasShinies);
Check("espécie nova e Pokémon guardados", text.Contains("Espécies novas: Zoroark") && text.Contains("+1 Pokémon guardados"));
Check("insígnias numa linha", text.Contains("Insígnias novas: Trio, Basic"));
Check("dinheiro", text.Contains("+$5"), text);

// Salvo no app: mudar algo e gravar
var pk = played.Boxes.Slots[1].Pkm!.Clone(); pk.Species = 612; pk.RefreshChecksum();
CoreAdapter.SetBoxSlot(played.ActiveTab!.Sav, pk, 0, 2);
Check("salvar no app", played.QuickSave());
var saved = SaveTimeline.Get(path).FirstOrDefault();
Check("gravação entra como \"Salvo no app\"", saved is { Kind: "saved" } && saved.Changes.Any(c => c.Contains("Haxorus")), string.Join(" | ", saved?.Changes ?? []));
Check("a linha do tempo da aba acompanha a gravação", played.ActiveTab!.Timeline?.FirstOrDefault()?.Kind == "saved");

// Backup como ponto de partida: outro save, sem resumo, com backup de uma versão anterior
var path2 = Path.Combine(work, "Black2.sav");
var older = BlankSaveFile.Get(GameVersion.B); older.OT = "Demo"; File.WriteAllBytes(path2, older.Write().ToArray());
SaveBackup.BeforeOverwrite(path2, "Salvar", "Black");
var newer = SaveUtil.GetSaveFile(path2)!; newer.PlayedHours += 1; File.WriteAllBytes(path2, newer.Write().ToArray());
var timeline = SaveTimeline.Record(path2, newer);
Check("backup vira o ponto de partida", SaveTimeline.Count(path2) == 2 && timeline.Count == 1 && timeline[0].Changes.Any(c => c.Contains("+1h00 de jogo")),
    string.Join(" | ", timeline.SelectMany(t => t.Changes)));

Console.WriteLine(fails == 0 ? "TUDO OK" : $"{fails} FALHAS"); return fails == 0 ? 0 : 1;
