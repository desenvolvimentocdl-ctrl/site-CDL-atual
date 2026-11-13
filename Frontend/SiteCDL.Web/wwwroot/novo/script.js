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

  const DATA=[["Vender mais", "/vender-mais", "Soluções"], ["CDL Mídia", "/cdl-midia", "Soluções › Vender mais"], ["SPC Dados", "/spc-dados", "Soluções › Vender mais"], ["Rede de Descontos", "/loja/descontos", "Soluções › Vender mais"], ["Gestão e economia", "/gestao-e-economia", "Soluções"], ["Certificado Digital", "/certificado-digital", "Soluções › Gestão e economia"], ["CDL Gestão e NF-e", "/cdl-gestao", "Soluções › Gestão e economia"], ["CDL Saúde", "/cdl-saude", "Soluções › Gestão e economia"], ["Parceria BDMG", "/bdmg", "Soluções › Gestão e economia"], ["Combo de Soluções", "/combo-solucoes", "Soluções › Gestão e economia"], ["Consultoria", "https://cdluberlandia.cdludi.org.br/mentoria-de-negocios-fdc4b17686d8495e6609", "Soluções › Gestão e economia"], ["Pessoas", "/pessoas", "Soluções"], ["Recrutamento e Seleção", "/cdl-talentos", "Soluções › Pessoas"], ["CDL Estágio", "/cdl-estagio", "Soluções › Pessoas"], ["CDL Aprendiz", "/cdl-aprendiz", "Soluções › Pessoas"], ["Espaços para eventos", "/cdl-convencoes-e-eventos", "Soluções › Auditório e salas"], ["Vantagens para associados", "/vantagens-associado", "Soluções"], ["Para sua empresa", "/spc-para-empresas", "SPC"], ["SPC Consultas e Registro", "/spc-consulta-registro", "SPC › Para sua empresa"], ["SPC Avisa", "/spc-avisa", "SPC › Para sua empresa"], ["SPC Dados", "/spc-dados", "SPC › Para sua empresa"], ["Cadastro Positivo", "/cadastro-positivo", "SPC › Para sua empresa"], ["Para você (CPF)", "/spc-para-voce", "SPC"], ["SPC Consulta CPF", "/spc-consulta", "SPC › Para você (CPF)"], ["SPC Negociação de Débito", "/spc-negociacao", "SPC › Para você (CPF)"], ["Alerta de Documentos", "/alerta-documento", "SPC › Para você (CPF)"], ["Sobre o SPC Brasil", "/spc-brasil", "SPC"], ["Aprender", "/aprender", "Cursos e Eventos"], ["Eventos", "/loja/eventos", "Cursos e Eventos › Aprender"], ["Cursos", "/loja/cursos", "Cursos e Eventos › Aprender"], ["Cursos EAD", "/loja/cursos-ead", "Cursos e Eventos › Aprender"], ["Treinamento in company", "/treinamento", "Cursos e Eventos › Aprender"], ["Economizar", "/economizar", "Cursos e Eventos"], ["Descontos", "/loja/descontos", "Cursos e Eventos › Economizar"], ["Parceiros", "/loja/parceiros", "Cursos e Eventos › Economizar"], ["CDL MenorPreço", "/loja/menor-preco", "Cursos e Eventos › Economizar"], ["Página inicial da Loja", "/loja", "Cursos e Eventos"], ["Notícias", "/publicacoes/noticias", "Conteúdo"], ["Opinião", "/publicacoes/cdldireto", "Conteúdo"], ["Comunicados", "/publicacoes/comunicados", "Conteúdo"], ["CDL em Ação", "/publicacoes/cdlacao", "Conteúdo"], ["Pesquisas", "/publicacoes/pesquisas", "Conteúdo"], ["CAE", "/publicacoes/cae", "Conteúdo"], ["Legislação", "/legislacao", "Conteúdo"], ["Reforma Tributária", "/reforma-tributaria", "Conteúdo"], ["Ver todas", "/publicacoes", "Conteúdo"], ["Quem somos", "/quem-somos", "A CDL"], ["Nossa História", "/nossa-historia", "A CDL"], ["Fundação CDL", "/fundacao-cdl", "A Fundação"], ["Reconhecimento", "/reconhecimento", "A CDL"], ["Corporativo", "/corporativo", "A CDL"], ["Diretoria", "/diretoria", "A CDL"], ["Política de Sustentabilidade", "/politica-de-sustentabilidade", "A CDL"], ["Selo Empresa Cidadã", "https://empresacidada.cdludi.org.br/", "A CDL"], ["Licitações", "/licitacoes", "A CDL"], ["Instagram", "https://www.instagram.com/cdluberlandia/", "Redes sociais"], ["Facebook", "https://www.facebook.com/cdluberlandia", "Redes sociais"], ["YouTube", "https://www.youtube.com/channel/UCGmeCRd9vgxMd8x6CJjsQHA/", "Redes sociais"], ["LinkedIn", "https://www.linkedin.com/company/cdl-uberl-ndia/", "Redes sociais"], ["X (Twitter)", "https://twitter.com/CDLUBERLANDIA", "Redes sociais"], ["WhatsApp", "https://api.whatsapp.com/send?phone=553432393457", "Redes sociais"], ["Ver tudo em uma página", "/redes-sociais", "Redes sociais"], ["Redes sociais", "/redes-sociais", "Conteúdo"], ["Associe-se", "/associe-se", "Menu"], ["Área do associado", "/painel-associado", "Entrar"], ["Acesse o SPC", "https://sistema.spc.org.br/spc/controleacesso/autenticacao/entry.action", "Entrar"], ["Segunda Via de Boleto", "https://www.itau.com.br/servicos/boletos", "Atalhos"], ["Candidate-se às nossas vagas", "https://jobs.quickin.io/cdludi/jobs?page=1", "Atalhos"], ["Área LGPD", "/lgpd", "A CDL"], ["Responsabilidade Ambiental", "/responsabilidade-ambiental", "Institucional › Política de Sustentabilidade"], ["Responsabilidade Social", "/responsabilidade-social", "Institucional › Política de Sustentabilidade"], ["Governança", "/governanca", "Institucional › Política de Sustentabilidade"], ["Representatividade", "/representatividade", "Institucional › Política de Sustentabilidade"], ["Diretoria da Fundação", "/diretoria-fundacao", "A Fundação"], ["CDL Auditório", "/cdl-auditorio", "Soluções › Espaços para eventos"], ["CDL Foyer", "/cdl-foyer", "Soluções › Espaços para eventos"], ["Sala Espaço Mais", "/sala-espaco-mais", "Soluções › Espaços para eventos"], ["Sala Executiva", "/sala-executiva", "Soluções › Espaços para eventos"], ["Sala Cecília Meireles", "/sala-cecilia-meireles", "Soluções › Espaços para eventos"], ["Sala Machado de Assis", "/sala-machado-de-assis", "Soluções › Espaços para eventos"], ["Jornada Empresarial", "/jornada-empresarial", "Vantagens para Sua Empresa"], ["Área do associado (login)", "/painel-associado/entrar", "Soluções"], ["Soluções (todas)", "/solucoes", "Soluções"], ["Cursos e Eventos (todos)", "/cursos-e-eventos", "Cursos e Eventos"], ["A CDL (tudo)", "/a-cdl", "A CDL"], ["Vagas de emprego e estágio (Fundação)", "https://fundacaocdl.abler.com.br/", "A Fundação"], ["Serviços da Fundação", "/fundacao-cdl#servicos", "A Fundação"]];

  // Mais procurados: aparecem ao abrir a busca, antes de digitar.
  const MAIS=[["2ª via de boleto","https://www.itau.com.br/servicos/boletos","Atalho"],["Consulta CPF","/spc-consulta","SPC › Para você"],["Negociar débitos","/spc-negociacao","SPC › Para você"],["Certificado Digital","/certificado-digital","Soluções"],["Vagas de emprego","https://jobs.quickin.io/cdludi/jobs?page=1","Atalho"]];
  // Palavras do dia a dia (sinônimos) por endereço: quem digita "negativar cliente" encontra SPC Consultas e Registro.
  const SIN={
    "/spc-consulta-registro":"negativar cliente, negativar, negativacao, registrar devedor, incluir no spc, colocar no spc, cliente inadimplente, cliente devendo, consultar cliente, analise de credito, vender a prazo, calote, fiado",
    "/spc-consulta":"nome sujo, meu nome, nome no spc, consultar meu cpf, estou negativado, estou no spc, ver meu nome, restricao no cpf",
    "/spc-negociacao":"pagar divida, limpar nome, limpar meu nome, quitar divida, acordo, renegociar, negociar divida, divida",
    "/spc-avisa":"monitorar cpf, monitorar cnpj, aviso de consulta, golpe, fraude no meu nome",
    "/spc-dados":"atualizar cadastro de clientes, base de clientes, lista de clientes, enriquecer cadastro, telefone de cliente",
    "/cadastro-positivo":"historico de pagamento, bom pagador, score",
    "/alerta-documento":"perdi documento, perdi meu rg, documento roubado, documento furtado, documento extraviado, perdi carteira, boletim de ocorrencia",
    "/certificado-digital":"e-cnpj, ecnpj, e-cpf, ecpf, token, assinatura digital, certificado a1, certificado a3, assinar documento",
    "/cdl-convencoes-e-eventos":"alugar sala, aluguel de sala, locar sala, locacao de sala, alugar auditorio, auditorio, espaco para evento, sala de reuniao, sala para curso, salao, palestra, convencao, coquetel",
    "/cdl-talentos":"contratar funcionario, contratar, selecionar candidato, recrutar, vaga na minha empresa",
    "/cdl-estagio":"estagiario, contratar estagiario, estagio",
    "/cdl-aprendiz":"jovem aprendiz, menor aprendiz, aprendiz",
    "/cdl-saude":"plano de saude, convenio medico, plano medico, saude",
    "/cdl-gestao":"nota fiscal, emitir nota, nfe, nf-e, nfc-e, sistema de gestao, erp, estoque, sistema para loja",
    "/cdl-midia":"divulgar empresa, divulgar minha loja, propaganda, anunciar, anuncio, marketing",
    "/loja/descontos":"desconto, cupom, beneficio, rede de descontos",
    "/loja/cursos":"curso, capacitacao, aula, qualificacao",
    "/loja/cursos-ead":"curso online, curso a distancia, ead",
    "/loja/eventos":"evento, palestra, feira, agenda",
    "/treinamento":"treinar equipe, treinamento na empresa, treinamento personalizado, in company",
    "/bdmg":"emprestimo, credito para empresa, financiamento, capital de giro, dinheiro para empresa",
    "/associe-se":"associar, ser associado, virar associado, filiar, mensalidade, quanto custa",
    "/reforma-tributaria":"ibs, cbs, imposto, impostos, reforma, split payment, iva, imposto seletivo",
    "/legislacao":"atestado medico, lei, lei trabalhista, duvida juridica, juridico, advogado",
    "/licitacoes":"licitacao, edital, prefeitura, ufu",
    "https://www.itau.com.br/servicos/boletos":"segunda via, 2 via, boleto, pagar mensalidade, pagar boleto",
    "https://jobs.quickin.io/cdludi/jobs?page=1":"emprego, trabalhar na cdl, vaga, vagas, curriculo, trabalhe conosco",
    "/redes-sociais":"instagram, facebook, youtube, linkedin, rede social",
    "/lgpd":"privacidade, dados pessoais, lgpd, politica de privacidade",
    "/painel-associado":"login, entrar, minha conta, area do associado",
    "/publicacoes/noticias":"noticia, novidades",
    "/publicacoes/pesquisas":"pesquisa, dados do varejo, intencao de compra"
  };
  const sov=document.getElementById('sov'),q=document.getElementById('sq'),res=document.getElementById('sres');
  const norm=t=>t.normalize('NFD').replace(/[\u0300-\u036f]/g,'').toLowerCase();
  const seen=new Set();const items=DATA.filter(([t,u])=>{const k=t+u;if(seen.has(k))return false;seen.add(k);return true});
  let act=0,cur=[];
  const esc=t=>t.replace(/[&<>"]/g,c=>({'&':'&amp;','<':'&lt;','>':'&gt;','"':'&quot;'}[c]));
  const hl=(t,v)=>{if(!v)return esc(t);const i=norm(t).indexOf(v);return i<0?esc(t):esc(t.slice(0,i))+'<mark>'+esc(t.slice(i,i+v.length))+'</mark>'+esc(t.slice(i+v.length))};
  const link=(t,u,p,i,v)=>`<a class="sr${i===0?' act':''}" href="${u}"${(!/^https?:/.test(u)||u.startsWith('https://cdludi.org.br'))?'':' target="_blank" rel="noopener"'}><b>${hl(t,v)}</b><span>${esc(p)}</span></a>`;
  const render=()=>{
    const v=norm(q.value.trim());
    act=0;
    if(!v){
      cur=MAIS;
      res.innerHTML='<div class="s-h">Mais procurados</div>'+MAIS.map(([t,u,p],i)=>link(t,u,p,i,'')).join('')
        +'<div class="s-h">Todos os serviços</div>'+items.map(([t,u,p],i)=>link(t,u,p,i+MAIS.length,'')).join('');
      return;
    }
    const pal=v.split(/\s+/).filter(Boolean);
    const nota=([t,u,p])=>{
      const tit=norm(t), sec=norm(p), sin=norm(SIN[u]||'');
      if(tit.includes(v))return 0;
      if(v.length>=3&&sin.split(',').some(x=>{x=x.trim();return x&&(x.includes(v)||(x.length>=5&&v.includes(x)))}))return 1;
      const tudo=tit+' '+sec+' '+sin;
      return pal.every(w=>tudo.includes(w))?2:-1;
    };
    cur=items.map(it=>[it,nota(it)]).filter(x=>x[1]>=0).sort((a,b)=>a[1]-b[1]).map(x=>x[0]);
    res.innerHTML=cur.length?cur.map(([t,u,p],i)=>link(t,u,p,i,v)).join(''):'<div class="sempty">Nada encontrado. Tente “SPC”, “nota fiscal”, “alugar sala” ou “certificado”.</div>';
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
