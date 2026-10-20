using Avalonia;
using Avalonia.Media.Imaging;

namespace PKHeX.Modern.ViewModels;

/// <summary>
/// Prévia do arraste de slots (Views/SlotDragController.cs): o sprite do Pokémon segue o cursor, com o número de
/// Pokémon do grupo e o que vai acontecer ao soltar ("Mover", "⇄ Trocar", "Copiar"...). A origem fica apagada
/// enquanto o Pokémon está "na mão". No toque, o sprite fica acima do dedo.
/// </summary>
public sealed class DragPreviewViewModel : ViewModelBase
{
    private SlotViewModel? _source;
    private bool _sourceWasDimmed;
    private bool _touch;

    public bool IsVisible { get; private set; }
    public Bitmap? Sprite { get; private set; }
    public double X { get; private set; }
    public double Y { get; private set; }
    /// <summary>Quantos Pokémon vão juntos (seleção múltipla); 1 = só o arrastado.</summary>
    public int Count { get; private set; }
    public bool HasCount => Count > 1;
    public string CountText => $"×{Count}";
    public string Action { get; private set; } = "";
    public bool HasAction => Action.Length > 0;

    public void Begin(SlotViewModel source, int count, bool touch)
    {
        _source = source;
        _touch = touch;
        _sourceWasDimmed = source.IsDimmed;
        source.IsDimmed = true;
        Sprite = source.Sprite;
        Count = count;
        Action = "";
        IsVisible = false; // aparece no primeiro movimento, já na posição certa
        RaiseAll();
    }

    /// <summary>Posição do ponteiro (coordenadas da MainView) e a ação ao soltar ali ("" = nenhuma).</summary>
    public void Move(Point pointer, string action)
    {
        if (_source is null) return;
        X = pointer.X + (_touch ? -40 : 14);
        Y = pointer.Y + (_touch ? -96 : 12);
        Action = action;
        IsVisible = true;
        RaiseAll();
    }

    /// <summary>Saiu da janela: some até voltar.</summary>
    public void Hide()
    {
        if (!IsVisible) return;
        IsVisible = false;
        Raise(nameof(IsVisible));
    }

    public void End()
    {
        if (_source is { } source)
            source.IsDimmed = _sourceWasDimmed;
        _source = null;
        IsVisible = false;
        Sprite = null;
        Action = "";
        RaiseAll();
    }

    /// <summary>Texto da ação para o modo e o destino (slot ocupado no modo mover = troca).</summary>
    public static string Describe(DropMode mode, SlotViewModel? target, SlotViewModel source, bool overArrow)
    {
        if (overArrow) return "Trocar de caixa…";
        if (target is null || target == source) return "";
        return mode switch
        {
            DropMode.Copy => "+ Copiar",
            DropMode.Overwrite => "Sobrescrever",
            _ => target.IsEmpty ? "Mover" : "⇄ Trocar",
        };
    }

    private void RaiseAll()
    {
        foreach (var p in (string[])[nameof(IsVisible), nameof(Sprite), nameof(X), nameof(Y), nameof(Count), nameof(HasCount), nameof(CountText), nameof(Action), nameof(HasAction)])
            Raise(p);
    }
}
