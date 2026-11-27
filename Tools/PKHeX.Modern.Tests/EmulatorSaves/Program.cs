using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Avalonia;
using Avalonia.Headless;
using Avalonia.Threading;
using PKHeX.Modern;
using PKHeX.Modern.Services;
using PKHeX.Modern.ViewModels;

// Saves dos emuladores (Android): busca numa "memoria" falsa montada com copias dos saves de teste.
AppBuilder.Configure<App>().UseSkia().UseHeadless(new AvaloniaHeadlessPlatformOptions { UseHeadlessDrawing = false }).WithInterFont().SetupWithoutStarting();
var work = args[0]; int fails = 0;
BankStorage.Root = Path.Combine(work, "bank"); SaveBackup.Folder = Path.Combine(work, "backups");
void Check(string n, bool ok, string extra = "") { Console.WriteLine($"{(ok ? "OK" : "FAIL")} {n} {extra}"); if (!ok) fails++; }
string In(string name) => Path.Combine(work, name);
if (!File.Exists(In("fr.sav")) || !File.Exists(In("hg.sav")) || !File.Exists(In("moon.sav"))) { Console.WriteLine("PULADO: faltam saves"); return 0; }

var root = Path.Combine(work, "storage");
void Put(string source, string relative)
{
    var dest = Path.Combine(root, relative.Replace('/', Path.DirectorySeparatorChar));
    Directory.CreateDirectory(Path.GetDirectoryName(dest)!);
    File.Copy(In(source), dest, true);
}
Put("fr.sav", "RetroArch/saves/Pokemon - FireRed Version (USA).srm");
Put("hg.sav", "DraStic/backup/POKEMON HG.dsv");
Put("moon.sav", "citra-emu/sdmc/Nintendo 3DS/00000000000000000000000000000000/00000000000000000000000000000000/title/00040000/00175e00/data/00000001/main");
Put("fr.sav", "Roms/GBA/Pokemon Leaf.sav");
Put("fr.sav", "MyOldBoy/save/Pokemon Crystal.sav");
Put("fr.sav", "DCIM/Camera/nao-e-save.sav");
Directory.CreateDirectory(Path.Combine(root, "Roms", "GBA"));
File.WriteAllBytes(Path.Combine(root, "Roms", "GBA", "pequeno.sav"), new byte[100]);

var found = EmulatorSaves.Find(root);
string Sources() => string.Join(", ", found.Select(f => f.Source + ":" + Path.GetFileName(f.Path)));
Check("RetroArch", found.Any(f => f.Source == "RetroArch" && f.Path.EndsWith(".srm")), Sources());
Check("DraStic", found.Any(f => f.Source == "DraStic"));
Check("Citra (main)", found.Any(f => f.Source == "Citra"));
Check("My OldBoy! pelo nome", found.Any(f => f.Source == "My OldBoy!" && f.Path.EndsWith("Pokemon Crystal.sav")), Sources());
Check("save ao lado da ROM", found.Any(f => f.Source == "Roms" && f.Path.EndsWith("Pokemon Leaf.sav")));
Check("DCIM pulada", !found.Any(f => f.Path.Contains("DCIM")));
Check("arquivo pequeno pulado", !found.Any(f => f.Path.EndsWith("pequeno.sav")));
Check("sem repetidos", found.Select(f => f.Path).Distinct().Count() == found.Count);

var entries = SaveLibrary.ReadFound(found);
Check("todos reconhecidos pelo Core", entries.Count == 5, string.Join(" | ", entries.Select(e => $"{e.Group}: {e.Game}")));
Check("grupo por emulador", entries.Any(e => e.Group.Contains("RetroArch") && e.Game.Contains("FireRed")) && entries.Any(e => e.Group.Contains("Citra")));

// Pagina Saves: com a busca ligada e o acesso dado, os saves dos emuladores entram na lista.
EmulatorSaves.StorageRoot = root;
bool access = false, requested = false;
EmulatorSaves.HasAccess = () => access;
EmulatorSaves.RequestAccess = () => requested = true;
var settings = new AppSettings { CheckForUpdates = false, SavesFolder = Path.Combine(work, "vazia") };
Directory.CreateDirectory(settings.SavesFolder);
var vm = new MainViewModel(settings);
void Pump() { for (int i = 0; i < 4; i++) Dispatcher.UIThread.RunJobs(); }
void Wait(System.Threading.Tasks.Task t) { var sw = Stopwatch.StartNew(); while (!t.IsCompleted && sw.ElapsedMilliseconds < 60000) { Pump(); if (vm.Dialog is { } d) d.Complete(true); System.Threading.Thread.Sleep(10); } Pump(); }
var page = vm.SaveManager;
Check("botão aparece", page.CanSearchEmulators);
Wait(page.SearchEmulatorsAsync());
Check("sem acesso: abre a configuração", requested && settings.EmulatorScan);
access = true;
page.OnResumed();
{ var sw = Stopwatch.StartNew(); while ((page.Groups.Count == 0 || page.IsLoading) && sw.ElapsedMilliseconds < 60000) { Pump(); System.Threading.Thread.Sleep(10); } Pump(); }
var groups = page.Groups.Select(g => g.Title).ToList();
Check("lista com os emuladores", groups.Any(g => g.Contains("RetroArch")) && groups.Any(g => g.Contains("DraStic")), string.Join(", ", groups));
Check("resumo conta os emuladores", page.Summary.Contains("emuladores"), page.Summary);

Console.WriteLine(fails == 0 ? "TUDO OK" : $"{fails} FALHA(S)");
return fails == 0 ? 0 : 1;
