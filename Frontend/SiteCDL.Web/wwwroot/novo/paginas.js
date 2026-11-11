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

  // Campo vazio + foco: mostra as 10 publicações mais lidas (GET /publicacoes/mais-lidas)
  const box=q.closest('.lsearch');if(!box)return;
  const pn=document.createElement('div');pn.className='mlidas';pn.id='mlidas';pn.hidden=true;pn.setAttribute('role','listbox');
  pn.innerHTML='<p class="mlidas-t">Mais lidas</p><ol></ol>';box.appendChild(pn);
  q.setAttribute('aria-controls','mlidas');q.setAttribute('aria-expanded','false');
  const ol=pn.querySelector('ol');let carregado=null;
  const esc=t=>String(t??'').replace(/[&<>"']/g,c=>({'&':'&amp;','<':'&lt;','>':'&gt;','"':'&quot;',"'":'&#39;'}[c]));
  const carregar=()=>carregado||(carregado=fetch('/publicacoes/mais-lidas',{headers:{Accept:'application/json'}})
    .then(r=>r.ok?r.json():[]).catch(()=>[]).then(l=>{
      ol.innerHTML=l.length?l.map((i,k)=>`<li><a href="${esc(i.url)}" role="option"><b>${k+1}</b><span>${esc(i.titulo)}${i.data?`<time>${esc(i.data)}</time>`:''}</span></a></li>`).join('')
        :'<li class="mlidas-vazio">Nenhuma publicação ainda.</li>';}));
  const abrir=()=>{if(q.value.trim())return fechar();carregar().then(()=>{if(document.activeElement===q||pn.contains(document.activeElement)){pn.hidden=false;q.setAttribute('aria-expanded','true')}})};
  const fechar=()=>{pn.hidden=true;q.setAttribute('aria-expanded','false')};
  q.addEventListener('focus',abrir);q.addEventListener('click',abrir);
  q.addEventListener('input',()=>q.value.trim()?fechar():abrir());
  q.addEventListener('keydown',e=>{if(e.key==='Escape'){fechar();}
    if(e.key==='ArrowDown'&&!pn.hidden){const a=ol.querySelector('a');if(a){e.preventDefault();a.focus()}}});
  pn.addEventListener('keydown',e=>{const as=[...ol.querySelectorAll('a')],i=as.indexOf(document.activeElement);
    if(e.key==='ArrowDown'&&i<as.length-1){e.preventDefault();as[i+1].focus()}
    if(e.key==='ArrowUp'){e.preventDefault();(i>0?as[i-1]:q).focus()}
    if(e.key==='Escape'){fechar();q.focus()}});
  document.addEventListener('pointerdown',e=>{if(!box.contains(e.target))fechar()});
  box.addEventListener('focusout',()=>setTimeout(()=>{if(!box.contains(document.activeElement))fechar()},0));
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
