# What's new in PKHeX Modern

<!--
Traducao do CHANGELOG.md (mesmas versoes, secoes e itens, na mesma ordem; as suites de idioma conferem).
Formato: "## versao — data", "### secao", "- item".
-->

## 0.5.4 — 2026-11-01

### 🌐 Language
- **What's new in your language:** Help's What's new tab is now in English, Spanish and Simplified Chinese, with every version, and the update window shows the notes in the app's language, without the install part.

### 🔄 PKHeX updated
- **PKHeX of 10/31/2026:** the Legends: Z-A Spewpa gift is now recognized, with its distribution dates, and the form rules for Scatterbug, Spewpa and Vivillon were revised. GO Pokémon that came through HOME can have any HOME language, without the Gen 8 language limit. The Master Rank Ribbon is no longer valid on Virtual Console FireRed/LeafGreen Pokémon that could only have it through Scarlet/Violet.

### 🔧 Fixes
- **Message after moving Pokémon:** in the other languages, the message after moving, copying or attaching Pokémon now appears translated. In Chinese, English names containing the word Pokémon, such as “Pokémon League”, are no longer altered, and the Bank has the same name across the interface.

## 0.5.3 — 2026-10-26

### 🔧 Fixes
- **Encounters summary in Chinese:** the line above the cards (“伊布 有 53 个遭遇”) showed a piece of Portuguese in the middle.

## 0.5.2 — 2026-10-26

### 🌐 Language
- **Simplified Chinese interface:** the whole app is now also in Simplified Chinese (简体中文), with the games' official Chinese terms (盒子, 同行, 包包, 训练家, 招式, 名人堂, 太晶属性...). Species, moves, abilities, items and legality messages also show in Chinese, as in the original PKHeX; “Game names in English” keeps English. Chinese systems (Simplified or Traditional) open in it on first launch. The Noto Sans SC font ships with the app, so Chinese looks the same on Windows, Linux, macOS and Android.

### 🔧 Fixes
- **Shiny and egg search in other languages:** search accepts “huevo” (Spanish) and “发光”/“蛋” (Chinese), besides “shiny”, “ovo” and “egg”, as each language's Help says.
- **Texts missing in English:** “Filters”, “Types”, “Variants” and “Sync” showed in Portuguese in the English interface.
- **Nuzlocke graveyard box in Chinese:** the report also recognizes boxes with names like “墓地” or “死亡”.

## 0.5.1 — 2026-10-25

### 🌐 Language
- **Spanish interface:** the whole app is now also in Spanish, with the games' official Spanish terms (Caja, Equipo, Bolsa, Guardería Pokémon, Hall de la Fama, Teratipo...). Pick “Español” in the 🌐 button or in ⚙.
- **Game names in the interface language:** in Spanish, species, moves, abilities, items, natures and legality messages also show in Spanish, with the games' official names, as in the original PKHeX. “Game names in English”, in 🌐 and ⚙, keeps English. Nothing changes in Portuguese and English (PKHeX has no Portuguese names). Copying to Showdown stays in English, and pasting a set accepts any language.
- **System language on first launch:** new installs now start in the computer's or phone's language (Portuguese from any country, Spanish on Spanish systems and English on the others), instead of always in Portuguese. Existing users keep the language they had.
- **Texts missing in English:** stat abbreviations (Atk, SpA, SpD, Spe, HP), “Missing” in the Living Dex, “Ready” in the status bar and Help titles showed in Portuguese in the English interface.
- **🌐 button at the bottom of the sidebar:** next to Help, it shows the current language (PT, EN, ES) and opens the list with each language written in itself, so you can find it without reading the screen. The restart prompt shows in the chosen language.

### 🎨 Interface
- **AllGenWiki on its own domain:** the “🌐 Open AllGenWiki” button (Help › About) now goes to allgenwiki.com.
- **Ball with sprite and shortcut:** the editor's ball list shows each ball's sprite, and clicking the ball on the Pokémon sheet goes straight to the Encounter tab, with the list focused.

### 🔧 Fixes
- **Hall of Fame without a nickname:** when you change a member without typing a nickname, the app writes the species name in the save's language, as the game does. Before, it always wrote the English name, even in saves from other languages.

### 🔄 PKHeX updated
- **PKHeX of 10/25/2026:** Pokéwalker Pokémon (HeartGold/SoulSilver) with the device's PP bug are now recognized as legal. The shiny check for Gen 3 event eggs (Mystery Gift) was revised, and the encounter generator now creates these eggs for the save's trainer.

## 0.5.0 — 2026-10-23

### 🎨 Interface
- **Titles with personality:** large titles (pages, save name, Pokémon in the editor, logo and showcase) now use Fredoka, a rounded font; body text stays in Inter. ⚙ › Font › Inter switches everything back to Inter.
- **Game icons:** the sidebar and the Home shortcuts replaced their generic icons with original, colorful drawings of in-game objects: Poké Ball, PC box, the Bank's PC, the Pokédex, the trainer's cap, the bag, tall grass, the cartridge and the handheld.
- **Pokédex sidebar:** the top became a Pokédex lid, with the band in the accent color (or the save's game color), the blue lens and the three lights; the line above the pages gained the Poké Ball button. Original drawings, no official art.
- **Drag with preview:** the Pokémon's sprite follows the pointer (on the phone, above your finger) with what will happen on release (Move, ⇄ Swap, Copy, Overwrite, Change box) and how many go together in a multiple selection; the source stays dimmed until you let go.
- **Game mode:** at the top of ⚙, one switch turns on the save's game color, the full wallpaper, the type color on cards and animations all at once. Turning it off restores the previous look; also available from the palette (Ctrl+K).
- **Save's game color:** a new accent color (the rainbow dot in ⚙) that follows the open version, with a soft glow of the game's color at the top of the screen. It changes with the tab; very light or dark colors are adjusted so buttons stay readable.
- **Short animations:** the Poké Ball opens when a save loads, the slot gives a little hop when it receives a Pokémon, the editor gives off a ✦ sparkle when the Pokémon becomes legal or shiny, and the ★ of shinies in the boxes twinkles now and then. All are light and do not block clicks; ⚙ › “Animations” turns them off.
- **Save timeline on Home:** each time you open a save, Home shows what changed since last time: hours played, new Pokémon and species, shinies (with the sprite), badges, Pokédex and money. It separates what came from the game from what was saved in the app; the first time, it uses the most recent backup as the starting point.
- **SwSh/SV-style stat hexagon:** on the Stats tab, the shape in the type's color, EVs as a second golden layer and each stat's value next to its name, in red or blue according to the nature.
- **Pokémon sheet in the editor:** the top of the editor gained a glow in the types' colors, the Tera Type (Scarlet/Violet), the level with the nature, the Poké Ball and the origin game's badge with the generation. It follows your edits right away.
- **Illustrated empty screens:** an empty saves folder, no backups, an empty box, no other save in the Bank, Search with no results and an empty Hall of Fame gained an original drawing and a tip on what to do. The empty box notice does not block dragging or clicking slots.
- **Command palette (Ctrl+K):** type “legalize”, “box 5”, “Pikachu” or “export” and Enter runs it: actions, pages, boxes by number or name and the save's Pokémon by species or nickname (opens in the editor). Ignores case and accents; on the phone, through the ⌘ next to the search.
- **Party showcase:** on the Party page, “🖼 Showcase” shows the six Pokémon large, with types, Tera Type, item, ability, nature and moves colored by type, in Grid (3×2) or Columns. “Save image” writes a high-resolution PNG to share.
- **Type color on cards:** each Pokémon in the boxes, party, bank and other save has its card background tinted with its types' colors (the first at the top, the second at the bottom). Eggs and empty slots stay neutral. You can turn it off in ⚙ › “Type color on cards”.
- **“Full” wallpaper:** a new option in ⚙ › Box wallpaper that shows the game's wallpaper sharp, as in the game itself, with translucent cards on top. The default is still Normal.

### 📱 Android
- **Gestures:** swipe sideways in the boxes to change boxes (on the Boxes page and in the Bank's three panels) and pull the Saves page list down to reload the folder. Holding your finger still marks and drags.

### 🔄 PKHeX updated
- **PKHeX of 10/21/2026:** reading Showdown sets became stricter. EV/IV lines with an invalid value, the same stat repeated (e.g. “30 Atk / 29 Atk”) or two stats marked with the same + or − are now reported as errors instead of the app guessing the value. In FireRed/LeafGreen, the Meteorite now sits among the bag's key items. Internally, PKHeX's encounter generator now ignores versions it does not handle yet (the app's own protection for this stays).

## 0.4.30 — 2026-10-19

### 🔄 PKHeX updated
- **PKHeX of 10/18/2026:** transfers from Virtual Console FireRed/LeafGreen to HOME are now recognized, with more egg move checks; in Gen 3 and 4, EVs are checked against the experience gained; the WCS 2026 Greninja is no longer treated as an expired event; flags for Legends: Z-A's item pouches.

### 🎨 Interface
- **Full list for species and item:** in the editor (and in the Shiny hunt's species), tapping or clicking the box shows the complete list, with the current name selected; typing replaces the name and filters by any part of it (“chu” finds Pichu, Pikachu and Raichu). Before, the list only appeared after the first letter.

### 🐛 Fixes
- **Forms in the editor:** changing the form (e.g. Meowth to Alola or Galar) left the form box blank, because the change also changed the ball and ability lists. Now only the box whose list changed is updated.
- **Legalize after the PKHeX update:** the new versions PKHeX had just listed (Pokémon Winds and Waves) have no encounters yet and broke the search: changing the form in legal mode gave “error (context None)”. They stay out until PKHeX handles them.
- **App frozen when bringing a Pokémon from a newer generation to Gen 3:** after the PKHeX update, taking a Pokémon from Gen 7 (or newer) to a FireRed, Emerald or other Gen 3 save (through the Bank, the other save or by importing a file) entered an endless loop. Now the app refuses right away, as before (“no transfer route”).
- **Evolving in Let's Go and Legends: Arceus:** the new PKHeX checks the stored height and weight, and evolving (or changing the species or form) made the Pokémon illegal (“Calculated Height/Weight does not match”). Now both are recalculated together.
- **Phone:** the Boxes “Select / Move” bar sat underneath the save tabs; now it sits right below them.

## 0.4.29 — 2026-10-17

### ✨ Shiny hunt
- **New Shiny hunt page** (at the end of the sidebar): a counter with +1, Space or +, undo and adding several; odds per encounter for each method in the open game (Masuda, Poké Radar, chain fishing, Friend Safari, SOS, Catch Combo, Dynamax Adventure, outbreaks and SV sandwiches...), the chance you should have found it by now, encounters for 50/90/99% with the curve, time hunting and pace per hour. Each method shows its source; those with doubtful values appear as “to be confirmed”.
- **Reads the save:** the Shiny Charm is checked if it is in the bag, and when you open a save with a new shiny of the hunt's species, the app offers to finish the hunt, with the capture location and date.
- **Phases and trophies:** “Log phase” stores a shiny that was not the target without resetting the hunt (a new shiny of another species in the save is also offered as a phase); a finished hunt becomes a trophy with game, method, encounters, phases, date and “Go” to the Pokémon in the save; “Export CSV” saves everything to a spreadsheet.
- **Where to find it:** the target's locations and levels in the game, a shiny lock warning and the wild Pokémon's moves that can ruin the hunt (Explosion, Self-Destruct, Roar, Whirlwind, Teleport, Perish Song, recoil...), with what to do.

### 🐛 Fixes
- **macOS “damaged”:** the macOS app said it was “damaged and can't be opened”, because the bundle shipped unsigned. Now the package is signed (ad-hoc) and checked during the build; macOS only asks you to allow the app in Settings › Privacy & Security. Anyone who downloaded an earlier version can run `xattr -dr com.apple.quarantine "/Applications/PKHeX Modern.app"`.
- **Sword/Shield raids:** editing a raid (for example, deactivating an event raid) froze and closed the app. The screen and the editor entered an endless loop; now each field ignores the value already there and the type list is fixed. The same care applies to Scarlet/Violet raid seeds.
- **Editor freezing (memories):** with a Gen 6/7 Pokémon that has memories, the editor could freeze the interface after a change (seen when changing the encounter of an Alpha Sapphire Pokémon). The memory feeling and detail lists were recreated on every update and entered a loop with legal mode; now they are fixed.

## 0.4.28 — 2026-10-16

### 📱 Android
- **Emulator saves:** the “🎮 Emulator saves” button on the Saves page finds saves from RetroArch, DraStic, melonDS, Dolphin, Azahar/Citra/Lime3DS, Eden/Yuzu, mGBA, My Boy! and Pizza Boy on its own, plus those next to the ROMs. It asks once for “all files access” and writes to the original file, with a backup.

### ✨ Legality
- **Legal Showdown set:** in legal mode, pasting an illegal set generates a legal Pokémon for this game keeping IVs, ability, nature, gender, shiny, EVs, moves and Tera Type whenever possible (Hyper Training, mints and capsules when the game allows them), and tells you what could not be kept.
- **TM/TR moves in Legalize:** in Sword/Shield and Scarlet/Violet, Legalize marks the TM/TR record, so the Pokémon's TM moves are no longer swapped for others.

### 📦 Boxes
- **Lock box:** under “⋯ Box”, the box is protected against dragging, deleting, applying, pasting, batch editing, the egg factory and the Living Dex until you unlock it; “Sort all boxes” skips locked ones. A padlock on the tab, and the lock is saved per save.

### ✏️ Editor
- **Nature keeping the shiny (Gen 3/4):** changing a shiny's nature no longer removes the shiny; in legal mode, the Pokémon is regenerated with the new nature, legal and shiny (before, the change was undone).

### 🎒 Bag
- **Fill pouch:** ready-made sets with all TMs, all Poké Balls, all medicine, all berries or everything in the open pouch, with 99 of each and in numeric order.

## 0.4.27 — 2026-10-16

### 🎨 Interface
- **Vector icons:** the sidebar and the Home shortcuts use line icons drawn by the app, in the text color (and in the accent color on the current page), instead of emojis, which changed size and style on each system, especially on Android.
- **Sidebar in groups:** Pokémon (Boxes, Party, Bank, Pokédex), Trainer (Trainer, Bag), Get (Encounters, Events) and Tools (Saves, Game, Search, Batch editing). The order and the Ctrl+1 to Ctrl+9 shortcuts did not change.
- **Save status on Home:** below the game card, unsaved changes (click to view and save), last backup (click to open the backups), free slots in the boxes and the file.
- **Collection summary in the Pokédex:** with no species chosen, the right panel shows bars for Living dex, Shiny dex, caught and seen, how many are missing in each generation (click to list only the missing ones) and the “Next missing” button; “‹ Summary” returns to it.

### 🥚 Egg factory
- **Eggs of the missing ones:** in the Pokédex summary, one button generates a legal egg for each species missing from the open save (of the basic form: Charmander for Charizard, Pichu for Pikachu); another generates one egg per family. They come from the game's own breeding eggs, with your trainer, and go to the free slots in the boxes, with the list shown before writing and a single Ctrl+Z. Works from Gen 2 to Gen 9 (in a Black save, 595 eggs in under a second).

### 🔒 Challenges
- **Hardcore mode:** in ⚙, “Hardcore mode (move Pokémon only)” turns off all data editing, for challenges like the Nuzlocke and for those who play with RetroAchievements. The editor only shows the data; creating, copying, importing, pasting Showdown, legalizing, generating encounters and events and completing the Pokédex are off, and Trainer, Bag, Game and Batch editing leave the sidebar. Moving, swapping, releasing, sorting and using the Bank still work. A red card in place of legal mode shows it is on.
- **Nuzlocke report:** a new tab in the Pokédex with the first Pokémon caught on each route, extra catches, dupes (a family already caught before), who is in the party, alive or dead (by the graveyard box, chosen by name) and a button to copy the report as text. Trades, other games and eggs stay out; from Gen 4 on, the order follows the capture date.

### 🐛 Fixes
- **Pokédex with wide fonts:** the species name in the panel shrinks instead of breaking in the middle ("Ivysa/ur"), and "No Pokémon of this species stored" is no longer cut off.

## 0.4.26 — 2026-10-15

### 📱 Phone and small screens
- **Name in the editor:** with wide fonts (like Pokémon GB), the Pokémon's name shrinks to fit and the ♂/♀ gender and the shiny star are always visible; the location ("party, position 1") shortens with "…".
- **Party cards:** long names (Blastoise, Charizard) shrink instead of being cut off.
- **Home:** cards in the same row have the same height.
- **Sidebar:** all pages have the same height (Android's emojis had different heights and misaligned the list), and a line separates "Home" from the list when it scrolls.
- **Boxes and Pokédex:** the box tabs gained some room from the save tabs, and the Pokédex details panel is narrower on smaller screens, so the filters are not cut off.

## 0.4.25 — 2026-10-15

### 🎨 Interface
- **More compact boxes:** the box header fits on one line (‹ name · count · ⋯ Box ›) and Rename, wallpaper, Showdown and CSV moved to the **⋯ Box** menu. On laptops (1280×720), almost two rows of slots show instead of one and a half; in 1100 px windows, four columns instead of three.
- **Adaptive editor panel:** 360 px in windows under 1400 px (410 px in larger ones), and the party strip shows only sprites when space is short, instead of cut-off names.
- **Bag with pouch names in Portuguese:** Items, Key Items, TMs and HMs, Medicine, Berries... (before, internal names showed up, like "KeyItems" and "TMHMs").
- **Batch editing:** Preview and Apply stay fixed at the bottom of the column, always visible.
- **Save ▾:** "Save as..." left the sidebar and became the arrow next to Save, in the footer; "Open save..." only stands out when no save is open.
- **One main button per screen:** the "Use" on Encounters and Events cards became neutral (the details panel has "Use this encounter"), and the footer no longer repeats actions already at the top of the page (Refresh, Sync, Use encounter, Sync attached, Apply bag).
- **Delete looks like a destructive action:** a red outline instead of a fill, so it is not confused with Save.
- **Same vocabulary across the app:** "Apply" takes the change to the open save and "Save" writes the file ("unsaved changes", "Remember to save", "Apply bag"); "export" is now only for .pk* files.
- **Cleaner bag:** shows the items you have and a few free slots to add, instead of dozens of "(none)"; "Show all slots" shows the whole pouch.
- **Details:** sidebar shortcuts only appear on hover or on the current page; Encounters filters say "All encounters" and "All versions"; the Game and Batch editing shortcuts on Home gained a description; Help has higher-contrast text and shorter lines.

### 🐛 Fixes
- **Help in the sidebar:** with Help open, the previous page is no longer marked; the Help button is what stands out.

## 0.4.24 — 2026-10-14

### ♟ Trainer
- **Badges, gender, coins, BP and dates:** the Trainer page now edits each region's badges (Gen 1 to 6, Sword/Shield and BD/SP, with “All” and “None”), the trainer's gender, Game Corner coins (Gen 1 to 4), Battle Points (Emerald, Gen 4 to 7 and Sword/Shield) and the adventure start and Hall of Fame dates (Gen 4 and 5).

### 📖 Pokédex
- **Complete the save's Pokédex:** a new button on the Pokédex page to mark all species as seen and caught in the open save (with forms, genders and languages where the game stores them, and seen shinies if you want), mark as seen only, remove the caught ones or clear everything. From Gen 1 to Z-A (except Legends: Arceus), with Ctrl+Z.

### ⚡ Shortcuts
- **Unlock everything:** new shortcuts on the Game page for what is tedious to farm: all O-Powers and all Super Training regimens (X/Y and Omega Ruby/Alpha Sapphire), all Zygarde Cells (Sun/Moon and Ultra Sun/Ultra Moon), Grand Underground items maxed out (BD/SP) and a full Underground sphere bag (Diamond/Pearl/Platinum).

## 0.4.23 — 2026-10-13

### 🔄 PKHeX updated
- **PKHeX fixes (kwsch, up to 10/13):** HeartGold/SoulSilver saves keep the right version when opened; Dratini with ExtremeSpeed from an egg in HeartGold/SoulSilver no longer comes out illegal; X/Y's PSS reads all contacts; updated translations.
- **Missing wallpapers:** Ruby/Sapphire and FireRed/LeafGreen with all 16, Emerald with 17 (Walda's), the right names for each, Colosseum/XD and Battle Revolution boxes with each box's wallpaper and the "My Wallpaper" drawn in Pokémon Box.

## 0.4.22 — 2026-10-10

### 🎨 Appearance
- **ORAS theme (Pokédex):** teal inspired by the Pokédex and PokéNav Plus of Omega Ruby/Alpha Sapphire, with the Water accent color.
- **Kanto theme (classic Pokédex):** the red and cream of the Red/Blue Pokédex.

### ⚙ Preferences
- **Sprite size:** Small, Normal, Large or Huge in the boxes, bank, other save and Pokédex, through ⚙.
- **Optional legalize on click:** the “Offer to legalize when clicking an illegal one” option in ⚙ turns off the question; the Pokémon just opens in the editor and the Legalize button stays available.

### 🌐 Project
- **Project site:** [carlosmozart.github.io/PKHeX-Modern](https://carlosmozart.github.io/PKHeX-Modern/), with features, screenshots and downloads for each system.

## 0.4.21 — 2026-10-08

### 🎮 Game
- **Battle Frontier:** a new tab in Emerald, Platinum and HeartGold/SoulSilver (and Diamond/Pearl's Battle Tower), with BP, Frontier Pass and silver/gold symbols (Emerald), each facility's prints (Gen 4) and the streaks for each facility, mode and level, including Factory swaps, Castle CP and the streak in progress.
- **Pokétch and Pokéwalker:** unlock Pokétch apps and choose the one on screen (Diamond/Pearl/Platinum); Pokéwalker watts, steps and routes (HeartGold/SoulSilver).
- **Pokéathlon medals:** the five medals of each of the 493 species, with search, an incomplete filter and “All medals”.
- **Secret base:** in Ruby/Sapphire/Emerald, edits each base's owner and shows its team; in Omega Ruby/Alpha Sapphire, edits your base's texts, level and flags, gives all decorations and deletes other players' bases.

## 0.4.20 — 2026-10-07

### 📖 Pokédex
- **Open save only:** a button in the Pokédex shows only what the open save has seen, caught and stored, without mixing with other saves or the bank.

### 🎒 Bag
- **All TMs:** a button puts all the game's TMs in the TM pouch, with 99 of each and in numeric order, without removing what was there.

### 🐛 Fixes
- **Gen 3 roamers:** Entei, Raikou and Suicune from FireRed/LeafGreen and Latias/Latios from Ruby/Sapphire generated in Encounters always came with zeroed IVs. Now IVs are rolled as in the game, which because of a bug only stores HP 0-31 and Attack 0-7.
- **Pokédex in small windows:** the buttons, search, filters and generations adjust to the width, without cutting off the title or the search field.

## 0.4.19 — 2026-10-06

### 🔎 Search
- **Duplicate audit:** identical copies by stored data, backup grouping by file pair (ZIP or save folders with many identical Pokémon) and attached/variant entries recorded in the Bank, with read-only comparison and navigation. Cancelable reading, including unsaved changes.

### 📖 Pokédex
- **Apply Living Dex:** a preview of the arrangement in the open save's boxes, with first box, room for missing ones, external copies, keeping occupants and protected slots, confirmation and a single undo. Changed sources require a new preview.

## 0.4.18 — 2026-10-06

### 📱 Android
- **Full screen:** the app hides the status and navigation bars, gaining screen space. Swiping from the edge shows the bars for a moment; the camera cutout stays clear.

### 🔗 Project
- **New address:** the repository is now called [PKHeX-Modern](https://github.com/carlosmozart/PKHeX-Modern). Old links keep working (GitHub redirects) and automatic updates carry on as normal.

## 0.4.17 — 2026-10-05

### ✏️ Editor
- **Assisted evolution:** Evolve brings together the open game's methods and branches, with sprite, requirements and a cancelable preview. It removes the Everstone, meets the Pokémon's requirements and leaves the result pending until Apply, with undo and legal mode protection.

### 📦 Boxes and party
- **CSV report:** exports the open save's party and boxes or all boxes of the chosen bank, with data, IVs, EVs, moves and legality, without changing the originals. PT/EN headers, UTF-8 with BOM and a file picker on Android too.
- **Showdown team:** the “📋 Showdown” button in Party and in Boxes pastes several sets at once into the party's slots or the box's empty slots, with a review list first (legal, illegal or out, with the reason) and a single undo. It also copies the whole party or box in Showdown format.

### 📖 Collection
- **Living Dex:** the plan uses the target game's number of slots per box (20 in Gen 1/2, instead of always 30), and the other Pokémon of the same species appear as “other specimens”, without suggesting they are copies.

### 🔧 Fixes
- **Search:** reads a copy of the open saves (it does not fight the editor over the save while searching) and no longer creates empty banks and boxes in the Bank.

## 0.4.16 — 2026-10-05

### ⚙️ Performance and diagnostics
- **Editor:** state, report, warnings and legality groups share one analysis per update.
- **Folder diagnostics:** counts are passed as data, without depending on the summary's language and without including file names.

### 💾 Backups
- **Reason and integrity:** new copies record operation, game, size and SHA-256. The list warns when the file changed and Restore lets you check before proceeding. Old backups remain available; records follow the cleanup of copies, in Android's private storage too.

### 🏦 Bank
- **Read-only lookup:** tap a Pokémon to see its data, shiny sprite, stats, moves, origin and legality. Export, copy Showdown, look at variants or open a copy in the editor of a compatible save, keeping the original.

### 🎨 Preferences
- **Theme preview:** the four themes appear in cards with real thumbnails of the boxes and the editor header, rendered by Avalonia and cached for light and dark.
- **Box wallpaper:** choose Normal, Soft or Off to make slots easier to read. The preference is kept and does not change the game's wallpaper.

### 📱 Touch on Android
- **Touch selection:** boxes and Bank have a “☐ Select” mode, also opened by a long press, with a count and actions for the group. Leaving the mode clears the marks.
- **Tips without a mouse:** touch and hold a button to see its description without running it; theme cards and the drag mode have ⓘ. Touch dragging only starts after holding your finger still; lists use inertia and a visible scroll indicator.

### 📖 Collection
- **Living Dex planner:** a new section in the Pokédex, with target game, forms, shiny and sources. It shows the count, best candidates, duplicates, missing ones and a national plan of 30 slots per box. Reading shows progress; no Pokémon is moved and no save is written.

### 📋 Diagnostics
- **Share diagnostics:** Help › About shows a preview with version, system, language, theme and recent log. Android opens the system share sheet; desktop copies the text. Free-form messages and private data are left out.

### ↔ Pokémon
- **Legality by topic:** reasons appear in groups by severity, with a count and the first invalid one open. Moves, Poké Ball, Ribbons and Encounter have fix actions tied to the problem (“Legal Poké Ball” picks one that matches the Pokémon's color, as in Batch editing).
- **Change preview:** Legalize shows the fields before and after, highlighting important changes and the encounter the Pokémon was generated from. The confirmation uses the candidate shown, without generating another. Transfers between generations only ask for confirmation when they change something important (shiny, PID, IVs, nature, trainer, nickname, Poké Ball or moves); batches show a summary per field and the complete list.

### ⚙ Batch editing
- **Batch legalize:** a new “Legalize” quick action that regenerates, from a real encounter in the game, the Pokémon that are illegal (after the other actions), keeping nature, level, item, nickname and moves when possible. The preview marks the legalized ones (✨) and counts those that failed; “Apply” writes exactly what was previewed.
- “Legal ball” is now called **“Legal Poké Ball”**.

## 0.4.15 — 2026-10-05

### ⬆ Update
- **Update animation:** while the new version downloads, a row of gray Poké Balls gains color (Poké, Great, Ultra... up to the Master Ball at 100%) and the open save's party walks in line over the bar — now and then one of them appears shiny. “Keep using” hides the panel and the download goes on. To see it without a new version: Help › About › “✨ View animation”.
- **Updating when a few versions behind:** if the newest version does not have your system's package yet (the Android APK goes up a few minutes later), the app offers the most recent one that does, instead of giving an error.

### 💾 Backups
- **Backups separated by save:** saves from different folders with the same name (like the Switch's “main”) now each have their own limit of 20 backups; before, one could delete the other's backups.

### 📱 Android
- Connecting a Bluetooth controller or keyboard no longer restarts the app (nor loses the edit in progress).

## 0.4.14 — 2026-10-04

### ✨ Legality
- **Legalize on click:** when you click an illegal Pokémon in the boxes or party, the app asks if you want to legalize it. “Legalize” regenerates the Pokémon from a real encounter and writes it to the slot (Ctrl+Z undoes it); “Just open” opens it in the editor as before and does not ask again about that Pokémon until you close the app.

### 🎮 Game page
- **Scarlet/Violet fashion:** a catalog by category and gender, search by category in English or ID, individual unlock and lock. Unlocking all or one category asks for confirmation and can be undone. The Core does not provide the pieces' commercial names.
- **Z-A donuts:** list, editing of berries, stars and powers, a generator by effect, duplication and deletion. Filling the bag asks for confirmation and has one undo step.
- **Roamers:** a Gen 3 and X/Y editor with reappear, stored data and undo. Legal mode validates encounters and the PID/IV correlation; Ruby/Sapphire/FireRed/LeafGreen show the IVs truncated by the game's bug.
- **Gen 3 clock:** a tab for Ruby/Sapphire/Emerald with initial and elapsed clocks, a fix for the stopped clock using PKHeX's method and advancing one day. Each edit can be undone.
- **Undo and redo:** flags, values, records, shortcuts, Hall of Fame, cards and raids now have their own history in the Game page's ↶/↷ buttons. The tooltip names the action; bulk operations count as one step. Up to 30 steps and 64 MB of differences, without reverting edits from other pages.

## 0.4.13 — 2026-10-04

### ✏️ Editor
- **Pokérus** (Extras tab): no Pokérus, infected or cured, “Give Pokérus”, strain and days. Legal mode only allows it where Pokérus is possible.
- **Hidden Power** (Stats tab, Gen 2–7): type and power; choosing the type adjusts the IVs, and legal mode refuses when the Pokémon would become illegal. The move shows the calculated type.
- **Gen 7 QR**: a button in the editor generates the QR for Sun/Moon's QR Scanner, with “Save image”. Gen 6/7 cards also generate QR in PKHeX's format.

### 📦 Boxes and party
- **Boxes in folders** (⋯ button on the Boxes page): export this box or all of them as .pk* files (with a subfolder per box, if you want) and import a folder into the boxes, with generation conversion, an offer to legalize the batch, a list of what was rejected and a single Ctrl+Z.
- **Day Care** (below the party): edit the Day Care Pokémon, put in and take out (with undo), waiting egg, experience and seed according to the game. Includes both Omega Ruby/Alpha Sapphire and Sword/Shield Day Cares.

### 🎮 Game page
- **Save's event cards**: the Mystery Gifts waiting to be received in the game. Import, export, delete, mark as not received and “Add to save” from the Events page, with each generation's rules (Gen 4 to 7 and Let's Go).
- **Raids**: Sword/Shield dens (Galar, Isle of Armor, Crown Tundra) and Scarlet/Violet Tera Raids (Paldea, Kitakami, Blueberry), with active, type, stars, seed and 7-star records; “Activate all” and “Deactivate all” ask for confirmation.

### 📱 Android
- The saves folder works with all the saves in the chosen folder. If you chose the folder before 0.4.10, tap “Choose folder...” and choose it again (those versions did not keep the choice).

## 0.4.12 — 2026-10-03

### 📱 Android
- **Saves folder**: after you choose the folder, tap Refresh or open the app, the status bar shows a summary of the reading (how many files the folder has, how many were read as saves, how many were skipped and the first error, if any). A file or subfolder with an error is now skipped without stopping the others from being read, and the error goes to crash.log. This helps investigate why some saves in the folder still do not appear.

## 0.4.11 — 2026-10-03

### 🔧 Preparation
- Automatic updates now also accept the repository's future name (`carlosmozart/PKHeX-Modern`). So when the project is renamed on GitHub, anyone on this version or newer keeps receiving updates on their own.

### ⚠️ Known issue
- On Android, the saves folder does not yet list all saves in the chosen folder. It is being investigated; meanwhile, open any missing save with the Open button.

## 0.4.10 — 2026-10-03

### 🎮 Per-game editors
- **Editable Hall of Fame** (★ tab on the Game page): click a Pokémon to change species, nickname and level (in Sun/Moon, only the species); in X/Y and Omega Ruby/Alpha Sapphire also the victory date and shiny. Each victory gained “Use current party” (swaps in the save's party Pokémon) and “Delete”, and “Register current party” adds the save's party as a new victory, as when beating the League. The tab appears even with no victories yet.

### 📱 Android
- **Saves folder on Android**: on the Saves page, “Choose folder...” now opens the Android picker and the app reads the whole folder, with subfolders, listing all saves as on desktop (before, only saves opened one by one appeared). “Refresh” and opening the app bring in new or changed saves and remove those that disappeared. Save writes back to the folder's file, with a backup; a save that is open or has unsaved changes is not replaced by the folder's version. The “Open folder” button no longer appears on Android.

## 0.4.9 — 2026-10-03

### 📱 Android
- **External bank folder on Android**: in Bank › Add folder, choose a phone folder with .pk* files (from another app or synced). It appears as 📁 in the Bank, as on desktop. The app syncs: on opening, it brings in what changed in the folder, and each Pokémon written or deleted in the Bank goes to the folder right away. If a file changed on both sides, the folder's version stays and the app's goes to the backups.
- **Automatic update on Android**: on opening, the app tells you when a new version is out and shows what's new. With “Update now”, it downloads the release's APK, checks the file (SHA-256) and opens the Android installer, which installs over the old one without losing anything. The first time, Android asks you to allow installing apps from PKHeX Modern. You can turn it off in Help › About.
- Help's links (releases, code, AllGenWiki) now open in the phone's browser.
- The sidebar's Home item no longer shows the Ctrl+0 shortcut either.

## 0.4.8 — 2026-10-03

### 📱 Android
- The ♂/♀ symbols and button icons (Create, Import, Delete, Export...) appeared as "≣" on Android. Now the app brings these symbols' fonts (Noto Sans Symbols 2 and Noto Emoji), without depending on the phone's fonts.
- These work again on Android: clicking a search result, opening a save through the Save Manager, the Bank panel, "Sync attached", "Back" in Help and the accent color.
- The sidebar's page list no longer leaves a line drawn over the others when scrolling.
- Keyboard shortcuts no longer appear on Android (Ctrl+N column, ⌨ button, "(Ctrl+F)" in the search and Help's Shortcuts section).

### ✨ Tweaks
- The sidebar's Help button now has text only, without being cut off on narrow screens.
- The README shows the supported systems (Windows, Linux, macOS and Android) and a screenshot of the app on the phone.

## 0.4.7 — 2026-10-03

### 📱 Android
- **PKHeX Modern now has an Android version** (APK in the release). The interface is the same as desktop, with the screen sideways: sidebar, boxes and editor side by side. On phones everything is scaled down to fit; on tablets it stays at normal size.
- Saves open through the Android file picker, without asking for access to all storage. The app edits its own copy and, on **Save**, writes back to the original file and checks the write; the previous original goes to the backups. If the file changed in another app, nothing is overwritten.
- Importing and exporting Pokémon, Save as and saves inside .zip work through the picker too. The Android Back button closes the open question or goes back like Esc.
- Not in this first version: automatic updates (install the new APK over the old one), folder explorer and external bank folder.

## 0.4.6 — 2026-10-03

### 🖥️ Linux and macOS
- **PKHeX Modern now runs on Linux and macOS**, besides Windows (the original PKHeX is Windows only). The release brings four packages: Windows x64, Linux x64 (also works for the Steam Deck), macOS Apple Silicon and macOS Intel. None needs .NET installed.
- On macOS the app comes as `PKHeX Modern.app`. It has no Apple signature, so the first time you open it macOS asks you to allow it: right-click the app › Open, or Settings › Privacy & Security › Open Anyway. The README explains this, along with the Linux dependencies.
- **Automatic updates** download the right package for each system. On Windows nothing changes; on Linux the execute permission is kept; on macOS the whole app is replaced.
- Sprites are now assembled without depending on Windows: the same PNGs and the same assembly as PKHeX, redone with SkiaSharp and checked pixel by pixel against the original (nothing changes on screen).
- Help says where preferences, backups and the bank live on each system (`~/.config/PKHeX.Modern` on Linux and macOS).

### 🐞 Fixes
- Automatic updates keep working when the program file was renamed (e.g. `PKHeX.Modern (1).exe`).

## 0.4.5 — 2026-10-03

### 🎮 Per-game editors
- **Gen 1, Let's Go and Z-A events** on the Game page. Gen 1: all 2560 flags and 256 values, with names on the flags of the map's fixed Pokémon. Let's Go: all 4096 flags and 1000 values, with names from PKHeX's list (Snorlax, legendary birds...) and the type in the code (Z zone, S system, V map object, E event). Z-A: the event, system, map item, mission, Mable task and title tables; the game stores only each event's hash, so map items (Colorful Screws and TMs) are the only ones with names. Search now also finds by code.
- **New shortcuts**: the map's fixed Pokémon again in Gen 1 (Mewtwo, Articuno, Zapdos, Moltres, Power Plant Voltorbs, Eevee, fossils, Yellow's gifts...), all Master Trainer titles in Let's Go, Colorful Screws and map TMs in Z-A, a full case of Pokéblocks (Ruby/Sapphire/Emerald and Omega Ruby/Alpha Sapphire) and Poffins (Diamond/Pearl/Platinum and BD/SP), and Pokéathlon points, Data Cards and medals (HeartGold/SoulSilver).
- **Hall of Fame** (new ★ tab, read-only): the teams that beat the League with sprite, nickname and level in Gen 1, Gen 3, X/Y and Omega Ruby/Alpha Sapphire (with the date), and the species of the first and most recent team in Sun/Moon.
- Shortcuts that give items (BD/SP Member Card, Colorful Screws, TMs) now update the Bag and the Pokédex right away, without losing what was being edited in the Bag.

## 0.4.4 — 2026-10-03

### 🎮 Per-game editors
- **Scarlet/Violet events** on the Game page: about 650 flags and 280 event values PKHeX knows by name (each event is a save block), grouped in categories: **Fly** (flight points), **Map**, **TM recipes**, **Unlocked features**, **Purchases**, **Stakes (Treasures of Ruin)**, **Auction**, **Outbreaks**, **Battles and challenges**, **Academy**, **Story and events** and others. Search, filters and “Activate shown” work as in the other games.
- **BD/SP events**: all 4000 flags, the 1000 system flags (shown as S#0000) and the 500 event values, with the names PKHeX knows.
- **BD/SP shortcuts** (new ⚡ Shortcuts tab): rematch with Dialga/Palkia, **Darkrai**, **Shaymin** and **Arceus** events (with the item, the National Pokédex and the right flags), **Spiritomb**, **Mesprit** and **Cresselia** roaming again, all map areas and all outfits. Each shortcut explains what it does, asks first and stays disabled when there is nothing to do.

### 🗂 Saves
- **Your saves in the sidebar**: with no save open, the sidebar lists the saves in the Save Manager folder (game badge, name and file) and one click opens the save. The disabled “Save as...” disappears until a save is open.

## 0.4.3 — 2026-10-03

### 🎨 Themes
- **Pixel font in the Pixel theme**: Consolas (a wide terminal font) gave way to **Pixelify Sans**, a pixel font with accents, embedded in the app, with true bold. It appears in menus, labels and titles; small captions (up to 13 px) stay in the default font, because pixel fonts blur at those sizes.
- **Font choice**: in the sidebar's ⚙, the **Font** section changes the interface font right away, in any theme: **From theme** (the default: Pixelify Sans in the Pixel theme, Inter in the others), **Inter**, **Pixelify Sans** or **Pokémon GB/GBC**. Each option is written in its own font, and the choice is saved.
- **Game Boy Pokémon font**: the **Pokémon GB/GBC** option uses Pokemon Classic, a fan recreation of the Red/Blue/Yellow/Gold/Silver/Crystal font (by TheLouster115, CC BY-SA 3.0), with accents. Since the letters are wide, it applies to titles (box name, Pokémon name, page titles, logo); normal text stays in Inter so it is not cut off.

### 📦 Boxes
- **Centered box tabs**: when all fit, the list sits in the center, aligned with the box name. When they do not fit (e.g. 24 or 32 boxes), it scrolls on its own to keep the open box in the middle, including when changing boxes with the arrows or keyboard.

## 0.4.2 — 2026-10-03

### 🧭 Sidebar
- **More compact**: all 13 pages fit without scrolling in the default window (and from 720 px of height). List items got shorter, the game card smaller and legal mode became a single line.
- **Preferences in ⚙**: theme, accent color, language and “Open last save on startup” live in the ⚙ button on the bottom row, next to ❔ Help, ⌨ (keyboard shortcuts) and ◐ (light/dark).
- “↺ Last” only appears with no save open (with a save open, the tabs and Home already show the recent ones).

## 0.4.1 — 2026-10-03

### 🎮 Per-game editors
- **Game page** (new): **event flags** from Gen 2 to 7 (hidden items already picked up, trainers beaten, flight points, story, encounters, gifts...) with names from PKHeX's lists, search by name or number, a category filter and “Activate/Deactivate shown” to change everything the filter shows at once (with confirmation).
- **Event values**: counters and states of each event, with known values in a list and the free number alongside.
- **Trainer records** in Gen 3, 5, 6, 7, Sword/Shield and BD/SP: steps, battles, catches, eggs hatched and the game's other counters.

## 0.4.0 — 2026-10-03

### ⚙ Batch editing
- **Batch Editor** (new page): changes several Pokémon of the open save at once. Choose the group (current box, all boxes, party, boxes and party, Pokémon selected with Ctrl+click or the **Search results** of this save) and what to do: quick actions (level 100, max IVs, zero EVs, max friendship, heal HP and PP, make shiny, suggested moves, legal ball) and/or **PKHeX's Batch Editor script** (`=Species=Pikachu` filters, `.CurrentLevel=100` changes), with a property list to insert.
- **Preview** shows each Pokémon with what will change (level, IVs, EVs, shiny, moves, ball...) and whether it stays legal, without writing anything. **Apply** asks first, writes everything in one step and **Ctrl+Z undoes** the whole batch.
- **Legal mode**: anything that would become illegal (⚠) is skipped; with legal mode off, it writes anyway and warns you.

### 🔎 Search
- **Database / search** (new page, **Ctrl+Shift+F**): all Pokémon from open saves (with unsaved changes), saves in the folder (including inside .zip) and the bank in a single list. Search by species, nickname, move, item, ability, nature, ball, trainer or game (all words must match; `#25` searches by number), filters for origin, shiny, perfect IVs, nature, ball, level, origin generation and eggs, and five sort orders. Clicking a result goes to the slot, opening the save in a tab if needed, or opens the bank box.

### 💾 Old saves
- **Gen 1–3 version and language from the name**: the app works out which game of the pair it is (Red/Blue, Gold/Silver, Ruby/Sapphire, FireRed/LeafGreen) and the language from the file name and now also from the **zip entry, the zip name and the folder** (e.g. `Pokemon Sapphire/jogo.sav`, `Rouge/save.sav`). Before, saves inside a zip were not identified (a zipped FireRed/LeafGreen always appeared as FireRed).
- **Double badge**: when there is no way to know, the save appears as the pair (“Ruby / Sapphire”) with both mascots on the badge.

## 0.3.9 — 2026-10-03

### ⌂ Home
- **Save home screen**: when you open a save, the app shows Home (⌂ in the sidebar, **Ctrl+0**), with the game card (trainer, play time, money and Pokédex), the **party on top**, a **large shortcut** to each page with a summary and the **recent saves**, which open in a new tab.

### 🎨 Themes
- **Full themes**: besides the accent color, the 🎨 button changes background, panels, corners, borders and font right away. Four themes, each in light and dark: **Default**, **PSS** (X/Y, blue and very rounded), **Pixel** (GBA, square corners, thick borders and terminal font) and **Z-A** (Lumiose, black and lime). Each theme applies its suggested accent color (new color: Lime).
- More compact sidebar: light/dark, theme and language on one line, and the shortcut tips in a “⌨ Keyboard shortcuts” (hover over it).

### Fixes
- **Save sometimes did not write**: with the mouse resting on 💾 Save, the button's tooltip opened on top of it (in the window's corner there was no room below) and the click landed on the tooltip. Bottom bar tooltips now open above the buttons and no tooltip captures clicks.
- **Bag on Save**: items changed in the Bag without clicking “Apply bag” were left out of the save and did not even mark changes. Now they count as a pending change and Save (or switching tabs) applies them before writing.
- The “🌐 Language” button is no longer cut off in the sidebar, and the Feebas tip explains what the evolve button does.

## 0.3.8 — 2026-10-03

### 🌐 Interface
- **Language menu**: “🌐 Language” in the sidebar switches the interface between **Português (Brasil)** (default) and **English**. The choice is saved and the app offers to restart to apply it. Game names and legality texts stay in English; release notes and AllGenWiki descriptions exist only in Portuguese (in English, descriptions are hidden).

### ♥ Editor
- **Feebas evolves by button**: “Evolve into Milotic” now meets the requirements on its own: raises Beauty to 170, adjusts Sheen to the legal range (Gen 3/4 and BDSP), removes the Everstone and raises one level. It is only blocked at level 100 before Gen 8 or on a Pokémon that never went through games with contests (e.g. a Feebas native to Black/White).
- **Friendship evolution** also meets the requirements: removes the Everstone and, for Sylveon in Gen 6/7, raises affection to 2 hearts.

## 0.3.7 — 2026-10-02

### ♥ Editor
- **Feebas → Milotic**: evolution by Beauty in the Overview, separate from friendship, requires Beauty ≥ 170 and respects the method available in the game. BDSP uses Beauty; Z-A offers a trade. Beauty is not raised automatically. The trade can be simulated without a Prism Scale; if it is holding the scale, it is consumed. In legal mode, Beauty and trade evolutions are only accepted after validating a copy.
- **Friendship evolution**: buttons in the Overview to evolve with the game's requirements. Friendship is raised only to the minimum needed, and the level goes up when the evolution requires it. Day/night is simulated by the button chosen. Eggs, Everstone, level limits and Sylveon's requirements (Fairy move, affection in Gen 6/7 and priority over Espeon/Umbreon) are checked. In legal mode, the result is tested on a copy and only accepted if it stays legal.

### Interface
- **Contextual bottom bar**: Boxes and Party show Pokémon actions; Saves offers Open/Refresh, Encounters and Events let you use the selection, Bank syncs attached ones, Pokédex offers refresh/sync and Bag lets you apply items. Save stays available on the save's pages; Help offers Back.

### 🎁 Import and saving
- **Mystery Gift in a slot**: drop an event file (`.wc*`, `.pgf`, `.pcd` and other formats PKHeX recognizes) on a box or the party to generate the Pokémon. Replacing asks first and Ctrl+Z undoes it; item gifts are refused with an explanation.
- **Save after editing**: Save and Ctrl+S now apply the Pokémon's pending edit before writing the save, including inside ZIP. In legal mode, an edit that cannot be applied blocks saving and explains why. Write errors appear in a window and keep the save marked with changes.

### 🔄 Updates
- **What's new before updating**: the window shows the notes published on GitHub and offers “Update now” or “Later”. The automatic offer also waits for your choice before downloading and installing. When running from source, the window offers to open the download in the browser.

### 🗂 Saves
- **Save Manager**: cards show the language identified by PKHeX and the adventure start date when available. Use the arrows to move between cards and Enter to open; the “Hide SID” option is saved in preferences. The count of skipped files also appears when the folder contains no recognized save.

### 📦 Boxes
- **Name and wallpaper**: rename the box with the button next to the title and choose the game's wallpaper from the list. Tab names follow the change and the background shows discreetly behind the cards. The options respect each game's support; use Save to write to the save.

## 0.3.6 — 03/10/2026

### ✏️ Editor
- **Shiny shortcuts** (as in PKHeX): the star next to the name is now clickable (☆ normal, ★ star, ◆ square). **Click** makes it shiny or removes the shiny; **Alt+click** makes it shiny keeping the PID (changes the original trainer's SID, so Gen 3/4 Pokémon with linked PID and IVs stay legal); **Shift+click** asks for square shiny and **Ctrl+click** star shiny (the difference only exists from Gen 8 on). The "Make shiny" button on the Extras tab accepts the same shortcuts and becomes "Remove shiny" when the Pokémon is already shiny. With legal mode, Legalize respects the requested format.
- **Form selector with sprite**: each form appears with its own sprite (with the Pokémon's shininess and gender), in the list and in the field's box.
- **Clickable gender**: click ♂/♀ next to the name to change the gender. Up to Gen 5 the gender comes from the PID, so a new PID is generated with the same nature (in legal mode, Legalize redoes it if needed). For species whose form is the gender (Meowstic, Indeedee, Basculegion, Oinkologne), form and gender change together, both ways.
- **Deoxys in Gen 3**: instead of the form list, the editor explains that the form depends on the game (Normal in Ruby/Sapphire, Attack in FireRed, Defense in LeafGreen, Speed in Emerald).

### 🛡️ Legality
- **Legalize removes "Fishy" warnings**: a legal Pokémon with warnings about zeroed EVs after leveling up or EXP exactly at the level's limit (e.g. a Whismur hatched from an egg in Sapphire) is now fixed without changing the encounter or PID: it gains EVs as if from battles and a little EXP on the way to the next level. When Legalize has to regenerate, the result also comes out without these warnings.
- **Colored suggested fixes** (moves in blue, relearn highlighted, encounter in amber, IVs in green) and checked ones:
  - **Suggested encounter** works with Pokémon hatched from eggs in Gen 3 (before it said "no possible encounter"): it tries the real encounters of the origin game and keeps the one that makes it legal;
  - **Max IVs** uses the same logic as the Stats tab button (in legal mode, it offers converting to egg-hatched);
  - in legal mode, a fix that would make the Pokémon illegal is not applied and the reason appears in the status bar (before it was silently undone and the message said "applied").

## 0.3.5 — 03/10/2026

### 💾 Saves
- **Silent save**: the **💾 Save** button (and Ctrl+S) writes straight over the save file, without opening the Windows dialog; the previous file goes to the backups first. To choose another place, use **Save as...** (Ctrl+E). Saves inside .zip are written back to the zip.

### ✏️ Editor
- **Evolution by item**: on the Overview tab, species that evolve with stones and the like show the button (e.g. Nidorino + Moon Stone → Nidoking, Eevee + Fire Stone → Flareon), respecting those that depend on gender.
- **Max IVs in Gen 3/4**: in those games' wild encounters, IVs are generated along with the PID, so 31 IVs across the board is illegal. With legal mode, if the species hatches from eggs, the app asks whether it can **convert to egg-hatched** (preferably an egg from the game itself), keeping nature, gender, shiny, level, item, nickname and moves, and with all IVs maxed.

### 🎒 Bag
- **Items in columns**: each slot becomes a card with a large icon, the item and the quantity, in 2 to 4 columns depending on the width. The page uses the full width (without the editor panel), and large pouches (260 slots in Scarlet) open instantly.

### ✏️ Editor
- **Contests in legal mode**: when you raise a contest stat (Cool, Beauty...), Sheen adjusts on its own to the legal minimum, as happens in the game with Pokéblocks/Poffins; before, the change was undone. In Omega Ruby/Alpha Sapphire Sheen stays 0, and in games without contests (e.g. Scarlet/Violet) the section does not appear.
- **Friendship** disappears in Gen 1 games, which do not store this value.

### Fixes
- **Legalize with a barred nickname**: a nickname the game does not accept (e.g. "KILLER", barred by the word filter) prevented any legalization. Now it is replaced by the species name and the app tells you.
- **Legalize prefers the game itself**: encounters and eggs of the save's version come before those of another version (in a Sapphire save, it no longer uses a LeafGreen encounter).
- **Editor in Gen 1 and 2 saves**: opening many Red/Blue/Yellow/Gold/Silver/Crystal Pokémon gave an error and the editor seemed frozen, and types appeared wrong (Gengar as Steel). Game Boy games number types differently; now they are converted.
- **Restart after updating**: the button did nothing (the app could not load part of itself after replacing the exe). Now this is prepared before the replacement and, if it still fails, the app tells you to reopen it manually. It applies from the next update on: anyone on 0.3.4 or earlier still needs to close and open the app after updating.

## 0.3.4 — 03/10/2026

### 💾 Saves
- **Several saves in tabs**: each open save becomes a tab at the top (game badge, file name and ● with unexported changes). Opening another save no longer discards the current one: each tab keeps its own changes, undo/redo, box and page. **Ctrl+Tab** / **Ctrl+Shift+Tab** switch tabs, **Ctrl+W** closes; closing a tab or the app asks if there are changes. In the Save Manager, all open saves get the "Open" badge, and restoring the backup of an open save reloads its tab.

### Fixes
- **Legalize evolved Pokémon**: when the encounter is of a pre-evolution, Legalize now raises the level to the minimum where the evolution is possible (e.g. a Lv. 7 Gengar becomes a Lv. 25 Gengar from a Pokémon Tower Gastly). Before it failed and, with legal mode on, the Pokémon could not be applied.
- **The game's own encounters first**: Legalize prefers encounters of the save's generation (in a Yellow save, Pokémon Tower instead of a Gold encounter through tradeback).
- **Each save's legality rules**: when you open or switch tabs, the app applies the rules that depend on the save, as PKHeX does (Game Boy cartridge era, Gen 1-3 virtual console, active trainer).
- **Koraidon/Miraidon icon**: the Scarlet/Violet badge and Gen 9 species appeared as an unknown sprite when the active save was from an older game (PKHeX only has these species in art mode). Now they use the art sprite in those cases (Save Manager, tabs, Pokédex and bank).

### Interface
- **Sharper sprites**: sprites are upscaled 4× at integer scale (nearest neighbor) before reaching the screen, so pixels stay sharp and the same size at any card size and with Windows scaling at 125%/150% (before, some pixel lines doubled and the sprite looked crooked).

## 0.3.3 — 02/10/2026

### 🏦 Bank
- **Variants**: when you select an attached Pokémon, the versions that came back from games of another generation appear (format, level and date). You can open a variant in the open save's editor (in place of the attached copy or in an empty slot; on Apply, the attached one then points to this save) or delete it, without touching the original.

### 📖 Pokédex
- **Forms in Sword/Shield and Legends Arceus**: under "Forms and genders", a form only counts as seen or caught if the game recorded it (e.g. Galarian Meowth, Hisuian Growlithe).

### Interface
- **More visible legal mode**: it became a card in the sidebar, green with a ✓ shield when on and amber with a ! shield when off, with the state written out. Click the card to turn it on or off.

### Fixes
- **Shiny sprites**: shiny Pokémon now appear with shiny colors and the same size as the others. Before they came with normal colors, small, and the corner's sparkle icon appeared as a loose fragment (boxes, party, bank, Pokédex, encounters and events).

## 0.3.2 — 02/10/2026

### 🏦 Bank
- **Attached Pokémon**: with "🔗 Attach when taking", taking from the bank to a save copies instead of moving; the original stays in the bank, linked to the save. **Sync attached** brings the game's version back (from another generation, it stores it as a variant without touching the original) and offers to detach those that disappeared.
- **Other save** with multiple selection (move groups between the two saves and the bank) and **its own undo/redo** (↶/↷ in the panel).

### 📖 Pokédex
- **Forms**: in Gen 4, 5, 6, BDSP, Scarlet/Violet and Legends Z-A, each form only counts as seen/caught if the game recorded it.
- **Sync all saves**: records in each save of the folder what was caught or seen in the others and what is stored; closed ones are written right away, with a backup.

### Fixes
- Marking "seen only" in the Gen 4 and 5 Pokédex now works; in games that do not accept it (Gen 7), sync does not promise seen entries.

## 0.3.1 — 02/10/2026

### ✏️ Editor
- New **Ribbons and memories** tab: all the format's ribbons, with search, "Only owned", **All legal** and **Remove all**; **memories** of the original and current trainer (Gen 6+), with detail, intensity and feeling.
- **Contest stats** (Cool, Beauty, Cute, Smart, Tough, Sheen), **Dynamax Level** and **Gigantamax** (Sword/Shield), **alpha** and **noble** (Legends) on the Extras tab.
- **Where to learn**: on hovering over a move, the TM's location and the tutor in this game (AllGenWiki).

### 🎒 Bag
- **TMs, TRs and HMs** show the move they teach, its description and where to get the machine in this game.

### 🔄 Automatic update
- First version delivered through 0.3.0's automatic update.

## 0.3.0 — 02/10/2026

### 🛡 Legal mode
- A switch in the sidebar, **on by default**: the editor only offers legal options (moves the Pokémon learns, balls allowed for the encounter, the game's species).
- Changes that would make the Pokémon illegal are undone right away, with the reason in the status bar.
- **Apply** only writes a legal Pokémon; changing the species or pasting a Showdown set legalizes on its own.
- A Pokémon **from outside** (.pk* file, bank or other save) that arrives illegal to the open save gets the option **✨ Legalize** or **Bring as is**.
- Ability, form and Tera Type list only legal options; battle forms leave the list.
- **Change encounter** (Encounter tab): choose a real encounter and the Pokémon is regenerated from it, instead of editing location and level by hand.
- **Make shiny** is blocked on shiny-locked encounters and, on the others, regenerates it already shiny with the right PID/IV.

### ✏️ Editor
- **Ability** chosen from a list (with the hidden one), **form** and **Tera Type** (Scarlet/Violet).
- **Editable PID and EC**, **markings** (●▲■♥★◆, blue/pink in Gen 7+) and **Hyper Training** (Gen 7+).
- Portuguese descriptions of **moves and abilities** (AllGenWiki), in the list and in the field.

### 🎮 Game badge
- The cover Pokémon in the version's colors identifies each save in the Save Manager and on the open save's card.

### ❔ Help, what's new and updates
- New **Help** page (F1) with all features explained, this changelog and the About tab.
- **Automatic update**: on opening, the app checks whether a new version is out on GitHub, downloads it, checks the file (SHA-256) and installs it on its own. The new version applies on restart (🔄 button in the sidebar, which asks first if there are unsaved changes). You can turn it off in Help › About.
- Also check out **AllGenWiki**, a Pokémon encyclopedia of all nine generations (Help › About).

### 🌿 Encounters and events
- **Details panel**: click a card to see what the encounter guarantees (shiny or shiny lock, IVs, ability, nature, gender, ball, item, Tera Type, alpha/Gigantamax, event trainer) and the moves it comes with.
- **Filters** by encounter type, game version and move; a **HOME** badge on Pokémon HOME gifts and 🔒 on shiny-locked encounters.

### 🗜 Saves inside .zip
- **JKSV backups** (or any .zip with saves) appear in the Save Manager with the **ZIP** badge, one card per save; you can also open a .zip directly.
- Save offers to **write back inside the zip** (with a backup of the zip first) or save as a separate file. The Pokédex and the "Other save" also read these saves.

### 🎒 Bag
- Each item's icon in the bag and in the editor's held item.
- On hovering over an item, its description in Portuguese and where to get it (AllGenWiki data).

## 0.2.0 — 02/10/2026

### 📖 Centralized Pokédex
- All 1025 species, combining the Pokédex of every save in the folder, the open save and the bank.
- Living dex and shiny dex, filters by status, generation, type and source; forms and genders as their own entries.
- Where each Pokémon is, with **Go** to open it in the editor, and **Sync** with the open save.

### 🏦 Local bank
- Its own storage in banks → boxes, with Pokémon as .pk* files in their original format.
- Two screens (bank | save) with generation conversion; **other save** in place of the bank; **external folders** as banks.

### 📦 Boxes and party
- **Multiple selection** (Ctrl+click, Shift+click, Ctrl+A) to move or delete in groups.
- **Sort boxes** by Pokédex, name, level, shiny, type, IVs or capture date.
- Boxes in tabs, ball icon on cards and a list of unexported changes.

### ✏️ Editor
- **Evolve by trade**, respecting the Everstone and recording the trade partner in Gen 6+.
- **Check legality** of the whole save, with a shortcut to the first problem.
- Gen 1–3 events in the Mystery Gift database.

### 🛟 Backups
- Automatic backup before saving over the file, with a list, **Restore** and **Restore as...**.

## 0.1.0 — 01/10/2026

First version: a new interface, in Portuguese, on top of the same PKHeX.Core.

### What's in it
- **Save Manager** as the home screen, with the saves in a folder grouped by console.
- **Boxes and party** with large cards, legality, drag and drop and a summary on hover.
- **Tabbed editor** with search as you type, moves the Pokémon learns in green, IV/EV with a chart.
- **Legalize** from a real encounter in the game (shiny included).
- **Encounter** and **event (Mystery Gift)** databases, with "Use" to take them to the editor.
- **Global search** (Ctrl+F), undo/redo, delete, keyboard shortcuts and confirmations.
- Light/dark theme and configurable accent color.
