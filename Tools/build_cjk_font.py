"""Corta a Noto Sans SC para os caracteres chineses que o PKHeX Modern mostra.

Uso: python Tools/build_cjk_font.py <pasta com NotoSansSC-Regular.otf e NotoSansSC-Bold.otf>

A fonte original (github.com/notofonts/noto-cjk, Sans/SubsetOTF/SC, licenca OFL) tem uns 8 MB por peso.
O corte fica com:
- todos os caracteres do GB2312 (os 6.763 hanzi comuns, pontuacao e kana);
- os caracteres dos textos zh-Hans do PKHeX.Core (especies, golpes, itens, lugares, legalidade);
- os do dicionario da interface (PKHeX.Modern/Assets/lang/zh-Hans.json).
So entram blocos CJK (sem letras latinas nem simbolos): a fonte e usada como reserva (FontFallback) e nao
muda a aparencia dos outros idiomas. Caracteres fora do corte (raros, em apelidos) caem na fonte do sistema.
Rode de novo quando o zh-Hans.json ganhar textos com caracteres novos. Requer: pip install fonttools.
"""
import json
import pathlib
import sys

from fontTools import subset

ROOT = pathlib.Path(__file__).resolve().parent.parent
OUT = ROOT / "PKHeX.Modern" / "Assets" / "Fonts"

# Blocos CJK: radicais, pontuacao, kana, bopomofo, letras CJK cercadas, ideogramas (ext. A, unificados,
# compatibilidade), formas verticais e formas de largura total.
RANGES = [(0x2E80, 0x2FDF), (0x3000, 0x312F), (0x31F0, 0x32FF), (0x3400, 0x4DBF), (0x4E00, 0x9FFF),
          (0xF900, 0xFAFF), (0xFE30, 0xFE4F), (0xFF00, 0xFFEF)]


def is_cjk(cp: int) -> bool:
    return any(a <= cp <= b for a, b in RANGES)


def gb2312() -> set[int]:
    chars = set()
    for hi in range(0xA1, 0xF8):
        for lo in range(0xA1, 0xFF):
            try:
                chars.add(ord(bytes([hi, lo]).decode("gb2312")))
            except UnicodeDecodeError:
                pass
    return chars


def texts() -> set[int]:
    chars = set()
    core = ROOT / "PKHeX.Core" / "Resources"
    for f in core.rglob("*"):
        if f.is_file() and ("zh-hans" in f.name.lower() or "zh-hans" in str(f.parent).lower()):
            chars.update(map(ord, f.read_text(encoding="utf-8", errors="ignore")))
    ui = ROOT / "PKHeX.Modern" / "Assets" / "lang" / "zh-Hans.json"
    for value in json.loads(ui.read_text(encoding="utf-8")).values():
        chars.update(map(ord, value))
    # Textos ja escritos em chines no codigo (nome do idioma e pergunta de reiniciar em Services/Loc.cs).
    chars.update(map(ord, (ROOT / "PKHeX.Modern" / "Services" / "Loc.cs").read_text(encoding="utf-8")))
    return chars


def main(src: pathlib.Path) -> None:
    unicodes = sorted(cp for cp in gb2312() | texts() if is_cjk(cp))
    print(f"{len(unicodes)} caracteres")
    for weight in ("Regular", "Bold"):
        options = subset.Options()
        options.layout_features = ["*"]
        options.name_IDs = ["*"]
        options.name_languages = ["*"]
        options.notdef_outline = True
        options.hinting = False
        font = subset.load_font(str(src / f"NotoSansSC-{weight}.otf"), options)
        sub = subset.Subsetter(options)
        sub.populate(unicodes=unicodes)
        sub.subset(font)
        target = OUT / f"NotoSansSC-{weight}.otf"
        subset.save_font(font, str(target), options)
        print(f"{target.name}: {target.stat().st_size / 1024 / 1024:.1f} MB")


if __name__ == "__main__":
    main(pathlib.Path(sys.argv[1]))
