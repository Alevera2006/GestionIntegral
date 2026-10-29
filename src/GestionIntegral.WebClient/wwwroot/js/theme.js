(function () {
    'use strict';

    const THEME_KEY = 'gestionintegral-theme';
    var html = document.documentElement;
    var body = document.body;
    var toggleBtn = document.getElementById('themeToggle');
    var icon = toggleBtn ? toggleBtn.querySelector('i') : null;

    function applyTheme(theme) {
        if (theme === 'dark') {
            html.setAttribute('data-bs-theme', 'dark');
            body.classList.add('dark-mode');
            if (icon) {
                icon.classList.remove('fa-moon');
                icon.classList.add('fa-sun');
            }
        } else {
            html.setAttribute('data-bs-theme', 'light');
            body.classList.remove('dark-mode');
            if (icon) {
                icon.classList.remove('fa-sun');
                icon.classList.add('fa-moon');
            }
        }
    }

    function getStoredTheme() {
        return localStorage.getItem(THEME_KEY) || 'light';
    }

    function setStoredTheme(theme) {
        localStorage.setItem(THEME_KEY, theme);
    }

    applyTheme(getStoredTheme());

    if (toggleBtn) {
        toggleBtn.addEventListener('click', function () {
            var current = getStoredTheme();
            var next = current === 'dark' ? 'light' : 'dark';
            setStoredTheme(next);
            applyTheme(next);
        });
    }
})();
