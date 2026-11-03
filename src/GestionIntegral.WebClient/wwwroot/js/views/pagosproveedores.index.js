(function () {
    'use strict';

    var proveedores = [];
    var mediosPago = [];
    var pagos = [];
    var token = $('input[name="__RequestVerificationToken"]').val();

    function formatear(valor) {
        return Number(valor || 0).toLocaleString('es-AR', { minimumFractionDigits: 2, maximumFractionDigits: 2 });
    }

    function escapar(valor) {
        return valor == null ? '' : valor.toString().replace(/&/g, '&amp;').replace(/</g, '&lt;')
            .replace(/>/g, '&gt;').replace(/"/g, '&quot;');
    }

    function cargarSelects() {
        $.when(
            $.get('/PagosProveedores/Proveedores'),
            $.get('/PagosProveedores/MediosPago')
        ).done(function (resProveedores, resMedios) {
            proveedores = (resProveedores[0] && resProveedores[0].exitoso ? resProveedores[0].datos : []) || [];
            mediosPago = (resMedios[0] && resMedios[0].exitoso ? resMedios[0].datos : []) || [];

            var selProveedor = $('#ppProveedor').empty().append(new Option('Seleccione un proveedor', ''));
            var selFiltro = $('#ppFiltroProveedor').empty().append(new Option('Todos los proveedores', ''));
            proveedores.forEach(function (p) {
                if (p.activo) {
                    selProveedor.append(new Option(p.razonSocial, p.idProveedor));
                    selFiltro.append(new Option(p.razonSocial, p.idProveedor));
                }
            });

            var selMedio = $('#ppMedioPago').empty().append(new Option('Seleccione...', ''));
            mediosPago.forEach(function (m) { if (m.activo) selMedio.append(new Option(m.descripcion, m.idMedioPago)); });

            $('#ppFecha').val(new Date().toISOString().split('T')[0]);
        });
    }

    function renderizarPagos() {
        var idFiltro = $('#ppFiltroProveedor').val();
        var visibles = pagos.filter(function (p) { return !idFiltro || p.idProveedor == idFiltro; });
        var html = '';
        if (!visibles.length) {
            html = '<tr><td colspan="6" class="text-center text-muted">No hay pagos registrados.</td></tr>';
        } else {
            visibles.forEach(function (p) {
                html += '<tr>' +
                    '<td>' + new Date(p.fecha).toLocaleDateString('es-AR') + '</td>' +
                    '<td>' + escapar(p.proveedorNombre) + '</td>' +
                    '<td>' + escapar(p.concepto) + '</td>' +
                    '<td>' + escapar(p.medioPago) + '</td>' +
                    '<td class="text-right font-weight-bold">' + formatear(p.monto) + '</td>' +
                    '<td>' + escapar((p.numeroCheque || '') + (p.banco ? ' - ' + p.banco : '')) + '</td>' +
                    '</tr>';
            });
        }
        $('#pagosProveedorTable tbody').html(html);
    }

    function listar() {
        $.get('/PagosProveedores/Listar').done(function (response) {
            if (response && response.exitoso) {
                pagos = response.datos || [];
                renderizarPagos();
            }
        });
    }

    function guardar(evento) {
        evento.preventDefault();
        var request = {
            idProveedor: parseInt($('#ppProveedor').val()),
            fecha: $('#ppFecha').val(),
            monto: parseFloat($('#ppMonto').val()),
            idMedioPago: parseInt($('#ppMedioPago').val()),
            numeroCheque: $('#ppNumeroCheque').val().trim() || null,
            banco: $('#ppBanco').val().trim() || null,
            fechaVencimientoCheque: $('#ppFechaVencimientoCheque').val() || null,
            concepto: $('#ppConcepto').val().trim(),
            observaciones: $('#ppObservaciones').val().trim()
        };

        if (!request.idProveedor || !request.fecha || isNaN(request.monto) || request.monto <= 0 || !request.idMedioPago || !request.concepto) {
            ModalHelper.notify({ type: 'warning', title: 'Validación', message: 'Complete todos los campos obligatorios.' });
            return;
        }

        $('#pagoProveedorForm button[type="submit"]').prop('disabled', true);
        $.ajax({
            url: '/PagosProveedores/Crear',
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(request),
            headers: { RequestVerificationToken: token },
            success: function (response) {
                if (response && response.exitoso) {
                    $('#pagoProveedorForm')[0].reset();
                    $('#ppFecha').val(new Date().toISOString().split('T')[0]);
                    $('#ppChequeFields').hide();
                    listar();
                    ModalHelper.notify({ type: 'success', title: 'Éxito', message: 'Pago registrado correctamente.' });
                } else {
                    ModalHelper.notify({ type: 'error', title: 'Error', message: response ? response.mensaje : 'Error al guardar.' });
                }
            },
            error: function (xhr) {
                var mensaje = xhr.responseJSON && xhr.responseJSON.mensaje;
                ModalHelper.notify({ type: 'error', title: 'Error', message: mensaje || 'No se pudo registrar el pago.' });
            },
            complete: function () { $('#pagoProveedorForm button[type="submit"]').prop('disabled', false); }
        });
    }

    $(function () {
        cargarSelects();
        listar();

        $('#ppMedioPago').on('change', function () {
            var medio = mediosPago.find(function (m) { return m.idMedioPago == $(this).val(); });
            var esCheque = medio && (medio.descripcion || '').toLowerCase().includes('cheque');
            $('#ppChequeFields').toggle(esCheque);
        });

        $('#ppFiltroProveedor').on('change', renderizarPagos);

        $('#pagoProveedorForm').on('submit', guardar);
    });
})();
