using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Media;
using PKHeX.Core;
using PKHeX.Modern.Services;

namespace PKHeX.Modern.ViewModels;

/// <summary>
/// Vitrine da equipe: os Pokémon da equipe grandes, com tipos, item, habilidade, natureza e golpes, pensada para print
/// e compartilhamento ("Salvar imagem" grava o quadro como PNG). Dois layouts: grade 3×2 e seis colunas.
/// </summary>
public sealed class TeamShowcaseViewModel : ViewModelBase
{
    private readonly Action _close;
    private readonly Func<TeamShowcaseViewModel, Task> _saveImage;

    public TeamShowcaseViewModel(SaveFile sav, IEnumerable<PKM> party, Action close, Func<TeamShowcaseViewModel, Task> saveImage)
    {
        _close = close;
        _saveImage = saveImage;
        Members = [.. party.Where(pk => !CoreAdapter.IsEmpty(pk)).Select(pk => new ShowcaseMemberViewModel(pk))];
        Title = string.IsNullOrWhiteSpace(sav.OT) ? "Equipe" : $"Equipe de {sav.OT}";
        Subtitle = CoreAdapter.GetGameName(sav);
    }

    public IReadOnlyList<ShowcaseMemberViewModel> Members { get; }
    public string Title { get; }
    public string Subtitle { get; }
    public bool IsEmpty => Members.Count == 0;

    public IReadOnlyList<string> LayoutOptions { get; } = ["Grade", "Colunas"];
    private int _layout;
    /// <summary>0 = grade 3×2 (cartões largos), 1 = seis colunas lado a lado.</summary>
    public int Layout { get => _layout; set { if (Set(ref _layout, value)) { Raise(nameof(IsGrid)); Raise(nameof(IsColumns)); } } }
    public bool IsGrid => Layout == 0;
    public bool IsColumns => Layout == 1;

    public RelayCommand CloseCommand => new(() => _close());
    public RelayCommand SaveImageCommand => new(() => _ = _saveImage(this));
}

/// <summary>Um Pokémon da vitrine.</summary>
public sealed class ShowcaseMemberViewModel
{
    public ShowcaseMemberViewModel(PKM pk)
    {
        var shiny = pk.IsShiny;
        Sprite = SpriteService.GetSpeciesSprite(pk.Species, shiny, pk.Form, pk.Gender, pk.Context);
        var species = CoreAdapter.SpeciesNames[pk.Species];
        Name = pk.IsNicknamed ? pk.Nickname : species;
        Species = pk.IsNicknamed ? species : "";
        Level = $"Nv. {pk.CurrentLevel}";
        Gender = CoreAdapter.GetGenderSymbol(pk);
        IsShiny = shiny;
        Types = [.. CoreAdapter.GetTypes(pk).Select(t => new TypeChip(t.Name, t.Argb))];
        // Tera (Scarlet/Violet); o Stellar (99) fica de fora, porque não tem cor de tipo
        if (pk is ITeraTypeReadOnly tera && pk.Format >= 9 && (byte)tera.TeraType is var teraType && teraType < 18 && teraType < GameInfo.Strings.types.Length)
            Tera = new TypeChip(GameInfo.Strings.types[teraType], (uint)Drawing.PokeSprite.TypeColor.GetTypeSpriteColor(teraType).ToArgb());
        var abilities = CoreAdapter.AbilityNames;
        Ability = pk.Format >= 3 && pk.Ability > 0 && pk.Ability < abilities.Count ? abilities[pk.Ability] : "";
        Nature = pk.Format >= 3 ? CoreAdapter.NatureNames[(int)pk.Nature] : "";
        var items = CoreAdapter.GetItemNames(pk);
        Item = pk.HeldItem > 0 && pk.HeldItem < items.Count ? items[pk.HeldItem] : "";
        ItemSprite = pk.HeldItem > 0 ? SpriteService.GetItemSprite(pk.HeldItem, pk.Context) : null;
        var moves = new List<ShowcaseMove>();
        for (int i = 0; i < 4; i++)
        {
            var move = CoreAdapter.GetMove(pk, i);
            if (move == 0 || move >= CoreAdapter.MoveNames.Count) continue;
            var type = CoreAdapter.GetMoveType(move, pk.Context);
            moves.Add(new ShowcaseMove(CoreAdapter.MoveNames[move], new SolidColorBrush(type?.Argb ?? 0xFF6B7280)));
        }
        Moves = moves;
        var (t1, t2) = CoreAdapter.GetSpeciesTypes(pk.PersonalInfo);
        Glow = TypeGlowBrush(t1, t2);
    }

    public Avalonia.Media.Imaging.Bitmap? Sprite { get; }
    public string Name { get; }
    /// <summary>Espécie, quando o Pokémon tem apelido (senão vazio).</summary>
    public string Species { get; }
    public bool HasSpecies => Species.Length > 0;
    public string Level { get; }
    public string Gender { get; }
    public bool IsMale => Gender == "♂";
    public bool IsFemale => Gender == "♀";
    public bool IsShiny { get; }
    public IReadOnlyList<TypeChip> Types { get; }
    public TypeChip? Tera { get; }
    public bool HasTera => Tera is not null;
    public string Ability { get; }
    public bool HasAbility => Ability.Length > 0;
    public string Nature { get; }
    public bool HasNature => Nature.Length > 0;
    public string Item { get; }
    public bool HasItem => Item.Length > 0;
    public Avalonia.Media.Imaging.Bitmap? ItemSprite { get; }
    public IReadOnlyList<ShowcaseMove> Moves { get; }
    public IBrush Glow { get; }

    /// <summary>Mesmo brilho dos cartões das caixas, um pouco mais forte (a vitrine é para print).</summary>
    private static IBrush TypeGlowBrush(byte t1, byte t2)
    {
        static Color C(byte type, byte a)
        {
            var c = Color.FromUInt32((uint)Drawing.PokeSprite.TypeColor.GetTypeSpriteColor(type).ToArgb());
            return Color.FromArgb(a, c.R, c.G, c.B);
        }
        return new LinearGradientBrush
        {
            StartPoint = new(0, 0, Avalonia.RelativeUnit.Relative), EndPoint = new(1, 1, Avalonia.RelativeUnit.Relative),
            GradientStops = { new(C(t1, 0x80), 0), new(C(t1, 0x18), 0.55), new(C(t2, t1 == t2 ? (byte)0x18 : (byte)0x60), 1) },
        };
    }
}

/// <summary>Golpe da vitrine: nome e a cor do tipo.</summary>
public sealed record ShowcaseMove(string Name, IBrush Brush);
