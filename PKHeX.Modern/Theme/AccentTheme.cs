using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Styling;

namespace PKHeX.Modern.Theme;

/// <summary>Uma cor de destaque, com a versao para o tema escuro e para o claro.</summary>
public sealed record AccentPreset(string Key, string Name, Color Dark, Color Light)
{
    public IBrush Brush { get; } = new SolidColorBrush(Dark);
}

/// <summary>
/// Cor de destaque configuravel. Sobrescreve Accent, AccentSoft e SystemAccentColor* nos dicionarios de tema
/// do Application (que tem prioridade sobre Palette.axaml), entao tudo que usa DynamicResource muda na hora.
/// </summary>
public static class AccentTheme
{
    public static IReadOnlyList<AccentPreset> Presets { get; } =
    [
        new("red", "Vermelho (Pokébola)", Color.Parse("#E5484D"), Color.Parse("#D93D42")),
        new("cyan", "Ciano", Color.Parse("#22B8CF"), Color.Parse("#0C8599")),
        new("blue", "Azul", Color.Parse("#4C8DF6"), Color.Parse("#2563EB")),
        new("purple", "Roxo", Color.Parse("#8E6CEF"), Color.Parse("#6D4AD8")),
        new("green", "Verde", Color.Parse("#3DBE6B"), Color.Parse("#2F8A42")),
        new("orange", "Laranja", Color.Parse("#F59E3B"), Color.Parse("#D97706")),
        new("lime", "Lima (Z-A)", Color.Parse("#B8E62E"), Color.Parse("#5E8A00")),
        new("teal", "Água (ORAS)", Color.Parse("#2EC4B6"), Color.Parse("#0F8F84")),
    ];

    public static AccentPreset Default => Presets[0];

    /// <summary>Chave da opção “Cor do jogo do save”: o destaque segue a cor da versão aberta (Services/GameArt).</summary>
    public const string GameKey = "game";
    /// <summary>A opção no seletor (bolinha em arco-íris); sem save aberto, vale o vermelho padrão.</summary>
    public static AccentPreset GameOption { get; } = new(GameKey, "Cor do jogo do save", Default.Dark, Default.Light);

    /// <summary>
    /// Destaque a partir da cor de uma versão, ajustada para dar leitura: no escuro, cores muito escuras clareiam;
    /// no claro, cores muito claras (Yellow, Gold, X) escurecem.
    /// </summary>
    public static AccentPreset ForGame(uint argb)
    {
        var c = Color.FromUInt32(argb | 0xFF000000);
        double lum = ((0.2126 * c.R) + (0.7152 * c.G) + (0.0722 * c.B)) / 255;
        // Os botões de destaque têm texto branco. Mistura o bastante para chegar ao brilho-alvo (misturar com preto
        // multiplica o brilho; com branco, aproxima de 1): no escuro, entre 0,42 e 0,62; no claro, até 0,46.
        var dark = lum < 0.38 ? Mix(c, Colors.White, Math.Max(0.18, (0.42 - lum) / (1 - lum)))
            : lum > 0.72 ? Mix(c, Colors.Black, 1 - (0.62 / lum)) : c;
        var light = lum > 0.5 ? Mix(c, Colors.Black, 1 - (0.46 / lum)) : c;
        return new(GameKey, GameOption.Name, dark, light);
    }

    /// <summary>
    /// Brilho suave da cor do jogo no alto da área principal (DynamicResource GameTint); null = sem brilho.
    /// </summary>
    public static void ApplyGameTint(uint? argb)
    {
        if (Application.Current is not { } app)
            return;
        if (argb is not { } v)
        {
            app.Resources["GameTint"] = Brushes.Transparent;
            return;
        }
        var c = Color.FromUInt32(v | 0xFF000000);
        app.Resources["GameTint"] = new LinearGradientBrush
        {
            StartPoint = new RelativePoint(0.5, 0, RelativeUnit.Relative), EndPoint = new RelativePoint(0.5, 1, RelativeUnit.Relative),
            GradientStops = { new GradientStop(Color.FromArgb(0x30, c.R, c.G, c.B), 0), new GradientStop(Color.FromArgb(0, c.R, c.G, c.B), 0.32) },
        };
    }

    public static AccentPreset Find(string? key) => Presets.FirstOrDefault(p => p.Key == key) ?? Default;

    /// <summary>Aplica a cor nos dois temas (claro e escuro).</summary>
    public static void Apply(AccentPreset preset)
    {
        if (Application.Current is not { } app)
            return;
        _opacity ??= ReadOpacities(app);
        // O fundo suave do destaque mistura a cor com o cartao do tema atual.
        Fill(GetThemeDictionary(app, ThemeVariant.Dark), preset.Dark, Color.Parse(AppTheme.Current.Dark.Card), 0.22);
        Fill(GetThemeDictionary(app, ThemeVariant.Light), preset.Light, Color.Parse(AppTheme.Current.Light.Card), 0.12);
    }

    /// <summary>
    /// Pinceis do Fluent que, no tema claro, guardam a cor de destaque do sistema ao carregar e nao seguem
    /// SystemAccentColor (o seletor de nivel ficava azul). Sobrescrevemos todos com a cor escolhida.
    /// Lista obtida varrendo os recursos do FluentTheme 11.3 (pinceis com a cor de destaque padrao).
    /// </summary>
    private static readonly string[] AccentBrushKeys =
    [
        "SystemControlBackgroundAccentBrush",
        "SystemControlDisabledAccentBrush",
        "SystemControlRevealFocusVisualBrush",
        "SystemControlForegroundAccentBrush",
        "SystemControlHighlightAccentBrush",
        "SystemControlHighlightAltAccentBrush",
        "SystemControlHighlightAltListAccentHighBrush",
        "SystemControlHighlightAltListAccentLowBrush",
        "SystemControlHighlightAltListAccentMediumBrush",
        "SystemControlHighlightListAccentHighBrush",
        "SystemControlHighlightListAccentLowBrush",
        "SystemControlHighlightListAccentMediumBrush",
        "SystemControlHyperlinkTextBrush",
        "SystemControlHighlightAccentRevealBackgroundBrush",
        "SystemControlHighlightAccent3RevealBackgroundBrush",
        "SystemControlHighlightAccent2RevealBackgroundBrush",
        "AccentButtonBackground",
        "ToggleButtonBackgroundChecked",
        "HyperlinkButtonForeground",
        "HyperlinkButtonForegroundPointerOver",
        "ComboBoxItemBackgroundSelected",
        "ComboBoxItemBackgroundSelectedPressed",
        "ComboBoxItemBackgroundSelectedPointerOver",
        "ComboBoxBackgroundUnfocused",
        "TextControlBorderBrushFocused",
        "TextControlSelectionHighlightColor",
        "TextControlButtonBackgroundPressed",
        "TextControlButtonForegroundPointerOver",
        "CheckBoxCheckBackgroundStrokeIndeterminate",
        "CheckBoxCheckBackgroundFillChecked",
        "CheckBoxCheckBackgroundFillIndeterminate",
        "CalendarViewSelectedHoverBorderBrush",
        "CalendarViewSelectedPressedBorderBrush",
        "CalendarViewSelectedBorderBrush",
        "RadioButtonOuterEllipseCheckedStroke",
        "RadioButtonOuterEllipseCheckedFill",
        "SliderThumbBackground",
        "SliderTrackValueFill",
        "SliderTrackValueFillPointerOver",
        "SliderTrackValueFillPressed",
        "ToggleSwitchFillOn",
        "ToggleSwitchStrokeOnPointerOver",
        "DatePickerFlyoutPresenterHighlightFill",
        "TimePickerFlyoutPresenterHighlightFill",
        "TabItemHeaderSelectedPipeFill",
        "TreeViewItemBackgroundSelected",
        "TreeViewItemBackgroundSelectedPointerOver",
        "SplitButtonBackgroundChecked",
        "CalendarDatePickerBackgroundFocused",
    ];

    // Opacidade original de cada pincel (as variantes Low/Medium sao translucidas).
    private static Dictionary<string, double>? _opacity;

    private static Dictionary<string, double> ReadOpacities(Application app)
    {
        var result = new Dictionary<string, double>();
        foreach (var key in AccentBrushKeys)
            result[key] = app.TryGetResource(key, ThemeVariant.Light, out var v) && v is IBrush b ? b.Opacity : 1;
        return result;
    }

    internal static ResourceDictionary GetThemeDictionary(Application app, ThemeVariant variant)
    {
        if (app.Resources.ThemeDictionaries.TryGetValue(variant, out var existing) && existing is ResourceDictionary rd)
            return rd;
        var created = new ResourceDictionary();
        app.Resources.ThemeDictionaries[variant] = created;
        return created;
    }

    private static void Fill(ResourceDictionary d, Color accent, Color card, double softAmount)
    {
        foreach (var key in AccentBrushKeys)
            d[key] = new SolidColorBrush(accent, _opacity!.GetValueOrDefault(key, 1));
        d["Accent"] = new SolidColorBrush(accent);
        d["AccentSoft"] = new SolidColorBrush(Mix(card, accent, softAmount));
        d["SystemAccentColor"] = accent;
        d["SystemAccentColorDark1"] = Mix(accent, Colors.Black, 0.12);
        d["SystemAccentColorDark2"] = Mix(accent, Colors.Black, 0.25);
        d["SystemAccentColorDark3"] = Mix(accent, Colors.Black, 0.38);
        d["SystemAccentColorLight1"] = Mix(accent, Colors.White, 0.15);
        d["SystemAccentColorLight2"] = Mix(accent, Colors.White, 0.30);
        d["SystemAccentColorLight3"] = Mix(accent, Colors.White, 0.45);
    }

    /// <summary>Mistura <paramref name="a"/> com <paramref name="b"/> (amount = quanto de b).</summary>
    private static Color Mix(Color a, Color b, double amount)
    {
        byte M(byte x, byte y) => (byte)Math.Round(x + (y - x) * amount);
        return Color.FromRgb(M(a.R, b.R), M(a.G, b.G), M(a.B, b.B));
    }
}
