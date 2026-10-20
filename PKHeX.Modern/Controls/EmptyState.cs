using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;

namespace PKHeX.Modern.Controls;

/// <summary>
/// Desenho das telas vazias (“nenhum save”, “caixa vazia”...): traço simples numa grade de 120×100, com um círculo
/// suave de fundo e os detalhes na cor de destaque. Desenhos próprios, sem arte oficial. As cores vêm do tema
/// (estilo de <c>c|EmptyArt</c> em Theme/Styles.axaml), então acompanham claro/escuro e a cor de destaque.
/// </summary>
public sealed class EmptyArt : Control
{
    public static readonly StyledProperty<string?> KindProperty = AvaloniaProperty.Register<EmptyArt, string?>(nameof(Kind));
    public static readonly StyledProperty<IBrush?> StrokeProperty = AvaloniaProperty.Register<EmptyArt, IBrush?>(nameof(Stroke));
    public static readonly StyledProperty<IBrush?> AccentProperty = AvaloniaProperty.Register<EmptyArt, IBrush?>(nameof(Accent));
    public static readonly StyledProperty<IBrush?> GlowProperty = AvaloniaProperty.Register<EmptyArt, IBrush?>(nameof(Glow));

    /// <summary>save, backup, box, transfer, search ou trophy.</summary>
    public string? Kind { get => GetValue(KindProperty); set => SetValue(KindProperty, value); }
    public IBrush? Stroke { get => GetValue(StrokeProperty); set => SetValue(StrokeProperty, value); }
    public IBrush? Accent { get => GetValue(AccentProperty); set => SetValue(AccentProperty, value); }
    public IBrush? Glow { get => GetValue(GlowProperty); set => SetValue(GlowProperty, value); }

    static EmptyArt() => AffectsRender<EmptyArt>(KindProperty, StrokeProperty, AccentProperty, GlowProperty);

    // (traço neutro, traço/realce na cor de destaque, preenchimento na cor de destaque)
    private static readonly Dictionary<string, (string Base, string Accent, string? Fill)> Art = new()
    {
        // Cartucho com uma Pokébola no rótulo e a seta de “soltar aqui”
        ["save"] = ("M38 26H80A4 4 0 0 1 84 30V82H34V30A4 4 0 0 1 38 26Z M41 34H77V60H41Z M41 70H50 M54 70H63 M67 70H77",
                    "M52 47A7 7 0 1 0 66 47A7 7 0 1 0 52 47Z M52 47H57 M61 47H66 M97 8V28 M90 21L97 28L104 21", null),
        // Disquete com o relógio do backup
        ["backup"] = ("M34 28H70L80 38V82H34Z M45 28V42H67V28 M45 82V64H70V82",
                      "M76 70A14 14 0 1 0 104 70A14 14 0 1 0 76 70Z M90 62V70L96 74", null),
        // Caixa aberta, vazia, com um brilho em cima
        ["box"] = ("M26 46L60 34L94 46L60 58Z M26 46V74L60 88L94 74V46 M60 58V88",
                   "M60 10V24 M53 17H67 M84 18L88 22 M36 18L32 22", null),
        // Dois cartuchos trocando Pokémon
        ["transfer"] = ("M18 30H44V76H18Z M22 36H40V52H22Z M76 30H102V76H76Z M80 36H98V52H80Z",
                        "M50 42H70 M64 36L70 42L64 48 M70 64H50 M56 58L50 64L56 70", null),
        // Lupa com interrogação
        ["search"] = ("M34 46A20 20 0 1 0 74 46A20 20 0 1 0 34 46Z M68 62L88 82",
                      "M47 41A7 7 0 1 1 56.5 47C55.2 47.7 54 48.8 54 50.4V52 M54 58V58.1 M92 18L95 25L102 28L95 31L92 38L89 31L82 28L89 25Z", null),
        // Troféu com estrela
        ["trophy"] = ("M44 24H76V42A16 16 0 0 1 44 42Z M44 30H34A9 9 0 0 0 44 46 M76 30H86A9 9 0 0 1 76 46 M60 58V70 M46 82H74 M51 70H69V82",
                      "", "M60 28L62.6 34L69 34.5L64.2 38.6L65.7 45L60 41.6L54.3 45L55.8 38.6L51 34.5L57.4 34Z"),
    };
    private static readonly Dictionary<string, Geometry> Cache = [];

    public static IEnumerable<string> Kinds => Art.Keys;

    private static Geometry? Parse(string? data)
    {
        if (string.IsNullOrEmpty(data)) return null;
        if (!Cache.TryGetValue(data, out var g))
            Cache[data] = g = Geometry.Parse(data);
        return g;
    }

    protected override Size MeasureOverride(Size availableSize) => new(Width is > 0 ? Width : 120, Height is > 0 ? Height : 100);

    public override void Render(DrawingContext context)
    {
        if (Kind is null || !Art.TryGetValue(Kind, out var art))
            return;
        double scale = System.Math.Min(Bounds.Width / 120, Bounds.Height / 100);
        if (scale <= 0) return;
        var offset = new Vector((Bounds.Width - 120 * scale) / 2, (Bounds.Height - 100 * scale) / 2);
        using var push = context.PushTransform(Matrix.CreateScale(scale, scale) * Matrix.CreateTranslation(offset));
        if (Glow is { } glow)
            context.DrawEllipse(glow, null, new Point(60, 54), 46, 46);
        var stroke = new Pen(Stroke ?? Brushes.Gray, 3, lineCap: PenLineCap.Round, lineJoin: PenLineJoin.Round);
        var accent = new Pen(Accent ?? Brushes.OrangeRed, 3, lineCap: PenLineCap.Round, lineJoin: PenLineJoin.Round);
        if (Parse(art.Base) is { } b) context.DrawGeometry(null, stroke, b);
        if (Parse(art.Accent) is { } a) context.DrawGeometry(null, accent, a);
        if (Parse(art.Fill) is { } f) context.DrawGeometry(Accent, null, f);
    }
}

/// <summary>
/// Tela vazia: desenho, título e dica, centralizados. Botões de ação podem vir como filhos no XAML (entram abaixo da dica).
/// </summary>
public sealed class EmptyState : StackPanel
{
    public static readonly StyledProperty<string?> ArtProperty = AvaloniaProperty.Register<EmptyState, string?>(nameof(Art));
    public static readonly StyledProperty<string?> TitleProperty = AvaloniaProperty.Register<EmptyState, string?>(nameof(Title));
    public static readonly StyledProperty<string?> HintProperty = AvaloniaProperty.Register<EmptyState, string?>(nameof(Hint));
    public static readonly StyledProperty<double> ArtSizeProperty = AvaloniaProperty.Register<EmptyState, double>(nameof(ArtSize), 120);

    public string? Art { get => GetValue(ArtProperty); set => SetValue(ArtProperty, value); }
    public string? Title { get => GetValue(TitleProperty); set => SetValue(TitleProperty, value); }
    public string? Hint { get => GetValue(HintProperty); set => SetValue(HintProperty, value); }
    /// <summary>Largura do desenho (a altura acompanha, 120×100).</summary>
    public double ArtSize { get => GetValue(ArtSizeProperty); set => SetValue(ArtSizeProperty, value); }

    private readonly EmptyArt _art = new() { HorizontalAlignment = HorizontalAlignment.Center };
    private readonly TextBlock _title = new() { HorizontalAlignment = HorizontalAlignment.Center, TextAlignment = TextAlignment.Center, TextWrapping = TextWrapping.Wrap };
    private readonly TextBlock _hint = new() { HorizontalAlignment = HorizontalAlignment.Center, TextAlignment = TextAlignment.Center, TextWrapping = TextWrapping.Wrap };

    public EmptyState()
    {
        Spacing = 10;
        HorizontalAlignment = HorizontalAlignment.Center;
        VerticalAlignment = VerticalAlignment.Center;
        MaxWidth = 520;
        _title.Classes.Add("title");
        _hint.Classes.Add("muted");
        Children.Add(_art);
        Children.Add(_title);
        Children.Add(_hint);
        Sync();
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);
        if (change.Property == ArtProperty || change.Property == TitleProperty || change.Property == HintProperty || change.Property == ArtSizeProperty)
            Sync();
    }

    private void Sync()
    {
        _art.Kind = Art;
        _art.Width = ArtSize;
        _art.Height = ArtSize * 100 / 120;
        _art.IsVisible = !string.IsNullOrEmpty(Art);
        _title.Text = Title;
        _title.IsVisible = !string.IsNullOrEmpty(Title);
        _hint.Text = Hint;
        _hint.IsVisible = !string.IsNullOrEmpty(Hint);
    }
}
