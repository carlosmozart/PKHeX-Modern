using System;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Animation;
using Avalonia.Animation.Easings;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Transformation;
using Avalonia.Styling;
using Avalonia.VisualTree;

namespace PKHeX.Modern.Controls;

/// <summary>Animações curtas de código (respeitam <see cref="PKHeX.Modern.Theme.Motion.Enabled"/>).</summary>
public static class Effects
{
    /// <summary>
    /// "Pulinho" de um cartão (slot que acabou de receber um Pokémon): 0,86 → 1,08 → 1 em 0,38 s, quadro a quadro.
    /// No fim devolve o RenderTransform ao estilo (o aumento ao passar o mouse continua funcionando).
    /// </summary>
    public static Task PulseAsync(Visual target)
    {
        if (!PKHeX.Modern.Theme.Motion.Enabled || TopLevel.GetTopLevel(target) is not { } top)
            return Task.CompletedTask;
        var done = new TaskCompletionSource();
        var start = TimeSpan.MinValue;
        const double duration = 380;
        void Frame(TimeSpan now)
        {
            if (start == TimeSpan.MinValue) start = now;
            double t = Math.Clamp((now - start).TotalMilliseconds / duration, 0, 1);
            if (t >= 1)
            {
                target.ClearValue(Visual.RenderTransformProperty);
                done.TrySetResult();
                return;
            }
            double eased = 1 - Math.Pow(1 - t, 3);
            double scale = eased < 0.55 ? 0.86 + ((1.08 - 0.86) * (eased / 0.55)) : 1.08 - (0.08 * ((eased - 0.55) / 0.45));
            target.RenderTransform = TransformOperations.Parse(FormattableString.Invariant($"scale({scale:0.###})"));
            top.RequestAnimationFrame(Frame);
        }
        top.RequestAnimationFrame(Frame);
        return done.Task;
    }
}

/// <summary>
/// Brilho de ✦ saindo do centro (editor: o Pokémon ficou legal ou virou shiny). Cada mudança de <see cref="Trigger"/>
/// dispara uma explosão de 0,7 s; sem animação (⚙), não desenha nada.
/// </summary>
public sealed class SparkleBurst : Avalonia.Controls.Control
{
    public static readonly StyledProperty<int> TriggerProperty = AvaloniaProperty.Register<SparkleBurst, int>(nameof(Trigger));
    public static readonly StyledProperty<double> ProgressProperty = AvaloniaProperty.Register<SparkleBurst, double>(nameof(Progress), 1);
    public static readonly StyledProperty<IBrush?> ColorProperty = AvaloniaProperty.Register<SparkleBurst, IBrush?>(nameof(Color));

    public int Trigger { get => GetValue(TriggerProperty); set => SetValue(TriggerProperty, value); }
    /// <summary>0 → 1 durante a explosão (1 = parado, nada na tela).</summary>
    public double Progress { get => GetValue(ProgressProperty); set => SetValue(ProgressProperty, value); }
    public IBrush? Color { get => GetValue(ColorProperty); set => SetValue(ColorProperty, value); }

    static SparkleBurst() => AffectsRender<SparkleBurst>(ProgressProperty, ColorProperty);

    public SparkleBurst() => IsHitTestVisible = false;

    private static readonly Animation Burst = new()
    {
        Duration = TimeSpan.FromMilliseconds(700),
        Easing = new QuadraticEaseOut(),
        Children =
        {
            new KeyFrame { Cue = new Cue(0), Setters = { new Setter(ProgressProperty, 0.0) } },
            new KeyFrame { Cue = new Cue(1), Setters = { new Setter(ProgressProperty, 1.0) } },
        },
    };

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);
        if (change.Property == TriggerProperty && change.GetNewValue<int>() > 0 && PKHeX.Modern.Theme.Motion.Enabled && this.GetVisualRoot() is not null)
            _ = Burst.RunAsync(this);
    }

    public override void Render(DrawingContext context)
    {
        var p = Progress;
        if (p >= 1 || p <= 0) return;
        var brush = Color ?? Brushes.Gold;
        var center = new Point(Bounds.Width / 2, Bounds.Height / 2);
        double reach = Math.Min(Bounds.Width, Bounds.Height) * 0.55;
        using var fade = context.PushOpacity(1 - p);
        for (int i = 0; i < 7; i++)
        {
            double angle = (i * Math.PI * 2 / 7) - (Math.PI / 2) + (p * 0.6);
            double r = reach * (0.25 + (0.75 * p)) * (i % 2 == 0 ? 1 : 0.75);
            double size = (i % 2 == 0 ? 7 : 5) * (1 - (0.4 * p));
            Star(context, brush, new Point(center.X + (r * Math.Cos(angle)), center.Y + (r * Math.Sin(angle))), size);
        }
    }

    /// <summary>✦ de quatro pontas.</summary>
    private static void Star(DrawingContext context, IBrush brush, Point c, double s)
    {
        var geo = new StreamGeometry();
        using (var g = geo.Open())
        {
            g.BeginFigure(new Point(c.X, c.Y - s), true);
            g.LineTo(new Point(c.X + (s * 0.28), c.Y - (s * 0.28)));
            g.LineTo(new Point(c.X + s, c.Y));
            g.LineTo(new Point(c.X + (s * 0.28), c.Y + (s * 0.28)));
            g.LineTo(new Point(c.X, c.Y + s));
            g.LineTo(new Point(c.X - (s * 0.28), c.Y + (s * 0.28)));
            g.LineTo(new Point(c.X - s, c.Y));
            g.LineTo(new Point(c.X - (s * 0.28), c.Y - (s * 0.28)));
            g.EndFigure(true);
        }
        context.DrawGeometry(brush, null, geo);
    }
}

/// <summary>
/// Pokébola que abre ao carregar um save (desenho próprio, 0,9 s, no centro da tela, sem bloquear cliques).
/// Cada mudança de <see cref="Trigger"/> toca de novo; sem animação (⚙), não aparece.
/// </summary>
public sealed class PokeballOpen : Avalonia.Controls.Control
{
    public static readonly StyledProperty<int> TriggerProperty = AvaloniaProperty.Register<PokeballOpen, int>(nameof(Trigger));
    public static readonly StyledProperty<double> ProgressProperty = AvaloniaProperty.Register<PokeballOpen, double>(nameof(Progress), 1);
    public static readonly StyledProperty<IBrush?> TopProperty = AvaloniaProperty.Register<PokeballOpen, IBrush?>(nameof(Top));
    public static readonly StyledProperty<IBrush?> BottomProperty = AvaloniaProperty.Register<PokeballOpen, IBrush?>(nameof(Bottom));
    public static readonly StyledProperty<IBrush?> LineProperty = AvaloniaProperty.Register<PokeballOpen, IBrush?>(nameof(Line));

    public int Trigger { get => GetValue(TriggerProperty); set => SetValue(TriggerProperty, value); }
    public double Progress { get => GetValue(ProgressProperty); set => SetValue(ProgressProperty, value); }
    public IBrush? Top { get => GetValue(TopProperty); set => SetValue(TopProperty, value); }
    public IBrush? Bottom { get => GetValue(BottomProperty); set => SetValue(BottomProperty, value); }
    public IBrush? Line { get => GetValue(LineProperty); set => SetValue(LineProperty, value); }

    static PokeballOpen() => AffectsRender<PokeballOpen>(ProgressProperty, TopProperty, BottomProperty, LineProperty);

    public PokeballOpen() => IsHitTestVisible = false;

    private static readonly Animation Play = new()
    {
        Duration = TimeSpan.FromMilliseconds(900),
        Children =
        {
            new KeyFrame { Cue = new Cue(0), Setters = { new Setter(ProgressProperty, 0.0) } },
            new KeyFrame { Cue = new Cue(1), Setters = { new Setter(ProgressProperty, 1.0) } },
        },
    };

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);
        if (change.Property == TriggerProperty && change.GetNewValue<int>() > 0 && PKHeX.Modern.Theme.Motion.Enabled && this.GetVisualRoot() is not null)
            _ = Play.RunAsync(this);
    }

    public override void Render(DrawingContext context)
    {
        var p = Progress;
        if (p >= 1 || p <= 0) return;
        double size = Math.Min(Bounds.Width, Bounds.Height);
        var c = new Point(Bounds.Width / 2, Bounds.Height / 2);
        double r = size * 0.3;
        // 0–0,3: aparece crescendo; 0,3–0,55: balança; 0,55–1: abre (metade de cima sobe e gira) e some
        double appear = Math.Clamp(p / 0.3, 0, 1);
        double scale = 0.6 + (0.4 * EaseOut(appear));
        double wobble = p is > 0.3 and < 0.55 ? Math.Sin((p - 0.3) / 0.25 * Math.PI * 3) * 14 * (1 - ((p - 0.3) / 0.25)) : 0;
        double open = Math.Clamp((p - 0.55) / 0.45, 0, 1);
        double fade = p < 0.8 ? 1 : 1 - ((p - 0.8) / 0.2);
        var line = new Pen(Line ?? Brushes.Black, r * 0.09);

        using var opacity = context.PushOpacity(fade * appear);
        using var transform = context.PushTransform(
            Matrix.CreateTranslation(-c.X, -c.Y) * Matrix.CreateScale(scale, scale) * Matrix.CreateRotation(wobble * Math.PI / 180) * Matrix.CreateTranslation(c.X, c.Y));

        // Luz saindo de dentro ao abrir
        if (open > 0)
            context.DrawEllipse(new SolidColorBrush(Colors.White, 0.55 * (1 - open)), null, c, r * (0.6 + open), r * (0.25 + (open * 0.6)));

        // Metade de baixo
        var bottom = new StreamGeometry();
        using (var g = bottom.Open())
        {
            g.BeginFigure(new Point(c.X - r, c.Y), true);
            g.ArcTo(new Point(c.X + r, c.Y), new Size(r, r), 0, false, SweepDirection.CounterClockwise);
            g.EndFigure(true);
        }
        context.DrawGeometry(Bottom ?? Brushes.White, line, bottom);

        // Metade de cima (sobe e gira para trás ao abrir)
        var top = new StreamGeometry();
        using (var g = top.Open())
        {
            g.BeginFigure(new Point(c.X - r, c.Y), true);
            g.ArcTo(new Point(c.X + r, c.Y), new Size(r, r), 0, false, SweepDirection.Clockwise);
            g.EndFigure(true);
        }
        var hinge = new Point(c.X - r, c.Y);
        using (context.PushTransform(Matrix.CreateTranslation(-hinge.X, -hinge.Y) * Matrix.CreateRotation(-open * 0.65) * Matrix.CreateTranslation(hinge.X, hinge.Y - (open * r * 0.12))))
        {
            context.DrawGeometry(Top ?? Brushes.Red, line, top);
            context.DrawLine(line, new Point(c.X - r, c.Y), new Point(c.X + r, c.Y));
        }

        // Botão do meio
        context.DrawEllipse(Bottom ?? Brushes.White, line, c, r * 0.24, r * 0.24);
    }

    private static double EaseOut(double t) => 1 - Math.Pow(1 - t, 3);
}
