(()=>{
  const reduce = matchMedia('(prefers-reduced-motion: reduce)').matches;

  // header on scroll (passive + rAF)
  const hdr=document.getElementById('hdr');let tick=false;
  addEventListener('scroll',()=>{if(tick)return;tick=true;requestAnimationFrame(()=>{hdr.classList.toggle('scrolled',scrollY>12);tick=false})},{passive:true});

  // hero headline word stagger
  const hero=document.getElementById('hero');
  hero.querySelectorAll('h1 .w').forEach((w,i)=>w.style.transitionDelay=(i*55)+'ms');
  requestAnimationFrame(()=>requestAnimationFrame(()=>hero.classList.add('go','in')));

  // count-up
  const fmt=(v,dec,sep)=>{let s=v.toFixed(dec).replace('.',',');if(sep)s=s.replace(/\B(?=(\d{3})+(?!\d))/g,'.');return s};
  const count=el=>{
    if(el.dataset.done)return;el.dataset.done=1;
    const to=parseFloat(el.dataset.count),dec=+(el.dataset.dec||0),sep=el.dataset.sep;
    if(reduce){el.textContent=fmt(to,dec,sep);return}
    const t0=performance.now(),dur=1400;
    const step=t=>{const p=Math.min((t-t0)/dur,1),e=1-Math.pow(1-p,3);el.textContent=fmt(to*e,dec,sep);if(p<1)requestAnimationFrame(step)};
    requestAnimationFrame(step);
  };

  // one observer for everything
  const io=new IntersectionObserver(es=>es.forEach(e=>{
    if(!e.isIntersecting)return;const el=e.target;el.classList.add('in');
    el.querySelectorAll('[data-count]').forEach(count);
    if(el.matches('[data-count]'))count(el);
    el.querySelectorAll('.fill').forEach(f=>f.style.width=f.dataset.w+'%');
    el.querySelectorAll('.ring .fg').forEach(c=>c.style.strokeDashoffset=314-314*c.dataset.p/100);
    io.unobserve(el);
  // threshold 0: revela assim que o topo entra na tela. Com threshold fracionado (ex.: .18), um bloco
  // mais alto que ~5x a tela (matéria longa no celular) nunca atinge a fração e ficava invisível.
  }),{threshold:0,rootMargin:'0px 0px -60px 0px'});
  document.querySelectorAll('.rv,.gridlines,.sec,#tl').forEach(el=>io.observe(el));

})();

// ---------- carrossel de campanhas (scroll-snap nativo, sem autoplay) ----------
(()=>{
  const sec=document.getElementById('campanhas');if(!sec)return;
  const dotsBox=sec.querySelector('.camp-dots');
  const active=()=>[...sec.querySelectorAll('.camp-track')].find(t=>getComputedStyle(t).display!=='none');
  let track=active();
  const build=()=>{
    track=active();const n=track.children.length;
    dotsBox.innerHTML='<i></i>'.repeat(n);update();
  };
  const update=()=>{
    const i=Math.round(track.scrollLeft/(track.children[0].offsetWidth+14));
    dotsBox.querySelectorAll('i').forEach((d,k)=>d.classList.toggle('on',k===i));
  };
  sec.querySelectorAll('.camp-track').forEach(t=>t.addEventListener('scroll',()=>requestAnimationFrame(update),{passive:true}));
  sec.querySelectorAll('.cbtn').forEach(b=>b.addEventListener('click',()=>{
    const w=track.children[0].offsetWidth+14;track.scrollBy({left:w*+b.dataset.dir});
  }));
  build();addEventListener('resize',build,{passive:true});
})();


// ---------- menu: mega (clique/teclado), drawer mobile e busca ----------
(()=>{
  const hasEls=[...document.querySelectorAll('nav .has')];
  const closeAll=x=>hasEls.forEach(h=>{if(h!==x){h.classList.remove('open');h.querySelector('.nb').setAttribute('aria-expanded','false')}});
  hasEls.forEach(h=>{
    const b=h.querySelector('.nb');
    b.addEventListener('click',()=>{const o=!h.classList.contains('open');closeAll(h);h.classList.toggle('open',o);b.setAttribute('aria-expanded',o)});
    h.addEventListener('mouseleave',()=>{h.classList.remove('open');b.setAttribute('aria-expanded','false')});
  });
  document.addEventListener('click',e=>{if(!e.target.closest('nav'))closeAll()});

  const dr=document.getElementById('drawer'),burger=document.querySelector('.burger');
  const openDr=o=>{dr.classList.toggle('on',o);dr.setAttribute('aria-hidden',!o);burger.setAttribute('aria-expanded',o);document.body.classList.toggle('lock',o)};
  burger.addEventListener('click',()=>openDr(true));
  dr.querySelectorAll('[data-close]').forEach(x=>x.addEventListener('click',()=>openDr(false)));
  dr.querySelectorAll('.drawer-scroll a').forEach(a=>a.addEventListener('click',()=>openDr(false)));
  dr.querySelectorAll('.dsec').forEach(d=>d.addEventListener('toggle',()=>{if(d.open)dr.querySelectorAll('.dsec').forEach(o=>{if(o!==d)o.open=false})}));

  const DATA=[["Página Inicial", "/loja", "Loja CDL"], ["Eventos", "/loja/eventos", "Loja CDL"], ["Cursos", "/loja/cursos", "Loja CDL"], ["Cursos EAD", "/loja/cursos-ead", "Loja CDL"], ["CDL IA", "https://cdlia.com.br/", "Loja CDL"], ["CDL MenorPreço", "/loja/menor-preco", "Loja CDL"], ["Descontos", "/loja/descontos", "Loja CDL"], ["Parceiros", "/loja/parceiros", "Loja CDL"], ["Quem somos", "/quem-somos", "Institucional"], ["Nossa História", "/nossa-historia", "Institucional"], ["Corporativo", "/corporativo", "Institucional"], ["Reconhecimento", "/reconhecimento", "Institucional"], ["Fundação CDL", "/fundacao-cdl", "Institucional"], ["Política de Sustentabilidade", "/politica-de-sustentabilidade", "Institucional"], ["Responsabilidade Ambiental", "/responsabilidade-ambiental", "Institucional › Política de Sustentabilidade"], ["Responsabilidade Social", "/responsabilidade-social", "Institucional › Política de Sustentabilidade"], ["Governança", "/governanca", "Institucional › Política de Sustentabilidade"], ["Representatividade", "/representatividade", "Institucional › Política de Sustentabilidade"], ["Diretoria CDL", "/diretoria", "Institucional"], ["Diretoria Fundação", "/diretoria-fundacao", "Institucional"], ["Área LGPD", "/lgpd", "Institucional"], ["Publicações", "/publicacoes", "Menu"], ["Notícias", "/publicacoes/noticias", "Publicações"], ["Opinião", "/publicacoes/cdldireto", "Publicações"], ["Comunicados", "/publicacoes/comunicados", "Publicações"], ["CDL em Ação", "/publicacoes/cdlacao", "Publicações"], ["Pesquisa", "/publicacoes/pesquisas", "Publicações"], ["CAE", "/publicacoes/cae", "Publicações"], ["Associe-se", "/associe-se", "Para você"], ["Candidate-se às nossas vagas", "https://jobs.quickin.io/cdludi/jobs?page=1", "Para você"], ["Certificado Digital", "/certificado-digital", "Para você"], ["CDL Saúde", "/cdl-saude", "Para você › Convênio e Benefícios"], ["CDL Convenções e Eventos", "/cdl-convencoes-e-eventos", "Para você"], ["CDL Auditório", "/cdl-auditorio", "Para você › CDL Convenções e Eventos"], ["CDL Foyer", "/cdl-foyer", "Para você › CDL Convenções e Eventos"], ["Sala Espaço Mais", "/sala-espaco-mais", "Para você › CDL Convenções e Eventos"], ["Sala Executiva", "/sala-executiva", "Para você › CDL Convenções e Eventos"], ["Sala Cecília Meireles", "/sala-cecilia-meireles", "Para você › CDL Convenções e Eventos"], ["Sala Machado de Assis", "/sala-machado-de-assis", "Para você › CDL Convenções e Eventos"], ["Rede de Descontos", "/loja/descontos", "Para você"], ["SPC Brasil", "/spc-brasil", "Para você › SPC"], ["SPC Consulta CPF", "/spc-consulta", "Para você › SPC"], ["SPC Negociação de Débito", "/spc-negociacao", "Para você › SPC"], ["SPC Avisa", "/spc-avisa", "Para você › SPC"], ["SPC Dados", "/spc-dados", "Para você › SPC"], ["SPC Consultas e Registro", "/spc-consulta-registro", "Para você › SPC"], ["Alerta de Documentos", "/alerta-documento", "Para você › SPC"], ["Cadastro Positivo", "/cadastro-positivo", "Para você › SPC"], ["Cursos - Programação Mensal", "/loja/cursos", "Para você › Treinamentos"], ["Treinamento in company", "/treinamento", "Para você › Treinamentos"], ["Associe-se", "/associe-se", "Para empresa"], ["CDL Mídia", "/cdl-midia", "Para empresa › Captação de Novos Clientes"], ["SPC Dados", "/spc-dados", "Para empresa › Captação de Novos Clientes"], ["CDL Convenções e Eventos", "/cdl-convencoes-e-eventos", "Para empresa"], ["CDL Auditório", "/cdl-auditorio", "Para empresa › CDL Convenções e Eventos"], ["CDL Foyer", "/cdl-foyer", "Para empresa › CDL Convenções e Eventos"], ["Sala Espaço Mais", "/sala-espaco-mais", "Para empresa › CDL Convenções e Eventos"], ["Sala Executiva", "/sala-executiva", "Para empresa › CDL Convenções e Eventos"], ["Sala Cecília Meireles", "/sala-cecilia-meireles", "Para empresa › CDL Convenções e Eventos"], ["Sala Machado de Assis", "/sala-machado-de-assis", "Para empresa › CDL Convenções e Eventos"], ["Consultoria", "https://cdluberlandia.cdludi.org.br/mentoria-de-negocios-fdc4b17686d8495e6609", "Para empresa"], ["CDL Saúde", "/cdl-saude", "Para empresa › Convênio e Benefícios"], ["Rita Saúde", "https://cdluberlandia.cdludi.org.br/rita-saude", "Para empresa › Convênio e Benefícios"], ["Cursos - Programação Mensal", "/loja/cursos", "Para empresa › Educação Corporativa"], ["Treinamento in company", "/treinamento", "Para empresa › Educação Corporativa"], ["Recrutamento e Seleção", "/cdl-talentos", "Para empresa › Gestão de Pessoas"], ["CDL Estágio", "/cdl-estagio", "Para empresa › Gestão de Pessoas"], ["CDL Aprendiz", "/cdl-aprendiz", "Para empresa › Gestão de Pessoas"], ["CDL Gestão e NF-e", "/cdl-gestao", "Para empresa › Gestão Operacional e Financeira"], ["SPC Consultas e Registro", "/spc-consulta-registro", "Para empresa › Gestão Operacional e Financeira"], ["Parceria BDMG", "/bdmg", "Para empresa › Gestão Operacional e Financeira"], ["Legislação", "/legislacao", "Para empresa"], ["Licitações", "/licitacoes", "Para empresa"], ["Selo Empresa Cidadã", "https://empresacidada.cdludi.org.br/", "Para empresa › Responsabilidade Social"], ["Segunda Via de Boleto", "https://www.itau.com.br/servicos/boletos", "Para empresa"], ["Certificado Digital", "/certificado-digital", "Para empresa › Segurança dos Negócios"], ["SPC Avisa", "/spc-avisa", "Para empresa › Segurança dos Negócios"], ["Cadastro Positivo", "/cadastro-positivo", "Para empresa › Segurança dos Negócios"], ["Combo de Soluções", "/combo-solucoes", "Para empresa › Vantagens para Sua Empresa"], ["Vantagens para Associados", "/vantagens-associado", "Para empresa › Vantagens para Sua Empresa"], ["Jornada Empresarial", "/jornada-empresarial", "Para empresa › Vantagens para Sua Empresa"], ["Rede de Descontos", "/loja/descontos", "Para empresa › Vantagens para Sua Empresa"], ["Painel do Associado", "/painel-associado", "Menu"], ["Área do associado (login)", "/painel-associado/entrar", "Menu"], ["Acesse o SPC", "https://sistema.spc.org.br/spc/controleacesso/autenticacao/entry.action", "Menu"]];
  const sov=document.getElementById('sov'),q=document.getElementById('sq'),res=document.getElementById('sres');
  const norm=t=>t.normalize('NFD').replace(/[\u0300-\u036f]/g,'').toLowerCase();
  const seen=new Set();const items=DATA.filter(([t,u])=>{const k=t+u;if(seen.has(k))return false;seen.add(k);return true});
  let act=0,cur=[];
  const esc=t=>t.replace(/[&<>"]/g,c=>({'&':'&amp;','<':'&lt;','>':'&gt;','"':'&quot;'}[c]));
  const hl=(t,v)=>{if(!v)return esc(t);const i=norm(t).indexOf(v);return i<0?esc(t):esc(t.slice(0,i))+'<mark>'+esc(t.slice(i,i+v.length))+'</mark>'+esc(t.slice(i+v.length))};
  const render=()=>{
    const v=norm(q.value.trim());
    cur=v?items.filter(([t,,p])=>norm(t+' '+p).includes(v)):items;
    act=0;
    res.innerHTML=cur.length?cur.map(([t,u,p],i)=>`<a class="sr${i===0?' act':''}" href="${u}"${(!/^https?:/.test(u)||u.startsWith('https://cdludi.org.br'))?'':' target="_blank" rel="noopener"'}><b>${hl(t,v)}</b><span>${esc(p)}</span></a>`).join(''):'<div class="sempty">Nada encontrado. Tente “SPC”, “curso” ou “certificado”.</div>';
  };
  const openS=o=>{sov.classList.toggle('on',o);sov.setAttribute('aria-hidden',!o);document.body.classList.toggle('lock',o||dr.classList.contains('on'));if(o){q.value='';render();setTimeout(()=>q.focus(),50)}};
  document.querySelectorAll('[data-search]').forEach(b=>b.addEventListener('click',()=>{openDr(false);openS(true)}));
  sov.querySelectorAll('[data-sclose]').forEach(b=>b.addEventListener('click',()=>openS(false)));
  q.addEventListener('input',render);
  q.addEventListener('keydown',e=>{
    const els=res.querySelectorAll('.sr');if(!els.length)return;
    if(e.key==='ArrowDown'||e.key==='ArrowUp'){e.preventDefault();els[act].classList.remove('act');act=(act+(e.key==='ArrowDown'?1:-1)+els.length)%els.length;els[act].classList.add('act');els[act].scrollIntoView({block:'nearest'})}
    if(e.key==='Enter'){e.preventDefault();els[act].click()}
  });
  document.addEventListener('keydown',e=>{
    if(e.key==='Escape'){openS(false);openDr(false);closeAll()}
    if((e.ctrlKey||e.metaKey)&&e.key.toLowerCase()==='k'){e.preventDefault();openS(true)}
  });
})();

// ---------- aviso de cookies (LGPD) ----------
(()=>{
  const el=document.getElementById('ck');if(!el)return;
  const KEY='cdl-cookies';let v=null;
  try{v=localStorage.getItem(KEY)}catch(e){}
  const show=()=>{el.hidden=false;requestAnimationFrame(()=>el.classList.add('on'))};
  const hide=()=>{el.classList.remove('on');setTimeout(()=>el.hidden=true,300)};
  if(v===null)setTimeout(show,900);
  el.querySelectorAll('[data-ck]').forEach(b=>b.addEventListener('click',()=>{try{localStorage.setItem(KEY,b.dataset.ck)}catch(e){}hide()}));
  document.querySelectorAll('[data-cookie-open]').forEach(b=>b.addEventListener('click',show));
})();
