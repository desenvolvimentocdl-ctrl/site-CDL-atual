#!/usr/bin/env python3
"""
Gera docs/apresentacao/esteira-dados.js a partir do histórico do Git.

A página esteira.html lê esse arquivo. Ele é refeito automaticamente a cada
push no master pelo GitHub Actions (.github/workflows/esteira.yml), mas também
pode ser gerado à mão:

    python docs/apresentacao/gerar-esteira.py

Classificação: o tipo de cada commit é deduzido do título. Para forçar um tipo,
escreva no corpo do commit:  [tipo: Correção]  (ou GEO, Visual, Funcionalidade,
Infra, Conteúdo, Melhoria).
"""
import json, os, re, subprocess, sys
from datetime import datetime, timezone

RAIZ = subprocess.run(["git", "rev-parse", "--show-toplevel"], capture_output=True, text=True, check=True).stdout.strip()
SAIDA = os.path.join(RAIZ, "docs", "apresentacao", "esteira-dados.js")

SEP_C, SEP_F = "\x1e", "\x1f"

# Ordem importa: a primeira regra que casar define o tipo.
REGRAS = [
    ("Base",           r"^adicionar arquivos de projeto"),
    ("GEO",            r"\bgeo\b|\bseo\b|llms\.txt|sitemap|robots\.txt|dados estruturados"),
    ("Infra",          r"solu[cç][aã]o|\.sln|nuget|docker|\bbuild\b|pipeline|workflow|deploy|esteira"),
    ("Correção",       r"corrig|corre[cç][aã]o|conserta|n[aã]o aparecia|aparecem|sumiam|cortad|espremid|perdid|quebrad|\berro\b|\bbug\b|falha"),
    ("Visual",         r"novo visual|\bfontes?\b|tipografia"),
    ("Funcionalidade", r"\bnov[oa]s?\b|painel|adiciona|\bcria\b|\brota\b|integra|vai direto|passam pelo"),
    ("Conteúdo",       r"hist[oó]ria|\btexto|conte[uú]do|publica[cç]"),
    ("Visual",         r"visual|layout|bot[aã]o|\bcor(es)?\b|mobile|celular|\bcss\b"),
]

def tipo_de(titulo, corpo):
    m = re.search(r"\[tipo:\s*([^\]]+)\]", corpo, re.I)
    if m:
        return m.group(1).strip()
    t = titulo.lower()
    for nome, rx in REGRAS:
        if re.search(rx, t):
            return nome
    return "Melhoria"

def area_de(caminho):
    p = caminho.replace("\\", "/")
    if p.startswith("docs/") or p.startswith(".github/"): return "Documentação"
    if p.startswith("Backend/"): return "Back-end (API)"
    if "/wwwroot/" in p:
        if re.search(r"\.(css|js)$", p): return "Estilos e scripts"
        return "Imagens e arquivos"
    if "/Views/" in p: return "Telas (Razor)"
    if p.startswith("Frontend/"): return "Front-end (C#)"
    return "Configuração"

def main():
    fmt = SEP_C + SEP_F.join(["%H", "%h", "%aI", "%an", "%s", "%b"]) + SEP_F
    out = subprocess.run(["git", "-C", RAIZ, "log", "--no-merges", "--numstat", "--format=" + fmt],
                         capture_output=True, text=True, encoding="utf-8", check=True).stdout
    commits = []
    for bloco in out.split(SEP_C)[1:]:
        partes = bloco.split(SEP_F)
        h, curto, data, autor, titulo, corpo = partes[:6]
        resto = partes[6] if len(partes) > 6 else ""
        if "[skip ci]" in titulo or autor.endswith("[bot]"):
            continue
        linhas_corpo = [l for l in corpo.strip().splitlines()
                        if not re.match(r"^\s*(co-authored-by|claude-session|signed-off-by):", l, re.I)]
        corpo_limpo = "\n".join(linhas_corpo).strip()
        arquivos, adic, rem, areas = [], 0, 0, {}
        for l in resto.strip().splitlines():
            c = l.split("\t")
            if len(c) != 3: continue
            a, r, caminho = c
            caminho = re.sub(r"\{[^}]*=> ([^}]*)\}", r"\1", caminho).replace("//", "/")
            a_i = int(a) if a.isdigit() else 0
            r_i = int(r) if r.isdigit() else 0
            adic += a_i; rem += r_i
            ar = area_de(caminho)
            areas[ar] = areas.get(ar, 0) + 1
            arquivos.append({"p": caminho, "a": a_i, "r": r_i, "bin": a == "-"})
        tipo = tipo_de(titulo, corpo)
        if arquivos and all(f["p"].startswith(("docs/", ".github/")) for f in arquivos):
            tipo = "Documentação"
        commits.append({
            "hash": h, "curto": curto, "data": data, "autor": autor,
            "titulo": titulo.strip(), "corpo": corpo_limpo,
            "tipo": tipo,
            "adicionadas": adic, "removidas": rem,
            "totalArquivos": len(arquivos), "areas": areas,
            "arquivos": sorted(arquivos, key=lambda x: -(x["a"] + x["r"]))[:40],
        })
    ramo = subprocess.run(["git", "-C", RAIZ, "rev-parse", "--abbrev-ref", "HEAD"], capture_output=True, text=True).stdout.strip()
    dados = {
        "geradoEm": datetime.now(timezone.utc).isoformat(timespec="seconds"),
        "ramo": ramo,
        "commits": commits,
    }
    with open(SAIDA, "w", encoding="utf-8", newline="\n") as f:
        f.write("// Arquivo gerado por gerar-esteira.py. Não edite à mão.\n")
        f.write("window.ESTEIRA = ")
        json.dump(dados, f, ensure_ascii=False, indent=1)
        f.write(";\n")
    print(f"{len(commits)} commits gravados em {os.path.relpath(SAIDA, RAIZ)}")

if __name__ == "__main__":
    sys.exit(main())
