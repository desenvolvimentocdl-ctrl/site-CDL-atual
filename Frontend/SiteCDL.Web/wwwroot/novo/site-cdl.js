// Comportamentos do layout novo que dependem do sistema (SiteCDL.Web)

// Newsletter do rodapé: mantém o mesmo id/nome do layout antigo (o RD Station captura o envio)
(() => {
  const f = document.getElementById('formNewsletter'); if (!f) return;
  f.addEventListener('submit', e => {
    e.preventDefault();
    const ok = document.getElementById('nlOk');
    document.getElementById('emailNewsletter').value = '';
    if (ok) ok.hidden = false;
  });
})();

// Abas do conteúdo que vem do banco (tema antigo usava as abas do Bootstrap)
document.querySelectorAll('.cms [data-toggle="tab"], .cms [data-bs-toggle="tab"]').forEach(a => a.addEventListener('click', e => {
  const alvo = document.querySelector(a.getAttribute('href') || a.dataset.bsTarget || a.dataset.target);
  if (!alvo) return;
  e.preventDefault();
  const nav = a.closest('.nav'); if (nav) nav.querySelectorAll('.nav-link').forEach(x => x.classList.toggle('active', x === a));
  alvo.parentElement.querySelectorAll(':scope > .tab-pane').forEach(p => p.classList.toggle('active', p === alvo));
}));

// Caixa de busca do cabeçalho: além dos atalhos do menu, oferece a busca completa do site (/buscar)
(() => {
  const q = document.getElementById('sq'), res = document.getElementById('sres'); if (!q || !res) return;
  const link = document.createElement('a'); link.className = 'sr sr-full'; link.hidden = true;
  res.insertAdjacentElement('afterend', link);
  q.addEventListener('input', () => {
    const v = q.value.trim(); link.hidden = v.length < 3;
    link.href = '/buscar?id=' + encodeURIComponent(v);
    link.innerHTML = '<b>Buscar “' + v.replace(/[&<>"]/g, '') + '” em todo o site</b><span>Notícias, comunicados e páginas</span>';
  });
})();

// Home: botão Ocultar/Mostrar do Termômetro do Varejo (a escolha fica salva neste navegador)
(()=>{
  const b=document.querySelector('[data-thermo-tg]');if(!b)return;
  const box=b.closest('.thermo'),txt=b.querySelector('span'),K='cdl-thermo-oculto';
  const set=min=>{box.classList.toggle('min',min);b.setAttribute('aria-expanded',String(!min));txt.textContent=min?'Mostrar':'Ocultar'};
  try{if(localStorage.getItem(K)==='1')set(true)}catch(e){}
  b.addEventListener('click',()=>{const min=!box.classList.contains('min');set(min);try{min?localStorage.setItem(K,'1'):localStorage.removeItem(K)}catch(e){}});
})();

/* Botão "Entrar" do topo: abre/fecha o menu com Área do associado e Acesse o SPC */
(function () {
  var box = document.querySelector('.entrar');
  if (!box) return;
  var btn = box.querySelector('button'), menu = box.querySelector('.entrar-menu');
  function fechar() { menu.hidden = true; btn.setAttribute('aria-expanded', 'false'); }
  btn.addEventListener('click', function (e) {
    e.stopPropagation();
    var abrir = menu.hidden;
    menu.hidden = !abrir;
    btn.setAttribute('aria-expanded', abrir ? 'true' : 'false');
  });
  document.addEventListener('click', function (e) { if (!box.contains(e.target)) fechar(); });
  document.addEventListener('keydown', function (e) { if (e.key === 'Escape') fechar(); });
})();
