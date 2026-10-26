using Android.App;
using Android.Content.PM;
using Avalonia;
using Avalonia.Android;
using Avalonia.Media;
using AndroidBuild = global::Android.OS.Build;

namespace PKHeX.Modern.Android;

[Activity(Label = "PKHeX Modern", Theme = "@style/MyTheme.NoActionBar", MainLauncher = true, Exported = true,
    // Paisagem: a interface e a mesma do desktop (barra lateral, paginas e editor lado a lado).
    ScreenOrientation = ScreenOrientation.SensorLandscape,
    ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize | ConfigChanges.UiMode
        // Controle ou teclado Bluetooth conectando nao reinicia o app (perderia a edicao em andamento).
        | ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden | ConfigChanges.Navigation)]
public sealed class MainActivity : AvaloniaMainActivity<App>
{
    protected override void OnCreate(global::Android.OS.Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
        App.MobileExit = Finish;
        PKHeX.Modern.Services.AutoUpdater.InstallApk = InstallApk;
        PKHeX.Modern.Services.DiagnosticReport.PlatformDescription = () =>
            $"Android {AndroidBuild.VERSION.Release} · API {(int)AndroidBuild.VERSION.SdkInt} · {AndroidBuild.Manufacturer} {AndroidBuild.Model}";
        PKHeX.Modern.Services.DiagnosticReport.ShareText = text =>
        {
            var intent = new global::Android.Content.Intent(global::Android.Content.Intent.ActionSend);
            intent.SetType("text/plain");
            intent.PutExtra(global::Android.Content.Intent.ExtraText, text);
            StartActivity(global::Android.Content.Intent.CreateChooser(intent, PKHeX.Modern.Services.Loc.T("Compartilhar diagnóstico")));
            return true;
        };
        EnterFullscreen();
        SetUpEmulatorSaves();
    }

    // Saves dos emuladores: a raiz da memoria e o "acesso a todos os arquivos" (Android 11+) ou a permissao comum.
    private void SetUpEmulatorSaves()
    {
        var root = global::Android.OS.Environment.ExternalStorageDirectory?.AbsolutePath;
        if (string.IsNullOrEmpty(root))
            return;
        PKHeX.Modern.Services.EmulatorSaves.StorageRoot = root;
        PKHeX.Modern.Services.EmulatorSaves.HasAccess = HasStorageAccess;
        PKHeX.Modern.Services.EmulatorSaves.RequestAccess = RequestStorageAccess;
    }

    private bool HasStorageAccess()
    {
        if (System.OperatingSystem.IsAndroidVersionAtLeast(30))
            return global::Android.OS.Environment.IsExternalStorageManager;
        return CheckSelfPermission(global::Android.Manifest.Permission.WriteExternalStorage) == Permission.Granted;
    }

    private void RequestStorageAccess()
    {
        try
        {
            if (System.OperatingSystem.IsAndroidVersionAtLeast(30))
            {
                var intent = new global::Android.Content.Intent(global::Android.Provider.Settings.ActionManageAppAllFilesAccessPermission,
                    global::Android.Net.Uri.Parse("package:" + PackageName));
                StartActivity(intent);
            }
            else
                RequestPermissions([global::Android.Manifest.Permission.ReadExternalStorage, global::Android.Manifest.Permission.WriteExternalStorage], 41);
        }
        catch (System.Exception ex)
        {
            // Alguns aparelhos nao tem a tela por app: abre a lista geral de "acesso a todos os arquivos".
            PKHeX.Modern.Services.CrashLog.Write(ex);
            if (System.OperatingSystem.IsAndroidVersionAtLeast(30))
                StartActivity(new global::Android.Content.Intent(global::Android.Provider.Settings.ActionManageAllFilesAccessPermission));
        }
    }

    // Voltou das configuracoes (ou do pedido de permissao): a pagina Saves rele com o acesso novo.
    protected override void OnResume()
    {
        base.OnResume();
        PKHeX.Modern.Services.EmulatorSaves.Resumed?.Invoke();
    }

    public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
    {
        base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        if (requestCode == 41)
            PKHeX.Modern.Services.EmulatorSaves.Resumed?.Invoke();
    }

    // Tela cheia (modo imersivo): sem barra de status e de navegacao; deslizar da borda mostra as duas por um tempo.
    // O recorte da camera continua de fora (modo padrao do Android em paisagem), entao nada fica embaixo dele.
    private void EnterFullscreen()
    {
        if (Window is not { } window)
            return;
        var controller = AndroidX.Core.View.WindowCompat.GetInsetsController(window, window.DecorView);
        controller.SystemBarsBehavior = AndroidX.Core.View.WindowInsetsControllerCompat.BehaviorShowTransientBarsBySwipe;
        controller.Hide(AndroidX.Core.View.WindowInsetsCompat.Type.SystemBars());
    }

    // Seletor de arquivos, instalador e outras janelas do sistema mostram as barras de novo: esconde ao voltar.
    public override void OnWindowFocusChanged(bool hasFocus)
    {
        base.OnWindowFocusChanged(hasFocus);
        if (hasFocus)
            EnterFullscreen();
    }

    // Simbolos e emoji que a Inter nao tem vem de fontes embutidas (Noto, OFL): a busca na fonte de emoji do sistema
    // desenhava "≣" no lugar de ♂/♀ e dos icones dos botoes.
    /// <summary>Abre o instalador do Android com o APK baixado (o sistema pede para permitir apps desta origem, se preciso).</summary>
    private bool InstallApk(string path)
    {
        try
        {
            var uri = AndroidX.Core.Content.FileProvider.GetUriForFile(this, PackageName + ".updates", new Java.IO.File(path));
            var intent = new global::Android.Content.Intent(global::Android.Content.Intent.ActionView);
            intent.SetDataAndType(uri, "application/vnd.android.package-archive");
            intent.AddFlags(global::Android.Content.ActivityFlags.GrantReadUriPermission | global::Android.Content.ActivityFlags.NewTask);
            StartActivity(intent);
            return true;
        }
        catch (System.Exception ex)
        {
            PKHeX.Modern.Services.CrashLog.Write(ex);
            return false;
        }
    }

    protected override AppBuilder CustomizeAppBuilder(AppBuilder builder) => base.CustomizeAppBuilder(builder).WithInterFont()
        .With(PKHeX.Modern.Theme.FontFallbacks.Options(android: true));
}
