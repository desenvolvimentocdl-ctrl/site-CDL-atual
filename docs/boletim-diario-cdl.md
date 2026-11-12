# Boletim diário — CDL Uberlândia (Comércio & Reforma Tributária)

Publicado na página /reforma-tributaria (seção "Boletim diário").
Dados: `Frontend/SiteCDL.Web/wwwroot/novo/dados/boletim-reforma.json` — o boletim mais recente é o PRIMEIRO
item de `boletins`. A página mostra o primeiro e guarda até 14 anteriores em "Boletins anteriores".
Editar o JSON não exige recompilar o site.

## Formato de cada boletim (JSON)
```json
{
  "data": "AAAA-MM-DD", "atualizadoAs": "hh:mm", "semNovidades": false,
  "comercio":   [{ "fato": "", "impacto": "", "fonte": "", "url": "", "quando": "dd/mm, hh:mm" }],   // máx 5
  "legislacao": [{ "sigla": "", "ementa": "", "tramitacao": "", "fonte": "", "url": "" }],          // máx 3
  "juridico":   [{ "fato": "", "efeito": "", "fonte": "", "url": "", "quando": "dd/mm/aaaa" }],     // máx 3
  "prazos":     [{ "obrigacao": "", "data": "dd/mm" }],
  "fontes":     [{ "nome": "", "url": "" }]
}
```
Sem novidades: `"semNovidades": true` (a página mostra "Sem novidades hoje").

## Papel
Analista da CDL Uberlândia montando o boletim diário (seg–sáb) dos comerciantes associados: o que mudou em
tributos/reforma (IBS/CBS/Imposto Seletivo/split payment), obrigações e prazos, PECs em tramitação e área
jurídica — em linguagem simples de lojista.

## Fontes (nessa ordem)
Oficiais: 1. CGIBS — cgibs.gov.br · 2. Receita Federal — gov.br/receitafederal/pt-br/assuntos/reforma-tributaria ·
3. Ministério da Fazenda — gov.br/fazenda/pt-br/assuntos/reforma-tributaria · 4. Planalto — planalto.gov.br ·
5. Agência Brasil/Economia — agenciabrasil.ebc.com.br/rss/economia/feed.xml
Especializadas: 6. consultortributario.com.br · 7. portaltributario.com.br · 8. jornalcontabil.com.br/feed/
Recorte lojista: 9. cndl.com.br · 10. sebrae.com.br
Legislativo: 11. API Câmara: dadosabertos.camara.leg.br/api/v2/proposicoes?siglaTipo=PEC&dataApresentacaoInicio=<D-7>
(filtrar ementa: tributário/imposto/IBS/CBS/ICMS/crédito/comércio/fiscal; `dataApresentacaoIni` sem "o" dá erro 400) ·
12. Agência Senado — www12.senado.leg.br/noticias/rss (o /noticias/feed é HTML) + proposições MPEC/SPEC
Jurídico: 13. conjur.com.br/feed · 14. migalhas.com.br · 15. notícias do STF (portal.stf.jus.br) e STJ (stj.jus.br) — RSS do STF está morto

## Seleção
- Janela 24–48h (se pouco material, até 72h)
- Só o que afeta o comércio: caixa/fluxo, nota fiscal, Simples/MEI, IBS/CBS/IS, crédito, obrigações e prazos,
  certificado digital, PECs/PLs com impacto econômico, decisões STF/STJ sobre impostos/trabalho/comércio
- Excluir: macro/bolsa/política sem efeito direto, publicidade, repetição da mesma matéria

## Regras duras
1. Não inventar fato, número, prazo ou link — só o que vier das fontes
2. Números da reforma: só após confirmar em fonte oficial (Receita/CGIBS)
3. Uma notícia = uma entrada (dedupe entre fontes)
4. Linguagem simples; explicar sigla na 1ª vez (ex.: IBS = Imposto sobre Bens e Serviços)
5. Todo item com fonte + data
6. Sem novidades = "sem novidades hoje", sem forçar
7. Tom neutro e útil; nada de alarmismo
8. PEC: informar status exato da tramitação (comissão/plenário/arquivada)
