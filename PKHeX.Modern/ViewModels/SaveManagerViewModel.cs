using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using PKHeX.Core;
using PKHeX.Modern.Services;

namespace PKHeX.Modern.ViewModels;

/// <summary>
/// Save Manager: lista os saves da pasta configurada, agrupados por console.
/// E a tela inicial (sem save aberto) e tambem uma pagina da barra lateral.
/// </summary>
public sealed class SaveManagerViewModel : PageViewModel
{
    private readonly AppSettings _settings;
    private readonly Action<string> _open;
    private IReadOnlyList<SaveEntryViewModel> _all = [];
    private string? _currentPath;
    private readonly Func<string, string, string, Task<bool>> _confirm;
    private readonly Action<string> _status;

    /// <param name="confirm">Confirmacao (titulo, mensagem, botao) → true/false.</param>
    public SaveManagerViewModel(AppSettings settings, Action<string> open,
        Func<string, string, string, Task<bool>>? confirm = null, Action<string>? status = null)
    {
        _settings = settings;
        _open = open;
        _confirm = confirm ?? ((_, _, _) => Task.FromResult(true));
        _status = status ?? (_ => { });
        RefreshCommand = new RelayCommand(() => _ = ShowBackups ? RefreshBackupsAsync() : RefreshAsync());
        OpenFolderCommand = new RelayCommand(OpenFolder);
        OpenBackupsCommand = new RelayCommand(() => { ShowBackups = true; _ = RefreshBackupsAsync(); });
        BackToSavesCommand = new RelayCommand(() => ShowBackups = false);
        OpenBackupsFolderCommand = new RelayCommand(() => OpenInExplorer(SaveBackup.Folder));
        RestoreCommand = new RelayCommand(p => { if (p is BackupEntryViewModel b && b.Source is { } src) _ = RestoreToAsync(b, src); });
        DeleteBackupCommand = new RelayCommand(p => { if (p is BackupEntryViewModel b) _ = DeleteBackupAsync(b); });
        CompareCommand = new RelayCommand(p => { if (p is BackupEntryViewModel b) _ = CompareAsync(b); });
        OpenCommand = new RelayCommand(p => { if (p is SaveEntryViewModel e) _open(e.Path); });
        SearchEmulatorsCommand = new RelayCommand(() => _ = SearchEmulatorsAsync());
    }

    /// <summary>Android: botao "Saves dos emuladores" (procura nas pastas padrao de cada emulador).</summary>
    public bool CanSearchEmulators => EmulatorSaves.IsAvailable;
    public RelayCommand SearchEmulatorsCommand { get; }
    public string EmulatorButtonText => _settings.EmulatorScan && EmulatorSaves.CanRead ? "🎮  Emuladores ✓" : "🎮  Saves dos emuladores";

    /// <summary>
    /// Liga a busca nos emuladores. Sem o acesso a todos os arquivos, explica e abre a tela do Android; a lista
    /// e relida quando o usuario volta ao app (<see cref="EmulatorSaves.Resumed"/>).
    /// </summary>
    public async Task SearchEmulatorsAsync()
    {
        if (!EmulatorSaves.IsAvailable)
            return;
        if (!EmulatorSaves.CanRead)
        {
            if (!await _confirm("Saves dos emuladores",
                    "Para achar sozinho os saves do RetroArch, DraStic, melonDS, Dolphin, Azahar/Citra, Eden e outros, o Android precisa dar ao app o “acesso a todos os arquivos”. "
                    + "Na tela que vai abrir, ligue a opção para o PKHeX Modern e volte ao app. Os saves são lidos e gravados no lugar original, com backup antes de gravar.",
                    "Abrir configuração"))
                return;
            _settings.EmulatorScan = true;
            _settings.Save();
            EmulatorSaves.RequestAccess?.Invoke();
            return;
        }
        _settings.EmulatorScan = true;
        _settings.Save();
        Raise(nameof(EmulatorButtonText));
        await RefreshAsync();
    }

    /// <summary>Voltou ao app (ex.: depois de dar o acesso): rele se a busca nos emuladores esta ligada.</summary>
    public void OnResumed()
    {
        Raise(nameof(EmulatorButtonText));
        if (_settings.EmulatorScan && EmulatorSaves.CanRead)
            _ = RefreshAsync();
    }

    /// <summary>O save esta aberto numa aba? (definido pelo MainViewModel; sem ele, so o save carregado conta)</summary>
    public Func<string, bool>? IsOpenPath { get; init; }
    /// <summary>Rele um save aberto do disco (ex.: depois de restaurar um backup dele).</summary>
    public Action<string>? Reload { get; init; }

    /// <summary>Antes de reler a pasta (Android: traz os saves da pasta escolhida no seletor).</summary>
    public Func<Task>? BeforeRefresh { get; set; }
    /// <summary>Nome mostrado da pasta (Android: o nome da pasta escolhida, nao o caminho da copia privada).</summary>
    public Func<string>? DescribeFolder { get; set; }
    public string FolderLabel => DescribeFolder?.Invoke() ?? Folder;
    /// <summary>"Abrir pasta" no explorador de arquivos (nao existe no Android).</summary>
    public bool CanOpenFolder => App.ShowShortcuts;
    public void RaiseFolder() { Raise(nameof(Folder)); Raise(nameof(FolderLabel)); }
    public void ReportStatus(string message) => _status(message);

    private bool IsOpen(string path) => IsOpenPath?.Invoke(path) ?? IsSamePath(path, _currentPath);

    public override string Title => "Saves";
    public override string Icon => "🗂";

    public override void Load(SaveFile sav)
    {
        _currentPath = sav.Metadata.FilePath;
        RefreshOpenMarks();
    }

    /// <summary>Atualiza o selo "Aberto" (saves nas abas).</summary>
    public void RefreshOpenMarks()
    {
        foreach (var e in _all)
            e.IsCurrent = IsOpen(e.Path);
    }

    /// <summary>Saves encontrados na pasta (a pagina Bank oferece estes no painel "Outro save").</summary>
    public IReadOnlyList<SaveEntryViewModel> Entries => _all;
    /// <summary>A pasta tem saves (a barra lateral oferece abrir um deles quando nada esta aberto).</summary>
    public bool HasEntries => _all.Count > 0;

    public RelayCommand RefreshCommand { get; }
    public RelayCommand OpenFolderCommand { get; }
    /// <summary>Mostra a lista de backups automaticos (feitos antes de salvar por cima de um save).</summary>
    public RelayCommand OpenBackupsCommand { get; }
    public RelayCommand BackToSavesCommand { get; }
    public RelayCommand OpenBackupsFolderCommand { get; }
    public RelayCommand RestoreCommand { get; }
    public RelayCommand DeleteBackupCommand { get; }
    public RelayCommand CompareCommand { get; }

    /// <summary>Janela do "Comparar" (titulo, mensagem, linhas, pode restaurar) → true quando escolhe Restaurar.</summary>
    public Func<string, string, IReadOnlyList<string>, bool, Task<bool>>? ShowComparison { get; init; }

    /// <summary>
    /// Compara o backup com o arquivo de origem como está gravado (Services/BackupCompare.cs): o que só o save atual
    /// tem (e se perde ao restaurar) e o que só o backup tem. Da janela dá para seguir para o Restaurar.
    /// </summary>
    public async Task CompareAsync(BackupEntryViewModel backup)
    {
        if (backup.Source is not { } target || !ZipSaves.Exists(target))
        {
            _status("O save de origem deste backup não existe mais: não há com o que comparar.");
            return;
        }
        BackupCompare.Result? result = null;
        try
        {
            result = await Task.Run(() => CoreAdapter.LoadSave(backup.Info.Path) is { } old && CoreAdapter.LoadSave(target) is { } now
                ? BackupCompare.Compare(old, now) : null);
        }
        catch (Exception) { result = null; }
        if (result is null)
        {
            _status("Não foi possível ler o backup ou o save atual para comparar.");
            return;
        }
        var name = System.IO.Path.GetFileName(ZipSaves.FileOf(target));
        var message = result.Same
            ? Loc.T($"O backup de {backup.When} e “{name}” têm o mesmo progresso: horas de jogo, Pokémon, Pokédex, insígnias e dinheiro iguais.")
            : Loc.T($"Backup de {backup.When} comparado com “{name}” como está gravado.");
        if (IsOpen(target))
            message += " " + Loc.T("Alterações ainda não salvas no app ficam de fora.");
        var lines = new List<string>();
        if (result.OnlyCurrent.Count > 0)
        {
            lines.Add(Loc.T("Só no save atual (perde ao restaurar):"));
            lines.AddRange(result.OnlyCurrent.Select(l => "• " + l));
        }
        if (result.OnlyBackup.Count > 0)
        {
            lines.Add(Loc.T("Só no backup (volta ao restaurar):"));
            lines.AddRange(result.OnlyBackup.Select(l => "• " + l));
        }
        if (ShowComparison is { } show && await show("Comparar com o save atual", message, lines, !result.Same))
            await RestoreToAsync(backup, target);
    }

    // Backups
    private bool _showBackups;
    /// <summary>Mostrando a lista de backups no lugar dos saves.</summary>
    public bool ShowBackups { get => _showBackups; set { if (Set(ref _showBackups, value)) Raise(nameof(ShowSaves)); } }
    public bool ShowSaves => !ShowBackups;
    public ObservableCollection<BackupGroupViewModel> BackupGroups { get; } = [];
    public string BackupFolder => SaveBackup.Folder;
    private string _backupSummary = "";
    public string BackupSummary { get => _backupSummary; private set => Set(ref _backupSummary, value); }
    public bool HasNoBackups => !IsLoading && BackupGroups.Count == 0;

    /// <summary>Le os backups (e o resumo de cada um) em segundo plano.</summary>
    public async Task RefreshBackupsAsync()
    {
        IsLoading = true;
        var items = await Task.Run(() => SaveBackup.List().Select(b => (Info: b, Entry: SaveLibrary.ReadOne(b.Path))).ToList());
        BackupGroups.Clear();
        foreach (var g in items.GroupBy(i => i.Info.SaveName, StringComparer.OrdinalIgnoreCase))
            BackupGroups.Add(new BackupGroupViewModel(g.Key, [.. g.Select(i => new BackupEntryViewModel(i.Info, i.Entry))]));
        BackupSummary = items.Count == 0 ? "" : $"{items.Count} backup(s) de {BackupGroups.Count} save(s) · até 20 por save, os mais antigos saem sozinhos";
        IsLoading = false;
        Raise(nameof(HasNoBackups));
    }

    /// <summary>Restaura o backup em <paramref name="target"/> (pergunta antes). O arquivo atual ganha um backup antes.</summary>
    public async Task RestoreToAsync(BackupEntryViewModel backup, string target)
    {
        bool isOpen = IsOpen(target);
        bool corrupt = SaveBackup.Verify(backup.Info.Path, SaveBackup.ReadMetadata(backup.Info.Path)) == false;
        var message = $"“{System.IO.Path.GetFileName(target)}” volta a ser como estava em {backup.When}."
                      + (File.Exists(target) ? " O arquivo atual ganha um backup antes, então dá para voltar atrás." : "")
                      + (isOpen ? " Esse save está aberto: ele será reaberto em seguida." : "");
        if (corrupt) message = Loc.T("⚠ O hash do backup não confere. O arquivo pode ter sido alterado ou corrompido. Restaurar mesmo assim?") + "\n\n" + message;
        if (!await _confirm("Restaurar backup?", message, corrupt ? "Restaurar mesmo assim" : "Restaurar"))
            return;
        try
        {
            var safety = SaveBackup.Restore(backup.Info, target, allowCorrupted: corrupt);
            _status($"Backup de {backup.When} restaurado em {target}."
                    + (safety is not null ? $" O arquivo anterior está nos backups ({System.IO.Path.GetFileName(safety)})." : ""));
        }
        catch (Exception ex)
        {
            _status($"Não deu para restaurar: {ex.Message}");
            return;
        }
        await RefreshBackupsAsync();
        if (isOpen)
            (Reload ?? _open)(target); // rele a aba do disco (o MainViewModel pergunta antes de descartar alteracoes)
    }

    private async Task DeleteBackupAsync(BackupEntryViewModel backup)
    {
        if (!await _confirm("Excluir backup?", $"O backup de {backup.SaveName} feito em {backup.When} será apagado. Isso não pode ser desfeito.", "Excluir"))
            return;
        try
        {
            SaveBackup.Delete(backup.Info);
            _status($"Backup de {backup.When} excluído.");
        }
        catch (Exception ex)
        {
            _status($"Não deu para excluir: {ex.Message}");
        }
        await RefreshBackupsAsync();
    }
    public RelayCommand OpenCommand { get; }

    public string Folder
    {
        get => _settings.SavesFolder ?? SaveLibrary.DefaultFolder;
        set
        {
            _settings.SavesFolder = value;
            _settings.Save();
            Raise();
            _ = RefreshAsync();
        }
    }

    public ObservableCollection<SaveGroupViewModel> Groups { get; } = [];
    public bool HideSID
    {
        get => _settings.HideSaveSID;
        set
        {
            if (value == _settings.HideSaveSID) return;
            _settings.HideSaveSID = value;
            _settings.Save();
            foreach (var entry in _all) entry.HideSID = value;
            Raise();
        }
    }

    private string _search = "";
    public string Search { get => _search; set { if (Set(ref _search, value)) ApplyFilter(); } }

    private int? _generation;
    /// <summary>Filtro por geracao (null = todas).</summary>
    public int? GenerationFilter
    {
        get => _generation;
        set
        {
            if (!Set(ref _generation, value))
                return;
            foreach (var g in Generations)
                g.IsActive = g.Generation == value;
            ApplyFilter();
        }
    }
    public IReadOnlyList<GenerationFilterViewModel> Generations { get; private set; } = [];
    public RelayCommand SetGenerationCommand => new(p => GenerationFilter = p is int g && g != GenerationFilter ? g : null);

    private bool _isLoading;
    public bool IsLoading { get => _isLoading; private set => Set(ref _isLoading, value); }

    private string _summary = "";
    public string Summary { get => _summary; private set => Set(ref _summary, value); }

    public bool IsEmpty => !IsLoading && _all.Count == 0;
    /// <summary>"Lendo saves..." so na primeira leitura; nas releituras a lista atual continua visivel.</summary>
    public bool ShowLoading => IsLoading && _all.Count == 0;
    public bool FolderExists => Directory.Exists(Folder);

    /// <summary>Le a pasta em segundo plano e monta os cartoes na thread da interface.</summary>
    public async Task RefreshAsync()
    {
        if (IsLoading)
            return;
        IsLoading = true;
        Raise(nameof(IsEmpty));
        Raise(nameof(ShowLoading));
        if (BeforeRefresh is { } before)
        {
            try { await before(); }
            catch (Exception ex) { _status($"Não foi possível ler a pasta de saves: {ex.Message}"); }
        }
        var folder = Folder;
        var (entries, skipped) = await Task.Run(() => (SaveLibrary.Scan(folder, out var s), s));
        int emulators = 0;
        if (_settings.EmulatorScan && EmulatorSaves.CanRead && EmulatorSaves.StorageRoot is { } root)
        {
            try
            {
                var found = await Task.Run(() => SaveLibrary.ReadFound(EmulatorSaves.Find(root)));
                var known = new HashSet<string>(entries.Select(e => e.Path), StringComparer.OrdinalIgnoreCase);
                var extra = found.Where(f => known.Add(f.Path)).ToList();
                emulators = extra.Count;
                entries = [.. entries, .. extra];
            }
            catch (Exception ex)
            {
                CrashLog.Write(ex);
                _status($"Não foi possível procurar os saves dos emuladores: {ex.Message}");
            }
        }

        _all = [.. entries.Select(e => new SaveEntryViewModel(e)
        {
            IsCurrent = IsOpen(e.Path),
            HideSID = HideSID,
        })];
        if (_generation is { } gen && !_all.Any(e => e.Entry.Generation == gen))
            _generation = null;
        Generations = [.. _all.Select(e => (int)e.Entry.Generation).Distinct().Order().Select(g => new GenerationFilterViewModel(g) { IsActive = g == _generation })];
        Summary = $"{entries.Count} save(s)" + (emulators > 0 ? " · " + string.Format(Loc.T("{0} dos emuladores"), emulators) : "") + (skipped > 0 ? $" · {skipped} arquivo(s) ignorado(s) por não serem saves reconhecidos" : "");
        IsLoading = false;
        foreach (var p in (string[])[nameof(IsEmpty), nameof(ShowLoading), nameof(FolderExists), nameof(Generations), nameof(GenerationFilter), nameof(Entries), nameof(HasEntries)])
            Raise(p);
        ApplyFilter();
    }

    private static bool IsSamePath(string a, string? b)
        => b is not null && string.Equals(Path.GetFullPath(a), Path.GetFullPath(b), StringComparison.OrdinalIgnoreCase);

    private void ApplyFilter()
    {
        var query = _search.Trim();
        var visible = _all.Where(e =>
            (_generation is null || e.Entry.Generation == _generation) &&
            (query.Length == 0 || e.SearchText.Contains(query, StringComparison.OrdinalIgnoreCase)));
        Groups.Clear();
        foreach (var g in visible.GroupBy(e => e.Entry.Group))
            Groups.Add(new SaveGroupViewModel(g.Key, [.. g]));
    }

    private static void OpenInExplorer(string folder)
    {
        try
        {
            Directory.CreateDirectory(folder);
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(folder) { UseShellExecute = true });
        }
        catch
        {
            // sem explorador de arquivos disponivel
        }
    }

    private void OpenFolder()
    {
        try
        {
            Directory.CreateDirectory(Folder);
            Raise(nameof(FolderExists));
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(Folder) { UseShellExecute = true });
        }
        catch
        {
            // sem explorador de arquivos disponivel: a pasta ja foi criada
        }
    }
}

public sealed class GenerationFilterViewModel(int generation) : ViewModelBase
{
    public int Generation { get; } = generation;
    public string Label => $"Gen {Generation}";
    private bool _isActive;
    public bool IsActive { get => _isActive; set => Set(ref _isActive, value); }
}

public sealed record SaveGroupViewModel(string Title, IReadOnlyList<SaveEntryViewModel> Items);

public sealed class SaveEntryViewModel(SaveEntry entry) : ViewModelBase
{
    public SaveEntry Entry { get; } = entry;
    public string Path => Entry.Path;
    public string FileName => ZipSaves.DisplayName(Entry.Path);
    /// <summary>Save dentro de um .zip (backup do JKSV): selo "ZIP" no cartao.</summary>
    public bool IsZipped => ZipSaves.IsZipPath(Entry.Path, out _, out _);
    public string Game => Entry.Game;
    public string GenerationBadge => $"Gen {Entry.Generation}";
    /// <summary>Selo do jogo (Pokemon da capa nas cores da versao).</summary>
    public GameArt Art => GameArt.Get(Entry.Version);
    public string Trainer => Entry.Trainer + (Entry.TrainerIsFemale ? "  ♀" : "  ♂");
    private bool _hideSID;
    public bool HideSID { get => _hideSID; set { if (Set(ref _hideSID, value)) Raise(nameof(Details)); } }
    public string Details => $"{(HideSID ? $"TID {Entry.TrainerId}" : Entry.Ids)} · {Entry.PlayTime} · ${Entry.Money:N0}" + (Entry.Caught > 0 ? $" · {Entry.Caught} capturados" : "");
    public string LanguageBadge => Entry.Language switch
    {
        1 => "JA", 2 => "EN", 3 => "FR", 4 => "IT", 5 => "DE", 7 => "ES", 8 => "KO", 9 => "ZH-CN", 10 => "ZH-TW", 11 => "ES-LA", _ => "",
    };
    public bool HasLanguage => LanguageBadge.Length > 0;
    public string AdventureStart => Entry.AdventureStart is { } date ? $"Aventura iniciada em {date:dd/MM/yyyy}" : "";
    public bool HasAdventureStart => Entry.AdventureStart is not null;
    public string LastWrite => $"Salvo em {Entry.LastWrite:dd/MM/yyyy HH:mm}";
    public string SearchText => $"{Entry.Game} {Entry.Trainer} {FileName} {Entry.Group}";

    private bool _isCurrent;
    /// <summary>Save aberto no momento (selo "Aberto").</summary>
    public bool IsCurrent { get => _isCurrent; set => Set(ref _isCurrent, value); }

    private IReadOnlyList<Bitmap>? _party;
    /// <summary>Sprites da equipe, gerados so quando o cartao aparece.</summary>
    public IReadOnlyList<Bitmap> PartySprites => _party ??= [.. Entry.Party.Select(SpriteService.GetSprite).OfType<Bitmap>()];
}

public sealed record BackupGroupViewModel(string Title, IReadOnlyList<BackupEntryViewModel> Items);

/// <summary>Um backup na lista: resumo do save (como os cartoes de saves), data e origem.</summary>
public sealed class BackupEntryViewModel(BackupInfo info, SaveEntry? entry) : ViewModelBase
{
    public BackupInfo Info { get; } = info;
    public SaveEntry? Entry { get; } = entry;
    public string SaveName => Info.SaveName;
    public string? Source => Info.Source;
    public bool CanRestore => Source is not null;
    public string Game => Entry?.Game ?? "Arquivo não reconhecido como save";
    public string Trainer => Entry is null ? "" : Entry.Trainer + (Entry.TrainerIsFemale ? "  ♀" : "  ♂");
    public string Details => Entry is null ? "" : $"{Entry.PlayTime} · ${Entry.Money:N0}" + (Entry.Caught > 0 ? $" · {Entry.Caught} capturados" : "");
    public string When => Info.Created.ToString("dd/MM/yyyy HH:mm:ss");
    public string Reason => Loc.T(Info.Metadata?.Reason ?? "Motivo desconhecido");
    public string Integrity => Info.IntegrityMatches == false ? Loc.T("⚠ Hash do backup não confere") : Info.IntegrityMatches == true ? "SHA-256 ✓" : "";
    public string SourceText => Source is null ? "Origem desconhecida (backup antigo): use “Restaurar como...”" : $"Volta para: {Source}";

    private IReadOnlyList<Bitmap>? _party;
    public IReadOnlyList<Bitmap> PartySprites => _party ??= Entry is null ? [] : [.. Entry.Party.Select(SpriteService.GetSprite).OfType<Bitmap>()];
}
