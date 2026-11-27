using System;
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
using PKHeX.Modern.Theme;
using PKHeX.Modern.ViewModels;
using PKHeX.Modern.Views;
using static PKHeX.Modern.Services.TypeChart;

// Analise de tipos da equipe: tabela de tipos por geracao, golpes de status fora da cobertura e o cartao na pagina
// Equipe. Saves sinteticos (Black, Red).
AppBuilder.Configure<App>().UseSkia().UseHeadless(new AvaloniaHeadlessPlatformOptions { UseHeadlessDrawing = false })
    .WithInterFont().With(FontFallbacks.Options(android: false)).SetupWithoutStarting();
int fails = 0;
void Check(string name, bool ok, string extra = "") { Console.WriteLine($"{(ok ? "OK" : "FAIL")} {name} {extra}"); if (!ok) fails++; }
var work = args.Length > 0 ? args[0] : Path.Combine(Path.GetTempPath(), "pkhex-teamtypes-" + Guid.NewGuid()); Directory.CreateDirectory(work);
BankStorage.Root = Path.Combine(work, "bank"); SaveBackup.Folder = Path.Combine(work, "backups");

// Tabela atual: 51 super efetivos, 61 pouco efetivos e 8 imunidades
var all = Enumerable.Range(0, Count).SelectMany(a => Enumerable.Range(0, Count).Select(d => Multiplier((byte)a, (byte)d, 9))).ToList();
Check("tabela atual: 51 × 2, 61 × ½ e 8 × 0", all.Count(x => x == 2) == 51 && all.Count(x => x == 0.5) == 61 && all.Count(x => x == 0) == 8,
    $"{all.Count(x => x == 2)}/{all.Count(x => x == 0.5)}/{all.Count(x => x == 0)}");
Check("Electric não afeta Ground; Dragon não afeta Fairy", Multiplier(Electric, Ground, 9) == 0 && Multiplier(Dragon, Fairy, 9) == 0);
Check("Fire contra Steel/Grass: 4×; Water contra Fire/Flying: 2×", Multiplier(Fire, Steel, Grass, 9) == 4 && Multiplier(Water, Fire, Flying, 9) == 2);
Check("Gen 1: 15 tipos, Ghost não afeta Psychic, Bug ⇄ Poison 2×, Ice neutro contra Fire",
    TypesIn(1).Count == 15 && Multiplier(Ghost, Psychic, 1) == 0 && Multiplier(Bug, Poison, 1) == 2 && Multiplier(Poison, Bug, 1) == 2 && Multiplier(Ice, Fire, 1) == 1
    && Multiplier(Ice, Fire, 2) == 0.5 && Multiplier(Ghost, Psychic, 2) == 2);
Check("Gen 2–5: 17 tipos e Steel resiste a Ghost e Dark; Gen 6: 18 tipos, neutro",
    TypesIn(5).Count == 17 && Multiplier(Ghost, Steel, 5) == 0.5 && Multiplier(Dark, Steel, 2) == 0.5 && TypesIn(6).Count == 18 && Multiplier(Ghost, Steel, 6) == 1);

// Golpes de status nao contam como ataque; dano fixo tambem nao; Bite muda de tipo na Gen 2
Check("status: Growl, Swords Dance e Hold Hands; ataque: Tackle e Thunderbolt",
    TeamTypeAnalysis.IsStatus((ushort)Move.Growl) && TeamTypeAnalysis.IsStatus((ushort)Move.SwordsDance) && TeamTypeAnalysis.IsStatus((ushort)Move.HoldHands)
    && !TeamTypeAnalysis.IsStatus((ushort)Move.Tackle) && !TeamTypeAnalysis.IsStatus((ushort)Move.Thunderbolt));
var probe = new PK5 { Species = (ushort)Species.Pikachu };
Check("Seismic Toss e Swords Dance ficam fora; Thunderbolt é Electric",
    TeamTypeAnalysis.AttackType(probe, (ushort)Move.SeismicToss, EntityContext.Gen5, 5) is null
    && TeamTypeAnalysis.AttackType(probe, (ushort)Move.SwordsDance, EntityContext.Gen5, 5) is null
    && TeamTypeAnalysis.AttackType(probe, (ushort)Move.Thunderbolt, EntityContext.Gen5, 5) == Electric);
Check("Bite: Normal na Gen 1, Dark na Gen 2", MoveInfo.GetType((ushort)Move.Bite, EntityContext.Gen1) == Normal && MoveInfo.GetType((ushort)Move.Bite, EntityContext.Gen2) == Dark);
probe.IV_HP = probe.IV_ATK = probe.IV_DEF = probe.IV_SPA = probe.IV_SPD = probe.IV_SPE = 31;
Check("Hidden Power usa o tipo dos IVs (31 em tudo: Dark)", TeamTypeAnalysis.AttackType(probe, (ushort)Move.HiddenPower, EntityContext.Gen5, 5) == Dark);

PK5 Make(Species species, params Move[] moves)
{
    var pk = new PK5 { Species = (ushort)species, CurrentLevel = 50, OriginalTrainerName = "TEST", Language = 2 };
    for (int i = 0; i < moves.Length; i++) pk.SetMove(i, (ushort)moves[i]);
    pk.Heal();
    pk.RefreshChecksum();
    return pk;
}

// Equipe: Charizard (Fire/Flying) e Ferrothorn (Grass/Steel)
var charizard = Make(Species.Charizard, Move.Flamethrower, Move.SwordsDance, Move.SeismicToss);
var ferrothorn = Make(Species.Ferrothorn, Move.GyroBall, Move.Growl);
var report = TeamTypeAnalysis.Analyze([charizard, ferrothorn], EntityContext.Gen5, 5);
var rock = report.Defense.First(r => r.Type == Rock);
Check("defesa: Rock tira 4× do Charizard e ½ do Ferrothorn", rock.Multipliers[0] == 4 && rock.Multipliers[1] == 0.5 && rock.Weak == 1 && rock.Resist == 1 && !rock.IsDanger);
var ground = report.Defense.First(r => r.Type == Ground);
Check("defesa: Ground não afeta o Charizard (imune conta à parte)", ground.Multipliers[0] == 0 && ground.Immune == 1);
Check("defesa: 17 linhas na Gen 5 (sem Fairy)", report.Defense.Count == 17 && report.Defense.All(r => r.Type != Fairy));
var superTypes = report.Coverage.Where(c => c.Best > 1).Select(c => c.Type).OrderBy(t => t).ToArray();
Check("ataque: Flamethrower e Gyro Ball super efetivos contra Rock, Bug, Grass, Ice e Steel",
    superTypes.SequenceEqual(new[] { Rock, Bug, Steel, Grass, Ice }.OrderBy(t => t)), string.Join(",", superTypes));
Check("ataque: quem acerta super efetivo aparece com o golpe",
    report.Coverage.First(c => c.Type == Grass).SuperEffective.Any(s => s.Contains("Flamethrower")) && report.Coverage.First(c => c.Type == Rock).SuperEffective.Any(s => s.Contains("Gyro Ball")));
var twoBirds = TeamTypeAnalysis.Analyze([charizard, Make(Species.Moltres, Move.Flamethrower)], EntityContext.Gen5, 5);
Check("dois Fire/Flying: Rock, Water e Electric viram pontos fracos", twoBirds.Defense.Where(r => r.IsDanger).Select(r => r.Type).Order().SequenceEqual(new[] { Water, Electric, Rock }.Order()),
    string.Join(",", twoBirds.Defense.Where(r => r.IsDanger).Select(r => r.Type)));
var noAttacks = TeamTypeAnalysis.Analyze([Make(Species.Chansey, Move.SoftBoiled, Move.SeismicToss)], EntityContext.Gen5, 5);
Check("só status e dano fixo: sem cobertura", !noAttacks.HasAttacks);
var egg = Make(Species.Pichu, Move.ThunderShock); egg.IsEgg = true;
Check("ovos ficam de fora", TeamTypeAnalysis.Analyze([egg, charizard], EntityContext.Gen5, 5).Members.Count == 1);

// Pagina Equipe com o cartao (Black)
void Pump(int n = 12) { for (int i = 0; i < n; i++) { Dispatcher.UIThread.RunJobs(); AvaloniaHeadlessPlatform.ForceRenderTimerTick(); } }
var sav = BlankSaveFile.Get(GameVersion.B);
sav.SetPartySlotAtIndex(charizard, 0); sav.SetPartySlotAtIndex(ferrothorn, 1);
sav.SetPartySlotAtIndex(Make(Species.Vaporeon, Move.Surf, Move.IceBeam), 2);
var path = Path.Combine(work, "Black.sav");
File.WriteAllBytes(path, sav.Write().ToArray());
var vm = new MainViewModel(new AppSettings { CheckForUpdates = false, LegalMode = false });
var win = new MainWindow { DataContext = vm, Width = 1440, Height = 1400 }; win.Show();
vm.Open(path); vm.CurrentPage = vm.Party; Pump(20);
Check("cartão aparece com três Pokémon", vm.Party.HasTypeAnalysis && vm.Party.TypeMembers.Count == 3 && vm.Party.DefenseRows.Count == 17, $"{vm.Party.TypeMembers.Count}");
Check("resumo do ataque", vm.Party.CoverageSummary.Contains("super efetivos contra"), vm.Party.CoverageSummary);
var texts = win.GetVisualDescendants().OfType<TextBlock>().Select(t => t.Text).ToList();
Check("tabela na tela", texts.Contains("Fracos") && texts.Contains("Resistem") && texts.Contains("4×"));
win.CaptureRenderedFrame()?.Save(Path.Combine(work, "party.png"));

// Trocar um golpe no slot atualiza a analise
var slot = vm.Party.Slots[2];
var vaporeon = slot.Pkm!.Clone(); vaporeon.SetMove(1, (ushort)Move.Tackle);
sav = (SaveFile)typeof(MainViewModel).GetField("_sav", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!.GetValue(vm)!;
sav.SetPartySlotAtIndex(vaporeon, 2); slot.Load(sav); Pump();
Check("a análise acompanha a troca do Pokémon no slot", !vm.Party.TypeReport!.Members[2].Attacks.Any(a => a.Move == (ushort)Move.IceBeam));

// Gen 1: Lick (Ghost) nao afeta Psychic
var red = BlankSaveFile.Get(GameVersion.RD);
var gengar = new PK1 { Species = (ushort)Species.Gengar, CurrentLevel = 40 }; gengar.SetMove(0, (ushort)Move.Lick);
var gen1 = TeamTypeAnalysis.Analyze([gengar], red.Context, red.Generation);
Check("Gen 1: Gengar Ghost/Poison, Lick não afeta Psychic e Psychic tira 2×",
    gen1.Members[0].Type1 == Ghost && gen1.Members[0].Type2 == Poison && gen1.Coverage.First(c => c.Type == Psychic).Best == 0
    && gen1.Defense.First(r => r.Type == Psychic).Multipliers[0] == 2 && gen1.Defense.Count == 15);

Console.WriteLine(fails == 0 ? "TUDO OK" : $"{fails} FALHAS");
return fails == 0 ? 0 : 1;
