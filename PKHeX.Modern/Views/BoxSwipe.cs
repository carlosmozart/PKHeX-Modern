using System;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.VisualTree;

namespace PKHeX.Modern.Views;

/// <summary>
/// Deslizar o dedo para o lado numa caixa troca de caixa (esquerda = próxima, direita = anterior). Vale nas áreas
/// com a classe <c>boxSwipe</c> (página Caixas e os três painéis do Bank) e usa as setas ‹ › da própria área, então
/// cada painel troca a sua caixa. Só toque e só um movimento rápido: segurar continua sendo marcar/arrastar
/// (<see cref="SlotDragController"/>), e quem começa numa lista que rola para o lado (abas das caixas) rola a lista.
/// </summary>
public sealed class BoxSwipe
{
    /// <summary>Distância mínima na horizontal (em DIPs).</summary>
    public const double MinDistance = 70;
    /// <summary>Mais rápido que o "segurar" do arraste (550 ms).</summary>
    public static readonly TimeSpan MaxDuration = TimeSpan.FromMilliseconds(500);

    private readonly Control _root;
    private Control? _area;
    private Point _start;
    private DateTime _startTime;

    public BoxSwipe(Control root)
    {
        _root = root;
        root.AddHandler(InputElement.PointerPressedEvent, OnPressed, RoutingStrategies.Tunnel, handledEventsToo: true);
        root.AddHandler(InputElement.PointerReleasedEvent, OnReleased, RoutingStrategies.Tunnel, handledEventsToo: true);
    }

    /// <summary>-1 = caixa anterior, 1 = próxima, 0 = não é deslize (curto, lento ou mais vertical que horizontal).</summary>
    public static int Classify(Vector delta, TimeSpan elapsed)
    {
        if (elapsed > MaxDuration || Math.Abs(delta.X) < MinDistance || Math.Abs(delta.X) < Math.Abs(delta.Y) * 1.8)
            return 0;
        return delta.X < 0 ? 1 : -1;
    }

    /// <summary>A área de caixas onde o toque começou, se não for dentro de uma lista que rola para o lado.</summary>
    public static Control? AreaAt(Visual? source)
    {
        foreach (var v in source?.GetSelfAndVisualAncestors() ?? [])
        {
            if (v is ScrollViewer sv && sv.Extent.Width > sv.Viewport.Width + 1)
                return null;
            if (v is Control c && c.Classes.Contains("boxSwipe"))
                return c;
        }
        return null;
    }

    /// <summary>Troca a caixa da área pela seta dela (‹ ou ›). Devolve se trocou.</summary>
    public static bool Swipe(Control area, int direction)
    {
        var arrow = area.GetVisualDescendants().OfType<Button>()
            .FirstOrDefault(b => b.Classes.Contains("boxArrow") && b.IsEffectivelyVisible && (string?)b.Content == (direction > 0 ? "›" : "‹"));
        if (arrow?.Command is not { } cmd || !cmd.CanExecute(arrow.CommandParameter))
            return false;
        cmd.Execute(arrow.CommandParameter);
        return true;
    }

    private void OnPressed(object? sender, PointerPressedEventArgs e)
    {
        _area = e.Pointer.Type == PointerType.Touch ? AreaAt(e.Source as Visual) : null;
        _start = e.GetPosition(_root);
        _startTime = DateTime.UtcNow;
    }

    private void OnReleased(object? sender, PointerReleasedEventArgs e)
    {
        var area = _area;
        _area = null;
        if (area is null || e.Pointer.Type != PointerType.Touch)
            return;
        int direction = Classify(e.GetPosition(_root) - _start, DateTime.UtcNow - _startTime);
        if (direction != 0 && Swipe(area, direction))
            e.Handled = true;
    }
}
