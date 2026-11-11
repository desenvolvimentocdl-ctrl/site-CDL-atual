const swiper = new Swiper('.swiper', {
    // Optional parameters
    slidesPerView: 1, // Número de slides visíveis por vez
    spaceBetween: 10, // Espaço entre os slides
    autoplay: {
        delay: 4000, // Intervalo entre as transições automáticas (em milissegundos)
        disableOnInteraction: false, // Pausa a reprodução automática quando o usuário interage com o carrossel
    },
    direction: 'horizontal',
    loop: true,

    // If we need pagination
    pagination: {
        el: '.swiper-pagination',
    },

    // Navigation arrows
    navigation: {
        nextEl: '.swiper-button-next',
        prevEl: '.swiper-button-prev',
    },

    // And if we need scrollbar
    scrollbar: {
        el: '.swiper-scrollbar',
    },
});