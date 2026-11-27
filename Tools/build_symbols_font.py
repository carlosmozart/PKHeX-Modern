"""Gera a PKHeX Modern Symbols: os simbolos da interface que nenhuma fonte embutida tem.

Uso: python Tools/build_symbols_font.py   (requer: pip install fonttools)

A interface usa simbolos Unicode como icones (desfazer, refazer, menu, atualizar...). Inter, Noto Sans Symbols 2 e
Noto Emoji nao tem estes onze; no Windows eles vem da Segoe UI Symbol, mas num Mac Intel apareceram como
quadradinhos (issue 1 do repositorio: o macOS caiu na fonte LastResort). Os desenhos sao feitos aqui, em codigo,
sem copiar nenhuma fonte; a fonte e registrada como reserva (Theme/FontFallbacks.cs) em todos os sistemas, para
os icones serem iguais em todo lugar. Para acrescentar um simbolo, desenhe-o em GLYPHS e rode de novo; a suite
Symbols confere que todo simbolo usado no app tem desenho numa fonte embutida.
"""
import math
import pathlib

from fontTools.fontBuilder import FontBuilder
from fontTools.misc.timeTools import timestampFromString
from fontTools.pens.ttGlyphPen import TTGlyphPen
from fontTools.ttLib.tables._g_l_y_f import flagOverlapSimple

ROOT = pathlib.Path(__file__).resolve().parent.parent
OUT = ROOT / "PKHeX.Modern" / "Assets" / "Fonts" / "PKHeXModernSymbols.ttf"

UPM = 1000
ASCENT, DESCENT = 960, -240  # as da Inter, para nao mudar a altura da linha
SIDE = 70                    # margem de cada lado do desenho
STROKE = 74                  # espessura do traco (perto do peso da Inter)


# ---------- geometria (contornos em y para cima; externos no sentido horario, furos no anti-horario) ----------

def area(pts):
    return sum(x0 * y1 - x1 * y0 for (x0, y0), (x1, y1) in zip(pts, pts[1:] + pts[:1])) / 2


def cw(pts):
    return pts[::-1] if area(pts) > 0 else pts


def ccw(pts):
    return pts[::-1] if area(pts) < 0 else pts


def circle(cx, cy, r, n=72):
    return cw([(cx + r * math.cos(2 * math.pi * i / n), cy + r * math.sin(2 * math.pi * i / n)) for i in range(n)])


def ring(cx, cy, r, w):
    return [circle(cx, cy, r + w / 2), ccw(circle(cx, cy, r - w / 2))]


def segment(p, q, w):
    (x0, y0), (x1, y1) = p, q
    d = math.hypot(x1 - x0, y1 - y0)
    nx, ny = -(y1 - y0) / d * w / 2, (x1 - x0) / d * w / 2
    return cw([(x0 + nx, y0 + ny), (x1 + nx, y1 + ny), (x1 - nx, y1 - ny), (x0 - nx, y0 - ny)])


def polyline(points, w, closed=False, caps=True):
    """Traco com juncoes redondas (um retangulo por trecho e um circulo em cada vertice)."""
    pts = points + points[:1] if closed else points
    out = [segment(a, b, w) for a, b in zip(pts, pts[1:])]
    joints = points if closed or caps else points[1:-1]
    out += [circle(x, y, w / 2, 36) for x, y in joints]
    return out


def arc(cx, cy, r, w, a0, a1, step=3):
    """Faixa de arco de a0 a a1 graus (a1 > a0: sentido anti-horario)."""
    n = max(2, int(abs(a1 - a0) / step) + 1)
    angles = [math.radians(a0 + (a1 - a0) * i / (n - 1)) for i in range(n)]
    outer = [(cx + (r + w / 2) * math.cos(a), cy + (r + w / 2) * math.sin(a)) for a in angles]
    inner = [(cx + (r - w / 2) * math.cos(a), cy + (r - w / 2) * math.sin(a)) for a in angles]
    return cw(outer + inner[::-1])


def head(base, direction, length, width):
    """Ponta de seta: base no fim do traco, apontando para direction."""
    (bx, by), (dx, dy) = base, direction
    d = math.hypot(dx, dy)
    dx, dy = dx / d, dy / d
    px, py = -dy * width / 2, dx * width / 2
    return cw([(bx + dx * length, by + dy * length), (bx + px, by + py), (bx - px, by - py)])


def arc_arrow(cx, cy, r, a0, a1, w=STROKE, hl=210, hw=250):
    """Arco de a0 a a1 com ponta de seta no fim (no sentido do movimento) e ponta redonda no comeco."""
    end = math.radians(a1)
    sign = 1 if a1 > a0 else -1
    tangent = (-math.sin(end) * sign, math.cos(end) * sign)
    start = math.radians(a0)
    return [arc(cx, cy, r, w, min(a0, a1), max(a0, a1)),
            circle(cx + r * math.cos(start), cy + r * math.sin(start), w / 2, 36),
            head((cx + r * math.cos(end), cy + r * math.sin(end)), tangent, hl, hw)]


def straight_arrow(tail, tip, w=70, hl=190, hw=210):
    (x0, y0), (x1, y1) = tail, tip
    d = math.hypot(x1 - x0, y1 - y0)
    dx, dy = (x1 - x0) / d, (y1 - y0) / d
    base = (x1 - dx * hl, y1 - dy * hl)
    return [segment(tail, (base[0] + dx * 2, base[1] + dy * 2), w), head(base, (dx, dy), hl, hw)]


def mirror(contours):
    return [cw([(-x, y) for x, y in c]) if area(c) < 0 else ccw([(-x, y) for x, y in c]) for c in contours]


# ---------- desenhos ----------

def undo():  # ↶ arco por cima, ponta descendo a esquerda
    return arc_arrow(0, 290, 270, 0, 180)


def redo():  # ↷
    return mirror(undo())


def clockwise():  # ⟳ como o icone de atualizar: aberto no canto de cima a direita, ponta no alto apontando a direita
    return arc_arrow(0, 320, 285, 10, -280, hl=200, hw=240)


def anticlockwise():  # ↺
    return mirror(clockwise())


def ellipsis():  # ⋯
    return [circle(x, 320, 58) for x in (0, 280, 560)]


def right_over_left():  # ⇄
    return straight_arrow((0, 470), (800, 470)) + straight_arrow((800, 210), (0, 210))


def up_and_down():  # ⇅
    return straight_arrow((0, -20), (0, 720)) + straight_arrow((320, 720), (320, -20))


def house():  # ⌂
    return polyline([(0, 0), (0, 390), (330, 690), (660, 390), (660, 0)], STROKE, closed=True)


def search():  # ⌕ lupa
    cx, cy, r = 280, 430, 225
    a = math.radians(-45)
    return ring(cx, cy, r, STROKE) + polyline(
        [(cx + (r + 20) * math.cos(a), cy + (r + 20) * math.sin(a)), (cx + 520 * math.cos(a), cy + 520 * math.sin(a))], 100)


def info():  # ⓘ
    return ring(0, 340, 335, 64) + [circle(0, 525, 52), cw([(-36, 130), (36, 130), (36, 410), (-36, 410)])]


def flag():  # ⚑
    return [cw([(0, -40), (72, -40), (72, 720), (0, 720)]),
            cw([(72, 720), (610, 720), (480, 555), (610, 390), (72, 390)])]


GLYPHS = {
    0x21B6: ("undo", undo), 0x21B7: ("redo", redo), 0x21BA: ("anticlockwise", anticlockwise),
    0x27F3: ("clockwise", clockwise), 0x22EF: ("ellipsis", ellipsis), 0x21C4: ("rightoverleft", right_over_left),
    0x21C5: ("upanddown", up_and_down), 0x2302: ("house", house), 0x2315: ("search", search),
    0x24D8: ("info", info), 0x2691: ("flag", flag),
}


def build():
    names = [".notdef", "space"] + [name for name, _ in GLYPHS.values()]
    fb = FontBuilder(UPM, isTTF=True)
    fb.setupGlyphOrder(names)
    fb.setupCharacterMap({0x20: "space", **{cp: name for cp, (name, _) in GLYPHS.items()}})

    glyphs, metrics = {}, {}
    empty = TTGlyphPen(None)
    glyphs[".notdef"] = glyphs["space"] = empty.glyph()
    metrics[".notdef"] = metrics["space"] = (300, 0)
    for name, draw in GLYPHS.values():
        contours = draw()
        xs = [x for c in contours for x, _ in c]
        shift = SIDE - min(xs)
        pen = TTGlyphPen(None)
        for c in contours:
            pts = [(round(x + shift), round(y)) for x, y in c]
            pen.moveTo(pts[0])
            for p in pts[1:]:
                pen.lineTo(p)
            pen.closePath()
        g = pen.glyph()
        g.flags[0] |= flagOverlapSimple  # contornos sobrepostos de proposito (uniao por nonzero)
        glyphs[name] = g
        width = round(max(xs) - min(xs))
        metrics[name] = (width + 2 * SIDE, SIDE)

    fb.setupGlyf(glyphs)
    fb.setupHorizontalMetrics(metrics)
    fb.setupHorizontalHeader(ascent=ASCENT, descent=DESCENT)
    fb.setupNameTable({
        "familyName": "PKHeX Modern Symbols",
        "styleName": "Regular",
        "copyright": "PKHeX Modern contributors",
        "version": "Version 1.000",
        "licenseDescription": "GPL-3.0, like the PKHeX Modern app (github.com/carlosmozart/PKHeX-Modern)",
    })
    fb.setupOS2(sTypoAscender=ASCENT, sTypoDescender=DESCENT, sTypoLineGap=0, usWinAscent=ASCENT,
                usWinDescent=-DESCENT, sxHeight=546, sCapHeight=727, fsType=0, achVendID="PKHM")
    fb.setupPost()
    fb.updateHead(created=timestampFromString("Fri Nov 27 00:00:00 2026"),
                  modified=timestampFromString("Fri Nov 27 00:00:00 2026"))
    fb.save(str(OUT))
    print(f"{OUT.relative_to(ROOT)}: {len(GLYPHS)} simbolos, {OUT.stat().st_size} bytes")


if __name__ == "__main__":
    build()
