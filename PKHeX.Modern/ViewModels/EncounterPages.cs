using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using PKHeX.Core;
using PKHeX.Modern.Services;

namespace PKHeX.Modern.ViewModels;

/// <summary>
/// Cartao de um encontro ou Mystery Gift (compartilhado pelas duas paginas). Em "Ver todos", um cartao junta as variacoes
/// da mesma especie, forma, local, tipo e versao (<paramref name="variants"/>): o primeiro e o que "Usar" gera.
/// </summary>
public sealed class EncounterCardViewModel(IEncounterInfo enc, IReadOnlyList<IEncounterInfo>? variants = null) : ViewModelBase
{
    public IEncounterInfo Encounter { get; } = enc;
    /// <summary>Encontros que o cartao representa (so ele mesmo fora do "Ver todos").</summary>
    public IReadOnlyList<IEncounterInfo> Variants { get; } = variants ?? [enc];
    public bool HasVariants => Variants.Count > 1;

    public string Title => EncounterDatabase.GetSpeciesFormName(Encounter.Species, Encounter.Form, Encounter.Context);
    /// <summary>Tipo do encontro (ex.: "Wild Encounter") ou titulo do card do evento.</summary>
    public string Kind => Encounter switch
    {
        MysteryGift g when !string.IsNullOrWhiteSpace(g.CardTitle) => g.CardTitle.Replace('　', ' ').Trim(),
        IEncounterable e => e.LongName,
        _ => "",
    };
    public string Level => LevelText(Variants.Min(v => v.LevelMin), Variants.Max(v => v.LevelMax));
    internal static string LevelText(byte min, byte max) => min == max ? $"Nv. {min}" : $"Nv. {min}–{max}";
    public string Location { get; } = LocationOf(enc);
    public bool HasLocation => !string.IsNullOrWhiteSpace(Location);
    /// <summary>Local no painel de detalhes ("—" quando nao ha).</summary>
    public string DisplayLocation => HasLocation ? Location : "—";

    /// <summary>
    /// Local do encontro. Ovo de criacao fica sem local: o PKHeX guarda nele so o lugar padrao onde o ovo choca (no Black,
    /// Skyarrow Bridge), que enganava no cartao e no filtro de local. Ovos de presente mantem o local.
    /// </summary>
    public static string LocationOf(IEncounterInfo enc) => enc is IEncounterEgg ? "" : EncounterDatabase.GetLocationName(enc);
    public string Game => $"{EncounterDatabase.GetVersionName(Encounter.Version)} · Gen {Encounter.Generation}";
    public bool IsEgg => Encounter.IsEgg;
    public bool IsShiny => Encounter.IsShiny;
    public string Details => EncounterDatabase.GetDetails(Encounter);
    public string SearchText => $"{Title} {Kind} {Location} {Game}";
    /// <summary>Selvagem, Estático, Troca, Ovo, Raid, Evento... (filtro por tipo).</summary>
    public string Category { get; } = EncounterDatabase.GetCategory(enc);
    public string VersionName => EncounterDatabase.GetVersionName(Encounter.Version);
    public bool IsHomeGift => EncounterDatabase.IsHomeGift(Encounter);
    public bool IsShinyLocked => Encounter.Shiny == Shiny.Never;
    private IReadOnlyList<ushort>? _moves;
    /// <summary>Golpes que ja vem com o encontro (filtro por golpe e painel de detalhes).</summary>
    public IReadOnlyList<ushort> FixedMoves => _moves ??= EncounterDatabase.GetFixedMoves(Encounter);

    private bool _isSelected;
    public bool IsSelected { get => _isSelected; set => Set(ref _isSelected, value); }

    private Bitmap? _sprite;
    private bool _spriteLoaded;
    /// <summary>Sprite gerado so quando o cartao aparece.</summary>
    public Bitmap? Sprite
    {
        get
        {
            if (!_spriteLoaded)
            {
                _spriteLoaded = true;
                try { _sprite = SpriteService.GetSprite(Encounter); } catch { _sprite = null; }
            }
            return _sprite;
        }
    }
}

/// <summary>Base das paginas de banco: lista de cartoes com "mostrar mais" e acao "Usar".</summary>
public abstract class EncounterListPageViewModel : PageViewModel
{
    private const int PageSize = 90;
    private IReadOnlyList<EncounterCardViewModel> _matches = [];

    protected EncounterListPageViewModel(Action<IEncounterInfo> use)
    {
        UseCommand = new RelayCommand(p => { if (p is EncounterCardViewModel c) use(c.Encounter); });
        ShowMoreCommand = new RelayCommand(() => ShowCount(Results.Count + PageSize));
        SelectCommand = new RelayCommand(p => Selected = p as EncounterCardViewModel);
        CloseDetailCommand = new RelayCommand(() => Selected = null);
        ClearFiltersCommand = new RelayCommand(() => { _kind = AllKinds; _version = AllVersions; _location = AllLocations; _move = ""; _name = ""; RaiseFilters(); ApplyFilters(); });
    }

    // Selecao e painel de detalhes
    public RelayCommand SelectCommand { get; }
    public RelayCommand CloseDetailCommand { get; }
    private EncounterCardViewModel? _selected;
    /// <summary>Cartao clicado: abre o painel de detalhes a direita.</summary>
    public EncounterCardViewModel? Selected
    {
        get => _selected;
        set
        {
            if (_selected == value)
                return;
            if (_selected is not null)
                _selected.IsSelected = false;
            _selected = value;
            if (value is not null)
                value.IsSelected = true;
            Detail = value is null ? null : new EncounterDetailViewModel(value);
            Raise();
            Raise(nameof(Detail));
            Raise(nameof(HasDetail));
        }
    }
    public EncounterDetailViewModel? Detail { get; private set; }
    public bool HasDetail => Detail is not null;

    // Filtros (tipo, versao, local, golpe, nome) sobre o resultado da busca
    protected const string AllKinds = "Todos os encontros";
    protected const string AllVersions = "Todas as versões";
    protected const string AllLocations = "Todos os locais";
    private IReadOnlyList<EncounterCardViewModel> _source = [];
    public RelayCommand ClearFiltersCommand { get; }
    public IReadOnlyList<string> KindOptions { get; private set; } = [AllKinds];
    public IReadOnlyList<string> VersionOptions { get; private set; } = [AllVersions];
    public IReadOnlyList<string> LocationOptions { get; private set; } = [AllLocations];
    /// <summary>O filtro de local so aparece quando ha mais de um local para escolher.</summary>
    public bool HasLocationOptions => LocationOptions.Count > 2;
    public IReadOnlyList<string> MoveNames => CoreAdapter.MoveNames;
    private string _kind = AllKinds, _version = AllVersions, _location = AllLocations, _move = "", _name = "";
    public string KindFilter { get => _kind; set { if (Set(ref _kind, value ?? AllKinds)) ApplyFilters(); } }
    public string VersionFilter { get => _version; set { if (Set(ref _version, value ?? AllVersions)) ApplyFilters(); } }
    public string LocationFilter { get => _location; set { if (Set(ref _location, value ?? AllLocations)) ApplyFilters(); } }
    /// <summary>So cartoes cujo nome (especie e forma) contem este texto.</summary>
    public string NameFilter { get => _name; set { if (Set(ref _name, value ?? "")) ApplyFilters(); } }
    /// <summary>So encontros que ja vem com este golpe (texto parcial e ignorado ate virar um golpe).</summary>
    public string MoveFilter
    {
        get => _move;
        set
        {
            if (!Set(ref _move, value ?? ""))
                return;
            if (_move.Length == 0 || CoreAdapter.FindIndex(CoreAdapter.MoveNames, _move) > 0)
                ApplyFilters();
        }
    }
    public bool HasFilters => _source.Count > 0;
    public bool IsFiltered => _kind != AllKinds || _version != AllVersions || _location != AllLocations || _move.Length > 0 || _name.Trim().Length > 0;

    private void RaiseFilters()
    {
        foreach (var p in (string[])[nameof(KindFilter), nameof(VersionFilter), nameof(LocationFilter), nameof(MoveFilter), nameof(NameFilter)])
            Raise(p);
    }

    /// <summary>Novo resultado de busca: refaz as opcoes dos filtros e aplica.</summary>
    protected void SetSource(IReadOnlyList<EncounterCardViewModel> cards)
    {
        _source = cards;
        KindOptions = [AllKinds, .. cards.Select(c => c.Category).Distinct().Order()];
        VersionOptions = [AllVersions, .. cards.Select(c => c.VersionName).Where(v => v.Length > 0).Distinct().Order()];
        LocationOptions = [AllLocations, .. cards.Select(c => c.Location).Where(l => !string.IsNullOrWhiteSpace(l)).Distinct().Order(StringComparer.CurrentCultureIgnoreCase)];
        if (!KindOptions.Contains(_kind)) _kind = AllKinds;
        if (!VersionOptions.Contains(_version)) _version = AllVersions;
        if (!LocationOptions.Contains(_location)) _location = AllLocations;
        foreach (var p in (string[])[nameof(KindOptions), nameof(VersionOptions), nameof(LocationOptions), nameof(HasLocationOptions), nameof(HasFilters)])
            Raise(p);
        RaiseFilters();
        ApplyFilters();
    }

    /// <summary>Filtro extra da pagina (ex.: texto da busca de eventos).</summary>
    protected virtual bool Matches(EncounterCardViewModel card) => true;

    /// <summary>Resumo depois de filtrar (cada pagina escreve o seu).</summary>
    protected virtual void OnFiltered(int shown, int total) { }

    protected void ApplyFilters()
    {
        var move = _move.Length == 0 ? 0 : CoreAdapter.FindIndex(CoreAdapter.MoveNames, _move);
        var name = _name.Trim();
        var list = _source.Where(c => (_kind == AllKinds || c.Category == _kind)
                                      && (_version == AllVersions || c.VersionName == _version)
                                      && (_location == AllLocations || c.Location == _location)
                                      && (name.Length == 0 || c.Title.Contains(name, StringComparison.CurrentCultureIgnoreCase))
                                      && (move <= 0 || c.FixedMoves.Contains((ushort)move))
                                      && Matches(c)).ToList();
        SetMatches(list);
        if (Selected is { } sel && !list.Contains(sel))
            Selected = null;
        Raise(nameof(IsFiltered));
        OnFiltered(list.Count, _source.Count);
    }

    protected SaveFile? Sav { get; private set; }

    public RelayCommand UseCommand { get; }
    public RelayCommand ShowMoreCommand { get; }
    public ObservableCollection<EncounterCardViewModel> Results { get; } = [];

    private bool _onlyThisGame = true;
    /// <summary>"Só deste jogo": o filtro "unavailable species" do PKHeX.</summary>
    public bool OnlyThisGame { get => _onlyThisGame; set { if (Set(ref _onlyThisGame, value)) OnOnlyThisGameChanged(); } }

    private bool _isBusy;
    public bool IsBusy { get => _isBusy; protected set => Set(ref _isBusy, value); }

    private string _summary = "";
    public string Summary { get => _summary; protected set => Set(ref _summary, value); }

    public bool HasMore => Results.Count < _matches.Count;

    public override void Load(SaveFile sav)
    {
        Sav = sav;
        Selected = null;
        _source = [];
        SetMatches([]);
        Raise(nameof(HasFilters));
        Summary = "";
    }

    protected virtual void OnOnlyThisGameChanged() { }

    protected void SetMatches(IReadOnlyList<EncounterCardViewModel> matches)
    {
        _matches = matches;
        Results.Clear();
        ShowCount(PageSize);
    }

    private void ShowCount(int count)
    {
        for (int i = Results.Count; i < Math.Min(count, _matches.Count); i++)
            Results.Add(_matches[i]);
        Raise(nameof(HasMore));
    }
}

/// <summary>Banco de encontros: todos os jeitos de obter uma especie (selvagem, estatico, troca, ovo, evento).</summary>
public sealed class EncounterDbViewModel : EncounterListPageViewModel
{
    private CancellationTokenSource? _cts;

    public EncounterDbViewModel(Action<IEncounterInfo> use) : base(use)
    {
        SearchCommand = new RelayCommand(() => _ = SearchAsync());
        ShowAllCommand = new RelayCommand(() => _ = ShowAllAsync());
    }

    public override string Title => "Encontros";
    public override string Icon => "🌿";

    public RelayCommand SearchCommand { get; }
    /// <summary>"Ver todos": todos os encontros do jogo do save, de todas as especies.</summary>
    public RelayCommand ShowAllCommand { get; }
    public IReadOnlyList<string> SpeciesNames => CoreAdapter.SpeciesNames;
    private string _speciesName = "";
    private bool _onlyGame;
    private bool _showingAll;
    private int _allSpecies;
    /// <summary>A lista atual e a do "Ver todos" (o filtro por nome aparece so nela).</summary>
    public bool IsShowingAll { get => _showingAll; private set => Set(ref _showingAll, value); }

    public override void Load(SaveFile sav)
    {
        base.Load(sav);
        IsShowingAll = false;
        Summary = "Escolha uma espécie ou clique em “Ver todos” para listar tudo o que aparece neste jogo.";
    }

    protected override void OnFiltered(int shown, int total)
    {
        if (IsShowingAll)
        {
            Summary = total == 0 ? Loc.T("Nenhum encontro neste jogo.")
                : Loc.T(shown == total ? $"{total} cartão(ões) de {_allSpecies} espécie(s) deste jogo." : $"{shown} de {total} cartão(ões) deste jogo com os filtros.")
                  + Loc.T(" Clique num cartão para ver os detalhes; Usar leva ao editor.");
            return;
        }
        // As duas frases sao traduzidas em separado: juntas, o modelo "{0} encontro(s) de {1}." engolia a segunda como
        // nome da especie e ela ficava em portugues.
        Summary = total == 0
            ? $"Nenhum encontro de {_speciesName}" + (_onlyGame ? " neste jogo. Desmarque “Só deste jogo” para ver outros jogos." : ".")
            : Loc.T(shown == total ? $"{total} encontro(s) de {_speciesName}." : $"{shown} de {total} encontro(s) de {_speciesName} com os filtros.")
              + Loc.T(" Clique num cartão para ver os detalhes; Usar leva ao editor.");
    }

    private string? _species;
    /// <summary>Nome da especie digitado/escolhido (busca automatica ao escolher).</summary>
    public string? Species
    {
        get => _species;
        set
        {
            if (Set(ref _species, value) && GetSpeciesId() > 0)
                _ = SearchAsync();
        }
    }

    protected override void OnOnlyThisGameChanged()
    {
        // O "Ver todos" ja e so deste jogo; a opcao vale para a busca por especie
        if (!IsShowingAll && GetSpeciesId() > 0)
            _ = SearchAsync();
    }

    /// <summary>
    /// Lista todos os encontros do jogo do save (<see cref="EncounterDatabase.SearchAllEncounters"/>), um cartao por
    /// especie, forma, local, tipo e versao: no BD/SP e no S/V, cada especie tem dezenas de "slots" no mesmo local.
    /// </summary>
    public async Task ShowAllAsync()
    {
        if (Sav is not { } sav)
            return;
        _cts?.Cancel();
        var cts = _cts = new CancellationTokenSource();
        IsBusy = true;
        Summary = "Buscando todos os encontros deste jogo...";
        try
        {
            var cards = await Task.Run(() => GroupCards(EncounterDatabase.SearchAllEncounters(sav, cts.Token)), cts.Token);
            if (cts.IsCancellationRequested)
                return;
            _allSpecies = cards.Select(c => c.Encounter.Species).Distinct().Count();
            _species = "";
            Raise(nameof(Species));
            IsShowingAll = true;
            Selected = null;
            SetSource(cards);
        }
        catch (OperationCanceledException)
        {
            // substituida por uma busca mais nova
        }
        catch (Exception ex)
        {
            Summary = $"Erro na busca: {ex.Message}";
        }
        finally
        {
            if (_cts == cts)
                IsBusy = false;
        }
    }

    /// <summary>Junta as variacoes (mesma especie, forma, local, tipo e versao) num cartao so, na ordem em que aparecem.</summary>
    public static List<EncounterCardViewModel> GroupCards(IReadOnlyList<IEncounterInfo> encounters) =>
    [
        .. encounters
            .GroupBy(e => (e.Species, e.Form, Location: EncounterCardViewModel.LocationOf(e), Kind: EncounterDatabase.GetCategory(e), e.Version))
            .Select(g => new EncounterCardViewModel(g.First(), [.. g])),
    ];

    private ushort GetSpeciesId()
    {
        if (string.IsNullOrWhiteSpace(_species))
            return 0;
        var names = CoreAdapter.SpeciesNames;
        for (int i = 1; i < names.Count; i++)
        {
            if (string.Equals(names[i], _species.Trim(), StringComparison.OrdinalIgnoreCase))
                return (ushort)i;
        }
        return 0;
    }

    public async Task SearchAsync()
    {
        if (Sav is not { } sav)
            return;
        var species = GetSpeciesId();
        if (species == 0)
        {
            Summary = "Escolha uma espécie para buscar.";
            return;
        }
        _cts?.Cancel();
        var cts = _cts = new CancellationTokenSource();
        IsBusy = true;
        Summary = $"Buscando encontros de {CoreAdapter.SpeciesNames[species]}...";
        try
        {
            bool only = OnlyThisGame;
            var found = await Task.Run(() => EncounterDatabase.SearchEncounters(sav, species, only, cts.Token), cts.Token);
            if (cts.IsCancellationRequested)
                return;
            _speciesName = CoreAdapter.SpeciesNames[species];
            _onlyGame = only;
            IsShowingAll = false;
            Selected = null;
            SetSource([.. found.Select(e => new EncounterCardViewModel(e))]);
        }
        catch (OperationCanceledException)
        {
            // substituida por uma busca mais nova
        }
        catch (Exception ex)
        {
            Summary = $"Erro na busca: {ex.Message}";
        }
        finally
        {
            if (_cts == cts)
                IsBusy = false;
        }
    }
}

/// <summary>Banco de Mystery Gift (eventos), com busca por texto.</summary>
public sealed class GiftDbViewModel(Action<IEncounterInfo> use) : EncounterListPageViewModel(use)
{
    private IReadOnlyList<EncounterCardViewModel> _all = [];
    private bool _loaded;

    public override string Title => "Eventos";
    public override string Icon => "🎁";

    private string _search = "";
    public string Search { get => _search; set { if (Set(ref _search, value)) ApplyFilters(); } }

    public override void Load(SaveFile sav)
    {
        base.Load(sav);
        _all = [];
        _loaded = false; // carrega ao entrar na pagina (o banco tem milhares de eventos)
    }

    protected override void OnOnlyThisGameChanged()
    {
        _loaded = false;
        _ = EnsureLoadedAsync();
    }

    /// <summary>Le o banco em segundo plano na primeira vez que a pagina e aberta.</summary>
    public async Task EnsureLoadedAsync()
    {
        if (_loaded || IsBusy || Sav is not { } sav)
            return;
        IsBusy = true;
        Summary = "Carregando eventos...";
        try
        {
            bool only = OnlyThisGame;
            var gifts = await Task.Run(() => EncounterDatabase.LoadGifts(sav, only));
            _all = [.. gifts.Select(g => new EncounterCardViewModel(g))];
            _loaded = true;
            SetSource(_all);
        }
        catch (Exception ex)
        {
            Summary = $"Erro ao carregar eventos: {ex.Message}";
        }
        finally
        {
            IsBusy = false;
        }
    }

    protected override bool Matches(EncounterCardViewModel card)
    {
        var q = _search.Trim();
        return q.Length == 0 || card.SearchText.Contains(q, StringComparison.OrdinalIgnoreCase);
    }

    protected override void OnFiltered(int shown, int total)
    {
        var q = _search.Trim();
        Summary = total == 0 ? "Nenhum evento disponível para este jogo."
            : $"{shown} de {total} evento(s)" + (q.Length > 0 ? $" para “{q}”" : "") + ". Clique num cartão para ver os detalhes; Usar leva ao editor.";
    }
}

/// <summary>Painel de detalhes do encontro/evento selecionado: o que ele garante, golpes e o texto completo do PKHeX.</summary>
public sealed class EncounterDetailViewModel(EncounterCardViewModel card)
{
    public EncounterCardViewModel Card { get; } = card;
    public IReadOnlyList<EncounterFact> Facts { get; } = [.. EncounterDatabase.GetFacts(card.Encounter).Select(f => new EncounterFact(f.Label, f.Value))];
    public IReadOnlyList<EncounterMoveViewModel> Moves { get; } = [.. card.FixedMoves.Select(m => new EncounterMoveViewModel(m, card.Encounter))];
    public bool HasMoves => Moves.Count > 0;
    public string MovesHint => HasMoves ? "" : "Vem com os golpes do nível em que é encontrado.";
    public Bitmap? BallIcon => Card.Encounter is IFixedBall { FixedBall: not Ball.None } b ? SpriteService.GetBallSprite((byte)b.FixedBall) : null;
    public string Details => Card.Details;

    // Variacoes juntadas no cartao (so no "Ver todos"); "Usar" gera a primeira. Linhas iguais (slots diferentes com o
    // mesmo tipo e nivel) viram uma so, com a contagem.
    private const int MaxVariants = 40;
    public bool HasVariants => Card.HasVariants;
    public string VariantsHint => HasVariants ? Loc.T($"Este cartão junta {Card.Variants.Count} encontros da mesma espécie neste local. Usar gera o primeiro.") : "";
    public IReadOnlyList<string> Variants { get; } = card.HasVariants ? VariantLines(card.Variants) : [];

    private static List<string> VariantLines(IReadOnlyList<IEncounterInfo> variants)
    {
        var lines = variants.Select(VariantLine).GroupBy(l => l).Select(g => g.Count() > 1 ? $"{g.Key} (×{g.Count()})" : g.Key).ToList();
        if (lines.Count <= MaxVariants)
            return lines;
        return [.. lines.Take(MaxVariants), Loc.T($"... e mais {lines.Count - MaxVariants}.")];
    }

    private static string VariantLine(IEncounterInfo v)
    {
        var kind = v switch
        {
            MysteryGift g when !string.IsNullOrWhiteSpace(g.CardTitle) => g.CardTitle.Replace('　', ' ').Trim(),
            IEncounterable e => e.LongName,
            _ => "",
        };
        var level = EncounterCardViewModel.LevelText(v.LevelMin, v.LevelMax);
        return kind.Length == 0 ? level : $"{kind} · {level}";
    }
}

public sealed record EncounterFact(string Label, string Value);

/// <summary>Golpe fixo do encontro, com o tipo e a descricao em portugues.</summary>
public sealed class EncounterMoveViewModel(ushort move, IEncounterInfo enc)
{
    private readonly (string Name, uint Argb)? _type = CoreAdapter.GetMoveType(move, enc.Context);
    public string Name => move < CoreAdapter.MoveNames.Count ? CoreAdapter.MoveNames[move] : $"#{move}";
    public string TypeName => _type?.Name ?? "";
    public Avalonia.Media.IBrush TypeBrush => new Avalonia.Media.SolidColorBrush(_type?.Argb ?? 0x00000000);
    public string? Tip => GameText.GetMove(Name, enc.Generation);
}
