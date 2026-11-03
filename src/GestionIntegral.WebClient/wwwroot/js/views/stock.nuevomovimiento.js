(function () {
    'use strict';

    var token = $('input[name="__RequestVerificationToken"]').val();
    var productosSeleccionados = [];
    var productoSeleccionadoId = null;

    function escapar(valor) {
        return valor == null ? '' : valor.toString().replace(/&/g, '&amp;').replace(/</g, '&lt;')
            .replace(/>/g, '&gt;').replace(/"/g, '&quot;');
    }

    function buscarProductos() {
        var busqueda = $('#busquedaProducto').val().trim();
        if (!busqueda) {
            ModalHelper.notify({ type: 'warning', title: 'Validación', message: 'Ingrese un texto de búsqueda.' });
            return;
        }

        $.ajax({
            url: '/Stock/BuscarProductos?busqueda=' + encodeURIComponent(busqueda),
            type: 'GET',
            success: function (response) {
                if (!response || !response.exitoso) {
                    ModalHelper.notify({ type: 'error', title: 'Error', message: response ? response.mensaje : 'Error al buscar productos.' });
                    return;
                }

                var datos = response.datos || [];
                var $select = $('#resultadosProducto').empty();
                if (!datos.length) {
                    $select.hide();
                    ModalHelper.notify({ type: 'info', title: 'Búsqueda', message: 'No se encontraron productos.' });
                    return;
                }

                datos.forEach(function (p) {
                    $select.append(new Option(p.codigo + ' - ' + p.descripcion, p.idProducto));
                });
                $select.show();
                $('#btnAgregarProducto').prop('disabled', false);
                productoSeleccionadoId = null;
            },
            error: function () {
                ModalHelper.notify({ type: 'error', title: 'Error', message: 'No se pudieron buscar los productos.' });
            }
        });
    }

    function agregarProducto() {
        var idProducto = $('#resultadosProducto').val();
        if (!idProducto) {
            ModalHelper.notify({ type: 'warning', title: 'Validación', message: 'Seleccione un producto de la lista.' });
            return;
        }

        idProducto = Number(idProducto);
        if (productosSeleccionados.some(function (p) { return p.idProducto === idProducto; })) {
            ModalHelper.notify({ type: 'warning', title: 'Validación', message: 'El producto ya fue agregado.' });
            return;
        }

        var texto = $('#resultadosProducto option:selected').text();
        productosSeleccionados.push({ idProducto: idProducto, descripcion: texto, cantidad: 1 });
        renderizarDetalles();
        $('#resultadosProducto').empty().hide();
        $('#busquedaProducto').val('');
        $('#btnAgregarProducto').prop('disabled', true);
    }

    function actualizarCantidad(idProducto, cantidad) {
        var item = productosSeleccionados.find(function (p) { return p.idProducto === idProducto; });
        if (item) {
            item.cantidad = cantidad;
        }
    }

    function eliminarProducto(idProducto) {
        productosSeleccionados = productosSeleccionados.filter(function (p) { return p.idProducto !== idProducto; });
        renderizarDetalles();
    }

    function renderizarDetalles() {
        var $tbody = $('#detallesTable tbody').empty();
        if (!productosSeleccionados.length) {
            $tbody.html('<tr class="sin-detalles"><td colspan="3" class="text-center text-muted">Agregue al menos un producto.</td></tr>');
            $('#btnGuardarMovimiento').prop('disabled', true);
            return;
        }

        productosSeleccionados.forEach(function (p) {
            $tbody.append(
                '<tr data-id="' + p.idProducto + '">' +
                '<td>' + escapar(p.descripcion) + '</td>' +
                '<td class="text-right"><input type="number" class="form-control form-control-sm cantidad-detalle" value="' + p.cantidad + '" min="0.0001" step="0.0001" style="width: 120px; display: inline-block;" /></td>' +
                '<td class="table-actions"><button type="button" class="btn btn-danger btn-sm btn-eliminar-detalle"><i class="fas fa-trash-alt"></i></button></td>' +
                '</tr>'
            );
        });

        $('#btnGuardarMovimiento').prop('disabled', false);
    }

    function guardar(evento) {
        evento.preventDefault();

        var idDeposito = Number($('#idDeposito').val());
        var idTipoMovimientoStock = Number($('#idTipoMovimientoStock').val());
        var idUsuarioResponsable = Number($('#idUsuarioResponsable').val());
        var referencia = $('#referencia').val().trim();
        var observaciones = $('#observaciones').val().trim();

        if (!idDeposito || !idTipoMovimientoStock || !idUsuarioResponsable) {
            ModalHelper.notify({ type: 'warning', title: 'Validación', message: 'Complete depósito, tipo de movimiento y responsable.' });
            return;
        }

        if (!productosSeleccionados.length) {
            ModalHelper.notify({ type: 'warning', title: 'Validación', message: 'Agregue al menos un producto.' });
            return;
        }

        var detallesInvalidos = productosSeleccionados.some(function (p) {
            return !p.cantidad || p.cantidad <= 0;
        });

        if (detallesInvalidos) {
            ModalHelper.notify({ type: 'warning', title: 'Validación', message: 'Todas las cantidades deben ser mayores a cero.' });
            return;
        }

        var request = {
            idDeposito: idDeposito,
            idTipoMovimientoStock: idTipoMovimientoStock,
            idUsuarioResponsable: idUsuarioResponsable,
            referencia: referencia || null,
            observaciones: observaciones || null,
            detalles: productosSeleccionados.map(function (p) {
                return { idProducto: p.idProducto, cantidad: p.cantidad };
            })
        };

        $('#btnGuardarMovimiento').prop('disabled', true);

        $.ajax({
            url: '/Stock/CrearMovimiento',
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(request),
            headers: { RequestVerificationToken: token },
            success: function (response) {
                if (response && response.exitoso) {
                    ModalHelper.notify({ type: 'success', title: 'Éxito', message: 'Movimiento registrado correctamente.' });
                    setTimeout(function () {
                        window.location.href = '/Stock/Movimientos';
                    }, 1200);
                } else {
                    ModalHelper.notify({ type: 'error', title: 'Error', message: response ? response.mensaje : 'No se pudo registrar el movimiento.' });
                    $('#btnGuardarMovimiento').prop('disabled', false);
                }
            },
            error: function () {
                ModalHelper.notify({ type: 'error', title: 'Error', message: 'No se pudo registrar el movimiento.' });
                $('#btnGuardarMovimiento').prop('disabled', false);
            }
        });
    }

    $(document).ready(function () {
        $('#btnBuscarProducto').on('click', buscarProductos);
        $('#busquedaProducto').on('keypress', function (e) {
            if (e.which === 13) {
                e.preventDefault();
                buscarProductos();
            }
        });

        $('#resultadosProducto').on('change', function () {
            productoSeleccionadoId = $(this).val();
        });

        $('#btnAgregarProducto').on('click', agregarProducto);

        $('#detallesTable').on('change', '.cantidad-detalle', function () {
            var id = Number($(this).closest('tr').data('id'));
            var cantidad = Number($(this).val());
            actualizarCantidad(id, cantidad);
        });

        $('#detallesTable').on('click', '.btn-eliminar-detalle', function () {
            var id = Number($(this).closest('tr').data('id'));
            eliminarProducto(id);
        });

        $('#movimientoForm').on('submit', guardar);
    });
})();
