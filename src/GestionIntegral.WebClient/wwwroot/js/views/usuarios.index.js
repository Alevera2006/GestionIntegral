(function () {
    'use strict';

    var token = $('input[name="__RequestVerificationToken"]').val();
    var tiposUsuario = [];

    function escapeHtml(text) {
        if (text === null || text === undefined) return '';
        return text.toString()
            .replace(/&/g, '&amp;')
            .replace(/</g, '&lt;')
            .replace(/>/g, '&gt;')
            .replace(/"/g, '&quot;');
    }

    function limpiarFormulario() {
        $('#usuarioId').val('');
        $('#nombre').val('');
        $('#apellido').val('');
        $('#usuarioNombre').val('').prop('readonly', false);
        $('#clave').val('').prop('required', true).attr('placeholder', 'Contraseña');
        $('#idTipoUsuario').val('');
        $('#activo').prop('checked', true);
        $('#bloqueado').prop('checked', false);
    }

    function cargarTiposUsuario() {
        return $.ajax({
            url: '/Usuarios/TiposUsuario',
            type: 'GET',
            success: function (response) {
                if (response && response.exitoso) {
                    tiposUsuario = response.datos || [];
                    var options = '<option value="">-- Seleccione --</option>';
                    tiposUsuario.forEach(function (t) {
                        options += '<option value="' + t.idTipoUsuario + '">' + escapeHtml(t.descripcion) + '</option>';
                    });
                    $('#idTipoUsuario').html(options);
                }
            }
        });
    }

    function getTipoDescripcion(id) {
        var tipo = tiposUsuario.find(function (t) { return t.idTipoUsuario === id; });
        return tipo ? tipo.descripcion : '';
    }

    function cargarGrilla() {
        $.ajax({
            url: '/Usuarios/Listar',
            type: 'GET',
            beforeSend: function () {
                $('#usuarioTable tbody').html('<tr><td colspan="6" class="text-center">Cargando...</td></tr>');
            },
            success: function (response) {
                if (!response || !response.exitoso) {
                    ModalHelper.notify({ type: 'error', title: 'Error', message: response ? response.mensaje : 'Error al cargar usuarios.' });
                    return;
                }

                var datos = response.datos || [];
                var rows = '';

                if (datos.length === 0) {
                    rows = '<tr><td colspan="6" class="text-center text-muted">No hay usuarios registrados.</td></tr>';
                } else {
                    datos.forEach(function (item) {
                        var activoBadge = item.activo ? '<span class="badge badge-success">Sí</span>' : '<span class="badge badge-secondary">No</span>';
                        var bloqueadoBadge = item.bloqueado ? '<span class="badge badge-danger">Sí</span>' : '<span class="badge badge-secondary">No</span>';

                        rows += '<tr data-id="' + item.idUsuario + '" data-nombre="' + escapeHtml(item.nombre) + '" data-apellido="' + escapeHtml(item.apellido) + '" data-usuario="' + escapeHtml(item.usuarioNombre) + '" data-tipo="' + item.idTipoUsuario + '" data-activo="' + item.activo + '" data-bloqueado="' + item.bloqueado + '">' +
                            '<td>' + escapeHtml(item.usuarioNombre) + '</td>' +
                            '<td>' + escapeHtml(item.nombreCompleto) + '</td>' +
                            '<td>' + escapeHtml(getTipoDescripcion(item.idTipoUsuario)) + '</td>' +
                            '<td>' + activoBadge + '</td>' +
                            '<td>' + bloqueadoBadge + '</td>' +
                            '<td class="table-actions">' +
                            '<button type="button" class="btn btn-info btn-sm btn-editar" title="Editar"><i class="fas fa-pencil-alt"></i></button>' +
                            '<button type="button" class="btn btn-danger btn-sm btn-eliminar ml-1" title="Eliminar"><i class="fas fa-trash-alt"></i></button>' +
                            '</td></tr>';
                    });
                }

                $('#usuarioTable tbody').html(rows);
            },
            error: function () {
                ModalHelper.notify({ type: 'error', title: 'Error', message: 'No se pudieron cargar los usuarios.' });
            }
        });
    }

    function guardarUsuario(e) {
        e.preventDefault();

        var id = $('#usuarioId').val();
        var dto = {
            nombre: $('#nombre').val().trim(),
            apellido: $('#apellido').val().trim(),
            usuarioNombre: $('#usuarioNombre').val().trim(),
            idTipoUsuario: parseInt($('#idTipoUsuario').val()),
            activo: $('#activo').is(':checked'),
            bloqueado: $('#bloqueado').is(':checked')
        };

        if (!dto.nombre || !dto.apellido || !dto.usuarioNombre || !dto.idTipoUsuario) {
            ModalHelper.notify({ type: 'warning', title: 'Validación', message: 'Complete todos los campos obligatorios.' });
            return;
        }

        var url = '/Usuarios/' + (id ? 'Actualizar' : 'Crear');
        var type = id ? 'PUT' : 'POST';

        if (id) {
            dto.idUsuario = parseInt(id);
        } else {
            var clave = $('#clave').val().trim();
            if (!clave) {
                ModalHelper.notify({ type: 'warning', title: 'Validación', message: 'Ingrese una contraseña para el nuevo usuario.' });
                return;
            }
            dto.clave = clave;
        }

        $.ajax({
            url: url,
            type: type,
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(dto),
            headers: { 'RequestVerificationToken': token },
            success: function (response) {
                if (response && response.exitoso) {
                    ModalHelper.notify({ type: 'success', title: 'Éxito', message: 'Usuario guardado correctamente.' });
                    limpiarFormulario();
                    cargarGrilla();
                } else {
                    ModalHelper.notify({ type: 'error', title: 'Error', message: response ? response.mensaje : 'Error al guardar.' });
                }
            },
            error: function () {
                ModalHelper.notify({ type: 'error', title: 'Error', message: 'No se pudo guardar el usuario.' });
            }
        });
    }

    function eliminarUsuario(id) {
        $.ajax({
            url: '/Usuarios/Eliminar?id=' + id,
            type: 'DELETE',
            headers: { 'RequestVerificationToken': token },
            success: function (response) {
                if (response && response.exitoso) {
                    ModalHelper.notify({ type: 'success', title: 'Éxito', message: 'Usuario eliminado correctamente.' });
                    limpiarFormulario();
                    cargarGrilla();
                } else {
                    ModalHelper.notify({ type: 'error', title: 'Error', message: response ? response.mensaje : 'Error al eliminar.' });
                }
            },
            error: function () {
                ModalHelper.notify({ type: 'error', title: 'Error', message: 'No se pudo eliminar el usuario.' });
            }
        });
    }

    $(document).ready(function () {
        $('.btn-administrar').on('click', function () {
            limpiarFormulario();
            cargarTiposUsuario().then(cargarGrilla);
            $('#usuarioModal').modal('show');
        });

        $('#btnNuevoUsuario').on('click', limpiarFormulario);

        $('#usuarioForm').on('submit', guardarUsuario);

        $('#usuarioTable tbody').on('click', '.btn-editar', function () {
            var $row = $(this).closest('tr');
            $('#usuarioId').val($row.data('id'));
            $('#nombre').val($row.data('nombre'));
            $('#apellido').val($row.data('apellido'));
            $('#usuarioNombre').val($row.data('usuario')).prop('readonly', true);
            $('#clave').val('').prop('required', false).attr('placeholder', 'No se modifica');
            $('#idTipoUsuario').val($row.data('tipo'));
            $('#activo').prop('checked', $row.data('activo').toString() === 'true');
            $('#bloqueado').prop('checked', $row.data('bloqueado').toString() === 'true');
        });

        $('#usuarioTable tbody').on('click', '.btn-eliminar', function () {
            var id = $(this).closest('tr').data('id');
            ModalHelper.confirm({
                title: 'Eliminar usuario',
                message: '¿Confirma que desea eliminar este usuario?',
                onConfirm: function () { eliminarUsuario(id); }
            });
        });
    });
})();
