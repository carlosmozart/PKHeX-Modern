using System;
using System.Collections.Generic;
using System.Linq;
using PKHeX.Modern.Services;

namespace PKHeX.Modern.ViewModels;

/// <summary>Paleta de comandos (Ctrl+K): monta os itens a partir do estado atual do app.</summary>
public sealed partial class MainViewModel
{
    private CommandPaletteViewModel? _palette;
    public CommandPaletteViewModel? Palette { get => _palette; private set { if (Set(ref _palette, value)) Raise(nameof(HasPalette)); } }
    public bool HasPalette => Palette is not null;

    /// <summary>Ações que dependem da View (seletores de arquivo): Abrir, Salvar, Salvar como, Importar e Exportar PKM.</summary>
    public Func<IEnumerable<PaletteItem>>? PaletteViewActions { get; set; }

    public RelayCommand OpenPaletteCommand => _openPalette ??= new RelayCommand(OpenPalette);
    private RelayCommand? _openPalette;

    public void OpenPalette()
    {
        IsHelpOpen = false;
        Palette = new CommandPaletteViewModel(BuildPaletteItems(), PaletteMatches, ClosePalette);
    }

    public void ClosePalette() => Palette = null;

    private static PaletteItem Item(string icon, string title, string detail, Action run, string keywords = "", string shortcut = "")
        // O nome traduzido entra nas palavras-chave: na interface em inglês, a busca acha pelo texto que aparece na tela.
        => new(icon, title, detail, run, $"{keywords} {Loc.T(title)}", shortcut);

    /// <summary>Ações e páginas disponíveis agora (a ordem é a da lista sem busca).</summary>
    private List<PaletteItem> BuildPaletteItems()
    {
        var items = new List<PaletteItem>();
        const string action = "Ação";
        var view = PaletteViewActions?.Invoke().ToList() ?? [];
        // Abrir save sempre; o resto da View só com save aberto
        items.AddRange(view.Where(v => HasSave || v.Title == "Abrir save"));
        if (HasSave)
        {
            if (Editor is { ReadOnly: false } editor && editor.LegalizeCommand.CanExecute(null))
                items.Add(Item("✨", "Legalizar o Pokémon selecionado", action, () => editor.LegalizeCommand.Execute(null), "legal consertar corrigir fix"));
            if (CheckLegalityCommand.CanExecute(null))
                items.Add(Item("✓", "Verificar legalidade do save", action, () => CheckLegalityCommand.Execute(null), "legal check ilegais"));
            if (CreateCommand.CanExecute(null) && !Hardcore)
                items.Add(Item("+", "Criar PKM", action, () => CreateCommand.Execute(null), "novo pokemon create new"));
            if (DeleteCommand.CanExecute(null) && !Hardcore)
                items.Add(Item("🗑", "Excluir o Pokémon selecionado", action, () => DeleteCommand.Execute(null), "apagar remover delete", "Del"));
            if (UndoCommand.CanExecute(null))
                items.Add(Item("↶", "Desfazer", action, () => UndoCommand.Execute(null), "undo voltar", "Ctrl+Z"));
            if (RedoCommand.CanExecute(null))
                items.Add(Item("↷", "Refazer", action, () => RedoCommand.Execute(null), "redo", "Ctrl+Y"));
            if (Party.Slots.Any(s => !s.IsEmpty))
                items.Add(Item("🖼", "Vitrine da equipe", action, OpenShowcase, "imagem compartilhar print showcase"));
            items.Add(Item("⌕", "Buscar no save", action, () => { CurrentPage = Boxes; FocusSearch?.Invoke(); }, "procurar pesquisar find", "Ctrl+F"));
        }
        items.Add(Item("◐", "Alternar tema claro/escuro", action, () => ToggleThemeCommand.Execute(null), "dark light escuro claro theme"));
        items.Add(Item("?", "Ajuda", action, OpenHelp, "help manual", "F1"));
        if (HasSave)
        {
            const string page = "Página";
            items.Add(Item(Home.Icon, "Ir para Início", page, () => GoHomeCommand.Execute(null), "home inicio", "Ctrl+0"));
            foreach (var p in Pages)
            {
                var target = p;
                items.Add(Item(p.Icon, $"Ir para {p.Title}", page, () => CurrentPage = target, $"{p.Title} {Loc.T(p.Title)}", p.Shortcut));
            }
        }
        return items;
    }

    /// <summary>Itens que dependem do texto: caixas (número ou nome) e Pokémon do save (espécie ou apelido).</summary>
    private IEnumerable<PaletteItem> PaletteMatches(string query)
    {
        if (_sav is not { } sav)
            yield break;
        var norm = CommandPaletteViewModel.Normalize(query);
        // "caixa 5", "box 5" ou só "5"
        var digits = new string(norm.Where(char.IsDigit).ToArray());
        if (digits.Length is > 0 and <= 3 && int.TryParse(digits, out var number) && number >= 1 && number <= sav.BoxCount)
        {
            int box = number - 1;
            yield return new PaletteItem("▦", $"Ir para a caixa {number}", CoreAdapter.GetBoxName(sav, box), () => GoToBox(box), $"caixa box {number}");
        }
        if (norm.Length >= 2)
        {
            for (int box = 0; box < sav.BoxCount; box++)
            {
                var name = CoreAdapter.GetBoxName(sav, box);
                if (CommandPaletteViewModel.Normalize(name).Contains(norm, StringComparison.Ordinal))
                {
                    int target = box;
                    yield return new PaletteItem("▦", $"Ir para a caixa {name}", $"Caixa {box + 1}", () => GoToBox(target), $"caixa box {name}");
                }
            }
            _searchIndex ??= EntitySearch.ReadAll(sav);
            int found = 0;
            foreach (var e in _searchIndex)
            {
                var pk = e.Pkm;
                var species = pk.IsEgg ? "Ovo" : CoreAdapter.SpeciesNames[pk.Species];
                var nick = pk.IsNicknamed && !pk.IsEgg ? pk.Nickname : "";
                if (!CommandPaletteViewModel.Normalize(species).Contains(norm, StringComparison.Ordinal)
                    && !(nick.Length > 0 && CommandPaletteViewModel.Normalize(nick).Contains(norm, StringComparison.Ordinal)))
                    continue;
                var title = nick.Length > 0 ? $"{nick} ({species})" : species;
                var where = e.Box < 0 ? $"Equipe · posição {e.Slot + 1}" : $"{CoreAdapter.GetBoxName(sav, e.Box)} · slot {e.Slot + 1}";
                var detail = pk.IsEgg ? where : $"Nv. {pk.CurrentLevel}{(pk.IsShiny ? " · ★" : "")} · {where}";
                var sprite = pk.IsEgg ? null : SpriteService.GetSpeciesSprite(pk.Species, pk.IsShiny, pk.Form, pk.Gender, pk.Context);
                var (b, s) = (e.Box, e.Slot);
                yield return new PaletteItem("●", title, detail, () => _ = GoToSlotAsync(b, s), "pokemon", "", sprite);
                if (++found >= 20) break;
            }
        }
    }

    /// <summary>Foco na busca global (definido pela View).</summary>
    public Action? FocusSearch { get; set; }

    private void GoToBox(int box)
    {
        CurrentPage = Boxes;
        Boxes.CurrentBox = box;
    }
}
