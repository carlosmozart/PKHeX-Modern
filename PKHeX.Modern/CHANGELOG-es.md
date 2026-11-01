# Novedades de PKHeX Modern

<!--
Traducao do CHANGELOG.md (mesmas versoes, secoes e itens, na mesma ordem; as suites de idioma conferem).
Formato: "## versao — data", "### secao", "- item".
-->

## 0.5.4 — 2026-11-01

### 🌐 Idioma
- **Novedades en tu idioma:** la pestaña Novedades de la Ayuda ahora está en inglés, español y chino simplificado, con todas las versiones, y la ventana de actualización muestra las notas en el idioma de la app, sin la parte de instalación.

### 🔄 PKHeX actualizado
- **PKHeX del 31/10/2026:** el regalo de Spewpa de Legends: Z-A pasa a reconocerse, con sus fechas de distribución, y se revisaron las reglas de forma de Scatterbug, Spewpa y Vivillon. Los Pokémon de GO que llegan por HOME pueden tener cualquier idioma de HOME, sin el límite de los idiomas de la Gen 8. La Master Rank Ribbon deja de valer para los Pokémon de FireRed/LeafGreen de la Consola Virtual que solo la tendrían por Scarlet/Violet.

### 🔧 Correcciones
- **Aviso al mover Pokémon:** en los otros idiomas, el mensaje después de mover, copiar o vincular Pokémon ahora aparece traducido. En chino, los nombres en inglés con la palabra Pokémon, como “Pokémon League”, ya no se alteran, y el Bank tiene el mismo nombre en toda la interfaz.

## 0.5.3 — 2026-10-26

### 🔧 Correcciones
- **Resumen de Encuentros en chino:** la frase sobre las tarjetas (“伊布 有 53 个遭遇”) mostraba un trozo en portugués en medio.

## 0.5.2 — 2026-10-26

### 🌐 Idioma
- **Interfaz en chino simplificado:** toda la app ahora también está en chino simplificado (简体中文), con los términos oficiales de los juegos en chino (盒子, 同行, 包包, 训练家, 招式, 名人堂, 太晶属性...). Especies, movimientos, habilidades, objetos y mensajes de legalidad también aparecen en chino, como en el PKHeX original; “Nombres del juego en inglés” mantiene el inglés. Los sistemas en chino (simplificado o tradicional) abren en él la primera vez. La fuente Noto Sans SC viene con la app, así que el chino se ve igual en Windows, Linux, macOS y Android.

### 🔧 Correcciones
- **Búsqueda de shiny y huevo en otros idiomas:** la búsqueda acepta “huevo” (español) y “发光”/“蛋” (chino), además de “shiny”, “ovo” y “egg”, como dice la Ayuda de cada idioma.
- **Textos que faltaban en inglés:** “Filters”, “Types”, “Variants” y “Sync” aparecían en portugués en la interfaz en inglés.
- **Caja del cementerio del Nuzlocke en chino:** el informe también reconoce cajas con nombres como “墓地” o “死亡”.

## 0.5.1 — 2026-10-25

### 🌐 Idioma
- **Interfaz en español:** toda la app ahora también está en español, con los términos oficiales de los juegos en español (Caja, Equipo, Bolsa, Guardería Pokémon, Hall de la Fama, Teratipo...). Elige “Español” en el botón 🌐 o en el ⚙.
- **Nombres del juego en el idioma de la interfaz:** en español, especies, movimientos, habilidades, objetos, naturalezas y mensajes de legalidad también aparecen en español, con los nombres oficiales de los juegos, como en el PKHeX original. “Nombres del juego en inglés”, en el 🌐 y el ⚙, mantiene el inglés. En portugués e inglés nada cambia (el PKHeX no tiene los nombres en portugués). Copiar para Showdown sigue en inglés, y pegar un set acepta cualquier idioma.
- **Idioma del sistema en la primera apertura:** quien instala la app ahora empieza en el idioma del ordenador o del móvil (portugués en cualquier país, español en los sistemas en español e inglés en los demás), en vez de siempre en portugués. Quien ya usa la app sigue en el idioma que tenía.
- **Textos que faltaban en inglés:** las siglas de las características (Atk, SpA, SpD, Spe, HP), “Missing” en la Living Dex, “Ready” en la barra de estado y títulos de la Ayuda aparecían en portugués en la interfaz en inglés.
- **Botón 🌐 en el pie de la barra lateral:** junto a Ayuda, muestra el idioma actual (PT, EN, ES) y abre la lista con cada idioma escrito en sí mismo, para encontrarlo sin leer la pantalla. La pregunta de reiniciar aparece en el idioma elegido.

### 🎨 Interfaz
- **AllGenWiki en su propio dominio:** el botón “🌐 Abrir AllGenWiki” (Ayuda › Acerca de) ahora lleva a allgenwiki.com.
- **Ball con sprite y atajo:** la lista de Balls del editor muestra el sprite de cada una, y al hacer clic en la Ball de la ficha del Pokémon se va directo a la pestaña Encuentro, con la lista enfocada.

### 🔧 Correcciones
- **Hall de la Fama sin mote:** al cambiar un miembro sin escribir un mote, la app guarda el nombre de la especie en el idioma de la partida, como hace el juego. Antes guardaba siempre el nombre en inglés, incluso en partidas de otros idiomas.

### 🔄 PKHeX actualizado
- **PKHeX del 25/10/2026:** los Pokémon del Pokéwalker (HeartGold/SoulSilver) con el bug de PP del aparato pasan a reconocerse como legales. Se revisó la comprobación de shiny de los huevos de eventos de la Gen 3 (Regalo Misterioso), y el generador de encuentros ya crea esos huevos para el entrenador de la partida.

## 0.5.0 — 2026-10-23

### 🎨 Interfaz
- **Títulos con personalidad:** los títulos grandes (páginas, nombre de la partida, Pokémon en el editor, logo y vitrina) pasaron a usar Fredoka, una fuente redondeada; el texto sigue en Inter. ⚙ › Fuente › Inter vuelve a poner todo en Inter.
- **Iconos de juego:** la barra lateral y los atajos del Inicio cambiaron los iconos genéricos por dibujos propios y coloridos de objetos de los juegos: Poké Ball, caja del PC, el PC del Bank, la Pokédex, la gorra del entrenador, la Bolsa, la hierba alta, el cartucho y la consola portátil.
- **Barra lateral de Pokédex:** la parte superior se convirtió en la tapa de una Pokédex, con la franja en el color de acento (o en el color del juego de la partida), la lente azul y las tres luces; la línea sobre las páginas ganó el botón de la Poké Ball. Dibujos propios, sin arte oficial.
- **Arrastrar con vista previa:** el sprite del Pokémon sigue al puntero (en el móvil, por encima del dedo) con lo que pasará al soltar (Mover, ⇄ Cambiar, Copiar, Sobrescribir, Cambiar de caja) y cuántos van juntos en la selección múltiple; el origen queda atenuado hasta soltar.
- **Modo juego:** arriba del ⚙, un interruptor activa de una vez el color del juego de la partida, el fondo completo, el color del tipo en las tarjetas y las animaciones. Al desactivarlo vuelve el aspecto anterior; también desde la paleta (Ctrl+K).
- **Color del juego de la partida:** nuevo color de acento (el punto arcoíris en el ⚙) que sigue a la versión abierta, con un brillo suave del color del juego en la parte superior de la pantalla. Cambia con la pestaña; los colores muy claros u oscuros se ajustan para que los botones sigan siendo legibles.
- **Animaciones cortas:** la Poké Ball se abre al cargar una partida, la casilla da un saltito al recibir un Pokémon, el editor suelta un brillo ✦ cuando el Pokémon pasa a ser legal o shiny, y la ★ de los shiny en las cajas parpadea de vez en cuando. Todas son ligeras y no bloquean los clics; ⚙ › “Animaciones” las desactiva.
- **Línea de tiempo de la partida en el Inicio:** cada vez que abres, el Inicio muestra lo que cambió desde la vez anterior: horas de juego, Pokémon y especies nuevos, shinies (con el sprite), medallas, Pokédex y dinero. Separa lo que vino del juego de lo que se guardó en la app; la primera vez, usa la copia de seguridad más reciente como punto de partida.
- **Hexágono de características al estilo SwSh/SV:** en la pestaña Características, la forma en el color del tipo, los EV como segunda capa dorada y el valor de cada característica junto a su nombre, en rojo o azul según la naturaleza.
- **Ficha del Pokémon en el editor:** la parte superior del editor ganó el brillo del color de los tipos, el Teratipo (Scarlet/Violet), el nivel con la naturaleza, la Poké Ball y la etiqueta del juego de origen con la generación. Sigue las ediciones al instante.
- **Pantallas vacías ilustradas:** carpeta de partidas vacía, ninguna copia de seguridad, caja vacía, ninguna otra partida en el Bank, Búsqueda sin resultados y Hall de la Fama vacío ganaron un dibujo propio y un consejo de qué hacer. El aviso de caja vacía no bloquea el arrastre ni el clic en las casillas.
- **Paleta de comandos (Ctrl+K):** escribe “legalizar”, “caja 5”, “Pikachu” o “exportar” y Enter lo ejecuta: acciones, páginas, cajas por número o nombre y Pokémon de la partida por especie o mote (se abre en el editor). Sin diferenciar mayúsculas ni acentos; en el móvil, con el ⌘ junto a la búsqueda.
- **Vitrina del equipo:** en la página Equipo, “🖼 Vitrina” muestra los seis Pokémon en grande, con tipos, Teratipo, objeto, habilidad, naturaleza y movimientos coloreados por tipo, en Cuadrícula (3×2) o Columnas. “Guardar imagen” graba un PNG en alta resolución para compartir.
- **Color del tipo en las tarjetas:** cada Pokémon de las cajas, del equipo, del bank y de la otra partida tiene el fondo de la tarjeta teñido con el color de sus tipos (el primero arriba, el segundo abajo). Los huevos y las casillas vacías quedan neutros. Se puede desactivar en ⚙ › “Color del tipo en las tarjetas”.
- **Fondo “Completo”:** nueva opción en ⚙ › Fondo de las cajas que muestra el fondo del juego nítido, como en el propio juego, con las tarjetas translúcidas encima. El predeterminado sigue siendo Normal.

### 📱 Android
- **Gestos:** desliza el dedo hacia un lado en las cajas para cambiar de caja (en la página Cajas y en los tres paneles del Bank) y tira de la lista de la página Partidas hacia abajo para releer la carpeta. Mantener el dedo sigue marcando y arrastrando.

### 🔄 PKHeX actualizado
- **PKHeX del 21/10/2026:** la lectura de sets de Showdown es más estricta. Las líneas de EV/IV con un valor no válido, la misma característica repetida (ej.: “30 Atk / 29 Atk”) o dos características marcadas con el mismo + o − pasan a señalarse como error en vez de que la app adivine el valor. En FireRed/LeafGreen, el Meteorite pasa a estar entre los objetos clave de la Bolsa. Por dentro, el generador de encuentros del PKHeX pasó a ignorar las versiones que aún no trata (la protección de la app para esto sigue).

## 0.4.30 — 2026-10-19

### 🔄 PKHeX actualizado
- **PKHeX del 18/10/2026:** las transferencias de FireRed/LeafGreen de la Consola Virtual a HOME pasan a reconocerse, con más comprobaciones de movimientos de huevo; en la Gen 3 y 4, los EV se comprueban contra la experiencia ganada; el Greninja del WCS 2026 dejó de tratarse como evento caducado; flags de los bolsillos de objetos de Legends: Z-A.

### 🎨 Interfaz
- **Lista completa en especie y objeto:** en el editor (y en las especies de la Caza shiny), tocar o hacer clic en la caja muestra la lista completa, con el nombre actual seleccionado; escribir reemplaza el nombre y filtra por cualquier parte de él (“chu” encuentra Pichu, Pikachu y Raichu). Antes, la lista solo aparecía tras la primera letra.

### 🐛 Correcciones
- **Formas en el editor:** cambiar la forma (ej.: Meowth a Alola o Galar) dejaba la caja de forma en blanco, porque el cambio también modificaba las listas de Ball y habilidad. Ahora solo se actualiza la caja de la lista que cambió.
- **Legalizar tras la actualización del PKHeX:** las versiones nuevas que el PKHeX acababa de listar (Pokémon Winds y Waves) aún no tienen encuentros y rompían la búsqueda: cambiar la forma en modo legal daba “error (context None)”. Quedan fuera hasta que el PKHeX las trate.
- **App bloqueada al traer Pokémon de una generación más nueva a la Gen 3:** con la actualización del PKHeX, llevar un Pokémon de la Gen 7 (o más nueva) a una partida de FireRed, Emerald u otra de la Gen 3 (por el Bank, la otra partida o importando un archivo) entraba en un bucle sin fin. Ahora la app lo rechaza al momento, como antes (“sin ruta de transferencia”).
- **Evolucionar en Let's Go y en Legends: Arceus:** el PKHeX nuevo comprueba la altura y el peso guardados, y evolucionar (o cambiar la especie o la forma) dejaba al Pokémon ilegal (“Calculated Height/Weight does not match”). Ahora ambos se recalculan a la vez.
- **Móvil:** la barra “Seleccionar / Mover” de las Cajas quedaba debajo de las pestañas de las partidas; ahora queda justo por debajo de ellas.

## 0.4.29 — 2026-10-17

### ✨ Caza shiny
- **Nueva página Caza shiny** (al final de la barra lateral): contador con +1, Espacio o +, deshacer y sumar varios; probabilidad por encuentro de cada método del juego abierto (Masuda, Poké Radar, pesca en cadena, Friend Safari, SOS, Catch Combo, Dynamax Adventure, brotes y bocadillo de SV...), probabilidad de haberlo encontrado ya, encuentros para el 50/90/99 % con la curva, tiempo cazando y ritmo por hora. Cada método muestra la fuente; los de valor dudoso aparecen como “por confirmar”.
- **Lee la partida:** el Shiny Charm se marca si está en la Bolsa, y al abrir una partida con un shiny nuevo de la especie de la caza la app ofrece concluirla, con el lugar y la fecha de captura.
- **Fases y trofeos:** “Registrar fase” guarda un shiny que no era el objetivo sin poner la caza a cero (un shiny nuevo de otra especie en la partida también se ofrece como fase); la caza concluida se vuelve un trofeo con juego, método, encuentros, fases, fecha e “Ir” al Pokémon en la partida; “Exportar CSV” guarda todo en una hoja de cálculo.
- **Dónde encontrarlo:** lugares y niveles del objetivo en el juego, aviso de shiny lock y los movimientos del salvaje que pueden estropear la caza (Explosion, Self-Destruct, Roar, Whirlwind, Teleport, Perish Song, retroceso...), con lo que hay que hacer.

### 🐛 Correcciones
- **macOS “dañada”:** la app de macOS decía que estaba “dañada y no se puede abrir”, porque el paquete salía sin firma. Ahora el paquete se firma (ad-hoc) y se comprueba en la compilación; macOS solo pide permitir la app en Ajustes › Privacidad y seguridad. Quien descargó una versión anterior puede ejecutar `xattr -dr com.apple.quarantine "/Applications/PKHeX Modern.app"`.
- **Incursiones de Sword/Shield:** editar una incursión (por ejemplo, desactivar una de evento) bloqueaba y cerraba la app. La pantalla y el editor entraban en un bucle sin fin; ahora cada campo ignora el valor que ya tiene y la lista de tipos es fija. El mismo cuidado vale para las seeds de las incursiones de Scarlet/Violet.
- **Editor congelado (recuerdos):** con un Pokémon de la Gen 6/7 que tiene recuerdos, el editor podía congelar la interfaz tras un cambio (visto al cambiar el encuentro de un Pokémon de Alpha Sapphire). Las listas de sentimiento y de detalle del recuerdo se recreaban en cada actualización y entraban en un bucle con el modo legal; ahora son fijas.

## 0.4.28 — 2026-10-16

### 📱 Android
- **Partidas de los emuladores:** el botón “🎮 Partidas de los emuladores” de la página Partidas encuentra solo las partidas de RetroArch, DraStic, melonDS, Dolphin, Azahar/Citra/Lime3DS, Eden/Yuzu, mGBA, My Boy! y Pizza Boy, y las que están junto a las ROM. Pide una vez el “acceso a todos los archivos” y guarda en el archivo original, con copia de seguridad.

### ✨ Legalidad
- **Set de Showdown legal:** en modo legal, pegar un set ilegal genera un Pokémon legal de este juego manteniendo IV, habilidad, naturaleza, sexo, shiny, EV, movimientos y Teratipo siempre que se pueda (Entrenamiento Extremo, mentas y cápsulas cuando el juego lo permite), y avisa de lo que no se pudo mantener.
- **Movimientos de MT/DT en Legalizar:** en Sword/Shield y Scarlet/Violet, Legalizar marca el registro de MT/DT, así que los movimientos de MT del Pokémon ya no se cambian por otros.

### 📦 Cajas
- **Bloquear caja:** en “⋯ Caja”, la caja queda protegida contra arrastrar, eliminar, aplicar, pegar, edición por lotes, fábrica de huevos y Living Dex hasta desbloquearla; “Ordenar todas las cajas” se salta las bloqueadas. Candado en la pestaña y bloqueo guardado por partida.

### ✏️ Editor
- **Naturaleza manteniendo el shiny (Gen 3/4):** cambiar la naturaleza de un shiny ya no le quita el shiny; en modo legal, el Pokémon se genera de nuevo con la naturaleza nueva, legal y shiny (antes el cambio se deshacía).

### 🎒 Bolsa
- **Llenar bolsillo:** conjuntos listos con todas las MT, todas las Poké Balls, todas las medicinas, todas las bayas o todo en el bolsillo abierto, con 99 de cada uno y en orden numérico.

## 0.4.27 — 2026-10-16

### 🎨 Interfaz
- **Iconos vectoriales:** la barra lateral y los atajos del Inicio usan iconos de trazo dibujados por la app, en el color del texto (y en el color de acento en la página actual), en lugar de los emojis, que cambiaban de tamaño y de estilo en cada sistema, sobre todo en Android.
- **Barra lateral en grupos:** Pokémon (Cajas, Equipo, Bank, Pokédex), Entrenador (Entrenador, Bolsa), Obtener (Encuentros, Eventos) y Herramientas (Partidas, Juego, Búsqueda, Edición por lotes). El orden y los atajos Ctrl+1 a Ctrl+9 no cambiaron.
- **Estado de la partida en el Inicio:** debajo de la tarjeta del juego, cambios sin guardar (clic para verlos y guardar), última copia de seguridad (clic para abrir las copias), huecos libres en las cajas y el archivo.
- **Resumen de la colección en la Pokédex:** sin especie elegida, el panel derecho muestra barras de Living dex, Shiny dex, capturadas y vistas, cuántas faltan en cada generación (clic para listar solo las que faltan) y el botón “Siguiente que falta”; “‹ Resumen” vuelve a él.

### 🥚 Fábrica de huevos
- **Huevos de las que faltan:** en el resumen de la Pokédex, un botón genera un huevo legal de cada especie que falta en la partida abierta (de la forma básica: Charmander para Charizard, Pichu para Pikachu); otro genera un huevo de cada familia. Vienen de los huevos de crianza del propio juego, con tu entrenador, y van a los huecos libres de las cajas, con la lista antes de guardar y un único Ctrl+Z. Funciona de la Gen 2 a la Gen 9 (en una partida de Black, 595 huevos en menos de un segundo).

### 🔒 Desafíos
- **Modo Hardcore:** en el ⚙, “Modo Hardcore (solo mover Pokémon)” desactiva toda edición de datos, para desafíos como el Nuzlocke y para quien juega con RetroAchievements. El editor solo muestra los datos; crear, copiar, importar, pegar Showdown, legalizar, generar encuentros y eventos y completar la Pokédex quedan desactivados, y Entrenador, Bolsa, Juego y Edición por lotes salen de la barra lateral. Mover, cambiar, liberar, ordenar y usar el Bank siguen funcionando. Una tarjeta roja en lugar del modo legal muestra que está activo.
- **Informe de Nuzlocke:** nueva pestaña en la Pokédex con el primer Pokémon capturado en cada ruta, las capturas de más, los repetidos (familia ya capturada antes), quién está en el equipo, vivo o muerto (por la caja del cementerio, elegida por el nombre) y un botón para copiar el informe en texto. Los intercambios, otros juegos y huevos quedan fuera; de la Gen 4 en adelante el orden sigue la fecha de captura.

### 🐛 Correcciones
- **Pokédex con fuentes anchas:** el nombre de la especie en el panel se encoge en vez de partirse por la mitad ("Ivysa/ur"), y "No hay ningún Pokémon de esta especie guardado" ya no se corta.

## 0.4.26 — 2026-10-15

### 📱 Móvil y pantallas pequeñas
- **Nombre en el editor:** con fuentes anchas (como Pokémon GB), el nombre del Pokémon se encoge para caber y el sexo ♂/♀ y la estrella de shiny quedan siempre visibles; el lugar ("equipo, posición 1") se acorta con "…".
- **Tarjetas del Equipo:** los nombres largos (Blastoise, Charizard) se encogen en vez de cortarse.
- **Inicio:** las tarjetas de una misma fila tienen la misma altura.
- **Barra lateral:** todas las páginas con la misma altura (los emojis de Android tenían alturas distintas y desalineaban la lista), y una línea separa el "Inicio" de la lista cuando se desplaza.
- **Cajas y Pokédex:** las pestañas de las cajas ganaron espacio respecto a las de las partidas, y el panel de detalles de la Pokédex es más estrecho en pantallas pequeñas, para que los filtros no se corten.

## 0.4.25 — 2026-10-15

### 🎨 Interfaz
- **Cajas más compactas:** la cabecera de la caja quedó en una sola línea (‹ nombre · recuento · ⋯ Caja ›) y Renombrar, fondo, Showdown y CSV pasaron al menú **⋯ Caja**. En portátiles (1280×720) aparecen casi dos filas de casillas en vez de una y media; en ventanas de 1100 px, cuatro columnas en vez de tres.
- **Panel del editor adaptable:** 360 px en ventanas de menos de 1400 px (410 px en las mayores), y la franja del equipo muestra solo los sprites cuando falta espacio, en vez de nombres cortados.
- **Bolsa con nombres de bolsillo en portugués:** Objetos, Objetos clave, MT y MO, Medicinas, Bayas... (antes aparecían nombres internos, como "KeyItems" y "TMHMs").
- **Edición por lotes:** Vista previa y Aplicar quedan fijos abajo de la columna, siempre visibles.
- **Guardar ▾:** "Guardar como..." salió de la barra lateral y pasó a ser la flecha junto a Guardar, en el pie; "Abrir partida..." solo destaca cuando no hay ninguna partida abierta.
- **Un botón principal por pantalla:** el "Usar" de las tarjetas de Encuentros y Eventos quedó neutro (el panel de detalles tiene "Usar este encuentro"), y el pie ya no repite las acciones que ya están arriba de la página (Actualizar, Sincronizar, Usar encuentro, Actualizar vinculados, Aplicar Bolsa).
- **Eliminar con aspecto de acción destructiva:** contorno rojo en vez de fondo, para no confundirse con Guardar.
- **Mismo vocabulario en toda la app:** "Aplicar" lleva el cambio a la partida abierta y "Guardar" graba el archivo ("cambios sin guardar", "Recuerda guardar", "Aplicar Bolsa"); "exportar" quedó solo para archivos .pk*.
- **Bolsa más limpia:** muestra los objetos que tienes y algunos huecos libres para añadir, en vez de decenas de "(ninguno)"; "Mostrar todos los huecos" muestra el bolsillo entero.
- **Detalles:** los atajos de la barra lateral solo aparecen al pasar el ratón o en la página actual; los filtros de Encuentros dicen "Todos los encuentros" y "Todas las versiones"; los atajos Juego y Edición por lotes del Inicio ganaron descripción; la Ayuda tiene texto con más contraste y líneas más cortas.

### 🐛 Correcciones
- **Ayuda en la barra lateral:** con la Ayuda abierta, la página anterior ya no queda marcada; es el botón Ayuda el que destaca.

## 0.4.24 — 2026-10-14

### ♟ Entrenador
- **Medallas, sexo, fichas, PB y fechas:** la página Entrenador ahora edita las medallas de cada región (Gen 1 a 6, Sword/Shield y BD/SP, con “Todas” y “Ninguna”), el sexo del entrenador, las fichas del Game Corner (Gen 1 a 4), los Battle Points (Emerald, Gen 4 a 7 y Sword/Shield) y las fechas de inicio de la aventura y del Hall de la Fama (Gen 4 y 5).

### 📖 Pokédex
- **Completar la Pokédex de la partida:** nuevo botón en la página Pokédex para marcar todas las especies como vistas y capturadas en la partida abierta (con formas, sexos e idiomas donde el juego los guarda, y shiny vistos si quieres), marcar solo como vistas, quitar las capturadas o limpiar todo. De la Gen 1 a Z-A (menos Legends: Arceus), con Ctrl+Z.

### ⚡ Atajos
- **Desbloquear todo:** nuevos atajos en la página Juego para lo que es pesado de conseguir: todos los O-Powers y todos los entrenamientos del Super Training (X/Y y Omega Ruby/Alpha Sapphire), todas las Zygarde Cells (Sun/Moon y Ultra Sun/Ultra Moon), objetos del Grand Underground al máximo (BD/SP) y la bolsa de esferas del Underground llena (Diamond/Pearl/Platinum).

## 0.4.23 — 2026-10-13

### 🔄 PKHeX actualizado
- **Correcciones del PKHeX (kwsch, hasta el 13/10):** las partidas de HeartGold/SoulSilver guardan la versión correcta al abrir; Dratini con ExtremeSpeed por huevo en HeartGold/SoulSilver deja de salir ilegal; el PSS de X/Y lee todos los contactos; traducciones actualizadas.
- **Fondos que faltaban:** Ruby/Sapphire y FireRed/LeafGreen con los 16, Emerald con los 17 (los de Walda), los nombres correctos de cada uno, las cajas de Colosseum/XD y de Battle Revolution con el fondo de cada caja y el "My Wallpaper" dibujado en Pokémon Box.

## 0.4.22 — 2026-10-10

### 🎨 Apariencia
- **Tema ORAS (Pokédex):** azul verdoso inspirado en la Pokédex y el PokéNav Plus de Omega Ruby/Alpha Sapphire, con el color de acento Agua.
- **Tema Kanto (Pokédex clásica):** rojo y crema de la Pokédex de Red/Blue.

### ⚙ Preferencias
- **Tamaño de los sprites:** Pequeño, Normal, Grande o Enorme en las cajas, el bank, la otra partida y la Pokédex, desde el ⚙.
- **Legalizar al hacer clic opcional:** la opción “Ofrecer legalizar al hacer clic en uno ilegal” en el ⚙ desactiva la pregunta; el Pokémon solo se abre en el editor y el botón Legalizar sigue disponible.

### 🌐 Proyecto
- **Sitio del proyecto:** [carlosmozart.github.io/PKHeX-Modern](https://carlosmozart.github.io/PKHeX-Modern/), con las funciones, capturas y descargas de cada sistema.

## 0.4.21 — 2026-10-08

### 🎮 Juego
- **Battle Frontier:** nueva pestaña en Emerald, Platinum y HeartGold/SoulSilver (y la Battle Tower de Diamond/Pearl), con PB, Frontier Pass y símbolos de plata/oro (Emerald), impresiones de cada instalación (Gen 4) y las rachas de cada instalación, modo y nivel, incluidos los cambios de la Factory, los CP del Castle y la racha en curso.
- **Pokétch y Pokéwalker:** desbloquear las aplicaciones del Pokétch y elegir la de la pantalla (Diamond/Pearl/Platinum); watts, pasos y rutas del Pokéwalker (HeartGold/SoulSilver).
- **Medallas del Pokéathlon:** las cinco medallas de cada una de las 493 especies, con búsqueda, filtro de incompletas y “Todas las medallas”.
- **Base Secreta:** en Ruby/Sapphire/Emerald, edita el dueño de cada base y muestra su equipo; en Omega Ruby/Alpha Sapphire, edita textos, nivel y banderas de tu base, da todas las decoraciones y borra bases de otros jugadores.

## 0.4.20 — 2026-10-07

### 📖 Pokédex
- **Solo la partida abierta:** un botón en la Pokédex muestra solo lo que la partida abierta ha visto, capturado y tiene guardado, sin mezclar con las otras partidas ni con el bank.

### 🎒 Bolsa
- **Todas las MT:** un botón pone todas las MT del juego en el bolsillo de MT, con 99 de cada una y en orden numérico, sin quitar lo que ya había.

### 🐛 Correcciones
- **Errantes de la Gen 3:** Entei, Raikou y Suicune de FireRed/LeafGreen y Latias/Latios de Ruby/Sapphire generados en Encuentros venían siempre con los IV a cero. Ahora los IV se sortean como en el juego, que por un bug solo guarda PS 0-31 y Ataque 0-7.
- **Pokédex en ventanas pequeñas:** los botones, la búsqueda, los filtros y las generaciones se ajustan al ancho, sin cortar el título ni el campo de búsqueda.

## 0.4.19 — 2026-10-06

### 🔎 Búsqueda
- **Auditoría de duplicados:** copias idénticas por datos almacenados, agrupación de copias de seguridad por par de archivos (ZIP o carpetas de partidas con muchos Pokémon iguales) y vinculados/variantes registrados en el Bank, con comparación y navegación de solo lectura. Lectura cancelable, incluidos los cambios sin guardar.

### 📖 Pokédex
- **Aplicar Living Dex:** vista previa de la organización en las cajas de la partida abierta, con primera caja, reserva de los que faltan, copias externas, conservación de ocupantes y casillas protegidas, confirmación y un único deshacer. Los orígenes modificados exigen una nueva vista previa.

## 0.4.18 — 2026-10-06

### 📱 Android
- **Pantalla completa:** la app oculta la barra de estado y la de navegación, ganando espacio en la pantalla. Deslizar desde el borde muestra las barras un momento; el recorte de la cámara sigue libre.

### 🔗 Proyecto
- **Nueva dirección:** el repositorio ahora se llama [PKHeX-Modern](https://github.com/carlosmozart/PKHeX-Modern). Los enlaces antiguos siguen funcionando (GitHub redirige) y la actualización automática sigue con normalidad.

## 0.4.17 — 2026-10-05

### ✏️ Editor
- **Evolución asistida:** Evolucionar reúne los métodos y ramificaciones del juego abierto, con sprite, requisitos y vista previa cancelable. Quita la Everstone, cumple los requisitos del Pokémon y deja el resultado pendiente hasta Aplicar, con deshacer y protección del modo legal.

### 📦 Cajas y equipo
- **Informe CSV:** exporta el equipo y las cajas de la partida abierta o todas las cajas del banco elegido, con datos, IV, EV, movimientos y legalidad, sin alterar los originales. Cabeceras PT/EN, UTF-8 con BOM y selector de archivos también en Android.
- **Equipo de Showdown:** el botón “📋 Showdown” en Equipo y en Cajas pega varios sets a la vez en los huecos del equipo o en las casillas vacías de la caja, con una lista de revisión antes (legal, ilegal o fuera, con el motivo) y un único deshacer. También copia el equipo o la caja entera en formato Showdown.

### 📖 Colección
- **Living Dex:** el plan usa el número de casillas por caja del juego objetivo (20 en la Gen 1/2, en vez de siempre 30), y los demás Pokémon de la misma especie aparecen como “otros ejemplares”, sin sugerir que son copias.

### 🔧 Correcciones
- **Búsqueda:** lee una copia de las partidas abiertas (no le disputa la partida al editor mientras busca) y ya no crea bancos ni cajas vacíos en el Bank.

## 0.4.16 — 2026-10-05

### ⚙️ Rendimiento y diagnóstico
- **Editor:** el estado, el informe, los avisos y los grupos de legalidad comparten un análisis por actualización.
- **Diagnóstico de la carpeta:** los recuentos pasan como datos, sin depender del idioma del resumen y sin incluir nombres de archivos.

### 💾 Copias de seguridad
- **Motivo e integridad:** las copias nuevas registran operación, juego, tamaño y SHA-256. La lista avisa cuando el archivo cambió y Restaurar permite comprobar antes de continuar. Las copias antiguas siguen disponibles; los registros acompañan a la limpieza de las copias, también en el almacenamiento privado de Android.

### 🏦 Bank
- **Consulta de solo lectura:** toca un Pokémon para ver sus datos, sprite shiny, características, movimientos, origen y legalidad. Exporta, copia Showdown, consulta variantes o abre una copia en el editor de una partida compatible, conservando el original.

### 🎨 Preferencias
- **Vista previa de los temas:** los cuatro temas aparecen en tarjetas con miniaturas reales de las cajas y de la cabecera del editor, renderizadas por Avalonia y guardadas en caché para claro y oscuro.
- **Fondo de las cajas:** elige Normal, Suave o Desactivado para facilitar la lectura de las casillas. La preferencia se conserva y no altera el fondo del juego.

### 📱 Tacto en Android
- **Selección táctil:** las cajas y el Bank tienen el modo “☐ Seleccionar”, también abierto con una pulsación larga, con recuento y acciones para el grupo. Al salir del modo se borran las marcas.
- **Consejos sin ratón:** mantén pulsado un botón para ver su descripción sin ejecutarlo; las tarjetas de tema y el modo de arrastre tienen ⓘ. El arrastre táctil solo empieza tras mantener el dedo quieto; las listas usan inercia e indicador de desplazamiento visible.

### 📖 Colección
- **Planificador de Living Dex:** nueva sección en la Pokédex, con juego objetivo, formas, shiny y fuentes. Muestra el recuento, los mejores candidatos, duplicados, los que faltan y un plan nacional de 30 casillas por caja. La lectura muestra el progreso; no se mueve ningún Pokémon ni se guarda ninguna partida.

### 📋 Diagnóstico
- **Compartir diagnóstico:** Ayuda › Acerca de muestra una vista previa con versión, sistema, idioma, tema y registro reciente. Android abre el compartir del sistema; el ordenador copia el texto. Los mensajes libres y los datos privados se omiten.

### ↔ Pokémon
- **Legalidad por tema:** los motivos aparecen en grupos por gravedad, con recuento y el primero no válido abierto. Movimientos, Poké Ball, Cintas y Encuentro tienen acciones de corrección ligadas al problema (“Poké Ball legal” elige una que combine con el color del Pokémon, como en la Edición por lotes).
- **Vista previa de los cambios:** Legalizar muestra los campos antes y después, con énfasis en los cambios importantes y el encuentro del que se generó el Pokémon. La confirmación usa el candidato mostrado, sin generar otro. Las transferencias entre generaciones solo piden confirmación cuando cambian algo importante (shiny, PID, IV, naturaleza, entrenador, mote, Poké Ball o movimientos); los lotes muestran un resumen por campo y la lista completa.

### ⚙ Edición por lotes
- **Legalizar por lotes:** nueva acción rápida “Legalizar”, que genera de nuevo, a partir de un encuentro real del juego, los Pokémon que estén ilegales (después de las otras acciones), manteniendo naturaleza, nivel, objeto, mote y movimientos cuando se puede. La vista previa marca los legalizados (✨) y cuenta los que no se pudo; “Aplicar” graba exactamente lo que se previsualizó.
- “Ball legal” pasó a llamarse **“Poké Ball legal”**.

## 0.4.15 — 2026-10-05

### ⬆ Actualización
- **Animación de la actualización:** mientras se descarga la versión nueva, una fila de Poké Balls grises gana color (Poké, Great, Ultra... hasta la Master Ball al 100 %) y el equipo de la partida abierta camina en fila sobre la barra — de vez en cuando uno de ellos aparece shiny. “Seguir usando” oculta el panel y la descarga continúa. Para verla sin necesidad de una versión nueva: Ayuda › Acerca de › “✨ Ver animación”.
- **Actualizar estando algunas versiones atrás:** si la versión más nueva aún no tiene el paquete de tu sistema (el APK de Android se sube unos minutos después), la app ofrece la más reciente que ya lo tiene, en vez de dar error.

### 💾 Copias de seguridad
- **Copias de seguridad separadas por partida:** las partidas de carpetas distintas con el mismo nombre (como el “main” de Switch) ahora tienen cada una su límite de 20 copias; antes una podía borrar las copias de la otra.

### 📱 Android
- Conectar un mando o teclado Bluetooth ya no reinicia la app (ni pierde la edición en curso).

## 0.4.14 — 2026-10-04

### ✨ Legalidad
- **Legalizar al hacer clic:** al hacer clic en un Pokémon ilegal de las cajas o del equipo, la app pregunta si quieres legalizarlo. “Legalizar” genera de nuevo el Pokémon a partir de un encuentro real y lo graba en la casilla (Ctrl+Z deshace); “Solo abrir” lo abre en el editor como antes y no vuelve a preguntar por ese Pokémon hasta cerrar la app.

### 🎮 Página Juego
- **Ropa de Scarlet/Violet:** catálogo por categoría y sexo, búsqueda por categoría en inglés o ID, desbloqueo y bloqueo individuales. Desbloquear toda o una categoría pide confirmación y se puede deshacer. El Core no proporciona los nombres comerciales de las prendas.
- **Donuts de Z-A:** lista, edición de bayas, estrellas y poderes, generador por efecto, duplicación y eliminación. Llenar la bolsa pide confirmación y tiene un paso de deshacer.
- **Errantes:** editor de la Gen 3 y X/Y con reaparecer, datos guardados y deshacer. El modo legal valida los encuentros y la correlación PID/IV; Ruby/Sapphire/FireRed/LeafGreen muestran los IV truncados por el bug del juego.
- **Reloj de la Gen 3:** pestaña para Ruby/Sapphire/Emerald con los relojes inicial y transcurrido, corrección del reloj parado con el método del PKHeX y avance de un día. Cada edición se puede deshacer.
- **Deshacer y rehacer:** flags, valores, récords, atajos, Hall de la Fama, tarjetas e incursiones pasan a tener historial propio en los botones ↶/↷ de la página Juego. El consejo indica la acción; las operaciones masivas forman un paso. Hasta 30 pasos y 64 MB de diferencias, sin revertir ediciones de otras páginas.

## 0.4.13 — 2026-10-04

### ✏️ Editor
- **Pokérus** (pestaña Extras): sin Pokérus, infectado o curado, “Dar Pokérus”, cepa y días. El modo legal solo lo permite donde el Pokérus es posible.
- **Hidden Power** (pestaña Características, Gen 2–7): tipo y potencia; elegir el tipo ajusta los IV, y el modo legal lo rechaza cuando el Pokémon quedaría ilegal. El movimiento muestra el tipo calculado.
- **QR de la Gen 7**: un botón en el editor genera el QR para el QR Scanner de Sun/Moon, con “Guardar imagen”. Las tarjetas de la Gen 6/7 también generan QR en el formato del PKHeX.

### 📦 Cajas y equipo
- **Cajas en carpetas** (botón ⋯ en la página Cajas): exportar esta caja o todas como archivos .pk* (con una subcarpeta por caja, si quieres) e importar una carpeta a las cajas, con conversión de generación, oferta de legalizar el lote, lista de lo rechazado y un único Ctrl+Z.
- **Guardería** (debajo del equipo): editar los Pokémon de la guardería, poner y quitar (con deshacer), huevo en espera, experiencia y semilla según el juego. Incluye las dos guarderías de Omega Ruby/Alpha Sapphire y Sword/Shield.

### 🎮 Página Juego
- **Tarjetas de evento de la partida**: los Regalos Misteriosos que esperan ser recibidos en el juego. Importar, exportar, borrar, marcar como no recibido y “Añadir a la partida” desde la página Eventos, con las reglas de cada generación (Gen 4 a 7 y Let's Go).
- **Incursiones**: guaridas de Sword/Shield (Galar, Isle of Armor, Crown Tundra) y Teraincursiones de Scarlet/Violet (Paldea, Kitakami, Blueberry), con activa, tipo, estrellas, seed y registros de 7 estrellas; “Activar todas” y “Desactivar todas” piden confirmación.

### 📱 Android
- La carpeta de partidas funciona con todas las partidas de la carpeta elegida. Si elegiste la carpeta antes de la 0.4.10, toca “Elegir carpeta...” y elígela de nuevo (esas versiones no guardaban la elección).

## 0.4.12 — 2026-10-03

### 📱 Android
- **Carpeta de partidas**: después de elegir la carpeta, tocar Actualizar o abrir la app, la barra de estado muestra un resumen de la lectura (cuántos archivos tiene la carpeta, cuántos se leyeron como partida, cuántos se ignoraron y el primer error, si lo hay). Un archivo o subcarpeta con error ahora se salta sin interrumpir la lectura de los demás, y el error va al crash.log. Esto ayuda a investigar por qué algunas partidas de la carpeta aún no aparecen.

## 0.4.11 — 2026-10-03

### 🔧 Preparación
- La actualización automática pasa a aceptar también el nombre futuro del repositorio (`carlosmozart/PKHeX-Modern`). Así, cuando el proyecto se renombre en GitHub, quien esté en esta versión o una más nueva seguirá recibiendo actualizaciones sola.

### ⚠️ Problema conocido
- En Android, la carpeta de partidas aún no lista todas las partidas de la carpeta elegida. Se está investigando; mientras tanto, abre la partida que falte con el botón Abrir.

## 0.4.10 — 2026-10-03

### 🎮 Editores por juego
- **Hall de la Fama editable** (pestaña ★ de la página Juego): haz clic en un Pokémon para cambiar especie, mote y nivel (en Sun/Moon, solo la especie); en X/Y y en Omega Ruby/Alpha Sapphire también la fecha de la victoria y el shiny. Cada victoria ganó “Usar equipo actual” (lo cambia por los Pokémon del equipo de la partida) y “Borrar”, y “Registrar equipo actual” añade el equipo de la partida como victoria nueva, como al vencer a la Liga. La pestaña aparece incluso sin ninguna victoria aún.

### 📱 Android
- **Carpeta de partidas en Android**: en la página Partidas, “Elegir carpeta...” ahora abre el selector de Android y la app lee toda la carpeta, con subcarpetas, listando todas las partidas como en el ordenador (antes solo aparecían las abiertas una a una). “Actualizar” y abrir la app traen partidas nuevas o modificadas y quitan las que desaparecieron. Guardar graba de vuelta en el archivo de la carpeta, con copia de seguridad; una partida abierta o con cambios sin guardar no se reemplaza por la versión de la carpeta. El botón “Abrir carpeta” ya no aparece en Android.

## 0.4.9 — 2026-10-03

### 📱 Android
- **Bank en carpeta externa en Android**: en Bank › Añadir carpeta, elige una carpeta del móvil con archivos .pk* (de otra app o sincronizada). Aparece como 📁 en el Bank, como en el ordenador. La app sincroniza: al abrir, trae lo que cambió en la carpeta, y cada Pokémon guardado o borrado en el Bank va a la carpeta al momento. Si un archivo cambió en los dos lados, se queda la versión de la carpeta y la de la app va a las copias de seguridad.
- **Actualización automática en Android**: al abrir, la app avisa cuando sale una versión nueva y muestra las novedades. Con “Actualizar ahora”, descarga el APK de la release, comprueba el archivo (SHA-256) y abre el instalador de Android, que instala encima sin perder nada. La primera vez, Android pide permitir la instalación de apps desde PKHeX Modern. Se puede desactivar en Ayuda › Acerca de.
- Los enlaces de la Ayuda (releases, código, AllGenWiki) ahora abren en el navegador del móvil.
- El elemento Inicio de la barra lateral tampoco muestra ya el atajo Ctrl+0.

## 0.4.8 — 2026-10-03

### 📱 Android
- Los símbolos ♂/♀ y los iconos de los botones (Crear, Importar, Eliminar, Exportar...) aparecían como "≣" en Android. Ahora la app trae las fuentes de esos símbolos (Noto Sans Symbols 2 y Noto Emoji), sin depender de las fuentes del móvil.
- Volvieron a funcionar en Android: hacer clic en un resultado de búsqueda, abrir una partida desde el Save Manager, el panel del Bank, "Actualizar vinculados", "Volver" en la Ayuda y el color de acento.
- La lista de páginas de la barra lateral ya no deja una línea dibujada encima de las otras al desplazarse.
- En Android ya no aparecen los atajos de teclado (columna Ctrl+N, botón ⌨, "(Ctrl+F)" en la búsqueda y la sección Atajos de la Ayuda).

### ✨ Ajustes
- El botón Ayuda de la barra lateral quedó solo con el texto, sin cortarse en pantallas estrechas.
- El README muestra los sistemas compatibles (Windows, Linux, macOS y Android) y una captura de la app en el móvil.

## 0.4.7 — 2026-10-03

### 📱 Android
- **PKHeX Modern ahora tiene versión para Android** (APK en la release). La interfaz es la misma que en el ordenador, con la pantalla en horizontal: barra lateral, cajas y editor lado a lado. En móviles todo se reduce para caber; en tabletas queda en el tamaño normal.
- Las partidas se abren con el selector de archivos de Android, sin pedir acceso a todo el almacenamiento. La app edita una copia propia y, al **Guardar**, graba de vuelta en el archivo original y comprueba la grabación; el original anterior va a las copias de seguridad. Si el archivo cambió en otra app, no se sobrescribe nada.
- Importar y exportar Pokémon, Guardar como y las partidas dentro de .zip también funcionan con el selector. El botón Atrás de Android cierra la pregunta abierta o vuelve como Esc.
- Fuera de esta primera versión: actualización automática (instala el APK nuevo encima), explorador de carpetas y bank en carpeta externa.

## 0.4.6 — 2026-10-03

### 🖥️ Linux y macOS
- **PKHeX Modern ahora funciona en Linux y macOS**, además de Windows (el PKHeX original es solo para Windows). La release trae cuatro paquetes: Windows x64, Linux x64 (sirve también para Steam Deck), macOS Apple Silicon y macOS Intel. Ninguno necesita .NET instalado.
- En macOS la app viene como `PKHeX Modern.app`. No tiene firma de Apple, así que en la primera apertura macOS pide permitirla: clic derecho en la app › Abrir, o Ajustes › Privacidad y seguridad › Abrir igualmente. El README lo explica, junto con las dependencias de Linux.
- La **actualización automática** descarga el paquete correcto de cada sistema. En Windows nada cambia; en Linux se mantiene el permiso de ejecución; en macOS se reemplaza la app entera.
- Los sprites ahora se montan sin depender de Windows: son los mismos PNG y el mismo montaje del PKHeX, rehechos con SkiaSharp y comprobados píxel a píxel contra el original (nada cambia en la pantalla).
- La Ayuda dice dónde están las preferencias, las copias de seguridad y el bank en cada sistema (`~/.config/PKHeX.Modern` en Linux y macOS).

### 🐞 Correcciones
- La actualización automática sigue funcionando cuando el archivo del programa se renombró (ej.: `PKHeX.Modern (1).exe`).

## 0.4.5 — 2026-10-03

### 🎮 Editores por juego
- **Eventos de la Gen 1, Let's Go y Z-A** en la página Juego. Gen 1: las 2560 flags y 256 valores, con nombre en las flags de los Pokémon fijos del mapa. Let's Go: las 4096 flags y 1000 valores, con los nombres de la lista del PKHeX (Snorlax, aves legendarias...) y el tipo en el código (Z zona, S sistema, V objeto del mapa, E evento). Z-A: las tablas de eventos, sistema, objetos del mapa, misiones, tareas de Mable y títulos; el juego guarda solo el hash de cada evento, así que los objetos del mapa (Colorful Screws y MT) son los únicos con nombre. La búsqueda ahora también encuentra por código.
- **Nuevos atajos**: Pokémon fijos del mapa de nuevo en la Gen 1 (Mewtwo, Articuno, Zapdos, Moltres, Voltorbs de la Power Plant, Eevee, fósiles, regalos de Yellow...), todos los títulos de Maestro Entrenador en Let's Go, Colorful Screws y MT del mapa en Z-A, estuche lleno de Pokécubos (Ruby/Sapphire/Emerald y Omega Ruby/Alpha Sapphire) y de Poffins (Diamond/Pearl/Platinum y BD/SP), y puntos, Data Cards y medallas del Pokéathlon (HeartGold/SoulSilver).
- **Hall de la Fama** (nueva pestaña ★, solo lectura): los equipos que vencieron a la Liga con sprite, mote y nivel en la Gen 1, Gen 3, X/Y y Omega Ruby/Alpha Sapphire (con la fecha), y las especies del primer equipo y del más reciente en Sun/Moon.
- Los atajos que dan objetos (Member Card de BD/SP, Colorful Screws, MT) ahora actualizan la Bolsa y la Pokédex al momento, sin perder lo que se estaba editando en la Bolsa.

## 0.4.4 — 2026-10-03

### 🎮 Editores por juego
- **Eventos de Scarlet/Violet** en la página Juego: cerca de 650 flags y 280 valores de evento que el PKHeX conoce por el nombre (cada evento es un bloque de la partida), agrupados en categorías: **Vuelo** (puntos de vuelo), **Mapa**, **Recetas de MT**, **Funciones desbloqueadas**, **Compras**, **Estacas (Treasures of Ruin)**, **Subasta**, **Brotes masivos**, **Combates y desafíos**, **Academia**, **Historia y eventos** y otras. La búsqueda, los filtros y “Activar mostradas” funcionan como en los otros juegos.
- **Eventos de BD/SP**: las 4000 flags, las 1000 flags de sistema (mostradas como S#0000) y los 500 valores de evento, con los nombres que el PKHeX conoce.
- **Atajos de BD/SP** (nueva pestaña ⚡ Atajos): revancha con Dialga/Palkia, eventos de **Darkrai**, **Shaymin** y **Arceus** (con el objeto, la Pokédex Nacional y las flags correctas), **Spiritomb**, **Mesprit** y **Cresselia** errantes de nuevo, todas las áreas del mapa y toda la ropa. Cada atajo explica lo que hace, pregunta antes y queda desactivado cuando no hay nada que hacer.

### 🗂 Partidas
- **Tus partidas en la barra lateral**: sin ninguna partida abierta, la barra lateral lista las partidas de la carpeta del Save Manager (etiqueta del juego, nombre y archivo) y un clic abre la partida. El “Guardar como...” desactivado desaparece hasta que haya una partida abierta.

## 0.4.3 — 2026-10-03

### 🎨 Temas
- **Fuente pixelada en el tema Pixel**: Consolas (fuente de terminal, ancha) dio paso a **Pixelify Sans**, una fuente pixelada con acentos, integrada en la app, con negrita de verdad. Aparece en los menús, etiquetas y títulos; las leyendas pequeñas (hasta 13 px) siguen en la fuente predeterminada, porque las fuentes pixeladas se emborronan en esos tamaños.
- **Elección de la fuente**: en el ⚙ de la barra lateral, la sección **Fuente** cambia la fuente de la interfaz al momento, en cualquier tema: **Del tema** (la predeterminada: Pixelify Sans en el tema Pixel, Inter en los otros), **Inter**, **Pixelify Sans** o **Pokémon GB/GBC**. Cada opción aparece escrita en su propia fuente, y la elección queda guardada.
- **Fuente del Pokémon de Game Boy**: la opción **Pokémon GB/GBC** usa Pokemon Classic, recreación hecha por fans de la fuente de Red/Blue/Yellow/Gold/Silver/Crystal (de TheLouster115, CC BY-SA 3.0), con acentos. Como las letras son anchas, se aplica en los títulos (nombre de la caja, nombre del Pokémon, títulos de las páginas, logo); el texto normal queda en Inter para no cortarse.

### 📦 Cajas
- **Pestañas de las cajas centradas**: cuando todas caben, la lista queda en el centro, alineada con el nombre de la caja. Cuando no caben (ej.: 24 o 32 cajas), se desplaza sola para dejar la caja abierta en el medio, incluso al cambiar de caja con las flechas o el teclado.

## 0.4.2 — 2026-10-03

### 🧭 Barra lateral
- **Más compacta**: las 13 páginas caben sin desplazarse en la ventana predeterminada (y a partir de 720 px de altura). Los elementos de la lista son más bajos, la tarjeta del juego más pequeña y el modo legal pasó a una sola línea.
- **Preferencias en el ⚙**: tema, color de acento, idioma y “Abrir la última partida al iniciar” están en el botón ⚙ de la fila inferior, junto a ❔ Ayuda, ⌨ (atajos de teclado) y ◐ (claro/oscuro).
- “↺ Última” aparece solo sin partida abierta (con una partida abierta, las pestañas y el Inicio ya muestran las recientes).

## 0.4.1 — 2026-10-03

### 🎮 Editores por juego
- **Página Juego** (nueva): **flags de evento** de la Gen 2 a la 7 (objetos ocultos ya recogidos, entrenadores vencidos, puntos de vuelo, historia, encuentros, regalos...) con los nombres de las listas del PKHeX, búsqueda por nombre o número, filtro por categoría y “Activar/Desactivar mostradas” para cambiar de una vez todo lo que muestra el filtro (con confirmación).
- **Valores de evento**: contadores y estados de cada evento, con los valores conocidos en una lista y el número libre al lado.
- **Récords del entrenador** en la Gen 3, 5, 6, 7, Sword/Shield y BD/SP: pasos, combates, capturas, huevos eclosionados y los demás contadores del juego.

## 0.4.0 — 2026-10-03

### ⚙ Edición por lotes
- **Batch Editor** (nueva página): cambia varios Pokémon de la partida abierta a la vez. Elige el grupo (caja actual, todas las cajas, equipo, cajas y equipo, Pokémon seleccionados con Ctrl+clic o los **resultados de la Búsqueda** de esta partida) y qué hacer: acciones rápidas (nivel 100, IV máximos, poner EV a cero, amistad máxima, curar PS y PP, hacer shiny, movimientos sugeridos, Ball legal) y/o el **script del Batch Editor del PKHeX** (`=Species=Pikachu` filtra, `.CurrentLevel=100` cambia), con una lista de propiedades para insertar.
- **Vista previa** muestra cada Pokémon con lo que va a cambiar (nivel, IV, EV, shiny, movimientos, Ball...) y si sigue siendo legal, sin grabar nada. **Aplicar** pregunta antes, graba todo en un solo paso y **Ctrl+Z deshace** el lote entero.
- **Modo legal**: quien quedaría ilegal (⚠) se salta; con el modo legal desactivado, se graba igualmente y avisa.

### 🔎 Búsqueda
- **Base de datos / búsqueda** (nueva página, **Ctrl+Shift+F**): todos los Pokémon de las partidas abiertas (con los cambios sin guardar), de las partidas de la carpeta (incluso dentro de .zip) y del bank en una sola lista. Búsqueda por especie, mote, movimiento, objeto, habilidad, naturaleza, Ball, entrenador o juego (todas las palabras deben coincidir; `#25` busca por el número), filtros de origen, shiny, IV perfectos, naturaleza, Ball, nivel, generación de origen y huevos, y cinco órdenes. Al hacer clic en un resultado se va a la casilla, abriendo la partida en una pestaña si hace falta, o se abre la caja del bank.

### 💾 Partidas antiguas
- **Versión e idioma de la Gen 1–3 por el nombre**: la app descubre qué juego del par es (Red/Blue, Gold/Silver, Ruby/Sapphire, FireRed/LeafGreen) y el idioma por el nombre del archivo y ahora también por la **entrada del zip, el nombre del zip y la carpeta** (ej.: `Pokemon Sapphire/jogo.sav`, `Rouge/save.sav`). Antes, las partidas dentro de un zip no se identificaban (un FireRed/LeafGreen comprimido aparecía siempre como FireRed).
- **Etiqueta doble**: cuando no se puede saber, la partida aparece como el par (“Ruby / Sapphire”) con las dos mascotas en la etiqueta.

## 0.3.9 — 2026-10-03

### ⌂ Inicio
- **Pantalla de inicio de la partida**: al abrir una partida, la app muestra el Inicio (⌂ en la barra lateral, **Ctrl+0**), con la tarjeta del juego (entrenador, tiempo de juego, dinero y Pokédex), el **equipo arriba**, un **atajo grande** a cada página con un resumen y las **partidas recientes**, que se abren en una pestaña nueva.

### 🎨 Temas
- **Temas completos**: además del color de acento, el botón 🎨 cambia fondo, paneles, esquinas, bordes y fuente al momento. Cuatro temas, cada uno en claro y oscuro: **Predeterminado**, **PSS** (X/Y, azul y muy redondeado), **Pixel** (GBA, esquinas rectas, bordes gruesos y fuente de terminal) y **Z-A** (Lumiose, negro y lima). Cada tema aplica el color de acento sugerido (nuevo color: Lima).
- Barra lateral más compacta: claro/oscuro, tema e idioma en una sola línea, y los consejos de atajos en un “⌨ Atajos de teclado” (pasa el ratón).

### Correcciones
- **Guardar a veces no grababa**: con el ratón parado sobre 💾 Guardar, el consejo del botón se abría encima de él (en la esquina de la ventana no había espacio abajo) y el clic caía en el consejo. Los consejos de la barra inferior ahora se abren encima de los botones y ningún consejo captura clics.
- **Bolsa al Guardar**: los objetos cambiados en la Bolsa sin hacer clic en “Aplicar Bolsa” quedaban fuera de la partida y ni siquiera marcaban cambios. Ahora cuentan como cambio pendiente y Guardar (o cambiar de pestaña) los aplica antes de grabar.
- El botón “🌐 Idioma” ya no queda cortado en la barra lateral, y el consejo de Feebas explica lo que hace el botón de evolucionar.

## 0.3.8 — 2026-10-03

### 🌐 Interfaz
- **Menú de idiomas**: “🌐 Idioma” en la barra lateral cambia la interfaz entre **Português (Brasil)** (predeterminado) y **English**. La elección queda guardada y la app ofrece reiniciar para aplicarla. Los nombres del juego y los textos de legalidad siguen en inglés; las notas de versión y las descripciones de AllGenWiki solo existen en portugués (en inglés, las descripciones quedan ocultas).

### ♥ Editor
- **Feebas evoluciona con el botón**: “Evolucionar a Milotic” ahora cumple los requisitos solo: sube Beauty hasta 170, ajusta Sheen a la franja legal (Gen 3/4 y BDSP), quita la Everstone y sube un nivel. Solo queda bloqueado en el nivel 100 antes de la Gen 8 o en un Pokémon que nunca pasó por juegos con concursos (ej.: un Feebas nativo de Black/White).
- **Evolución por amistad** también cumple los requisitos: quita la Everstone y, en Sylveon de la Gen 6/7, sube el cariño a 2 corazones.

## 0.3.7 — 2026-10-02

### ♥ Editor
- **Feebas → Milotic**: evolución por Beauty en el Resumen, separada de la amistad, exige Beauty ≥ 170 y respeta el método disponible en el juego. BDSP usa Beauty; Z-A ofrece intercambio. Beauty no se aumenta automáticamente. El intercambio se puede simular sin Prism Scale; si lleva la escama, se consume. En modo legal, las evoluciones por Beauty e intercambio solo se aceptan tras validar una copia.
- **Evolución por amistad**: botones en el Resumen para evolucionar con los requisitos del juego. La amistad se aumenta solo hasta el mínimo necesario, y el nivel sube cuando la evolución lo exige. Día/noche se simula con el botón elegido. Se comprueban los huevos, la Everstone, los límites de nivel y los requisitos de Sylveon (movimiento Fairy, cariño en las Gen 6/7 y prioridad sobre Espeon/Umbreon). En modo legal, el resultado se prueba en una copia y solo se acepta si sigue siendo legal.

### Interfaz
- **Barra inferior contextual**: Cajas y Equipo muestran acciones de Pokémon; Partidas ofrece Abrir/Actualizar, Encuentros y Eventos permiten usar la selección, Bank actualiza los vinculados, Pokédex ofrece actualizar/sincronizar y Bolsa permite aplicar los objetos. Guardar sigue disponible en las páginas de la partida; Ayuda ofrece Volver.

### 🎁 Importación y guardado
- **Regalo Misterioso en una casilla**: suelta un archivo de evento (`.wc*`, `.pgf`, `.pcd` y otros formatos que el PKHeX reconoce) en una caja o en el equipo para generar el Pokémon. Reemplazar pregunta antes y Ctrl+Z deshace; los regalos de objetos se rechazan con una explicación.
- **Guardar después de editar**: Guardar y Ctrl+S ahora aplican la edición pendiente del Pokémon antes de grabar la partida, incluso dentro de ZIP. En modo legal, una edición que no se puede aplicar bloquea el guardado y explica el motivo. Los errores de grabación aparecen en una ventana y mantienen la partida marcada con cambios.

### 🔄 Actualizaciones
- **Novedades antes de actualizar**: la ventana muestra las notas publicadas en GitHub y ofrece “Actualizar ahora” o “Más tarde”. La oferta automática también espera tu elección antes de descargar e instalar. Al ejecutar desde el código, la ventana ofrece abrir la descarga en el navegador.

### 🗂 Partidas
- **Save Manager**: las tarjetas muestran el idioma identificado por el PKHeX y la fecha de inicio de la aventura cuando está disponible. Usa las flechas para navegar entre tarjetas y Enter para abrir; la opción “Ocultar SID” queda guardada en las preferencias. El recuento de archivos ignorados también aparece cuando la carpeta no contiene ninguna partida reconocida.

### 📦 Cajas
- **Nombre y fondo**: renombra la caja con el botón junto al título y elige el fondo del juego en la lista. Los nombres de las pestañas siguen el cambio y el fondo aparece discretamente detrás de las tarjetas. Las opciones respetan lo que admite cada juego; usa Guardar para grabarlo en la partida.

## 0.3.6 — 03/10/2026

### ✏️ Editor
- **Atajos de shiny** (como en el PKHeX): la estrella junto al nombre ahora se puede pulsar (☆ normal, ★ estrella, ◆ cuadrado). **Clic** hace shiny o quita el shiny; **Alt+clic** hace shiny manteniendo el PID (cambia el SID del entrenador original, así que los Pokémon de la Gen 3/4 con PID e IV ligados siguen legales); **Shift+clic** pide shiny cuadrado y **Ctrl+clic** shiny estrella (la diferencia solo existe a partir de la Gen 8). El botón "Hacer shiny" de la pestaña Extras acepta los mismos atajos y pasa a "Quitar shiny" cuando el Pokémon ya es shiny. Con el modo legal, Legalizar respeta el formato pedido.
- **Selector de forma con sprite**: cada forma aparece con su propio sprite (con el mismo shiny y sexo del Pokémon), en la lista y en la caja del campo.
- **Sexo pulsable**: haz clic en ♂/♀ junto al nombre para cambiar el sexo. Hasta la Gen 5 el sexo viene del PID, así que se genera un PID nuevo con la misma naturaleza (en modo legal, Legalizar lo rehace si hace falta). En las especies en las que la forma es el sexo (Meowstic, Indeedee, Basculegion, Oinkologne), forma y sexo cambian juntos, en los dos sentidos.
- **Deoxys en la Gen 3**: en vez de la lista de formas, el editor explica que la forma depende del juego (Normal en Ruby/Sapphire, Ataque en FireRed, Defensa en LeafGreen, Velocidad en Emerald).

### 🛡️ Legalidad
- **Legalizar quita los avisos "Fishy"**: un Pokémon legal con avisos de EV a cero después de subir de nivel o EXP justo en el límite del nivel (ej.: un Whismur nacido de huevo en Sapphire) ahora se corrige sin cambiar el encuentro ni el PID: gana EV como en combates y un poco de EXP camino del siguiente nivel. Cuando Legalizar tiene que generar de nuevo, el resultado también sale sin esos avisos.
- **Correcciones sugeridas en color** (movimientos en azul, recordar destacado, encuentro en ámbar, IV en verde) y comprobadas:
  - **Encuentro sugerido** funciona con Pokémon nacidos de huevo en la Gen 3 (antes decía "ningún encuentro posible"): prueba los encuentros reales del juego de origen y se queda con el que lo deja legal;
  - **IV máximos** usa la misma lógica que el botón de la pestaña Características (en modo legal, ofrece convertir en nacido de huevo);
  - en modo legal, una corrección que dejaría al Pokémon ilegal no se aplica y el motivo aparece en la barra de estado (antes se deshacía en silencio y el mensaje decía "aplicado").

## 0.3.5 — 03/10/2026

### 💾 Partidas
- **Guardado silencioso**: el botón **💾 Guardar** (y Ctrl+S) graba directamente sobre el archivo de la partida, sin abrir la ventana de Windows; el archivo anterior va antes a las copias de seguridad. Para elegir otro lugar, usa **Guardar como...** (Ctrl+E). Las partidas dentro de .zip se graban de vuelta en el zip.

### ✏️ Editor
- **Evolución por objeto**: en la pestaña Resumen, las especies que evolucionan con piedras y similares muestran el botón (ej.: Nidorino + Moon Stone → Nidoking, Eevee + Fire Stone → Flareon), respetando las que dependen del sexo.
- **IV máximos en la Gen 3/4**: en los encuentros salvajes de esos juegos los IV se generan junto con el PID, así que tener 31 en todos los IV es ilegal. Con el modo legal, si la especie nace de huevo, la app pregunta si puede **convertir en nacido de huevo** (de preferencia un huevo del propio juego), manteniendo naturaleza, sexo, shiny, nivel, objeto, mote y movimientos, y con todos los IV al máximo.

### 🎒 Bolsa
- **Objetos en columnas**: cada casilla pasa a ser una tarjeta con el icono grande, el objeto y la cantidad, en 2 a 4 columnas según el ancho. La página usa todo el ancho (sin el panel del editor), y los bolsillos grandes (260 casillas en Scarlet) se abren al instante.

### ✏️ Editor
- **Concursos en modo legal**: al subir una característica de concurso (Cool, Beauty...), Sheen se ajusta solo al mínimo legal, como pasa en el juego con Pokécubos/Poffins; antes el cambio se deshacía. En Omega Ruby/Alpha Sapphire Sheen queda en 0, y en los juegos sin concursos (ej.: Scarlet/Violet) la sección no aparece.
- **Amistad** desaparece en los juegos de la Gen 1, que no guardan ese valor.

### Correcciones
- **Legalizar con mote vetado**: un mote que el juego no acepta (ej.: "KILLER", vetado por el filtro de palabras) impedía cualquier legalización. Ahora se cambia por el nombre de la especie y la app avisa.
- **Legalizar prefiere el propio juego**: los encuentros y huevos de la versión de la partida van antes que los de otra versión (en una partida de Sapphire, ya no usa un encuentro de LeafGreen).
- **Editor en partidas de la Gen 1 y 2**: abrir muchos Pokémon de Red/Blue/Yellow/Gold/Silver/Crystal daba error y el editor parecía bloqueado, y los tipos aparecían mal (Gengar como Acero). Los juegos de Game Boy numeran los tipos de otra forma; ahora se convierten.
- **Reiniciar después de actualizar**: el botón no hacía nada (la app no conseguía cargar parte de sí misma tras cambiar el exe). Ahora eso se prepara antes del cambio y, si aun así falla, la app avisa para reabrirla manualmente. Vale a partir de la próxima actualización: quien esté en la 0.3.4 o anterior aún tiene que cerrar y abrir la app después de actualizar.

## 0.3.4 — 03/10/2026

### 💾 Partidas
- **Varias partidas en pestañas**: cada partida abierta se convierte en una pestaña arriba (etiqueta del juego, nombre del archivo y ● con cambios no exportados). Abrir otra partida ya no descarta la actual: cada pestaña guarda sus propios cambios, el deshacer/rehacer, la caja y la página. **Ctrl+Tab** / **Ctrl+Shift+Tab** cambian de pestaña, **Ctrl+W** cierra; cerrar una pestaña o la app pregunta si hay cambios. En el Save Manager, todas las partidas abiertas llevan la etiqueta "Abierta", y restaurar la copia de seguridad de una partida abierta recarga su pestaña.

### Correcciones
- **Legalizar Pokémon evolucionados**: cuando el encuentro es de una preevolución, Legalizar ahora sube el nivel hasta el mínimo en que la evolución es posible (ej.: un Gengar Nv. 7 pasa a ser un Gengar Nv. 25 que viene de un Gastly de la Pokémon Tower). Antes fallaba y, con el modo legal activo, el Pokémon no se podía aplicar.
- **Encuentros del propio juego primero**: Legalizar prefiere los encuentros de la misma generación de la partida (en una partida de Yellow, la Pokémon Tower en vez de un encuentro de Gold por tradeback).
- **Reglas de legalidad de cada partida**: al abrir o cambiar de pestaña, la app aplica las reglas que dependen de la partida, como hace el PKHeX (era del cartucho de Game Boy, consola virtual de la Gen 1-3, entrenador activo).
- **Icono de Koraidon/Miraidon**: la etiqueta de Scarlet/Violet y las especies de la Gen 9 aparecían como sprite desconocido cuando la partida activa era de un juego más antiguo (el PKHeX solo tiene esas especies en modo arte). Ahora usan el sprite de arte en esos casos (Save Manager, pestañas, Pokédex y bank).

### Interfaz
- **Sprites más nítidos**: los sprites se amplían 4× en escala entera (nearest neighbor) antes de ir a la pantalla, así que los píxeles quedan nítidos y del mismo tamaño en cualquier tamaño de tarjeta y con la escala de Windows al 125 %/150 % (antes algunas líneas de píxeles se duplicaban y el sprite quedaba torcido).

## 0.3.3 — 02/10/2026

### 🏦 Bank
- **Variantes**: al seleccionar un Pokémon vinculado, aparecen las versiones que volvieron de juegos de otra generación (formato, nivel y fecha). Se puede abrir una variante en el editor de la partida abierta (en lugar de la copia vinculada o en una casilla vacía; al Aplicar, el vinculado pasa a apuntar a esta partida) o eliminarla, sin tocar el original.

### 📖 Pokédex
- **Formas en Sword/Shield y Legends Arceus**: en "Formas y sexos", una forma solo cuenta como vista o capturada si el juego la registró (ej.: Meowth de Galar, Growlithe de Hisui).

### Interfaz
- **Modo legal más visible**: pasó a ser una tarjeta en la barra lateral, verde con escudo ✓ cuando está activo y ámbar con escudo ! cuando está desactivado, con el estado escrito. Haz clic en la tarjeta para activarlo o desactivarlo.

### Correcciones
- **Sprites shiny**: los Pokémon shiny ahora aparecen con los colores shiny y del mismo tamaño que los demás. Antes venían con los colores normales, pequeños, y el icono de brillo de la esquina aparecía como un fragmento suelto (cajas, equipo, bank, Pokédex, encuentros y eventos).

## 0.3.2 — 02/10/2026

### 🏦 Bank
- **Pokémon vinculado**: con "🔗 Vincular al traer", llevar del bank a una partida copia en vez de mover; el original queda en el bank, ligado a la partida. **Actualizar vinculados** trae de vuelta la versión del juego (de otra generación, la guarda como variante sin tocar el original) y ofrece desvincular los que desaparecieron.
- **Otra partida** con selección múltiple (mover grupos entre las dos partidas y el bank) y **deshacer/rehacer propio** (↶/↷ en el panel).

### 📖 Pokédex
- **Formas**: en la Gen 4, 5, 6, BDSP, Scarlet/Violet y Legends Z-A, cada forma solo cuenta como vista/capturada si el juego la registró.
- **Sincronizar todas las partidas**: registra en cada partida de la carpeta lo que se capturó o vio en las otras y lo que está guardado; las cerradas se graban al momento, con copia de seguridad.

### Correcciones
- Marcar "solo vista" en la Pokédex de la Gen 4 y 5 ahora funciona; en los juegos que no lo aceptan (Gen 7), la sincronización no promete las vistas.

## 0.3.1 — 02/10/2026

### ✏️ Editor
- Nueva pestaña **Cintas y recuerdos**: todas las cintas del formato, con búsqueda, "Solo las que tiene", **Todas las legales** y **Quitar todas**; **recuerdos** del entrenador original y del actual (Gen 6+), con detalle, intensidad y sentimiento.
- **Contest stats** (Cool, Beauty, Cute, Smart, Tough, Sheen), **Dynamax Level** y **Gigamax** (Sword/Shield), **alfa** y **noble** (Legends) en la pestaña Extras.
- **Dónde aprenderlo**: al pasar el ratón por un movimiento, el lugar de la MT y el tutor en este juego (AllGenWiki).

### 🎒 Bolsa
- **MT, DT y MO** muestran el movimiento que enseñan, la descripción y dónde conseguir la máquina en este juego.

### 🔄 Actualización automática
- Primera versión entregada por la actualización automática de la 0.3.0.

## 0.3.0 — 02/10/2026

### 🛡 Modo legal
- Interruptor en la barra lateral, **activo de forma predeterminada**: el editor solo ofrece opciones legales (movimientos que el Pokémon aprende, Balls permitidas para el encuentro, especies del juego).
- Los cambios que dejarían al Pokémon ilegal se deshacen al momento, con el motivo en la barra de estado.
- **Aplicar** solo graba Pokémon legales; cambiar la especie o pegar un set de Showdown legaliza solo.
- Un Pokémon **de fuera** (archivo .pk*, bank u otra partida) que llega ilegal a la partida abierta recibe la opción **✨ Legalizar** o **Traer tal cual**.
- Habilidad, forma y Teratipo solo listan opciones legales; las formas de combate salen de la lista.
- **Otro encuentro** (pestaña Encuentro): elige un encuentro real y el Pokémon se genera de nuevo a partir de él, en vez de tocar lugar y nivel a mano.
- **Hacer shiny** queda bloqueado en encuentros con shiny lock y, en los demás, genera de nuevo ya shiny con PID/IV correctos.

### ✏️ Editor
- **Habilidad** elegida en una lista (con la oculta), **forma** y **Teratipo** (Scarlet/Violet).
- **PID y EC editables**, **marcas** (●▲■♥★◆, azul/rosa en la Gen 7+) y **Entrenamiento Extremo** (Gen 7+).
- Descripción en portugués de **movimientos y habilidades** (AllGenWiki), en la lista y en el campo.

### 🎮 Etiqueta del juego
- El Pokémon de la portada en los colores de la versión identifica cada partida en el Save Manager y en la tarjeta de la partida abierta.

### ❔ Ayuda, novedades y actualizaciones
- Nueva página **Ayuda** (F1) con todas las funciones explicadas, este registro de cambios y la pestaña Acerca de.
- **Actualización automática**: al abrir, la app comprueba si salió una versión nueva en GitHub, la descarga, comprueba el archivo (SHA-256) y la instala sola. La versión nueva se aplica al reiniciar (botón 🔄 en la barra lateral, que pregunta antes si hay cambios sin guardar). Se puede desactivar en Ayuda › Acerca de.
- Conoce también **AllGenWiki**, enciclopedia Pokémon de las nueve generaciones (Ayuda › Acerca de).

### 🌿 Encuentros y eventos
- **Panel de detalles**: haz clic en una tarjeta para ver lo que garantiza el encuentro (shiny o shiny lock, IV, habilidad, naturaleza, sexo, Ball, objeto, Teratipo, alfa/Gigamax, entrenador del evento) y los movimientos con los que viene.
- **Filtros** por tipo de encuentro, versión del juego y movimiento; etiqueta **HOME** en los regalos de Pokémon HOME y 🔒 en los encuentros con shiny lock.

### 🗜 Partidas dentro de .zip
- Las **copias de JKSV** (o cualquier .zip con partidas) aparecen en el Save Manager con la etiqueta **ZIP**, una tarjeta por partida; también se puede abrir un .zip directamente.
- Guardar ofrece **grabar de vuelta dentro del zip** (con copia de seguridad del zip antes) o guardar como archivo separado. La Pokédex y la "Otra partida" también leen esas partidas.

### 🎒 Bolsa
- Icono de cada objeto en la Bolsa y en el objeto equipado del editor.
- Al pasar el ratón por un objeto, la descripción en portugués y dónde conseguirlo (datos de AllGenWiki).

## 0.2.0 — 02/10/2026

### 📖 Pokédex centralizada
- Las 1025 especies, juntando la Pokédex de todas las partidas de la carpeta, la partida abierta y el bank.
- Living dex y shiny dex, filtros por situación, generación, tipo y fuente; formas y sexos como entradas propias.
- Dónde está cada Pokémon, con **Ir** para abrirlo en el editor, y **Sincronizar** con la partida abierta.

### 🏦 Bank local
- Almacenamiento propio en bancos → cajas, con los Pokémon como archivos .pk* en el formato original.
- Dos pantallas (bank | partida) con conversión de generación; **otra partida** en lugar del bank; **carpetas externas** como bancos.

### 📦 Cajas y equipo
- **Selección múltiple** (Ctrl+clic, Shift+clic, Ctrl+A) para mover o eliminar en grupo.
- **Ordenar cajas** por Pokédex, nombre, nivel, shiny, tipo, IV o fecha de captura.
- Cajas en pestañas, icono de la Ball en las tarjetas y lista de los cambios no exportados.

### ✏️ Editor
- **Evolucionar por intercambio**, respetando la Everstone y registrando al compañero de intercambio en la Gen 6+.
- **Verificar legalidad** de toda la partida, con atajo al primer problema.
- Eventos de la Gen 1–3 en el banco de Regalo Misterioso.

### 🛟 Copias de seguridad
- Copia de seguridad automática antes de guardar encima, con lista, **Restaurar** y **Restaurar como...**.

## 0.1.0 — 01/10/2026

Primera versión: una interfaz nueva, en portugués, sobre el mismo PKHeX.Core.

### Qué incluye
- **Save Manager** como pantalla inicial, con las partidas de una carpeta agrupadas por consola.
- **Cajas y equipo** con tarjetas grandes, legalidad, arrastrar y soltar y el resumen al pasar el ratón.
- **Editor en pestañas** con búsqueda mientras escribes, movimientos que el Pokémon aprende en verde, IV/EV con gráfico.
- **Legalizar** a partir de un encuentro real del juego (incluso shiny).
- Bases de datos de **encuentros** y de **eventos (Regalo Misterioso)**, con "Usar" para llevarlos al editor.
- **Búsqueda global** (Ctrl+F), deshacer/rehacer, eliminar, atajos de teclado y confirmaciones.
- Tema claro/oscuro y color de acento configurable.
