(function () {
    'use strict';

    const state = {
        proveedores: [],
        condiciones: [],
        provincias: [],
        domicilios: [],
        telefonos: [],
        emails: []
    };

    const notify = (type, message) => {
        if (window.ModalHelper?.notify) {
            window.ModalHelper.notify({ type, title: type === 'error' ? 'Error' : 'Éxito', message });
            return;
        }
        alert(message);
    };

    const first = (value, fallback = '') => (value === null || value === undefined ? fallback : value);
    const toBoolText = value => value ? 'Sí' : 'No';

    const loadCondiciones = async () => {
        const res = await $.getJSON('/Proveedores/CondicionesPago');
        state.condiciones = res?.datos ?? res?.Datos ?? [];
        const $sel = $('#proveedorCondicionPago');
        $sel.empty().append('<option value="">-- Sin condición --</option>');
        state.condiciones.forEach(c => {
            const id = c.idCondicionPago ?? c.IdCondicionPago;
            const desc = c.descripcion ?? c.Descripcion;
            $sel.append(`<option value="${id}">${desc}</option>`);
        });
    };

    const loadProvincias = async () => {
        const res = await $.getJSON('/Proveedores/Provincias');
        state.provincias = res?.datos ?? res?.Datos ?? [];
        const $sel = $('#domProvProvincia');
        $sel.empty().append('<option value="">-- Seleccione --</option>');
        state.provincias.forEach(p => {
            const id = p.idProvincia ?? p.IdProvincia;
            const desc = p.descripcion ?? p.Descripcion;
            $sel.append(`<option value="${id}">${desc}</option>`);
        });
    };

    const loadProveedores = async () => {
        const res = await $.getJSON('/Proveedores/Listar');
        if (!(res?.exitoso ?? res?.Exitoso)) {
            notify('error', res?.mensaje ?? res?.Mensaje ?? 'No se pudieron cargar proveedores.');
            return;
        }
        state.proveedores = res?.datos ?? res?.Datos ?? [];
        renderProveedores();
    };

    const renderProveedores = () => {
        const $tb = $('#proveedorTable tbody');
        $tb.empty();

        const filtro = ($('#proveedorFiltro').val() || '').toString().trim().toLowerCase();
        const visibles = state.proveedores.filter(p => {
            if (!filtro) return true;
            const codigo = (p.codigo ?? p.Codigo ?? '').toString().toLowerCase();
            const razon = (p.razonSocial ?? p.RazonSocial ?? '').toString().toLowerCase();
            const cuit = (p.cuit ?? p.Cuit ?? '').toString().toLowerCase();
            const condicion = (p.condicionPagoDescripcion ?? p.CondicionPagoDescripcion ?? '').toString().toLowerCase();
            return codigo.includes(filtro) || razon.includes(filtro) || cuit.includes(filtro) || condicion.includes(filtro);
        });

        if (!visibles.length) {
            $tb.html('<tr><td colspan="6" class="text-center text-muted">No hay proveedores para el filtro aplicado.</td></tr>');
            return;
        }

        visibles.forEach(p => {
            const id = p.idProveedor ?? p.IdProveedor;
            const codigo = p.codigo ?? p.Codigo;
            const razon = p.razonSocial ?? p.RazonSocial;
            const cuit = first(p.cuit ?? p.Cuit, '-');
            const condicion = first(p.condicionPagoDescripcion ?? p.CondicionPagoDescripcion, '-');
            const activo = p.activo ?? p.Activo;

            $tb.append(`
                <tr>
                    <td>${codigo}</td>
                    <td>${razon}</td>
                    <td>${cuit}</td>
                    <td>${condicion}</td>
                    <td>${toBoolText(activo)}</td>
                    <td class="table-actions">
                        <button class="btn btn-sm btn-primary mr-1 btn-editar" data-id="${id}"><i class="fas fa-edit"></i></button>
                        <button class="btn btn-sm btn-info mr-1 btn-domicilios" data-id="${id}"><i class="fas fa-map-marker-alt"></i></button>
                        <button class="btn btn-sm btn-secondary mr-1 btn-telefonos" data-id="${id}"><i class="fas fa-phone"></i></button>
                        <button class="btn btn-sm btn-warning mr-1 btn-emails" data-id="${id}"><i class="fas fa-envelope"></i></button>
                        <button class="btn btn-sm btn-danger btn-eliminar" data-id="${id}"><i class="fas fa-trash"></i></button>
                    </td>
                </tr>
            `);
        });
    };

    const toggleProveedorContactButtons = id => {
        const enabled = Number(id || 0) > 0;
        $('#btnProveedorDomicilios').prop('disabled', !enabled).data('id', enabled ? Number(id) : 0);
        $('#btnProveedorTelefonos').prop('disabled', !enabled).data('id', enabled ? Number(id) : 0);
        $('#btnProveedorEmails').prop('disabled', !enabled).data('id', enabled ? Number(id) : 0);
    };

    const clearProveedorForm = () => {
        $('#proveedorId').val('');
        $('#proveedorCodigo').val('');
        $('#proveedorRazonSocial').val('');
        $('#proveedorNombreFantasia').val('');
        $('#proveedorCuit').val('');
        $('#proveedorCondicionPago').val('');
        $('#proveedorObservaciones').val('');
        $('#proveedorActivo').prop('checked', true);
        $('#proveedorFormTitle').text('Nuevo Proveedor');
        $('#proveedorContactButtons').hide();
        toggleProveedorContactButtons(0);
    };

    const buildProveedorPayload = () => ({
        codigo: $('#proveedorCodigo').val()?.trim(),
        razonSocial: $('#proveedorRazonSocial').val()?.trim(),
        nombreFantasia: $('#proveedorNombreFantasia').val()?.trim() || null,
        cuit: $('#proveedorCuit').val()?.trim() || null,
        idCondicionPago: $('#proveedorCondicionPago').val() ? Number($('#proveedorCondicionPago').val()) : null,
        observaciones: $('#proveedorObservaciones').val()?.trim() || null
    });

    const saveProveedor = async e => {
        e.preventDefault();

        const id = Number($('#proveedorId').val() || 0);
        const payload = buildProveedorPayload();

        if (!payload.codigo || !payload.razonSocial) {
            notify('error', 'Código y razón social son obligatorios.');
            return;
        }

        const url = id > 0 ? '/Proveedores/Actualizar' : '/Proveedores/Crear';
        const method = id > 0 ? 'PUT' : 'POST';

        if (id > 0) {
            payload.idProveedor = id;
            payload.activo = $('#proveedorActivo').is(':checked');
        }

        const res = await fetch(url, {
            method,
            headers: { 'Content-Type': 'application/json', 'X-Requested-With': 'XMLHttpRequest' },
            body: JSON.stringify(payload)
        });

        const data = await res.json();
        if (!(data?.exitoso ?? data?.Exitoso)) {
            notify('error', data?.mensaje ?? data?.Mensaje ?? 'No se pudo guardar el proveedor.');
            return;
        }

        notify('success', 'Proveedor guardado correctamente.');
        $('#proveedorFormModal').modal('hide');
        clearProveedorForm();
        await loadProveedores();
    };

    const editProveedor = id => {
        const p = state.proveedores.find(x => (x.idProveedor ?? x.IdProveedor) === id);
        if (!p) return;

        $('#proveedorId').val(id);
        $('#proveedorCodigo').val(p.codigo ?? p.Codigo);
        $('#proveedorRazonSocial').val(p.razonSocial ?? p.RazonSocial);
        $('#proveedorNombreFantasia').val(first(p.nombreFantasia ?? p.NombreFantasia));
        $('#proveedorCuit').val(first(p.cuit ?? p.Cuit));
        $('#proveedorCondicionPago').val(first(p.idCondicionPago ?? p.IdCondicionPago, ''));
        $('#proveedorObservaciones').val(first(p.observaciones ?? p.Observaciones));
        $('#proveedorActivo').prop('checked', !!(p.activo ?? p.Activo));
        $('#proveedorFormTitle').text('Editar Proveedor');
        $('#proveedorContactButtons').show();
        toggleProveedorContactButtons(id);
        $('#proveedorFormModal').modal('show');
    };

    const nuevoProveedor = () => {
        clearProveedorForm();
        $('#proveedorFormModal').modal('show');
    };

    const deleteProveedor = async id => {
        if (!confirm('¿Eliminar proveedor?')) return;

        const res = await fetch(`/Proveedores/Eliminar?id=${id}`, {
            method: 'DELETE',
            headers: { 'X-Requested-With': 'XMLHttpRequest' }
        });

        const data = await res.json();
        if (!(data?.exitoso ?? data?.Exitoso)) {
            notify('error', data?.mensaje ?? data?.Mensaje ?? 'No se pudo eliminar el proveedor.');
            return;
        }

        notify('success', 'Proveedor eliminado.');
        await loadProveedores();
    };

    const clearDomicilioForm = () => {
        $('#domProvId').val('');
        $('#domProvDomicilio').val('');
        $('#domProvCiudad').val('');
        $('#domProvLocalidad').val('');
        $('#domProvProvincia').val('');
        $('#domProvCodigoPostal').val('');
    };

    const loadDomicilios = async idProveedor => {
        const res = await $.getJSON(`/Proveedores/ListarDomicilios?idProveedor=${idProveedor}`);
        const list = res?.datos ?? res?.Datos ?? [];
        state.domicilios = list;

        const $tb = $('#domiciliosProveedorTable tbody');
        $tb.empty();

        list.forEach(d => {
            const id = d.idDomicilio ?? d.IdDomicilio;
            const domicilio = d.domicilio ?? d.Domicilio;
            const ciudad = d.ciudad ?? d.Ciudad;
            const prov = first(d.provinciaDescripcion ?? d.ProvinciaDescripcion, String(d.idProvincia ?? d.IdProvincia));
            const cp = first(d.codigoPostal ?? d.CodigoPostal, '-');

            $tb.append(`
                <tr>
                    <td>${domicilio}</td>
                    <td>${ciudad}</td>
                    <td>${prov}</td>
                    <td>${cp}</td>
                    <td class="table-actions">
                        <button class="btn btn-sm btn-primary mr-1 btn-edit-dom" data-id="${id}"><i class="fas fa-edit"></i></button>
                        <button class="btn btn-sm btn-danger btn-del-dom" data-id="${id}"><i class="fas fa-trash"></i></button>
                    </td>
                </tr>
            `);
        });
    };

    const saveDomicilio = async () => {
        const idProveedor = Number($('#domProveedorId').val() || 0);
        const id = Number($('#domProvId').val() || 0);

        const payload = {
            idProveedor,
            domicilio: $('#domProvDomicilio').val()?.trim(),
            ciudad: $('#domProvCiudad').val()?.trim(),
            localidad: $('#domProvLocalidad').val()?.trim() || null,
            idProvincia: Number($('#domProvProvincia').val() || 0),
            codigoPostal: $('#domProvCodigoPostal').val()?.trim() || null
        };

        if (!payload.domicilio || !payload.ciudad || payload.idProvincia <= 0) {
            notify('error', 'Complete domicilio, ciudad y provincia.');
            return;
        }

        const url = id > 0 ? '/Proveedores/ActualizarDomicilio' : '/Proveedores/CrearDomicilio';
        const method = id > 0 ? 'PUT' : 'POST';

        if (id > 0) {
            payload.idDomicilio = id;
            payload.activo = true;
        }

        const res = await fetch(url, {
            method,
            headers: { 'Content-Type': 'application/json', 'X-Requested-With': 'XMLHttpRequest' },
            body: JSON.stringify(payload)
        });

        const data = await res.json();
        if (!(data?.exitoso ?? data?.Exitoso)) {
            notify('error', data?.mensaje ?? data?.Mensaje ?? 'No se pudo guardar el domicilio.');
            return;
        }

        clearDomicilioForm();
        await loadDomicilios(idProveedor);
    };

    const deleteDomicilio = async id => {
        const idProveedor = Number($('#domProveedorId').val() || 0);
        if (!confirm('¿Eliminar domicilio?')) return;

        const res = await fetch(`/Proveedores/EliminarDomicilio?id=${id}`, {
            method: 'DELETE',
            headers: { 'X-Requested-With': 'XMLHttpRequest' }
        });

        const data = await res.json();
        if (!(data?.exitoso ?? data?.Exitoso)) {
            notify('error', data?.mensaje ?? data?.Mensaje ?? 'No se pudo eliminar el domicilio.');
            return;
        }

        await loadDomicilios(idProveedor);
    };

    const clearTelefonoForm = () => {
        $('#telProvId').val('');
        $('#telProvNumero').val('');
        $('#telProvTipo').val('Fijo');
    };

    const loadTelefonos = async idProveedor => {
        const res = await $.getJSON(`/Proveedores/ListarTelefonos?idProveedor=${idProveedor}`);
        const list = res?.datos ?? res?.Datos ?? [];
        state.telefonos = list;

        const $tb = $('#telefonosProveedorTable tbody');
        $tb.empty();

        list.forEach(t => {
            const id = t.idTelefono ?? t.IdTelefono;
            const numero = t.numeroTelefono ?? t.NumeroTelefono;
            const tipo = t.tipoTelefono ?? t.TipoTelefono;

            $tb.append(`
                <tr>
                    <td>${numero}</td>
                    <td>${tipo}</td>
                    <td class="table-actions">
                        <button class="btn btn-sm btn-primary mr-1 btn-edit-tel" data-id="${id}"><i class="fas fa-edit"></i></button>
                        <button class="btn btn-sm btn-danger btn-del-tel" data-id="${id}"><i class="fas fa-trash"></i></button>
                    </td>
                </tr>
            `);
        });
    };

    const saveTelefono = async () => {
        const idProveedor = Number($('#telProveedorId').val() || 0);
        const id = Number($('#telProvId').val() || 0);

        const payload = {
            idProveedor,
            numeroTelefono: $('#telProvNumero').val()?.trim(),
            tipoTelefono: $('#telProvTipo').val()
        };

        if (!payload.numeroTelefono) {
            notify('error', 'Ingrese un número de teléfono.');
            return;
        }

        const url = id > 0 ? '/Proveedores/ActualizarTelefono' : '/Proveedores/CrearTelefono';
        const method = id > 0 ? 'PUT' : 'POST';

        if (id > 0) {
            payload.idTelefono = id;
            payload.activo = true;
        }

        const res = await fetch(url, {
            method,
            headers: { 'Content-Type': 'application/json', 'X-Requested-With': 'XMLHttpRequest' },
            body: JSON.stringify(payload)
        });

        const data = await res.json();
        if (!(data?.exitoso ?? data?.Exitoso)) {
            notify('error', data?.mensaje ?? data?.Mensaje ?? 'No se pudo guardar el teléfono.');
            return;
        }

        clearTelefonoForm();
        await loadTelefonos(idProveedor);
    };

    const deleteTelefono = async id => {
        const idProveedor = Number($('#telProveedorId').val() || 0);
        if (!confirm('¿Eliminar teléfono?')) return;

        const res = await fetch(`/Proveedores/EliminarTelefono?id=${id}`, {
            method: 'DELETE',
            headers: { 'X-Requested-With': 'XMLHttpRequest' }
        });

        const data = await res.json();
        if (!(data?.exitoso ?? data?.Exitoso)) {
            notify('error', data?.mensaje ?? data?.Mensaje ?? 'No se pudo eliminar el teléfono.');
            return;
        }

        await loadTelefonos(idProveedor);
    };

    const clearEmailForm = () => {
        $('#emailProvId').val('');
        $('#emailProvValor').val('');
    };

    const loadEmails = async idProveedor => {
        const res = await $.getJSON(`/Proveedores/ListarEmails?idProveedor=${idProveedor}`);
        const list = res?.datos ?? res?.Datos ?? [];
        state.emails = list;

        const $tb = $('#emailsProveedorTable tbody');
        $tb.empty();

        list.forEach(m => {
            const id = m.idEmail ?? m.IdEmail;
            const email = m.email ?? m.Email;

            $tb.append(`
                <tr>
                    <td>${email}</td>
                    <td class="table-actions">
                        <button class="btn btn-sm btn-primary mr-1 btn-edit-email" data-id="${id}"><i class="fas fa-edit"></i></button>
                        <button class="btn btn-sm btn-danger btn-del-email" data-id="${id}"><i class="fas fa-trash"></i></button>
                    </td>
                </tr>
            `);
        });
    };

    const saveEmail = async () => {
        const idProveedor = Number($('#emailProveedorId').val() || 0);
        const id = Number($('#emailProvId').val() || 0);

        const payload = {
            idProveedor,
            email: $('#emailProvValor').val()?.trim()
        };

        if (!payload.email) {
            notify('error', 'Ingrese un email.');
            return;
        }

        const url = id > 0 ? '/Proveedores/ActualizarEmail' : '/Proveedores/CrearEmail';
        const method = id > 0 ? 'PUT' : 'POST';

        if (id > 0) {
            payload.idEmail = id;
            payload.activo = true;
        }

        const res = await fetch(url, {
            method,
            headers: { 'Content-Type': 'application/json', 'X-Requested-With': 'XMLHttpRequest' },
            body: JSON.stringify(payload)
        });

        const data = await res.json();
        if (!(data?.exitoso ?? data?.Exitoso)) {
            notify('error', data?.mensaje ?? data?.Mensaje ?? 'No se pudo guardar el email.');
            return;
        }

        clearEmailForm();
        await loadEmails(idProveedor);
    };

    const deleteEmail = async id => {
        const idProveedor = Number($('#emailProveedorId').val() || 0);
        if (!confirm('¿Eliminar email?')) return;

        const res = await fetch(`/Proveedores/EliminarEmail?id=${id}`, {
            method: 'DELETE',
            headers: { 'X-Requested-With': 'XMLHttpRequest' }
        });

        const data = await res.json();
        if (!(data?.exitoso ?? data?.Exitoso)) {
            notify('error', data?.mensaje ?? data?.Mensaje ?? 'No se pudo eliminar el email.');
            return;
        }

        await loadEmails(idProveedor);
    };

    const bindEvents = () => {
        Promise.all([loadCondiciones(), loadProvincias(), loadProveedores()]);

        $('#proveedorFiltro').on('input', renderProveedores);
        $('#btnNuevoProveedor').on('click', nuevoProveedor);
        $('#proveedorForm').on('submit', saveProveedor);

        $('#proveedorFormModal').on('hidden.bs.modal', () => {
            clearProveedorForm();
        });

        $('#proveedorTable').on('click', '.btn-editar', function () {
            editProveedor(Number($(this).data('id')));
        });

        $('#proveedorTable').on('click', '.btn-eliminar', function () {
            deleteProveedor(Number($(this).data('id')));
        });

        $('#proveedorTable').on('click', '.btn-domicilios', async function () {
            const id = Number($(this).data('id'));
            $('#domProveedorId').val(id);
            clearDomicilioForm();
            $('#proveedorDomiciliosModal').modal('show');
            await loadDomicilios(id);
        });

        $('#proveedorTable').on('click', '.btn-telefonos', async function () {
            const id = Number($(this).data('id'));
            $('#telProveedorId').val(id);
            clearTelefonoForm();
            $('#proveedorTelefonosModal').modal('show');
            await loadTelefonos(id);
        });

        $('#proveedorTable').on('click', '.btn-emails', async function () {
            const id = Number($(this).data('id'));
            $('#emailProveedorId').val(id);
            clearEmailForm();
            $('#proveedorEmailsModal').modal('show');
            await loadEmails(id);
        });

        $('#btnGuardarDomicilioProv').on('click', saveDomicilio);
        $('#btnGuardarTelefonoProv').on('click', saveTelefono);
        $('#btnGuardarEmailProv').on('click', saveEmail);

        $('#domiciliosProveedorTable').on('click', '.btn-edit-dom', function () {
            const id = Number($(this).data('id'));
            const d = state.domicilios.find(x => (x.idDomicilio ?? x.IdDomicilio) === id);
            if (!d) return;

            $('#domProvId').val(d.idDomicilio ?? d.IdDomicilio);
            $('#domProvDomicilio').val(d.domicilio ?? d.Domicilio);
            $('#domProvCiudad').val(d.ciudad ?? d.Ciudad);
            $('#domProvLocalidad').val(first(d.localidad ?? d.Localidad));
            $('#domProvProvincia').val(d.idProvincia ?? d.IdProvincia);
            $('#domProvCodigoPostal').val(first(d.codigoPostal ?? d.CodigoPostal));
        });

        $('#domiciliosProveedorTable').on('click', '.btn-del-dom', function () {
            deleteDomicilio(Number($(this).data('id')));
        });

        $('#telefonosProveedorTable').on('click', '.btn-edit-tel', function () {
            const id = Number($(this).data('id'));
            const t = state.telefonos.find(x => (x.idTelefono ?? x.IdTelefono) === id);
            if (!t) return;

            $('#telProvId').val(t.idTelefono ?? t.IdTelefono);
            $('#telProvNumero').val(t.numeroTelefono ?? t.NumeroTelefono);
            $('#telProvTipo').val(t.tipoTelefono ?? t.TipoTelefono);
        });

        $('#telefonosProveedorTable').on('click', '.btn-del-tel', function () {
            deleteTelefono(Number($(this).data('id')));
        });

        $('#emailsProveedorTable').on('click', '.btn-edit-email', function () {
            const id = Number($(this).data('id'));
            const m = state.emails.find(x => (x.idEmail ?? x.IdEmail) === id);
            if (!m) return;

            $('#emailProvId').val(m.idEmail ?? m.IdEmail);
            $('#emailProvValor').val(m.email ?? m.Email);
        });

        $('#emailsProveedorTable').on('click', '.btn-del-email', function () {
            deleteEmail(Number($(this).data('id')));
        });
    };

    $(bindEvents);
})();
