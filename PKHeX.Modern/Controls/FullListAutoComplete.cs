using System;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.VisualTree;

namespace PKHeX.Modern.Controls;

/// <summary>
/// Caixa de busca que mostra a lista inteira ao tocar ou clicar e vai filtrando conforme se digita (espécie e item no
/// editor). O AutoCompleteBox comum só abre a lista depois da primeira letra e, com o nome atual já escrito, mostraria
/// só ele. Ao entrar, o texto fica selecionado: digitar substitui o nome e passa a filtrar ("contém", sem diferenciar
/// maiúsculas).
/// </summary>
public class FullListAutoComplete : AutoCompleteBox
{
    protected override Type StyleKeyOverride => typeof(AutoCompleteBox);

    /// <summary>Enquanto true, o filtro deixa passar tudo (lista inteira); a primeira tecla digitada volta a filtrar.</summary>
    private bool _showAll;

    public FullListAutoComplete()
    {
        MinimumPrefixLength = 0;
        FilterMode = AutoCompleteFilterMode.Custom;
        TextFilter = (search, text) => _showAll || string.IsNullOrEmpty(search)
            || (text?.Contains(search, StringComparison.OrdinalIgnoreCase) ?? false);
        // Antes da caixa de texto interna tratar a tecla: daqui em diante o que foi digitado filtra
        AddHandler(TextInputEvent, (_, _) => _showAll = false, RoutingStrategies.Tunnel);
        AddHandler(KeyDownEvent, (_, e) => { if (e.Key is Key.Back or Key.Delete) _showAll = false; }, RoutingStrategies.Tunnel);
        AddHandler(PointerReleasedEvent, (_, _) => OpenFullList(), RoutingStrategies.Tunnel, handledEventsToo: true);
    }

    protected override void OnGotFocus(GotFocusEventArgs e)
    {
        base.OnGotFocus(e);
        OpenFullList();
    }

    /// <summary>Mostra todos os itens (com o texto atual selecionado, para a próxima tecla substituir).</summary>
    public void OpenFullList()
    {
        if (!IsEnabled || IsDropDownOpen)
            return;
        _showAll = true;
        PopulateComplete();
        IsDropDownOpen = true;
        // Depois do clique (que põe o cursor no fim): o texto fica selecionado e a próxima tecla substitui o nome
        if (this.FindDescendantOfType<TextBox>() is { } box)
            Avalonia.Threading.Dispatcher.UIThread.Post(box.SelectAll, Avalonia.Threading.DispatcherPriority.Input);
    }
}
