using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
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

// Interface em ingles (ou em espanhol, no projeto LanguageEs, e em chines simplificado, no LanguageZh): cada pagina e
// renderizada e os textos visiveis que ficaram em portugues sao listados.
#if LANG_ES
var lang = Loc.Spanish;
#elif LANG_ZH
var lang = Loc.ChineseSimplified;
#else
var lang = Loc.English;
#endif
bool es = lang == Loc.Spanish;
bool zh = lang == Loc.ChineseSimplified;
// Idiomas com dicionario proprio conferido contra o en.json e com nomes do jogo do PKHeX.Core no mesmo idioma.
bool foreign = es || zh;
string Pick(string en, string esText, string zhText) => zh ? zhText : es ? esText : en;
AppBuilder.Configure<App>().UseSkia().UseHeadless(new AvaloniaHeadlessPlatformOptions { UseHeadlessDrawing = false }).WithInterFont().With(PKHeX.Modern.Theme.FontFallbacks.Options(android: false)).SetupWithoutStarting();
int fails = 0;
void Check(string name, bool ok) { Console.WriteLine($"{(ok ? "OK" : "FAIL")} {name}"); if (!ok) fails++; }
var work = args.Length > 0 ? args[0] : Path.Combine(Path.GetTempPath(), "pkhex-lang-" + Guid.NewGuid()); Directory.CreateDirectory(work);
BankStorage.Root = Path.Combine(work, "bank"); SaveBackup.Folder = Path.Combine(work, "backups");

// Unidade: exato, modelo, frases juntadas, prefixo de icone e texto sem traducao.
Check("carrega " + lang, Loc.Load(lang) && Loc.IsTranslating);
Check("exato", Loc.T("Caixas") == Pick("Boxes", "Cajas", "盒子"));
Check("modelo", Loc.T("Pikachu gravado em caixa 1, slot 2. Lembre-se de salvar.") == Pick("Pikachu written to box 1, slot 2. Remember to save.",
    "Pikachu grabado en caja 1, casilla 2. Recuerda guardar.", "Pikachu 已写入 盒子 1，格子 2。别忘了保存。"));
Check("modelo aninhado", Loc.T("Pikachu ({0})".Replace("{0}", "x")) == "Pikachu (x)");
Check("nome do jogo intacto", Loc.T("Pikachu") == "Pikachu" && Loc.T("Thunderbolt") == "Thunderbolt");
Check("frases juntadas", Loc.T("Nada para desfazer (Ctrl+Z). Nada para refazer (Ctrl+Y).") is var j && !j.Contains("desfazer") && !j.Contains("refazer"));
if (foreign)
{
    // O dicionario tem as mesmas chaves do ingles, com os mesmos marcadores e quebras de linha (o formato de um
    // marcador pode mudar, como a data em chines: {0:yyyy/MM/dd}).
    static Dictionary<string, string> Dict(string code)
    {
        using var stream = typeof(Loc).Assembly.GetManifestResourceStream($"lang.{code}.json")!;
        return System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(stream)!;
    }
    var enDict = Dict(Loc.English); var langDict = Dict(lang);
    var marks = new Regex(@"\{(\d+)(:[^}]*)?\}");
    string Marks(string t) => string.Join(",", marks.Matches(t).Select(m => m.Groups[1].Value).Order());
    Check($"{lang}.json com as {enDict.Count} chaves do en.json", enDict.Keys.All(langDict.ContainsKey) && langDict.Count == enDict.Count);
    var badMarks = langDict.Where(e => e.Value.Length == 0 || Marks(e.Key) != Marks(e.Value) || e.Key.Count(c => c == '\n') != e.Value.Count(c => c == '\n')).Select(e => e.Key).ToList();
    foreach (var b in badMarks.Take(20)) Console.WriteLine("  marcadores/quebras: " + b);
    Check($"{lang}.json com os mesmos marcadores e quebras", badMarks.Count == 0);
}
Loc.Hook();

PKM Make(SaveFile sav, ushort species)
{
    foreach (var enc in EncounterDatabase.SearchEncounters(sav, species, true).Where(e => e.Species == species))
        if (EncounterDatabase.ToEntity(sav, enc, out _) is { IsEgg: false } pk && new LegalityAnalysis(pk).Valid) return pk;
    throw new Exception("Sem encontro legal: " + species);
}
var sav = BlankSaveFile.Get(GameVersion.B); sav.OT = "Demo"; CoreAdapter.Activate(sav);
int slot = 0;
foreach (ushort sp in new ushort[] { 495, 498, 501, 25, 570, 349 }) { var pk = Make(sav, sp); sav.SetPartySlotAtIndex(pk, slot); sav.SetBoxSlotAtIndex(pk.Clone(), 0, slot++); }
var lib = Path.Combine(work, "saves"); Directory.CreateDirectory(lib);
var path = Path.Combine(lib, "Black.sav"); File.WriteAllBytes(path, sav.Write().ToArray());

var vm = new MainViewModel(new AppSettings { SavesFolder = lib, UiLanguage = lang });
// Nomes do jogo: em espanhol, os do PKHeX.Core em espanhol (como no PKHeX original); em ingles, os de sempre.
Check("nomes do jogo no idioma da interface", GameInfo.CurrentLanguage == Pick("en", "es", "zh-Hans") && CoreAdapter.MoveNames[85] == Pick("Thunderbolt", "Rayo", "十万伏特"));
Check("opção dos nomes " + (foreign ? "visível" : "escondida"), vm.HasGameNamesOption == foreign);
var win = new MainWindow { DataContext = vm, Width = 1440, Height = 900 }; win.Show();
void Pump(int n = 10) { for (int i = 0; i < n; i++) { Dispatcher.UIThread.RunJobs(); AvaloniaHeadlessPlatform.ForceRenderTimerTick(); } }
// No espanhol, acentos e palavras como "de", "para", "que", "abrir" e "dos" existem nos dois idiomas: conta so o que e exclusivo do portugues.
var pt = es
    ? new Regex(@"[ãõçâêô]|ção\b|ções\b|ões\b|\b(não|uma|em|ou|os|das|caixas?|equipe|golpes?|treinador|salvar|clique|nenhuma?|selecione|escolha|arquivo|pasta|nova|voltar|excluir|ajuda|atualizar|você)\b", RegexOptions.IgnoreCase)
    : new Regex(@"[ãõçâêôáíóúàé]|\b(de|da|não|para|com|uma|os|em|ou|que|caixa|equipe|golpes?|treinador|mochila|salvar|abrir|clique|todos|todas|nenhum|selecione|escolha|arquivo|pasta|novo|nova|voltar|excluir|ordenar|buscar|ajuda|atualizar|vista|capturada)\b", RegexOptions.IgnoreCase);
var gameWords = new Regex(@"Pok[ée]\w*|Poké", RegexOptions.IgnoreCase);
bool IsPortuguese(string t) => pt.IsMatch(gameWords.Replace(t, ""));
var missing = new SortedDictionary<string, string>();
// Espanhol e chines: textos curtos que nao passaram pelo dicionario (iguais em portugues e ingles, como "Saves", nao
// entram no en.json). So para conferir a mao em untranslated.txt; nomes do jogo tambem aparecem.
var esValues = foreign ? new HashSet<string>(LangValues(lang)) : [];
static IEnumerable<string> LangValues(string code)
{
    using var stream = typeof(Loc).Assembly.GetManifestResourceStream($"lang.{code}.json")!;
    return System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(stream)!.Values;
}
var untranslated = new SortedDictionary<string, string>();
void Scan(string page, Control? popup = null)
{
    Pump(20); win.UpdateLayout(); Pump(5);
    foreach (var v in win.GetVisualDescendants().Concat(popup?.GetVisualDescendants() ?? []))
    {
        var texts = new List<string?>();
        if (v is TextBlock tb && tb.IsEffectivelyVisible) texts.Add(tb.Text);
        if (v is Control c && ToolTip.GetTip(c) is string tip) texts.Add(tip);
        if (v is TextBox box) texts.Add(box.Watermark);
        if (v is ComboBox cb) texts.Add(cb.PlaceholderText);
        foreach (var t in texts)
        {
            if (!string.IsNullOrWhiteSpace(t) && IsPortuguese(t) && !missing.ContainsKey(t) && !t.Contains("Português") && !t.StartsWith("¡") && !t.StartsWith("http")) missing[t] = page;
            if (foreign && t is { Length: > 1 and <= 40 } && t.Any(char.IsLetter) && !esValues.Contains(t)) untranslated.TryAdd(t, page);
        }
    }
}
Pump(); Scan("Save Manager");
vm.Open(path); Pump(); Scan("Início");
foreach (var page in vm.Pages)
{
    vm.CurrentPage = page; Scan(page.Title);
    if (page == vm.Boxes)
    {
        vm.SelectSlotAsync(vm.Boxes.Slots[5]).GetAwaiter().GetResult();
        for (int t = 0; t < 7; t++) { vm.Editor!.SelectedTab = t; Scan("Editor " + t); }
        vm.Editor!.SelectedTab = 0;
        win.CaptureRenderedFrame()?.Save(Path.Combine(work, $"boxes_{lang}.png"));
    }
}
vm.CurrentPage = vm.Pokedex; vm.Pokedex.RefreshAsync().GetAwaiter().GetResult(); Scan("Pokédex");
// Completar Pokedex: menu do botao e pergunta de confirmacao (com e sem shiny).
var dexButton = win.GetVisualDescendants().OfType<Button>().FirstOrDefault(b => b.Flyout is Flyout && b.IsEffectivelyVisible);
if (dexButton?.Flyout is Flyout dexFlyout) { dexFlyout.ShowAt(dexButton); Scan("Completar Pokédex (menu)", dexFlyout.Content as Control); dexFlyout.Hide(); }
foreach (var shiny in new[] { false, true })
    foreach (var action in new[] { "Complete", "SeenAll", "CaughtNone", "Clear" })
    {
        vm.Pokedex.IncludeShiny = shiny; vm.Pokedex.DexActionCommand.Execute(action); Scan("Completar Pokédex " + action); vm.Dialog?.Complete(false); Pump();
    }
var living = vm.Pokedex.LivingDex!;
living.TargetIndex = Array.IndexOf(LivingDexViewModel.Versions.ToArray(), GameVersion.B);
vm.Pokedex.ShowLivingDex = true;
var plannerWait = System.Diagnostics.Stopwatch.StartNew();
while (living.IsBusy && plannerWait.ElapsedMilliseconds < 30000) { Pump(2); System.Threading.Thread.Sleep(10); }
Check("Living Dex terminou", living.Plan is not null);
living.Selected = living.Rows.FirstOrDefault(r => r.CanOpen);
Scan("Living Dex");
vm.Pokedex.ShowLivingDex = false;
// Novidades (aba 1) e o changelog: fica em portugues, com um aviso; so a aba Funcoes e Sobre sao conferidas.
vm.OpenHelp(); foreach (var t in new[] { 0, 2 }) { vm.Help.SelectedTab = t; Scan("Help " + t); }
win.CaptureRenderedFrame()?.Save(Path.Combine(work, $"help_{lang}.png"));
// Campo editavel nunca e traduzido: um apelido em portugues continua igual.
vm.CloseHelpCommand.Execute(null); vm.CurrentPage = vm.Boxes; vm.SelectSlotAsync(vm.Boxes.Slots[0]).GetAwaiter().GetResult(); Pump();
var nick = win.GetVisualDescendants().OfType<TextBox>().First(b => b.Text == "Snivy" && b.IsEffectivelyVisible);
nick.Text = "Caixas"; Pump();
var nickBox = nick.Text == "Caixas" ? nick : null;
Check("TextBox não é traduzido", nickBox is not null);

// Special Game tabs and QR are shared by desktop and Android.
vm.CurrentPage = vm.Game; vm.Game.Tab = 5; Scan("Stored cards");
var originalContent = win.Content;
foreach (var version in new[] { GameVersion.SH, GameVersion.SL, GameVersion.E, GameVersion.FR, GameVersion.Y, GameVersion.ZA })
{
    var demo = BlankSaveFile.Get(version); CoreAdapter.Activate(demo);
    var page = new GamePageViewModel((_, _, _) => System.Threading.Tasks.Task.FromResult(true), _ => { }); page.Load(demo); page.Tab = version == GameVersion.E ? 7 : version is GameVersion.FR or GameVersion.Y ? 8 : 6;
    win.Content = new ContentControl { Content = page, Margin = new Thickness(16) }; Scan("Raids " + version);
    if (version == GameVersion.ZA) { page.Tab = 9; page.Donuts!.GenerateCommand.Execute(null); Scan("Donuts"); }
    if (version == GameVersion.SL) { page.RaidRegion = page.RaidRegions.Last(); Scan("Seven-star records"); }
    if (version == GameVersion.SL) { page.Tab = 10; Scan("Fashion"); }
}
// Editores da etapa 4: Battle Frontier, Pokétch/Pokéwalker, Pokéathlon e base secreta.
foreach (var (version, tabs) in new (GameVersion, int[])[] { (GameVersion.E, [11, 14]), (GameVersion.Pt, [11, 12, 3]), (GameVersion.HG, [11, 12, 13]), (GameVersion.OR, [14, 3]), (GameVersion.Y, [3]), (GameVersion.SN, [3]), (GameVersion.BD, [3]) })
{
    var demo = BlankSaveFile.Get(version); CoreAdapter.Activate(demo);
    var page = new GamePageViewModel((_, _, _) => System.Threading.Tasks.Task.FromResult(true), _ => { }); page.Load(demo);
    win.Content = new ContentControl { Content = page, Margin = new Thickness(16) };
    foreach (var tab in tabs) { page.Tab = tab; if (tab == 11) page.Frontier!.FacilityIndex = version == GameVersion.E ? 4 : 3; Scan($"Game tab {tab} {version}"); }
}
win.Content = originalContent; vm.ShowPokemonQr(new PK7 { Species = 25 }); Scan("QR"); vm.CloseQr();

// New preference cards, touch toolbar, Bank consultation and backup metadata.
BankStorage.WriteSlot(BankStorage.GetBoxes(BankStorage.GetBanks()[0])[0],0,new PK5{Species=25,Version=GameVersion.B,CurrentLevel=42,OriginalTrainerName="DEMO"});
vm.Bank.Reload();vm.CurrentPage=vm.Bank;vm.SelectSlotAsync(vm.Bank.Slots[0]).GetAwaiter().GetResult();Scan("Bank consultation");
SaveBackup.BeforeOverwrite(path,"Salvar como");vm.CurrentPage=vm.SaveManager;vm.SaveManager.ShowBackups=true;
var backupsTask=vm.SaveManager.RefreshBackupsAsync();while(!backupsTask.IsCompleted){Pump(1);System.Threading.Thread.Sleep(5);}Scan("Backup metadata");
var prefs=win.GetVisualDescendants().OfType<Button>().Single(b=>b.Name=="PrefsButton");prefs.Flyout!.ShowAt(prefs);Scan("Theme preferences",(prefs.Flyout as Flyout)?.Content as Control);prefs.Flyout.Hide();
App.ShowShortcuts=false;vm.IsTouchUI=true;vm.CurrentPage=vm.Boxes;vm.TouchSelectionMode=true;Scan("Touch selection");
prefs.Flyout.ShowAt(prefs);Scan("Touch preferences",(prefs.Flyout as Flyout)?.Content as Control);prefs.Flyout.Hide();

if (foreign) File.WriteAllLines(Path.Combine(work, "untranslated.txt"), untranslated.Select(m => $"{m.Value}\t{m.Key.Replace("\n", " ")}"));
File.WriteAllLines(Path.Combine(work, "missing.txt"), missing.Select(m => $"{m.Value}\t{m.Key.Replace("\n", "\n")}"));
Console.WriteLine($"Textos em português na tela: {missing.Count} (lista em {Path.Combine(work, "missing.txt")})");
foreach (var m in missing.Take(400)) Console.WriteLine($"  [{m.Value}] {m.Key.Replace("\n", "\n")}");
Check("sem textos em português na interface", missing.Count == 0);

// Primeira abertura: idioma do sistema (portugues de qualquer pais, espanhol so com dicionario, o resto em ingles).
Check("primeira abertura sem idioma gravado", new AppSettings().UiLanguage is null);
string Detect(string culture) => Loc.Detect(new System.Globalization.CultureInfo(culture));
bool hasEs = Loc.Languages.Any(l => l.Code == Loc.Spanish);
Check("sistema pt-BR/pt-PT → português", Detect("pt-BR") == Loc.Portuguese && Detect("pt-PT") == Loc.Portuguese);
Check("sistema en-US/ja-JP/fr-FR → inglês", Detect("en-US") == Loc.English && Detect("ja-JP") == Loc.English && Detect("fr-FR") == Loc.English);
Check("sistema es-MX → " + (hasEs ? "espanhol" : "inglês (sem es.json)"), Detect("es-MX") == (hasEs ? Loc.Spanish : Loc.English));
bool hasZh = Loc.Languages.Any(l => l.Code == Loc.ChineseSimplified);
Check("sistema zh-CN/zh-TW → " + (hasZh ? "chinês simplificado" : "inglês (sem zh-Hans.json)"),
    Detect("zh-CN") == (hasZh ? Loc.ChineseSimplified : Loc.English) && Detect("zh-TW") == Detect("zh-CN"));
// Botao 🌐 sempre visivel no rodape, com os nomes de cada idioma nele mesmo.
var langButton = win.GetVisualDescendants().OfType<Button>().Single(b => b.Name == "LanguageButton");
Check("botão 🌐 visível", langButton.IsEffectivelyVisible && langButton.Content as string == Pick("🌐 EN", "🌐 ES", "🌐 中"));
Check("dica do 🌐 em todos os idiomas", ToolTip.GetTip(langButton) is string langTip && langTip.Contains("Idioma") && langTip.Contains("Language"));
langButton.Flyout!.ShowAt(langButton); Pump();
var langNames = ((langButton.Flyout as Flyout)!.Content as Control)!.GetVisualDescendants().OfType<TextBlock>().Select(t => t.Text ?? "").ToList();
Check("menu com Português (Brasil) sem traduzir", langNames.Any(t => t.Contains("Português (Brasil)")) && langNames.Any(t => t.Contains("✓") && t.Contains(Pick("English", "Español", "简体中文"))));
langButton.Flyout.Hide();
// Trocar para portugues numa tela em ingles: a pergunta vem em portugues (o idioma escolhido), com uma linha em ingles.
vm.LanguageOptions.Single(l => l.Code == Loc.Portuguese).SelectCommand.Execute(null); Pump();
var langDialog = vm.Dialog;
Check("pergunta no idioma escolhido", langDialog?.Title == "Reiniciar para usar Português?" && langDialog.Message.Contains(Pick("The interface language changes", "El idioma de la interfaz cambia", "界面语言会在重新启动应用后更改")));
var shownTexts = win.GetVisualDescendants().OfType<TextBlock>().Where(t => t.IsEffectivelyVisible).Select(t => t.Text).ToList();
Check("pergunta não é traduzida na tela", shownTexts.Contains("Reiniciar para usar Português?") && shownTexts.Contains("Reiniciar agora"));
langDialog?.Complete(false); Pump();
Check("botão mostra PT e status em português", langButton.Content as string == "🌐 PT" && vm.Status.Contains("vale na próxima vez"));
// Voltar ao idioma da tela nao pergunta nada.
vm.LanguageOptions.Single(l => l.Code == lang).SelectCommand.Execute(null); Pump();
Check("voltar ao idioma atual sem pergunta", vm.Dialog is null && vm.Settings.UiLanguage == lang);
// "Nomes do jogo em ingles": no espanhol e no chines, pergunta se pode reiniciar; a troca vale na partida seguinte.
if (foreign)
{
    vm.GameNamesInEnglish = true; Pump();
    Check("pergunta de reiniciar ao trocar os nomes", vm.Dialog?.Title == "Reiniciar para trocar os nomes do jogo?" && vm.Settings.GameNamesInEnglish);
    vm.Dialog?.Complete(false); Pump();
}
_ = new MainViewModel(new AppSettings { SavesFolder = lib, UiLanguage = lang, GameNamesInEnglish = true });
Check("nomes do jogo em inglês com a opção", GameInfo.CurrentLanguage == "en" && CoreAdapter.MoveNames[85] == "Thunderbolt");
Check("português sem nomes próprios no Core", Loc.GameLanguageFor(Loc.Portuguese, false) == "en" && !Loc.HasGameNames(Loc.Portuguese) && !Loc.HasGameNames(Loc.English));
Console.WriteLine(fails == 0 ? "TUDO OK" : $"{fails} FALHAS"); return fails == 0 ? 0 : 1;
