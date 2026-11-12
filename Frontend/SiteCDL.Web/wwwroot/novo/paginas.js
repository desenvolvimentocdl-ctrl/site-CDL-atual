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

// Galeria de fotos dos espaços: agora aberta pelo visualizador geral (novo/visualizador.js)

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

// Tour virtual 360° dos espaços (Views/Shared/_Tour360.cshtml). O visualizador (Pannellum) só é baixado ao iniciar o tour.
(()=>{
  const box=document.querySelector('[data-t360]');if(!box)return;
  const stage=box.querySelector('.t360-stage'),view=box.querySelector('.t360-view'),
        capa=box.querySelector('.t360-cover img'),nome=box.querySelector('.t360-nome'),go=box.querySelector('.t360-go'),
        tabs=[...box.querySelectorAll('.t360-tab')];
  const CDN='https://cdnjs.cloudflare.com/ajax/libs/pannellum/2.5.6/';
  let viewer=null,lib=null,atual=tabs.find(t=>t.classList.contains('on'))||tabs[0];

  const carregarLib=()=>lib||(lib=new Promise((ok,erro)=>{
    const css=document.createElement('link');css.rel='stylesheet';css.href=CDN+'pannellum.css';document.head.appendChild(css);
    const s=document.createElement('script');s.src=CDN+'pannellum.js';s.onload=ok;s.onerror=erro;document.head.appendChild(s);
  }));

  const mostrar=t=>{
    atual=t;
    tabs.forEach(x=>{const on=x===t;x.classList.toggle('on',on);x.setAttribute('aria-selected',on)});
    capa.src=t.dataset.capa;nome.textContent=t.dataset.nome;
    stage.classList.toggle('sem',!t.dataset.pano);
    box.querySelector('.t360-erro')?.remove();
    if(viewer){viewer.destroy();viewer=null}
    stage.classList.remove('ativo');
    if(t.dataset.pano&&lib)iniciar(); // tour já em uso: troca direto para o novo espaço
  };

  const iniciar=async()=>{
    if(!atual.dataset.pano)return;
    go.disabled=true;
    try{await carregarLib()}catch(e){go.disabled=false;return}
    go.disabled=false;
    if(viewer){viewer.destroy();viewer=null}
    stage.classList.add('ativo');
    viewer=pannellum.viewer(view,{
      type:'equirectangular',panorama:atual.dataset.pano,title:atual.dataset.nome,
      autoLoad:true,autoRotate:-2,autoRotateInactivityDelay:4000,hfov:100,minHfov:40,maxHfov:120,
      showZoomCtrl:true,showFullscreenCtrl:true,compass:false,mouseZoom:'fullscreenonly',
      strings:{loadButtonLabel:'Clique para<br>carregar',loadingLabel:'Carregando…',bylineLabel:'',
        noPanoramaError:'Nenhuma imagem 360° foi informada.',fileAccessError:'Não foi possível abrir a imagem 360°.',
        malformedURLError:'Endereço da imagem inválido.',iOS8WebGLError:'Seu navegador não suporta o tour 360°.',
        genericWebGLError:'Seu navegador não suporta o tour 360° (WebGL).',
        textureSizeError:'A imagem 360° é grande demais para este aparelho. Tente em um computador.',
        unknownError:'Não foi possível carregar o tour 360°.'}
    });
  };

  go.addEventListener('click',iniciar);
  tabs.forEach(t=>t.addEventListener('click',()=>{if(t!==atual)mostrar(t)}));
  stage.classList.toggle('sem',!atual.dataset.pano);
})();
