const swiper = new Swiper('.swiper', {
    // Optional parameters
    // slidesPerView: 6, // Número de slides visíveis por vez
    // spaceBetween: 10, // Espaço entre os slides
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

    breakpoints: {
        // when window width is >= 320px
        320: {
            slidesPerView: 1,
            spaceBetween: 20
        },
        // when window width is >= 480px
        480: {
            slidesPerView: 3,
            spaceBetween: 30
        },
        // when window width is >= 640px
        640: {
            slidesPerView: 4,
            spaceBetween: 40
        }
    }

});