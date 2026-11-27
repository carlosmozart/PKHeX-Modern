"""Gera PKHeX.Modern/Assets/move-info.json: os golpes de status, que a analise de tipos da equipe nao conta como ataque.

Uso: python Tools/build_move_info.py [caminho do HoennKantoWiki]

O PKHeX.Core sabe o tipo de cada golpe, mas nao a classe (fisico, especial ou status). Ela vem do AllGenWiki
(projeto HoennKantoWiki, data/genN/moves.json, chaves como "night-shade"), casando com os nomes em ingles do Core.
Do mais novo para o mais velho: um golpe que saiu dos jogos novos ainda aparece nas geracoes antigas. A classe de
status nao muda entre geracoes. Golpes sem par (Z, Max, alguns exclusivos) ficam de fora e contam como ataque.
"""
import json
import os
import re
import sys

ROOT = os.path.join(os.path.dirname(os.path.abspath(__file__)), '..')
wiki = sys.argv[1] if len(sys.argv) > 1 else os.path.join(ROOT, '..', 'HoennKantoWiki')
names_path = os.path.join(ROOT, 'PKHeX.Core', 'Resources', 'text', 'other', 'en', 'text_Moves_en.txt')
out_path = os.path.join(ROOT, 'PKHeX.Modern', 'Assets', 'move-info.json')


def slug(name: str) -> str:
    name = name.lower().replace("'", '').replace('’', '').replace('.', '').replace(',', '')
    return re.sub(r'[^a-z0-9]+', '-', name).strip('-')


classes = {}
for gen in range(9, 0, -1):
    path = os.path.join(wiki, 'data', f'gen{gen}', 'moves.json')
    if not os.path.exists(path):
        continue
    with open(path, encoding='utf-8') as f:
        for key, move in json.load(f).items():
            classes.setdefault(key, move.get('classe'))

with open(names_path, encoding='utf-8') as f:
    names = f.read().splitlines()

# Golpes de status que o AllGenWiki nao lista (eventos do Gen 6/7); Vise Grip e Hold Back sao de ataque.
EXTRA_STATUS = {603: 'Happy Hour', 606: 'Celebrate', 607: 'Hold Hands'}
# Golpes Z, Max e os Torque dos Starmobiles nao ficam nos Pokemon: nao entram na lista de sem par.
UNKNOWABLE = set(range(622, 659)) | set(range(695, 704)) | {719} | set(range(723, 729)) | set(range(743, 775)) | set(range(896, 901))

status, missing = [], []
for move_id, name in enumerate(names):
    if move_id == 0 or not name.strip():
        continue
    cls = 'status' if move_id in EXTRA_STATUS else classes.get(slug(name))
    if cls is None:
        if move_id not in UNKNOWABLE:
            missing.append(f'{move_id} {name}')
    elif cls == 'status':
        status.append(move_id)

with open(out_path, 'w', encoding='utf-8', newline='\n') as f:
    json.dump({'source': 'AllGenWiki (HoennKantoWiki data/genN/moves.json)', 'status': status}, f, separators=(',', ':'))
    f.write('\n')
print(f'{len(status)} golpes de status; {len(missing)} sem par no AllGenWiki (contam como ataque):')
print(', '.join(missing))
