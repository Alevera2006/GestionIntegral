(function () {
    'use strict';

    var apiUrl = '';
    var titulo = '';
    var requiereAfip = false;
    var datos = [];
    var token = $('input[name="__RequestVerificationToken"]').val();
    var avisoGuardado = false;

    function escapeHtml(value) {
        return value == null ? '' : value.toString()
            .replace(/&/g, '&amp;').replace(/</g, '&lt;')
            .replace(/>/g, '&gt;').replace(/"/g, '&quot;');
    }

    function limpiarFormulario() {
        $('#parametroId, #parametroCodigo, #parametroDescripcion, #parametroCodigoAFIP').val('');
        $('#parametroActivo').prop('checked', true);
    }

    function url(accion) {
        return '/Parametros/' + accion + '?apiUrl=' + encodeURIComponent(apiUrl);
    }

    function mostrarFilas() {
        var filtro = $('#parametroFiltroActivo').val();
        var visibles = datos.filter(function (item) {
            return filtro === 'todos' || (filtro === 'activos' ? item.activo : !item.activo);
        });
        var filas = '';
        if (!visibles.length) {
            filas = '<tr><td colspan="5" class="text-center text-muted">No hay registros.</td></tr>';
        } else {
            visibles.forEach(function (item) {
                var id = item.id || item.idProvincia || item.idTipoComprobante || item.idMotivoAnulacion || item.idEstadoPedido || 0;
                var afip = item.codigoAFIP || '';
                filas += '<tr data-id="' + id + '" data-codigo="' + escapeHtml(item.codigo) +
                    '" data-descripcion="' + escapeHtml(item.descripcion) + '" data-afip="' + escapeHtml(afip) +
                    '" data-activo="' + item.activo + '"><td>' + escapeHtml(item.codigo) + '</td><td>' +
                    escapeHtml(item.descripcion) + '</td><td class="col-afip" ' + (requiereAfip ? '' : 'style="display:none;"') +
                    '>' + escapeHtml(afip) + '</td><td><span class="badge badge-' +
                    (item.activo ? 'success">Sí' : 'secondary">No') + '</span></td><td class="table-actions">' +
                    '<button type="button" class="btn btn-info btn-sm btn-editar" title="Editar"><i class="fas fa-pencil-alt"></i></button>' +
                    (item.activo ? '<button type="button" class="btn btn-danger btn-sm btn-eliminar ml-1" title="Eliminar"><i class="fas fa-trash-alt"></i></button>' : '') +
                    '</td></tr>';
            });
        }
        $('#parametroTable tbody').html(filas);
    }

    function cargarGrilla() {
        $.ajax({
            url: url('Listar'), type: 'GET',
            beforeSend: function () {
                $('#parametroTable tbody').html('<tr><td colspan="5" class="text-center">Cargando...</td></tr>');
            },
            success: function (response) {
                if (!response || !response.exitoso) {
                    ModalHelper.notify({ type: 'error', title: 'Error', message: response ? response.mensaje : 'Error al cargar datos.' });
                    return;
                }
                datos = response.datos || [];
                mostrarFilas();
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

    function guardar(e) {
        e.preventDefault();
        var id = $('#parametroId').val();
        var dto = {
            codigo: $('#parametroCodigo').val().trim(),
            descripcion: $('#parametroDescripcion').val().trim(),
            codigoAFIP: requiereAfip ? $('#parametroCodigoAFIP').val().trim() : null,
            activo: $('#parametroActivo').is(':checked')
        };
        if (!dto.codigo || !dto.descripcion) {
            ModalHelper.notify({ type: 'warning', title: 'Validación', message: 'Ingrese código y descripción.' });
            return;
        }
        if (id) dto.id = parseInt(id, 10);
        $('#parametroForm button[type="submit"]').prop('disabled', true);
        $.ajax({
            url: url(id ? 'Actualizar' : 'Crear'), type: id ? 'PUT' : 'POST',
            contentType: 'application/json; charset=utf-8', data: JSON.stringify(dto),
            headers: { RequestVerificationToken: token },
            success: function (response) {
                if (response && response.exitoso) {
                    avisoGuardado = true;
                    $('#parametroFormModal').modal('hide');
                } else {
                    ModalHelper.notify({ type: 'error', title: 'Error', message: response ? response.mensaje : 'Error al guardar.' });
                }
            },
            error: function () {
                ModalHelper.notify({ type: 'error', title: 'Error', message: 'No se pudo guardar el registro.' });
            },
            complete: function () { $('#parametroForm button[type="submit"]').prop('disabled', false); }
        });
    }

    function eliminarParametro(id) {
        $.ajax({
            url: url('Eliminar') + '&id=' + id, type: 'DELETE',
            headers: { RequestVerificationToken: token },
            success: function (response) {
                if (response && response.exitoso && response.datos === true) {
                    cargarGrilla();
                    ModalHelper.notify({ type: 'success', title: 'Éxito', message: 'Registro eliminado correctamente.' });
                } else {
                    ModalHelper.notify({ type: 'error', title: 'Error', message: response ? response.mensaje : 'Error al eliminar.' });
                    cargarGrilla();
                }
            },
            error: function () {
                cargarGrilla();
                ModalHelper.notify({ type: 'error', title: 'Error', message: 'No se pudo eliminar el registro.' });
            }
        });
    }

    $(function () {
        $('.btn-administrar').on('click', function () {
            var card = $(this).closest('.tarjeta-parametro');
            apiUrl = card.data('api-url');
            titulo = card.find('.card-title').text().trim();
            requiereAfip = card.data('requiere-afip') === true || card.data('requiere-afip') === 'true';
            $('#parametroModalTitle').text('Administrar ' + titulo);
            $('#parametroModalHeader').removeClass('bg-primary bg-success bg-warning bg-info')
                .addClass(card.find('.card-header').attr('class').replace('card-header', '').trim());
            $('#parametroFiltroActivo').val('todos');
            $('.col-afip').toggle(requiereAfip);
            $('#parametroCodigoAFIPRow').toggle(requiereAfip);
            cargarGrilla();
            $('#parametroModal').modal('show');
        });
        $('#parametroFiltroActivo').on('change', mostrarFilas);
        $('#btnNuevoParametro').on('click', function () {
            limpiarFormulario();
            $('#parametroFormTitle').text('Nuevo - ' + titulo);
            abrirFormulario();
        });
        $('#parametroFormModal').on('hidden.bs.modal', function () {
            var notificar = avisoGuardado;
            avisoGuardado = false;
            $('#parametroModal').one('shown.bs.modal', function () {
                if (notificar) {
                    cargarGrilla();
                    ModalHelper.notify({ type: 'success', title: 'Éxito', message: 'Registro guardado correctamente.' });
                }
            }).modal('show');
        });
        $('#parametroForm').on('submit', guardar);
        $('#parametroTable tbody').on('click', '.btn-editar', function () {
            var row = $(this).closest('tr');
            $('#parametroId').val(row.data('id'));
            $('#parametroCodigo').val(row.data('codigo'));
            $('#parametroDescripcion').val(row.data('descripcion'));
            $('#parametroCodigoAFIP').val(row.data('afip'));
            $('#parametroActivo').prop('checked', row.data('activo') === true);
            $('#parametroFormTitle').text('Editar - ' + titulo);
            abrirFormulario();
        });
        $('#parametroTable tbody').on('click', '.btn-eliminar', function () {
            var row = $(this).closest('tr');
            var id = Number(row.data('id'));
            if (row.data('activo') !== true || !Number.isInteger(id) || id <= 0) return;
            ModalHelper.confirm({
                title: 'Eliminar', message: '¿Confirma que desea eliminar este registro?',
                onConfirm: function () { eliminarParametro(id); }
            });
        });
    });
})();
