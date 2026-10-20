namespace PKHeX.Modern.Theme;

/// <summary>
/// Liga e desliga as animações curtas (⚙ › Animações): pulo do slot que recebe um Pokémon, brilho ✦ no editor,
/// Pokébola ao abrir um save e a piscada da ★ dos shiny (esta pela classe <c>noMotion</c> na MainView).
/// </summary>
public static class Motion
{
    public static bool Enabled { get; set; } = true;
}
