using System.Collections.Generic;

namespace PKHeX.Modern.ViewModels;

public sealed partial class MainViewModel
{
    public IReadOnlyList<string> SpriteSizeOptions => Theme.SpriteScale.Names;
    public int SpriteSizeIndex
    {
        get => Theme.SpriteScale.Clamp(Settings.SpriteSize);
        set
        {
            if (value < 0 || value == SpriteSizeIndex) return;
            Settings.SpriteSize = Theme.SpriteScale.Clamp(value); Settings.Save();
            Theme.SpriteScale.Apply(Settings.SpriteSize);
            Raise(nameof(SpriteSizeIndex));
        }
    }

    // "Inteiro" fica no fim para não mudar o índice salvo das outras opções
    public IReadOnlyList<string> WallpaperIntensityOptions { get; } = ["Normal", "Suave", "Desligado", "Inteiro"];
    public int WallpaperIntensityIndex
    {
        get => System.Math.Clamp(Settings.WallpaperIntensity, 0, 3);
        set
        {
            if (value is < 0 or > 3 || value == WallpaperIntensityIndex) return;
            Settings.WallpaperIntensity = value; Settings.Save();
            Boxes.WallpaperIntensity = value;
            Raise(nameof(WallpaperIntensityIndex));
        }
    }

    /// <summary>Brilho da cor do tipo nos cartões de Pokémon.</summary>
    public bool SlotTypeColor
    {
        get => Settings.SlotTypeColor;
        set
        {
            if (value == Settings.SlotTypeColor) return;
            Settings.SlotTypeColor = value; Settings.Save();
            Theme.SlotTypeColor.Apply(value);
            Raise(nameof(SlotTypeColor));
        }
    }
}
