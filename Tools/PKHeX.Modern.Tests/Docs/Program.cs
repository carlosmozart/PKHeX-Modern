using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Headless;
using Avalonia.Threading;
using PKHeX.Core;
using PKHeX.Modern;
using PKHeX.Modern.Services;
using PKHeX.Modern.ViewModels;
using PKHeX.Modern.Views;

// Public documentation captures use generated saves only; no personal save or desktop capture.
// Uso: -- <pasta> [idioma]. Com o idioma (en, es ou zh-Hans), as mesmas capturas saem na interface desse idioma em <pasta>/<idioma>/,
// para a pagina de divulgacao naquele idioma (menos a captura "english").
AppBuilder.Configure<App>().UseSkia().UseHeadless(new AvaloniaHeadlessPlatformOptions { UseHeadlessDrawing = false }).WithInterFont().With(PKHeX.Modern.Theme.FontFallbacks.Options(android: false)).SetupWithoutStarting();
var lang = args.Length > 1 ? args[1] : null;
var output = Path.GetFullPath(lang is null ? args[0] : Path.Combine(args[0], lang)); Directory.CreateDirectory(output);
if (lang is not null && (!Loc.Load(lang) || !Loc.IsTranslating))
    throw new ArgumentException("Idioma sem dicionário: " + lang);
Loc.Hook();
// Numeros e datas no formato de quem usa o app nesse idioma (o PC das capturas esta em pt-BR: "14,0%" em vez de "14.0%").
if (lang is not null) System.Globalization.CultureInfo.CurrentCulture = new(lang switch { Loc.English => "en-US", Loc.Spanish => "es-ES", Loc.ChineseSimplified => "zh-CN", _ => lang });
// Nomes gravados nos saves de exemplo (caixa, banco, cemiterio do Nuzlocke) e idioma dos saves: o das capturas.
var collection = lang switch { Loc.Spanish => "Colección", Loc.English => "Collection", Loc.ChineseSimplified => "收藏", _ => "Coleção" };
var graveyard = lang switch { Loc.Spanish => "Cementerio", Loc.English => "Graveyard", Loc.ChineseSimplified => "墓地", _ => "Cemitério" };
// Black (Gen 5) nao tem save em chines: as capturas em chines usam um save em ingles.
var saveLanguage = lang == Loc.Spanish ? (int)LanguageID.Spanish : (int)LanguageID.English;
var work = Path.Combine(Path.GetTempPath(), "pkhex-docs-" + Guid.NewGuid()); Directory.CreateDirectory(work);
var library = Path.Combine(work, "saves"); Directory.CreateDirectory(library);
BankStorage.Root = Path.Combine(work, "bank"); SaveBackup.Folder = Path.Combine(work, "backups"); ShinyHuntStore.Folder = Path.Combine(work, "data");
CoreAdapter.SetLanguage("en");
PKM Make(SaveFile sav, ushort species)
{
    foreach (var enc in EncounterDatabase.SearchEncounters(sav, species, true).Where(e => e.Species == species))
        if (EncounterDatabase.ToEntity(sav, enc, out _) is { IsEgg: false } pk && new LegalityAnalysis(pk).Valid) return pk;
    throw new Exception("Sem encontro legal: " + species);
}
var sav = BlankSaveFile.Get(GameVersion.B); sav.OT = "Demo"; sav.Language = saveLanguage;
sav.SecondsToStart = (uint)(new DateTime(2012, 5, 14) - new DateTime(2000, 1, 1)).TotalSeconds;
CoreAdapter.Activate(sav);
ushort[] species = [133, 25, 175, 447, 42, 113, 1, 4, 7, 129, 147, 143, 172, 54, 37];
for (int i = 0; i < species.Length; i++)
{
    var pk = Make(sav, species[i]); sav.SetBoxSlotAtIndex(pk, 0, i);
    if (i < 6) sav.SetPartySlotAtIndex(pk.Clone(), i);
}
((IBoxDetailName)sav).SetBoxName(0, collection);
((IBoxDetailWallpaper)sav).SetBoxWallpaper(0, 0);
var black = Path.Combine(library, "Black.sav"); File.WriteAllBytes(black, sav.Write().ToArray());
foreach (var version in new[] { GameVersion.BD, GameVersion.SP })
{
    var demo = BlankSaveFile.Get(version); demo.OT = "Demo"; demo.Language = 2;
    File.WriteAllBytes(Path.Combine(library, version + ".sav"), demo.Write().ToArray());
}
using (var zip = ZipFile.Open(Path.Combine(library, "Black-backup.zip"), ZipArchiveMode.Create))
    zip.CreateEntryFromFile(black, "main.sav");
var bank = collection; BankStorage.CreateBank(bank);
var bankBox = BankStorage.GetBoxes(bank).First();
for (int i = 0; i < species.Length; i++) BankStorage.WriteSlot(bankBox, i, sav.GetBoxSlotAtIndex(0, i));
// Sem animacoes: a Pokebola de abertura aparecia no meio da captura do Inicio
var vm = new MainViewModel(new AppSettings { SavesFolder = library, CheckForUpdates = false, Animations = false, UiLanguage = lang ?? Loc.Portuguese });
// Caminho neutro na pagina Saves: a pasta temporaria tem o nome do usuario do Windows.
vm.SaveManager.DescribeFolder = () => "Documentos/PKHeX/saves";
var win = new MainWindow { DataContext = vm, Width = 1500, Height = 1100 }; win.Show();
// Nomes das especies no idioma dos nomes do jogo na tela (em chines, Eevee e 伊布).
string Name(ushort species) => CoreAdapter.SpeciesNames[species];
void Pump() { for (int i = 0; i < 4; i++) { Dispatcher.UIThread.RunJobs(); AvaloniaHeadlessPlatform.ForceRenderTimerTick(); } }
void Wait(Task task) { for (int i = 0; i < 6000 && !task.IsCompleted; i++) { Pump(); System.Threading.Thread.Sleep(5); } if (!task.IsCompleted) throw new TimeoutException(); task.GetAwaiter().GetResult(); Pump(); }
void Shot(string name) { Pump(); win.CaptureRenderedFrame()!.Save(Path.Combine(output, name + ".png")); Console.WriteLine(name); }
vm.Open(black); Shot("home"); vm.CurrentPage = vm.Boxes; Wait(vm.SelectSlotAsync(vm.Boxes.Slots[0])); Shot("boxes"); Shot("friendship");
vm.Editor!.SelectedTab = 2; Shot("moves");
vm.Editor.SelectedTab = 6; Shot("ribbons");
vm.Editor.SelectedTab = 3; Shot("legalmode");
vm.Editor.SelectedTab = 0;
vm.SearchText = Name(133); vm.RunSearch(); Shot("search"); vm.SearchText = ""; vm.RunSearch();
vm.CurrentPage = vm.Party; Wait(vm.SelectSlotAsync(vm.Party.Slots[0])); Shot("party");
vm.CurrentPage = vm.Bank; vm.Bank.SelectedBank = bank; vm.Bank.Reload(); Shot("bank");
vm.CurrentPage = vm.Pokedex; Wait(vm.Pokedex.RefreshAsync()); Shot("pokedex");
var trainer = (TrainerPageViewModel)vm.Pages.First(p => p is TrainerPageViewModel); vm.CurrentPage = trainer;
foreach (var b in trainer.BadgeGroups[0].Badges.Take(5)) b.Has = true; trainer.BP = 248; trainer.GenderIndex = 1; Shot("trainer");
vm.CurrentPage = vm.Encounters; vm.Encounters.Species = Name(133); Wait(vm.Encounters.SearchAsync()); Shot("encounters");
vm.Encounters.Selected = vm.Encounters.Results.First(); Shot("encounter_detail"); vm.Encounters.CloseDetailCommand.Execute(null);
vm.CurrentPage = vm.SaveManager; Wait(vm.SaveManager.RefreshAsync()); Shot("savemanager");
vm.IsHelpOpen = true; Shot("help"); vm.IsHelpOpen = false;
vm.CurrentPage = vm.Boxes; win.Width = 1100; win.Height = 800; Shot("compact"); win.Width = 1500; win.Height = 1100;
// Fabrica de ovos: confirma a lista sozinho e mostra a caixa com os ovos; depois desfaz
void Run(Task task)
{
    for (int i = 0; i < 6000 && !task.IsCompleted; i++) { Pump(); if (vm.Dialog is { } d) d.Complete(true); System.Threading.Thread.Sleep(5); }
    task.GetAwaiter().GetResult(); Pump();
}
vm.Boxes.CurrentBox = 0; Run(vm.EggFactoryAsync(true)); vm.Boxes.CurrentBox = 1; Shot("eggs"); vm.Boxes.CurrentBox = 0;
vm.UndoCommand.Execute(null); Pump();
// Modo Hardcore: editor so leitura e o cartao no lugar do modo legal
vm.Hardcore = true; vm.CurrentPage = vm.Boxes; Wait(vm.SelectSlotAsync(vm.Boxes.Slots[0])); Shot("hardcore"); vm.Hardcore = false;
// Relatorio de Nuzlocke: um save so com capturas do proprio treinador, em rotas diferentes e com datas
var nz = BlankSaveFile.Get(GameVersion.B); nz.OT = "Demo"; nz.Language = saveLanguage; CoreAdapter.Activate(nz);
var probe = Make(nz, 504);
// Rotas pelo nome em ingles, qualquer que seja o idioma dos nomes do jogo na tela
var englishNames = GameInfo.GetStrings("en");
ushort Route(string name) => (ushort)CoreAdapter.GetMetLocations(probe).First(x => englishNames.GetLocationName(false, (ushort)x.Value, 5, 5, GameVersion.B) == name).Value;
PKM Caught(ushort species, string route, int day, int level)
{
    var pk = Make(nz, species); pk.Version = nz.Version; pk.MetLocation = Route(route); pk.MetDate = new DateOnly(2012, 5, day);
    pk.CurrentLevel = Math.Max(pk.CurrentLevel, (byte)level); pk.RefreshChecksum(); return pk;
}
(ushort, string, int, int)[] run = [(504, "Route 1", 14, 24), (506, "Route 1", 14, 22), (509, "Route 2", 15, 25), (519, "Route 3", 16, 27), (531, "Route 3", 16, 23),
    (527, "Wellspring Cave", 17, 21), (524, "Wellspring Cave", 19, 20), (532, "Pinwheel Forest", 18, 26), (540, "Pinwheel Forest", 18, 22), (543, "Pinwheel Forest", 18, 24),
    (546, "Route 4", 20, 28), (551, "Route 4", 20, 26), (557, "Desert Resort", 21, 27), (554, "Desert Resort", 23, 29), (562, "Relic Castle", 24, 28), (505, "Route 5", 25, 30)];
int partyN = 0, boxN = 0;
foreach (var (sp, route, day, level) in run)
{
    var pk = Caught(sp, route, day, level);
    if (partyN < 6 && (sp is 504 or 509 or 519 or 532 or 546 or 554)) nz.SetPartySlotAtIndex(pk, partyN++);
    else if (sp is 527 or 557) nz.SetBoxSlotAtIndex(pk, 1, sp == 527 ? 0 : 1); // morreram
    else nz.SetBoxSlotAtIndex(pk, 0, boxN++);
}
((IBoxDetailName)nz).SetBoxName(1, graveyard);
var nzPath = Path.Combine(library, "Nuzlocke.sav"); File.WriteAllBytes(nzPath, nz.Write().ToArray());
vm.Open(nzPath); vm.CurrentPage = vm.Pokedex; vm.Pokedex.ShowNuzlockeCommand.Execute(null); Shot("nuzlocke");
vm.Pokedex.ShowCollectionCommand.Execute(null);
var bd = BlankSaveFile.Get(GameVersion.BD); bd.OT = "Demo"; CoreAdapter.Activate(bd);
var feebas = Make(bd, 349); ((IContestStats)feebas).ContestBeauty = 0; ((IContestStats)feebas).ContestSheen = 0; // o botao mostra "Beauty sobe de 0 para 170"
bd.SetBoxSlotAtIndex(feebas, 0, 0); var bdPath = Path.Combine(library, "Feebas.sav"); File.WriteAllBytes(bdPath, bd.Write().ToArray());
vm.Open(bdPath); Wait(vm.SelectSlotAsync(vm.Boxes.Slots[0])); vm.Editor!.SelectedTab = 0; Shot("beauty");
// Set do Showdown legal: cola um set competitivo num slot vazio do Black e mostra o resultado legalizado
Wait(vm.OpenAsync(black)); vm.CurrentPage = vm.Boxes; Wait(vm.SelectSlotAsync(vm.Boxes.Slots[20]));
vm.Editor!.ImportShowdown(string.Join('\n', "Haxorus @ Life Orb", "Ability: Mold Breaker", "EVs: 252 Atk / 4 SpD / 252 Spe", "Jolly Nature",
    "- Dragon Dance", "- Outrage", "- Earthquake", "- Brick Break"));
for (int i = 0; i < 6000 && vm.Editor.IsLegalizing; i++) { Pump(); System.Threading.Thread.Sleep(5); }
vm.Editor.SelectedTab = 0; Shot("showdown"); Run(vm.SelectSlotAsync(vm.Boxes.Slots[0])); // descarta o set colado (a pergunta e respondida pelo Run)
// Saves dos emuladores (Android): uma "memoria" com os saves sinteticos nas pastas do DraStic, melonDS e Eden
var storage = Path.Combine(work, "storage");
void PutSave(string source, string relative)
{
    var dest = Path.Combine(storage, relative.Replace('/', Path.DirectorySeparatorChar));
    Directory.CreateDirectory(Path.GetDirectoryName(dest)!); File.Copy(source, dest, true);
}
PutSave(black, "DraStic/backup/POKEMON B.dsv");
PutSave(nzPath, "melonDS/saves/Pokemon Black - Nuzlocke.sav");
PutSave(Path.Combine(library, "BD.sav"), "eden/nand/user/save/0000000000000000/00000000000000000000000000000001/0100000011D90000/main");
EmulatorSaves.StorageRoot = storage; EmulatorSaves.HasAccess = () => true;
var emptyLibrary = Path.Combine(work, "vazia"); Directory.CreateDirectory(emptyLibrary);
vm.Settings.EmulatorScan = true; vm.SaveManager.Folder = emptyLibrary; vm.SaveManager.DescribeFolder = () => "📱 Memória do aparelho"; vm.SaveManager.RaiseFolder();
vm.CurrentPage = vm.SaveManager; Wait(vm.SaveManager.RefreshAsync()); Shot("emulators");
vm.Settings.EmulatorScan = false; EmulatorSaves.StorageRoot = null; EmulatorSaves.HasAccess = null; vm.SaveManager.Folder = library; vm.SaveManager.DescribeFolder = () => "Documentos/PKHeX/saves"; vm.SaveManager.RaiseFolder();
// Temas completos: o Inicio em cada tema (e o Pixel tambem no claro), depois volta ao Padrao.
// Caçada shiny: Patrat no Black, com encontros contados e a ficha do alvo carregada
Wait(vm.OpenAsync(black)); vm.CurrentPage = vm.ShinyHunt; vm.ShinyHunt.TargetName = Name(504);
vm.ShinyHunt.Step = 1234; vm.ShinyHunt.AddStepCommand.Execute(null);
for (int i = 0; i < 400 && vm.ShinyHunt.IsLoadingLocations; i++) { Pump(); System.Threading.Thread.Sleep(10); }
Shot("shinyhunt"); vm.ShinyHunt.NewHuntCommand.Execute(null);
Pump(); vm.Dialog?.Complete(true); Pump(); // "Apagar caçada": sem responder, a pergunta ficava por cima das capturas seguintes
Wait(vm.OpenAsync(black)); vm.CurrentPage = vm.Home; // a aba do Black (com equipe)
foreach (var key in new[] { "pss", "pixel", "za", "oras", "kanto" })
{
    vm.ThemeOptions.First(o => o.Preset.Key == key).SelectCommand.Execute(null);
    if (key != "kanto") Shot("theme_" + key);
    if (key is "pixel" or "kanto") { Avalonia.Application.Current!.RequestedThemeVariant = Avalonia.Styling.ThemeVariant.Light; Shot($"theme_{key}_light"); Avalonia.Application.Current.RequestedThemeVariant = Avalonia.Styling.ThemeVariant.Dark; }
}
vm.ThemeOptions.First(o => o.Preset.Key == "default").SelectCommand.Execute(null);
// Android: o layout do celular (MobileShell) em paisagem, numa janela com a proporção de um celular
var phone = new Avalonia.Controls.Window { Width = 1536, Height = 650, Content = new MobileShell(vm) }; phone.Show();
vm.CurrentPage = vm.Party; Wait(vm.SelectSlotAsync(vm.Party.Slots[1]));
Pump(); phone.UpdateLayout(); Pump(); phone.CaptureRenderedFrame()!.Save(Path.Combine(output, "android.png")); Console.WriteLine("android");
phone.Content = null; phone.Close();
if (lang is not null)
{
    Console.WriteLine("Capturas sintéticas concluídas: " + output);
    return;
}
// Interface em ingles: a traducao so e instalada na partida, entao vai por ultimo, numa janela nova.
Loc.Load(Loc.English); Loc.Hook();
var vmEn = new MainViewModel(new AppSettings { SavesFolder = library, CheckForUpdates = false, Animations = false, UiLanguage = Loc.English });
win.Close(); win = new MainWindow { DataContext = vmEn, Width = 1500, Height = 1100 }; win.Show();
vmEn.Open(black); Wait(vmEn.SelectSlotAsync(vmEn.Boxes.Slots[0])); Shot("english");
Console.WriteLine("Capturas sintéticas concluídas: " + output);
