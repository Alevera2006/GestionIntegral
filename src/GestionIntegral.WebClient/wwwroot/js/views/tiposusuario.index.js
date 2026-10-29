(function () {
    'use strict';

    var token = $('input[name="__RequestVerificationToken"]').val();

    function escapeHtml(text) {
        if (text === null || text === undefined) return '';
        return text.toString()
            .replace(/&/g, '&amp;')
            .replace(/</g, '&lt;')
            .replace(/>/g, '&gt;')
            .replace(/"/g, '&quot;');
    }

    function limpiarFormulario() {
        $('#tipoUsuarioId').val('');
        $('#tipoUsuarioDescripcion').val('');
    }

    function cargarGrilla() {
        $.ajax({
            url: '/TiposUsuario/Listar',
            type: 'GET',
            beforeSend: function () {
                $('#tipoUsuarioTable tbody').html('<tr><td colspan="3" class="text-center">Cargando...</td></tr>');
            },
            success: function (response) {
                if (!response || !response.exitoso) {
                    ModalHelper.notify({ type: 'error', title: 'Error', message: response ? response.mensaje : 'Error al cargar tipos de usuario.' });
                    return;
                }

                var datos = response.datos || [];
                var rows = '';

                if (datos.length === 0) {
                    rows = '<tr><td colspan="3" class="text-center text-muted">No hay tipos de usuario registrados.</td></tr>';
                } else {
                    datos.forEach(function (item) {
                        rows += '<tr data-id="' + item.idTipoUsuario + '" data-descripcion="' + escapeHtml(item.descripcion) + '">' +
                            '<td>' + item.idTipoUsuario + '</td>' +
                            '<td>' + escapeHtml(item.descripcion) + '</td>' +
                            '<td class="table-actions">' +
                            '<button type="button" class="btn btn-info btn-sm btn-editar" title="Editar"><i class="fas fa-pencil-alt"></i></button>' +
                            '<button type="button" class="btn btn-danger btn-sm btn-eliminar ml-1" title="Eliminar"><i class="fas fa-trash-alt"></i></button>' +
                            '</td></tr>';
                    });
                }

                $('#tipoUsuarioTable tbody').html(rows);
            },
            error: function () {
                ModalHelper.notify({ type: 'error', title: 'Error', message: 'No se pudieron cargar los tipos de usuario.' });
            }
        });
    }

    function guardarTipoUsuario(e) {
        e.preventDefault();

        var id = $('#tipoUsuarioId').val();
        var descripcion = $('#tipoUsuarioDescripcion').val().trim();

        if (!descripcion) {
            ModalHelper.notify({ type: 'warning', title: 'Validación', message: 'Ingrese una descripción.' });
            return;
        }

        var dto = { descripcion: descripcion };
        var url = '/TiposUsuario/' + (id ? 'Actualizar' : 'Crear');
        var type = id ? 'PUT' : 'POST';

        if (id) {
            dto.idTipoUsuario = parseInt(id);
        }

        $.ajax({
            url: url,
            type: type,
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(dto),
            headers: { 'RequestVerificationToken': token },
            success: function (response) {
                if (response && response.exitoso) {
                    ModalHelper.notify({ type: 'success', title: 'Éxito', message: 'Tipo de usuario guardado correctamente.' });
                    limpiarFormulario();
                    cargarGrilla();
                } else {
                    ModalHelper.notify({ type: 'error', title: 'Error', message: response ? response.mensaje : 'Error al guardar.' });
                }
            },
            error: function () {
                ModalHelper.notify({ type: 'error', title: 'Error', message: 'No se pudo guardar el tipo de usuario.' });
            }
        });
    }

    function eliminarTipoUsuario(id) {
        $.ajax({
            url: '/TiposUsuario/Eliminar?id=' + id,
            type: 'DELETE',
            headers: { 'RequestVerificationToken': token },
            success: function (response) {
                if (response && response.exitoso) {
                    ModalHelper.notify({ type: 'success', title: 'Éxito', message: 'Tipo de usuario eliminado correctamente.' });
                    limpiarFormulario();
                    cargarGrilla();
                } else {
                    ModalHelper.notify({ type: 'error', title: 'Error', message: response ? response.mensaje : 'Error al eliminar.' });
                }
            },
            error: function () {
                ModalHelper.notify({ type: 'error', title: 'Error', message: 'No se pudo eliminar el tipo de usuario.' });
            }
        });
    }

    $(document).ready(function () {
        $('.btn-administrar').on('click', function () {
            limpiarFormulario();
            cargarGrilla();
            $('#tipoUsuarioModal').modal('show');
        });

        $('#btnNuevoTipoUsuario').on('click', limpiarFormulario);

        $('#tipoUsuarioForm').on('submit', guardarTipoUsuario);

        $('#tipoUsuarioTable tbody').on('click', '.btn-editar', function () {
            var $row = $(this).closest('tr');
            $('#tipoUsuarioId').val($row.data('id'));
            $('#tipoUsuarioDescripcion').val($row.data('descripcion'));
        });

        $('#tipoUsuarioTable tbody').on('click', '.btn-eliminar', function () {
            var id = $(this).closest('tr').data('id');
            ModalHelper.confirm({
                title: 'Eliminar tipo de usuario',
                message: '¿Confirma que desea eliminar este tipo de usuario?',
                onConfirm: function () { eliminarTipoUsuario(id); }
            });
        });
    });
})();
