# Tarefa: interface do PKHeX Modern em espanhol

Enunciado para o Claude de outro PC. Leia antes o `CLAUDE.md` da raiz, o `PKHeX.Modern/CONTINUAR.md` (seção “Idiomas (0.3.8)”) e os commits recentes do `modern-ui`.

## Objetivo

Criar `PKHeX.Modern/Assets/lang/es.json`, com a interface inteira em espanhol e a terminologia oficial dos jogos em espanhol, conferida na **Bulbapedia**. Depois, estender a suíte `Language` para conferir o espanhol.

## O que já está pronto (não refazer)

- **Detecção do idioma do sistema:** na primeira abertura, `Loc.Detect` usa o idioma do sistema. Com o `es.json` presente, um sistema `es-*` (es-ES, es-MX, es-AR...) abre em espanhol.
- **Seletor:** “Español” aparece sozinho no botão 🌐 do rodapé e no ⚙ › Idioma, porque `Loc.Languages` oferece todo idioma que tem dicionário embutido. O `.csproj` já embute `Assets\lang\*.json` como `lang.<código>.json`, então não há nada a registrar.
- **Textos da troca de idioma em espanhol:** ficam no `Catalog` de `PKHeX.Modern/Services/Loc.cs` (pergunta de reiniciar, “Reiniciar ahora”, “Más tarde”, aviso do Android e status). **Revise-os** com o mesmo glossário.
- **Fontes:** Inter, Fredoka, Pixelify Sans e Pokémon GB/GBC têm ñ, ¿, ¡ e todos os acentos do espanhol (conferido com fontTools). Não é preciso fonte nova.

## Como a tradução funciona (resumo do `Services/Loc.cs`)

- **Texto de origem:** a interface é escrita em português. A **chave** de cada entrada é o texto exato em português e o **valor** é a tradução.
- **Hoje existe só o inglês:** o `en.json`, com 2.591 entradas, 557 delas modelos com `{0}`, `{1}`.
- **Onde a troca acontece:** os tratadores trocam, na exibição, o texto de `TextBlock`, dicas, placeholders e títulos de janela. `TextBox.Text` nunca é traduzido.
- **Modelos:** em `"{0} gravado em {1}"`, cada `{n}` captura qualquer trecho e é traduzido de novo. **O valor precisa ter exatamente os mesmos marcadores** da chave, e a ordem pode mudar.
- **Textos compostos:** o `Loc.T` também traduz por partes: linhas (`\n`), frases (`. `), partes separadas por ` · ` e por vírgula, e prefixos de ícone (`✓ `, `⚠ `). Por isso o `en.json` tem pedaços soltos, que também precisam de tradução.
- **Textos que não passam pelo dicionário:** os marcados com `Loc.Keep` (nomes dos idiomas e a pergunta de reiniciar) aparecem como estão.

## Como gerar o `es.json`

1. **Mesmas chaves, mesma ordem do `en.json`.** Gere o arquivo com um script que percorre o `en.json` e grava `{"chave em PT": "valor em ES"}`:
   - UTF-8 sem BOM;
   - `ensure_ascii=False`;
   - indentação de 2 espaços.

   **Não altere o `en.json`:** ele tem indentação mista, e reformatá-lo gera um diff enorme.
2. **Traduza a partir da chave em português** (é o texto de origem) e use o valor em inglês como apoio para entender o contexto.
3. **Trabalhe em lotes** (por exemplo, de 200 em 200 entradas) e confira cada lote antes do próximo. Ao terminar, rode um script que confere:
   - todas as chaves do `en.json` presentes;
   - nenhum valor vazio;
   - os mesmos `{n}` em chave e valor;
   - as mesmas quebras `\n`;
   - os mesmos ícones e emojis no começo;
   - os mesmos atalhos (`Ctrl+K`, `F1`, `Esc`...).
4. **Contexto:** para entender onde um texto aparece, procure a chave em `PKHeX.Modern/**/*.axaml` e `*.cs`. Os textos longos da Ajuda vêm de `Services/HelpContent.cs`.

## Regras de tradução

- **Fonte da verdade:** a **Bulbapedia** (bulbapedia.bulbagarden.net). Cada página de conceito tem a seção “In other languages” com o termo oficial em espanhol. Consulte as páginas pela web, sem baixar dumps nem arquivos (o `CLAUDE.md` exige pedir permissão para downloads). Quando a Bulbapedia não tiver o termo, use o dos jogos mais recentes (Scarlet/Violet) e anote a dúvida no relatório.
- **Monte primeiro um glossário** com os termos recorrentes e siga-o em todo o arquivo. Grave-o em `PKHeX.Modern/Assets/lang/es-glossary.md` (PT → ES → página da Bulbapedia). Ponto de partida, **a confirmar** na Bulbapedia:

  | PT (interface) | ES (a confirmar) |
  |---|---|
  | Caixa / Caixas | Caja / Cajas |
  | Equipe | Equipo |
  | Mochila | Mochila |
  | Treinador | Entrenador |
  | Pokédex | Pokédex |
  | Encontros | Encuentros |
  | Eventos / Mystery Gift | Regalo Misterioso |
  | Item (segurado) | Objeto |
  | Golpe(s) | Movimiento(s) |
  | Habilidade | Habilidad |
  | Natureza | Naturaleza |
  | Shiny | Variocolor (o app usa “shiny” como termo; decida e siga o glossário) |
  | Ovo | Huevo |
  | Creche | Guardería / Pensión Pokémon (varia por geração) |
  | Insígnias | Medallas |
  | Hall da Fama | Hall de la Fama / Salón de la Fama |
  | EVs / IVs | EV / IV (os jogos usam “puntos de esfuerzo” e “valores individuales”) |
  | Atributos (HP, Atk...) | Características / Estadísticas |
  | Felicidade / Amizade | Amistad |
  | Tera | Teratipo |
  | Bola de captura | Poké Ball / Ball |

- **Nomes do jogo continuam em inglês.** Espécies, golpes, itens, habilidades, nomes de jogos e de lugares ficam em inglês (o app usa os nomes em inglês do PKHeX.Core). Se a chave em português mantém um nome em inglês (“Shiny Charm”, “Oval Charm”, “Battle Frontier”, “Pokétch”), mantenha-o também.
- **Glossário só para conceitos:** os termos oficiais servem para os conceitos genéricos citados no texto da interface (caixa, equipe, mochila, naturaleza...), não para os nomes próprios do jogo.
- **Espanhol neutro, tratando o usuário por “tú”,** como nos jogos. Evite regionalismos e “vosotros”. Use ¿ e ¡ corretamente.
- **Textos curtos:** o espanhol fica uns 20–30% maior que o inglês. Em botões, abas, itens da barra lateral, rótulos de seção (em maiúsculas, como `TEMA` → `TEMA`) e cartões, prefira a forma curta. Mantenha as maiúsculas e minúsculas e a pontuação final da chave.
- **Termos técnicos e de interface:** “save” vira “partida guardada” ou “archivo de guardado”; escolha uma forma no glossário e siga-a sempre. “Bank”, “Showdown”, “Living Dex”, “Nuzlocke”, “Hardcore”, “PKHeX” e “Ctrl+K” ficam como estão.
- **O que não se traduz:** o CHANGELOG continua em português (a Ajuda › Novidades mostra um aviso, que é chave do dicionário). As descrições do AllGenWiki também não são traduzidas.

## Testes

1. **Estenda a suíte `Language`** (`Tools/PKHeX.Modern.Tests/Language/Program.cs`), que hoje só confere o inglês, para rodar também com `es`. Por exemplo:
   - um argumento ou variável de ambiente com o código do idioma;
   - uma segunda entrada no `run-tests.ps1` (`'LanguageEs' = @{}` apontando para o mesmo projeto, ou o código como parâmetro).
2. **Adapte o detector de português (`pt`) para o espanhol.** Acentos como á, é, í, ó, ú e palavras como “de”, “para”, “que”, “buscar”, “vista” existem nos dois idiomas. Para o espanhol, conte como português que sobrou:
   - os caracteres exclusivos (ã, õ, ç, â, ê, ô);
   - as terminações “-ção”, “-ões”;
   - as palavras exclusivas do português: não, uma, em, ou, os, das, dos, caixa, equipe, golpe(s), treinador, salvar, abrir, clique, nenhum(a), selecione, escolha, arquivo, pasta, nova, voltar, excluir, ajuda, atualizar, você.
3. **Some à suíte:**
   - a verificação de chaves, marcadores e quebras descrita acima;
   - um caso de `Loc.Detect` com `es-MX` → `es` (o teste atual já espera isso quando o `es.json` existe).
4. **Confira na tela:** renderize sem janela, como faz a suíte `Language` ao salvar `boxes_en.png` e `help_en.png`, a barra lateral, as Caixas, o editor (todas as abas), o ⚙ e a Ajuda em espanhol, na janela pequena (1100×720) e no `MobileShell` (892×412). Procure textos cortados. **Nunca capture a tela do computador.**
5. **Rode a suíte inteira, sozinha**, antes de entregar.

## Depois do arquivo pronto

- **`CLAUDE.md`:** a regra “Toda função nova entra em três lugares” passa a incluir o `es.json` (cada texto novo também em espanhol, acrescentado no fim, sem reformatar).
- **`CONTINUAR.md`:** atualize a seção de idiomas.
- **`HelpContent.cs`:** a seção “Idioma da interface” passa a citar o espanhol, com a chave nova no `en.json` e no `es.json`.
- **`CHANGELOG.md`:** em `## Próxima versão` › `### 🌐 Idioma`, um item “**Interface em espanhol**”.
- **Revisão humana:** antes da release, vale pedir a alguém do público que pediu o espanhol para revisar (o autor recebe pedidos no X).

## Fora desta tarefa

- Nomes do jogo (espécies, golpes, itens) em espanhol: é uma decisão do autor, que ainda não foi tomada.
- Página de divulgação (`docs/index.html`) em espanhol e o chinês: só se o autor pedir.
- Commit, push, tag e release: só quando o autor pedir.

## Entrega

- **Arquivos:**
  - `PKHeX.Modern/Assets/lang/es.json`;
  - `PKHeX.Modern/Assets/lang/es-glossary.md`;
  - a suíte `Language` com o espanhol;
  - as anotações acima.
- **Relatório:** um resumo em `RELATORIO-TRADUCAO-ES.md` com:
  - o número de entradas;
  - os termos em que a Bulbapedia não ajudou e a escolha feita;
  - os textos que ficaram longos demais na tela;
  - o resultado da suíte.
