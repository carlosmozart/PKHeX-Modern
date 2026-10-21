using System.Collections.Generic;
using System.Globalization;
using Avalonia;
using Avalonia.Controls.Documents;
using Avalonia.Media;

namespace PKHeX.Modern.Controls;

/// <summary>
/// Icone vetorial de traco (grade de 24×24) na cor do texto em volta, com a cor do objeto por baixo, para a barra lateral e o Inicio. Substitui os
/// emojis, que no Android tem alturas e larguras diferentes e desalinhavam a lista. <see cref="Kind"/> aceita o
/// proprio emoji da pagina (<c>PageViewModel.Icon</c>), entao os ViewModels nao mudam; sem desenho, mostra o texto.
/// </summary>
public sealed class LineIcon : Avalonia.Controls.Control
{
    public static readonly StyledProperty<string?> KindProperty =
        AvaloniaProperty.Register<LineIcon, string?>(nameof(Kind));
    public static readonly StyledProperty<IBrush?> ForegroundProperty =
        TextElement.ForegroundProperty.AddOwner<LineIcon>();
    public static readonly StyledProperty<double> StrokeWidthProperty =
        AvaloniaProperty.Register<LineIcon, double>(nameof(StrokeWidth), 2);

    public string? Kind { get => GetValue(KindProperty); set => SetValue(KindProperty, value); }
    public IBrush? Foreground { get => GetValue(ForegroundProperty); set => SetValue(ForegroundProperty, value); }
    /// <summary>Espessura do traco na grade de 24 (escala junto com o icone).</summary>
    public double StrokeWidth { get => GetValue(StrokeWidthProperty); set => SetValue(StrokeWidthProperty, value); }

    static LineIcon() => AffectsRender<LineIcon>(KindProperty, ForegroundProperty, StrokeWidthProperty);

    /// <summary>Um desenho: o traco, a area preenchida por baixo dele (opcional) e a cor dessa area no modo colorido.</summary>
    private sealed record Glyph(string Stroke, string? Fill = null, uint Tint = 0);

    /// <summary>Desenhos proprios com traco de jogo (Pokebola, Pokedex, PC, mochila...), pelo emoji que cada pagina ja usa.</summary>
    private static readonly Dictionary<string, Glyph> Paths = new()
    {
        // Inicio: casa com o telhado do Centro Pokemon
        ["⌂"] = new("M3.5 11L12 4L20.5 11 M6 9.5V20H18V9.5 M10 20V15.5A2 2 0 0 1 14 15.5V20", "M3.5 11L12 4L20.5 11L18 11V9.5L6 9.5V11Z", 0xE5484D),
        // Caixas: caixa do PC (tampa e corpo, com a alca)
        ["▦"] = new("M3 5.5A1 1 0 0 1 4 4.5H20A1 1 0 0 1 21 5.5V9H3Z M4.5 9V19A1 1 0 0 0 5.5 20H18.5A1 1 0 0 0 19.5 19V9 M10 13H14", "M4.5 9H19.5V19A1 1 0 0 1 18.5 20H5.5A1 1 0 0 1 4.5 19Z", 0xC9894A),
        // Equipe: Pokebola
        ["◉"] = new("M3 12A9 9 0 1 0 21 12A9 9 0 1 0 3 12Z M3 12H9 M15 12H21 M9 12A3 3 0 1 0 15 12A3 3 0 1 0 9 12Z", "M3 12A9 9 0 0 1 21 12H15A3 3 0 0 0 9 12Z", 0xE5484D),
        // Bank: o PC (monitor com uma Pokebola na tela)
        ["🏦"] = new("M3 5A1 1 0 0 1 4 4H20A1 1 0 0 1 21 5V15A1 1 0 0 1 20 16H4A1 1 0 0 1 3 15Z M12 16V19.5 M8 20H16 M9.5 10A2.5 2.5 0 1 0 14.5 10A2.5 2.5 0 1 0 9.5 10Z M9.5 10H14.5", "M3 5A1 1 0 0 1 4 4H20A1 1 0 0 1 21 5V15A1 1 0 0 1 20 16H4A1 1 0 0 1 3 15Z", 0x3E8ED0),
        // Pokedex: o aparelho, com a lente, as luzes e a tela
        ["📖"] = new("M5 3H19A1.5 1.5 0 0 1 20.5 4.5V19.5A1.5 1.5 0 0 1 19 21H5A1.5 1.5 0 0 1 3.5 19.5V4.5A1.5 1.5 0 0 1 5 3Z M6.5 7.5A2 2 0 1 0 10.5 7.5A2 2 0 1 0 6.5 7.5Z M13.5 6.5V6.51 M16.5 6.5V6.51 M7 12.5H17V17.5H7Z", "M5 3H19A1.5 1.5 0 0 1 20.5 4.5V19.5A1.5 1.5 0 0 1 19 21H5A1.5 1.5 0 0 1 3.5 19.5V4.5A1.5 1.5 0 0 1 5 3Z", 0xE5484D),
        // Treinador: cabeca com bone e ombros
        ["♟"] = new("M8.5 9.5A3.5 3.5 0 0 0 15.5 9.5 M8.5 9.5V10.5A3.5 3.5 0 0 0 15.5 10.5V9.5 M7.5 9.5A4.5 4.5 0 0 1 16.5 9.5H20 M7.5 9.5H16.5 M4.5 21A7.5 7.5 0 0 1 19.5 21", "M7.5 9.5A4.5 4.5 0 0 1 16.5 9.5Z", 0xE5484D),
        // Mochila: corpo, alca e bolso
        ["🎒"] = new("M5 10.5A4.5 4.5 0 0 1 9.5 6H14.5A4.5 4.5 0 0 1 19 10.5V19.5A1 1 0 0 1 18 20.5H6A1 1 0 0 1 5 19.5Z M9.5 6V4.5A1 1 0 0 1 10.5 3.5H13.5A1 1 0 0 1 14.5 4.5V6 M8 13.5H16V17.5H8Z M8 13.5L9 12H15L16 13.5 M5 16H3.5 M19 16H20.5", "M5 10.5A4.5 4.5 0 0 1 9.5 6H14.5A4.5 4.5 0 0 1 19 10.5V19.5A1 1 0 0 1 18 20.5H6A1 1 0 0 1 5 19.5Z", 0xD9A23A),
        // Encontros: mato alto
        ["🌿"] = new("M3 20.5H21 M4.5 20.5L7 11L9.5 20.5 M9 20.5L12 6.5L15 20.5 M14.5 20.5L17 11.5L19.5 20.5", "M4.5 20.5L7 11L9.5 20.5Z M9 20.5L12 6.5L15 20.5Z M14.5 20.5L17 11.5L19.5 20.5Z", 0x4CB05A),
        // Eventos: presente do Mystery Gift
        ["🎁"] = new("M4 8.5H20V12.5H4Z M5.5 12.5H18.5V20.5H5.5Z M12 8.5V20.5 M12 8.5C10.5 4.5 7 4.5 7 6.8C7 8.2 9 8.5 12 8.5Z M12 8.5C13.5 4.5 17 4.5 17 6.8C17 8.2 15 8.5 12 8.5Z", "M5.5 12.5H18.5V20.5H5.5Z M4 8.5H20V12.5H4Z", 0x9B6BD6),
        // Saves: cartucho do jogo
        ["🗂"] = new("M6 3H15.5L18.5 6V20A1 1 0 0 1 17.5 21H7A1 1 0 0 1 6 20Z M9 3V6.5 M11.5 3V6.5 M8.5 10H16V17.5H8.5Z", "M8.5 10H16V17.5H8.5Z", 0x3E8ED0),
        // Jogo: portatil de duas telas
        ["🎮"] = new("M5 3H19A1 1 0 0 1 20 4V11H4V4A1 1 0 0 1 5 3Z M7 5.5H17V9H7Z M4 13H20V20A1 1 0 0 1 19 21H5A1 1 0 0 1 4 20Z M7.5 15.5V18.5 M6 17H9 M15.5 16V16.01 M17.5 18V18.01", "M7 5.5H17V9H7Z M4 13H20V20A1 1 0 0 1 19 21H5A1 1 0 0 1 4 20Z", 0x6C7A89),
        // Pesquisa: lupa com uma Pokebola na lente
        ["🔎"] = new("M4 10.5A6.5 6.5 0 1 0 17 10.5A6.5 6.5 0 1 0 4 10.5Z M4 10.5H8.5 M12.5 10.5H17 M8.5 10.5A2 2 0 1 0 12.5 10.5A2 2 0 1 0 8.5 10.5Z M15.3 15.3L20.5 20.5", "M4 10.5A6.5 6.5 0 0 1 17 10.5H12.5A2 2 0 0 0 8.5 10.5Z", 0xE5484D),
        // Edicao em lote: controles deslizantes
        ["⚙"] = new("M4 7H11 M15 7H20 M13 5V9 M4 17H7 M11 17H20 M9 15V19 M4 12H15 M19 12H20 M17 10V14"),
        // Ajuda: balao com interrogacao
        ["❔"] = new("M3 12A9 9 0 1 0 21 12A9 9 0 1 0 3 12Z M9.5 9.5A2.5 2.5 0 1 1 13 11.8C12.4 12.1 12 12.6 12 13.3V14 M12 17V17.01", "M3 12A9 9 0 1 0 21 12A9 9 0 1 0 3 12Z", 0x3E8ED0),
        // Cacada shiny: brilhos
        ["✨"] = new("M11 3L12.8 9.2L19 11L12.8 12.8L11 19L9.2 12.8L3 11L9.2 9.2Z M19 3V7 M17 5H21", "M11 3L12.8 9.2L19 11L12.8 12.8L11 19L9.2 12.8L3 11L9.2 9.2Z", 0xF2C230),
        // Salvar: disquete
        ["💾"] = new("M5 4H16L20 8V19A1 1 0 0 1 19 20H5A1 1 0 0 1 4 19V5A1 1 0 0 1 5 4Z M8 4V9H15V4 M8 20V14H16V20", "M8 14H16V20H8Z", 0x3E8ED0),
    };

    private static readonly Dictionary<string, Geometry> Cache = [];
    private static readonly Dictionary<uint, IImmutableBrush> Tints = [];

    /// <summary>Tem desenho para este emoji (os testes conferem que toda pagina tem um).</summary>
    public static bool HasPath(string? kind) => kind is not null && Paths.ContainsKey(kind);

    private static Geometry? GetGeometry(string? data)
    {
        if (data is null)
            return null;
        if (!Cache.TryGetValue(data, out var geometry))
            Cache[data] = geometry = Geometry.Parse(data);
        return geometry;
    }

    protected override Size MeasureOverride(Size availableSize) => new(Width is > 0 ? Width : 20, Height is > 0 ? Height : 20);

    public override void Render(DrawingContext context)
    {
        var brush = Foreground ?? Brushes.Gray;
        double size = System.Math.Min(Bounds.Width, Bounds.Height);
        if (size <= 0)
            return;
        var glyph = Kind is null ? null : Paths.GetValueOrDefault(Kind);
        if (GetGeometry(glyph?.Stroke) is not { } geometry)
        {
            if (string.IsNullOrEmpty(Kind))
                return;
            var text = new FormattedText(Kind, CultureInfo.CurrentUICulture, FlowDirection.LeftToRight, Typeface.Default, size * 0.75, brush);
            context.DrawText(text, new Point((Bounds.Width - text.Width) / 2, (Bounds.Height - text.Height) / 2));
            return;
        }
        double scale = size / 24;
        var offset = new Point((Bounds.Width - size) / 2, (Bounds.Height - size) / 2);
        using (context.PushTransform(Matrix.CreateScale(scale, scale) * Matrix.CreateTranslation(offset.X, offset.Y)))
        {
            // Cor do objeto (Pokebola vermelha, mato verde...) por baixo do traco, um pouco translucida
            if (GetGeometry(glyph!.Fill) is { } fill)
            {
                if (!Tints.TryGetValue(glyph.Tint, out var tint))
                    Tints[glyph.Tint] = tint = new SolidColorBrush(Color.FromUInt32(0xFF000000 | glyph.Tint), 0.75).ToImmutable();
                context.DrawGeometry(tint, null, fill);
            }
            var pen = new Pen(brush, StrokeWidth, lineCap: PenLineCap.Round, lineJoin: PenLineJoin.Round);
            context.DrawGeometry(null, pen, geometry);
        }
    }
}
