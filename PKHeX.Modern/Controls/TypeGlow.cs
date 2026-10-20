using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.VisualTree;

namespace PKHeX.Modern.Controls;

/// <summary>
/// Brilho da cor do tipo atrás do conteúdo de um cartão de Pokémon (SlotViewModel.TypeGlow). Vai como primeiro filho
/// de um Panel dentro do botão e cobre o cartão inteiro, por baixo do recuo (Padding) e dentro da borda.
/// Some quando a opção "Cor do tipo nos cartões" está desligada (DynamicResource SlotTypeGlowOn, Theme/SlotTypeColor.cs).
/// Como está em todo cartão de Pokémon, também dá o pulinho do cartão quando o slot recebe outro Pokémon
/// (<see cref="ViewModels.SlotViewModel.Pulse"/>).
/// </summary>
public sealed class TypeGlow : Border
{
    private Button? _button;
    private ViewModels.SlotViewModel? _slot;

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
        Watch(DataContext as ViewModels.SlotViewModel);
    }

    protected override void OnDataContextChanged(System.EventArgs e)
    {
        base.OnDataContextChanged(e);
        if (_button is not null) Watch(DataContext as ViewModels.SlotViewModel);
    }

    private void Watch(ViewModels.SlotViewModel? slot)
    {
        if (_slot is not null) _slot.PropertyChanged -= OnSlotChanged;
        _slot = slot;
        if (_slot is not null) _slot.PropertyChanged += OnSlotChanged;
    }

    private void OnSlotChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(ViewModels.SlotViewModel.Pulse) && _button is { } button)
            _ = Effects.PulseAsync(button);
    }

    protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnDetachedFromVisualTree(e);
        if (_button is not null)
            _button.PropertyChanged -= OnButtonChanged;
        _button = null;
        Watch(null);
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
