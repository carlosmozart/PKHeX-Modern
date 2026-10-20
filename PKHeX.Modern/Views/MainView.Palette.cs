using System.Collections.Generic;
using Avalonia.Interactivity;
using PKHeX.Modern.ViewModels;

namespace PKHeX.Modern.Views;

public sealed partial class MainView
{
    /// <summary>Ações da paleta que precisam da View (seletores de arquivo). O MainViewModel filtra pelo save aberto.</summary>
    private IEnumerable<PaletteItem> PaletteActions()
    {
        const string action = "Ação";
        string? Key(string shortcut) => App.ShowShortcuts ? shortcut : "";
        yield return new PaletteItem("📂", "Abrir save", action, () => OnOpen(this, new RoutedEventArgs()), "open arquivo " + Services.Loc.T("Abrir save"), Key("Ctrl+O")!);
        yield return new PaletteItem("💾", "Salvar", action, () => OnQuickSave(this, new RoutedEventArgs()), "save gravar " + Services.Loc.T("Salvar"), Key("Ctrl+S")!);
        yield return new PaletteItem("💾", "Salvar como...", action, () => OnExport(this, new RoutedEventArgs()), "save as exportar save " + Services.Loc.T("Salvar como..."), Key("Ctrl+E")!);
        yield return new PaletteItem("⭳", "Importar Pokémon ou evento", action, () => OnImportEntity(this, new RoutedEventArgs()), "import pkm arquivo wc " + Services.Loc.T("Importar Pokémon ou evento"));
        if (VM.CanExportEntity)
            yield return new PaletteItem("⭱", "Exportar PKM", action, () => OnExportEntity(this, new RoutedEventArgs()), "export arquivo " + Services.Loc.T("Exportar PKM"));
    }
}
