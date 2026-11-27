using System;
using Avalonia.Media;

namespace PKHeX.Modern.Theme;

/// <summary>
/// Fontes de reserva para os caracteres que as fontes do tema nao tem. A Noto Sans SC (cortada para os blocos CJK por
/// Tools/build_cjk_font.py) desenha o chines simplificado igual em todo sistema; no Linux, sem ela, o chines viraria
/// quadradinhos. Como so tem blocos CJK, nao muda a aparencia dos outros idiomas. No Android entram tambem os
/// simbolos e emoji, que o aparelho nem sempre tem.
/// A PKHeX Modern Symbols (Tools/build_symbols_font.py) vem primeiro em todo sistema: tem os onze simbolos de icone
/// (↶ ↷ ↺ ⟳ ⋯ ⇄ ⇅ ⌂ ⌕ ⓘ ⚑) que nenhuma outra fonte embutida tem e que viraram quadradinhos num Mac Intel (issue 1).
/// No macOS e no Linux, a Noto Sans Symbols 2 cobre setas, simbolos tecnicos e formas geometricas (⭳ ⭱ ⌨ ◐ ▾...),
/// que nem todo sistema tem; os simbolos diversos, os dingbats (✓ ✕ ★ ⚙) e os emoji continuam com as fontes do
/// sistema, coloridas. O Windows tem tudo isso na Segoe UI Symbol e fica como estava.
/// </summary>
public static class FontFallbacks
{
    private const string Fonts = "avares://PKHeX.Modern.UI/Assets/Fonts";

    /// <summary>Faixas que a Noto Sans Symbols 2 cobre no macOS e no Linux: setas, simbolos tecnicos, formas e setas diversas.</summary>
    public const string DesktopSymbolRanges = "U+2190-23FF, U+25A0-25FF, U+2B00-2BFF";

    public static FontManagerOptions Options(bool android) => Options(android, OperatingSystem.IsWindows());

    public static FontManagerOptions Options(bool android, bool windows)
    {
        var symbols = new FontFallback { FontFamily = new FontFamily(Fonts + "#PKHeX Modern Symbols") };
        var chinese = new FontFallback { FontFamily = new FontFamily(Fonts + "#Noto Sans SC") };
        return new FontManagerOptions
        {
            FontFallbacks = android
                ?
                [
                    symbols,
                    new FontFallback { FontFamily = new FontFamily(Fonts + "#Noto Sans Symbols 2") },
                    new FontFallback { FontFamily = new FontFamily(Fonts + "#Noto Emoji") },
                    chinese,
                ]
                : windows
                    ? [symbols, chinese]
                    :
                    [
                        symbols,
                        new FontFallback { FontFamily = new FontFamily(Fonts + "#Noto Sans Symbols 2"), UnicodeRange = UnicodeRange.Parse(DesktopSymbolRanges) },
                        chinese,
                    ],
        };
    }
}
