using System;
using System.Collections.Generic;
using System.Globalization;
using Avalonia;
using Avalonia.Media;

namespace PKHeX.Modern.Controls;

/// <summary>
/// Grafico hexagonal (radar) dos 6 atributos, no estilo de Sword/Shield e Scarlet/Violet: hexagono de fundo
/// preenchido, a forma dos atributos na cor do tipo, os EVs como segunda camada e o valor de cada atributo junto
/// do nome, em vermelho quando a natureza aumenta e em azul quando diminui.
/// </summary>
public sealed class StatRadar : Avalonia.Controls.Control
{
    public static readonly StyledProperty<IReadOnlyList<double>?> ValuesProperty =
        AvaloniaProperty.Register<StatRadar, IReadOnlyList<double>?>(nameof(Values));
    public static readonly StyledProperty<IReadOnlyList<double>?> SecondValuesProperty =
        AvaloniaProperty.Register<StatRadar, IReadOnlyList<double>?>(nameof(SecondValues));
    public static readonly StyledProperty<IReadOnlyList<string>?> LabelsProperty =
        AvaloniaProperty.Register<StatRadar, IReadOnlyList<string>?>(nameof(Labels));
    public static readonly StyledProperty<IReadOnlyList<string>?> ValueTextsProperty =
        AvaloniaProperty.Register<StatRadar, IReadOnlyList<string>?>(nameof(ValueTexts));
    public static readonly StyledProperty<IReadOnlyList<int>?> ModifiersProperty =
        AvaloniaProperty.Register<StatRadar, IReadOnlyList<int>?>(nameof(Modifiers));
    public static readonly StyledProperty<IBrush?> FillProperty =
        AvaloniaProperty.Register<StatRadar, IBrush?>(nameof(Fill));
    public static readonly StyledProperty<IBrush?> SecondFillProperty =
        AvaloniaProperty.Register<StatRadar, IBrush?>(nameof(SecondFill));
    public static readonly StyledProperty<IBrush?> PlateProperty =
        AvaloniaProperty.Register<StatRadar, IBrush?>(nameof(Plate));
    public static readonly StyledProperty<IBrush?> GridBrushProperty =
        AvaloniaProperty.Register<StatRadar, IBrush?>(nameof(GridBrush));
    public static readonly StyledProperty<IBrush?> LabelBrushProperty =
        AvaloniaProperty.Register<StatRadar, IBrush?>(nameof(LabelBrush));
    /// <summary>Cor do texto em volta (herdada), usada nos números quando <see cref="ValueBrush"/> não vem.</summary>
    public static readonly StyledProperty<IBrush?> ForegroundProperty =
        Avalonia.Controls.Documents.TextElement.ForegroundProperty.AddOwner<StatRadar>();
    public static readonly StyledProperty<IBrush?> ValueBrushProperty =
        AvaloniaProperty.Register<StatRadar, IBrush?>(nameof(ValueBrush));

    static StatRadar() => AffectsRender<StatRadar>(ValuesProperty, SecondValuesProperty, LabelsProperty, ValueTextsProperty, ModifiersProperty,
        FillProperty, SecondFillProperty, PlateProperty, GridBrushProperty, LabelBrushProperty, ValueBrushProperty, ForegroundProperty);

    /// <summary>Valores normalizados (0..1), ordem PS, Atq, Def, AtE, DeE, Vel.</summary>
    public IReadOnlyList<double>? Values { get => GetValue(ValuesProperty); set => SetValue(ValuesProperty, value); }
    /// <summary>Segunda camada (EVs, 0..1), mesma ordem; null = sem camada.</summary>
    public IReadOnlyList<double>? SecondValues { get => GetValue(SecondValuesProperty); set => SetValue(SecondValuesProperty, value); }
    public IReadOnlyList<string>? Labels { get => GetValue(LabelsProperty); set => SetValue(LabelsProperty, value); }
    /// <summary>Número mostrado embaixo de cada nome (o atributo final); null = só o nome.</summary>
    public IReadOnlyList<string>? ValueTexts { get => GetValue(ValueTextsProperty); set => SetValue(ValueTextsProperty, value); }
    /// <summary>+1 / -1 / 0 da natureza, mesma ordem: o nome fica vermelho (+) ou azul (−).</summary>
    public IReadOnlyList<int>? Modifiers { get => GetValue(ModifiersProperty); set => SetValue(ModifiersProperty, value); }
    public IBrush? Fill { get => GetValue(FillProperty); set => SetValue(FillProperty, value); }
    public IBrush? SecondFill { get => GetValue(SecondFillProperty); set => SetValue(SecondFillProperty, value); }
    /// <summary>Preenchimento do hexágono de fundo.</summary>
    public IBrush? Plate { get => GetValue(PlateProperty); set => SetValue(PlateProperty, value); }
    public IBrush? GridBrush { get => GetValue(GridBrushProperty); set => SetValue(GridBrushProperty, value); }
    public IBrush? LabelBrush { get => GetValue(LabelBrushProperty); set => SetValue(LabelBrushProperty, value); }
    public IBrush? ValueBrush { get => GetValue(ValueBrushProperty); set => SetValue(ValueBrushProperty, value); }
    public IBrush? Foreground { get => GetValue(ForegroundProperty); set => SetValue(ForegroundProperty, value); }

    // Ordem no hexagono (sentido horario a partir do topo): PS, Atq, Def, Vel, DeE, AtE
    private static readonly int[] Order = [0, 1, 2, 5, 4, 3];
    private static readonly IBrush Up = new SolidColorBrush(Color.Parse("#F0605A"));
    private static readonly IBrush Down = new SolidColorBrush(Color.Parse("#5B9BF0"));

    public override void Render(DrawingContext ctx)
    {
        var size = Math.Min(Bounds.Width, Bounds.Height);
        var center = new Point(Bounds.Width / 2, Bounds.Height / 2);
        bool withValues = ValueTexts is { Count: 6 };
        var radius = (size / 2) - (withValues ? 30 : 22);
        if (radius <= 0)
            return;

        Point At(int i, double r)
        {
            var angle = (-Math.PI / 2) + (i * Math.PI / 3);
            return new Point(center.X + (r * Math.Cos(angle)), center.Y + (r * Math.Sin(angle)));
        }

        var gridPen = new Pen(GridBrush ?? Brushes.Gray, 1);
        ctx.DrawGeometry(Plate, new Pen(GridBrush ?? Brushes.Gray, 1.5), Polygon(i => At(i, radius)));
        foreach (var f in (double[])[0.25, 0.5, 0.75])
            ctx.DrawGeometry(null, gridPen, Polygon(i => At(i, radius * f)));
        for (int i = 0; i < 6; i++)
            ctx.DrawLine(gridPen, center, At(i, radius));

        if (Values is { Count: 6 } v)
        {
            var fill = Fill as ISolidColorBrush ?? Brushes.CornflowerBlue;
            var geo = Polygon(i => At(i, radius * Math.Clamp(v[Order[i]], 0.04, 1)));
            ctx.DrawGeometry(new SolidColorBrush(fill.Color, 0.45), new Pen(fill, 2.5, lineJoin: PenLineJoin.Round), geo);
        }
        if (SecondValues is { Count: 6 } ev)
        {
            var fill = SecondFill as ISolidColorBrush ?? Brushes.Gold;
            var geo = Polygon(i => At(i, radius * Math.Clamp(ev[Order[i]], 0, 1)));
            ctx.DrawGeometry(new SolidColorBrush(fill.Color, 0.30), new Pen(fill, 1.5, lineJoin: PenLineJoin.Round), geo);
        }

        if (Labels is { Count: 6 } l)
        {
            for (int i = 0; i < 6; i++)
            {
                int stat = Order[i];
                var mod = Modifiers is { Count: 6 } m ? m[stat] : 0;
                var nameBrush = mod > 0 ? Up : mod < 0 ? Down : LabelBrush ?? Brushes.Gray;
                var name = new FormattedText(l[stat], CultureInfo.CurrentCulture, FlowDirection.LeftToRight,
                    new Typeface(Typeface.Default.FontFamily, weight: mod != 0 ? FontWeight.Bold : FontWeight.Normal), 11, nameBrush);
                var p = At(i, radius + (withValues ? 20 : 13));
                if (!withValues)
                {
                    ctx.DrawText(name, new Point(p.X - (name.Width / 2), p.Y - (name.Height / 2)));
                    continue;
                }
                var value = new FormattedText(ValueTexts![stat], CultureInfo.CurrentCulture, FlowDirection.LeftToRight,
                    new Typeface(Typeface.Default.FontFamily, weight: FontWeight.Bold), 12, ValueBrush ?? Foreground ?? Brushes.White);
                double h = name.Height + value.Height;
                ctx.DrawText(name, new Point(p.X - (name.Width / 2), p.Y - (h / 2)));
                ctx.DrawText(value, new Point(p.X - (value.Width / 2), p.Y - (h / 2) + name.Height));
            }
        }
    }

    private static StreamGeometry Polygon(Func<int, Point> point)
    {
        var geo = new StreamGeometry();
        using var g = geo.Open();
        g.BeginFigure(point(0), true);
        for (int i = 1; i < 6; i++)
            g.LineTo(point(i));
        g.EndFigure(true);
        return geo;
    }
}
