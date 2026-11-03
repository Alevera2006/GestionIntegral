(function () {
    'use strict';

    var apiUrl = '';
    var titulo = '';
    var requiereAfip = false;
    var registros = [];
    var token = $('input[name="__RequestVerificationToken"]').val();
    var guardado = false;

    function escapar(valor) {
        return valor == null ? '' : valor.toString().replace(/&/g, '&amp;').replace(/</g, '&lt;')
            .replace(/>/g, '&gt;').replace(/"/g, '&quot;');
    }

    function esAlicuota() {
        return apiUrl.toLowerCase() === 'api/alicuotasimpuesto';
    }

    function esSubCategoria() {
        return apiUrl.toLowerCase() === 'api/subcategorias';
    }

    function esMovimientoTipo() {
        return apiUrl.toLowerCase() === 'api/tiposmovimientocaja' || apiUrl.toLowerCase() === 'api/tiposmovimientostock';
    }

    function limpiarFormulario() {
        $('#parametroId, #parametroCodigo, #parametroDescripcion, #parametroCodigoAFIP, #parametroIdTipoImpuesto, #parametroIdCategoria').val('');
        $('#parametroPorcentaje').val('0');
        $('#parametroActivo').prop('checked', true);
        $('input[name="signo"]').prop('checked', false);
    }

    function urlAccion(accion) {
        return '/Parametros/' + accion + '?apiUrl=' + encodeURIComponent(apiUrl);
    }

    function cargarTiposImpuesto(idSeleccionado) {
        var select = $('#parametroIdTipoImpuesto');
        select.empty().append(new Option('Seleccione un tipo de impuesto', '')).prop('disabled', true);
        $.ajax({
            url: '/Parametros/Listar?apiUrl=' + encodeURIComponent('api/tiposimpuesto'),
            type: 'GET',
            success: function (response) {
                if (!response || !response.exitoso) {
                    ModalHelper.notify({ type: 'error', title: 'Error', message: response ? response.mensaje : 'No se pudieron cargar los tipos de impuesto.' });
                    return;
                }
                (response.datos || []).forEach(function (tipo) {
                    select.append(new Option(tipo.descripcion, tipo.idTipoImpuesto));
                });
                select.val(idSeleccionado ? String(idSeleccionado) : '');
            },
            error: function () {
                ModalHelper.notify({ type: 'error', title: 'Error', message: 'No se pudieron cargar los tipos de impuesto.' });
            },
            complete: function () { select.prop('disabled', false); }
        });
    }

    function cargarCategorias(idSeleccionado) {
        var select = $('#parametroIdCategoria');
        select.empty().append(new Option('Seleccione una categoría', '')).prop('disabled', true);
        $.ajax({
            url: '/Parametros/Listar?apiUrl=' + encodeURIComponent('api/categorias'),
            type: 'GET',
            success: function (response) {
                if (!response || !response.exitoso) {
                    ModalHelper.notify({ type: 'error', title: 'Error', message: response ? response.mensaje : 'No se pudieron cargar las categorías.' });
                    return;
                }
                (response.datos || []).forEach(function (categoria) {
                    if (categoria.activo || categoria.idCategoria === Number(idSeleccionado)) {
                        select.append(new Option(categoria.descripcion, categoria.idCategoria));
                    }
                });
                select.val(idSeleccionado ? String(idSeleccionado) : '');
                select.prop('disabled', false);
            },
            error: function () {
                ModalHelper.notify({ type: 'error', title: 'Error', message: 'No se pudieron cargar las categorías.' });
            }
        });
    }

    function renderizar() {
        var filtro = $('#parametroFiltroActivo').val();
        var visibles = registros.filter(function (item) {
            return filtro === 'todos' || (filtro === 'activos' ? item.activo : !item.activo);
        });
        var html = '';
        if (!visibles.length) {
            html = '<tr><td colspan="' + (esSubCategoria() ? 6 : 5) + '" class="text-center text-muted">No hay registros.</td></tr>';
        } else {
            visibles.forEach(function (item) {
                var id = item.id || item.idProvincia || item.idTipoComprobante || item.idMotivoAnulacion ||
                    item.idEstadoPedido || item.idEstadoFactura || item.idEstadoCompra || item.idEstadoProceso ||
                    item.idCondicionPago || item.idCondicionIva || item.idCondicionIVA || item.idTipoImpuesto ||
                    item.idAlicuotaImpuesto || item.idMoneda || item.idMedioPago || item.idTipoMovimientoCaja ||
                    item.idEstadoCaja || item.idDeposito || item.idTipoMovimientoStock || item.idCategoria ||
                    item.idSubCategoria || item.idListaPrecio || 0;
                var afip = item.codigoAFIP || '';
                html += '<tr data-id="' + id + '" data-codigo="' + escapar(item.codigo) +
                    '" data-descripcion="' + escapar(item.descripcion) + '" data-afip="' + escapar(afip) +
                    '" data-activo="' + item.activo + '" data-id-categoria="' + (item.idCategoria || 0) +
                    '" data-id-tipo-impuesto="' + (item.idTipoImpuesto || 0) +
                    '" data-porcentaje="' + (item.porcentaje == null ? 0 : item.porcentaje) + '">' +
                    '<td>' + escapar(item.codigo) + '</td><td>' + escapar(item.descripcion) + '</td>' +
                    (esSubCategoria() ? '<td>' + escapar(item.categoriaDescripcion) + '</td>' : '') +
                    '<td class="col-afip" ' + (requiereAfip ? '' : 'style="display:none;"') + '>' + escapar(afip) + '</td>' +
                    '<td><span class="badge badge-' + (item.activo ? 'success">Sí' : 'secondary">No') + '</span></td>' +
                    '<td class="table-actions"><button type="button" class="btn btn-info btn-sm btn-editar" title="Editar"><i class="fas fa-pencil-alt"></i></button>' +
                    (item.activo ? '<button type="button" class="btn btn-danger btn-sm btn-eliminar ml-1" title="Eliminar"><i class="fas fa-trash-alt"></i></button>' : '') +
                    '</td></tr>';
            });
        }
        $('#parametroTable tbody').html(html);
    }

    function listar() {
        $.ajax({
            url: urlAccion('Listar'),
            type: 'GET',
            beforeSend: function () {
                $('#parametroTable tbody').html('<tr><td colspan="' + (esSubCategoria() ? 6 : 5) + '" class="text-center">Cargando...</td></tr>');
            },
            success: function (response) {
                if (!response || !response.exitoso) {
                    ModalHelper.notify({ type: 'error', title: 'Error', message: response ? response.mensaje : 'Error al cargar datos.' });
                    return;
                }
                registros = response.datos || [];
                renderizar();
            },
            error: function () {
                ModalHelper.notify({ type: 'error', title: 'Error', message: 'No se pudieron cargar los datos.' });
            }
        });
    }

    function abrirFormulario() {
        $('#parametroModal').one('hidden.bs.modal', function () {
            $('#parametroFormModal').modal('show');
        }).modal('hide');
    }

    function guardar(evento) {
        evento.preventDefault();
        var id = $('#parametroId').val();
        var request = {
            codigo: $('#parametroCodigo').val().trim(),
            descripcion: $('#parametroDescripcion').val().trim(),
            codigoAFIP: requiereAfip ? $('#parametroCodigoAFIP').val().trim() : null,
            activo: $('#parametroActivo').is(':checked')
        };
        if (!request.codigo || !request.descripcion) {
            ModalHelper.notify({ type: 'warning', title: 'Validación', message: 'Ingrese código y descripción.' });
            return;
        }
        if (id) request.id = parseInt(id, 10);
        if (esSubCategoria()) {
            request.idCategoria = Number($('#parametroIdCategoria').val());
            if ($('#parametroIdCategoria').prop('disabled') || !Number.isInteger(request.idCategoria) || request.idCategoria <= 0) {
                ModalHelper.notify({ type: 'warning', title: 'Validación', message: 'Seleccione una categoría válida.' });
                return;
            }
        }
        if (esAlicuota()) {
            request.idTipoImpuesto = Number($('#parametroIdTipoImpuesto').val());
            request.porcentaje = Number($('#parametroPorcentaje').val());
            if (!Number.isInteger(request.idTipoImpuesto) || request.idTipoImpuesto <= 0 ||
                $('#parametroPorcentaje').val() === '' || !Number.isFinite(request.porcentaje) || request.porcentaje < 0) {
                ModalHelper.notify({ type: 'warning', title: 'Validación', message: 'Seleccione un tipo de impuesto e ingrese un porcentaje válido.' });
                return;
            }
        }
        if (esMovimientoTipo()) {
            var signoSeleccionado = $('input[name="signo"]:checked').val();
            if (signoSeleccionado === undefined) {
                ModalHelper.notify({ type: 'warning', title: 'Validación', message: 'Seleccione un signo (Positivo o Negativo).' });
                return;
            }
            request.signo = signoSeleccionado === 'positivo' ? 1 : -1;
        }
        $('#parametroForm button[type="submit"]').prop('disabled', true);
        $.ajax({
            url: urlAccion(id ? 'Actualizar' : 'Crear'),
            type: id ? 'PUT' : 'POST',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(request),
            headers: { RequestVerificationToken: token },
            success: function (response) {
                if (response && response.exitoso) {
                    guardado = true;
                    $('#parametroFormModal').modal('hide');
                } else {
                    ModalHelper.notify({ type: 'error', title: 'Error', message: response ? response.mensaje : 'Error al guardar.' });
                }
            },
            error: function (xhr) {
                var mensaje = xhr.responseJSON && xhr.responseJSON.mensaje;
                ModalHelper.notify({ type: 'error', title: 'Error', message: mensaje || 'No se pudo guardar el registro.' });
            },
            complete: function () { $('#parametroForm button[type="submit"]').prop('disabled', false); }
        });
    }

    function eliminar(id) {
        $.ajax({
            url: urlAccion('Eliminar') + '&id=' + id,
            type: 'DELETE',
            headers: { RequestVerificationToken: token },
            success: function (response) {
                if (response && response.exitoso && response.datos === true) {
                    listar();
                    ModalHelper.notify({ type: 'success', title: 'Éxito', message: 'Registro eliminado correctamente.' });
                } else {
                    ModalHelper.notify({ type: 'error', title: 'Error', message: response ? response.mensaje : 'Error al eliminar.' });
                    listar();
                }
            },
            error: function () {
                listar();
                ModalHelper.notify({ type: 'error', title: 'Error', message: 'No se pudo eliminar el registro.' });
            }
        });
    }

    $(function () {
        var grupoInicial = $('#gruposParametros').data('grupo-inicial') || 'parametricos';
        if (grupoInicial && grupoInicial !== 'parametricos') {
            $('#gruposParametros button[data-grupo="' + grupoInicial + '"]').trigger('click');
        }

        $('#gruposParametros button').on('click', function () {
            var boton = $(this);
            var grupo = boton.data('grupo');
            $('#gruposParametros button').removeClass('btn-primary active').addClass('btn-outline-primary').attr('aria-pressed', 'false');
            boton.removeClass('btn-outline-primary').addClass('btn-primary active').attr('aria-pressed', 'true');
            $('#tarjetasParametros > [data-grupo]').each(function () {
                this.hidden = this.getAttribute('data-grupo') !== grupo;
            });
        });
        $('.btn-administrar').on('click', function () {
            var tarjeta = $(this).closest('.tarjeta-parametro');
            apiUrl = tarjeta.data('api-url');
            titulo = tarjeta.find('.inner p').text().trim();
            requiereAfip = tarjeta.data('requiere-afip') === true || tarjeta.data('requiere-afip') === 'true';
            $('#parametroModalTitle').text('Administrar ' + titulo);
            $('#parametroModalHeader').removeClass('bg-primary bg-success bg-warning bg-info').addClass('bg-primary');
            $('#parametroFiltroActivo').val('todos');
            $('.col-afip').toggle(requiereAfip);
            $('.col-categoria').toggle(esSubCategoria());
            $('#parametroCodigoAFIPRow').toggle(requiereAfip);
            $('#parametroAlicuotaRow').toggle(esAlicuota());
            $('#parametroCategoriaRow').toggle(esSubCategoria());
            $('#parametroIdCategoria').prop('required', esSubCategoria()).prop('disabled', true);
            $('#grupoSigno').toggle(esMovimientoTipo());
            listar();
            $('#parametroModal').modal('show');
        });
        $('#parametroFiltroActivo').on('change', renderizar);
        $('#btnNuevoParametro').on('click', function () {
            limpiarFormulario();
            if (esAlicuota()) cargarTiposImpuesto(null);
            if (esSubCategoria()) cargarCategorias(null);
            $('#parametroFormTitle').text('Nuevo - ' + titulo);
            abrirFormulario();
        });
        $('#parametroFormModal').on('hidden.bs.modal', function () {
            var actualizado = guardado;
            guardado = false;
            $('#parametroModal').one('shown.bs.modal', function () {
                if (actualizado) {
                    listar();
                    ModalHelper.notify({ type: 'success', title: 'Éxito', message: 'Registro guardado correctamente.' });
                }
            }).modal('show');
        });
        $('#parametroForm').on('submit', guardar);
        $('#parametroTable tbody').on('click', '.btn-editar', function () {
            var fila = $(this).closest('tr');
            $('#parametroId').val(fila.data('id'));
            $('#parametroCodigo').val(fila.data('codigo'));
            $('#parametroDescripcion').val(fila.data('descripcion'));
            $('#parametroCodigoAFIP').val(fila.data('afip'));
            $('#parametroActivo').prop('checked', fila.data('activo') === true || fila.data('activo') === 'true');
            if (esAlicuota()) {
                $('#parametroPorcentaje').val(fila.data('porcentaje'));
                cargarTiposImpuesto(fila.data('id-tipo-impuesto'));
            }
            if (esSubCategoria()) cargarCategorias(fila.data('id-categoria'));
            if (esMovimientoTipo()) {
                $('input[name="signo"][value="' + (fila.data('signo') === 1 ? 'positivo' : 'negativo') + '"]').prop('checked', true);
            }
            $('#parametroFormTitle').text('Editar - ' + titulo);
            abrirFormulario();
        });
        $('#parametroTable tbody').on('click', '.btn-eliminar', function () {
            var fila = $(this).closest('tr');
            var id = Number(fila.data('id'));
            if (fila.data('activo') !== true && fila.data('activo') !== 'true') return;
            if (!Number.isInteger(id) || id <= 0) return;
            ModalHelper.confirm({
                title: 'Eliminar',
                message: '¿Confirma que desea eliminar este registro?',
                onConfirm: function () { eliminar(id); }
            });
        });
    });
})();
