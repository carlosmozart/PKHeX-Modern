using System.IO;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media.Imaging;

namespace PKHeX.Modern.Views;

public partial class TeamShowcaseView : UserControl
{
    public TeamShowcaseView() => InitializeComponent();

    /// <summary>A vitrine (sem a barra de ações) como PNG, no dobro da resolução da tela.</summary>
    public byte[]? RenderPng()
    {
        var frame = this.FindControl<Border>("ShowcaseFrame");
        if (frame is null || frame.Bounds.Width <= 0 || frame.Bounds.Height <= 0)
            return null;
        const double scale = 2;
        var size = new PixelSize((int)(frame.Bounds.Width * scale), (int)(frame.Bounds.Height * scale));
        using var bitmap = new RenderTargetBitmap(size, new Vector(96 * scale, 96 * scale));
        bitmap.Render(frame);
        using var stream = new MemoryStream();
        bitmap.Save(stream);
        return stream.ToArray();
    }
}
