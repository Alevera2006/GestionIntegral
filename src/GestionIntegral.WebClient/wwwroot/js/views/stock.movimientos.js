(function () {
    'use strict';

    var token = $('input[name="__RequestVerificationToken"]').val();

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

    function renderizar(datos) {
        if (!datos.length) {
            $('#movimientosTable tbody').html('<tr><td colspan="7" class="text-center text-muted">No hay movimientos registrados.</td></tr>');
            return;
        }

        var html = '';
        datos.forEach(function (item) {
            var detalles = item.detalles || [];
            var cantidadProductos = detalles.length;
            var productosResumen = cantidadProductos > 0
                ? detalles.slice(0, 2).map(function (d) { return escapar(d.productoDescripcion); }).join(', ') +
                  (cantidadProductos > 2 ? ' y ' + (cantidadProductos - 2) + ' más' : '')
                : 'Sin productos';

            html += '<tr data-id="' + item.idMovimientoStock + '">' +
                '<td>' + formatearFecha(item.fechaMovimiento) + '</td>' +
                '<td>' + escapar(item.depositoDescripcion) + '</td>' +
                '<td>' + escapar(item.tipoMovimientoStockDescripcion) + '</td>' +
                '<td>' + escapar(item.nombreUsuarioResponsable) + '</td>' +
                '<td>' + escapar(item.referencia) + '</td>' +
                '<td title="' + detalles.map(function (d) { return escapar(d.productoDescripcion) + ' (' + d.cantidad + ')'; }).join('\n') + '">' +
                    productosResumen + '</td>' +
                '<td class="table-actions">' +
                    '<button type="button" class="btn btn-danger btn-sm btn-eliminar" title="Eliminar"><i class="fas fa-trash-alt"></i></button>' +
                '</td>' +
                '</tr>';
        });
        $('#movimientosTable tbody').html(html);
    }

    function listar() {
        $.ajax({
            url: '/Stock/ListarMovimientos',
            type: 'GET',
            beforeSend: function () {
                $('#movimientosTable tbody').html('<tr><td colspan="7" class="text-center">Cargando...</td></tr>');
            },
            success: function (response) {
                if (!response || !response.exitoso) {
                    $('#movimientosTable tbody').html('<tr><td colspan="7" class="text-center text-danger">' + (response ? response.mensaje : 'Error al cargar movimientos.') + '</td></tr>');
                    return;
                }
                renderizar(response.datos || []);
            },
            error: function () {
                $('#movimientosTable tbody').html('<tr><td colspan="7" class="text-center text-danger">No se pudieron cargar los movimientos.</td></tr>');
            }
        });
    }

    function eliminar(id) {
        if (!confirm('¿Está seguro de eliminar el movimiento? Se reversará el stock afectado.'))
            return;

        $.ajax({
            url: '/Stock/EliminarMovimiento/' + id,
            type: 'DELETE',
            headers: { RequestVerificationToken: token },
            success: function (response) {
                if (response && response.exitoso) {
                    ModalHelper.notify({ type: 'success', title: 'Éxito', message: 'Movimiento eliminado correctamente.' });
                    listar();
                } else {
                    ModalHelper.notify({ type: 'error', title: 'Error', message: response ? response.mensaje : 'No se pudo eliminar.' });
                }
            },
            error: function () {
                ModalHelper.notify({ type: 'error', title: 'Error', message: 'No se pudo eliminar el movimiento.' });
            }
        });
    }

    $(document).ready(function () {
        listar();

        $('#movimientosTable').on('click', '.btn-eliminar', function () {
            var id = $(this).closest('tr').data('id');
            eliminar(id);
        });
    });
})();
