using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Media.Imaging;
using PKHeX.Core;
using PKHeX.Modern.Services;

namespace PKHeX.Modern.ViewModels;

/// <summary>
/// Tela inicial do save aberto: cartao do jogo, a equipe no topo, atalhos grandes para cada pagina e os saves recentes.
/// Fica fora da lista numerada da barra lateral (Ctrl+1..9 continuam iguais); o atalho e Ctrl+0.
/// </summary>
public sealed class HomePageViewModel(Action<PageViewModel> navigate, Action<int> openPartySlot, Action<string> openSave,
    Func<IReadOnlyList<string>> recentPaths, Func<string?> currentPath) : PageViewModel
{
    public override string Title => "Início";
    public override string Icon => "⌂";

    private SaveFile? _sav;

    /// <summary>Paginas que viram atalhos (definido pelo MainViewModel, na ordem da barra lateral).</summary>
    public Func<IReadOnlyList<PageViewModel>>? Pages { get; set; }
    /// <summary>Situacao do save (definida pelo MainViewModel): alteracoes nao salvas e as acoes do cartao de status.</summary>
    public Func<(bool Dirty, int Count)>? Pending { get; set; }
    public RelayCommand? ShowPendingCommand { get; set; }
    /// <summary>Linha do tempo do save ativo (definida pelo MainViewModel, mais nova primeiro).</summary>
    public Func<IReadOnlyList<TimelineEntry>>? Timeline { get; set; }
    public RelayCommand? BackupsCommand { get; set; }

    public override void Load(SaveFile sav)
    {
        _sav = sav;
        Refresh();
    }

    /// <summary>Rele tudo (ao voltar para o Inicio, a equipe e os numeros podem ter mudado).</summary>
    public void Refresh()
    {
        if (_sav is not { } sav)
            return;
        Art = GameArt.Get(sav.Version);
        GameName = CoreAdapter.GetGameName(sav);
        Trainer = $"{sav.OT} · TID {sav.DisplayTID}";
        var facts = new List<string>();
        try { facts.Add($"{sav.PlayedHours}h {sav.PlayedMinutes:00}m"); } catch { /* sem tempo de jogo */ }
        try { facts.Add($"${sav.Money:N0}"); } catch { /* sem dinheiro */ }
        if (sav.HasPokeDex)
            try { facts.Add($"Pokédex {sav.CaughtCount}/{sav.MaxSpeciesID}"); } catch { /* Pokedex ilegivel */ }
        Facts = string.Join(" · ", facts);

        var party = new List<HomePartyViewModel>();
        if (sav.HasParty)
            for (int i = 0; i < sav.PartyCount; i++)
                if (sav.GetPartySlotAtIndex(i) is { } pk && !CoreAdapter.IsEmpty(pk))
                {
                    int index = i;
                    party.Add(new HomePartyViewModel(pk, new RelayCommand(() => openPartySlot(index))));
                }
        Party = party;

        int stored = 0;
        try
        {
            for (int b = 0; b < sav.BoxCount; b++)
                for (int s = 0; s < sav.BoxSlotCount; s++)
                    if (!CoreAdapter.IsEmpty(sav.GetBoxSlotAtIndex(b, s)))
                        stored++;
        }
        catch
        {
            // caixas ilegiveis
        }
        Status = BuildStatus(sav, stored);

        var tiles = new List<HomeTileViewModel>();
        foreach (var page in Pages?.Invoke() ?? [])
            tiles.Add(new HomeTileViewModel(page.Icon, page.Title, TileDetail(page, sav, stored, party.Count), page.Shortcut, new RelayCommand(() => navigate(page))));
        Tiles = tiles;

        var current = currentPath();
        var recent = new List<HomeRecentViewModel>();
        foreach (var path in recentPaths())
        {
            if (string.Equals(path, current, StringComparison.OrdinalIgnoreCase) || !ZipSaves.Exists(path))
                continue;
            if (SaveLibrary.ReadOne(path) is not { } entry)
                continue;
            recent.Add(new HomeRecentViewModel(entry, new RelayCommand(() => openSave(path))));
            if (recent.Count == 6)
                break;
        }
        Recent = recent;
        TimelineEntries = [.. (Timeline?.Invoke() ?? []).Take(6).Select(e => new HomeTimelineViewModel(e, WhenText(e.When)))];
        foreach (var p in (string[])[nameof(Art), nameof(GameName), nameof(Trainer), nameof(Facts), nameof(Status), nameof(Party), nameof(HasParty), nameof(Tiles), nameof(Recent), nameof(HasRecent),
            nameof(TimelineEntries), nameof(HasTimeline), nameof(HasNoTimeline)])
            Raise(p);
    }

    /// <summary>Cartao de status: o que importa antes de fechar o app (alteracoes, backup, espaco, arquivo).</summary>
    private List<HomeStatusViewModel> BuildStatus(SaveFile sav, int stored)
    {
        var list = new List<HomeStatusViewModel>();
        var (dirty, count) = Pending?.Invoke() ?? (false, 0);
        list.Add(dirty
            ? new HomeStatusViewModel("Alterações", count switch { 0 => "Não salvas", 1 => "1 não salva", _ => $"{count} não salvas" }, true,
                ShowPendingCommand, "Ver a lista e salvar")
            : new HomeStatusViewModel("Alterações", "Tudo salvo", false, null, "O arquivo do save está igual ao que você vê"));

        var path = currentPath();
        var backup = path is null ? null : SaveBackup.LatestFor(path);
        list.Add(new HomeStatusViewModel("Último backup", backup is { } b ? WhenText(b) : "Nenhum ainda", false, BackupsCommand,
            "Antes de salvar por cima, o arquivo anterior vai para os backups. Clique para ver a lista."));

        int total = sav.BoxCount * sav.BoxSlotCount;
        if (total > 0)
            list.Add(new HomeStatusViewModel("Vagas nas caixas", $"{total - stored}/{total}", total - stored == 0, null,
                total - stored == 0 ? "As caixas estão cheias" : "Espaços livres para novos Pokémon"));

        if (path is not null)
            list.Add(new HomeStatusViewModel("Arquivo", ZipSaves.DisplayName(path), false, null, path));
        return list;
    }

    private static string WhenText(DateTime when)
    {
        var days = (DateTime.Today - when.Date).Days;
        return days switch
        {
            0 => $"Hoje, {when:HH:mm}",
            1 => $"Ontem, {when:HH:mm}",
            < 7 => $"Há {days} dias",
            _ => when.ToString("dd/MM/yyyy"),
        };
    }

    private static string TileDetail(PageViewModel page, SaveFile sav, int stored, int partyCount) => page switch
    {
        BoxesPageViewModel => $"{sav.BoxCount} caixas · {stored} Pokémon",
        PartyPageViewModel => $"{partyCount} de 6",
        BankPageViewModel => "Guardar e trocar entre jogos",
        PokedexPageViewModel => "Todos os saves e o bank",
        TrainerPageViewModel => sav.OT,
        BagPageViewModel => "Itens por bolso",
        EncounterDbViewModel => "Onde e como obter",
        GiftDbViewModel => "Mystery Gift",
        SaveManagerViewModel => "Todos os saves da pasta",
        SearchPageViewModel => "Pokémon de todos os saves e do bank",
        GamePageViewModel => "Flags, recordes, atalhos e editores do jogo",
        BatchPageViewModel => "Alterar vários Pokémon de uma vez",
        ShinyHuntPageViewModel => "Contador, chances e onde encontrar",
        _ => "",
    };

    public GameArt? Art { get; private set; }
    public string GameName { get; private set; } = "";
    public string Trainer { get; private set; } = "";
    public string Facts { get; private set; } = "";
    public IReadOnlyList<HomeStatusViewModel> Status { get; private set; } = [];
    public IReadOnlyList<HomePartyViewModel> Party { get; private set; } = [];
    public bool HasParty => Party.Count > 0;
    public IReadOnlyList<HomeTileViewModel> Tiles { get; private set; } = [];
    public IReadOnlyList<HomeRecentViewModel> Recent { get; private set; } = [];
    public bool HasRecent => Recent.Count > 0;
    public IReadOnlyList<HomeTimelineViewModel> TimelineEntries { get; private set; } = [];
    public bool HasTimeline => TimelineEntries.Count > 0;
    public bool HasNoTimeline => TimelineEntries.Count == 0;
}

/// <summary>Uma linha da linha do tempo do Início: quando, de onde veio a mudança e o que mudou.</summary>
public sealed class HomeTimelineViewModel(TimelineEntry entry, string when)
{
    public string When { get; } = when;
    /// <summary>"No jogo" (mudou fora do app, entre uma abertura e outra) ou "Salvo no app".</summary>
    public string Source { get; } = entry.Kind == "saved" ? "Salvo no app" : "No jogo";
    public bool IsFromApp => entry.Kind == "saved";
    public IReadOnlyList<string> Changes { get; } = entry.Changes;
    /// <summary>Sprites dos shinies novos (até 6).</summary>
    public IReadOnlyList<Bitmap?> Shinies { get; } = [.. entry.NewShinies.Take(6).Select(s => SpriteService.GetSpeciesSprite(s, true))];
    public bool HasShinies => Shinies.Count > 0;
}

/// <summary>Um Pokemon da equipe no topo do Inicio (clique abre na pagina Equipe).</summary>
public sealed class HomePartyViewModel(PKM pk, RelayCommand open)
{
    public Bitmap? Sprite { get; } = SpriteService.GetSprite(pk);
    public string Name { get; } = pk.IsEgg ? "Ovo" : pk.IsNicknamed ? pk.Nickname : CoreAdapter.SpeciesNames[pk.Species];
    public string Level { get; } = pk.IsEgg ? "" : $"Nv. {pk.CurrentLevel}";
    public bool IsShiny { get; } = pk.IsShiny;
    public RelayCommand OpenCommand { get; } = open;
}

/// <summary>Uma linha do cartao de status do Inicio (com acao opcional ao clicar).</summary>
public sealed record HomeStatusViewModel(string Label, string Value, bool IsWarning, RelayCommand? Command, string Tip)
{
    public bool HasCommand => Command is not null;
}

/// <summary>Atalho grande para uma pagina.</summary>
public sealed record HomeTileViewModel(string Icon, string Title, string Detail, string Shortcut, RelayCommand Command);

/// <summary>Um save recente (clique abre numa aba).</summary>
public sealed class HomeRecentViewModel(SaveEntry entry, RelayCommand open)
{
    public GameArt Art { get; } = GameArt.Get(entry.Version);
    public string Game { get; } = entry.Game;
    public string Detail { get; } = $"{entry.Trainer} · {entry.PlayTime} · {ZipSaves.DisplayName(entry.Path)}";
    public RelayCommand OpenCommand { get; } = open;
}
