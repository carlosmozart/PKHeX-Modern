using System; using System.IO; using System.Linq; using System.Threading.Tasks; using System.Diagnostics; using Avalonia; using Avalonia.Headless; using Avalonia.Threading; using Avalonia.Input; using Avalonia.VisualTree; using Avalonia.LogicalTree; using Avalonia.Controls;
using PKHeX.Core; using PKHeX.Modern; using PKHeX.Modern.Services; using PKHeX.Modern.ViewModels; using PKHeX.Modern.Views;
AppBuilder.Configure<App>().UseSkia().UseHeadless(new AvaloniaHeadlessPlatformOptions { UseHeadlessDrawing = false }).WithInterFont().SetupWithoutStarting();
var dir = args[0]; int fails = 0;
SaveBackup.Folder = Path.Combine(dir, "backups");
BankStorage.Root = Path.Combine(dir, "bank");
void Check(string n, bool ok, string extra = "") { Console.WriteLine($"{(ok ? "OK  " : "FAIL")} {n} {extra}"); if (!ok) fails++; }

// prepara as copias: Vulpix e Meowstic no Shield (caixa 1, slots 1-2), Deoxys no Emerald (caixa 1, slot 1)
PKM Make(SaveFile sav, ushort sp, byte form, byte gender)
{
    var enc = EncounterDatabase.SearchEncounters(sav, sp, true).First(e => e.Form == form);
    var pk = EncounterDatabase.ToEntity(sav, enc, EncounterCriteria.Unrestricted, out _)!;
    return pk;
}
var sh = CoreAdapter.LoadSave(Path.Combine(dir, "sh.sav"))!; CoreAdapter.Activate(sh);
sh.SetBoxSlotAtIndex(Make(sh, 37, 0, 0), 0, 0);
var meow = Make(sh, 678, 0, 0); meow.Species = 678; meow.Form = 0; meow.Gender = 0; meow.ClearNickname(); meow.RefreshAbility(0); meow.CurrentLevel = 30; meow.ResetPartyStats();
Console.WriteLine($"Meowstic preparado: legal {new LegalityAnalysis(meow).Valid}");
sh.SetBoxSlotAtIndex(meow, 0, 1);
sh.SetBoxSlotAtIndex(Make(sh, 52, 0, 0), 0, 2); // Meowth de Kanto: tem forma de Alola e de Galar
File.WriteAllBytes(Path.Combine(dir, "sh.sav"), sh.Write().ToArray());
foreach (var i in new[] { 0, 1 }) { var x = sh.GetBoxSlotAtIndex(0, i); Console.WriteLine($"slot {i}: {GameInfo.Strings.specieslist[x.Species]} form {x.Form} gender {x.Gender} legal {new LegalityAnalysis(x).Valid} dual {x.PersonalInfo.IsDualGender} formCount {x.PersonalInfo.FormCount}"); }
Console.WriteLine("Meowstic forms SWSH: " + string.Join(",", Enumerable.Range(0, 2).Select(f => sh.Personal.IsPresentInGame(678, (byte)f))));
var em = CoreAdapter.LoadSave(Path.Combine(dir, "em.sav"))!; CoreAdapter.Activate(em);
var deoxys = em.BlankPKM; deoxys.Species = 386; deoxys.CurrentLevel = 30; em.SetBoxSlotAtIndex(deoxys, 0, 0);
File.WriteAllBytes(Path.Combine(dir, "em.sav"), em.Write().ToArray());

var vm = new MainViewModel();
var win = new MainWindow { DataContext = vm, Width = 1500, Height = 950 }; win.Show();
void Pump() { for (int i = 0; i < 4; i++) { Dispatcher.UIThread.RunJobs(); AvaloniaHeadlessPlatform.ForceRenderTimerTick(); } }
void Wait(Func<bool> done, int ms = 60000) { var sw = Stopwatch.StartNew(); while (!done() && sw.ElapsedMilliseconds < ms) { Pump(); if (vm.Dialog is { } d) d.Complete(true); System.Threading.Thread.Sleep(10); } Pump(); }
void Select(int box, int slot) { vm.CurrentPage = vm.Boxes; vm.Boxes.CurrentBox = box; Pump(); var t = vm.SelectSlotAsync(vm.Boxes.Slots[slot]); Wait(() => t.IsCompleted); }
var o = vm.OpenAsync(Path.Combine(dir, "sh.sav")); Wait(() => o.IsCompleted);

// Vulpix: formas com sprite
Select(0, 0);
var ed = vm.Editor!;
Check("Vulpix tem formas com sprite", ed.HasForms && ed.FormOptions.All(f => f.Sprite is not null), string.Join(", ", ed.FormOptions.Select(f => f.Text)));
var combo = win.GetVisualDescendants().OfType<ComboBox>().First(c => c.ItemsSource == ed.FormOptions);
combo.IsDropDownOpen = true; Pump(); Pump();
win.CaptureRenderedFrame()!.Save(Path.Combine(dir, "forms_open.png"));
foreach (var tl in TopLevel.GetTopLevel(combo)!.GetVisualDescendants().OfType<Avalonia.Controls.Primitives.Popup>()) { }
combo.IsDropDownOpen = false; Pump();

// Meowstic: genero e forma juntos (modo legal desligado e ligado)
Select(0, 1);
ed = vm.Editor!;
Console.WriteLine($"Meowstic: genero {(int)ed.GenderSymbol[0]} forma {ed.SelectedForm?.Text} podeTrocar={ed.CanToggleGender}");
vm.LegalMode = false; Pump();
ed.ToggleGender(); Pump();
Check("Meowstic (sem modo legal): genero e forma trocam juntos", ed.GenderSymbol == "♀" && ed.SelectedForm?.Value == 1, $"{ed.GenderSymbol} {ed.SelectedForm?.Text}");
vm.LegalMode = true; Select(0, 1); ed = vm.Editor!;
ed.ToggleGender(); Wait(() => !ed.IsLegalizing);
Check("Meowstic (modo legal): genero/forma trocados e legal", ed.GenderSymbol == "♀" && ed.SelectedForm?.Value == 1 && ed.IsLegal, $"{ed.GenderSymbol} {ed.SelectedForm?.Text} | {vm.Status}");
ed.SelectedForm = ed.FormOptions.First(f => f.Value == 0); Wait(() => !ed.IsLegalizing);
Check("Meowstic: trocar a forma troca o genero", ed.GenderSymbol == "♂" && ed.IsLegal, $"{ed.GenderSymbol} | {vm.Status}");

// Meowth: trocar para Alola e Galar com um clique de verdade (abre a lista e clica no item). Antes, trocar a forma
// mudava as listas de bola e habilidade e a caixa de forma ficava em branco (SelectionBoxItem vazio).
void ClickForm(string name)
{
    var c = win.GetVisualDescendants().OfType<ComboBox>().First(x => x.ItemsSource is System.Collections.Generic.IReadOnlyList<FormOption>);
    var p = c.TranslatePoint(new Point(c.Bounds.Width / 2, c.Bounds.Height / 2), win)!.Value;
    win.MouseDown(p, MouseButton.Left); win.MouseUp(p, MouseButton.Left); Pump(); Pump();
    var item = c.GetLogicalDescendants().OfType<ComboBoxItem>().First(i => (i.DataContext as FormOption)?.Text.Contains(name) == true);
    var ip = item.TranslatePoint(new Point(item.Bounds.Width / 2, item.Bounds.Height / 2), win)!.Value;
    win.MouseDown(ip, MouseButton.Left); win.MouseUp(ip, MouseButton.Left); Pump(); Pump();
}
void CheckMeowth(string game)
{
    var ed = vm.Editor!;
    foreach (var (name, value) in new[] { ("Alola", 1), ("Galar", 2), ("Normal", 0) })
    {
        ClickForm(name); Wait(() => !ed.IsLegalizing); Pump(); Pump();
        var c = win.GetVisualDescendants().OfType<ComboBox>().First(x => x.ItemsSource is System.Collections.Generic.IReadOnlyList<FormOption>);
        var shown = c.SelectionBoxItem as FormOption;
        Check($"{game}: Meowth {name} pelo clique, a caixa mostra a forma", ed.SelectedForm?.Value == value && shown?.Value == value && c.ItemCount >= 3 && ed.IsLegal,
            $"pk={ed.SelectedForm?.Text} caixa={shown?.Text ?? "(vazia)"} | {vm.Status}");
    }
}
Select(0, 2);
CheckMeowth("Shield");
win.CaptureRenderedFrame()!.Save(Path.Combine(dir, "meowth.png"));

// Espécie: um clique mostra a lista inteira com o nome selecionado; digitar substitui o nome e filtra por qualquer parte
{
    var search = win.GetVisualDescendants().OfType<PKHeX.Modern.Controls.FullListAutoComplete>().First();
    var p = search.TranslatePoint(new Point(search.Bounds.Width / 2, search.Bounds.Height / 2), win)!.Value;
    win.MouseDown(p, MouseButton.Left); win.MouseUp(p, MouseButton.Left); Pump(); Pump();
    var box = search.GetVisualDescendants().OfType<TextBox>().First();
    Check("busca de espécie: o clique abre a lista com o nome selecionado", search.IsDropDownOpen && box.SelectionStart == 0 && box.SelectionEnd == (search.Text ?? "").Length,
        $"aberta={search.IsDropDownOpen} seleção={box.SelectionStart}-{box.SelectionEnd} texto={search.Text}");
    win.KeyTextInput("chu"); Pump(); System.Threading.Thread.Sleep(400); Pump(); Pump();
    win.KeyPress(Key.Down, RawInputModifiers.None, PhysicalKey.ArrowDown, null); win.KeyPress(Key.Enter, RawInputModifiers.None, PhysicalKey.Enter, null);
    var ed2 = vm.Editor!; Wait(() => !ed2.IsLegalizing);
    Check("busca de espécie: digitar substitui o nome e filtra (chu)", ed2.SpeciesName.Contains("chu"), ed2.SpeciesName);
}

// Gen 3: genero pelo PID (Emerald), Deoxys
var sv = CoreAdapter.LoadSave(Path.Combine(dir, "sv.sav"))!; CoreAdapter.Activate(sv);
sv.SetBoxSlotAtIndex(Make(sv, 52, 0, 0), 0, 0);
File.WriteAllBytes(Path.Combine(dir, "sv.sav"), sv.Write().ToArray());
o = vm.OpenAsync(Path.Combine(dir, "sv.sav")); Wait(() => o.IsCompleted);
Select(0, 0);
CheckMeowth("Scarlet (legaliza Alola e Galar; versões novas do PKHeX sem contexto ficam fora da busca)");

o = vm.OpenAsync(Path.Combine(dir, "em.sav")); Wait(() => o.IsCompleted);
Select(0, 0); ed = vm.Editor!;
Check("Deoxys Gen 3: aviso da forma", ed.HasFormNote && !ed.HasForms, ed.FormNote);
o = vm.OpenAsync(Path.Combine(dir, "sa.sav")); Wait(() => o.IsCompleted);
var sav3 = (SaveFile)typeof(MainViewModel).GetField("_sav", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!.GetValue(vm)!;
var all3 = EntitySearch.ReadAll(sav3).Where(x => x.Box >= 0 && x.Pkm.Species != 0 && !x.Pkm.IsEgg).ToList();
Console.WriteLine($"Emerald: {all3.Count} nas caixas, {all3.Count(x => x.Pkm.PersonalInfo.IsDualGender)} com dois generos, {all3.Count(x => new LegalityAnalysis(x.Pkm).Valid)} legais");
var dual = all3.Where(x => x.Pkm.PersonalInfo.IsDualGender && new LegalityAnalysis(x.Pkm).Valid).First();
Select(dual.Box, dual.Slot); ed = vm.Editor!;
var g0 = ed.GenderSymbol; var nat = ed.Nature; Console.WriteLine($"   {ed.SpeciesName} legal antes: {ed.IsLegal}");
ed.ToggleGender(); Wait(() => !ed.IsLegalizing);
Console.WriteLine($"   legal depois: {ed.IsLegal}");
Check("Gen 3: troca de genero (PID novo, mesma natureza, legal)", ed.GenderSymbol != g0 && ed.Nature == nat && ed.IsLegal, $"{ed.SpeciesName} {g0}->{ed.GenderSymbol} | {vm.Status}");
Console.WriteLine(fails == 0 ? "TUDO OK" : $"{fails} FALHAS");
return fails == 0 ? 0 : 1;
