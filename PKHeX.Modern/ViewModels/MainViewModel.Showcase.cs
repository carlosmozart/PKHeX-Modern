using System;
using System.Linq;
using System.Threading.Tasks;
using PKHeX.Modern.Services;

namespace PKHeX.Modern.ViewModels;

public sealed partial class MainViewModel
{
    private TeamShowcaseViewModel? _showcase;
    /// <summary>Vitrine da equipe aberta por cima do app (null = fechada).</summary>
    public TeamShowcaseViewModel? Showcase { get => _showcase; private set { if (Set(ref _showcase, value)) Raise(nameof(HasShowcase)); } }
    public bool HasShowcase => Showcase is not null;
    /// <summary>Grava a imagem da vitrine (a view renderiza e pede o arquivo).</summary>
    public Func<TeamShowcaseViewModel, Task>? SaveShowcaseImage { get; set; }

    public void OpenShowcase()
    {
        if (_sav is null) return;
        var party = Enumerable.Range(0, CoreAdapter.GetPartyCount(_sav)).Select(i => CoreAdapter.GetPartySlot(_sav, i)).ToList();
        if (party.All(CoreAdapter.IsEmpty))
        {
            Status = "A equipe está vazia: não há o que mostrar na vitrine.";
            return;
        }
        Showcase = new TeamShowcaseViewModel(_sav, party, CloseShowcase, s => SaveShowcaseImage?.Invoke(s) ?? Task.CompletedTask);
    }

    public void CloseShowcase() => Showcase = null;
}
