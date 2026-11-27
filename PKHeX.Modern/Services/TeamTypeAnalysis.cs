using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using PKHeX.Core;

namespace PKHeX.Modern.Services;

/// <summary>
/// Analise de tipos da equipe (pagina Equipe): quanto cada tipo de golpe tira de cada Pokemon (defesa) e contra
/// quais tipos os golpes da equipe sao super efetivos (ataque). So os tipos entram: habilidades (Levitate...),
/// itens, Tera e golpes de dano fixo nao mudam a conta.
/// </summary>
public static class TeamTypeAnalysis
{
    public sealed record Member(PKM Pk, string Name, byte Type1, byte Type2, IReadOnlyList<(ushort Move, byte Type)> Attacks);
    public sealed record DefenseRow(byte Type, IReadOnlyList<double> Multipliers)
    {
        public int Weak => Multipliers.Count(m => m > 1);
        public int Resist => Multipliers.Count(m => m is > 0 and < 1);
        public int Immune => Multipliers.Count(m => m == 0);
        /// <summary>Tipo perigoso: pega pelo menos dois a mais do que os que resistem ou sao imunes.</summary>
        public bool IsDanger => Weak >= 2 && Weak - Resist - Immune >= 2;
    }
    /// <summary>O melhor que os golpes da equipe fazem contra um tipo, e quem acerta super efetivo (Pokemon e golpe).</summary>
    public sealed record CoverageCell(byte Type, double Best, IReadOnlyList<string> SuperEffective);
    public sealed record Report(int Generation, IReadOnlyList<Member> Members, IReadOnlyList<DefenseRow> Defense, IReadOnlyList<CoverageCell> Coverage)
    {
        public bool HasAttacks => Members.Any(m => m.Attacks.Count > 0);
    }

    /// <summary>Golpes de dano fixo ou que nocauteiam de uma vez: o tipo so decide a imunidade, entao nao contam como cobertura.</summary>
    private static readonly HashSet<ushort> FixedDamage =
    [
        (ushort)Move.SeismicToss, (ushort)Move.NightShade, (ushort)Move.DragonRage, (ushort)Move.SonicBoom, (ushort)Move.SuperFang,
        (ushort)Move.NaturesMadness, (ushort)Move.Ruination, (ushort)Move.Endeavor, (ushort)Move.FinalGambit, (ushort)Move.Counter,
        (ushort)Move.MirrorCoat, (ushort)Move.MetalBurst, (ushort)Move.Comeuppance, (ushort)Move.Bide, (ushort)Move.Psywave,
        (ushort)Move.Guillotine, (ushort)Move.HornDrill, (ushort)Move.Fissure, (ushort)Move.SheerCold,
    ];

    private static HashSet<ushort>? _status;

    /// <summary>Golpe de status (Assets/move-info.json, gerado do AllGenWiki por Tools/build_move_info.py).</summary>
    public static bool IsStatus(ushort move)
    {
        if (_status is null)
        {
            var set = new HashSet<ushort>();
            try
            {
                using var stream = typeof(TeamTypeAnalysis).Assembly.GetManifestResourceStream("move-info.json");
                if (stream is not null)
                {
                    using var doc = JsonDocument.Parse(stream);
                    foreach (var id in doc.RootElement.GetProperty("status").EnumerateArray())
                        set.Add(id.GetUInt16());
                }
            }
            catch (Exception) { }
            _status = set;
        }
        return _status.Contains(move);
    }

    /// <summary>Tipo do golpe neste formato; Hidden Power usa o tipo calculado dos IVs. null = golpe que nao conta como ataque.</summary>
    public static byte? AttackType(PKM pk, ushort move, EntityContext context, int generation)
    {
        if (move == 0 || IsStatus(move) || FixedDamage.Contains(move))
            return null;
        byte type = move == (ushort)Move.HiddenPower ? (byte)(pk.HPType + 1) : MoveInfo.GetType(move, context);
        return TypeChart.Exists(type, generation) ? type : null;
    }

    public static Report Analyze(IEnumerable<PKM> party, EntityContext context, int generation)
    {
        var members = new List<Member>();
        foreach (var pk in party)
        {
            if (CoreAdapter.IsEmpty(pk) || pk.IsEgg)
                continue;
            var (t1, t2) = CoreAdapter.GetSpeciesTypes(pk.PersonalInfo);
            var attacks = new List<(ushort, byte)>();
            for (int i = 0; i < 4; i++)
            {
                var move = CoreAdapter.GetMove(pk, i);
                if (AttackType(pk, move, context, generation) is { } type)
                    attacks.Add((move, type));
            }
            var name = pk.IsNicknamed ? pk.Nickname : CoreAdapter.SpeciesNames[pk.Species];
            members.Add(new Member(pk, name, t1, t2, attacks));
        }

        var types = TypeChart.TypesIn(generation);
        var defense = types.Select(a => new DefenseRow(a, [.. members.Select(m => TypeChart.Multiplier(a, m.Type1, m.Type2, generation))])).ToList();
        var moveNames = GameInfo.Strings.movelist;
        var coverage = types.Select(d =>
        {
            double best = 0;
            var super = new List<string>();
            foreach (var m in members)
            {
                foreach (var (move, type) in m.Attacks)
                {
                    var x = TypeChart.Multiplier(type, d, generation);
                    best = Math.Max(best, x);
                    if (x > 1)
                        super.Add($"{m.Name} ({(move < moveNames.Length ? moveNames[move] : move.ToString())})");
                }
            }
            return new CoverageCell(d, members.Any(m => m.Attacks.Count > 0) ? best : 1, super);
        }).ToList();
        return new Report(generation, members, defense, coverage);
    }
}
