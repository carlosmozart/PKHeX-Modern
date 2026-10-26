using Avalonia.Media;

namespace PKHeX.Modern.Theme;

/// <summary>
/// Fontes de reserva para os caracteres que as fontes do tema nao tem. A Noto Sans SC (cortada para os blocos CJK por
/// Tools/build_cjk_font.py) desenha o chines simplificado igual em todo sistema; no Linux, sem ela, o chines viraria
/// quadradinhos. Como so tem blocos CJK, nao muda a aparencia dos outros idiomas. No Android entram tambem os
/// simbolos e emoji, que o aparelho nem sempre tem.
/// </summary>
public static class FontFallbacks
{
    private const string Fonts = "avares://PKHeX.Modern.UI/Assets/Fonts";

    public static FontManagerOptions Options(bool android) => new()
    {
        FontFallbacks = android
            ?
            [
                new FontFallback { FontFamily = new FontFamily(Fonts + "#Noto Sans Symbols 2") },
                new FontFallback { FontFamily = new FontFamily(Fonts + "#Noto Emoji") },
                new FontFallback { FontFamily = new FontFamily(Fonts + "#Noto Sans SC") },
            ]
            : [new FontFallback { FontFamily = new FontFamily(Fonts + "#Noto Sans SC") }],
    };
}
