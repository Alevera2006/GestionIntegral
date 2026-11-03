(function () {
    'use strict';

    var productos = [];
    var categorias = [];
    var subcategorias = [];
    var token = $('input[name="__RequestVerificationToken"]').val();

    function escapar(valor) {
        return valor == null ? '' : valor.toString().replace(/&/g, '&amp;').replace(/</g, '&lt;')
            .replace(/>/g, '&gt;').replace(/"/g, '&quot;');
    }

    function formatearDecimal(valor) {
        return valor == null ? '' : Number(valor).toLocaleString('es-AR', { minimumFractionDigits: 2, maximumFractionDigits: 2 });
    }

    function cargarCategorias() {
        return $.ajax({
            url: '/Productos/Categorias',
            type: 'GET',
            success: function (response) {
                categorias = response && response.exitoso ? response.datos || [] : [];
                var select = $('#productoCategoria');
                select.empty().append(new Option('Seleccione una categoría', ''));
                categorias.forEach(function (c) {
                    if (c.activo) select.append(new Option(c.descripcion, c.idCategoria));
                });
            }
        });
    }

    function cargarSubCategorias() {
        return $.ajax({
            url: '/Productos/SubCategorias',
            type: 'GET',
            success: function (response) {
                subcategorias = response && response.exitoso ? response.datos || [] : [];
                filtrarSubcategorias($('#productoCategoria').val());
            }
        });
    }

    function filtrarSubcategorias(idCategoria) {
        var select = $('#productoSubCategoria');
        var valActual = select.val();
        select.empty().append(new Option('Seleccione una subcategoría', ''));
        subcategorias.filter(function (s) {
            return s.idCategoria == idCategoria;
        }).forEach(function (s) {
            select.append(new Option(s.descripcion, s.idSubCategoria));
        });
        if (valActual) select.val(valActual);
    }

    function renderizar() {
        var filtro = $('#productoFiltro').val().toLowerCase();
        var visibles = productos.filter(function (p) {
            return !filtro ||
                (p.codigo || '').toLowerCase().includes(filtro) ||
                (p.descripcion || '').toLowerCase().includes(filtro) ||
                (p.categoriaDescripcion || '').toLowerCase().includes(filtro) ||
                (p.subCategoriaDescripcion || '').toLowerCase().includes(filtro);
        });
        var html = '';
        if (!visibles.length) {
            html = '<tr><td colspan="7" class="text-center text-muted">No hay productos.</td></tr>';
        } else {
            visibles.forEach(function (p) {
                html += '<tr data-id="' + p.idProducto + '">' +
                    '<td>' + escapar(p.codigo) + '</td>' +
                    '<td>' + escapar(p.descripcion) + '</td>' +
                    '<td>' + escapar(p.categoriaDescripcion) + '</td>' +
                    '<td>' + escapar(p.subCategoriaDescripcion || '') + '</td>' +
                    '<td>' + formatearDecimal(p.costo) + '</td>' +
                    '<td><span class="badge badge-' + (p.activo ? 'success">Sí' : 'secondary">No') + '</span></td>' +
                    '<td class="table-actions"><button type="button" class="btn btn-info btn-sm btn-editar" title="Editar"><i class="fas fa-pencil-alt"></i></button>' +
                    (p.activo ? '<button type="button" class="btn btn-danger btn-sm btn-eliminar ml-1" title="Eliminar"><i class="fas fa-trash-alt"></i></button>' : '') +
                    '</td></tr>';
            });
        }
        $('#productoTable tbody').html(html);
    }

    function listar() {
        $.ajax({
            url: '/Productos/Listar',
            type: 'GET',
            beforeSend: function () {
                $('#productoTable tbody').html('<tr><td colspan="7" class="text-center">Cargando...</td></tr>');
            },
            success: function (response) {
                if (!response || !response.exitoso) {
                    ModalHelper.notify({ type: 'error', title: 'Error', message: response ? response.mensaje : 'Error al cargar productos.' });
                    return;
                }
                productos = response.datos || [];
                renderizar();
            },
            error: function () {
                ModalHelper.notify({ type: 'error', title: 'Error', message: 'No se pudieron cargar los productos.' });
            }
        });
    }

    function limpiarFormulario() {
        $('#productoId').val('');
        $('#productoCodigo, #productoDescripcion, #productoCodigoBarras, #productoCosto, #productoStockMinimo, #productoStockMaximo, #productoObservaciones').val('');
        $('#productoCategoria').val('');
        $('#productoSubCategoria').val('');
        $('#productoActivo').prop('checked', true);
        filtrarSubcategorias('');
    }

    function abrirFormulario() {
        $('#productoFormModal').modal('show');
    }

    function guardar(evento) {
        evento.preventDefault();
        var id = $('#productoId').val();
        var request = {
            codigo: $('#productoCodigo').val().trim(),
            descripcion: $('#productoDescripcion').val().trim(),
            idCategoria: Number($('#productoCategoria').val()),
            idSubCategoria: $('#productoSubCategoria').val() ? Number($('#productoSubCategoria').val()) : null,
            codigoBarras: $('#productoCodigoBarras').val().trim() || null,
            costo: $('#productoCosto').val() === '' ? null : Number($('#productoCosto').val()),
            stockMinimo: $('#productoStockMinimo').val() === '' ? null : Number($('#productoStockMinimo').val()),
            stockMaximo: $('#productoStockMaximo').val() === '' ? null : Number($('#productoStockMaximo').val()),
            observaciones: $('#productoObservaciones').val().trim() || null
        };

        if (!request.codigo || !request.descripcion) {
            ModalHelper.notify({ type: 'warning', title: 'Validación', message: 'Ingrese código y descripción.' });
            return;
        }
        if (!Number.isInteger(request.idCategoria) || request.idCategoria <= 0) {
            ModalHelper.notify({ type: 'warning', title: 'Validación', message: 'Seleccione una categoría.' });
            return;
        }

        if (id) {
            request.idProducto = parseInt(id, 10);
            request.activo = $('#productoActivo').is(':checked');
        }

        $('#productoForm button[type="submit"]').prop('disabled', true);
        $.ajax({
            url: id ? '/Productos/Actualizar' : '/Productos/Crear',
            type: id ? 'PUT' : 'POST',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(request),
            headers: { RequestVerificationToken: token },
            success: function (response) {
                if (response && response.exitoso) {
                    $('#productoFormModal').modal('hide');
                    listar();
                    ModalHelper.notify({ type: 'success', title: 'Éxito', message: 'Producto guardado correctamente.' });
                } else {
                    ModalHelper.notify({ type: 'error', title: 'Error', message: response ? response.mensaje : 'Error al guardar.' });
                }
            },
            error: function (xhr) {
                var mensaje = xhr.responseJSON && xhr.responseJSON.mensaje;
                ModalHelper.notify({ type: 'error', title: 'Error', message: mensaje || 'No se pudo guardar el producto.' });
            },
            complete: function () { $('#productoForm button[type="submit"]').prop('disabled', false); }
        });
    }

    $(function () {
        $.when(cargarCategorias(), cargarSubCategorias()).always(function () {
            // listo para usar
        });

        listar();

        $('#productoFiltro').on('input', renderizar);

        $('#btnNuevoProducto').on('click', function () {
            limpiarFormulario();
            $('#productoFormTitle').text('Nuevo Producto');
            abrirFormulario();
        });

        $('#productoForm').on('submit', guardar);

        $('#productoCategoria').on('change', function () {
            filtrarSubcategorias($(this).val());
        });

        $('#productoTable tbody').on('click', '.btn-editar', function () {
            var fila = $(this).closest('tr');
            var id = Number(fila.data('id'));
            var producto = productos.find(function (p) { return p.idProducto === id; });
            if (!producto) return;

            $('#productoId').val(producto.idProducto);
            $('#productoCodigo').val(producto.codigo);
            $('#productoDescripcion').val(producto.descripcion);
            $('#productoCategoria').val(producto.idCategoria).trigger('change');
            $('#productoSubCategoria').val(producto.idSubCategoria || '');
            $('#productoCodigoBarras').val(producto.codigoBarras || '');
            $('#productoCosto').val(producto.costo == null ? '' : producto.costo);
            $('#productoStockMinimo').val(producto.stockMinimo == null ? '' : producto.stockMinimo);
            $('#productoStockMaximo').val(producto.stockMaximo == null ? '' : producto.stockMaximo);
            $('#productoObservaciones').val(producto.observaciones || '');
            $('#productoActivo').prop('checked', producto.activo);

            $('#productoFormTitle').text('Editar Producto');
            abrirFormulario();
        });

        $('#productoTable tbody').on('click', '.btn-eliminar', function () {
            var fila = $(this).closest('tr');
            var id = Number(fila.data('id'));
            var producto = productos.find(function (p) { return p.idProducto === id; });
            if (!producto || !producto.activo) return;

            ModalHelper.confirm({
                title: 'Eliminar',
                message: '¿Confirma que desea eliminar el producto?',
                onConfirm: function () {
                    $.ajax({
                        url: '/Productos/Eliminar?id=' + id,
                        type: 'DELETE',
                        headers: { RequestVerificationToken: token },
                        success: function (response) {
                            if (response && response.exitoso && response.datos === true) {
                                listar();
                                ModalHelper.notify({ type: 'success', title: 'Éxito', message: 'Producto eliminado correctamente.' });
                            } else {
                                ModalHelper.notify({ type: 'error', title: 'Error', message: response ? response.mensaje : 'Error al eliminar.' });
                            }
                        },
                        error: function () {
                            ModalHelper.notify({ type: 'error', title: 'Error', message: 'No se pudo eliminar el producto.' });
                        }
                    });
                }
            });
        });
    });
})();
