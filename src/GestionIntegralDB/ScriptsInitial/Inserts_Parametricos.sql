-- Inserts tipicos para tablas parametricas
-- Requiere un usuario administrador con IdUsuario = 1. Si no existe, se crea uno de respaldo.

IF NOT EXISTS (SELECT 1 FROM TiposUsuario WHERE IdTipoUsuario = 1)
BEGIN
    SET IDENTITY_INSERT TiposUsuario ON;
    INSERT INTO TiposUsuario (IdTipoUsuario, Descripcion, Activo, IdUsuarioCreacion, FechaCreacion)
    VALUES (1, 'Administrador', 1, 1, SYSUTCDATETIME());
    SET IDENTITY_INSERT TiposUsuario OFF;
END

IF NOT EXISTS (SELECT 1 FROM Usuarios WHERE IdUsuario = 1)
BEGIN
    SET IDENTITY_INSERT Usuarios ON;
    INSERT INTO Usuarios (IdUsuario, Nombre, Apellido, IdTipoUsuario, Usuario, ClaveHash, Bloqueado, Activo, IdUsuarioCreacion, FechaCreacion)
    VALUES (
        1,
        'Administrador',
        'Sistema',
        1,
        'admin',
        -- Hash BCrypt para la clave 'Admin123!'
        '$2a$11$gk/lE5669hXTdIvf3SGj5OFgxu773hnnt4ub/XOUrqXuKDbFtbds.',
        0,
        1,
        1,
        SYSUTCDATETIME()
    );
    SET IDENTITY_INSERT Usuarios OFF;
END

INSERT INTO CondicionesIVA (Codigo, Descripcion, Activo, IdUsuarioCreacion, FechaCreacion)
VALUES
    ('RI',    'Responsable Inscripto', 1, 1, SYSUTCDATETIME()),
    ('MON',   'Responsable Monotributo', 1, 1, SYSUTCDATETIME()),
    ('EX',    'Exento',                  1, 1, SYSUTCDATETIME()),
    ('CF',    'Consumidor Final',        1, 1, SYSUTCDATETIME()),
    ('NR',    'No Responsable',          1, 1, SYSUTCDATETIME());

INSERT INTO CondicionesPago (Codigo, Descripcion, Dias, Activo, IdUsuarioCreacion, FechaCreacion)
VALUES
    ('CONTADO', 'Contado',           0,  1, 1, SYSUTCDATETIME()),
    ('30D',     '30 dias',          30,  1, 1, SYSUTCDATETIME()),
    ('60D',     '60 dias',          60,  1, 1, SYSUTCDATETIME()),
    ('90D',     '90 dias',          90,  1, 1, SYSUTCDATETIME()),
    ('15D',     '15 dias',          15,  1, 1, SYSUTCDATETIME());

INSERT INTO Monedas (Codigo, Descripcion, Simbolo, CantidadDecimales, Activo, IdUsuarioCreacion, FechaCreacion)
VALUES
    ('ARS', 'Pesos Argentinos', '$',  2, 1, 1, SYSUTCDATETIME()),
    ('USD', 'Dolares Estadounidenses', 'US$', 2, 1, 1, SYSUTCDATETIME()),
    ('EUR', 'Euros', '€', 2, 1, 1, SYSUTCDATETIME());

INSERT INTO MediosPago (Codigo, Descripcion, EsEfectivo, RequiereReferencia, Activo, IdUsuarioCreacion, FechaCreacion)
VALUES
    ('EFECTIVO',   'Efectivo',        1, 0, 1, 1, SYSUTCDATETIME()),
    ('TRANSFERENCIA','Transferencia',  0, 1, 1, 1, SYSUTCDATETIME()),
    ('DEBITO',     'Tarjeta de Debito',0, 1, 1, 1, SYSUTCDATETIME()),
    ('CREDITO',    'Tarjeta de Credito',0, 1, 1, 1, SYSUTCDATETIME()),
    ('CHEQUE',     'Cheque',          0, 1, 1, 1, SYSUTCDATETIME()),
    ('MERCADOPAGO','MercadoPago',     0, 1, 1, 1, SYSUTCDATETIME());

INSERT INTO Provincias (Codigo, Descripcion, CodigoAFIP, Activo, IdUsuarioCreacion, FechaCreacion)
VALUES
    ('B',  'Buenos Aires',                '06', 1, 1, SYSUTCDATETIME()),
    ('C',  'Ciudad Autonoma de Buenos Aires','01', 1, 1, SYSUTCDATETIME()),
    ('X',  'Cordoba',                     '14', 1, 1, SYSUTCDATETIME()),
    ('S',  'Santa Fe',                    '21', 1, 1, SYSUTCDATETIME()),
    ('M',  'Mendoza',                     '13', 1, 1, SYSUTCDATETIME());

INSERT INTO TiposComprobante (Codigo, Descripcion, CodigoAFIP, Activo, IdUsuarioCreacion, FechaCreacion)
VALUES
    ('FA',   'Factura A',          '1',  1, 1, SYSUTCDATETIME()),
    ('FB',   'Factura B',          '6',  1, 1, SYSUTCDATETIME()),
    ('FC',   'Factura C',          '11', 1, 1, SYSUTCDATETIME()),
    ('NCA',  'Nota de Credito A',  '3',  1, 1, SYSUTCDATETIME()),
    ('NCB',  'Nota de Credito B',  '8',  1, 1, SYSUTCDATETIME()),
    ('REMITO','Remito',            '91', 1, 1, SYSUTCDATETIME());

INSERT INTO MotivosAnulacion (Codigo, Descripcion, Activo, IdUsuarioCreacion, FechaCreacion)
VALUES
    ('ERR_CARGA',     'Error de carga',         1, 1, SYSUTCDATETIME()),
    ('ANUL_CLI',      'Anulacion por cliente',  1, 1, SYSUTCDATETIME()),
    ('PROD_DEF',      'Producto defectuoso',    1, 1, SYSUTCDATETIME()),
    ('NO_STOCK',      'Sin stock',              1, 1, SYSUTCDATETIME()),
    ('CAMBIO_PRECIO', 'Cambio de precio',       1, 1, SYSUTCDATETIME());

INSERT INTO EstadosPedido (Codigo, Descripcion, Activo, IdUsuarioCreacion, FechaCreacion)
VALUES
    ('PENDIENTE',  'Pendiente',   1, 1, SYSUTCDATETIME()),
    ('EN_PROCESO', 'En proceso',  1, 1, SYSUTCDATETIME()),
    ('ENTREGADO',  'Entregado',   1, 1, SYSUTCDATETIME()),
    ('CANCELADO',  'Cancelado',   1, 1, SYSUTCDATETIME());

INSERT INTO EstadosFactura (Codigo, Descripcion, Activo, IdUsuarioCreacion, FechaCreacion)
VALUES
    ('PENDIENTE', 'Pendiente de pago', 1, 1, SYSUTCDATETIME()),
    ('PAGADA',    'Pagada',            1, 1, SYSUTCDATETIME()),
    ('ANULADA',   'Anulada',           1, 1, SYSUTCDATETIME());

INSERT INTO EstadosCompra (Codigo, Descripcion, Activo, IdUsuarioCreacion, FechaCreacion)
VALUES
    ('PENDIENTE', 'Pendiente de recepcion', 1, 1, SYSUTCDATETIME()),
    ('RECIBIDA',  'Recibida',               1, 1, SYSUTCDATETIME()),
    ('ANULADA',   'Anulada',                1, 1, SYSUTCDATETIME());

INSERT INTO EstadosProceso (Codigo, Descripcion, Activo, IdUsuarioCreacion, FechaCreacion)
VALUES
    ('INICIADO',   'Iniciado',    1, 1, SYSUTCDATETIME()),
    ('EN_CURSO',   'En curso',    1, 1, SYSUTCDATETIME()),
    ('FINALIZADO', 'Finalizado',  1, 1, SYSUTCDATETIME()),
    ('CANCELADO',  'Cancelado',   1, 1, SYSUTCDATETIME());

INSERT INTO EstadosCaja (Codigo, Descripcion, Activo, IdUsuarioCreacion, FechaCreacion)
VALUES
    ('ABIERTA', 'Abierta', 1, 1, SYSUTCDATETIME()),
    ('CERRADA', 'Cerrada', 1, 1, SYSUTCDATETIME());

INSERT INTO Depositos (Codigo, Descripcion, Ubicacion, Activo, IdUsuarioCreacion, FechaCreacion)
VALUES
    ('PRINCIPAL', 'Deposito Principal', 'Planta baja', 1, 1, SYSUTCDATETIME()),
    ('SECUNDARIO','Deposito Secundario','Primer piso', 1, 1, SYSUTCDATETIME());

INSERT INTO Categorias (Codigo, Descripcion, Activo, IdUsuarioCreacion, FechaCreacion)
VALUES
    ('GRAL',    'General',       1, 1, SYSUTCDATETIME()),
    ('ELECTRO', 'Electrodomesticos', 1, 1, SYSUTCDATETIME()),
    ('FERRE',   'Ferreteria',    1, 1, SYSUTCDATETIME());

INSERT INTO SubCategorias (IdCategoria, Codigo, Descripcion, Activo, IdUsuarioCreacion, FechaCreacion)
VALUES
    (1, 'VARIOS', 'Varios',         1, 1, SYSUTCDATETIME()),
    (2, 'COCINA', 'Cocina',         1, 1, SYSUTCDATETIME()),
    (2, 'LAVADO', 'Lavado',         1, 1, SYSUTCDATETIME()),
    (3, 'HERRAM', 'Herramientas',   1, 1, SYSUTCDATETIME());

INSERT INTO ListasPrecios (Codigo, Descripcion, FechaDesde, FechaHasta, Activo, IdUsuarioCreacion, FechaCreacion)
VALUES
    ('BASE',    'Lista de precios base',   '1900-01-01', NULL, 1, 1, SYSUTCDATETIME()),
    ('MAYOR',   'Lista de precios mayorista','1900-01-01', NULL, 1, 1, SYSUTCDATETIME());

INSERT INTO TiposImpuesto (Codigo, Descripcion, Activo, IdUsuarioCreacion, FechaCreacion)
VALUES
    ('IVA',      'Impuesto al Valor Agregado',        1, 1, SYSUTCDATETIME()),
    ('IIBB',     'Ingresos Brutos',                   1, 1, SYSUTCDATETIME()),
    ('GAN',      'Impuesto a las Ganancias',          1, 1, SYSUTCDATETIME()),
    ('INT',      'Impuesto Interno',                  1, 1, SYSUTCDATETIME()),
    ('EXENTO',   'Exento / No gravado',               1, 1, SYSUTCDATETIME());

INSERT INTO AlicuotasImpuesto (IdTipoImpuesto, Codigo, Descripcion, Porcentaje, FechaDesde, FechaHasta, Activo, IdUsuarioCreacion, FechaCreacion)
VALUES
    (1, 'IVA_21',    'IVA 21%',          21.00, '1900-01-01', NULL, 1, 1, SYSUTCDATETIME()),
    (1, 'IVA_10_5',  'IVA 10,5%',        10.50, '1900-01-01', NULL, 1, 1, SYSUTCDATETIME()),
    (1, 'IVA_27',    'IVA 27%',          27.00, '1900-01-01', NULL, 1, 1, SYSUTCDATETIME()),
    (1, 'IVA_0',     'IVA 0%',            0.00, '1900-01-01', NULL, 1, 1, SYSUTCDATETIME()),
    (1, 'IVA_EX',    'IVA Exento',        0.00, '1900-01-01', NULL, 1, 1, SYSUTCDATETIME()),
    (2, 'IIBB_1',    'IIBB 1%',           1.00, '1900-01-01', NULL, 1, 1, SYSUTCDATETIME()),
    (2, 'IIBB_2',    'IIBB 2%',           2.00, '1900-01-01', NULL, 1, 1, SYSUTCDATETIME()),
    (3, 'GAN_5',     'Ganancias 5%',      5.00, '1900-01-01', NULL, 1, 1, SYSUTCDATETIME());

INSERT INTO TiposMovimientoCaja (Codigo, Descripcion, Signo, Activo, IdUsuarioCreacion, FechaCreacion)
VALUES
    ('INGRESO',  'Ingreso de dinero',        1,  1, 1, SYSUTCDATETIME()),
    ('EGRESO',   'Egreso de dinero',        -1,  1, 1, SYSUTCDATETIME()),
    ('APERTURA', 'Apertura de caja',         1,  1, 1, SYSUTCDATETIME()),
    ('CIERRE',   'Cierre de caja',          -1,  1, SYSUTCDATETIME()),
    ('VENTA',    'Venta / cobro',            1,  1, 1, SYSUTCDATETIME()),
    ('COMPRA',   'Pago a proveedor',        -1,  1, 1, SYSUTCDATETIME()),
    ('RETIRO',   'Retiro de efectivo',      -1,  1, 1, SYSUTCDATETIME()),
    ('DEPOSITO', 'Deposito en cuenta',      -1,  1, 1, SYSUTCDATETIME());

INSERT INTO TiposMovimientoStock (Codigo, Descripcion, Signo, Activo, IdUsuarioCreacion, FechaCreacion)
VALUES
    ('ENTRADA',      'Entrada de stock',             1, 1, 1, SYSUTCDATETIME()),
    ('SALIDA',       'Salida de stock',             -1, 1, 1, SYSUTCDATETIME()),
    ('AJUSTE_POS',   'Ajuste positivo de stock',     1, 1, 1, SYSUTCDATETIME()),
    ('AJUSTE_NEG',   'Ajuste negativo de stock',    -1, 1, 1, SYSUTCDATETIME()),
    ('COMPRA',       'Ingreso por compra',           1, 1, 1, SYSUTCDATETIME()),
    ('VENTA',        'Egreso por venta',            -1, 1, 1, SYSUTCDATETIME()),
    ('TRANSFERENCIA','Transferencia entre depositos', 1, 1, 1, SYSUTCDATETIME()),
    ('MERMA',        'Merma / rotura',              -1, 1, 1, SYSUTCDATETIME());
