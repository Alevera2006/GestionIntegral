(function () {
    'use strict';

    var items = [];
    var clientes = [];
    var productos = [];
    var condicionesPago = [];
    var tiposComprobante = [];
    var alicuotas = [];
    var token = $('input[name="__RequestVerificationToken"]').val();

    function formatear(valor) {
        return Number(valor || 0).toLocaleString('es-AR', { minimumFractionDigits: 2, maximumFractionDigits: 2 });
    }

    function cargarSelects() {
        $.when(
            $.get('/Facturacion/Clientes'),
            $.get('/Facturacion/Productos'),
            $.get('/Facturacion/CondicionesPago'),
            $.get('/Facturacion/TiposComprobante'),
            $.get('/Facturacion/AlicuotasIva')
        ).done(function (resClientes, resProductos, resCondiciones, resTipos, resAlicuotas) {
            clientes = (resClientes[0] && resClientes[0].exitoso ? resClientes[0].datos : []) || [];
            productos = (resProductos[0] && resProductos[0].exitoso ? resProductos[0].datos : []) || [];
            condicionesPago = (resCondiciones[0] && resCondiciones[0].exitoso ? resCondiciones[0].datos : []) || [];
            tiposComprobante = (resTipos[0] && resTipos[0].exitoso ? resTipos[0].datos : []) || [];
            alicuotas = (resAlicuotas[0] && resAlicuotas[0].exitoso ? resAlicuotas[0].datos : []) || [];

            var selCliente = $('#factCliente').empty().append(new Option('Consumidor Final', ''));
            clientes.forEach(function (c) { if (c.activo) selCliente.append(new Option(c.razonSocial, c.idCliente)); });

            var selProducto = $('#factProducto').empty().append(new Option('Seleccione un producto', ''));
            productos.forEach(function (p) { if (p.activo) selProducto.append(new Option(p.codigo + ' - ' + p.descripcion, p.idProducto)); });

            var selCondicion = $('#factCondicionPago').empty().append(new Option('Seleccione...', ''));
            condicionesPago.forEach(function (c) { if (c.activo) selCondicion.append(new Option(c.descripcion, c.idCondicionPago)); });

            var selTipo = $('#factTipoComprobante').empty().append(new Option('Seleccione...', ''));
            tiposComprobante.forEach(function (t) { if (t.activo) selTipo.append(new Option(t.descripcion, t.idTipoComprobante)); });

            var selIva = $('#factIva').empty();
            alicuotas.forEach(function (a) { if (a.activo) selIva.append(new Option(a.porcentaje + '%', a.porcentaje)); });
            if (alicuotas.length) selIva.val(alicuotas[0].porcentaje);

            $('#factFecha').val(new Date().toISOString().split('T')[0]);
        });
    }

    function calcularTotales() {
        var subtotal = 0, iva = 0;
        items.forEach(function (i) {
            subtotal += i.cantidad * i.precioUnitario;
            iva += i.cantidad * i.precioUnitario * (i.porcentajeIva / 100);
        });
        $('#totalSubtotal').text(formatear(subtotal));
        $('#totalIva').text(formatear(iva));
        $('#totalGeneral').text(formatear(subtotal + iva));
    }

    function renderizarItems() {
        var html = '';
        if (!items.length) {
            html = '<tr><td colspan="7" class="text-center text-muted">No hay ítems agregados.</td></tr>';
        } else {
            items.forEach(function (i, idx) {
                var st = i.cantidad * i.precioUnitario;
                var iv = st * (i.porcentajeIva / 100);
                html += '<tr data-index="' + idx + '">' +
                    '<td>' + i.descripcion + '</td>' +
                    '<td>' + i.cantidad + '</td>' +
                    '<td>' + formatear(i.precioUnitario) + '</td>' +
                    '<td>' + i.porcentajeIva + '%</td>' +
                    '<td>' + formatear(st) + '</td>' +
                    '<td>' + formatear(iv) + '</td>' +
                    '<td class="table-actions"><button type="button" class="btn btn-danger btn-sm btn-quitar-item"><i class="fas fa-trash-alt"></i></button></td>' +
                    '</tr>';
            });
        }
        $('#itemsTable tbody').html(html);
        calcularTotales();
    }

    function agregarItem() {
        var idProducto = $('#factProducto').val();
        var cantidad = parseFloat($('#factCantidad').val());
        var precio = parseFloat($('#factPrecio').val());
        var iva = parseFloat($('#factIva').val());

        if (!idProducto || !cantidad || cantidad <= 0 || isNaN(precio) || precio < 0) {
            ModalHelper.notify({ type: 'warning', title: 'Validación', message: 'Seleccione producto, cantidad y precio válidos.' });
            return;
        }

        var producto = productos.find(function (p) { return p.idProducto == idProducto; });
        items.push({
            idProducto: parseInt(idProducto, 10),
            descripcion: producto ? producto.descripcion : '',
            cantidad: cantidad,
            precioUnitario: precio,
            porcentajeIva: iva || 0
        });

        $('#factProducto, #factCantidad, #factPrecio').val('');
        renderizarItems();
    }

    function guardar(evento) {
        evento.preventDefault();
        if (!items.length) {
            ModalHelper.notify({ type: 'warning', title: 'Validación', message: 'Agregue al menos un ítem.' });
            return;
        }

        var idCliente = $('#factCliente').val();
        var request = {
            idCliente: idCliente ? parseInt(idCliente, 10) : null,
            nombreCliente: idCliente ? null : $('#factNombreCliente').val() || 'Consumidor Final',
            idTipoComprobante: parseInt($('#factTipoComprobante').val()),
            numeroComprobante: $('#factNumero').val().trim(),
            fecha: $('#factFecha').val(),
            idCondicionPago: parseInt($('#factCondicionPago').val()),
            observaciones: $('#factObservaciones').val().trim(),
            detalles: items
        };

        if (!request.idTipoComprobante || !request.numeroComprobante || !request.fecha || !request.idCondicionPago) {
            ModalHelper.notify({ type: 'warning', title: 'Validación', message: 'Complete todos los datos de la factura.' });
            return;
        }

        $('#facturacionForm button[type="submit"]').prop('disabled', true);
        $.ajax({
            url: '/Facturacion/Crear',
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(request),
            headers: { RequestVerificationToken: token },
            success: function (response) {
                if (response && response.exitoso) {
                    ModalHelper.notify({ type: 'success', title: 'Éxito', message: 'Factura registrada correctamente.' });
                    setTimeout(function () { location.reload(); }, 1200);
                } else {
                    ModalHelper.notify({ type: 'error', title: 'Error', message: response ? response.mensaje : 'Error al guardar.' });
                }
            },
            error: function (xhr) {
                var mensaje = xhr.responseJSON && xhr.responseJSON.mensaje;
                ModalHelper.notify({ type: 'error', title: 'Error', message: mensaje || 'No se pudo registrar la factura.' });
            },
            complete: function () { $('#facturacionForm button[type="submit"]').prop('disabled', false); }
        });
    }

    $(function () {
        cargarSelects();

        $('#btnAgregarItem').on('click', agregarItem);

        $('#itemsTable tbody').on('click', '.btn-quitar-item', function () {
            var idx = $(this).closest('tr').data('index');
            items.splice(idx, 1);
            renderizarItems();
        });

        $('#facturacionForm').on('submit', guardar);
    });
})();
