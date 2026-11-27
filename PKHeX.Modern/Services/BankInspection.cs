using System;
using PKHeX.Core;

namespace PKHeX.Modern.Services;

public static class BankInspection
{
    /// <summary>Synchronous UI callback; original Bank format; restores the active save context.</summary>
    public static (bool Valid, string Report) Analyze(PKM pokemon, SaveFile? activeSave)
    {
        var flags = ParseEraFlags.Capture();
        try
        {
            ParseSettings.ClearActiveTrainer();
            ParseSettings.AllowEraCartGB = pokemon.Format <= 2;
            var analysis = new LegalityAnalysis(pokemon);
            return (analysis.Valid, analysis.Report());
        }
        catch (Exception ex) { return (false, ex.Message); }
        finally
        {
            if (activeSave is null) ParseSettings.ClearActiveTrainer(); else ParseSettings.InitFromSaveFileData(activeSave);
            flags.Restore();
        }
    }
}
