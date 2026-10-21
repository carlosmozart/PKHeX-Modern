using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Documents;
using Avalonia.Media;

namespace PKHeX.Modern.Controls;

/// <summary>
/// Tampa da Pokédex no topo da barra lateral: faixa na cor de destaque com o degrau da tampa embaixo (alta na
/// esquerda, onde fica a lente, e mais baixa na direita). O texto de dentro fica branco ou escuro conforme a cor.
/// </summary>
public sealed class DexBand : Decorator
{
    public static readonly StyledProperty<IBrush?> FillProperty = AvaloniaProperty.Register<DexBand, IBrush?>(nameof(Fill));
    public static readonly StyledProperty<IBrush?> EdgeProperty = AvaloniaProperty.Register<DexBand, IBrush?>(nameof(Edge));

    public IBrush? Fill { get => GetValue(FillProperty); set => SetValue(FillProperty, value); }
    /// <summary>Linha escura do contorno do degrau.</summary>
    public IBrush? Edge { get => GetValue(EdgeProperty); set => SetValue(EdgeProperty, value); }

    /// <summary>Altura do degrau (o lado direito sobe isso).</summary>
    private const double Step = 12;

    static DexBand() => AffectsRender<DexBand>(FillProperty, EdgeProperty);

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);
        if (change.Property == FillProperty)
            SetValue(TextElement.ForegroundProperty, Fill is ISolidColorBrush s && Luminance(s.Color) > 0.55 ? new SolidColorBrush(Color.FromRgb(0x1E, 0x1E, 0x24)) : Brushes.White);
    }

    private static double Luminance(Color c) => ((0.299 * c.R) + (0.587 * c.G) + (0.114 * c.B)) / 255;

    public override void Render(DrawingContext context)
    {
        double w = Bounds.Width, h = Bounds.Height, cut = w * 0.58;
        var geo = new StreamGeometry();
        using (var g = geo.Open())
        {
            g.BeginFigure(new Point(0, 0), true);
            g.LineTo(new Point(w, 0));
            g.LineTo(new Point(w, h - Step));
            g.LineTo(new Point(cut + Step, h - Step));
            g.LineTo(new Point(cut, h));
            g.LineTo(new Point(0, h));
            g.EndFigure(true);
        }
        context.DrawGeometry(Fill, null, geo);
        if (Edge is null) return;
        var pen = new Pen(Edge, 2);
        context.DrawLine(pen, new Point(0, h - 1), new Point(cut, h - 1));
        context.DrawLine(pen, new Point(cut, h - 1), new Point(cut + Step, h - Step - 1));
        context.DrawLine(pen, new Point(cut + Step, h - Step - 1), new Point(w, h - Step - 1));
    }
}

/// <summary>Lente azul da Pokédex (desenho próprio).</summary>
public sealed class DexLens : Control
{
    internal static readonly Pen Outline = new(new SolidColorBrush(Color.FromArgb(0x99, 0x10, 0x10, 0x18)), 1.2);
    private static readonly IBrush Rim = new SolidColorBrush(Color.FromRgb(0xF5, 0xF5, 0xF7));
    private static readonly IBrush Glass = new RadialGradientBrush
    {
        GradientOrigin = new RelativePoint(0.35, 0.3, RelativeUnit.Relative),
        Center = new RelativePoint(0.4, 0.35, RelativeUnit.Relative),
        GradientStops = { new GradientStop(Color.FromRgb(0x9B, 0xE3, 0xFF), 0), new GradientStop(Color.FromRgb(0x2F, 0x9B, 0xF0), 0.45), new GradientStop(Color.FromRgb(0x10, 0x4E, 0xA8), 1) },
    };
    private static readonly IBrush Shine = new SolidColorBrush(Color.FromArgb(0xD0, 0xFF, 0xFF, 0xFF));

    public override void Render(DrawingContext context)
    {
        double r = System.Math.Min(Bounds.Width, Bounds.Height) / 2;
        var c = new Point(r, r);
        context.DrawEllipse(Rim, Outline, c, r - 0.6, r - 0.6);
        double g = r * 0.74;
        context.DrawEllipse(Glass, Outline, c, g, g);
        context.DrawEllipse(Shine, null, new Point(r - (g * 0.38), r - (g * 0.4)), g * 0.24, g * 0.17);
    }
}

/// <summary>As três luzes da Pokédex (vermelha, amarela e verde), lado a lado na altura do controle.</summary>
public sealed class DexLights : Control
{
    private static readonly IBrush[] Lights =
    [
        new SolidColorBrush(Color.FromRgb(0xF0, 0x3A, 0x3A)),
        new SolidColorBrush(Color.FromRgb(0xFF, 0xD2, 0x3F)),
        new SolidColorBrush(Color.FromRgb(0x4C, 0xC2, 0x5A)),
    ];

    protected override Size MeasureOverride(Size availableSize)
    {
        double d = double.IsFinite(Height) ? Height : 8;
        return new Size((d * Lights.Length) + (d * 0.5 * (Lights.Length - 1)), d);
    }

    public override void Render(DrawingContext context)
    {
        double d = Bounds.Height, r = (d / 2) - 0.6;
        for (int i = 0; i < Lights.Length; i++)
            context.DrawEllipse(Lights[i], DexLens.Outline, new Point((d / 2) + (i * d * 1.5), d / 2), r, r);
    }
}

/// <summary>Divisória com a linha e o botão do meio da Pokébola.</summary>
public sealed class BallDivider : Control
{
    public static readonly StyledProperty<IBrush?> StrokeProperty = AvaloniaProperty.Register<BallDivider, IBrush?>(nameof(Stroke));
    public static readonly StyledProperty<IBrush?> FillProperty = AvaloniaProperty.Register<BallDivider, IBrush?>(nameof(Fill));

    public IBrush? Stroke { get => GetValue(StrokeProperty); set => SetValue(StrokeProperty, value); }
    /// <summary>Cor de dentro do botão (a do fundo, para "cortar" a linha).</summary>
    public IBrush? Fill { get => GetValue(FillProperty); set => SetValue(FillProperty, value); }

    static BallDivider() => AffectsRender<BallDivider>(StrokeProperty, FillProperty);

    protected override Size MeasureOverride(Size availableSize) => new(0, 16);

    public override void Render(DrawingContext context)
    {
        if (Stroke is null) return;
        double w = Bounds.Width, y = Bounds.Height / 2;
        var pen = new Pen(Stroke, 2);
        context.DrawLine(pen, new Point(0, y), new Point(w, y));
        var c = new Point(w / 2, y);
        context.DrawEllipse(Fill, pen, c, 7, 7);
        context.DrawEllipse(null, new Pen(Stroke, 1.5), c, 3, 3);
    }
}
