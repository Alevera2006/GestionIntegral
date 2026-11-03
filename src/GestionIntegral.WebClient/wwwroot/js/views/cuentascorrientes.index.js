(function () {
    'use strict';

    var clientesCc = [];
    var movimientos = [];
    var mediosPago = [];
    var clienteActual = null;
    var token = $('input[name="__RequestVerificationToken"]').val();

    function formatear(valor) {
        return Number(valor || 0).toLocaleString('es-AR', { minimumFractionDigits: 2, maximumFractionDigits: 2 });
    }

    function escapar(valor) {
        return valor == null ? '' : valor.toString().replace(/&/g, '&amp;').replace(/</g, '&lt;')
            .replace(/>/g, '&gt;').replace(/"/g, '&quot;');
    }

    function cargarMediosPago() {
        return $.get('/CuentasCorrientes/MediosPago').done(function (response) {
            mediosPago = response && response.exitoso ? response.datos || [] : [];
            var select = $('#pagoMedioPago').empty().append(new Option('Seleccione...', ''));
            mediosPago.forEach(function (m) { if (m.activo) select.append(new Option(m.descripcion, m.idMedioPago)); });
        });
    }

    function listarClientes() {
        $.ajax({
            url: '/CuentasCorrientes/ListarClientes',
            type: 'GET',
            beforeSend: function () {
                $('#clientesCcTable tbody').html('<tr><td colspan="3" class="text-center">Cargando...</td></tr>');
            },
            success: function (response) {
                if (!response || !response.exitoso) {
                    ModalHelper.notify({ type: 'error', title: 'Error', message: response ? response.mensaje : 'Error al cargar clientes.' });
                    return;
                }
                clientesCc = response.datos || [];
                renderizarClientes();
            },
            error: function () {
                ModalHelper.notify({ type: 'error', title: 'Error', message: 'No se pudieron cargar las cuentas corrientes.' });
            }
        });
    }

    function renderizarClientes() {
        var filtro = $('#ccFiltro').val().toLowerCase();
        var visibles = clientesCc.filter(function (c) {
            return !filtro || (c.clienteNombre || '').toLowerCase().includes(filtro);
        });
        var html = '';
        if (!visibles.length) {
            html = '<tr><td colspan="3" class="text-center text-muted">No hay clientes con cuenta corriente.</td></tr>';
        } else {
            visibles.forEach(function (c) {
                html += '<tr data-id="' + c.idCliente + '">' +
                    '<td>' + escapar(c.clienteNombre) + '</td>' +
                    '<td class="text-right font-weight-bold ' + (c.saldo > 0 ? 'text-danger' : 'text-success') + '">' + formatear(c.saldo) + '</td>' +
                    '<td class="table-actions"><button type="button" class="btn btn-info btn-sm btn-ver"><i class="fas fa-eye"></i></button></td>' +
                    '</tr>';
            });
        }
        $('#clientesCcTable tbody').html(html);
    }

    function verMovimientos(idCliente) {
        clienteActual = idCliente;
        var cliente = clientesCc.find(function (c) { return c.idCliente === idCliente; });
        $('#movimientosTitulo').text('Movimientos - ' + (cliente ? cliente.clienteNombre : ''));
        $('#pagoIdCliente').val(idCliente);
        $('#ccSaldoActual').text('Saldo: $ ' + formatear(cliente ? cliente.saldo : 0));

        $.ajax({
            url: '/CuentasCorrientes/Movimientos?idCliente=' + idCliente,
            type: 'GET',
            beforeSend: function () {
                $('#movimientosTable tbody').html('<tr><td colspan="7" class="text-center">Cargando...</td></tr>');
            },
            success: function (response) {
                if (!response || !response.exitoso) {
                    ModalHelper.notify({ type: 'error', title: 'Error', message: response ? response.mensaje : 'Error al cargar movimientos.' });
                    return;
                }
                movimientos = response.datos || [];
                renderizarMovimientos();

                var selFactura = $('#pagoFactura').empty().append(new Option('Pago a cuenta (sin factura)', ''));
                movimientos.filter(function (m) { return m.tipo === 'Factura' && m.debe > 0; }).forEach(function (m) {
                    selFactura.append(new Option(m.concepto + ' - ' + m.numeroComprobante + ' ($ ' + formatear(m.debe) + ')', m.idMovimiento));
                });

                $('#movimientosModal').modal('show');
            },
            error: function () {
                ModalHelper.notify({ type: 'error', title: 'Error', message: 'No se pudieron cargar los movimientos.' });
            }
        });
    }

    function renderizarMovimientos() {
        var html = '';
        if (!movimientos.length) {
            html = '<tr><td colspan="7" class="text-center text-muted">No hay movimientos.</td></tr>';
        } else {
            movimientos.forEach(function (m) {
                var tipoClass = m.tipo === 'Factura' || m.tipo === 'NotaDebito' ? 'text-danger' : 'text-success';
                html += '<tr>' +
                    '<td>' + new Date(m.fecha).toLocaleDateString('es-AR') + '</td>' +
                    '<td><span class="badge badge-' + (m.tipo === 'Pago' ? 'success' : 'info') + '">' + escapar(m.tipo) + '</span></td>' +
                    '<td>' + escapar(m.concepto) + '</td>' +
                    '<td>' + escapar(m.numeroComprobante || '') + '</td>' +
                    '<td class="text-right ' + (m.debe > 0 ? 'text-danger' : '') + '">' + (m.debe > 0 ? formatear(m.debe) : '') + '</td>' +
                    '<td class="text-right ' + (m.haber > 0 ? 'text-success' : '') + '">' + (m.haber > 0 ? formatear(m.haber) : '') + '</td>' +
                    '<td class="text-right font-weight-bold">' + formatear(m.saldo) + '</td>' +
                    '</tr>';
            });
        }
        $('#movimientosTable tbody').html(html);
    }

    function guardarPago(evento) {
        evento.preventDefault();
        var request = {
            idCliente: parseInt($('#pagoIdCliente').val()),
            idFactura: $('#pagoFactura').val() ? parseInt($('#pagoFactura').val()) : null,
            fecha: $('#pagoFecha').val(),
            monto: parseFloat($('#pagoMonto').val()),
            idMedioPago: parseInt($('#pagoMedioPago').val()),
            numeroCheque: $('#pagoNumeroCheque').val().trim() || null,
            banco: $('#pagoBanco').val().trim() || null,
            fechaVencimientoCheque: $('#pagoFechaVencimientoCheque').val() || null,
            observaciones: $('#pagoObservaciones').val().trim()
        };

        if (!request.fecha || isNaN(request.monto) || request.monto <= 0 || !request.idMedioPago) {
            ModalHelper.notify({ type: 'warning', title: 'Validación', message: 'Complete fecha, monto y medio de pago.' });
            return;
        }

        $('#pagoForm button[type="submit"]').prop('disabled', true);
        $.ajax({
            url: '/CuentasCorrientes/RegistrarPago',
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(request),
            headers: { RequestVerificationToken: token },
            success: function (response) {
                if (response && response.exitoso) {
                    $('#pagoModal').modal('hide');
                    $('#pagoForm')[0].reset();
                    listarClientes();
                    verMovimientos(request.idCliente);
                    ModalHelper.notify({ type: 'success', title: 'Éxito', message: 'Pago registrado correctamente.' });
                } else {
                    ModalHelper.notify({ type: 'error', title: 'Error', message: response ? response.mensaje : 'Error al guardar.' });
                }
            },
            error: function (xhr) {
                var mensaje = xhr.responseJSON && xhr.responseJSON.mensaje;
                ModalHelper.notify({ type: 'error', title: 'Error', message: mensaje || 'No se pudo registrar el pago.' });
            },
            complete: function () { $('#pagoForm button[type="submit"]').prop('disabled', false); }
        });
    }

    function iniciarDesdeUrl() {
        var params = new URLSearchParams(window.location.search);
        var accion = params.get('accion');
        if (accion === 'cobranza') {
            window.history.replaceState({}, document.title, window.location.pathname);
            if (clientesCc.length) {
                abrirCobranzaRapida();
            } else {
                var intervalo = setInterval(function () {
                    if (clientesCc.length) {
                        clearInterval(intervalo);
                        abrirCobranzaRapida();
                    }
                }, 200);
                setTimeout(function () { clearInterval(intervalo); }, 5000);
            }
        }
    }

    function abrirCobranzaRapida() {
        var filtro = $('#ccFiltro').val().toLowerCase();
        var visible = clientesCc.find(function (c) {
            return !filtro || (c.clienteNombre || '').toLowerCase().includes(filtro);
        });
        if (visible) {
            verMovimientos(visible.idCliente);
            setTimeout(function () { $('#btnNuevoPago').trigger('click'); }, 300);
        }
    }

    $(function () {
        cargarMediosPago();
        listarClientes();
        iniciarDesdeUrl();

        $('#ccFiltro').on('input', renderizarClientes);

        $('#clientesCcTable tbody').on('click', '.btn-ver', function () {
            verMovimientos(Number($(this).closest('tr').data('id')));
        });

        $('#btnNuevoPago').on('click', function () {
            $('#pagoFecha').val(new Date().toISOString().split('T')[0]);
            $('#pagoMonto, #pagoObservaciones, #pagoNumeroCheque, #pagoBanco, #pagoFechaVencimientoCheque').val('');
            $('#pagoMedioPago, #pagoFactura').val('');
            $('#chequeFields').hide();
            $('#pagoModal').modal('show');
        });

        $('#pagoMedioPago').on('change', function () {
            var medio = mediosPago.find(function (m) { return m.idMedioPago == $(this).val(); });
            var esCheque = medio && (medio.descripcion || '').toLowerCase().includes('cheque');
            $('#chequeFields').toggle(esCheque);
        });

        $('#pagoForm').on('submit', guardarPago);
    });
})();
