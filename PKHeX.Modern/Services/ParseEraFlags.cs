using System.Reflection;
using PKHeX.Core;

namespace PKHeX.Modern.Services;

/// <summary>
/// As tres opcoes de era do <see cref="ParseSettings"/> (cartucho GB, cartucho GBA, GBA do Switch). O Core so deixa grava-las
/// (o get e privado), entao a leitura e por reflexao. Antes o app guardava valores calculados a partir delas
/// (<c>AllowGBACrossTransferRSE</c>, <c>AllowGen3EventTicketsAll</c>), que nao voltam ao valor original: depois de conferir o Bank,
/// a legalidade do save aberto podia mudar. Desde o PKHeX de 21/11/2026, essas funcoes nem dependem mais das opcoes.
/// </summary>
public readonly record struct ParseEraFlags(bool CartGB, bool CartGBA, bool SwitchGBA)
{
    public static ParseEraFlags Capture() => new(Get(nameof(ParseSettings.AllowEraCartGB)), Get(nameof(ParseSettings.AllowEraCartGBA)), Get(nameof(ParseSettings.AllowEraSwitchGBA)));

    public void Restore()
    {
        ParseSettings.AllowEraCartGB = CartGB;
        ParseSettings.AllowEraCartGBA = CartGBA;
        ParseSettings.AllowEraSwitchGBA = SwitchGBA;
    }

    private static bool Get(string name) =>
        (bool)typeof(ParseSettings).GetProperty(name, BindingFlags.Public | BindingFlags.Static)!.GetValue(null)!;
}
