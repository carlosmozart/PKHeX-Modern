using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Threading;
using Avalonia.VisualTree;
using PKHeX.Modern.ViewModels;

namespace PKHeX.Modern.Views;

public partial class CommandPaletteView : UserControl
{
    public CommandPaletteView()
    {
        InitializeComponent();
        // Tunel: as setas e o Enter chegam aqui antes do TextBox mexer no cursor
        PaletteInput.AddHandler(KeyDownEvent, OnInputKey, RoutingStrategies.Tunnel);
        PaletteList.AddHandler(PointerReleasedEvent, OnListClick, RoutingStrategies.Bubble, handledEventsToo: true);
        PaletteList.SelectionChanged += (_, _) => { if (PaletteList.SelectedItem is { } item) PaletteList.ScrollIntoView(item); };
    }

    protected override void OnDataContextChanged(System.EventArgs e)
    {
        base.OnDataContextChanged(e);
        if (DataContext is CommandPaletteViewModel)
            Dispatcher.UIThread.Post(() => PaletteInput.Focus(), DispatcherPriority.Input);
    }

    private void OnInputKey(object? sender, KeyEventArgs e)
    {
        if (DataContext is not CommandPaletteViewModel vm) return;
        switch (e.Key)
        {
            case Key.Down: vm.Move(+1); e.Handled = true; break;
            case Key.Up: vm.Move(-1); e.Handled = true; break;
            case Key.Enter or Key.Return: vm.RunSelected(); e.Handled = true; break;
            case Key.Escape: vm.CloseCommand.Execute(null); e.Handled = true; break;
        }
    }

    private void OnListClick(object? sender, PointerReleasedEventArgs e)
    {
        if (e.InitialPressMouseButton != MouseButton.Left) return;
        if (DataContext is CommandPaletteViewModel vm && (e.Source as Visual)?.FindAncestorOfType<ListBoxItem>(includeSelf: true)?.DataContext is PaletteItem item)
            vm.Run(item);
    }
}
