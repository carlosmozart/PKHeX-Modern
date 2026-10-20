using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Headless;
using Avalonia.Threading;
using Avalonia.VisualTree;
using PKHeX.Core;
using PKHeX.Modern;
using PKHeX.Modern.Controls;
using PKHeX.Modern.Services;
using PKHeX.Modern.Theme;
using PKHeX.Modern.ViewModels;
using PKHeX.Modern.Views;

AppBuilder.Configure<App>().UseSkia().UseHeadless(new AvaloniaHeadlessPlatformOptions { UseHeadlessDrawing = false }).WithInterFont().SetupWithoutStarting();
var work=args[0]; int fails=0;
BankStorage.Root=Path.Combine(work,"bank"); SaveBackup.Folder=Path.Combine(work,"backups");
void Check(string n,bool ok) { Console.WriteLine($"{(ok?"OK":"FAIL")} {n}"); if(!ok) fails++; }
void Pump() { Dispatcher.UIThread.RunJobs(); AvaloniaHeadlessPlatform.ForceRenderTimerTick(); }
var hashes=new System.Collections.Generic.HashSet<string>();
var originalTheme=AppTheme.Current;
foreach(var dark in new[]{true,false})
foreach(var theme in AppTheme.Presets)
{
    var bitmap=ThemePreview.Get(theme,dark); using var bytes=new MemoryStream(); bitmap.Save(bytes);
    var data=bytes.ToArray(); hashes.Add(Convert.ToHexString(SHA256.HashData(data)));
    Check($"miniatura {theme.Key} {(dark?"dark":"light")} renderizada",data.Length>1000 && bitmap.PixelSize==new PixelSize(232,126));
    Check("miniatura em cache",ReferenceEquals(bitmap,ThemePreview.Get(theme,dark)));
    bitmap.Save(Path.Combine(work,$"theme-{theme.Key}-{(dark?"dark":"light")}.png"));
}
Check("uma miniatura distinta por tema e variante",hashes.Count==AppTheme.Presets.Count*2);
Check("prévia não troca tema do app",ReferenceEquals(originalTheme,AppTheme.Current));
double Res(string k)=>(double)Application.Current!.Resources[k]!;
SpriteScale.Apply(3);Check("sprites enormes",Res("SlotSpriteSize")==126&&Res("DexSpriteSize")==72);
SpriteScale.Apply(0);Check("sprites pequenos",Res("CompactSpriteSize")==36);
SpriteScale.Apply(99);Check("tamanho fora da faixa vira o maior",Res("SlotSpriteSize")==126);
SpriteScale.Apply(1);Check("tamanho normal",Res("SlotSpriteSize")==84);
var sav=BlankSaveFile.Get(GameVersion.B); sav.OT="DEMO"; sav.SetBoxSlotAtIndex(new PK5{Species=25,Version=GameVersion.B,CurrentLevel=10},0,0);
var path=Path.Combine(work,"Black.sav"); File.WriteAllBytes(path,sav.Write().ToArray());
var settings=new AppSettings{CheckForUpdates=false}; var vm=new MainViewModel(settings); vm.Open(path); vm.CurrentPage=vm.Boxes;
var window=new MainWindow{DataContext=vm,Width=1440,Height=950}; window.Show(); Pump();
var image=window.GetVisualDescendants().OfType<Image>().Single(i=>i.Name=="BoxWallpaperImage");
Check("intensidade normal",image.Opacity==0.25);
vm.WallpaperIntensityIndex=1; Pump(); Check("intensidade suave",image.Opacity==0.10);
vm.WallpaperIntensityIndex=2; Pump(); Check("papel desligado",image.Opacity==0);
vm.WallpaperIntensityIndex=3; Pump();
var slots=window.GetVisualDescendants().OfType<ItemsControl>().Single(i=>i.Name=="BoxSlots");
var card=window.GetVisualDescendants().OfType<Button>().First(b=>b.Classes.Contains("slot")&&b.DataContext is SlotViewModel{IsEmpty:false});
Check("papel inteiro nítido e cartões translúcidos",image.Opacity==1 && slots.Classes.Contains("fullWallpaper") && card.Background is Avalonia.Media.ISolidColorBrush{Color.A:<255});
var glow=card.GetVisualDescendants().OfType<TypeGlow>().Single();
Check("brilho do tipo no cartão",glow.IsVisible && glow.Background is Avalonia.Media.LinearGradientBrush && glow.Margin.Left<0);
var emptyGlow=window.GetVisualDescendants().OfType<Button>().First(b=>b.Classes.Contains("slot")&&b.DataContext is SlotViewModel{IsEmpty:true}).GetVisualDescendants().OfType<TypeGlow>().Single();
Check("slot vazio sem brilho",emptyGlow.Background is null);
vm.SlotTypeColor=false; Pump(); Check("brilho desligado",!glow.IsVisible && !settings.SlotTypeColor);
vm.SlotTypeColor=true; Pump(); Check("brilho religado",glow.IsVisible);
vm.WallpaperIntensityIndex=2; Pump();
// Telas vazias: o aviso da caixa vazia aparece so na caixa sem Pokemon e nao bloqueia os slots
var hint=window.GetVisualDescendants().OfType<Border>().Single(b=>b.Name=="EmptyBoxHint");
Check("caixa com Pokémon sem aviso",!vm.Boxes.IsBoxEmpty && !hint.IsVisible);
vm.Boxes.CurrentBox=1; Pump();
hint=window.GetVisualDescendants().OfType<Border>().Single(b=>b.Name=="EmptyBoxHint");
Check("caixa vazia mostra o aviso",vm.Boxes.IsBoxEmpty && hint.IsVisible && !hint.IsHitTestVisible);
vm.Boxes.CurrentBox=0; Pump();
foreach(var kind in EmptyArt.Kinds)
{
    var art=new EmptyArt{Kind=kind,Width=120,Height=100,Stroke=Avalonia.Media.Brushes.White,Accent=Avalonia.Media.Brushes.Red,Glow=Avalonia.Media.Brushes.DarkRed};
    art.Measure(new Size(120,100)); art.Arrange(new Rect(0,0,120,100));
    using var rtb=new Avalonia.Media.Imaging.RenderTargetBitmap(new PixelSize(120,100)); rtb.Render(art);
    using var ms=new MemoryStream(); rtb.Save(ms);
    Check($"desenho {kind} ({ms.Length} bytes)",ms.Length>800);
}
// Destaque "Cor do jogo do save": segue a versão do save ativo, com o brilho no alto; as outras cores tiram o brilho
var gameOption=vm.AccentOptions.Single(o=>o.Preset.Key==AccentTheme.GameKey);
vm.SetAccentCommand.Execute(gameOption); Pump();
var expected=AccentTheme.ForGame(GameArt.Get(GameVersion.B).Color1);
Avalonia.Media.Color AccentNow(){ Application.Current!.TryGetResource("Accent",Application.Current.ActualThemeVariant,out var v); return ((Avalonia.Media.ISolidColorBrush)v!).Color; }
var variantDark=Application.Current!.ActualThemeVariant==Avalonia.Styling.ThemeVariant.Dark;
Check("cor do jogo vira o destaque",gameOption.IsSelected && settings.AccentColor=="game" && AccentNow()==(variantDark?expected.Dark:expected.Light));
Check("brilho do jogo no alto",Application.Current.Resources["GameTint"] is Avalonia.Media.LinearGradientBrush);
var white=AccentTheme.ForGame(0xFFF2F4F7); double L(Avalonia.Media.Color c)=>(0.2126*c.R+0.7152*c.G+0.0722*c.B)/255;
Check("cor clara escurece para dar leitura",L(white.Dark)<0.72 && L(white.Light)<0.5);
vm.SetAccentCommand.Execute(vm.AccentOptions[0]); Pump();
Check("outra cor tira o brilho",Application.Current.Resources["GameTint"] is Avalonia.Media.ISolidColorBrush{Color.A:0} && settings.AccentColor=="red");
var again=new MainViewModel(settings); Check("preferência ao reabrir",again.WallpaperIntensityIndex==2 && again.Boxes.WallpaperOpacity==0 && !settings.Persist);
Check("papel do jogo preservado",vm.ActiveTab!.Sav is IBoxDetailWallpaper wp && wp.GetBoxWallpaper(0)==((IBoxDetailWallpaper)sav).GetBoxWallpaper(0) && !vm.IsDirty);
var before=vm.ThemeOptions[0].Preview; vm.ToggleThemeCommand.Execute(null); Pump(); Check("miniatura acompanha claro/escuro",!ReferenceEquals(before,vm.ThemeOptions[0].Preview));
var prefs=window.GetVisualDescendants().OfType<Button>().Single(b=>b.Name=="PrefsButton"); prefs.Flyout!.ShowAt(prefs); Pump(); window.UpdateLayout(); for(int i=0;i<8;i++)Pump();
window.CaptureRenderedFrame()?.Save(Path.Combine(work,"theme-picker.png")); prefs.Flyout.Hide(); window.Close();
Console.WriteLine(fails==0?"TUDO OK":$"{fails} FALHAS"); return fails==0?0:1;
