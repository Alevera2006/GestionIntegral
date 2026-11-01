(function () {
    'use strict';

    function redirectOnExpiredSession(status, loginUrl) {
        if (status !== 401 || !loginUrl) return;
        window.location.assign(loginUrl);
    }

    $(document).ajaxError(function (_event, xhr) {
        redirectOnExpiredSession(xhr.status, xhr.getResponseHeader('X-Login-Url'));
    });

    const originalFetch = window.fetch;
    window.fetch = async function (input, init) {
        const request = new Request(input, init);
        if (new URL(request.url).origin === window.location.origin) {
            request.headers.set('X-Requested-With', 'XMLHttpRequest');
        }

        const response = await originalFetch.call(this, request);
        redirectOnExpiredSession(response.status, response.headers.get('X-Login-Url'));
        return response;
    };
})();
