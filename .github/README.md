<!-- Pagina principal do repositorio (o GitHub mostra .github/README.md antes do README.md da raiz, que e o do PKHeX original e fica intocado para os merges). -->

# PKHeX Modern

**Uma interface moderna para o [PKHeX](https://github.com/kwsch/PKHeX), o editor de saves de Pokémon, para Windows, Linux, macOS e Android.**

Feita em Avalonia, com tema escuro e claro, temas inspirados nos jogos e interface em português ou inglês. Por baixo continua o `PKHeX.Core` original, sem alterações: as regras de save, legalidade e encontros são as mesmas do PKHeX, e as atualizações do PKHeX oficial entram por merge.

[![Última versão](https://img.shields.io/github/v/release/carlosmozart/PKHeX-Modern?label=vers%C3%A3o&sort=semver)](https://github.com/carlosmozart/PKHeX-Modern/releases/latest)
[![Downloads](https://img.shields.io/github/downloads/carlosmozart/PKHeX-Modern/total?label=downloads)](https://github.com/carlosmozart/PKHeX-Modern/releases)

🌐 **Site do projeto: [carlosmozart.github.io/PKHeX-Modern](https://carlosmozart.github.io/PKHeX-Modern/)**

![Início](/PKHeX.Modern/docs/home.png)

## Download

O PKHeX original é só para Windows. O PKHeX Modern roda nos quatro sistemas, sem precisar instalar o .NET:

| Sistema | Pacote |
|---|---|
| Windows 10/11 (64 bits) | `PKHeX.Modern-win-x64.zip` |
| Linux x64 (também Steam Deck) | `PKHeX.Modern-linux-x64.zip` |
| macOS 14+ com Apple Silicon (M1, M2...) | `PKHeX.Modern-osx-arm64.zip` |
| macOS 14+ com Intel | `PKHeX.Modern-osx-x64.zip` |
| Android 6+ | `PKHeX.Modern-Android.apk` |

**[⬇ Baixar a versão mais recente](https://github.com/carlosmozart/PKHeX-Modern/releases/latest)**. Depois de instalado, o app avisa quando sai versão nova e se atualiza sozinho, inclusive no Android. Como instalar em cada sistema: [README completo](/PKHeX.Modern/README.md#download).

## Destaques

- **Modo legal**: o editor só oferece opções legais (golpes, bolas, habilidades, formas), desfaz mudanças que deixariam o Pokémon ilegal e legaliza Pokémon de fora ao trazer para o save.
- **Caixas, equipe e editor lado a lado**, com arrastar e soltar, desfazer/refazer, busca global (Ctrl+F) e verificação de legalidade do save inteiro.
- **Save Manager**: todos os saves da pasta com o selo de cada jogo, saves dentro de .zip (JKSV), backups automáticos antes de salvar e restauração.
- **Bank local** em bancos e caixas, pastas de `.pk*` como banco e um segundo save aberto para trocar Pokémon entre jogos, com conversão de geração.
- **Pokédex centralizada** juntando todos os saves e o bank, com living dex, shiny dex, formas e gêneros, um **planejador de Living Dex** que organiza as caixas do save com prévia e um único desfazer, e **completar a Pokédex do save** (vistas, capturadas, formas, gêneros e idiomas) em um clique.
- **Pesquisa em todos os saves e no bank**, com **auditoria de duplicados** (cópias idênticas, backups e anexados) e **Edição em lote** (inclusive legalizar vários de uma vez).
- **Banco de encontros e eventos** para gerar Pokémon legais a partir de encontros reais e Mystery Gifts.
- **Página Jogo**: flags e valores de evento com nome, recordes, atalhos de eventos (lendários de novo, itens, títulos), Hall da Fama editável, Battle Frontier, Pokétch e Pokéwalker, medalhas do Pokéathlon, base secreta e atalhos de **liberar tudo** (O-Powers, Super Training, Zygarde Cells, Underground).
- **Treinador completo**: nome, TID/SID, dinheiro e tempo de jogo, mais insígnias de cada região, gênero, moedas do Game Corner, BP e datas da aventura.
- **Caçada shiny**: contador com as chances reais de cada método do jogo (Masuda, Poké Radar, pesca em corrente, SOS, surtos e sanduíche...), encontros para 50/90/99%, onde o alvo aparece com aviso de shiny lock e dos golpes que estragam a caçada, e o "Achei!" lido do save.
- **Desafios e coleção**: **relatório de Nuzlocke** (primeiro encontro de cada rota, repetidos, vivos e mortos, pronto para copiar), **modo Hardcore** que desliga a edição e deixa só mover Pokémon (para desafios e RetroAchievements) e **fábrica de ovos**, que gera ovos legais de todas as espécies que faltam no save em menos de um segundo.
- **Evolução assistida**: uma lista com todas as evoluções da espécie no jogo (nível, item, troca, felicidade, beleza, golpe, local...), com o requisito, prévia e sem precisar de um segundo jogo.
- **Equipe Showdown e relatório CSV**: cole ou copie a equipe ou a caixa inteira no formato Showdown e exporte o save ou o bank para planilha.
- **Temas inspirados nos jogos** (PSS de X/Y, Pixel de GBA, Z-A, Pokédex de ORAS e Pokédex clássica de Kanto), oito cores de destaque e tamanho dos sprites ajustável.
- **Set do Showdown → Pokémon legal**, offline: cole um set (ou uma equipe) e receba Pokémon legais do seu jogo, mantendo IVs, habilidade, natureza, EVs, golpes e Tera Type sempre que possível.
- **Android** com a mesma interface do desktop, em paisagem: acha sozinho os saves do RetroArch, DraStic, melonDS, Dolphin, Azahar/Citra, Eden e outros emuladores, além de saves e pastas pelo seletor do sistema, gravando de volta no arquivo original.

![Caixas e editor](/PKHeX.Modern/docs/boxes.png)

| Golpes com busca | Pokédex centralizada |
|---|---|
| ![Golpes](/PKHeX.Modern/docs/moves.png) | ![Pokédex](/PKHeX.Modern/docs/pokedex.png) |
| **Save Manager** | **Tema Pixel (GBA)** |
| ![Save Manager](/PKHeX.Modern/docs/savemanager.png) | ![Tema Pixel](/PKHeX.Modern/docs/theme_pixel.png) |
| **Tema ORAS (Pokédex)** | **Tema Kanto no modo claro** |
| ![Tema ORAS](/PKHeX.Modern/docs/theme_oras.png) | ![Tema Kanto](/PKHeX.Modern/docs/theme_kanto_light.png) |
| **Treinador com insígnias** | **Banco de encontros** |
| ![Treinador](/PKHeX.Modern/docs/trainer.png) | ![Encontros](/PKHeX.Modern/docs/encounters.png) |
| **Set do Showdown legalizado** | **Saves dos emuladores (Android)** |
| ![Set do Showdown](/PKHeX.Modern/docs/showdown.png) | ![Saves dos emuladores](/PKHeX.Modern/docs/emulators.png) |
| **Relatório de Nuzlocke** | **Fábrica de ovos** |
| ![Nuzlocke](/PKHeX.Modern/docs/nuzlocke.png) | ![Fábrica de ovos](/PKHeX.Modern/docs/eggs.png) |
| **Caçada shiny** | |
| ![Caçada shiny](/PKHeX.Modern/docs/shinyhunt.png) | |

**No Android**, com a mesma interface:

![PKHeX Modern no Android](/PKHeX.Modern/docs/android.png)

Mais capturas, a lista completa de funções, como compilar e a arquitetura estão no **[README completo do PKHeX Modern](/PKHeX.Modern/README.md)**. As novidades de cada versão estão nas [Releases](https://github.com/carlosmozart/PKHeX-Modern/releases).

> **Faça backup dos seus saves antes de editar.** O app já guarda uma cópia antes de salvar por cima, mas um backup seu nunca é demais.

## English

**PKHeX Modern** is a modern UI for the [PKHeX](https://github.com/kwsch/PKHeX) Pokémon save editor, running on **Windows, Linux, macOS and Android** (the original PKHeX is Windows-only). It is built on top of the unmodified `PKHeX.Core`, so save handling and legality checks are the same as PKHeX. Features include a legal mode, drag and drop between boxes and party, a save manager, a local bank, a combined Pokédex, encounter and event databases, event flag editors, an editable Hall of Fame, Battle Frontier, Pokéathlon and secret base editors, trainer badges, one-click Pokédex completion, a Nuzlocke report, a Hardcore mode (move only, for challenges and RetroAchievements), an egg factory, a shiny hunt counter with real odds per method, game-inspired themes, legal Pokémon from pasted Showdown sets, automatic emulator save detection on Android and automatic updates. The interface is available in Portuguese and English (⚙ › Language). [Download the latest release](https://github.com/carlosmozart/PKHeX-Modern/releases/latest) or visit the [project site](https://carlosmozart.github.io/PKHeX-Modern/).

**macOS:** the app is free and not notarized by Apple. The first time, open it once, then go to **System Settings › Privacy & Security** and click **Open Anyway**. If macOS says the app “is damaged and can't be opened”, run `xattr -dr com.apple.quarantine "/Applications/PKHeX Modern.app"` in Terminal and open it again.

## Créditos

- **[PKHeX](https://github.com/kwsch/PKHeX)**, de Kaphotics e colaboradores: o `PKHeX.Core`, os sprites e toda a base de regras. O README original do PKHeX está [na raiz do repositório](/README.md).
- [AllGenWiki](https://allgenwiki.com/): descrições e locais de itens.
- Licença: [GPLv3](/LICENSE), a mesma do PKHeX.
