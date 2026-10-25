# PKHeX Modern: instruções para o Claude

Este arquivo reúne o que o Claude sabia do projeto e do usuário (antes guardado só na memória da conta), para qualquer conta ou PC continuar do mesmo jeito. O estado detalhado e as armadilhas técnicas ficam em `PKHeX.Modern/CONTINUAR.md` e `PKHeX.Modern/ROADMAP.md`. **Leia os dois antes de trabalhar** e confira os commits recentes, porque o trabalho é feito em mais de um PC e em sessões diferentes.

## Projeto

- **O que é:** o PKHeX.Modern é uma interface Avalonia 11.3 (.NET 10) sobre o PKHeX.Core, para Windows, Linux, macOS e Android.
- **Autor:** Carlos Mozart (GitHub `carlosmozart`).
- **Repositório:** `carlosmozart/PKHeX-Modern`. Até 06/10/2026 se chamava `carlosmozart/PKHeX`, e o GitHub redireciona o nome antigo. O `upstream` é o `kwsch/PKHeX`.
- **Branch:** `modern-ui`, que é o branch padrão.
- **Releases:** tags `modern-vX.Y.Z`.
- **Caminho no disco:** neste PC, `E:\GitHub\pkhex`; no outro, `C:\Users\carlos.alcantara\Documents\GitHub\PKHeX`.
- **Divulgação:** o projeto é divulgado no X desde out/2026. Pese as funções também pelo apelo público (primeira impressão, README, capturas, multiplataforma), e não só pelo uso do autor.

## Regras do usuário (obrigatórias)

- **Idioma:** responda em português do Brasil. Os textos da interface também ficam em PT-BR. Nomes de jogos, Pokémon, itens, golpes e habilidades ficam em inglês.
- **Upstream intocado:** **nunca altere** `PKHeX.Core`, `PKHeX.WinForms`, `PKHeX.Drawing*` nem nenhum projeto do upstream. O WinForms serve só como referência.
- **Commit, push, tag e release só quando o usuário pedir.**
- **Toda função nova entra em três lugares:**
  - `PKHeX.Modern/Services/HelpContent.cs`, na seção do assunto;
  - `PKHeX.Modern/CHANGELOG.md`, em `## Próxima versão` (subseções com emoji, itens com o nome da função em negrito);
  - `PKHeX.Modern/Assets/lang/en.json` e `PKHeX.Modern/Assets/lang/es.json`, com a tradução de todo texto novo em inglês e em espanhol (termos do `es-glossary.md`).
- **Tradução no `en.json` e no `es.json`:** **acrescente as chaves no fim, sem reformatar o arquivo.** A indentação do `en.json` é mista, e um serializador gera um diff enorme. Os dois têm as mesmas chaves na mesma ordem; textos com valores usam `{0}`, `{1}`. Um texto igual em português e inglês (ex.: “Saves”) também entra no `en.json`, com o valor igual, para ter a versão em espanhol. As suítes `Language` e `LanguageEs` precisam terminar sem textos em português na interface em inglês e em espanhol.
- **Saves de teste:** os saves reais em `saves/` (fora do Git) são **somente leitura**. Copie antes de usar e **nunca** os mostre em capturas públicas.
- **Nunca capture a tela do computador** para conferir a interface. Uma vez a captura pegou o navegador do usuário. Para ver a interface, renderize sem janela com `Tools/PKHeX.Modern.Render` ou com um programa no mesmo estilo (Avalonia.Headless + Skia + `CaptureRenderedFrame`).
- **Testes:** suítes headless em `Tools/PKHeX.Modern.Tests/`, registradas no `run-tests.ps1`. Nelas, `SaveBackup.Folder` e `BankStorage.Root` apontam para uma pasta temporária. Antes de uma release, rode todas e compile o Android.
- **Downloads:** baixar qualquer coisa (fontes, pacotes, arquivos) exige pedir permissão antes.
- **Instalador:** o instalador (MSIX/Inno), o ícone próprio e a assinatura do exe estão **adiados**. Não proponha nada disso até o usuário tocar no assunto.
- **Keystore:** nunca peça nem manipule a keystore ou as senhas do Android. Elas estão nos secrets `ANDROID_*` do repositório. Perder a chave quebra as atualizações.

## Como soltar uma versão

1. Suba a versão em dois lugares:
   - `PKHeX.Modern.Version.props` (`<Version>X.Y.Z</Version>`);
   - `PKHeX.Modern.Android/PKHeX.Modern.Android.csproj` (`<ApplicationVersion>`, ex.: 0.4.18 → `41801`).
2. No `CHANGELOG.md`, troque `## Próxima versão` por `## X.Y.Z — AAAA-MM-DD`. No `CONTINUAR.md`, atualize a linha "Última release".
3. Faça o commit, crie a tag `modern-vX.Y.Z` e faça o push do branch e da tag. O workflow `modern-build.yml` gera os 4 zips de desktop na release.
4. Com o build pronto, publique as notas **em português e em inglês**, no formato da 0.4.29 e da 0.4.30 (veja com `gh release view modern-v0.4.30 -R carlosmozart/PKHeX-Modern`):
   - no topo, `🇧🇷 **Português** · 🇺🇸 [English below](#english)`;
   - em português: `## Novidades` (as subseções da versão no CHANGELOG), `## Como instalar` (a tabela com o arquivo de cada sistema e a frase de que o app se atualiza sozinho) e `### 🍎 macOS` (extrair e arrastar para Aplicativos, abrir uma vez, “Abrir Mesmo Assim” em Ajustes do Sistema › Privacidade e Segurança, e o `xattr -dr com.apple.quarantine "/Applications/PKHeX Modern.app"` para o aviso de “danificado”), mais o lembrete de backup;
   - depois de um `---` e de `<a id="english"></a>`, o mesmo em inglês (`## What's new`, `## How to install`, `### 🍎 macOS`);
   - no fim, o link "Full Changelog" (`compare/modern-vANTERIOR...modern-vX.Y.Z`).

   A parte de instalação muda pouco: copie da release anterior e só ajuste se algo mudou (sistemas, nomes dos arquivos, passos do Mac).
   `gh release edit modern-vX.Y.Z -R carlosmozart/PKHeX-Modern --title "PKHeX Modern X.Y.Z" --notes-file <notas> --latest`
5. Anexe o APK assinado, **só depois que o build da tag terminar** (o anexo precisa da release criada por ele; na 0.4.28 o APK ficou pronto antes e falhou com "release not found"):
   `gh workflow run modern-android.yml -R carlosmozart/PKHeX-Modern --ref modern-ui -f signed_release=true -f release_tag=modern-vX.Y.Z`
6. **Confira se a release tem os 5 arquivos:** `PKHeX.Modern-Android.apk` e os zips win-x64, linux-x64, osx-arm64 e osx-x64. Sem o APK, o celular não atualiza: aconteceu na 0.4.14. Desde a 0.4.15, o app oferece a release mais nova que tem o pacote do sistema.

## Sincronizar com o upstream (kwsch/PKHeX)

1. `git fetch upstream` e `git merge upstream/master` no `modern-ui`. O merge entra limpo, porque o fork não altera os projetos do upstream.
2. **Rode o `SpriteParity`** (`run-tests.ps1 SpriteParity`, só no Windows). O `PKHeX.Modern.Sprites` é um port próprio do `PKHeX.Drawing*` (`SpriteBuilder`, `SpriteUtil`, `WallpaperUtil`, `ImageUtil`), sem System.Drawing, e não acompanha o upstream sozinho. O teste compara o port com o original byte a byte e lista cada diferença (ex.: "papel de parede DP 22: box_wp23dp x box_wp23"). Se houver diferença, copie a mudança do upstream para o arquivo equivalente em `PKHeX.Modern.Sprites/` até ele voltar a dar "TUDO OK" e todos os nomes iguais. Na 0.4.23, o upstream mudou o `WallpaperUtil` (Colosseum/XD, Battle Revolution, sufixo dp, My Wallpaper do Pokémon Box) e foi preciso portar.
3. Vale também ao mexer no próprio port de sprites, em qualquer PC: rode o `SpriteParity` antes do commit.
4. Rode a suíte inteira **sozinha**: duas execuções ao mesmo tempo (ou junto de outro build) compilam os mesmos projetos em paralelo e dão falhas falsas como "No precompiled XAML found".
5. Anote no CHANGELOG, em `## Próxima versão`, uma seção "🔄 PKHeX atualizado" com o que muda para o usuário.

## Trabalho com o Codex

O usuário delega levas de funções ao Codex com enunciados `TAREFA-CODEX-FUNCOES-N.md` na raiz do repositório. Depois que a leva é revisada, juntada e publicada, o enunciado, a worktree e a branch são apagados (o histórico fica no Git e no CHANGELOG).
- **Como o Codex trabalha:** numa worktree e branch próprios (`modern-features-N`), sem push, merge ou release. Ao terminar, escreve um `RELATORIO-CODEX-FUNCOES-N.md`.
- **O papel do Claude:** revisar o branch, corrigir, juntar ao `modern-ui`, atualizar o `CONTINUAR.md` e publicar.
- **Formato do enunciado:** siga o das anteriores (regras do projeto, ordem das funções, testes obrigatórios, "fora desta tarefa", entrega).
- **Próxima tarefa:** nenhuma leva pendente; a sexta (auditoria de duplicados e aplicar a Living Dex) saiu na 0.4.19. A próxima leva será a sétima (`TAREFA-CODEX-FUNCOES-7.md`).

## Pendências em aberto (25/10/2026)

Última release: **0.5.1** (25/10/2026). Nenhuma tarefa pedida ficou pela metade; o que segue são ideias e conferências ainda não feitas.

- **No celular (testar no aparelho do usuário, tema ORAS + fonte Pokémon GB):** as telas da 0.4.26 e 0.4.27 (Início com status, barra lateral em grupos com ícones, resumo e fábrica de ovos da Pokédex, aba Nuzlocke, cartão e editor do modo Hardcore); também a tela cheia (0.4.18), o toque longo nos botões e o segurar-e-arrastar de slot (0.4.16); e os gestos da 0.5.0 (deslizar entre caixas sem atrapalhar o segurar-e-arrastar nem a rolagem, e puxar a lista de Saves para reler). Para conferir sem o aparelho, renderize o `MobileShell` em 892×412 (como o `Tools/PKHeX.Modern.Tests/Docs` faz com a captura `android.png`).
- **Ideias vindas do PKForge** (github.com/sofianeelhor/PKForge, concorrente no Android, analisado em 16/10/2026), em ordem de interesse:
  1. ~~Achar sozinho os saves dos emuladores no Android~~: feito na 0.4.28 (`Services/EmulatorSaves.cs`). **Falta conferir no aparelho**: o pedido do “acesso a todos os arquivos”, as pastas reais de cada emulador e a gravação de volta.
  2. ~~Auto Legality Mod~~: coberto pelo legalizador de sets próprio (`EncounterDatabase.LegalizeSet`, 0.4.28), sem dependência externa.
  3. **Pequenas:** travar caixa, natureza mantendo o shiny e conjuntos prontos na mochila saíram na 0.4.28; o QR com o set do Showdown ficou de lado (copiar e colar resolve).
  - Fora de escopo: ROM hacks (exigem um fork do Core), o "Poképark" e a segunda tela de portáteis duplos.
- **"✨ Caçada shiny": etapas 1 e 2 feitas** (saíram na 0.4.29). Etapa 1: contador, chances por método (`Services/ShinyOdds.cs`), ficha do alvo e "Achei!" pelo save. Etapa 2: fases (à mão ou oferecidas pelo "Achei!" quando aparece shiny de outra espécie), troféus com "Ir" para o Pokémon e CSV (`ShinyHuntData` em `shiny-hunt.json`; o arquivo da etapa 1 ainda abre). Ficaram de fora, por falta de valores seguros: Poké Radar do XY/BDSP, DexNav, Ultra Wormholes, Number Battled/Brilliant Aura e os bônus do Z-A; e o som da contagem (o Avalonia não tem áudio pronto). Também fora, de propósito: contador flutuante sobre o jogo, atalhos globais, Stream Deck e receitas de sanduíche.
- **macOS:** a 0.4.29 passou a assinar o bundle (ad-hoc) para tirar o aviso “danificado”. **Falta a confirmação de um usuário com Mac** (um usuário no macOS 26.6.2 reportou o problema). A notarização paga continua adiada.
- **Save de teste inválido:** `saves/legends Arceus.sav` (345.600 bytes) não é reconhecido pelo Core; trocar por um save real de Legends: Arceus para cobrir o editor desse jogo.
- **0.5.0: identidade visual própria** (pedido em 18/10/2026): os 17 itens do `PKHeX.Modern/ROADMAP.md` saíram na 0.5.0. Ideias seguintes, vindas da análise do dataDex (app de consulta para Android, 22/10/2026): análise de tipos da equipe (fraquezas, resistências e cobertura dos golpes na página Equipe), botão de Pokébola flutuante no celular abrindo a paleta e “quem aprende este golpe” no editor. Divulgação da 0.5.0 (vídeo, capturas e texto do X) em `divulgacao/0.5.0/`, fora do Git.
- **Idiomas:** a etapa 1 (idioma do sistema na primeira abertura e botão 🌐 no rodapé) e a interface em espanhol (`es.json`, glossário em `es-glossary.md`, suíte `LanguageEs`; relatório em `RELATORIO-TRADUCAO-ES.md`) saíram na 0.5.1. Ninguém nativo revisou o `es.json` ainda: as correções do público hispano entram nas próximas versões. Os nomes do jogo seguem o idioma da interface quando o PKHeX.Core o tem (`LanguageInfo.GameCode`: só o espanhol, `es`), com a opção “Nomes do jogo em inglês” (`AppSettings.GameNamesInEnglish`); o Showdown exporta sempre em inglês. Falta o chinês simplificado, que precisa de fonte com CJK; o `es-419` (espanhol latino-americano do Core) pode virar uma opção depois.
- **Editores de nicho** (candidatos à 7ª leva do Codex): arranjo das decorações da base secreta, ranks por espécie do Battle Hall e do Battle Castle.
- **Capturas:** o projeto `Tools/PKHeX.Modern.Tests/Docs` regenera todas as capturas do README e do site (inclusive `android.png`, temas ORAS/Kanto, Nuzlocke, fábrica de ovos e Hardcore): `dotnet run -c Release --project Tools/PKHeX.Modern.Tests/Docs -- PKHeX.Modern/docs`.
- **Página de divulgação:** `docs/index.html`, publicada em https://allgenwiki.com/pkhex/ (domínio da All Gen Wiki) pelo Worker `allgenwiki-pkhex` de `Tools/Site` (rota `allgenwiki.com/pkhex*`, serve a própria pasta `docs/`; depois de mudar a página, rode `npx wrangler deploy` em `Tools/Site`). O GitHub Pages da pasta `/docs` do `modern-ui` continua no ar (https://carlosmozart.github.io/PKHeX-Modern/), com o canonical apontando para o domínio. As imagens vêm de `PKHeX.Modern/docs` pelo raw do GitHub (capturas headless com saves sintéticos, nunca saves reais) e os downloads usam `releases/latest/download/<pacote>`, então a página não precisa mudar a cada release. Idiomas: português no HTML, inglês e espanhol nos dicionários `EN` e `ES` do script (mesmas chaves `data-i18n`; legendas das capturas com `data-es`), escolhidos por `?lang=pt|en|es`, pela escolha salva ou pelo idioma do navegador (outros idiomas abrem em inglês). **O espanhol está pronto, mas desligado** (`ES_READY = false` no script: sem o botão ES, sem detecção pelo navegador, só a prévia por `?lang=es`); ligue quando o app tiver o `es.json` numa release, junto com o `og:locale:alternate` es_ES e o `hreflang="es"` no `<head>`, e tire a frase “la traducción al español está en camino” do `dl.sub` e da legenda do inglês. Ao mudar um texto da página, mude nos três. No fim da página, a seção “Conheça o All Gen Wiki” leva a `https://allgenwiki.com/sobre/?lang=pt|en` (espanhol vai para o inglês) e mostra a captura `pokedex.png` da própria página Sobre (`img/` em português, `img/en/` em inglês). Ao criar uma função visível, considere uma captura nova e um cartão na página. A imagem de compartilhamento (og:image/twitter:image, 1200×630) é `PKHeX.Modern/docs/social.png`, gerada de `Tools/SocialCard/card.html` com o Edge sem janela (comando no topo do arquivo); refaça-a quando a captura das Caixas ou o visual mudarem bastante.
