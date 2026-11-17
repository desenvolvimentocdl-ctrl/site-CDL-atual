/* Agenda de Uberlândia (/agenda-uberlandia): calendário mensal + lista, a partir de #agenda-dados (wwwroot/novo/dados/agenda-uberlandia.json). */
(function () {
  var el = document.getElementById('agenda-dados');
  if (!el) return;
  var D; try { D = JSON.parse(el.textContent || '{}'); } catch (e) { D = {}; }
  var EV = (D.eventos || []).filter(function (e) { return /^\d{4}-\d{2}-\d{2}$/.test(e.data || ''); });
  var FE = (D.feriados || []).filter(function (f) { return /^\d{4}-\d{2}-\d{2}$/.test(f.data || ''); });

  var MESES = ['janeiro', 'fevereiro', 'março', 'abril', 'maio', 'junho', 'julho', 'agosto', 'setembro', 'outubro', 'novembro', 'dezembro'];
  var MES3 = ['JAN', 'FEV', 'MAR', 'ABR', 'MAI', 'JUN', 'JUL', 'AGO', 'SET', 'OUT', 'NOV', 'DEZ'];
  var SEMANA = ['domingo', 'segunda-feira', 'terça-feira', 'quarta-feira', 'quinta-feira', 'sexta-feira', 'sábado'];
  var CATS = {
    feriado: { nome: 'Feriados', cor: '#e5484d' },
    negocios: { nome: 'Negócios', cor: '#0b5cd5' },
    cultura: { nome: 'Cultura', cor: '#8e4ec6' },
    educacao: { nome: 'Educação', cor: '#0d9488' },
    natal: { nome: 'Natal', cor: '#d6336c' },
    comunidade: { nome: 'Comunidade', cor: '#e8590c' }
  };

  function esc(s) { return String(s == null ? '' : s).replace(/[&<>"']/g, function (c) { return { '&': '&amp;', '<': '&lt;', '>': '&gt;', '"': '&quot;', "'": '&#39;' }[c]; }); }
  function iso(d) { return d.getFullYear() + '-' + ('0' + (d.getMonth() + 1)).slice(-2) + '-' + ('0' + d.getDate()).slice(-2); }
  function parse(s) { var p = s.split('-'); return new Date(+p[0], +p[1] - 1, +p[2]); }
  function url(u) { return /^https?:\/\//i.test(u || '') || /^\/[^/]/.test(u || '') ? u : ''; }
  var HOJE = iso(new Date());

  // Cada evento ocupa os dias de data até fim (inclusive).
  function dias(e) {
    var out = [], a = parse(e.data), b = parse(e.fim && /^\d{4}-\d{2}-\d{2}$/.test(e.fim) ? e.fim : e.data), n = 0;
    while (a <= b && n++ < 62) { out.push(iso(a)); a.setDate(a.getDate() + 1); }
    return out;
  }
  var porDia = {};
  EV.forEach(function (e) { dias(e).forEach(function (d) { (porDia[d] = porDia[d] || []).push(e); }); });
  var ferDia = {};
  FE.forEach(function (f) { (ferDia[f.data] = ferDia[f.data] || []).push(f); });
  function ultimoDia(e) { return e.fim && /^\d{4}-\d{2}-\d{2}$/.test(e.fim) ? e.fim : e.data; }
  var proximos = EV.filter(function (e) { return ultimoDia(e) >= HOJE; }).sort(function (a, b) { return (a.data + (a.hora || '')).localeCompare(b.data + (b.hora || '')); });

  function dataLonga(s) { var d = parse(s); return SEMANA[d.getDay()] + ', ' + d.getDate() + ' de ' + MESES[d.getMonth()]; }
  function periodo(e) {
    var a = parse(e.data), t = a.getDate() + ' de ' + MESES[a.getMonth()];
    if (e.fim && e.fim !== e.data) { var b = parse(e.fim); t = a.getMonth() === b.getMonth() ? a.getDate() + (b - a > 864e5 * 1.5 ? ' a ' : ' e ') + b.getDate() + ' de ' + MESES[b.getMonth()] : t + ' a ' + b.getDate() + ' de ' + MESES[b.getMonth()]; }
    return t;
  }
  function horaTxt(h) { return h ? h.replace(':00', 'h').replace(':', 'h') : 'Horário a confirmar'; }
  function cat(e) { return CATS[e.categoria] ? e.categoria : 'comunidade'; }
  // Imagem de divulgação (campo "imagem" do JSON); sem imagem, mostra uma capa com a data na cor da categoria.
  function img(e, cls) {
    var u = /^https:\/\//i.test(e.imagem || '') ? e.imagem : '', d = parse(e.data || HOJE), c = CATS[cat(e)] || CATS.comunidade;
    if (!u) return '<div class="ag-img ph ' + (cls || '') + '" style="--c:' + c.cor + '" aria-hidden="true"><b>' + d.getDate() + '</b><span>' + MES3[d.getMonth()] + '</span></div>';
    return '<div class="ag-img ' + (cls || '') + '" style="--img:url(&quot;' + esc(u) + '&quot;)"><img src="' + esc(u) + '" alt="Imagem de divulgação: ' + esc(e.titulo) + '" loading="lazy" referrerpolicy="no-referrer" onerror="this.parentNode.classList.add(&#39;erro&#39;)"></div>';
  }

  /* ---------- Resumo no topo ---------- */
  var pe = document.getElementById('ag-prox-evento');
  if (pe) {
    var e0 = proximos.filter(function (e) { return !e.secundario; })[0] || proximos[0];
    pe.innerHTML = '<small>Próximo evento</small>' + (e0
      ? '<b>' + esc(e0.titulo) + '</b><span>' + esc(periodo(e0)) + (e0.hora ? ' · ' + esc(horaTxt(e0.hora)) : '') + ' · ' + esc(e0.local) + '</span>'
      : '<b>Nenhum evento agendado</b>');
  }
  var pf = document.getElementById('ag-prox-feriado');
  if (pf) {
    var f0 = FE.filter(function (f) { return f.data >= HOJE && f.tipo !== 'facultativo'; }).sort(function (a, b) { return a.data.localeCompare(b.data); })[0];
    if (f0) {
      var falta = Math.round((parse(f0.data) - parse(HOJE)) / 864e5);
      pf.innerHTML = '<small>Próximo feriado</small><b>' + esc(f0.nome) + '</b><span>' + esc(dataLonga(f0.data)) + '</span><em>' + (falta === 0 ? 'É hoje' : falta === 1 ? 'Amanhã' : 'Faltam ' + falta + ' dias') + '</em>';
    } else pf.innerHTML = '<small>Próximo feriado</small><b>—</b>';
  }

  /* ---------- Destaques ---------- */
  var dest = document.getElementById('ag-destaques');
  function cardEv(e, grande) {
    var d = parse(e.data), c = CATS[cat(e)], u = url(e.url);
    return '<article class="ag-card' + (grande ? ' g' : '') + (grande && e.imagem ? ' com-img' : '') + '" style="--c:' + c.cor + '">' +
      (grande && !e.imagem ? '<span class="ag-wm" aria-hidden="true">' + ('0' + d.getDate()).slice(-2) + '<small>' + MES3[d.getMonth()] + '</small></span>' : '') +
      (grande && e.imagem ? img(e, 'topo') : '') + (!grande ? img(e, 'mini') : '') +
      '<div class="ag-card-top"><span class="ag-chip-d"><b>' + d.getDate() + '</b>' + MES3[d.getMonth()] + '</span><span class="ag-tag">' + esc(c.nome) + '</span></div>' +
      '<h3>' + esc(e.titulo) + '</h3>' +
      (grande && e.descricao ? '<p>' + esc(e.descricao) + '</p>' : '') +
      '<ul class="ag-meta"><li>' + esc(periodo(e)) + ' · ' + esc(horaTxt(e.hora)) + '</li><li>' + esc(e.local) + '</li></ul>' +
      (u ? '<a class="ag-link" href="' + esc(u) + '" target="_blank" rel="noopener">Detalhes em ' + esc(e.fonte || 'fonte') + ' ↗</a>' : '') +
      '</article>';
  }
  if (dest) {
    dest.innerHTML = proximos.length
      ? proximos.filter(function (e) { return !e.secundario; }).slice(0, 4).map(function (e, i) { return cardEv(e, i === 0); }).join('')
      : '<p class="ag-vazio">Nenhum evento agendado no momento. Confira as fontes no fim da página.</p>';
  }

  /* ---------- Programação fixa ---------- */
  var fx = document.getElementById('ag-fixos');
  if (fx) {
    var F = D.fixos || [];
    fx.innerHTML = F.length ? '<span class="eyebrow">Toda semana</span>' + F.map(function (f) {
      var u = url(f.url);
      return '<div class="ag-fixo">' + (f.imagem ? img({ titulo: f.titulo, imagem: f.imagem, data: HOJE, categoria: 'comunidade' }, 'fixo') : '<div class="ag-fixo-i" aria-hidden="true"><svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><rect x="3" y="4" width="18" height="17" rx="3"/><path d="M8 2v4M16 2v4M3 10h18"/><path d="m9 15 2 2 4-4"/></svg></div>') +
        '<div class="ag-fixo-t"><h3>' + esc(f.titulo) + '</h3><p class="ag-fixo-q">' + esc(f.quando) + '</p><p>' + esc(f.local) + '</p><p class="ag-fixo-d">' + esc(f.descricao) + '</p></div>' +
        (u ? '<a class="btn btn-light" href="' + esc(u) + '" target="_blank" rel="noopener">Fonte: ' + esc(f.fonte) + ' <span class="arr">↗</span></a>' : '') + '</div>';
    }).join('') : '';
    if (!F.length) fx.closest('section').hidden = true;
  }

  /* ---------- Calendário ---------- */
  var grade = document.getElementById('ag-grade'), lista = document.getElementById('ag-lista'), corpo = document.getElementById('ag-corpo');
  if (!grade || !lista) return;
  var titulo = document.getElementById('ag-titulo'), ladoT = document.getElementById('ag-lado-t'), limpa = document.getElementById('ag-lado-limpa');

  // Meses disponíveis: do mês atual (ou do 1º dado) até dezembro de 2027 / último dado.
  var todas = EV.map(function (e) { return e.data; }).concat(FE.map(function (f) { return f.data; })).sort();
  var ini = todas.length ? todas[0].slice(0, 7) : HOJE.slice(0, 7);
  var fim = todas.length ? todas[todas.length - 1].slice(0, 7) : HOJE.slice(0, 7);
  if (fim < '2027-12') fim = '2027-12';
  function addMes(ym, n) { var y = +ym.slice(0, 4), m = +ym.slice(5, 7) - 1 + n; y += Math.floor(m / 12); m = ((m % 12) + 12) % 12; return y + '-' + ('0' + (m + 1)).slice(-2); }
  var mes = HOJE.slice(0, 7);
  if (mes < ini) mes = ini; if (mes > fim) mes = fim;
  var sel = null; // dia selecionado
  var filtro = 'todos', vis = 'cal';

  // Filtros
  var usados = { feriado: 1 }; EV.forEach(function (e) { usados[cat(e)] = 1; });
  var fl = document.getElementById('ag-filtros');
  if (fl) {
    fl.innerHTML = '<button type="button" class="on" data-f="todos">Tudo</button>' + Object.keys(CATS).filter(function (k) { return usados[k]; }).map(function (k) {
      return '<button type="button" data-f="' + k + '" style="--c:' + CATS[k].cor + '"><i></i>' + CATS[k].nome + '</button>';
    }).join('');
    fl.addEventListener('click', function (ev) {
      var b = ev.target.closest('button[data-f]'); if (!b) return;
      filtro = b.getAttribute('data-f');
      fl.querySelectorAll('button').forEach(function (x) { x.classList.toggle('on', x === b); });
      render();
    });
  }
  function passa(e) { return filtro === 'todos' || filtro === cat(e); }
  function mostraFer() { return filtro === 'todos' || filtro === 'feriado'; }

  function itemFer(f) {
    return '<div class="ag-it fe' + (f.tipo === 'facultativo' ? ' pf' : '') + '"><span class="ag-it-h">' + (f.tipo === 'facultativo' ? 'Ponto facultativo' : 'Feriado ' + (f.tipo === 'municipal' ? 'municipal' : 'nacional')) + '</span><b>' + esc(f.nome) + '</b></div>';
  }
  function itemEv(e, comImg) {
    var c = CATS[cat(e)], u = url(e.url);
    return '<div class="ag-it' + (comImg ? ' ci' : '') + '" style="--c:' + c.cor + '">' + (comImg ? img(e, 'lista') + '<div class="ag-it-t">' : '') + '<span class="ag-it-h">' + esc(horaTxt(e.hora)) + ' · ' + esc(c.nome) + (e.fim && e.fim !== e.data ? ' · ' + esc(periodo(e)) : '') + '</span>' +
      '<b>' + esc(e.titulo) + '</b><span class="ag-it-l">' + esc(e.local) + '</span>' +
      (e.descricao ? '<p>' + esc(e.descricao) + '</p>' : '') +
      (u ? '<a href="' + esc(u) + '" target="_blank" rel="noopener">' + esc(e.fonte || 'Ver fonte') + ' ↗</a>' : '') + (comImg ? '</div>' : '') + '</div>';
  }
  function blocoDia(d) {
    var fs = mostraFer() ? (ferDia[d] || []) : [], es = (porDia[d] || []).filter(passa);
    // Na lista, evento de vários dias aparece uma vez só: no 1º dia (ou hoje, se já começou).
    if (vis === 'lista') es = es.filter(function (e) { return e.data === d || (e.data < HOJE && d === HOJE); });
    if (!fs.length && !es.length) return '';
    var dd = parse(d);
    return '<div class="ag-dia' + (d === HOJE ? ' hoje' : '') + (d < HOJE ? ' passou' : '') + '"><div class="ag-dia-h"><b>' + dd.getDate() + ' ' + MES3[dd.getMonth()] + '</b><span>' + SEMANA[dd.getDay()] + (d === HOJE ? ' · hoje' : '') + '</span></div>' +
      fs.map(itemFer).join('') + es.map(function (e) { return itemEv(e, vis === 'lista'); }).join('') + '</div>';
  }

  function render() {
    var y = +mes.slice(0, 4), m = +mes.slice(5, 7) - 1;
    titulo.textContent = MESES[m].charAt(0).toUpperCase() + MESES[m].slice(1) + ' de ' + y;
    document.getElementById('ag-ant').disabled = mes <= ini;
    document.getElementById('ag-prox').disabled = mes >= fim;
    corpo.setAttribute('data-vis', vis);

    // Grade
    var p1 = new Date(y, m, 1), nd = new Date(y, m + 1, 0).getDate(), h = '';
    for (var i = 0; i < p1.getDay(); i++) h += '<span class="ag-c vazio"></span>';
    for (var dnum = 1; dnum <= nd; dnum++) {
      var d = iso(new Date(y, m, dnum)), fs = mostraFer() ? (ferDia[d] || []) : [], es = (porDia[d] || []).filter(passa);
      var cls = 'ag-c' + (d === HOJE ? ' hoje' : '') + (d === sel ? ' sel' : '') + (d < HOJE ? ' passou' : '') +
        (fs.some(function (f) { return f.tipo !== 'facultativo'; }) ? ' fer' : fs.length ? ' pfac' : '') + (es.length ? ' tem' : '');
      var marcas = es.slice(0, 3).map(function (e) { return '<i style="background:' + CATS[cat(e)].cor + '"></i>'; }).join('') + (es.length > 3 ? '<em>+' + (es.length - 3) + '</em>' : '');
      var rot = fs.length ? '<small class="ag-c-f">' + esc(fs[0].nome) + '</small>' : es.length ? '<small class="ag-c-e">' + esc(es[0].titulo) + '</small>' : '';
      var aria = dnum + ' de ' + MESES[m] + (fs.length ? ', ' + fs.map(function (f) { return f.nome; }).join(', ') : '') + (es.length ? ', ' + es.length + (es.length > 1 ? ' eventos' : ' evento') : '');
      h += '<button type="button" class="' + cls + '" data-d="' + d + '"' + (fs.length || es.length ? '' : ' tabindex="-1"') + ' aria-label="' + esc(aria) + '"><span class="ag-c-n">' + dnum + '</span>' + rot + '<span class="ag-c-m">' + marcas + '</span></button>';
    }
    grade.innerHTML = h;

    // Lista lateral / lista completa
    var html = '';
    if (vis === 'lista') {
      ladoT.textContent = 'Todos os eventos e feriados a partir de hoje';
      limpa.hidden = true;
      var dd = {}; Object.keys(porDia).concat(Object.keys(ferDia)).forEach(function (d) { if (d >= HOJE) dd[d] = 1; });
      var ds = Object.keys(dd).sort(), mesAtual = '';
      ds.forEach(function (d) {
        var b = blocoDia(d); if (!b) return;
        if (d.slice(0, 7) !== mesAtual) { mesAtual = d.slice(0, 7); html += '<h4 class="ag-tl-m">' + MESES[+mesAtual.slice(5) - 1] + ' de ' + mesAtual.slice(0, 4) + '</h4>'; }
        html += b;
      });
    } else if (sel && sel.slice(0, 7) === mes) {
      ladoT.textContent = dataLonga(sel).replace(/^./, function (c) { return c.toUpperCase(); });
      limpa.hidden = false;
      html = blocoDia(sel) || '<p class="ag-vazio">Nenhum evento ou feriado neste dia' + (filtro !== 'todos' ? ' com o filtro escolhido' : '') + '.</p>';
    } else {
      ladoT.textContent = 'Em ' + MESES[m] + ' de ' + y;
      limpa.hidden = true;
      for (var k = 1; k <= nd; k++) html += blocoDia(iso(new Date(y, m, k)));
    }
    lista.innerHTML = html || '<p class="ag-vazio">Nada marcado ' + (vis === 'lista' ? 'daqui para a frente' : 'neste mês') + (filtro !== 'todos' ? ' com o filtro escolhido' : '') + '.</p>';
  }

  document.getElementById('ag-ant').addEventListener('click', function () { mes = addMes(mes, -1); sel = null; render(); });
  document.getElementById('ag-prox').addEventListener('click', function () { mes = addMes(mes, 1); sel = null; render(); });
  document.getElementById('ag-hoje').addEventListener('click', function () { mes = HOJE.slice(0, 7); if (mes < ini) mes = ini; if (mes > fim) mes = fim; sel = HOJE.slice(0, 7) === mes ? HOJE : null; vis = 'cal'; sincVis(); render(); });
  limpa.addEventListener('click', function () { sel = null; render(); });
  grade.addEventListener('click', function (ev) {
    var b = ev.target.closest('.ag-c[data-d]'); if (!b) return;
    sel = sel === b.getAttribute('data-d') ? null : b.getAttribute('data-d');
    render();
    if (sel && window.matchMedia('(max-width: 900px)').matches) lista.scrollIntoView({ behavior: 'smooth', block: 'start' });
  });
  var visBtns = document.querySelectorAll('.ag-vis button');
  function sincVis() { visBtns.forEach(function (b) { b.setAttribute('aria-selected', b.getAttribute('data-vis') === vis ? 'true' : 'false'); }); }
  visBtns.forEach(function (b) { b.addEventListener('click', function () { vis = b.getAttribute('data-vis'); sincVis(); render(); }); });

  render();
})();
