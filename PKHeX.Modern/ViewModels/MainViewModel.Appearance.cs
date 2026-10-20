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
            Raise(nameof(GameMode));
        }
    }

    /// <summary>Prévia do arraste de slots (sprite seguindo o ponteiro, grupo e ação ao soltar).</summary>
    public DragPreviewViewModel DragPreview { get; } = new();

    /// <summary>Sobe a cada save aberto numa aba nova: a Pokébola abre no centro da tela.</summary>
    public int OpenToken { get; private set; }

    /// <summary>Animações curtas (Theme/Motion.cs); a View põe a classe noMotion quando desligadas.</summary>
    public bool Animations
    {
        get => Settings.Animations;
        set
        {
            if (value == Settings.Animations) return;
            Settings.Animations = value; Settings.Save();
            Theme.Motion.Enabled = value;
            Raise(nameof(Animations));
            Raise(nameof(GameMode));
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
            Raise(nameof(GameMode));
        }
    }

    /// <summary>
    /// “Modo jogo” (⚙): junta a cor do jogo do save, o papel de parede inteiro, a cor do tipo nos cartões e as
    /// animações num clique. Ligado = as quatro estão ligadas (mexer numa delas desliga o indicador). Desligar volta ao
    /// que estava antes de ligar (guardado em Settings.BeforeGameMode).
    /// </summary>
    public bool GameMode
    {
        get => Settings.AccentColor == Theme.AccentTheme.GameKey && Settings.WallpaperIntensity == 3 && Settings.SlotTypeColor && Settings.Animations;
        set
        {
            if (value == GameMode) return;
            if (value)
            {
                Settings.BeforeGameMode = new Services.AppSettings.LookSnapshot(Settings.AccentColor, Settings.WallpaperIntensity, Settings.SlotTypeColor, Settings.Animations);
                ApplyLook(Theme.AccentTheme.GameKey, 3, true, true);
                Status = "Modo jogo ligado: cores do jogo do save, papel de parede inteiro, cor dos tipos e animações.";
            }
            else
            {
                var before = Settings.BeforeGameMode;
                // Sem registro (ou se antes já estava tudo ligado), volta ao visual limpo: destaque do tema e papel de parede normal
                ApplyLook(before is { AccentColor: not Theme.AccentTheme.GameKey } ? before.AccentColor : Theme.AppTheme.Current.AccentKey,
                    before is { WallpaperIntensity: not 3 } ? before.WallpaperIntensity : 0, before?.SlotTypeColor ?? true, before?.Animations ?? true);
                Settings.BeforeGameMode = null;
                Status = "Modo jogo desligado: visual de antes.";
            }
            Settings.Save();
        }
    }

    private void ApplyLook(string? accent, int wallpaper, bool typeColor, bool animations)
    {
        var preset = accent == Theme.AccentTheme.GameKey ? Theme.AccentTheme.GameOption : Theme.AccentTheme.Find(accent);
        SetAccent(preset);
        WallpaperIntensityIndex = wallpaper;
        SlotTypeColor = typeColor;
        Animations = animations;
        Raise(nameof(GameMode));
    }

    public RelayCommand ToggleGameModeCommand => _toggleGameMode ??= new RelayCommand(() => GameMode = !GameMode);
    private RelayCommand? _toggleGameMode;
}
