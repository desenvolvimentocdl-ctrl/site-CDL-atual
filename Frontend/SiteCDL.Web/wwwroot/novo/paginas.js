// Páginas internas: destaca no sumário lateral a seção que está na tela
(()=>{
  const links=[...document.querySelectorAll('.toc a[href^="#"]')];if(!links.length)return;
  const map=new Map(links.map(a=>[a.getAttribute('href').slice(1),a]));
  const io=new IntersectionObserver(es=>es.forEach(e=>{
    if(!e.isIntersecting)return;
    links.forEach(a=>a.classList.remove('on'));map.get(e.target.id)?.classList.add('on');
  }),{rootMargin:'-30% 0px -60% 0px'});
  map.forEach((a,id)=>{const s=document.getElementById(id);if(s)io.observe(s)});
})();

// Galeria de fotos com visualizador (setas do teclado, Esc para fechar)
(()=>{
  const lb=document.getElementById('lb'),fotos=window.GALERIA;if(!lb||!fotos)return;
  const img=lb.querySelector('img'),cap=lb.querySelector('figcaption');let i=0,last=null;
  const show=n=>{i=(n+fotos.length)%fotos.length;img.src=fotos[i];cap.textContent=(i+1)+' de '+fotos.length};
  const open=n=>{last=document.activeElement;show(n);lb.hidden=false;document.body.classList.add('lock');lb.querySelector('.lb-x').focus()};
  const close=()=>{lb.hidden=true;document.body.classList.remove('lock');last&&last.focus()};
  document.querySelectorAll('[data-gal] [data-i]').forEach(b=>b.addEventListener('click',()=>open(+b.dataset.i)));
  lb.addEventListener('click',e=>{const a=e.target.closest('[data-lb]');if(a)a.dataset.lb==='x'?close():show(i+ +a.dataset.lb);else if(e.target===lb)close()});
  document.addEventListener('keydown',e=>{if(lb.hidden)return;if(e.key==='Escape')close();if(e.key==='ArrowRight')show(i+1);if(e.key==='ArrowLeft')show(i-1)});
})();

// Formulários do protótipo: não enviam, só mostram o aviso
document.querySelectorAll('form[data-proto]').forEach(f=>f.addEventListener('submit',e=>{e.preventDefault();const ok=f.querySelector('.f-ok');if(ok)ok.hidden=false}));

// Lista de publicações: filtra os cards da página pelo texto digitado
(()=>{
  const q=document.querySelector('[data-filtro]');if(!q)return;
  const cards=[...document.querySelectorAll('[data-busca]')],nores=document.querySelector('.nores');
  const norm=t=>t.normalize('NFD').replace(/[̀-ͯ]/g,'').toLowerCase();
  q.addEventListener('input',()=>{const v=norm(q.value.trim());let n=0;
    cards.forEach(c=>{const ok=!v||norm(c.dataset.busca).includes(v);c.hidden=!ok;if(ok)n++});
    if(nores)nores.hidden=n>0});
})();

// Matéria: copiar o link da página
document.querySelectorAll('[data-copiar]').forEach(b=>b.addEventListener('click',async()=>{
  try{await navigator.clipboard.writeText(location.href)}catch(e){}
  b.classList.add('ok');setTimeout(()=>b.classList.remove('ok'),1800);
}));

// Loja: filtro por categoria (chips), segmento e busca
(()=>{
  const lista=document.querySelector('[data-lista]');if(!lista)return;
  const itens=[...lista.children],q=document.querySelector('[data-filtro-loja]'),seg=document.querySelector('[data-seg]'),nores=document.querySelector('.nores');
  const norm=t=>(t||'').normalize('NFD').replace(/[̀-ͯ]/g,'').toLowerCase();
  let cat='';
  const aplicar=()=>{const v=norm(q&&q.value.trim()),s=norm(seg&&seg.value.replace(/\s*\(\d+\)$/,''));let n=0;
    itens.forEach(c=>{const ok=(!cat||c.dataset.cat===cat)&&(!v||norm(c.dataset.busca).includes(v))&&(!s||norm(c.dataset.busca).includes(s));c.hidden=!ok;if(ok)n++});
    if(nores)nores.hidden=n>0};
  document.querySelectorAll('[data-chips] .chip').forEach(b=>b.addEventListener('click',()=>{document.querySelectorAll('[data-chips] .chip').forEach(x=>x.classList.toggle('on',x===b));cat=b.dataset.cat;aplicar()}));
  q&&q.addEventListener('input',aplicar);seg&&seg.addEventListener('change',aplicar);
})();
