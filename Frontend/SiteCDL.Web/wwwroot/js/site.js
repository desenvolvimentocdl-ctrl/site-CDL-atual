window.addEventListener('resize', function () {
    alternarEntreClasses();
});

document.addEventListener('DOMContentLoaded', function () {
    alternarEntreClasses();
});

function alternarEntreClasses() {
    if (window.innerWidth <= 991) {
        document.querySelector('.menu-one').classList.remove('d-md-block');
        document.querySelector('.menu-two').classList.remove('d-md-block');
    } else {
        document.querySelector('.menu-one').classList.add('d-md-block');
        document.querySelector('.menu-two').classList.add('d-md-block');
    }
}


// Pagina associa-se
//document.querySelector('#form2').style.display = 'none';

function altenarDiv(none, block) {
    document.querySelector(`#${none}`).style.display = 'none';
    document.querySelector(`#${block}`).style.display = 'block';
}

var typing = document.getElementById('typing-effect');

if (typing) {

    new Typed('#typing-effect', {
        strings: [
            '<span class="text-green">Estamos Juntos</span> <span class="text-black">Pelo seu Negócio</span>',
            '<span class="text-green">Estamos Juntos</span> <span class="text-black">Pela sua Capacitação Profissional</span>',
            '<span class="text-green">Estamos Juntos</span> <span class="text-black">Pela sua Segurança</span>',
            '<span class="text-green">Estamos Juntos</span> <span class="text-black">Pelos Seus Resultados</span>'
        ],
        typeSpeed: 50,
        backSpeed: 50,
        backDelay: 1000,
        startDelay: 500,
        loop: true,
        showCursor: true,
        cursorChar: '|'
    });
}