// Visualizador de imagens do site: clique em qualquer imagem de conteúdo para ampliar, dar zoom e baixar.
// - Vale para todas as imagens dentro de <main>, exceto as que são links/botões (cards), banners de campanha, logos, ícones e o tour 360°.
// - Galerias dos espaços ([data-gal] + window.GALERIA) abrem aqui com todas as fotos do espaço.
// - Zoom: botões + / −, rolagem do mouse, duplo clique, pinça no celular; arraste para mover a imagem ampliada.
// - Teclado: ← → navegam, + − 0 controlam o zoom, Esc fecha.
// - Para tirar uma imagem do visualizador, coloque data-no-zoom nela (ou num elemento pai).
(()=>{
  const FORA = 'a,button,header,footer,nav,.camp,.t360,.pnlm-container,.zv,.lb,[data-no-zoom],.logo,.spc-float';
  const MAX = 5;

  // ---------- quais imagens podem ser ampliadas ----------
  const elegivel = img => {
    if (!img || img.tagName !== 'IMG' || !img.closest('main') || img.closest(FORA)) return false;
    const r = img.getBoundingClientRect();
    return r.width >= 120 && r.height >= 80 && (img.naturalWidth || 0) >= 160;
  };
  // versão grande da imagem (miniaturas "-p" do visual novo e "-300x200" do WordPress)
  const grande = src => {
    if (/\/novo\/img\//.test(src)) return src.replace(/-p\.(webp|jpe?g|png)(\?|$)/i, '.$1$2');
    if (/\/wp-content\/uploads\//.test(src)) return src.replace(/-\d{2,4}x\d{2,4}\.(jpe?g|png|webp)(\?|$)/i, '.$1$2');
    return src;
  };
  const deImg = img => { const s = img.currentSrc || img.src; return { src: grande(s), fb: s, alt: img.alt || '' }; };

  // ---------- montagem do visualizador ----------
  const ic = d => `<svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" aria-hidden="true">${d}</svg>`;
  const zv = document.createElement('div');
  zv.className = 'zv'; zv.hidden = true;
  zv.setAttribute('role', 'dialog'); zv.setAttribute('aria-modal', 'true'); zv.setAttribute('aria-label', 'Visualizador de imagens');
  zv.innerHTML = `
    <div class="zv-bar">
      <span class="zv-cont" aria-live="polite"></span>
      <div class="zv-acts">
        <button type="button" data-zv="menos" aria-label="Diminuir zoom" title="Diminuir zoom (−)">${ic('<circle cx="11" cy="11" r="7"/><path d="m20 20-3.5-3.5M8 11h6"/>')}</button>
        <span class="zv-pct">100%</span>
        <button type="button" data-zv="mais" aria-label="Aumentar zoom" title="Aumentar zoom (+)">${ic('<circle cx="11" cy="11" r="7"/><path d="m20 20-3.5-3.5M8 11h6M11 8v6"/>')}</button>
        <button type="button" data-zv="ajustar" aria-label="Ajustar à tela" title="Ajustar à tela (0)">${ic('<path d="M4 9V4h5M20 9V4h-5M4 15v5h5M20 15v5h-5"/>')}</button>
        <i class="zv-sep"></i>
        <button type="button" data-zv="baixar" class="zv-dl" aria-label="Baixar imagem" title="Baixar imagem">${ic('<path d="M12 4v11M7 10l5 5 5-5M5 20h14"/>')}<span>Baixar</span></button>
        <button type="button" data-zv="fechar" aria-label="Fechar" title="Fechar (Esc)">${ic('<path d="M6 6l12 12M18 6 6 18"/>')}</button>
      </div>
    </div>
    <div class="zv-stage"><img class="zv-img" alt="" draggable="false"><span class="zv-load" aria-hidden="true"></span></div>
    <button type="button" class="zv-nav zv-ant" data-zv="ant" aria-label="Imagem anterior">${ic('<path d="M15 18 9 12l6-6"/>')}</button>
    <button type="button" class="zv-nav zv-prox" data-zv="prox" aria-label="Próxima imagem">${ic('<path d="m9 18 6-6-6-6"/>')}</button>
    <p class="zv-cap"></p>`;
  document.body.appendChild(zv);

  const stage = zv.querySelector('.zv-stage'), im = zv.querySelector('.zv-img'),
        cont = zv.querySelector('.zv-cont'), cap = zv.querySelector('.zv-cap'), pct = zv.querySelector('.zv-pct');
  let itens = [], i = 0, s = 1, x = 0, y = 0, ultimoFoco = null;

  // ---------- zoom e movimento ----------
  const limitar = () => {
    const mx = Math.max(0, (im.offsetWidth * s - stage.clientWidth) / 2),
          my = Math.max(0, (im.offsetHeight * s - stage.clientHeight) / 2);
    x = Math.min(mx, Math.max(-mx, x)); y = Math.min(my, Math.max(-my, y));
  };
  const aplicar = (suave = true) => {
    if (s <= 1) { s = 1; x = 0; y = 0; } else limitar();
    im.style.transition = suave ? '' : 'none';
    im.style.transform = `translate(${x}px,${y}px) scale(${s})`;
    pct.textContent = Math.round(s * 100) + '%';
    zv.classList.toggle('ampliada', s > 1);
  };
  // zoom mantendo fixo o ponto (px,py) — coordenadas da tela
  const zoomEm = (ns, px, py, suave = true) => {
    ns = Math.min(MAX, Math.max(1, ns));
    const r = stage.getBoundingClientRect();
    const cx = (px ?? r.left + r.width / 2) - (r.left + r.width / 2),
          cy = (py ?? r.top + r.height / 2) - (r.top + r.height / 2);
    x = cx - (cx - x) * (ns / s); y = cy - (cy - y) * (ns / s); s = ns;
    aplicar(suave);
  };

  // ---------- abrir / navegar / fechar ----------
  const mostrar = n => {
    if (!itens.length) return;
    i = (n + itens.length) % itens.length;
    const it = itens[i];
    s = 1; x = 0; y = 0; aplicar(false);
    zv.classList.add('carregando');
    im.onload = () => zv.classList.remove('carregando');
    im.onerror = () => { if (it.fb && im.src !== new URL(it.fb, location.href).href) im.src = it.fb; else zv.classList.remove('carregando'); };
    im.src = it.src; im.alt = it.alt;
    cont.textContent = itens.length > 1 ? `${i + 1} de ${itens.length}` : '';
    cap.textContent = it.alt; cap.hidden = !it.alt;
    zv.classList.toggle('unica', itens.length < 2);
    [i + 1, i - 1].forEach(k => { const p = itens[(k + itens.length) % itens.length]; if (p) new Image().src = p.src; });
  };
  const abrir = (lista, n) => {
    itens = lista; ultimoFoco = document.activeElement;
    zv.hidden = false; document.documentElement.classList.add('zv-aberto');
    mostrar(n);
    zv.querySelector('[data-zv="fechar"]').focus();
  };
  const fechar = () => {
    zv.hidden = true; document.documentElement.classList.remove('zv-aberto');
    im.removeAttribute('src');
    ultimoFoco && ultimoFoco.focus && ultimoFoco.focus();
  };

  // ---------- baixar ----------
  const nomeArquivo = (url, alt) => {
    const base = (alt || 'imagem-cdl-uberlandia').normalize('NFD').replace(/[̀-ͯ]/g, '')
      .toLowerCase().replace(/[^a-z0-9]+/g, '-').replace(/^-|-$/g, '').slice(0, 60) || 'imagem';
    if (url.startsWith('data:')) return base + '.' + ((url.match(/^data:image\/(\w+)/) || [])[1] || 'jpg').replace('jpeg', 'jpg');
    let n = decodeURIComponent(new URL(url, location.href).pathname.split('/').pop() || '');
    if (!/\.(jpe?g|png|webp|gif|avif)$/i.test(n)) n = base + '.jpg';
    return n;
  };
  const baixar = async () => {
    const url = im.currentSrc || im.src; if (!url) return;
    const nome = nomeArquivo(url, itens[i]?.alt);
    try {
      let href = url, temp = false;
      if (!url.startsWith('data:') && new URL(url, location.href).origin !== location.origin) {
        const r = await fetch(url, { mode: 'cors' }); if (!r.ok) throw 0;
        href = URL.createObjectURL(await r.blob()); temp = true;
      }
      const a = document.createElement('a'); a.href = href; a.download = nome;
      document.body.appendChild(a); a.click(); a.remove();
      if (temp) setTimeout(() => URL.revokeObjectURL(href), 5000);
    } catch (e) { window.open(url, '_blank', 'noopener'); } // outro site sem permissão: abre a imagem para salvar
  };

  // ---------- eventos ----------
  document.addEventListener('click', e => {
    const g = e.target.closest('[data-gal] [data-i]');
    if (g && window.GALERIA) {
      e.preventDefault();
      const fotos = window.GALERIA.map(f => /^(\/|https?:|data:)/.test(f) ? f : '/novo/' + f);
      const titulo = document.querySelector('h1')?.textContent.trim() || '';
      abrir(fotos.map((f, k) => ({ src: f, fb: f, alt: titulo ? `${titulo} — foto ${k + 1}` : '' })), +g.dataset.i);
      return;
    }
    if (!elegivel(e.target)) return;
    const todas = [...document.querySelectorAll('main img')].filter(elegivel);
    abrir(todas.map(deImg), Math.max(0, todas.indexOf(e.target)));
  });
  // cursor de lupa nas imagens que ampliam
  document.addEventListener('mouseover', e => { if (e.target.tagName === 'IMG' && elegivel(e.target)) e.target.classList.add('zv-ok'); });

  zv.addEventListener('click', e => {
    const b = e.target.closest('[data-zv]');
    if (!b) { if (e.target === stage && s === 1) fechar(); return; }
    ({ menos: () => zoomEm(s / 1.5), mais: () => zoomEm(s * 1.5), ajustar: () => { s = 1; aplicar(); },
       baixar, fechar, ant: () => mostrar(i - 1), prox: () => mostrar(i + 1) })[b.dataset.zv]?.();
  });
  document.addEventListener('keydown', e => {
    if (zv.hidden) return;
    const k = e.key;
    if (k === 'Escape') fechar();
    else if (k === 'ArrowRight') mostrar(i + 1);
    else if (k === 'ArrowLeft') mostrar(i - 1);
    else if (k === '+' || k === '=') zoomEm(s * 1.5);
    else if (k === '-') zoomEm(s / 1.5);
    else if (k === '0') { s = 1; aplicar(); }
    else if (k === 'Tab') { // mantém o foco dentro do visualizador
      const f = [...zv.querySelectorAll('button')].filter(b => b.offsetParent);
      if (e.shiftKey && document.activeElement === f[0]) { e.preventDefault(); f[f.length - 1].focus(); }
      else if (!e.shiftKey && document.activeElement === f[f.length - 1]) { e.preventDefault(); f[0].focus(); }
      return;
    } else return;
    e.preventDefault();
  });
  stage.addEventListener('wheel', e => { e.preventDefault(); zoomEm(s * Math.exp(-e.deltaY * 0.0018), e.clientX, e.clientY, false); }, { passive: false });
  let tipoPonteiro = 'mouse';
  stage.addEventListener('dblclick', e => { if (tipoPonteiro !== 'touch') zoomEm(s > 1 ? 1 : 2.5, e.clientX, e.clientY); });

  // arrastar, pinça e deslizar (mouse e toque)
  const toques = new Map(); let ini = null, pinca = null, ultimoToque = 0;
  stage.addEventListener('pointerdown', e => {
    tipoPonteiro = e.pointerType;
    stage.setPointerCapture(e.pointerId);
    toques.set(e.pointerId, { x: e.clientX, y: e.clientY });
    if (toques.size === 1) ini = { px: e.clientX, py: e.clientY, x, y, t: Date.now() };
    if (toques.size === 2) {
      const [a, b] = [...toques.values()];
      pinca = { d: Math.hypot(a.x - b.x, a.y - b.y), s };
    }
  });
  stage.addEventListener('pointermove', e => {
    if (!toques.has(e.pointerId)) return;
    toques.set(e.pointerId, { x: e.clientX, y: e.clientY });
    if (toques.size === 2 && pinca) {
      const [a, b] = [...toques.values()];
      zoomEm(pinca.s * Math.hypot(a.x - b.x, a.y - b.y) / pinca.d, (a.x + b.x) / 2, (a.y + b.y) / 2, false);
    } else if (toques.size === 1 && ini && s > 1) {
      x = ini.x + e.clientX - ini.px; y = ini.y + e.clientY - ini.py; aplicar(false);
    }
  });
  const soltar = e => {
    if (!toques.has(e.pointerId)) return;
    toques.delete(e.pointerId);
    if (toques.size < 2) pinca = null;
    if (toques.size === 0 && ini) {
      const dx = e.clientX - ini.px, dy = e.clientY - ini.py, rapido = Date.now() - ini.t < 500;
      if (s === 1 && rapido && Math.abs(dx) > 60 && Math.abs(dx) > Math.abs(dy) * 1.5 && itens.length > 1) mostrar(i + (dx < 0 ? 1 : -1));
      else if (e.pointerType === 'touch' && Math.abs(dx) < 10 && Math.abs(dy) < 10) { // toque duplo = zoom
        if (Date.now() - ultimoToque < 300) { zoomEm(s > 1 ? 1 : 2.5, e.clientX, e.clientY); ultimoToque = 0; }
        else ultimoToque = Date.now();
      }
      ini = null;
    }
  };
  stage.addEventListener('pointerup', soltar);
  stage.addEventListener('pointercancel', soltar);
  window.addEventListener('resize', () => { if (!zv.hidden) aplicar(false); });
})();
