# Novidades do PKHeX Modern

<!--
Este arquivo vai embutido no executável (Ajuda › Novidades).
Ao adicionar uma função, anote em "Próxima versão"; na release, troque o título pela versão e a data.
Formato: "## versão — data", "### seção", "- item".
-->

## 0.5.0 — 2026-10-23

### 🎨 Interface
- **Títulos com personalidade:** os títulos grandes (páginas, nome do save, Pokémon no editor, logo e vitrine) passaram a usar a Fredoka, uma fonte arredondada; o texto continua na Inter. ⚙ › Fonte › Inter volta tudo para a Inter.
- **Ícones de jogo:** a barra lateral e os atalhos do Início trocaram os ícones genéricos por desenhos próprios de objetos dos jogos, coloridos: Pokébola, caixa do PC, o PC do Bank, a Pokédex, o boné do treinador, a mochila, o mato alto, o cartucho e o portátil.
- **Barra lateral de Pokédex:** o topo virou a tampa de uma Pokédex, com a faixa na cor de destaque (ou na cor do jogo do save), a lente azul e as três luzes; a linha acima das páginas ganhou o botão da Pokébola. Desenhos próprios, sem arte oficial.
- **Arrastar com prévia:** o sprite do Pokémon segue o ponteiro (no celular, acima do dedo) com o que vai acontecer ao soltar (Mover, ⇄ Trocar, Copiar, Sobrescrever, Trocar de caixa) e quantos vão juntos na seleção múltipla; a origem fica apagada até soltar.
- **Modo jogo:** no topo do ⚙, uma chave liga de uma vez a cor do jogo do save, o papel de parede inteiro, a cor do tipo nos cartões e as animações. Desligar volta ao visual de antes; também pela paleta (Ctrl+K).
- **Cor do jogo do save:** nova cor de destaque (a bolinha em arco-íris no ⚙) que segue a versão aberta, com um brilho suave da cor do jogo no alto da tela. Troca junto com a aba; cores muito claras ou escuras são ajustadas para os botões continuarem legíveis.
- **Animações curtas:** a Pokébola abre ao carregar um save, o slot dá um pulinho ao receber um Pokémon, o editor solta um brilho ✦ quando o Pokémon fica legal ou vira shiny, e a ★ dos shiny nas caixas pisca de vez em quando. Todas são leves e não bloqueiam cliques; ⚙ › “Animações” desliga.
- **Linha do tempo do save no Início:** a cada abertura, o Início mostra o que mudou desde a vez anterior: horas de jogo, Pokémon e espécies novas, shinies (com o sprite), insígnias, Pokédex e dinheiro. Separa o que veio do jogo do que foi salvo no app; na primeira vez, usa o backup mais recente como ponto de partida.
- **Hexágono de atributos no estilo SwSh/SV:** na aba Atributos, a forma na cor do tipo, os EVs como segunda camada dourada e o valor de cada atributo junto do nome, em vermelho ou azul conforme a natureza.
- **Ficha do Pokémon no editor:** o topo do editor ganhou o brilho da cor dos tipos, o Tera (Scarlet/Violet), o nível com a natureza, a bola de captura e o selo do jogo de origem com a geração. Acompanha as edições na hora.
- **Telas vazias ilustradas:** pasta de saves vazia, nenhum backup, caixa vazia, nenhum outro save no Bank, Pesquisa sem resultado e Hall da Fama vazio ganharam um desenho próprio e a dica do que fazer. O aviso da caixa vazia não bloqueia o arraste nem o clique nos slots.
- **Paleta de comandos (Ctrl+K):** digite “legalizar”, “caixa 5”, “Pikachu” ou “exportar” e Enter executa: ações, páginas, caixas pelo número ou nome e Pokémon do save pela espécie ou apelido (abre no editor). Sem diferença de maiúsculas e acentos; no celular, pelo ⌘ ao lado da busca.
- **Vitrine da equipe:** na página Equipe, “🖼 Vitrine” mostra os seis Pokémon grandes, com tipos, Tera, item, habilidade, natureza e golpes coloridos pelo tipo, em Grade (3×2) ou Colunas. “Salvar imagem” grava um PNG em alta resolução para compartilhar.
- **Cor do tipo nos cartões:** cada Pokémon das caixas, da equipe, do bank e do outro save tem o fundo do cartão tingido com a cor dos tipos dele (o primeiro no alto, o segundo embaixo). Ovos e slots vazios ficam neutros. Dá para desligar em ⚙ › “Cor do tipo nos cartões”.
- **Papel de parede “Inteiro”:** nova opção em ⚙ › Papel de parede das caixas que mostra o papel de parede do jogo nítido, como no próprio jogo, com os cartões translúcidos por cima. O padrão continua o Normal.

### 📱 Android
- **Gestos:** deslize o dedo para o lado nas caixas para trocar de caixa (na página Caixas e nos três painéis do Bank) e puxe a lista da página Saves para baixo para reler a pasta. Segurar o dedo continua marcando e arrastando.

### 🔄 PKHeX atualizado
- **PKHeX de 21/10/2026:** a leitura de sets do Showdown ficou mais rigorosa. Linhas de EVs/IVs com valor inválido, o mesmo atributo repetido (ex.: “30 Atk / 29 Atk”) ou dois atributos marcados com o mesmo + ou − passam a ser apontados como erro em vez de o app adivinhar o valor. No FireRed/LeafGreen, o Meteorite passa a ficar entre os itens-chave da mochila. Por dentro, o gerador de encontros do PKHeX passou a ignorar as versões que ele ainda não trata (a proteção do app para isso continua).

## 0.4.30 — 2026-10-19

### 🔄 PKHeX atualizado
- **PKHeX de 18/10/2026:** transferências do FireRed/LeafGreen do Virtual Console para o HOME passam a ser reconhecidas, com mais verificações de golpes de ovo; na Gen 3 e 4, os EVs são conferidos contra a experiência ganha; o Greninja do WCS 2026 deixou de ser tratado como evento expirado; flags dos bolsos de itens do Legends: Z-A.

### 🎨 Interface
- **Lista inteira na espécie e no item:** no editor (e nas espécies da Caçada shiny), tocar ou clicar na caixa mostra a lista completa, com o nome atual selecionado; digitar substitui o nome e filtra por qualquer parte dele (“chu” acha Pichu, Pikachu e Raichu). Antes, a lista só aparecia depois da primeira letra.

### 🐛 Correções
- **Formas no editor:** trocar a forma (ex.: Meowth para Alola ou Galar) deixava a caixa de forma em branco, porque a troca também mudava as listas de bola e habilidade. Agora só a caixa da lista que mudou se atualiza.
- **Legalizar depois da atualização do PKHeX:** as versões novas que o PKHeX acabou de listar (Pokémon Winds e Waves) ainda não têm encontros e quebravam a busca: trocar a forma no modo legal dava “erro (context None)”. Elas ficam de fora até o PKHeX tratá-las.
- **App travado ao trazer Pokémon de geração mais nova para a Gen 3:** com a atualização do PKHeX, levar um Pokémon da Gen 7 (ou mais nova) para um save de FireRed, Emerald ou outro da Gen 3 (pelo Bank, pelo outro save ou importando arquivo) entrava num laço sem fim. Agora o app recusa na hora, como antes (“sem rota de transferência”).
- **Evoluir no Let's Go e no Legends: Arceus:** o PKHeX novo confere a altura e o peso guardados, e evoluir (ou trocar a espécie ou a forma) deixava o Pokémon ilegal (“Calculated Height/Weight does not match”). Agora os dois são recalculados junto.
- **Celular:** a barra “Selecionar / Mover” das Caixas ficava por baixo das abas dos saves; agora fica logo abaixo delas.

## 0.4.29 — 2026-10-17

### ✨ Caçada shiny
- **Nova página Caçada shiny** (no fim da barra lateral): contador com +1, Espaço ou +, desfazer e somar vários; chance por encontro de cada método do jogo aberto (Masuda, Poké Radar, pesca em corrente, Friend Safari, SOS, Catch Combo, Dynamax Adventure, surtos e sanduíche do SV...), chance de já ter achado, encontros para 50/90/99% com a curva, tempo caçando e ritmo por hora. Cada método mostra a fonte; os de valor duvidoso aparecem como “a conferir”.
- **Lê o save:** o Shiny Charm vem marcado se estiver na mochila, e ao abrir um save com um shiny novo da espécie da caçada o app oferece concluir, com o local e a data de captura.
- **Fases e troféus:** “Registrar fase” guarda um shiny que não era o alvo sem zerar a caçada (um shiny novo de outra espécie no save também é oferecido como fase); a caçada concluída vira troféu com jogo, método, encontros, fases, data e “Ir” para o Pokémon no save; “Exportar CSV” salva tudo numa planilha.
- **Onde encontrar:** locais e níveis do alvo no jogo, aviso de shiny lock e os golpes do selvagem que podem estragar a caçada (Explosion, Self-Destruct, Roar, Whirlwind, Teleport, Perish Song, recuo...), com o que fazer.

### 🐛 Correções
- **macOS “danificado”:** o app do macOS dizia que estava “danificado e não pode ser aberto”, porque o bundle saía sem assinatura. Agora o pacote é assinado (ad-hoc) e conferido no build; o macOS só pede para liberar o app em Ajustes › Privacidade e Segurança. Quem baixou uma versão anterior pode rodar `xattr -dr com.apple.quarantine "/Applications/PKHeX Modern.app"`.
- **Raids de Sword/Shield:** editar uma raid (por exemplo, desativar uma raid de evento) travava e fechava o app. A tela e o editor entravam num laço sem fim; agora cada campo ignora o valor que já está lá e a lista de tipos é fixa. O mesmo cuidado vale para as seeds das raids de Scarlet/Violet.
- **Editor congelando (memórias):** com um Pokémon da Gen 6/7 que tem memórias, o editor podia congelar a interface depois de uma alteração (visto ao trocar o encontro de um Pokémon do Alpha Sapphire). As listas de sentimento e de detalhe da memória eram recriadas a cada atualização e entravam num laço com o modo legal; agora ficam fixas.

## 0.4.28 — 2026-10-16

### 📱 Android
- **Saves dos emuladores:** o botão “🎮 Saves dos emuladores” da página Saves acha sozinho os saves do RetroArch, DraStic, melonDS, Dolphin, Azahar/Citra/Lime3DS, Eden/Yuzu, mGBA, My Boy! e Pizza Boy, e os que ficam ao lado das ROMs. Pede uma vez o “acesso a todos os arquivos” e grava no arquivo original, com backup.

### ✨ Legalidade
- **Set do Showdown legal:** no modo legal, colar um set ilegal gera um Pokémon legal deste jogo mantendo IVs, habilidade, natureza, gênero, shiny, EVs, golpes e Tera Type sempre que possível (Hyper Training, mentas e cápsulas quando o jogo permite), e avisa o que não deu para manter.
- **Golpes de TM/TR no Legalizar:** em Sword/Shield e Scarlet/Violet, o Legalizar marca o registro de TM/TR, então golpes de TM do Pokémon não são mais trocados por outros.

### 📦 Caixas
- **Travar caixa:** em “⋯ Caixa”, a caixa fica protegida contra arrastar, excluir, aplicar, colar, edição em lote, fábrica de ovos e Living Dex até destravar; “Ordenar todas as caixas” pula as travadas. Cadeado na aba e trava salva por save.

### ✏️ Editor
- **Natureza mantendo o shiny (Gen 3/4):** trocar a natureza de um shiny não tira mais o shiny; no modo legal, o Pokémon é gerado de novo com a natureza nova, legal e shiny (antes a mudança era desfeita).

### 🎒 Mochila
- **Encher bolso:** conjuntos prontos com todos os TMs, todas as Poké Balls, todos os remédios, todas as berries ou tudo no bolso aberto, com 99 de cada e em ordem numérica.

## 0.4.27 — 2026-10-16

### 🎨 Interface
- **Ícones vetoriais:** a barra lateral e os atalhos do Início usam ícones de traço desenhados pelo app, na cor do texto (e na cor de destaque na página atual), no lugar dos emojis, que mudavam de tamanho e de estilo em cada sistema, principalmente no Android.
- **Barra lateral em grupos:** Pokémon (Caixas, Equipe, Bank, Pokédex), Treinador (Treinador, Mochila), Obter (Encontros, Eventos) e Ferramentas (Saves, Jogo, Pesquisa, Edição em lote). A ordem e os atalhos Ctrl+1 a Ctrl+9 não mudaram.
- **Status do save no Início:** abaixo do cartão do jogo, alterações não salvas (clique para ver e salvar), último backup (clique para abrir os backups), vagas livres nas caixas e o arquivo.
- **Resumo da coleção na Pokédex:** sem espécie escolhida, o painel da direita mostra barras de Living dex, Shiny dex, capturadas e vistas, quantas faltam em cada geração (clique para listar só as que faltam) e o botão “Próxima que falta”; “‹ Resumo” volta para ele.

### 🥚 Fábrica de ovos
- **Ovos das que faltam:** no resumo da Pokédex, um botão gera um ovo legal para cada espécie que falta no save aberto (da forma básica: Charmander para o Charizard, Pichu para o Pikachu); outro gera um ovo de cada família. Vêm dos ovos de criação do próprio jogo, com o seu treinador, e vão para as vagas livres das caixas, com a lista antes de gravar e um único Ctrl+Z. Funciona da Gen 2 à Gen 9 (num save de Black, 595 ovos em menos de um segundo).

### 🔒 Desafios
- **Modo Hardcore:** no ⚙, “Modo Hardcore (só mover Pokémon)” desliga toda edição de dados, para desafios como o Nuzlocke e para quem joga com RetroAchievements. O editor só mostra os dados; criar, copiar, importar, colar Showdown, legalizar, gerar encontros e eventos e completar a Pokédex ficam desligados, e Treinador, Mochila, Jogo e Edição em lote saem da barra lateral. Mover, trocar, soltar, ordenar e usar o Bank continuam. Um cartão vermelho no lugar do modo legal mostra que está ligado.
- **Relatório de Nuzlocke:** nova aba na Pokédex com o primeiro Pokémon capturado em cada rota, as capturas a mais, os repetidos (família já capturada antes), quem está na equipe, vivo ou morto (pela caixa do cemitério, escolhida pelo nome) e um botão para copiar o relatório em texto. Trocas, outros jogos e ovos ficam de fora; da Gen 4 em diante a ordem segue a data de captura.

### 🐛 Correções
- **Pokédex com fontes largas:** o nome da espécie no painel encolhe em vez de quebrar no meio ("Ivysa/ur"), e "Nenhum Pokémon desta espécie guardado" não é mais cortado.

## 0.4.26 — 2026-10-15

### 📱 Celular e telas pequenas
- **Nome no editor:** com fontes largas (como Pokémon GB), o nome do Pokémon encolhe para caber e o gênero ♂/♀ e a estrela de shiny ficam sempre visíveis; o local ("equipe, posição 1") encurta com "…".
- **Cartões da Equipe:** nomes longos (Blastoise, Charizard) encolhem em vez de serem cortados.
- **Início:** os cartões de uma mesma linha têm a mesma altura.
- **Barra lateral:** todas as páginas com a mesma altura (os emojis do Android tinham alturas diferentes e desalinhavam a lista), e uma linha separa o "Início" da lista quando ela rola.
- **Caixas e Pokédex:** as abas das caixas ganharam folga das abas dos saves, e o painel de detalhes da Pokédex fica mais estreito em telas menores, para os filtros não cortarem.

## 0.4.25 — 2026-10-15

### 🎨 Interface
- **Caixas mais compactas:** o cabeçalho da caixa ficou numa linha só (‹ nome · contagem · ⋯ Caixa ›) e Renomear, papel de parede, Showdown e CSV foram para o menu **⋯ Caixa**. Em notebooks (1280×720) aparecem quase duas linhas de slots a mais de uma e meia; em janelas de 1100 px, quatro colunas em vez de três.
- **Painel do editor adaptável:** 360 px em janelas abaixo de 1400 px (410 px nas maiores), e a faixa da equipe mostra só os sprites quando falta espaço, em vez de nomes cortados.
- **Mochila com nomes de bolso em português:** Itens, Itens-chave, TMs e HMs, Remédios, Frutas... (antes apareciam nomes internos, como "KeyItems" e "TMHMs").
- **Edição em lote:** Pré-visualizar e Aplicar ficam fixos embaixo da coluna, sempre visíveis.
- **Salvar ▾:** "Salvar como..." saiu da barra lateral e virou a seta ao lado do Salvar, no rodapé; "Abrir save..." só fica em destaque quando nenhum save está aberto.
- **Um botão principal por tela:** o "Usar" dos cartões de Encontros e Eventos ficou neutro (o painel de detalhes tem "Usar este encontro"), e o rodapé não repete mais as ações que já ficam no topo da página (Atualizar, Sincronizar, Usar encontro, Atualizar anexados, Aplicar mochila).
- **Excluir com cara de ação destrutiva:** contorno vermelho em vez de fundo, para não se confundir com o Salvar.
- **Mesmo vocabulário em todo o app:** "Aplicar" leva a mudança ao save aberto e "Salvar" grava o arquivo ("alterações não salvas", "Lembre-se de salvar", "Aplicar mochila"); "exportar" ficou só para arquivos .pk*.
- **Mochila mais limpa:** mostra os itens que você tem e algumas vagas livres para adicionar, em vez de dezenas de "(nenhum)"; "Mostrar todas as vagas" mostra o bolso inteiro.
- **Detalhes:** atalhos da barra lateral só aparecem ao passar o mouse ou na página atual; filtros de Encontros dizem "Todos os encontros" e "Todas as versões"; os atalhos Jogo e Edição em lote do Início ganharam descrição; a Ajuda tem texto com mais contraste e linhas mais curtas.

### 🐛 Correções
- **Ajuda na barra lateral:** com a Ajuda aberta, a página anterior não fica mais marcada; o botão Ajuda é que fica em destaque.

## 0.4.24 — 2026-10-14

### ♟ Treinador
- **Insígnias, gênero, moedas, BP e datas:** a página Treinador agora edita as insígnias de cada região (Gen 1 a 6, Sword/Shield e BD/SP, com “Todas” e “Nenhuma”), o gênero do treinador, as moedas do Game Corner (Gen 1 a 4), os Battle Points (Emerald, Gen 4 a 7 e Sword/Shield) e as datas de início da aventura e do Hall da Fama (Gen 4 e 5).

### 📖 Pokédex
- **Completar a Pokédex do save:** novo botão na página Pokédex para marcar todas as espécies como vistas e capturadas no save aberto (com formas, gêneros e idiomas onde o jogo guarda, e shiny vistos se quiser), marcar só como vistas, tirar as capturadas ou limpar tudo. Da Gen 1 ao Z-A (menos Legends: Arceus), com Ctrl+Z.

### ⚡ Atalhos
- **Liberar tudo:** novos atalhos na página Jogo para o que é chato de farmar: todos os O-Powers e todos os treinos do Super Training (X/Y e Omega Ruby/Alpha Sapphire), todas as Zygarde Cells (Sun/Moon e Ultra Sun/Ultra Moon), itens do Grand Underground no máximo (BD/SP) e a bolsa de esferas do Underground cheia (Diamond/Pearl/Platinum).

## 0.4.23 — 2026-10-13

### 🔄 PKHeX atualizado
- **Correções do PKHeX (kwsch, até 13/10):** saves de HeartGold/SoulSilver guardam a versão certa ao abrir; Dratini com ExtremeSpeed por ovo em HeartGold/SoulSilver deixa de sair ilegal; o PSS de X/Y lê todos os contatos; traduções atualizadas.
- **Papéis de parede que faltavam:** Ruby/Sapphire e FireRed/LeafGreen com os 16, Emerald com os 17 (os de Walda), os nomes certos de cada um, as caixas de Colosseum/XD e de Battle Revolution com o papel de parede de cada caixa e o "My Wallpaper" desenhado no Pokémon Box.

## 0.4.22 — 2026-10-10

### 🎨 Aparência
- **Tema ORAS (Pokédex):** azul-esverdeado inspirado na Pokédex e no PokéNav Plus de Omega Ruby/Alpha Sapphire, com a cor de destaque Água.
- **Tema Kanto (Pokédex clássica):** vermelho e creme da Pokédex de Red/Blue.

### ⚙ Preferências
- **Tamanho dos sprites:** Pequeno, Normal, Grande ou Enorme nas caixas, no bank, no outro save e na Pokédex, pelo ⚙.
- **Legalizar ao clicar opcional:** a opção “Oferecer legalizar ao clicar num ilegal” no ⚙ desliga a pergunta; o Pokémon só abre no editor e o botão Legalizar continua disponível.

### 🌐 Projeto
- **Site do projeto:** [carlosmozart.github.io/PKHeX-Modern](https://carlosmozart.github.io/PKHeX-Modern/), com as funções, capturas e downloads de cada sistema.

## 0.4.21 — 2026-10-08

### 🎮 Jogo
- **Battle Frontier:** nova aba em Emerald, Platinum e HeartGold/SoulSilver (e a Battle Tower de Diamond/Pearl), com BP, Frontier Pass e símbolos de prata/ouro (Emerald), impressões de cada instalação (Gen 4) e as sequências de cada instalação, modo e nível, inclusive trocas da Factory, CP do Castle e a série em andamento.
- **Pokétch e Pokéwalker:** liberar os aplicativos do Pokétch e escolher o da tela (Diamond/Pearl/Platinum); watts, passos e rotas do Pokéwalker (HeartGold/SoulSilver).
- **Medalhas do Pokéathlon:** as cinco medalhas de cada uma das 493 espécies, com busca, filtro de incompletas e “Todas as medalhas”.
- **Base secreta:** em Ruby/Sapphire/Emerald, edita o dono de cada base e mostra a equipe dela; em Omega Ruby/Alpha Sapphire, edita textos, nível e bandeiras da sua base, dá todas as decorações e apaga bases de outros jogadores.

## 0.4.20 — 2026-10-07

### 📖 Pokédex
- **Só o save aberto:** um botão na Pokédex mostra só o que o save aberto viu, capturou e tem guardado, sem misturar com os outros saves nem com o bank.

### 🎒 Mochila
- **Todos os TMs:** um botão põe todos os TMs do jogo no bolso de TMs, com 99 de cada e em ordem numérica, sem tirar o que já estava lá.

### 🐛 Correções
- **Roamers da Gen 3:** Entei, Raikou e Suicune de FireRed/LeafGreen e Latias/Latios de Ruby/Sapphire gerados em Encontros vinham sempre com IVs zerados. Agora os IVs são sorteados como no jogo, que por um bug só guarda PS 0-31 e Ataque 0-7.
- **Pokédex em janelas pequenas:** os botões, a busca, os filtros e as gerações se ajustam à largura, sem cortar o título nem o campo de busca.

## 0.4.19 — 2026-10-06

### 🔎 Pesquisa
- **Auditoria de duplicados:** cópias idênticas por dados armazenados, agrupamento de backups por par de arquivos (ZIP ou pastas de saves com muitos Pokémon iguais) e anexados/variantes registrados no Bank, com comparação e navegação somente leitura. Leitura cancelável, incluindo alterações não salvas.

### 📖 Pokédex
- **Aplicar Living Dex:** prévia da organização nas caixas do save aberto, com primeira caixa, reserva de faltantes, cópias externas, preservação de ocupantes e slots protegidos, confirmação e um único desfazer. Origens alteradas exigem nova prévia.

## 0.4.18 — 2026-10-06

### 📱 Android
- **Tela cheia:** o app esconde a barra de status e a de navegação, ganhando espaço na tela. Deslizar da borda mostra as barras por um momento; o recorte da câmera continua livre.

### 🔗 Projeto
- **Novo endereço:** o repositório agora se chama [PKHeX-Modern](https://github.com/carlosmozart/PKHeX-Modern). Os links antigos continuam funcionando (o GitHub redireciona) e a atualização automática segue normal.

## 0.4.17 — 2026-10-05

### ✏️ Editor
- **Evolução assistida:** Evoluir reúne os métodos e ramificações do jogo aberto, com sprite, requisitos e prévia cancelável. Remove a Everstone, cumpre os requisitos do Pokémon e deixa o resultado pendente até Aplicar, com desfazer e proteção do modo legal.

### 📦 Caixas e equipe
- **Relatório CSV:** exporta equipe e caixas do save aberto ou todas as caixas do banco escolhido, com dados, IVs, EVs, golpes e legalidade, sem alterar os originais. Cabeçalhos PT/EN, UTF-8 com BOM e seletor de arquivos também no Android.
- **Equipe Showdown:** o botão “📋 Showdown” em Equipe e em Caixas cola vários sets de uma vez nas vagas da equipe ou nos slots vazios da caixa, com uma lista de conferência antes (legal, ilegal ou fora, com o motivo) e um único desfazer. Também copia a equipe ou a caixa inteira no formato Showdown.

### 📖 Coleção
- **Living Dex:** o plano usa o número de slots por caixa do jogo alvo (20 na Gen 1/2, em vez de sempre 30), e os demais Pokémon da mesma espécie aparecem como “outros exemplares”, sem sugerir que são cópias.

### 🔧 Correções
- **Pesquisa:** lê uma cópia dos saves abertos (não disputa o save com o editor enquanto busca) e não cria mais bancos e caixas vazios no Bank.

## 0.4.16 — 2026-10-05

### ⚙️ Desempenho e diagnóstico
- **Editor:** estado, relatório, avisos e grupos de legalidade compartilham uma análise por atualização.
- **Diagnóstico da pasta:** as contagens passam como dados, sem depender do idioma do resumo e sem incluir nomes de arquivos.

### 💾 Backups
- **Motivo e integridade:** cópias novas registram operação, jogo, tamanho e SHA-256. A lista avisa quando o arquivo mudou e Restaurar permite conferir antes de prosseguir. Backups antigos continuam disponíveis; os registros acompanham a limpeza das cópias, também no armazenamento privado Android.

### 🏦 Bank
- **Consulta somente leitura:** toque num Pokémon para ver seus dados, sprite shiny, atributos, golpes, origem e legalidade. Exporte, copie Showdown, consulte variantes ou abra uma cópia no editor de um save compatível, preservando o original.

### 🎨 Preferências
- **Prévia dos temas:** os quatro temas aparecem em cartões com miniaturas reais das caixas e do cabeçalho do editor, renderizadas pelo Avalonia e guardadas em cache para claro e escuro.
- **Papel de parede das caixas:** escolha Normal, Suave ou Desligado para facilitar a leitura dos slots. A preferência é preservada e não altera o papel de parede do jogo.

### 📱 Toque no Android
- **Seleção por toque:** caixas e Bank têm o modo “☐ Selecionar”, também aberto por toque longo, com contagem e ações para o grupo. Sair do modo limpa as marcas.
- **Dicas sem mouse:** toque e segure um botão para ver a descrição dele sem executá-lo; os cartões de tema e o modo de arraste têm ⓘ. O arraste por toque só começa depois de manter o dedo parado; listas usam inércia e indicador de rolagem visível.

### 📖 Coleção
- **Planejador de Living Dex:** nova seção na Pokédex, com jogo alvo, formas, shiny e fontes. Mostra contagem, melhores candidatos, duplicados, faltantes e um plano nacional de 30 slots por caixa. A leitura tem progresso; nenhum Pokémon é movido e nenhum save é gravado.

### 📋 Diagnóstico
- **Compartilhar diagnóstico:** Ajuda › Sobre mostra uma prévia com versão, sistema, idioma, tema e registro recente. Android abre o compartilhamento do sistema; desktop copia o texto. Mensagens livres e dados privados são omitidos.

### ↔ Pokémon
- **Legalidade por assunto:** os motivos aparecem em grupos por gravidade, com contagem e primeiro inválido aberto. Golpes, Pokébola, Fitas e Encontro têm ações de correção ligadas ao problema (“Pokébola legal” escolhe uma que combina com a cor do Pokémon, como na Edição em lote).
- **Prévia de alterações:** Legalizar mostra os campos antes e depois, com destaque para mudanças importantes e o encontro de onde o Pokémon foi gerado. A confirmação usa o candidato exibido, sem gerar outro. Transferências entre gerações só pedem confirmação quando mudam algo importante (shiny, PID, IVs, natureza, treinador, apelido, Pokébola ou golpes); lotes mostram um resumo por campo e a lista completa.

### ⚙ Edição em lote
- **Legalizar em lote:** nova ação rápida “Legalizar”, que gera de novo, a partir de um encontro real do jogo, os Pokémon que estiverem ilegais (depois das outras ações), mantendo natureza, nível, item, apelido e golpes quando possível. A pré-visualização marca os legalizados (✨) e conta os que não deram; “Aplicar” grava exatamente o que foi pré-visualizado.
- “Bola legal” passou a se chamar **“Pokébola legal”**.

## 0.4.15 — 2026-10-05

### ⬆ Atualização
- **Animação da atualização:** enquanto a versão nova baixa, uma fileira de Pokébolas cinzas ganha cor (Poké, Great, Ultra... até a Master Ball nos 100%) e a equipe do save aberto anda em fila sobre a barra — de vez em quando um deles aparece shiny. “Continuar usando” esconde o painel e o download segue. Para ver sem precisar de versão nova: Ajuda › Sobre › “✨ Ver animação”.
- **Atualizar estando algumas versões atrás:** se a versão mais nova ainda não tiver o pacote do seu sistema (o APK do Android sobe alguns minutos depois), o app oferece a mais recente que já tem, em vez de dar erro.

### 💾 Backups
- **Backups separados por save:** saves de pastas diferentes com o mesmo nome (como o “main” do Switch) agora têm cada um o seu limite de 20 backups; antes um podia apagar os backups do outro.

### 📱 Android
- Conectar um controle ou teclado Bluetooth não reinicia mais o app (nem perde a edição em andamento).

## 0.4.14 — 2026-10-04

### ✨ Legalidade
- **Legalizar ao clicar:** ao clicar num Pokémon ilegal das caixas ou da equipe, o app pergunta se você quer legalizá-lo. “Legalizar” gera o Pokémon de novo a partir de um encontro real e já grava no slot (Ctrl+Z desfaz); “Só abrir” abre no editor como antes e não pergunta de novo sobre esse Pokémon até fechar o app.

### 🎮 Página Jogo
- **Roupas de Scarlet/Violet:** catálogo por categoria e gênero, busca por categoria em inglês ou ID, liberação e bloqueio individuais. Liberar todas ou uma categoria pede confirmação e pode ser desfeito. O Core não fornece nomes comerciais das peças.
- **Donuts de Z-A:** lista, edição de frutas, estrelas e poderes, gerador por efeito, duplicação e exclusão. Encher a bolsa pede confirmação e tem um passo de desfazer.
- **Errantes:** editor da Gen 3 e X/Y com reaparecer, dados guardados e desfazer. O modo legal valida encontros e a correlação PID/IV; Ruby/Sapphire/FireRed/LeafGreen mostram os IVs truncados pelo bug do jogo.
- **Relógio da Gen 3:** aba para Ruby/Sapphire/Emerald com relógios inicial e decorrido, correção do relógio parado pelo método do PKHeX e avanço de um dia. Cada edição pode ser desfeita.
- **Desfazer e refazer:** flags, valores, recordes, atalhos, Hall da Fama, cartões e raids passam a ter histórico próprio nos botões ↶/↷ da página Jogo. A dica informa a ação; operações em massa formam um passo. Até 30 passos e 64 MB de diferenças, sem reverter edições de outras páginas.

## 0.4.13 — 2026-10-04

### ✏️ Editor
- **Pokérus** (aba Extras): sem Pokérus, infectado ou curado, “Dar Pokérus”, cepa e dias. O modo legal só permite onde o Pokérus é possível.
- **Hidden Power** (aba Atributos, Gen 2–7): tipo e poder; escolher o tipo ajusta os IVs, e o modo legal recusa quando o Pokémon ficaria ilegal. O golpe mostra o tipo calculado.
- **QR da Gen 7**: botão no editor gera o QR para o QR Scanner do Sun/Moon, com “Salvar imagem”. Cartões da Gen 6/7 também geram QR no formato do PKHeX.

### 📦 Caixas e equipe
- **Caixas em pasta** (botão ⋯ na página Caixas): exportar esta caixa ou todas como arquivos .pk* (com subpasta por caixa, se quiser) e importar uma pasta para as caixas, com conversão de geração, oferta de legalizar o lote, lista do que foi recusado e um único Ctrl+Z.
- **Creche** (abaixo da equipe): editar os Pokémon da creche, pôr e tirar (com desfazer), ovo esperando, experiência e semente conforme o jogo. Inclui as duas creches de Omega Ruby/Alpha Sapphire e Sword/Shield.

### 🎮 Página Jogo
- **Cartões de evento do save**: os Mystery Gifts que esperam ser recebidos no jogo. Importar, exportar, apagar, marcar como não recebido e “Adicionar ao save” a partir da página Eventos, com as regras de cada geração (Gen 4 a 7 e Let's Go).
- **Raids**: tocas de Sword/Shield (Galar, Isle of Armor, Crown Tundra) e Tera Raids de Scarlet/Violet (Paldea, Kitakami, Blueberry), com ativa, tipo, estrelas, seed e registros de 7 estrelas; “Ativar todas” e “Desativar todas” pedem confirmação.

### 📱 Android
- A pasta de saves funciona com todos os saves da pasta escolhida. Se você escolheu a pasta antes da 0.4.10, toque em “Escolher pasta...” e escolha de novo (essas versões não guardavam a escolha).

## 0.4.12 — 2026-10-03

### 📱 Android
- **Pasta de saves**: depois de escolher a pasta, tocar em Atualizar ou abrir o app, a barra de status mostra um resumo da leitura (quantos arquivos a pasta tem, quantos foram lidos como save, quantos foram ignorados e o primeiro erro, se houver). Um arquivo ou subpasta com erro agora é pulado sem interromper a leitura dos outros, e o erro vai para o crash.log. Isso ajuda a investigar por que alguns saves da pasta ainda não aparecem.

## 0.4.11 — 2026-10-03

### 🔧 Preparação
- A atualização automática passa a aceitar também o nome futuro do repositório (`carlosmozart/PKHeX-Modern`). Assim, quando o projeto for renomeado no GitHub, quem estiver nesta versão ou mais nova continua recebendo atualizações sozinho.

### ⚠️ Problema conhecido
- No Android, a pasta de saves ainda não lista todos os saves da pasta escolhida. Está sendo investigado; enquanto isso, abra o save que faltar pelo botão Abrir.

## 0.4.10 — 2026-10-03

### 🎮 Editores por jogo
- **Hall da Fama editável** (aba ★ da página Jogo): clique num Pokémon para trocar espécie, apelido e nível (no Sun/Moon, só a espécie); no X/Y e no Omega Ruby/Alpha Sapphire também a data da vitória e o shiny. Cada vitória ganhou “Usar equipe atual” (troca pelos Pokémon da equipe do save) e “Apagar”, e “Registrar equipe atual” adiciona a equipe do save como vitória nova, como ao vencer a Liga. A aba aparece mesmo sem nenhuma vitória ainda.

### 📱 Android
- **Pasta de saves no Android**: na página Saves, “Escolher pasta...” agora abre o seletor do Android e o app lê a pasta inteira, com subpastas, listando todos os saves como no desktop (antes, só apareciam os saves abertos um por um). “Atualizar” e a abertura do app trazem saves novos ou mudados e tiram os que sumiram. Salvar grava de volta no arquivo da pasta, com backup; um save aberto ou com alterações não salvas não é trocado pela versão da pasta. O botão “Abrir pasta” não aparece mais no Android.

## 0.4.9 — 2026-10-03

### 📱 Android
- **Bank em pasta externa no Android**: em Bank › Adicionar pasta, escolha uma pasta do celular com arquivos .pk* (de outro app ou sincronizada). Ela aparece como 📁 no Bank, como no desktop. O app sincroniza: ao abrir, traz o que mudou na pasta, e cada Pokémon gravado ou apagado no Bank vai para a pasta na hora. Se um arquivo mudou nos dois lados, fica a versão da pasta e a do app vai para os backups.
- **Atualização automática no Android**: ao abrir, o app avisa quando sai versão nova e mostra as novidades. Em “Atualizar agora”, baixa o APK da release, confere o arquivo (SHA-256) e abre o instalador do Android, que instala por cima sem perder nada. Na primeira vez, o Android pede para permitir instalar apps do PKHeX Modern. Dá para desligar em Ajuda › Sobre.
- Os links da Ajuda (releases, código, AllGenWiki) agora abrem no navegador do celular.
- O item Início da barra lateral também não mostra mais o atalho Ctrl+0.

## 0.4.8 — 2026-10-03

### 📱 Android
- Os símbolos ♂/♀ e os ícones dos botões (Criar, Importar, Excluir, Exportar...) apareciam como "≣" no Android. Agora o app traz as fontes desses símbolos (Noto Sans Symbols 2 e Noto Emoji), sem depender das fontes do celular.
- Voltaram a funcionar no Android: clicar num resultado da busca, abrir um save pelo Save Manager, o painel do Bank, "Atualizar anexados", "Voltar" na Ajuda e a cor de destaque.
- A lista de páginas da barra lateral não deixa mais uma linha desenhada por cima das outras ao rolar.
- No Android não aparecem mais os atalhos de teclado (coluna Ctrl+N, botão ⌨, "(Ctrl+F)" na busca e a seção Atalhos da Ajuda).

### ✨ Ajustes
- O botão Ajuda da barra lateral ficou só com o texto, sem cortar em telas estreitas.
- O README mostra os sistemas suportados (Windows, Linux, macOS e Android) e um print do app no celular.

## 0.4.7 — 2026-10-03

### 📱 Android
- **O PKHeX Modern agora tem versão para Android** (APK na release). A interface é a mesma do desktop, com a tela deitada: barra lateral, caixas e editor lado a lado. Em celulares tudo é reduzido para caber; em tablets fica no tamanho normal.
- Os saves são abertos pelo seletor de arquivos do Android, sem pedir acesso a todo o armazenamento. O app edita uma cópia própria e, ao **Salvar**, grava de volta no arquivo original e confere a gravação; o original anterior vai para os backups. Se o arquivo mudou em outro app, nada é sobrescrito.
- Importar e exportar Pokémon, Salvar como e saves dentro de .zip funcionam pelo seletor também. O botão Voltar do Android fecha a pergunta aberta ou volta como o Esc.
- Fora desta primeira versão: atualização automática (instale o APK novo por cima), explorador de pastas e bank em pasta externa.

## 0.4.6 — 2026-10-03

### 🖥️ Linux e macOS
- **O PKHeX Modern agora roda no Linux e no macOS**, além do Windows (o PKHeX original é só Windows). A release traz quatro pacotes: Windows x64, Linux x64 (serve também para o Steam Deck), macOS Apple Silicon e macOS Intel. Nenhum precisa do .NET instalado.
- No macOS o app vem como `PKHeX Modern.app`. Ele não tem assinatura da Apple, então na primeira abertura o macOS pede para liberar: botão direito no app › Abrir, ou Ajustes › Privacidade e Segurança › Abrir mesmo assim. O README explica, junto com as dependências do Linux.
- A **atualização automática** baixa o pacote certo de cada sistema. No Windows nada muda; no Linux a permissão de execução é mantida; no macOS o app inteiro é trocado.
- Os sprites agora são montados sem depender do Windows: são os mesmos PNGs e a mesma montagem do PKHeX, refeitos com SkiaSharp e conferidos pixel a pixel contra o original (nada muda na tela).
- A Ajuda diz onde ficam as preferências, os backups e o bank em cada sistema (`~/.config/PKHeX.Modern` no Linux e no macOS).

### 🐞 Correções
- A atualização automática continua funcionando quando o arquivo do programa foi renomeado (ex.: `PKHeX.Modern (1).exe`).

## 0.4.5 — 2026-10-03

### 🎮 Editores por jogo
- **Eventos do Gen 1, Let's Go e Z-A** na página Jogo. Gen 1: as 2560 flags e 256 valores, com nome nas flags dos Pokémon fixos do mapa. Let's Go: as 4096 flags e 1000 valores, com os nomes da lista do PKHeX (Snorlax, aves lendárias...) e o tipo no código (Z zona, S sistema, V objeto do mapa, E evento). Z-A: as tabelas de eventos, sistema, itens do mapa, missões, tarefas da Mable e títulos; o jogo guarda só o hash de cada evento, então os itens do mapa (Colorful Screws e TMs) são os únicos com nome. A busca agora também encontra pelo código.
- **Novos atalhos**: Pokémon fixos do mapa de novo no Gen 1 (Mewtwo, Articuno, Zapdos, Moltres, Voltorbs da Power Plant, Eevee, fósseis, presentes do Yellow...), todos os títulos de Mestre Treinador no Let's Go, Colorful Screws e TMs do mapa no Z-A, estojo cheio de Pokéblocks (Ruby/Sapphire/Emerald e Omega Ruby/Alpha Sapphire) e de Poffins (Diamond/Pearl/Platinum e BD/SP), e pontos, Data Cards e medalhas do Pokéathlon (HeartGold/SoulSilver).
- **Hall da Fama** (nova aba ★, só leitura): as equipes que venceram a Liga com sprite, apelido e nível no Gen 1, Gen 3, X/Y e Omega Ruby/Alpha Sapphire (com a data), e as espécies da primeira equipe e da mais recente no Sun/Moon.
- Atalhos que dão itens (Member Card do BD/SP, Colorful Screws, TMs) agora atualizam a Mochila e a Pokédex na hora, sem perder o que estava sendo editado na Mochila.

## 0.4.4 — 2026-10-03

### 🎮 Editores por jogo
- **Eventos do Scarlet/Violet** na página Jogo: cerca de 650 flags e 280 valores de evento que o PKHeX conhece pelo nome (cada evento é um bloco do save), agrupados em categorias: **Voo** (pontos de voo), **Mapa**, **Receitas de TM**, **Recursos liberados**, **Compras**, **Estacas (Treasures of Ruin)**, **Leilão**, **Surtos**, **Batalhas e desafios**, **Academia**, **História e eventos** e outras. Busca, filtros e “Ativar mostradas” funcionam como nos outros jogos.
- **Eventos do BD/SP**: as 4000 flags, as 1000 flags de sistema (mostradas como S#0000) e os 500 valores de evento, com os nomes que o PKHeX conhece.
- **Atalhos do BD/SP** (nova aba ⚡ Atalhos): revanche com Dialga/Palkia, eventos do **Darkrai**, **Shaymin** e **Arceus** (com o item, a Pokédex Nacional e as flags certas), **Spiritomb**, **Mesprit** e **Cresselia** errantes de novo, todas as áreas do mapa e todas as roupas. Cada atalho explica o que faz, pergunta antes e fica desativado quando não há nada a fazer.

### 🗂 Saves
- **Seus saves na barra lateral**: sem nenhum save aberto, a barra lateral lista os saves da pasta do Save Manager (selo do jogo, nome e arquivo) e um clique abre o save. O “Salvar como...” desativado some até haver um save aberto.

## 0.4.3 — 2026-10-03

### 🎨 Temas
- **Fonte pixelada no tema Pixel**: a Consolas (fonte de terminal, larga) deu lugar à **Pixelify Sans**, uma fonte pixelada com acentos, embutida no app, com negrito de verdade. Ela aparece nos menus, rótulos e títulos; as legendas pequenas (até 13 px) continuam na fonte padrão, porque fontes pixeladas embolam nesses tamanhos.
- **Escolha da fonte**: no ⚙ da barra lateral, a seção **Fonte** troca a fonte da interface na hora, em qualquer tema: **Do tema** (o padrão: Pixelify Sans no tema Pixel, Inter nos outros), **Inter**, **Pixelify Sans** ou **Pokémon GB/GBC**. Cada opção aparece escrita na própria fonte, e a escolha fica salva.
- **Fonte do Pokémon de Game Boy**: a opção **Pokémon GB/GBC** usa a Pokemon Classic, recriação feita por fãs da fonte de Red/Blue/Yellow/Gold/Silver/Crystal (de TheLouster115, CC BY-SA 3.0), com acentos. Como as letras são largas, ela vale nos títulos (nome da caixa, nome do Pokémon, títulos das páginas, logo); o texto normal fica na Inter para não cortar.

### 📦 Caixas
- **Abas das caixas centralizadas**: quando todas cabem, a lista fica no centro, alinhada ao nome da caixa. Quando não cabem (ex.: 24 ou 32 caixas), ela rola sozinha para deixar a caixa aberta no meio, inclusive ao trocar de caixa pelas setas ou pelo teclado.

## 0.4.2 — 2026-10-03

### 🧭 Barra lateral
- **Mais compacta**: as 13 páginas cabem sem rolar na janela padrão (e a partir de 720 px de altura). Os itens da lista ficaram mais baixos, o cartão do jogo menor e o modo legal virou uma linha só.
- **Preferências no ⚙**: tema, cor de destaque, idioma e “Abrir último save ao iniciar” ficam no botão ⚙ da linha de baixo, ao lado de ❔ Ajuda, ⌨ (atalhos do teclado) e ◐ (claro/escuro).
- “↺ Último” aparece só sem save aberto (com um save aberto, as abas e o Início já mostram os recentes).

## 0.4.1 — 2026-10-03

### 🎮 Editores por jogo
- **Página Jogo** (nova): **flags de evento** da Gen 2 à 7 (itens escondidos já pegos, treinadores vencidos, pontos de voo, história, encontros, presentes...) com os nomes das listas do PKHeX, busca por nome ou número, filtro por categoria e “Ativar/Desativar mostradas” para mudar de uma vez tudo que o filtro mostra (com confirmação).
- **Valores de evento**: contadores e estados de cada evento, com os valores conhecidos numa lista e o número livre ao lado.
- **Recordes do treinador** na Gen 3, 5, 6, 7, Sword/Shield e BD/SP: passos, batalhas, capturas, ovos chocados e os demais contadores do jogo.

## 0.4.0 — 2026-10-03

### ⚙ Edição em lote
- **Batch Editor** (nova página): altera vários Pokémon do save aberto de uma vez. Escolha o grupo (caixa atual, todas as caixas, equipe, caixas e equipe, Pokémon selecionados com Ctrl+clique ou os **resultados da Pesquisa** deste save) e o que fazer: ações rápidas (nível 100, IVs máximos, zerar EVs, felicidade máxima, curar PS e PP, tornar shiny, golpes sugeridos, bola legal) e/ou o **script do Batch Editor do PKHeX** (`=Species=Pikachu` filtra, `.CurrentLevel=100` altera), com lista de propriedades para inserir.
- **Pré-visualizar** mostra cada Pokémon com o que vai mudar (nível, IVs, EVs, shiny, golpes, bola...) e se continua legal, sem gravar nada. **Aplicar** pergunta antes, grava tudo num passo só e **Ctrl+Z desfaz** o lote inteiro.
- **Modo legal**: quem ficaria ilegal (⚠) é pulado; com o modo legal desligado, grava mesmo assim e avisa.

### 🔎 Pesquisa
- **Banco de dados / pesquisa** (nova página, **Ctrl+Shift+F**): todos os Pokémon dos saves abertos (com as alterações não salvas), dos saves da pasta (inclusive dentro de .zip) e do bank numa lista só. Busca por espécie, apelido, golpe, item, habilidade, natureza, bola, treinador ou jogo (todas as palavras precisam bater; `#25` busca pelo número), filtros de origem, shiny, IVs perfeitos, natureza, bola, nível, geração de origem e ovos, e cinco ordens. Clicar num resultado vai até o slot, abrindo o save numa aba se preciso, ou abre a caixa do bank.

### 💾 Saves antigos
- **Versão e idioma da Gen 1–3 pelo nome**: o app descobre qual jogo do par é (Red/Blue, Gold/Silver, Ruby/Sapphire, FireRed/LeafGreen) e o idioma pelo nome do arquivo e agora também pela **entrada do zip, pelo nome do zip e pela pasta** (ex.: `Pokemon Sapphire/jogo.sav`, `Rouge/save.sav`). Antes, saves dentro de zip não eram identificados (um FireRed/LeafGreen zipado aparecia sempre como FireRed).
- **Selo duplo**: quando não dá para saber, o save aparece como o par (“Ruby / Sapphire”) com os dois mascotes no selo.

## 0.3.9 — 2026-10-03

### ⌂ Início
- **Tela inicial do save**: ao abrir um save, o app mostra o Início (⌂ na barra lateral, **Ctrl+0**), com o cartão do jogo (treinador, tempo de jogo, dinheiro e Pokédex), a **equipe no topo**, um **atalho grande** para cada página com um resumo e os **saves recentes**, que abrem numa aba nova.

### 🎨 Temas
- **Temas completos**: além da cor de destaque, o botão 🎨 troca fundo, painéis, cantos, bordas e fonte na hora. Quatro temas, cada um em claro e escuro: **Padrão**, **PSS** (X/Y, azul e bem arredondado), **Pixel** (GBA, cantos retos, bordas grossas e fonte de terminal) e **Z-A** (Lumiose, preto e lima). Cada tema aplica a cor de destaque sugerida (nova cor: Lima).
- Barra lateral mais compacta: claro/escuro, tema e idioma numa linha só, e as dicas de atalho num “⌨ Atalhos do teclado” (passe o mouse).

### Correções
- **Salvar às vezes não gravava**: com o mouse parado sobre o 💾 Salvar, a dica do botão abria por cima dele (no canto da janela não havia espaço embaixo) e o clique caía na dica. As dicas da barra de baixo agora abrem acima dos botões e nenhuma dica captura clique.
- **Mochila no Salvar**: itens mudados na Mochila sem clicar em “Aplicar mochila” ficavam de fora do save e nem marcavam alterações. Agora contam como alteração pendente e o Salvar (ou trocar de aba) aplica antes de gravar.
- O botão “🌐 Idioma” não fica mais cortado na barra lateral, e a dica do Feebas explica o que o botão de evoluir faz.

## 0.3.8 — 2026-10-03

### 🌐 Interface
- **Menu de idiomas**: “🌐 Idioma” na barra lateral troca a interface entre **Português (Brasil)** (padrão) e **English**. A escolha fica salva e o app oferece reiniciar para aplicar. Nomes do jogo e textos de legalidade continuam em inglês; as notas de versão e as descrições do AllGenWiki só existem em português (em inglês, as descrições ficam ocultas).

### ♥ Editor
- **Feebas evolui pelo botão**: “Evoluir para Milotic” agora cumpre os requisitos sozinho: sobe o Beauty até 170, ajusta o Sheen à faixa legal (Gen 3/4 e BDSP), tira a Everstone e sobe um nível. Só fica bloqueado no nível 100 antes da Gen 8 ou num Pokémon que nunca passou por jogos com concursos (ex.: um Feebas nativo de Black/White).
- **Evolução por felicidade** também cumpre os requisitos: tira a Everstone e, no Sylveon da Gen 6/7, sobe o carinho para 2 corações.

## 0.3.7 — 2026-10-02

### ♥ Editor
- **Feebas → Milotic**: evolução por Beauty na Visão geral, separada da felicidade, exige Beauty ≥ 170 e respeita o método disponível no jogo. BDSP usa Beauty; Z-A oferece troca. Beauty não é aumentada automaticamente. A troca pode ser simulada sem Prism Scale; se estiver segurando a escala, ela é consumida. No modo legal, as evoluções por Beauty e troca só são aceitas após validar uma cópia.
- **Evolução por felicidade**: botões na Visão geral para evoluir com os requisitos do jogo. A felicidade é aumentada apenas até o mínimo necessário, e o nível sobe quando a evolução exige isso. Dia/noite é simulado pelo botão escolhido. Ovos, Everstone, limites de nível e requisitos de Sylveon (golpe Fairy, carinho nas Gen 6/7 e prioridade sobre Espeon/Umbreon) são conferidos. No modo legal, o resultado é testado numa cópia e só é aceito se continuar legal.

### Interface
- **Barra inferior contextual**: Caixas e Equipe mostram ações de Pokémon; Saves oferece Abrir/Atualizar, Encontros e Eventos permitem usar a seleção, Bank atualiza anexados, Pokédex oferece atualização/sincronização e Mochila permite aplicar os itens. Salvar continua disponível nas páginas do save; Ajuda oferece Voltar.

### 🎁 Importação e salvamento
- **Mystery Gift num slot**: solte um arquivo de evento (`.wc*`, `.pgf`, `.pcd` e outros formatos reconhecidos pelo PKHeX) numa caixa ou na equipe para gerar o Pokémon. Substituir pergunta antes e Ctrl+Z desfaz; presentes de itens são recusados com uma explicação.
- **Salvar depois de editar**: Salvar e Ctrl+S agora aplicam a edição pendente do Pokémon antes de gravar o save, inclusive dentro de ZIP. No modo legal, uma edição que não pode ser aplicada bloqueia o salvamento e explica o motivo. Erros de gravação aparecem numa janela e mantêm o save marcado com alterações.

### 🔄 Atualizações
- **Novidades antes de atualizar**: a janela mostra as notas publicadas no GitHub e oferece “Atualizar agora” ou “Depois”. A oferta automática também aguarda sua escolha antes de baixar e instalar. Rodando pelo código, a janela oferece abrir o download no navegador.

### 🗂 Saves
- **Save Manager**: cartões mostram o idioma identificado pelo PKHeX e a data de início da aventura quando disponível. Use setas para navegar entre cartões e Enter para abrir; a opção “Esconder SID” fica salva nas preferências. A contagem de arquivos ignorados também aparece quando a pasta não contém nenhum save reconhecido.

### 📦 Caixas
- **Nome e papel de parede**: renomeie a caixa pelo botão ao lado do título e escolha o papel de parede do jogo na lista. Os nomes das abas acompanham a alteração e o fundo aparece discretamente atrás dos cartões. As opções respeitam o suporte de cada jogo; use Salvar para gravar no save.

## 0.3.6 — 03/10/2026

### ✏️ Editor
- **Atalhos de shiny** (como no PKHeX): a estrela ao lado do nome agora é clicável (☆ normal, ★ estrela, ◆ quadrado). **Clique** torna shiny ou tira o shiny; **Alt+clique** deixa shiny mantendo o PID (troca o SID do treinador original, então Pokémon da Gen 3/4 com PID e IVs ligados continuam legais); **Shift+clique** pede shiny quadrado e **Ctrl+clique** shiny estrela (a diferença só existe a partir da Gen 8). O botão "Tornar shiny" da aba Extras aceita os mesmos atalhos e vira "Tirar shiny" quando o Pokémon já é shiny. Com o modo legal, o Legalizar respeita o formato pedido.
- **Seletor de forma com sprite**: cada forma aparece com o próprio sprite (no mesmo shiny e gênero do Pokémon), na lista e na caixa do campo.
- **Gênero clicável**: clique no ♂/♀ ao lado do nome para trocar o gênero. Até a Gen 5 o gênero vem do PID, então um PID novo é gerado com a mesma natureza (no modo legal, o Legalizar refaz se precisar). Nas espécies em que a forma é o gênero (Meowstic, Indeedee, Basculegion, Oinkologne), forma e gênero trocam juntos, nos dois sentidos.
- **Deoxys na Gen 3**: em vez da lista de formas, o editor explica que a forma depende do jogo (Normal em Ruby/Sapphire, Ataque no FireRed, Defesa no LeafGreen, Velocidade no Emerald).

### 🛡️ Legalidade
- **Legalizar tira os avisos "Fishy"**: um Pokémon legal com avisos de EVs zerados depois de subir de nível ou EXP exatamente no limite do nível (ex.: um Whismur nascido de ovo no Sapphire) agora é corrigido sem trocar o encontro nem o PID: ganha EVs como em batalhas e um pouco de EXP a caminho do próximo nível. Quando o Legalizar precisa gerar de novo, o resultado também sai sem esses avisos.
- **Correções sugeridas coloridas** (golpes em azul, reaprender em destaque, encontro em âmbar, IVs em verde) e conferidas:
  - **Encontro sugerido** funciona com Pokémon nascidos de ovo na Gen 3 (antes dizia "nenhum encontro possível"): tenta os encontros reais do jogo de origem e fica com o que deixa legal;
  - **IVs máximos** usa a mesma lógica do botão da aba Atributos (no modo legal, oferece converter em nascido de ovo);
  - no modo legal, uma correção que deixaria o Pokémon ilegal não é aplicada e o motivo aparece na barra de status (antes ela era desfeita em silêncio e a mensagem dizia "aplicado").

## 0.3.5 — 03/10/2026

### 💾 Saves
- **Salvar silencioso**: o botão **💾 Salvar** (e Ctrl+S) grava direto por cima do arquivo do save, sem abrir a janela do Windows; o arquivo anterior vai para os backups antes. Para escolher outro lugar, use **Salvar como...** (Ctrl+E). Saves dentro de .zip são gravados de volta no zip.

### ✏️ Editor
- **Evolução por item**: na aba Visão geral, as espécies que evoluem com pedras e afins mostram o botão (ex.: Nidorino + Moon Stone → Nidoking, Eevee + Fire Stone → Flareon), respeitando as que dependem do gênero.
- **IVs máximos na Gen 3/4**: nos encontros selvagens desses jogos os IVs são gerados junto com o PID, então IVs 31 em tudo é ilegal. Com o modo legal, se a espécie nasce de ovo, o app pergunta se pode **converter em nascido de ovo** (de preferência um ovo do próprio jogo), mantendo natureza, gênero, shiny, nível, item, apelido e golpes, e com todos os IVs no máximo.

### 🎒 Mochila
- **Itens em colunas**: cada slot vira um cartão com o ícone grande, o item e a quantidade, em 2 a 4 colunas conforme a largura. A página usa a largura toda (sem o painel do editor), e bolsos grandes (260 slots no Scarlet) abrem na hora.

### ✏️ Editor
- **Concursos no modo legal**: ao subir um atributo de concurso (Cool, Beauty...), o Sheen é ajustado sozinho para o mínimo legal, como acontece no jogo com Pokéblocks/Poffins; antes a mudança era desfeita. Em Omega Ruby/Alpha Sapphire o Sheen fica 0, e nos jogos sem concursos (ex.: Scarlet/Violet) a seção não aparece.
- **Felicidade** some nos jogos da Gen 1, que não guardam esse valor.

### Correções
- **Legalizar com apelido barrado**: um apelido que o jogo não aceita (ex.: "KILLER", barrado pelo filtro de palavras) impedia qualquer legalização. Agora ele é trocado pelo nome da espécie e o app avisa.
- **Legalizar prefere o próprio jogo**: encontros e ovos da versão do save vêm antes dos de outra versão (num save de Sapphire, não usa mais um encontro de LeafGreen).
- **Editor nos saves da Gen 1 e 2**: abrir muitos Pokémon de Red/Blue/Yellow/Gold/Silver/Crystal dava erro e o editor parecia travado, e os tipos apareciam errados (Gengar como Aço). Os jogos do Game Boy numeram os tipos de outro jeito; agora são convertidos.
- **Reiniciar depois de atualizar**: o botão não fazia nada (o app não conseguia carregar parte de si mesmo depois de trocar o exe). Agora isso é preparado antes da troca e, se mesmo assim falhar, o app avisa para reabrir manualmente. Vale a partir da próxima atualização: quem está na 0.3.4 ou anterior ainda precisa fechar e abrir o app depois de atualizar.

## 0.3.4 — 03/10/2026

### 💾 Saves
- **Vários saves em abas**: cada save aberto vira uma aba no topo (selo do jogo, nome do arquivo e ● com alterações não exportadas). Abrir outro save não descarta mais o atual: cada aba guarda as próprias alterações, o desfazer/refazer, a caixa e a página. **Ctrl+Tab** / **Ctrl+Shift+Tab** trocam de aba, **Ctrl+W** fecha; fechar uma aba ou o app pergunta se houver alterações. No Save Manager, todos os saves abertos ganham o selo "Aberto", e restaurar o backup de um save aberto recarrega a aba dele.

### Correções
- **Legalizar Pokémon evoluídos**: quando o encontro é de uma pré-evolução, o Legalizar agora sobe o nível até o mínimo em que a evolução é possível (ex.: um Gengar nv. 7 vira um Gengar nv. 25 vindo de um Gastly da Pokémon Tower). Antes ele falhava e, com o modo legal ligado, o Pokémon ficava sem poder ser aplicado.
- **Encontros do próprio jogo primeiro**: o Legalizar prefere encontros da mesma geração do save (num save de Yellow, a Pokémon Tower em vez de um encontro de Gold por tradeback).
- **Regras de legalidade de cada save**: ao abrir ou trocar de aba, o app aplica as regras que dependem do save, como o PKHeX faz (era do cartucho do Game Boy, console virtual da Gen 1-3, treinador ativo).
- **Ícone do Koraidon/Miraidon**: o selo de Scarlet/Violet e as espécies da Gen 9 apareciam como sprite desconhecido quando o save ativo era de um jogo mais antigo (o PKHeX só tem essas espécies no modo arte). Agora usam o sprite de arte nesses casos (Save Manager, abas, Pokédex e bank).

### Interface
- **Sprites mais nítidos**: os sprites são ampliados 4× em escala inteira (nearest neighbor) antes de ir para a tela, então os pixels ficam nítidos e do mesmo tamanho em qualquer tamanho de cartão e com a escala do Windows em 125%/150% (antes algumas linhas de pixel dobravam e o sprite ficava torto).

## 0.3.3 — 02/10/2026

### 🏦 Bank
- **Variantes**: ao selecionar um Pokémon anexado, aparecem as versões que voltaram de jogos de outra geração (formato, nível e data). Dá para abrir uma variante no editor do save aberto (no lugar da cópia anexada ou num slot vazio; ao Aplicar, o anexado passa a apontar para este save) ou excluí-la, sem mexer no original.

### 📖 Pokédex
- **Formas em Sword/Shield e Legends Arceus**: em "Formas e gêneros", uma forma só conta como vista ou capturada se o jogo a registrou (ex.: Meowth de Galar, Growlithe de Hisui).

### Interface
- **Modo legal mais visível**: virou um cartão na barra lateral, verde com escudo ✓ quando ligado e âmbar com escudo ! quando desligado, com o estado escrito. Clique no cartão para ligar ou desligar.

### Correções
- **Sprites shiny**: os Pokémon shiny agora aparecem com as cores shiny e no mesmo tamanho dos outros. Antes vinham com as cores normais, pequenos, e o ícone de brilho do canto aparecia como um fragmento solto (caixas, equipe, bank, Pokédex, encontros e eventos).

## 0.3.2 — 02/10/2026

### 🏦 Bank
- **Pokémon anexado**: com "🔗 Anexar ao trazer", levar do bank para um save copia em vez de mover; o original fica no bank, ligado ao save. **Atualizar anexados** traz a versão do jogo de volta (de outra geração, guarda como variante sem mexer no original) e oferece desanexar os que sumiram.
- **Outro save** com seleção múltipla (mover grupos entre os dois saves e o bank) e **desfazer/refazer próprio** (↶/↷ no painel).

### 📖 Pokédex
- **Formas**: em Gen 4, 5, 6, BDSP, Scarlet/Violet e Legends Z-A, cada forma só conta como vista/capturada se o jogo a registrou.
- **Sincronizar todos os saves**: registra em cada save da pasta o que foi capturado ou visto nos outros e o que está guardado; os fechados são gravados na hora, com backup.

### Correções
- Marcar "só vista" na Pokédex da Gen 4 e 5 agora funciona; nos jogos que não aceitam (Gen 7), a sincronização não promete as vistas.

## 0.3.1 — 02/10/2026

### ✏️ Editor
- Nova aba **Fitas e memórias**: todas as fitas do formato, com busca, "Só as que tem", **Todas as legais** e **Remover todas**; **memórias** do treinador original e do atual (Gen 6+), com detalhe, intensidade e sentimento.
- **Contest stats** (Cool, Beauty, Cute, Smart, Tough, Sheen), **Dynamax Level** e **Gigantamax** (Sword/Shield), **alpha** e **nobre** (Legends) na aba Extras.
- **Onde aprender**: ao passar o mouse num golpe, o local da TM e o tutor neste jogo (AllGenWiki).

### 🎒 Mochila
- **TMs, TRs e HMs** mostram o golpe que ensinam, a descrição e onde pegar a máquina neste jogo.

### 🔄 Atualização automática
- Primeira versão entregue pela atualização automática da 0.3.0.

## 0.3.0 — 02/10/2026

### 🛡 Modo legal
- Chave na barra lateral, **ligada por padrão**: o editor só oferece opções legais (golpes que o Pokémon aprende, bolas permitidas para o encontro, espécies do jogo).
- Mudanças que deixariam o Pokémon ilegal são desfeitas na hora, com o motivo na barra de status.
- **Aplicar** só grava Pokémon legal; trocar a espécie ou colar um set Showdown legaliza sozinho.
- Pokémon **de fora** (arquivo .pk*, bank ou outro save) que chega ilegal ao save aberto ganha a opção **✨ Legalizar** ou **Trazer como está**.
- Habilidade, forma e Tera Type só listam opções legais; formas de batalha somem da lista.
- **Trocar encontro** (aba Encontro): escolha um encontro real e o Pokémon é gerado de novo a partir dele, em vez de mexer em local e nível à mão.
- **Tornar shiny** fica bloqueado em encontros com shiny lock e, nos outros, gera de novo já shiny com PID/IV corretos.

### ✏️ Editor
- **Habilidade** escolhida numa lista (com a oculta), **forma** e **Tera Type** (Scarlet/Violet).
- **PID e EC editáveis**, **marcações** (●▲■♥★◆, azul/rosa na Gen 7+) e **Hyper Training** (Gen 7+).
- Descrição em português de **golpes e habilidades** (AllGenWiki), na lista e no campo.

### 🎮 Selo do jogo
- O Pokémon da capa nas cores da versão identifica cada save no Save Manager e no cartão do save aberto.

### ❔ Ajuda, novidades e atualizações
- Nova página **Ajuda** (F1) com todas as funções explicadas, este changelog e a aba Sobre.
- **Atualização automática**: ao abrir, o app verifica se saiu uma versão nova no GitHub, baixa, confere o arquivo (SHA-256) e instala sozinho. A versão nova vale ao reiniciar (botão 🔄 na barra lateral, que pergunta antes se houver alterações não salvas). Dá para desligar em Ajuda › Sobre.
- Conheça também o **AllGenWiki**, enciclopédia Pokémon das nove gerações (Ajuda › Sobre).

### 🌿 Encontros e eventos
- **Painel de detalhes**: clique num cartão para ver o que o encontro garante (shiny ou shiny lock, IVs, habilidade, natureza, gênero, bola, item, Tera Type, alpha/Gigantamax, treinador do evento) e os golpes com que ele vem.
- **Filtros** por tipo de encontro, versão do jogo e golpe; selo **HOME** nos presentes do Pokémon HOME e 🔒 nos encontros com shiny lock.

### 🗜 Saves dentro de .zip
- **Backups do JKSV** (ou qualquer .zip com saves) aparecem no Save Manager com o selo **ZIP**, um cartão por save; também dá para abrir um .zip direto.
- Salvar oferece **gravar de volta dentro do zip** (com backup do zip antes) ou salvar como arquivo separado. A Pokédex e o "Outro save" também leem esses saves.

### 🎒 Mochila
- Ícone de cada item na mochila e no item segurado do editor.
- Ao passar o mouse num item, a descrição em português e onde conseguir (dados do AllGenWiki).

## 0.2.0 — 02/10/2026

### 📖 Pokédex centralizada
- As 1025 espécies, juntando a Pokédex de todos os saves da pasta, o save aberto e o bank.
- Living dex e shiny dex, filtros por situação, geração, tipo e fonte; formas e gêneros como entradas próprias.
- Onde está cada Pokémon, com **Ir** para abrir no editor, e **Sincronizar** com o save aberto.

### 🏦 Bank local
- Armazenamento próprio em bancos → caixas, com os Pokémon como arquivos .pk* no formato original.
- Duas telas (bank | save) com conversão de geração; **outro save** no lugar do bank; **pastas externas** como bancos.

### 📦 Caixas e equipe
- **Seleção múltipla** (Ctrl+clique, Shift+clique, Ctrl+A) para mover ou excluir em grupo.
- **Ordenar caixas** por Pokédex, nome, nível, shiny, tipo, IVs ou data de captura.
- Caixas em abas, ícone da bola nos cartões e lista das alterações não exportadas.

### ✏️ Editor
- **Evoluir por troca**, respeitando a Everstone e registrando o parceiro de troca na Gen 6+.
- **Verificar legalidade** do save inteiro, com atalho para o primeiro problema.
- Eventos da Gen 1–3 no banco de Mystery Gift.

### 🛟 Backups
- Backup automático antes de salvar por cima, com lista, **Restaurar** e **Restaurar como...**.

## 0.1.0 — 01/10/2026

Primeira versão: uma interface nova, em português, por cima do mesmo PKHeX.Core.

### O que tem
- **Save Manager** como tela inicial, com os saves de uma pasta agrupados por console.
- **Caixas e equipe** com cartões grandes, legalidade, arrastar e soltar e o resumo ao passar o mouse.
- **Editor em abas** com busca enquanto digita, golpes que o Pokémon aprende em verde, IV/EV com gráfico.
- **Legalizar** a partir de um encontro real do jogo (inclusive shiny).
- Bancos de **encontros** e de **eventos (Mystery Gift)**, com "Usar" para levar ao editor.
- **Busca global** (Ctrl+F), desfazer/refazer, excluir, atalhos de teclado e confirmações.
- Tema claro/escuro e cor de destaque configurável.
