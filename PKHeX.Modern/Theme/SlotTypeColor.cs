using Avalonia;

namespace PKHeX.Modern.Theme;

/// <summary>Liga e desliga o brilho da cor do tipo nos cartões de Pokémon (Controls/TypeGlow.cs).</summary>
public static class SlotTypeColor
{
    public static void Apply(bool on)
    {
        if (Application.Current is { } app)
            app.Resources["SlotTypeGlowOn"] = on;
    }
}
