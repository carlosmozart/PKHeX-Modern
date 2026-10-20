using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace PKHeX.Modern.ViewModels;

/// <summary>Uma linha da paleta de comandos: ação, página, caixa ou Pokémon do save.</summary>
public sealed class PaletteItem(string icon, string title, string detail, Action run, string keywords = "", string shortcut = "", Avalonia.Media.Imaging.Bitmap? sprite = null)
{
    public string Icon { get; } = icon;
    public string Title { get; } = title;
    /// <summary>Categoria ou onde está ("Ação", "Página", "Caixa 3 · slot 4").</summary>
    public string Detail { get; } = detail;
    public string Shortcut { get; } = shortcut;
    public bool HasShortcut => Shortcut.Length > 0;
    public Avalonia.Media.Imaging.Bitmap? Sprite { get; } = sprite;
    public bool HasSprite => Sprite is not null;
    public bool HasIcon => Sprite is null;
    /// <summary>Palavras extras para a busca (sinônimos, nome em inglês).</summary>
    public string Keywords { get; } = keywords;
    public Action Run { get; } = run;
}

/// <summary>
/// Paleta de comandos (Ctrl+K): digitar “legalizar”, “caixa 5”, “Pikachu” ou “exportar” e Enter executa. Junta os
/// itens fixos (ações e páginas, montados ao abrir) com os que dependem do texto (Pokémon do save, caixas).
/// A busca ignora maiúsculas e acentos; começo do título vale mais que começo de palavra, que vale mais que trecho.
/// </summary>
public sealed class CommandPaletteViewModel : ViewModelBase
{
    private const int MaxResults = 40;
    private readonly IReadOnlyList<PaletteItem> _fixed;
    private readonly Func<string, IEnumerable<PaletteItem>> _dynamic;
    private readonly Action _close;

    public CommandPaletteViewModel(IReadOnlyList<PaletteItem> fixedItems, Func<string, IEnumerable<PaletteItem>> dynamic, Action close)
    {
        _fixed = fixedItems;
        _dynamic = dynamic;
        _close = close;
        Refresh();
    }

    private string _query = "";
    public string Query { get => _query; set { if (Set(ref _query, value ?? "")) Refresh(); } }

    public IReadOnlyList<PaletteItem> Results { get; private set; } = [];
    public bool HasResults => Results.Count > 0;
    public bool IsEmpty => Results.Count == 0;

    private int _selectedIndex;
    public int SelectedIndex { get => _selectedIndex; set => Set(ref _selectedIndex, value); }

    /// <summary>Seta para cima/baixo, circular.</summary>
    public void Move(int delta)
    {
        if (Results.Count == 0) return;
        SelectedIndex = ((SelectedIndex < 0 ? 0 : SelectedIndex) + delta + Results.Count) % Results.Count;
    }

    /// <summary>Enter: fecha a paleta e executa o item selecionado (ou o primeiro).</summary>
    public void RunSelected()
    {
        if (Results.Count == 0) return;
        Run(Results[Math.Clamp(SelectedIndex, 0, Results.Count - 1)]);
    }

    public void Run(PaletteItem item)
    {
        _close();
        item.Run();
    }

    public RelayCommand RunCommand => new(p => { if (p is PaletteItem item) Run(item); });
    public RelayCommand CloseCommand => new(() => _close());

    private void Refresh()
    {
        var query = Normalize(Query.Trim());
        IEnumerable<PaletteItem> items;
        if (query.Length == 0)
        {
            items = _fixed;
        }
        else
        {
            var all = _fixed.Concat(_dynamic(Query.Trim()));
            items = all.Select(i => (Item: i, Score: Score(i, query)))
                .Where(x => x.Score > 0)
                .OrderByDescending(x => x.Score)
                .Select(x => x.Item);
        }
        Results = [.. items.Take(MaxResults)];
        SelectedIndex = Results.Count > 0 ? 0 : -1;
        Raise(nameof(Results));
        Raise(nameof(HasResults));
        Raise(nameof(IsEmpty));
    }

    /// <summary>Relevância de um item para a busca (0 = não combina). Todas as palavras digitadas precisam aparecer.</summary>
    public static int Score(PaletteItem item, string normalizedQuery)
    {
        var title = Normalize(item.Title);
        var haystack = title + " " + Normalize(item.Keywords) + " " + Normalize(item.Detail);
        var words = normalizedQuery.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (words.Any(w => !haystack.Contains(w, StringComparison.Ordinal)))
            return 0;
        if (title.StartsWith(normalizedQuery, StringComparison.Ordinal)) return 400 - Math.Min(title.Length, 99);
        if ((" " + title).Contains(" " + normalizedQuery, StringComparison.Ordinal)) return 300 - Math.Min(title.Length, 99);
        if (title.Contains(normalizedQuery, StringComparison.Ordinal)) return 200 - Math.Min(title.Length, 99);
        return 100;
    }

    /// <summary>Minúsculas e sem acentos ("Pokédex" → "pokedex").</summary>
    public static string Normalize(string text)
    {
        var d = text.ToLowerInvariant().Normalize(NormalizationForm.FormD);
        var sb = new StringBuilder(d.Length);
        foreach (var c in d)
            if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                sb.Append(c);
        return sb.ToString().Normalize(NormalizationForm.FormC);
    }
}
