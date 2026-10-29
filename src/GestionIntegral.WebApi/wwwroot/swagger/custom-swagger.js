(function () {
    'use strict';

    function customizeSwaggerHeader() {
        var wrapper = document.querySelector('.topbar-wrapper');
        if (!wrapper) return;

        // Evitar duplicados
        if (wrapper.querySelector('.swagger-logo')) return;

        // Ocultar SVG por defecto de Swagger
        var svg = wrapper.querySelector('svg');
        if (svg) {
            svg.style.display = 'none';
        }

        // Insertar logo y título personalizado al principio
        var customHtml =
            '<img class="swagger-logo" src="/images/logo-api.png" alt="GestionIntegral API" />' +
            '<span class="swagger-title">GestionIntegral API</span>';

        wrapper.insertAdjacentHTML('afterbegin', customHtml);
    }

    // Ejecutar cuando el DOM esté listo y después de un pequeño delay por si Swagger renderiza asíncrono
    if (document.readyState === 'loading') {
        document.addEventListener('DOMContentLoaded', function () {
            setTimeout(customizeSwaggerHeader, 100);
            setTimeout(customizeSwaggerHeader, 500);
        });
    } else {
        setTimeout(customizeSwaggerHeader, 100);
        setTimeout(customizeSwaggerHeader, 500);
    }
})();
