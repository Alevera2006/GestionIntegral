(function () {
    'use strict';

    function escapar(valor) {
        return valor == null ? '' : valor.toString().replace(/&/g, '&amp;').replace(/</g, '&lt;')
            .replace(/>/g, '&gt;').replace(/"/g, '&quot;');
    }

    function formatearFecha(fecha) {
        if (!fecha) return '-';
        var d = new Date(fecha);
        if (isNaN(d.getTime())) return '-';
        return d.toLocaleString('es-AR');
    }

    function cargarStock() {
        var idDeposito = $('#filtroDeposito').val();
        var idCategoria = $('#filtroCategoria').val();
        var idSubCategoria = $('#filtroSubCategoria').val();
        var busqueda = $('#filtroBusqueda').val().trim();

        var url = '/Stock/Filtrar?';
        var params = [];
        if (idDeposito) params.push('IdDeposito=' + encodeURIComponent(idDeposito));
        if (idCategoria) params.push('IdCategoria=' + encodeURIComponent(idCategoria));
        if (idSubCategoria) params.push('IdSubCategoria=' + encodeURIComponent(idSubCategoria));
        if (busqueda) params.push('BusquedaProducto=' + encodeURIComponent(busqueda));

        $.ajax({
            url: url + params.join('&'),
            type: 'GET',
            beforeSend: function () {
                $('#stockTable tbody').html('<tr><td colspan="5" class="text-center">Cargando...</td></tr>');
            },
            success: function (response) {
                if (!response || !response.exitoso) {
                    $('#stockTable tbody').html('<tr><td colspan="5" class="text-center text-danger">' + (response ? response.mensaje : 'Error al cargar stock.') + '</td></tr>');
                    return;
                }

                var datos = response.datos || [];
                if (!datos.length) {
                    $('#stockTable tbody').html('<tr><td colspan="5" class="text-center text-muted">No se encontraron registros.</td></tr>');
                    return;
                }

                var html = '';
                datos.forEach(function (item) {
                    html += '<tr>' +
                        '<td>' + escapar(item.depositoDescripcion) + '</td>' +
                        '<td>' + escapar(item.productoCodigo) + '</td>' +
                        '<td>' + escapar(item.productoDescripcion) + '</td>' +
                        '<td class="text-right font-weight-bold">' + item.cantidad.toLocaleString('es-AR') + '</td>' +
                        '<td>' + formatearFecha(item.fechaUltimoMovimiento) + '</td>' +
                        '</tr>';
                });
                $('#stockTable tbody').html(html);
            },
            error: function () {
                $('#stockTable tbody').html('<tr><td colspan="5" class="text-center text-danger">No se pudo cargar el stock.</td></tr>');
            }
        });
    }

    function limpiarFiltros() {
        $('#filtroDeposito, #filtroCategoria, #filtroSubCategoria, #filtroBusqueda').val('');
        $('#stockTable tbody').html('<tr><td colspan="5" class="text-center text-muted">Presione Buscar para cargar el stock.</td></tr>');
    }

    function filtrarSubCategorias() {
        var idCategoria = $('#filtroCategoria').val();
        $('#filtroSubCategoria option').each(function () {
            var $opt = $(this);
            var categoria = $opt.data('categoria');
            if (!idCategoria || !categoria || categoria === idCategoria) {
                $opt.show();
            } else {
                $opt.hide();
            }
        });
        $('#filtroSubCategoria').val('');
    }

    $(document).ready(function () {
        $('#btnBuscarStock').on('click', cargarStock);
        $('#btnLimpiarStock').on('click', limpiarFiltros);
        $('#filtroCategoria').on('change', filtrarSubCategorias);
    });
})();
