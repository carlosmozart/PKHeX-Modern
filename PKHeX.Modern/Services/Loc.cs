using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using Avalonia;
using Avalonia.Controls;

namespace PKHeX.Modern.Services;

/// <summary>
/// Idioma da interface. O app e escrito em portugues; em outro idioma, cada texto em portugues que aparece na tela e
/// trocado pela traducao do dicionario (Assets/lang/&lt;codigo&gt;.json, embutido). Textos com partes variaveis ("{0} gravado
/// em {1}") entram como modelos. Nomes do jogo (especies, golpes, itens) e textos de legalidade ficam como estao.
/// A troca vale ao reiniciar: os tratadores so sao instalados na partida.
/// </summary>
public static class Loc
{
    public const string Portuguese = "pt-BR";
    public const string English = "en";

    public const string Spanish = "es";

    /// <summary>
    /// Idiomas conhecidos, com os textos da troca ja no proprio idioma (a pergunta de reiniciar aparece no idioma
    /// escolhido, e nao no da tela). So entram no seletor os que tem dicionario embutido (<see cref="Languages"/>):
    /// basta acrescentar Assets/lang/es.json para o espanhol aparecer.
    /// </summary>
    private static readonly LanguageInfo[] Catalog =
    [
        new(Portuguese, "Português (Brasil)", "PT", "Idioma",
            "Reiniciar para usar Português?",
            "O idioma da interface muda ao reiniciar o app. Nomes do jogo (espécies, golpes, itens) continuam em inglês.",
            "A interface muda de idioma ao reiniciar o app.",
            "Reiniciar agora", "Depois",
            "Feche e abra o app de novo para usar Português.",
            "Idioma definido para Português: vale na próxima vez que abrir o app."),
        new(English, "English", "EN", "Language",
            "Restart to switch to English?",
            "The interface language changes when the app restarts. Game names (species, moves, items) stay in English.",
            "The interface language changes when the app restarts.",
            "Restart now", "Later",
            "Close and reopen the app to use English.",
            "Language set to English: it takes effect the next time you open the app.", GameCode: English),
        new(Spanish, "Español", "ES", "Idioma",
            "¿Reiniciar para usar Español?",
            "El idioma de la interfaz cambia al reiniciar la app. Los nombres del juego (especies, movimientos, objetos) también aparecen en español; puedes dejarlos en inglés en ⚙ › Idioma.",
            "El idioma de la interfaz cambia al reiniciar la app.",
            "Reiniciar ahora", "Más tarde",
            "Cierra y vuelve a abrir la app para usar Español.",
            "Idioma configurado en Español: se aplicará la próxima vez que abras la app.", GameCode: "es"),
    ];

    /// <summary>Idiomas oferecidos no seletor: o portugues e os que tem dicionario embutido.</summary>
    public static IReadOnlyList<LanguageInfo> Languages { get; } = [.. Catalog.Where(l => l.Code == Portuguese || HasDictionary(l.Code))];

    private static bool HasDictionary(string code) => typeof(Loc).Assembly.GetManifestResourceInfo($"lang.{code}.json") is not null;

    /// <summary>Dados do idioma (o portugues se o codigo nao for conhecido).</summary>
    public static LanguageInfo Find(string? code) => Catalog.FirstOrDefault(l => l.Code == code) ?? Catalog[0];

    /// <summary>
    /// Idioma para a primeira abertura, pelo idioma do sistema: o oferecido com o mesmo codigo ("es-MX" vale "es",
    /// "pt-PT" vale "pt-BR"), ou ingles quando o app nao tem o idioma do sistema.
    /// </summary>
    public static string Detect(System.Globalization.CultureInfo culture)
    {
        var exact = Languages.FirstOrDefault(l => string.Equals(l.Code, culture.Name, StringComparison.OrdinalIgnoreCase));
        if (exact is not null)
            return exact.Code;
        var two = culture.TwoLetterISOLanguageName;
        return Languages.FirstOrDefault(l => string.Equals(l.Code.Split('-')[0], two, StringComparison.OrdinalIgnoreCase))?.Code ?? English;
    }

    /// <summary>
    /// Idioma dos nomes do jogo no PKHeX.Core (especies, golpes, itens, legalidade) para a interface <paramref name="uiCode"/>:
    /// o mesmo idioma quando o Core o tem (espanhol), senao ingles (o Core nao tem portugues). <paramref name="english"/>
    /// mantem o ingles.
    /// </summary>
    public static string GameLanguageFor(string? uiCode, bool english) => english ? English : Find(uiCode).GameCode ?? English;

    /// <summary>O idioma tem nomes do jogo proprios no Core (a opcao "Nomes do jogo em ingles" so aparece nele).</summary>
    public static bool HasGameNames(string? uiCode) => Find(uiCode).GameCode is { } code && code != English;

    public static string Current { get; private set; } = Portuguese;
    public static bool IsTranslating => _exact.Count > 0;

    // Textos ja escritos no idioma certo (nomes dos idiomas, pergunta de reiniciar): passam sem traducao.
    private static readonly HashSet<string> _keep = new(StringComparer.Ordinal);

    /// <summary>Marca um texto para aparecer como esta, sem passar pelo dicionario.</summary>
    public static string Keep(string text)
    {
        _keep.Add(text);
        return text;
    }

    private static Dictionary<string, string> _exact = [];
    private static List<(Regex Pattern, string Template, string Prefix)> _patterns = [];
    private static readonly Dictionary<string, string> _cache = [];
    private static bool _hooked;

    /// <summary>Carrega o dicionario do idioma (portugues = sem traducao). Retorna false se nao houver dicionario.</summary>
    public static bool Load(string? code)
    {
        _exact = [];
        _patterns = [];
        _cache.Clear();
        Current = Portuguese;
        if (string.IsNullOrEmpty(code) || code == Portuguese)
            return true;
        try
        {
            using var stream = typeof(Loc).Assembly.GetManifestResourceStream($"lang.{code}.json");
            if (stream is null)
                return false;
            var map = JsonSerializer.Deserialize<Dictionary<string, string>>(stream) ?? [];
            LoadMap(map);
            Current = code;
            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>Usa um dicionario em memoria (testes).</summary>
    public static void LoadMap(IReadOnlyDictionary<string, string> map)
    {
        _exact = new(StringComparer.Ordinal);
        _patterns = [];
        _cache.Clear();
        foreach (var (pt, tr) in map)
        {
            if (string.IsNullOrEmpty(pt) || string.IsNullOrEmpty(tr) || pt.StartsWith("//", StringComparison.Ordinal))
                continue;
            if (Placeholder.IsMatch(pt))
            {
                // Modelo: o texto fixo vira literal e cada {n} captura qualquer coisa (o menor trecho possivel).
                var parts = PlaceholderSplit.Split(pt);
                var names = Placeholder.Matches(pt).Select(m => m.Groups[1].Value).ToList();
                var regex = "^" + string.Concat(parts.Select((p, i) => Regex.Escape(p) + (i < names.Count ? $"(?<p{names[i]}>.*?)" : ""))) + "$";
                _patterns.Add((new Regex(regex, RegexOptions.Singleline | RegexOptions.CultureInvariant), tr, parts[0]));
            }
            else
            {
                _exact[pt] = tr;
            }
        }
        // Pedacos que o codigo junta com espaco ou virgula na frente (" Salve o save.", ", sabendo golpe Fairy"):
        // tambem valem sozinhos, sem o separador.
        foreach (var (pt, tr) in map)
        {
            if (string.IsNullOrEmpty(pt) || string.IsNullOrEmpty(tr) || Placeholder.IsMatch(pt))
                continue;
            var key = pt.TrimStart(' ', ',', '.').Trim();
            if (key.Length > 0 && key != pt && !_exact.ContainsKey(key))
                _exact[key] = tr.TrimStart(' ', ',', '.').Trim();
        }
        // Modelos com mais texto fixo primeiro (mais especificos).
        _patterns = [.. _patterns.OrderByDescending(p => p.Pattern.ToString().Length)];
    }

    private static readonly Regex Placeholder = new(@"\{(\d+)\}", RegexOptions.Compiled);
    // Sem grupo de captura: Regex.Split com grupo poe o numero do marcador entre as partes.
    private static readonly Regex PlaceholderSplit = new(@"\{\d+\}", RegexOptions.Compiled);

    /// <summary>Traducao do texto (ou o proprio texto, se nao houver).</summary>
    public static string T(string? text)
    {
        if (string.IsNullOrEmpty(text) || _exact.Count == 0 || _keep.Contains(text))
            return text ?? "";
        if (_exact.TryGetValue(text, out var exact))
            return exact;
        if (_cache.TryGetValue(text, out var cached))
            return cached;
        var result = TranslateComposite(text);
        if (_cache.Count > 20000)
            _cache.Clear();
        _cache[text] = result;
        return result;
    }

    private static string TranslateComposite(string text)
    {
        if (TryPattern(text, out var viaPattern))
            return viaPattern;
        // Mensagens juntadas (ex.: "Feito. Lembre-se de salvar."): traduz cada linha e cada frase separadamente.
        if (text.Contains('\n'))
        {
            var lines = text.Split('\n');
            var translated = lines.Select(T).ToArray();
            return string.Join('\n', translated);
        }
        // Prefixos de icone ("✓ Texto", "⚠  Texto"): traduz o resto.
        int start = 0;
        while (start < text.Length && !char.IsLetterOrDigit(text[start]) && text[start] != '(' && text[start] != '"' && text[start] != '“')
            start++;
        if (start > 0 && start < text.Length)
        {
            var rest = text[start..];
            var t = _exact.TryGetValue(rest, out var e) ? e : TryPattern(rest, out var p) ? p : null;
            if (t is not null)
                return text[..start] + t;
        }
        // Partes separadas por " · " (cartoes: "#025 Pikachu · Possuida (2)", "PKHeX Modern · Versao 0.3.7").
        if (text.Contains(" · "))
        {
            var parts = text.Split(" · ").Select(T).ToArray();
            var joined = string.Join(" · ", parts);
            if (joined != text)
                return joined;
        }
        // Trechos separados por virgula (requisitos montados em partes: "subir 1 nivel, felicidade ≥ 220").
        if (text.Contains(", "))
        {
            var pieces = text.Split(", ");
            var translated = pieces.Select(p => _exact.TryGetValue(p, out var e) ? e : TryPattern(p, out var t) ? t : null).ToArray();
            if (translated.All(t => t is not null))
                return string.Join(", ", translated);
        }
        // Frases separadas por ". " (status com varias partes).
        var sentences = SplitSentences(text);
        if (sentences.Count > 1)
        {
            bool any = false;
            var parts = sentences.Select(s =>
            {
                var trimmed = s.Trim();
                var t = T(trimmed);
                // Tenta sem a pontuacao final (a chave costuma vir sem o ponto que o codigo acrescenta).
                if (t == trimmed && trimmed.Length > 1 && trimmed[^1] is '.' or '!' or '?')
                {
                    var bare = T(trimmed[..^1]);
                    if (bare != trimmed[..^1])
                        t = bare + trimmed[^1];
                }
                any |= t != trimmed;
                return s.StartsWith(' ') ? " " + t : t;
            }).ToList();
            if (any)
                return string.Concat(parts);
        }
        // Frase unica com ponto final que a chave nao tem.
        if (text.Length > 1 && text[^1] is '.' or '!' && TryPattern(text[..^1], out var noDot))
            return noDot + text[^1];
        if (text.Length > 1 && text[^1] is '.' or '!' && _exact.TryGetValue(text[..^1], out var exactNoDot))
            return exactNoDot + text[^1];
        return text;
    }

    private static bool TryPattern(string text, out string result)
    {
        foreach (var (pattern, template, prefix) in _patterns)
        {
            if (prefix.Length > 0 && !text.StartsWith(prefix, StringComparison.Ordinal))
                continue;
            var m = pattern.Match(text);
            if (!m.Success)
                continue;
            result = Placeholder.Replace(template, x =>
            {
                var g = m.Groups["p" + x.Groups[1].Value];
                return g.Success ? T(g.Value) : x.Value;
            });
            return true;
        }
        result = text;
        return false;
    }

    /// <summary>Quebra em frases mantendo o separador (". ", "! ", "? ") com a frase anterior.</summary>
    private static List<string> SplitSentences(string text)
    {
        var list = new List<string>();
        int last = 0;
        for (int i = 0; i < text.Length - 1; i++)
        {
            if (text[i] is '.' or '!' or '?' && text[i + 1] == ' ')
            {
                list.Add(text[last..(i + 1)]);
                last = i + 1;
            }
        }
        list.Add(text[last..]);
        return list;
    }

    /// <summary>
    /// Instala a traducao na tela: todo texto de TextBlock, dica (ToolTip) e texto de espera (Watermark/Placeholder)
    /// passa pelo dicionario. Usa SetCurrentValue, que nao desfaz as ligacoes (bindings) com os ViewModels.
    /// Campos de texto editaveis (TextBox.Text) nunca sao traduzidos.
    /// </summary>
    public static void Hook()
    {
        if (_hooked || !IsTranslating)
            return;
        _hooked = true;
        TextBlock.TextProperty.Changed.AddClassHandler<TextBlock>((tb, e) => Apply(tb, TextBlock.TextProperty, e.NewValue as string));
        ToolTip.TipProperty.Changed.AddClassHandler<Control>((c, e) =>
        {
            if (e.NewValue is string s)
                Apply(c, ToolTip.TipProperty, s);
        });
        TextBox.WatermarkProperty.Changed.AddClassHandler<TextBox>((tb, e) => Apply(tb, TextBox.WatermarkProperty, e.NewValue as string));
        ComboBox.PlaceholderTextProperty.Changed.AddClassHandler<ComboBox>((cb, e) => Apply(cb, ComboBox.PlaceholderTextProperty, e.NewValue as string));
        Window.TitleProperty.Changed.AddClassHandler<Window>((w, e) => Apply(w, Window.TitleProperty, e.NewValue as string));
    }

    private static void Apply<TValue>(AvaloniaObject target, AvaloniaProperty<TValue> property, string? value)
    {
        if (string.IsNullOrEmpty(value))
            return;
        var translated = T(value);
        if (!ReferenceEquals(translated, value) && translated != value)
            target.SetCurrentValue(property, (TValue)(object)translated);
    }

    /// <summary>Textos em portugues que aparecem na tela e nao estao no dicionario (para completar a traducao).</summary>
    public static IEnumerable<string> FindMissing(IEnumerable<string> texts)
        => texts.Where(t => !string.IsNullOrWhiteSpace(t) && T(t) == t && t.Any(char.IsLetter)).Distinct();
}

/// <summary>Um idioma da interface, com os textos da troca escritos nele mesmo.</summary>
/// <remarks><see cref="GameCode"/>: idioma dos nomes do jogo no PKHeX.Core, ou null quando o Core nao o tem (portugues).</remarks>
public sealed record LanguageInfo(string Code, string Name, string Short, string Word, string RestartTitle, string RestartBody,
    string RestartNote, string RestartNow, string Later, string Reopen, string Saved, string? GameCode = null);
