using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using PKHeX.Core;
using PKHeX.Modern.Services;

namespace PKHeX.Modern.ViewModels;

/// <summary>Analise de tipos da equipe: tabela de defesa (cada tipo de golpe contra cada Pokemon) e cobertura dos golpes.</summary>
public sealed partial class PartyPageViewModel
{
    public TeamTypeAnalysis.Report? TypeReport { get; private set; }
    public bool HasTypeAnalysis => TypeReport is { Members.Count: > 0 };
    public IReadOnlyList<TypeMemberViewModel> TypeMembers { get; private set; } = [];
    public IReadOnlyList<DefenseRowViewModel> DefenseRows { get; private set; } = [];
    public IReadOnlyList<CoverageTileViewModel> CoverageTiles { get; private set; } = [];
    public bool HasCoverage => TypeReport?.HasAttacks == true;
    public string DefenseSummary { get; private set; } = "";
    public string CoverageSummary { get; private set; } = "";
    public string CoverageGaps { get; private set; } = "";
    public bool HasCoverageGaps => CoverageGaps.Length > 0;

    private void WatchTypeSlots()
    {
        foreach (var slot in Slots)
            slot.PropertyChanged += OnTypeSlotChanged;
    }

    private void OnTypeSlotChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(SlotViewModel.Pkm))
            RefreshTypes();
    }

    public void RefreshTypes()
    {
        TypeReport = _sav is null ? null
            : TeamTypeAnalysis.Analyze(Slots.Where(s => s.Pkm is not null).Select(s => s.Pkm!), _sav.Context, _sav.Generation);
        var report = TypeReport;
        if (report is null || report.Members.Count == 0)
        {
            TypeMembers = []; DefenseRows = []; CoverageTiles = [];
            DefenseSummary = CoverageSummary = CoverageGaps = "";
        }
        else
        {
            TypeMembers = [.. report.Members.Select(m => new TypeMemberViewModel(m.Name, SpriteService.GetSprite(m.Pk)))];
            DefenseRows = [.. report.Defense.Select(r => new DefenseRowViewModel(r))];
            CoverageTiles = [.. report.Coverage.Select(c => new CoverageTileViewModel(c))];

            var danger = report.Defense.Where(r => r.IsDanger).OrderByDescending(r => r.Weak).ToList();
            DefenseSummary = danger.Count == 0
                ? "Nenhum tipo pega a equipe desprevenida: para cada tipo, há quem resista ou quase ninguém é fraco."
                : $"Pontos fracos: {string.Join(", ", danger.Select(r => $"{TypeName(r.Type)} ({r.Weak})"))}.";
            int super = report.Coverage.Count(c => c.Best > 1);
            CoverageSummary = report.HasAttacks
                ? $"Os golpes da equipe são super efetivos contra {super} de {report.Coverage.Count} tipos."
                : "Nenhum golpe de ataque na equipe.";
            var gaps = report.Coverage.Where(c => c.Best < 1).Select(c => TypeName(c.Type)).ToList();
            CoverageGaps = report.HasAttacks && gaps.Count > 0 ? $"Nenhum golpe acerta bem: {string.Join(", ", gaps)}." : "";
        }
        foreach (var name in new[] { nameof(TypeReport), nameof(HasTypeAnalysis), nameof(TypeMembers), nameof(DefenseRows), nameof(CoverageTiles),
                     nameof(HasCoverage), nameof(DefenseSummary), nameof(CoverageSummary), nameof(CoverageGaps), nameof(HasCoverageGaps) })
            Raise(name);
    }

    internal static string TypeName(byte type) => type < GameInfo.Strings.types.Length ? GameInfo.Strings.types[type] : "?";

    internal static TypeChip Chip(byte type) => new(TypeName(type), (uint)Drawing.PokeSprite.TypeColor.GetTypeSpriteColor(type).ToArgb());

    internal static string Format(double x) => x switch { 0 => "0", 0.25 => "¼", 0.5 => "½", 1 => "", 2 => "2×", 4 => "4×", _ => $"{x}×" };

    private static readonly IBrush Weak2 = new SolidColorBrush(Color.FromArgb(0x55, 0xE5, 0x48, 0x4D));
    private static readonly IBrush Weak4 = new SolidColorBrush(Color.FromArgb(0xB0, 0xE5, 0x48, 0x4D));
    private static readonly IBrush Resist2 = new SolidColorBrush(Color.FromArgb(0x50, 0x30, 0xA4, 0x6C));
    private static readonly IBrush Resist4 = new SolidColorBrush(Color.FromArgb(0x99, 0x30, 0xA4, 0x6C));
    private static readonly IBrush ImmuneBrush = new SolidColorBrush(Color.FromArgb(0x70, 0x5B, 0x5F, 0x6E));

    /// <summary>Fundo da celula: vermelho para fraqueza (mais forte no 4×), verde para resistencia, cinza para imunidade.</summary>
    internal static IBrush? CellBrush(double x) => x switch { 0 => ImmuneBrush, < 0.5 => Resist4, < 1 => Resist2, > 2 => Weak4, > 1 => Weak2, _ => null };
}

public sealed record TypeMemberViewModel(string Name, Bitmap? Sprite);

public sealed record TypeCellViewModel(string Text, IBrush? Background, string Tip);

public sealed class DefenseRowViewModel(TeamTypeAnalysis.DefenseRow row)
{
    public TypeChip Chip { get; } = PartyPageViewModel.Chip(row.Type);
    public IReadOnlyList<TypeCellViewModel> Cells { get; } = [.. row.Multipliers.Select(x => new TypeCellViewModel(PartyPageViewModel.Format(x), PartyPageViewModel.CellBrush(x),
        x == 1 ? "Dano normal" : x == 0 ? "Imune" : x > 1 ? $"Fraco: {PartyPageViewModel.Format(x)}" : $"Resiste: {PartyPageViewModel.Format(x)}"))];
    public string Weak => row.Weak == 0 ? "–" : row.Weak.ToString();
    public string Resist => row.Resist + row.Immune == 0 ? "–" : (row.Resist + row.Immune).ToString();
    public bool IsDanger => row.IsDanger;
    public IBrush? RowBackground => row.IsDanger ? DangerRow : null;
    private static readonly IBrush DangerRow = new SolidColorBrush(Color.FromArgb(0x24, 0xE5, 0x48, 0x4D));
}

public sealed class CoverageTileViewModel(TeamTypeAnalysis.CoverageCell cell)
{
    public TypeChip Chip { get; } = PartyPageViewModel.Chip(cell.Type);
    public string Text => cell.Best == 1 ? "1×" : PartyPageViewModel.Format(cell.Best);
    public IBrush? Background => cell.Best switch { > 1 => PartyPageViewModel.CellBrush(0.5), 1 => null, 0 => PartyPageViewModel.CellBrush(0), _ => PartyPageViewModel.CellBrush(2) };
    public string Tip => cell.SuperEffective.Count > 0 ? "Super efetivo: " + string.Join(", ", cell.SuperEffective)
        : cell.Best == 0 ? "Nenhum golpe da equipe afeta este tipo" : cell.Best < 1 ? "Os golpes da equipe são pouco efetivos contra este tipo" : "Nenhum golpe super efetivo contra este tipo";
}
