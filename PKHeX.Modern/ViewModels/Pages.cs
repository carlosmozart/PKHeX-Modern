using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using PKHeX.Core;
using PKHeX.Modern.Services;

namespace PKHeX.Modern.ViewModels;

/// <summary>
/// Uma pagina da barra lateral. Para adicionar uma nova funcao:
/// crie uma classe derivada, registre em <see cref="MainViewModel"/> e um DataTemplate em App.axaml.
/// </summary>
public abstract class PageViewModel : ViewModelBase
{
    public abstract string Title { get; }
    public abstract string Icon { get; }
    /// <summary>Atalho exibido na barra lateral (Ctrl+1, Ctrl+2...). Definido pelo MainViewModel.</summary>
    public string Shortcut { get; set; } = "";
    private string _groupHeader = "";
    /// <summary>Titulo do grupo na barra lateral, so na primeira pagina de cada grupo (definido pelo MainViewModel).</summary>
    public string GroupHeader
    {
        get => _groupHeader;
        set { if (Set(ref _groupHeader, value)) { Raise(nameof(IsGroupStart)); Raise(nameof(NavMargin)); } }
    }
    public bool IsGroupStart => _groupHeader.Length > 0;
    /// <summary>Espaco acima da linha para o titulo do grupo (a linha em si tem altura fixa).</summary>
    public Avalonia.Thickness NavMargin => IsGroupStart ? new(0, 26, 0, 0.5) : new(0, 0.5);
    /// <summary>Chamado quando a pagina altera o save (marca alteracoes nao exportadas).</summary>
    public Action? Changed { get; set; }
    /// <summary>Indica se a pagina se aplica ao save atual (ex.: jogos sem mochila).</summary>
    public virtual bool IsAvailable => true;
    public abstract void Load(SaveFile sav);
}

/// <summary>Base para paginas que exibem slots de Pokemon (caixas e equipe).</summary>
public abstract class SlotPageViewModel(Action<SlotViewModel> select) : PageViewModel
{
    public ObservableCollection<SlotViewModel> Slots { get; } = [];
    /// <summary>Chamado depois que os slots sao recarregados (a busca global reaplica o destaque).</summary>
    public Action? SlotsLoaded { get; set; }
    public RelayCommand SelectSlotCommand { get; } = new(p => { if (p is SlotViewModel s) select(s); });
    /// <summary>Colar/copiar varios sets Showdown (equipe e caixas; definidos pelo MainViewModel).</summary>
    public RelayCommand? PasteShowdownCommand { get; set; }
    public RelayCommand? CopyShowdownCommand { get; set; }
    public RelayCommand? ExportReportCommand { get; set; }
    /// <summary>Abre a vitrine da equipe (definido pelo MainViewModel).</summary>
    public RelayCommand? ShowcaseCommand { get; set; }
}

public sealed class BoxesPageViewModel : SlotPageViewModel
{
    private SaveFile? _sav;
    private bool _loading;

    public BoxesPageViewModel(Action<SlotViewModel> select) : base(select)
    {
        PreviousBoxCommand = new RelayCommand(() => CurrentBox--, () => CurrentBox > 0);
        NextBoxCommand = new RelayCommand(() => CurrentBox++, () => _sav is not null && CurrentBox < _sav.BoxCount - 1);
    }

    /// <summary>Faixa da equipe exibida acima das caixas (permite arrastar entre caixa e equipe).</summary>
    public PartyPageViewModel? Party { get; init; }
    public bool ShowParty => Party is { Slots.Count: > 0 };

    /// <summary>Ordenar (definido pelo MainViewModel, que guarda o desfazer): criterio e se vale para todas as caixas.</summary>
    public Action<CoreAdapter.BoxSortOption, bool>? Sort { get; set; }
    /// <summary>Criterios do menu "Ordenar" (cada um com "esta caixa" e "todas as caixas").</summary>
    public IReadOnlyList<SortOptionViewModel> SortOptions { get; private set; } = [];

    public override string Title => "Caixas";
    public override string Icon => "▦";
    public RelayCommand PreviousBoxCommand { get; }
    public RelayCommand NextBoxCommand { get; }
    public Func<string, string, string, System.Threading.Tasks.Task<string?>>? Prompt { get; set; }
    public bool CanRename => _sav is IBoxDetailName;
    public bool HasWallpapers => Wallpapers.Count > 0;
    public IReadOnlyList<string> Wallpapers { get; private set; } = [];
    public Avalonia.Media.Imaging.Bitmap? Wallpaper { get; private set; }
    private int _wallpaperIntensity;
    public int WallpaperIntensity { get => _wallpaperIntensity; set { if (Set(ref _wallpaperIntensity, value)) { Raise(nameof(WallpaperOpacity)); Raise(nameof(IsFullWallpaper)); } } }
    public double WallpaperOpacity => WallpaperIntensity switch { 1 => 0.10, 2 => 0, 3 => 1, _ => 0.25 };
    /// <summary>Papel de parede "Inteiro": nítido como no jogo, com os cartões translúcidos por cima (SlotGlass).</summary>
    public bool IsFullWallpaper => WallpaperIntensity == 3 && Wallpaper is not null;
    public RelayCommand RenameCommand => new(async () =>
    {
        if (_sav is not IBoxDetailName names || Prompt is null) return;
        var sav = _sav;
        int box = CurrentBox, max = CoreAdapter.GetBoxNameLength(sav);
        var name = await Prompt("Renomear caixa", $"Nome da caixa (até {max} caracteres):", BoxName);
        while (name is { } && name.Length > max && sav == _sav)
            name = await Prompt("Nome muito longo", $"Este jogo aceita até {max} caracteres. Escolha um nome menor:", name);
        if (name is null || sav != _sav || names.GetBoxName(box) == name) return;
        names.SetBoxName(box, name);
        RefreshNames();
        Changed?.Invoke();
    });
    public int WallpaperIndex
    {
        get => _sav is IBoxDetailWallpaper wp && HasWallpapers ? wp.GetBoxWallpaper(CurrentBox) : -1;
        set
        {
            if (_loading || _sav is not IBoxDetailWallpaper wp || value < 0 || value >= Wallpapers.Count || value == WallpaperIndex) return;
            wp.SetBoxWallpaper(CurrentBox, value);
            RefreshWallpaper();
            Changed?.Invoke();
        }
    }
    private void RefreshNames()
    {
        if (_sav is null) return;
        for (int i = 0; i < BoxTabs.Count; i++) BoxTabs[i].Name = CoreAdapter.GetBoxName(_sav, i);
        Raise(nameof(BoxName));
    }
    private void RefreshWallpaper()
    {
        var previous = Wallpaper;
        Wallpaper = _sav is not null && HasWallpapers ? SpriteService.GetBoxWallpaper(_sav, CurrentBox) : null;
        Raise(nameof(Wallpaper));
        Raise(nameof(IsFullWallpaper));
        Raise(nameof(WallpaperIndex));
        previous?.Dispose();
    }

    private int _currentBox;
    public int CurrentBox
    {
        get => _currentBox;
        set
        {
            if (_sav is null || value < 0 || value >= _sav.BoxCount || !Set(ref _currentBox, value))
                return;
            LoadBox();
        }
    }

    public string BoxName => _sav is null ? "" : CoreAdapter.GetBoxName(_sav, CurrentBox);

    /// <summary>Caixa aberta travada (nada muda nela ate destravar; ver Services.BoxLocks).</summary>
    public bool IsLocked => _sav is not null && BoxLocks.IsLocked(_sav, CurrentBox);
    public bool CanLock => _sav is not null && BoxLocks.CanLock(_sav);
    public string LockText => IsLocked ? "🔓  Destravar caixa" : "🔒  Travar caixa";
    public RelayCommand ToggleLockCommand => _toggleLock ??= new(() =>
    {
        if (_sav is null) return;
        bool locked = !IsLocked;
        BoxLocks.Set(_sav, CurrentBox, locked);
        RefreshLocks();
        Status?.Invoke(locked
            ? $"{BoxName} travada: nada muda nela (arrastar, excluir, aplicar, ordenar, colar, lote) até destravar."
            : $"{BoxName} destravada.");
    });
    private RelayCommand? _toggleLock;
    /// <summary>Mensagem na barra de status (definida pelo MainViewModel).</summary>
    public Action<string>? Status { get; set; }

    private void RefreshLocks()
    {
        if (_sav is null) return;
        for (int i = 0; i < BoxTabs.Count; i++)
            BoxTabs[i].IsLocked = BoxLocks.IsLocked(_sav, i);
        Raise(nameof(IsLocked));
        Raise(nameof(LockText));
        Raise(nameof(CanLock));
    }

    /// <summary>Abas com o nome de cada caixa (clique ou pare em cima durante o arraste para trocar).</summary>
    public ObservableCollection<BoxTabViewModel> BoxTabs { get; } = [];
    public int FilledCount => Slots.Count(s => !s.IsEmpty);
    public string BoxLabel => _sav is null ? "" : $"Caixa {CurrentBox + 1} de {_sav.BoxCount} · {FilledCount}/{Slots.Count} Pokémon";

    public override void Load(SaveFile sav)
    {
        _loading = true;
        _sav = sav;
        _currentBox = 0;
        Wallpapers = CoreAdapter.GetBoxWallpapers(sav);
        Raise(nameof(Wallpapers));
        Raise(nameof(HasWallpapers));
        Raise(nameof(CanRename));
        BoxTabs.Clear();
        for (int i = 0; i < sav.BoxCount; i++)
        {
            int box = i;
            BoxTabs.Add(new BoxTabViewModel(CoreAdapter.GetBoxName(sav, i), new RelayCommand(() => CurrentBox = box)));
        }
        SortOptions = [.. CoreAdapter.GetBoxSortOptions(sav).Select(o => new SortOptionViewModel(o.Name,
            new RelayCommand(() => Sort?.Invoke(o, false)), new RelayCommand(() => Sort?.Invoke(o, true))))];
        Raise(nameof(SortOptions));
        Raise(nameof(CurrentBox));
        LoadBox();
        Raise(nameof(ShowParty));
        _loading = false;
    }

    public void Reload() => LoadBox();

    /// <summary>Caixa aberta sem nenhum Pokémon: mostra a tela vazia por cima dos slots.</summary>
    public bool IsBoxEmpty => Slots.Count > 0 && Slots.All(s => s.IsEmpty);

    private void LoadBox()
    {
        if (_sav is null)
            return;
        bool wasLoading = _loading;
        _loading = true;
        Slots.Clear();
        for (int i = 0; i < _sav.BoxSlotCount; i++)
        {
            var s = new SlotViewModel(CurrentBox, i);
            s.Load(_sav);
            s.PropertyChanged += (_, e) => { if (e.PropertyName == nameof(SlotViewModel.IsEmpty)) Raise(nameof(IsBoxEmpty)); };
            Slots.Add(s);
        }
        Raise(nameof(BoxName));
        Raise(nameof(BoxLabel));
        Raise(nameof(IsBoxEmpty));
        RefreshNames();
        RefreshWallpaper();
        for (int i = 0; i < BoxTabs.Count; i++)
            BoxTabs[i].IsCurrent = i == CurrentBox;
        RefreshLocks();
        SlotsLoaded?.Invoke();
        PreviousBoxCommand.NotifyCanExecuteChanged();
        NextBoxCommand.NotifyCanExecuteChanged();
        _loading = wasLoading;
    }
}

public sealed partial class PartyPageViewModel(Action<SlotViewModel> select) : SlotPageViewModel(select)
{
    private SaveFile? _sav;
    public override string Title => "Equipe";
    public override string Icon => "◉";
    public override bool IsAvailable => _sav is null || CoreAdapter.GetPartyCount(_sav) > 0 || Daycares.All(_sav).Length > 0;

    public override void Load(SaveFile sav)
    {
        _sav = sav;
        Slots.Clear();
        for (int i = 0; i < CoreAdapter.GetPartyCount(sav); i++)
        {
            var s = new SlotViewModel(SlotViewModel.Party, i);
            s.Load(sav);
            Slots.Add(s);
        }
        SlotsLoaded?.Invoke();
        Raise(nameof(IsAvailable));
        RefreshDaycare();
    }
}

public sealed partial class TrainerPageViewModel : PageViewModel
{
    private SaveFile? _sav;
    public override string Title => "Treinador";
    public override string Icon => "♟";

    public override void Load(SaveFile sav)
    {
        _sav = sav;
        LoadExtras(sav);
        Raise(string.Empty);
    }

    public string Game => _sav is null ? "" : CoreAdapter.GetGameName(_sav);
    public string Generation => _sav is null ? "" : $"Geração {_sav.Generation}";
    public string Checksum => _sav is null ? "" : _sav.ChecksumsValid ? "Válidos" : _sav.ChecksumInfo;
    public int MaxMoney => _sav?.MaxMoney ?? 0;

    public string OT { get => _sav?.OT ?? ""; set { if (_sav is not null) { _sav.OT = value; Changed?.Invoke(); } Raise(); } }
    public decimal TID { get => _sav?.DisplayTID ?? 0; set { if (_sav is not null) { _sav.DisplayTID = (uint)value; Changed?.Invoke(); } Raise(); } }
    public decimal SID { get => _sav?.DisplaySID ?? 0; set { if (_sav is not null) { _sav.DisplaySID = (uint)value; Changed?.Invoke(); } Raise(); } }
    public decimal Money { get => _sav?.Money ?? 0; set { if (_sav is not null) { _sav.Money = (uint)Math.Min(value, MaxMoney); Changed?.Invoke(); } Raise(); } }
    public decimal Hours { get => _sav?.PlayedHours ?? 0; set { if (_sav is not null) { _sav.PlayedHours = (int)value; Changed?.Invoke(); } Raise(); } }
    public decimal Minutes { get => _sav?.PlayedMinutes ?? 0; set { if (_sav is not null) { _sav.PlayedMinutes = (int)value; Changed?.Invoke(); } Raise(); } }
    public decimal Seconds { get => _sav?.PlayedSeconds ?? 0; set { if (_sav is not null) { _sav.PlayedSeconds = (int)value; Changed?.Invoke(); } Raise(); } }
}

public sealed class BagPageViewModel(Action<string> status) : PageViewModel
{
    private SaveFile? _sav;
    private PlayerBag? _bag;

    public override string Title => "Mochila";
    public override string Icon => "🎒";
    public override bool IsAvailable => _bag is null || Pouches.Count > 0;

    public ObservableCollection<PouchViewModel> Pouches { get; } = [];

    private PouchViewModel? _selected;
    public PouchViewModel? SelectedPouch { get => _selected; set { if (Set(ref _selected, value)) { value?.Refresh(); FillCurrentPouchCommand.NotifyCanExecuteChanged(); } } }

    private bool _showEmptySlots;
    /// <summary>Mostra todas as vagas vazias do bolso (sem isso, so os itens que existem e algumas vagas para adicionar).</summary>
    public bool ShowEmptySlots
    {
        get => _showEmptySlots;
        set { if (!Set(ref _showEmptySlots, value)) return; foreach (var p in Pouches) p.ShowEmpty = value; }
    }

    public RelayCommand SaveCommand => new(Save);

    /// <summary>Itens mudados na tela que ainda nao foram gravados no save (Salvar aplica antes de gravar).</summary>
    public bool HasPendingChanges { get; private set; }

    public override void Load(SaveFile sav)
    {
        _sav = sav;
        HasPendingChanges = false;
        Pouches.Clear();
        try
        {
            _bag = CoreAdapter.GetBag(sav);
            FillPouches(sav, _bag);
        }
        catch (Exception ex)
        {
            status($"Mochila indisponível: {ex.Message}");
        }
        SelectedPouch = Pouches.FirstOrDefault();
        Raise(nameof(IsAvailable));
        foreach (var c in (RelayCommand[])[GiveAllTMsCommand, GiveAllBallsCommand, GiveAllMedicineCommand, GiveAllBerriesCommand, FillCurrentPouchCommand])
            c.NotifyCanExecuteChanged();
    }

    private void FillPouches(SaveFile sav, PlayerBag bag)
    {
        bool editable = CoreAdapter.IsBagItemIdEditable(sav);
        foreach (var p in bag.Pouches)
            Pouches.Add(new PouchViewModel(bag, p, editable, CoreAdapter.GetItemNames(sav), sav.Context, sav.Version, OnItemChanged) { ShowEmpty = _showEmptySlots });
    }

    /// <summary>Todos os TMs (e HMs/TRs do mesmo bolso), 99 de cada, sem apagar o que ja estava la.</summary>
    public RelayCommand GiveAllTMsCommand => _giveAllTMs ??= GiveAllCommand(InventoryType.TMHMs);
    private RelayCommand? _giveAllTMs;
    /// <summary>Conjuntos prontos (menu "Encher bolso"): Poke Balls, remedios e berries do jogo.</summary>
    public RelayCommand GiveAllBallsCommand => _giveAllBalls ??= GiveAllCommand(InventoryType.Balls);
    private RelayCommand? _giveAllBalls;
    public RelayCommand GiveAllMedicineCommand => _giveAllMedicine ??= GiveAllCommand(InventoryType.Medicine);
    private RelayCommand? _giveAllMedicine;
    public RelayCommand GiveAllBerriesCommand => _giveAllBerries ??= GiveAllCommand(InventoryType.Berries);
    private RelayCommand? _giveAllBerries;
    /// <summary>Todos os itens que cabem no bolso aberto (menos itens-chave, que mexem no andamento do jogo).</summary>
    public RelayCommand FillCurrentPouchCommand => _fillCurrent ??= new(
        () => { if (SelectedPouch is { } p) GiveAll(p.Type); },
        () => _bag is not null && SelectedPouch is { Type: not InventoryType.KeyItems });
    private RelayCommand? _fillCurrent;

    private RelayCommand GiveAllCommand(InventoryType type) => new(() => GiveAll(type), () => _bag?.Pouches.Any(p => p.Type == type) == true);

    /// <summary>
    /// Da todos os itens que o jogo aceita nos bolsos do tipo, 99 de cada (ou o maximo do jogo, se for menor: HMs,
    /// TMs da Gen 5+ e itens unicos ficam 1), em ordem numerica, sem apagar o que ja estava la.
    /// </summary>
    private void GiveAll(InventoryType type)
    {
        if (_sav is null || _bag is null)
            return;
        int given = 0, full = 0;
        foreach (var pouch in _bag.Pouches.Where(p => p.Type == type))
        {
            foreach (var id in pouch.GetAllItems())
            {
                if (id == 0 || !_bag.IsLegal(pouch.Type, id, 1))
                    continue;
                if (pouch.GiveItem(_bag, id, 1) < 0)
                    full++;
                else
                    given++;
            }
            foreach (var item in pouch.Items.Where(i => i.Index != 0))
                item.Count = _bag.Clamp(pouch.Type, item.Index, 99);
            pouch.SortByIndex();
        }
        var index = SelectedPouch is { } sel ? Pouches.IndexOf(sel) : 0;
        Pouches.Clear();
        FillPouches(_sav, _bag);
        SelectedPouch = Pouches.FirstOrDefault(p => p.Type == type) ?? Pouches.ElementAtOrDefault(index);
        OnItemChanged();
        if (type == InventoryType.TMHMs)
            status(full > 0
                ? $"{given} TM(s) na mochila, com 99 de cada; {full} não couberam (bolso cheio). Aplique a mochila e salve para manter."
                : $"{given} TM(s) na mochila, com 99 de cada. Aplique a mochila e salve para manter.");
        else
            status(full > 0
                ? $"{given} item(ns) no bolso, com 99 de cada; {full} não couberam (bolso cheio). Aplique a mochila e salve para manter."
                : $"{given} item(ns) no bolso, com 99 de cada. Aplique a mochila e salve para manter.");
    }

    /// <summary>Mexeu num item: o save passa a ter alteracoes pendentes (antes, o Salvar gravava sem a mochila).</summary>
    private void OnItemChanged()
    {
        HasPendingChanges = true;
        Changed?.Invoke();
    }

    /// <summary>Grava no save os itens mudados na tela, se houver (chamado pelo Salvar e ao trocar de aba).</summary>
    public bool ApplyPending()
    {
        if (!HasPendingChanges || _sav is null || _bag is null)
            return false;
        CoreAdapter.SaveBag(_sav, _bag);
        HasPendingChanges = false;
        return true;
    }

    private void Save()
    {
        if (_sav is null || _bag is null)
            return;
        CoreAdapter.SaveBag(_sav, _bag);
        HasPendingChanges = false;
        Changed?.Invoke();
        status("Mochila aplicada. Lembre-se de salvar.");
    }
}

public sealed class PouchViewModel : ViewModelBase
{
    /// <param name="names">Nomes na numeracao do save (Gen 1-3 tem numeracao propria).</param>
    public PouchViewModel(PlayerBag bag, InventoryPouch pouch, bool editable, IReadOnlyList<string> names, EntityContext context, GameVersion version = GameVersion.Any, Action? changed = null)
    {
        Type = pouch.Type;
        Name = GetName(pouch.Type);
        Options = [.. pouch.GetAllItems().ToArray().Prepend((ushort)0).Distinct()
            .Select(id => new ItemOption(id, id == 0 ? "(nenhum)" : id < names.Count && names[id].Length > 0 ? names[id] : $"Item #{id}", context, version))];
        Items = [.. pouch.Items.Select(it => new BagItemViewModel(bag, pouch.Type, it, Options, editable, changed))];
    }

    public InventoryType Type { get; }
    public string Name { get; }
    public IReadOnlyList<ItemOption> Options { get; }
    public IReadOnlyList<BagItemViewModel> Items { get; }

    /// <summary>Vagas vazias que continuam na tela quando as outras ficam escondidas (para adicionar itens).</summary>
    public const int FreeSlotsShown = 8;
    private bool _showEmpty;
    public bool ShowEmpty { get => _showEmpty; set { if (Set(ref _showEmpty, value)) Refresh(); } }
    /// <summary>O que a tela mostra: tudo, ou os itens que existem mais algumas vagas livres (antes, dezenas de "(nenhum)").</summary>
    public IReadOnlyList<BagItemViewModel> VisibleItems { get; private set; } = [];
    public string FreeText { get; private set; } = "";
    /// <summary>Recalcula a lista (ao trocar de bolso ou de modo; nao no meio da edicao, para a linha nao sair do lugar).</summary>
    public void Refresh()
    {
        var empty = Items.Where(i => i.IsEmptySlot).ToList();
        VisibleItems = _showEmpty ? Items : [.. Items.Where(i => !i.IsEmptySlot), .. empty.Take(FreeSlotsShown)];
        FreeText = empty.Count == 0 ? "Bolso cheio" : $"{empty.Count} vagas livres";
        Raise(nameof(VisibleItems)); Raise(nameof(FreeText));
    }

    /// <summary>Nome do bolso para a tela (o enum do Core tem nomes internos, como "KeyItems" e "TMHMs").</summary>
    public static string GetName(InventoryType type) => type switch
    {
        InventoryType.Items => "Itens",
        InventoryType.KeyItems => "Itens-chave",
        InventoryType.TMHMs => "TMs e HMs",
        InventoryType.Medicine => "Remédios",
        InventoryType.Berries => "Frutas",
        InventoryType.Balls => "Pokébolas",
        InventoryType.BattleItems => "Itens de batalha",
        InventoryType.MailItems => "Cartas",
        InventoryType.PCItems => "PC",
        InventoryType.FreeSpace => "Espaço livre",
        InventoryType.ZCrystals => "Z-Crystals",
        InventoryType.Candy => "Doces",
        InventoryType.Treasure => "Tesouros",
        InventoryType.Ingredients => "Ingredientes",
        InventoryType.MegaStones => "Mega Stones",
        _ => type.ToString(),
    };
}

/// <summary>Um item da lista da mochila; o icone e gerado so quando aparece na tela.</summary>
public sealed record ItemOption(int Id, string Name, EntityContext Context = EntityContext.None, GameVersion Version = GameVersion.Any)
{
    public Avalonia.Media.Imaging.Bitmap? Icon => SpriteService.GetItemSprite(Id, Context);
    /// <summary>Descricao e onde conseguir (AllGenWiki), quando houver.</summary>
    public string? Tip => Id == 0 ? null : ItemInfo.GetTooltip(Name, Context.Generation, Version);
    public override string ToString() => Name;
}

public sealed class BagItemViewModel(PlayerBag bag, InventoryType type, InventoryItem item, IReadOnlyList<ItemOption> options, bool editable, Action? changed = null) : ViewModelBase
{
    public IReadOnlyList<ItemOption> Options { get; } = options;
    public bool IsEditable { get; } = editable;

    public ItemOption? Selected
    {
        get => Options.FirstOrDefault(o => o.Id == item.Index);
        set
        {
            if (value is null || value.Id == item.Index)
                return;
            item.Index = value.Id;
            if (value.Id == 0)
                item.Count = 0;
            changed?.Invoke();
            Raise();
            Raise(nameof(Count));
            Raise(nameof(Icon));
            Raise(nameof(IsEmptySlot));
            Raise(nameof(Tip));
        }
    }

    public Avalonia.Media.Imaging.Bitmap? Icon => Selected?.Icon;
    /// <summary>Slot sem item (cartao apagado na grade da mochila).</summary>
    public bool IsEmptySlot => Selected is null or { Id: 0 };
    public string? Tip => Selected?.Tip;

    public decimal Count
    {
        get => item.Count;
        set
        {
            var count = item.Index == 0 ? 0 : bag.Clamp(type, item.Index, (int)value);
            if (count == item.Count)
                return;
            item.Count = count;
            changed?.Invoke();
            Raise();
        }
    }
}

/// <summary>Uma aba de caixa no topo da pagina Caixas.</summary>
public sealed class BoxTabViewModel(string name, RelayCommand go) : ViewModelBase
{
    private string _name = name;
    public string Name { get => _name; set => Set(ref _name, value); }
    public RelayCommand GoCommand { get; } = go;
    private bool _isCurrent;
    public bool IsCurrent { get => _isCurrent; set => Set(ref _isCurrent, value); }
    private bool _isLocked;
    /// <summary>Caixa travada (cadeado na aba).</summary>
    public bool IsLocked { get => _isLocked; set => Set(ref _isLocked, value); }
}

/// <summary>Um criterio do menu "Ordenar".</summary>
public sealed class SortOptionViewModel(string name, RelayCommand sortCurrent, RelayCommand? sortAll = null)
{
    public string Name { get; } = name;
    public RelayCommand SortCurrentCommand { get; } = sortCurrent;
    public RelayCommand? SortAllCommand { get; } = sortAll;
}
