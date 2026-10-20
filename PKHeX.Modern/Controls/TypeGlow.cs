using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.VisualTree;

namespace PKHeX.Modern.Controls;

/// <summary>
/// Brilho da cor do tipo atrás do conteúdo de um cartão de Pokémon (SlotViewModel.TypeGlow). Vai como primeiro filho
/// de um Panel dentro do botão e cobre o cartão inteiro, por baixo do recuo (Padding) e dentro da borda.
/// Some quando a opção "Cor do tipo nos cartões" está desligada (DynamicResource SlotTypeGlowOn, Theme/SlotTypeColor.cs).
/// </summary>
public sealed class TypeGlow : Border
{
    private Button? _button;

    public TypeGlow()
    {
        IsHitTestVisible = false;
        Bind(BackgroundProperty, new Binding("TypeGlow"));
        Bind(IsVisibleProperty, this.GetResourceObservable("SlotTypeGlowOn", o => o is not false));
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        _button = this.FindAncestorOfType<Button>();
        if (_button is null)
            return;
        _button.PropertyChanged += OnButtonChanged;
        Fit();
    }

    protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnDetachedFromVisualTree(e);
        if (_button is not null)
            _button.PropertyChanged -= OnButtonChanged;
        _button = null;
    }

    private void OnButtonChanged(object? sender, AvaloniaPropertyChangedEventArgs e)
    {
        if (e.Property == Button.PaddingProperty || e.Property == Button.CornerRadiusProperty || e.Property == Button.BorderThicknessProperty)
            Fit();
    }

    private void Fit()
    {
        if (_button is null)
            return;
        var p = _button.Padding;
        Margin = new Thickness(-p.Left, -p.Top, -p.Right, -p.Bottom);
        var r = _button.CornerRadius;
        var b = _button.BorderThickness.Left;
        CornerRadius = new CornerRadius(System.Math.Max(0, r.TopLeft - b), System.Math.Max(0, r.TopRight - b), System.Math.Max(0, r.BottomRight - b), System.Math.Max(0, r.BottomLeft - b));
    }
}
