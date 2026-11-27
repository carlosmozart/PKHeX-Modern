using System.Collections.Generic;
using System.Linq;

namespace PKHeX.Modern.Services;

/// <summary>
/// Tabela de tipos (o PKHeX.Core nao tem uma). Indices do Core (MoveType): Normal 0, Fighting 1, Flying 2, Poison 3,
/// Ground 4, Rock 5, Bug 6, Ghost 7, Steel 8, Fire 9, Water 10, Grass 11, Electric 12, Psychic 13, Ice 14, Dragon 15,
/// Dark 16, Fairy 17. Diferencas por geracao:
/// - Gen 1: sem Steel, Dark e Fairy; Bug e Poison sao super efetivos um contra o outro, Ghost nao afeta Psychic
///   (o erro do Red/Blue/Yellow) e Ice e neutro contra Fire;
/// - Gen 2 a 5: sem Fairy; Steel resiste a Ghost e Dark;
/// - Gen 6 em diante: a tabela atual.
/// </summary>
public static class TypeChart
{
    public const byte Normal = 0, Fighting = 1, Flying = 2, Poison = 3, Ground = 4, Rock = 5, Bug = 6, Ghost = 7, Steel = 8,
        Fire = 9, Water = 10, Grass = 11, Electric = 12, Psychic = 13, Ice = 14, Dragon = 15, Dark = 16, Fairy = 17;
    public const int Count = 18;

    private static readonly double[,] Modern = Build();

    private static double[,] Build()
    {
        var t = new double[Count, Count];
        for (int a = 0; a < Count; a++)
            for (int d = 0; d < Count; d++)
                t[a, d] = 1;
        void Set(byte attack, double factor, params byte[] defenders) { foreach (var d in defenders) t[attack, d] = factor; }
        Set(Normal, 0.5, Rock, Steel); Set(Normal, 0, Ghost);
        Set(Fighting, 2, Normal, Rock, Steel, Ice, Dark); Set(Fighting, 0.5, Flying, Poison, Bug, Psychic, Fairy); Set(Fighting, 0, Ghost);
        Set(Flying, 2, Fighting, Bug, Grass); Set(Flying, 0.5, Rock, Steel, Electric);
        Set(Poison, 2, Grass, Fairy); Set(Poison, 0.5, Poison, Ground, Rock, Ghost); Set(Poison, 0, Steel);
        Set(Ground, 2, Poison, Rock, Steel, Fire, Electric); Set(Ground, 0.5, Bug, Grass); Set(Ground, 0, Flying);
        Set(Rock, 2, Flying, Bug, Fire, Ice); Set(Rock, 0.5, Fighting, Ground, Steel);
        Set(Bug, 2, Grass, Psychic, Dark); Set(Bug, 0.5, Fighting, Flying, Poison, Ghost, Steel, Fire, Fairy);
        Set(Ghost, 2, Ghost, Psychic); Set(Ghost, 0.5, Dark); Set(Ghost, 0, Normal);
        Set(Steel, 2, Rock, Ice, Fairy); Set(Steel, 0.5, Steel, Fire, Water, Electric);
        Set(Fire, 2, Bug, Steel, Grass, Ice); Set(Fire, 0.5, Rock, Fire, Water, Dragon);
        Set(Water, 2, Ground, Rock, Fire); Set(Water, 0.5, Water, Grass, Dragon);
        Set(Grass, 2, Ground, Rock, Water); Set(Grass, 0.5, Flying, Poison, Bug, Steel, Fire, Grass, Dragon);
        Set(Electric, 2, Flying, Water); Set(Electric, 0.5, Grass, Electric, Dragon); Set(Electric, 0, Ground);
        Set(Psychic, 2, Fighting, Poison); Set(Psychic, 0.5, Steel, Psychic); Set(Psychic, 0, Dark);
        Set(Ice, 2, Flying, Ground, Grass, Dragon); Set(Ice, 0.5, Steel, Fire, Water, Ice);
        Set(Dragon, 2, Dragon); Set(Dragon, 0.5, Steel); Set(Dragon, 0, Fairy);
        Set(Dark, 2, Ghost, Psychic); Set(Dark, 0.5, Fighting, Dark, Fairy);
        Set(Fairy, 2, Fighting, Dragon, Dark); Set(Fairy, 0.5, Poison, Steel, Fire);
        return t;
    }

    /// <summary>Tipos que existem na geracao (na ordem do Core).</summary>
    public static IReadOnlyList<byte> TypesIn(int generation) =>
        [.. Enumerable.Range(0, Count).Select(t => (byte)t).Where(t => Exists(t, generation))];

    public static bool Exists(byte type, int generation) => type < Count && !(type == Fairy && generation < 6)
        && !(generation == 1 && type is Steel or Dark);

    /// <summary>Multiplicador de um golpe do tipo <paramref name="attack"/> contra um tipo.</summary>
    public static double Multiplier(byte attack, byte defend, int generation)
    {
        if (!Exists(attack, generation) || !Exists(defend, generation))
            return 1;
        if (generation == 1)
        {
            switch (attack, defend)
            {
                case (Bug, Poison): case (Poison, Bug): return 2;
                case (Ghost, Psychic): return 0;
                case (Ice, Fire): return 1;
            }
        }
        if (generation <= 5 && defend == Steel && attack is Ghost or Dark)
            return 0.5;
        return Modern[attack, defend];
    }

    /// <summary>Multiplicador contra um Pokemon de um ou dois tipos (tipos iguais = um tipo so).</summary>
    public static double Multiplier(byte attack, byte type1, byte type2, int generation) =>
        type1 == type2 ? Multiplier(attack, type1, generation) : Multiplier(attack, type1, generation) * Multiplier(attack, type2, generation);
}
