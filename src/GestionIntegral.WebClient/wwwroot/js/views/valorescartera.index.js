(function () {
    'use strict';

    var valores = [];

    function formatear(valor) {
        return Number(valor || 0).toLocaleString('es-AR', { minimumFractionDigits: 2, maximumFractionDigits: 2 });
    }

    function escapar(valor) {
        return valor == null ? '' : valor.toString().replace(/&/g, '&amp;').replace(/</g, '&lt;')
            .replace(/>/g, '&gt;').replace(/"/g, '&quot;');
    }

    function formatearFecha(valor) {
        return valor ? new Date(valor).toLocaleDateString('es-AR') : '';
    }

    function badgeEstado(estado) {
        switch ((estado || '').toLowerCase()) {
            case 'cartera': return 'badge-warning';
            case 'depositado': return 'badge-info';
            case 'entregadoproveedor': return 'badge-secondary';
            case 'acreditado': return 'badge-success';
            default: return 'badge-light';
        }
    }

    function listar() {
        $.get('/ValoresCartera/Listar').done(function (response) {
            if (response && response.exitoso) {
                valores = response.datos || [];
                renderizar();
            } else {
                ModalHelper.notify({ type: 'error', title: 'Error', message: response ? response.mensaje : 'Error al cargar valores.' });
            }
        }).fail(function () {
            ModalHelper.notify({ type: 'error', title: 'Error', message: 'No se pudieron cargar los valores en cartera.' });
        });
    }

    function renderizar() {
        var filtro = $('#vcFiltro').val().toLowerCase();
        var estado = $('#vcFiltroEstado').val();

        var visibles = valores.filter(function (v) {
            var coincideTexto = !filtro ||
                (v.clienteNombre || '').toLowerCase().includes(filtro) ||
                (v.numero || '').toLowerCase().includes(filtro) ||
                (v.banco || '').toLowerCase().includes(filtro);
            var coincideEstado = !estado || v.estado === estado;
            return coincideTexto && coincideEstado;
        });

        var html = '';
        if (!visibles.length) {
            html = '<tr><td colspan="9" class="text-center text-muted">No hay valores en cartera.</td></tr>';
        } else {
            visibles.forEach(function (v) {
                html += '<tr>' +
                    '<td>' + escapar(v.clienteNombre) + '</td>' +
                    '<td>' + escapar(v.tipoValor) + '</td>' +
                    '<td>' + escapar(v.numero) + '</td>' +
                    '<td>' + escapar(v.banco || '') + '</td>' +
                    '<td>' + formatearFecha(v.fechaEmision) + '</td>' +
                    '<td>' + formatearFecha(v.fechaVencimiento) + '</td>' +
                    '<td class="text-right font-weight-bold">' + formatear(v.importe) + '</td>' +
                    '<td><span class="badge ' + badgeEstado(v.estado) + '">' + escapar(v.estado) + '</span></td>' +
                    '<td>' + escapar(v.observaciones || '') + '</td>' +
                    '</tr>';
            });
        }
        $('#valoresCarteraTable tbody').html(html);
    }

    $(function () {
        listar();
        $('#vcFiltro').on('input', renderizar);
        $('#vcFiltroEstado').on('change', renderizar);
    });
})();
