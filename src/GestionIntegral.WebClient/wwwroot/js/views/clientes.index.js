(function () {
    'use strict';

    const state = {
        clientes: [],
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
        const res = await $.getJSON('/Clientes/CondicionesPago');
        state.condiciones = res?.datos ?? res?.Datos ?? [];
        const $sel = $('#clienteCondicionPago');
        $sel.empty().append('<option value="">-- Sin condición --</option>');
        state.condiciones.forEach(c => {
            const id = c.idCondicionPago ?? c.IdCondicionPago;
            const desc = c.descripcion ?? c.Descripcion;
            $sel.append(`<option value="${id}">${desc}</option>`);
        });
    };

    const loadProvincias = async () => {
        const res = await $.getJSON('/Clientes/Provincias');
        state.provincias = res?.datos ?? res?.Datos ?? [];
        const $sel = $('#domProvincia');
        $sel.empty().append('<option value="">-- Seleccione --</option>');
        state.provincias.forEach(p => {
            const id = p.idProvincia ?? p.IdProvincia;
            const desc = p.descripcion ?? p.Descripcion;
            $sel.append(`<option value="${id}">${desc}</option>`);
        });
    };

    const loadClientes = async () => {
        const res = await $.getJSON('/Clientes/Listar');
        if (!(res?.exitoso ?? res?.Exitoso)) {
            notify('error', res?.mensaje ?? res?.Mensaje ?? 'No se pudieron cargar clientes.');
            return;
        }
        state.clientes = res?.datos ?? res?.Datos ?? [];
        renderClientes();
    };

    const renderClientes = () => {
        const $tb = $('#clienteTable tbody');
        $tb.empty();

        const filtro = ($('#clienteFiltro').val() || '').toString().trim().toLowerCase();
        const visibles = state.clientes.filter(c => {
            if (!filtro) return true;
            const codigo = (c.codigo ?? c.Codigo ?? '').toString().toLowerCase();
            const razon = (c.razonSocial ?? c.RazonSocial ?? '').toString().toLowerCase();
            const cuit = (c.cuit ?? c.Cuit ?? '').toString().toLowerCase();
            const condicion = (c.condicionPagoDescripcion ?? c.CondicionPagoDescripcion ?? '').toString().toLowerCase();
            return codigo.includes(filtro) || razon.includes(filtro) || cuit.includes(filtro) || condicion.includes(filtro);
        });

        if (!visibles.length) {
            $tb.html('<tr><td colspan="6" class="text-center text-muted">No hay clientes para el filtro aplicado.</td></tr>');
            return;
        }

        visibles.forEach(c => {
            const id = c.idCliente ?? c.IdCliente;
            const codigo = c.codigo ?? c.Codigo;
            const razon = c.razonSocial ?? c.RazonSocial;
            const cuit = first(c.cuit ?? c.Cuit, '-');
            const condicion = first(c.condicionPagoDescripcion ?? c.CondicionPagoDescripcion, '-');
            const activo = c.activo ?? c.Activo;

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

    const toggleClienteContactButtons = id => {
        const enabled = Number(id || 0) > 0;
        $('#btnClienteDomicilios').prop('disabled', !enabled).data('id', enabled ? Number(id) : 0);
        $('#btnClienteTelefonos').prop('disabled', !enabled).data('id', enabled ? Number(id) : 0);
        $('#btnClienteEmails').prop('disabled', !enabled).data('id', enabled ? Number(id) : 0);
    };

    const clearClienteForm = () => {
        $('#clienteId').val('');
        $('#clienteCodigo').val('');
        $('#clienteRazonSocial').val('');
        $('#clienteNombreFantasia').val('');
        $('#clienteCuit').val('');
        $('#clienteCondicionPago').val('');
        $('#clienteObservaciones').val('');
        $('#clienteActivo').prop('checked', true);
        $('#clienteFormTitle').text('Nuevo Cliente');
        $('#clienteContactButtons').hide();
        toggleClienteContactButtons(0);
    };

    const buildClientePayload = () => ({
        codigo: $('#clienteCodigo').val()?.trim(),
        razonSocial: $('#clienteRazonSocial').val()?.trim(),
        nombreFantasia: $('#clienteNombreFantasia').val()?.trim() || null,
        cuit: $('#clienteCuit').val()?.trim() || null,
        idCondicionPago: $('#clienteCondicionPago').val() ? Number($('#clienteCondicionPago').val()) : null,
        observaciones: $('#clienteObservaciones').val()?.trim() || null
    });

    const saveCliente = async e => {
        e.preventDefault();

        const id = Number($('#clienteId').val() || 0);
        const payload = buildClientePayload();

        if (!payload.codigo || !payload.razonSocial) {
            notify('error', 'Código y razón social son obligatorios.');
            return;
        }

        const url = id > 0 ? '/Clientes/Actualizar' : '/Clientes/Crear';
        const method = id > 0 ? 'PUT' : 'POST';

        if (id > 0) {
            payload.idCliente = id;
            payload.activo = $('#clienteActivo').is(':checked');
        }

        const res = await fetch(url, {
            method,
            headers: { 'Content-Type': 'application/json', 'X-Requested-With': 'XMLHttpRequest' },
            body: JSON.stringify(payload)
        });

        const data = await res.json();
        if (!(data?.exitoso ?? data?.Exitoso)) {
            notify('error', data?.mensaje ?? data?.Mensaje ?? 'No se pudo guardar el cliente.');
            return;
        }

        notify('success', 'Cliente guardado correctamente.');
        $('#clienteFormModal').modal('hide');
        clearClienteForm();
        await loadClientes();
    };

    const editCliente = id => {
        const c = state.clientes.find(x => (x.idCliente ?? x.IdCliente) === id);
        if (!c) return;

        $('#clienteId').val(id);
        $('#clienteCodigo').val(c.codigo ?? c.Codigo);
        $('#clienteRazonSocial').val(c.razonSocial ?? c.RazonSocial);
        $('#clienteNombreFantasia').val(first(c.nombreFantasia ?? c.NombreFantasia));
        $('#clienteCuit').val(first(c.cuit ?? c.Cuit));
        $('#clienteCondicionPago').val(first(c.idCondicionPago ?? c.IdCondicionPago, ''));
        $('#clienteObservaciones').val(first(c.observaciones ?? c.Observaciones));
        $('#clienteActivo').prop('checked', !!(c.activo ?? c.Activo));
        $('#clienteFormTitle').text('Editar Cliente');
        $('#clienteContactButtons').show();
        toggleClienteContactButtons(id);
        $('#clienteFormModal').modal('show');
    };

    const nuevoCliente = () => {
        clearClienteForm();
        $('#clienteFormModal').modal('show');
    };

    const deleteCliente = async id => {
        if (!confirm('¿Eliminar cliente?')) return;

        const res = await fetch(`/Clientes/Eliminar?id=${id}`, {
            method: 'DELETE',
            headers: { 'X-Requested-With': 'XMLHttpRequest' }
        });

        const data = await res.json();
        if (!(data?.exitoso ?? data?.Exitoso)) {
            notify('error', data?.mensaje ?? data?.Mensaje ?? 'No se pudo eliminar el cliente.');
            return;
        }

        notify('success', 'Cliente eliminado.');
        await loadClientes();
    };

    const clearDomicilioForm = () => {
        $('#domId').val('');
        $('#domDomicilio').val('');
        $('#domCiudad').val('');
        $('#domLocalidad').val('');
        $('#domProvincia').val('');
        $('#domCodigoPostal').val('');
    };

    const loadDomicilios = async idCliente => {
        const res = await $.getJSON(`/Clientes/ListarDomicilios?idCliente=${idCliente}`);
        const list = res?.datos ?? res?.Datos ?? [];
        state.domicilios = list;

        const $tb = $('#domiciliosClienteTable tbody');
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
        const idCliente = Number($('#domClienteId').val() || 0);
        const id = Number($('#domId').val() || 0);

        const payload = {
            idCliente,
            domicilio: $('#domDomicilio').val()?.trim(),
            ciudad: $('#domCiudad').val()?.trim(),
            localidad: $('#domLocalidad').val()?.trim() || null,
            idProvincia: Number($('#domProvincia').val() || 0),
            codigoPostal: $('#domCodigoPostal').val()?.trim() || null
        };

        if (!payload.domicilio || !payload.ciudad || payload.idProvincia <= 0) {
            notify('error', 'Complete domicilio, ciudad y provincia.');
            return;
        }

        const url = id > 0 ? '/Clientes/ActualizarDomicilio' : '/Clientes/CrearDomicilio';
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
        await loadDomicilios(idCliente);
    };

    const deleteDomicilio = async id => {
        const idCliente = Number($('#domClienteId').val() || 0);
        if (!confirm('¿Eliminar domicilio?')) return;

        const res = await fetch(`/Clientes/EliminarDomicilio?id=${id}`, {
            method: 'DELETE',
            headers: { 'X-Requested-With': 'XMLHttpRequest' }
        });

        const data = await res.json();
        if (!(data?.exitoso ?? data?.Exitoso)) {
            notify('error', data?.mensaje ?? data?.Mensaje ?? 'No se pudo eliminar el domicilio.');
            return;
        }

        await loadDomicilios(idCliente);
    };

    const clearTelefonoForm = () => {
        $('#telId').val('');
        $('#telNumero').val('');
        $('#telTipo').val('Fijo');
    };

    const loadTelefonos = async idCliente => {
        const res = await $.getJSON(`/Clientes/ListarTelefonos?idCliente=${idCliente}`);
        const list = res?.datos ?? res?.Datos ?? [];
        state.telefonos = list;

        const $tb = $('#telefonosClienteTable tbody');
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
        const idCliente = Number($('#telClienteId').val() || 0);
        const id = Number($('#telId').val() || 0);

        const payload = {
            idCliente,
            numeroTelefono: $('#telNumero').val()?.trim(),
            tipoTelefono: $('#telTipo').val()
        };

        if (!payload.numeroTelefono) {
            notify('error', 'Ingrese un número de teléfono.');
            return;
        }

        const url = id > 0 ? '/Clientes/ActualizarTelefono' : '/Clientes/CrearTelefono';
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
        await loadTelefonos(idCliente);
    };

    const deleteTelefono = async id => {
        const idCliente = Number($('#telClienteId').val() || 0);
        if (!confirm('¿Eliminar teléfono?')) return;

        const res = await fetch(`/Clientes/EliminarTelefono?id=${id}`, {
            method: 'DELETE',
            headers: { 'X-Requested-With': 'XMLHttpRequest' }
        });

        const data = await res.json();
        if (!(data?.exitoso ?? data?.Exitoso)) {
            notify('error', data?.mensaje ?? data?.Mensaje ?? 'No se pudo eliminar el teléfono.');
            return;
        }

        await loadTelefonos(idCliente);
    };

    const clearEmailForm = () => {
        $('#emailId').val('');
        $('#emailValor').val('');
    };

    const loadEmails = async idCliente => {
        const res = await $.getJSON(`/Clientes/ListarEmails?idCliente=${idCliente}`);
        const list = res?.datos ?? res?.Datos ?? [];
        state.emails = list;

        const $tb = $('#emailsClienteTable tbody');
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
        const idCliente = Number($('#emailClienteId').val() || 0);
        const id = Number($('#emailId').val() || 0);

        const payload = {
            idCliente,
            email: $('#emailValor').val()?.trim()
        };

        if (!payload.email) {
            notify('error', 'Ingrese un email.');
            return;
        }

        const url = id > 0 ? '/Clientes/ActualizarEmail' : '/Clientes/CrearEmail';
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
        await loadEmails(idCliente);
    };

    const deleteEmail = async id => {
        const idCliente = Number($('#emailClienteId').val() || 0);
        if (!confirm('¿Eliminar email?')) return;

        const res = await fetch(`/Clientes/EliminarEmail?id=${id}`, {
            method: 'DELETE',
            headers: { 'X-Requested-With': 'XMLHttpRequest' }
        });

        const data = await res.json();
        if (!(data?.exitoso ?? data?.Exitoso)) {
            notify('error', data?.mensaje ?? data?.Mensaje ?? 'No se pudo eliminar el email.');
            return;
        }

        await loadEmails(idCliente);
    };

    const bindEvents = () => {
        $('#btnAbrirClientes').on('click', async () => {
            $('#clienteModal').modal('show');
            $('#clienteFiltro').val('');
            await Promise.all([loadCondiciones(), loadProvincias(), loadClientes()]);
        });

        $('#clienteFiltro').on('input', renderClientes);
        $('#btnNuevoCliente').on('click', nuevoCliente);
        $('#clienteForm').on('submit', saveCliente);

        $('#clienteFormModal').on('hidden.bs.modal', () => {
            clearClienteForm();
        });

        $('#clienteTable').on('click', '.btn-editar', function () {
            editCliente(Number($(this).data('id')));
        });

        $('#clienteTable').on('click', '.btn-eliminar', function () {
            deleteCliente(Number($(this).data('id')));
        });

        $('#clienteTable').on('click', '.btn-domicilios', async function () {
            const id = Number($(this).data('id'));
            $('#domClienteId').val(id);
            clearDomicilioForm();
            $('#clienteDomiciliosModal').modal('show');
            await loadDomicilios(id);
        });

        $('#clienteTable').on('click', '.btn-telefonos', async function () {
            const id = Number($(this).data('id'));
            $('#telClienteId').val(id);
            clearTelefonoForm();
            $('#clienteTelefonosModal').modal('show');
            await loadTelefonos(id);
        });

        $('#clienteTable').on('click', '.btn-emails', async function () {
            const id = Number($(this).data('id'));
            $('#emailClienteId').val(id);
            clearEmailForm();
            $('#clienteEmailsModal').modal('show');
            await loadEmails(id);
        });

        $('#btnGuardarDomicilio').on('click', saveDomicilio);
        $('#btnGuardarTelefono').on('click', saveTelefono);
        $('#btnGuardarEmail').on('click', saveEmail);

        $('#domiciliosClienteTable').on('click', '.btn-edit-dom', function () {
            const id = Number($(this).data('id'));
            const d = state.domicilios.find(x => (x.idDomicilio ?? x.IdDomicilio) === id);
            if (!d) return;

            $('#domId').val(d.idDomicilio ?? d.IdDomicilio);
            $('#domDomicilio').val(d.domicilio ?? d.Domicilio);
            $('#domCiudad').val(d.ciudad ?? d.Ciudad);
            $('#domLocalidad').val(first(d.localidad ?? d.Localidad));
            $('#domProvincia').val(d.idProvincia ?? d.IdProvincia);
            $('#domCodigoPostal').val(first(d.codigoPostal ?? d.CodigoPostal));
        });

        $('#domiciliosClienteTable').on('click', '.btn-del-dom', function () {
            deleteDomicilio(Number($(this).data('id')));
        });

        $('#telefonosClienteTable').on('click', '.btn-edit-tel', function () {
            const id = Number($(this).data('id'));
            const t = state.telefonos.find(x => (x.idTelefono ?? x.IdTelefono) === id);
            if (!t) return;

            $('#telId').val(t.idTelefono ?? t.IdTelefono);
            $('#telNumero').val(t.numeroTelefono ?? t.NumeroTelefono);
            $('#telTipo').val(t.tipoTelefono ?? t.TipoTelefono);
        });

        $('#telefonosClienteTable').on('click', '.btn-del-tel', function () {
            deleteTelefono(Number($(this).data('id')));
        });

        $('#emailsClienteTable').on('click', '.btn-edit-email', function () {
            const id = Number($(this).data('id'));
            const m = state.emails.find(x => (x.idEmail ?? x.IdEmail) === id);
            if (!m) return;

            $('#emailId').val(m.idEmail ?? m.IdEmail);
            $('#emailValor').val(m.email ?? m.Email);
        });

        $('#emailsClienteTable').on('click', '.btn-del-email', function () {
            deleteEmail(Number($(this).data('id')));
        });
    };

    $(bindEvents);
})();
