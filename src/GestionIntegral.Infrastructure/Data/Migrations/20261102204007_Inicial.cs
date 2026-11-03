using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionIntegral.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class Inicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "AlicuotasImpuesto",
                schema: "dbo",
                columns: table => new
                {
                    IdAlicuotaImpuesto = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdTipoImpuesto = table.Column<int>(type: "int", nullable: false),
                    Codigo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Porcentaje = table.Column<decimal>(type: "decimal(8,4)", nullable: false),
                    FechaDesde = table.Column<DateTime>(type: "date", nullable: true),
                    FechaHasta = table.Column<DateTime>(type: "date", nullable: true),
                    Activo = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    FechaBaja = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                    IdUsuarioCreacion = table.Column<int>(type: "int", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    IdUsuarioUltimaModificacion = table.Column<int>(type: "int", nullable: true),
                    FechaUltimaModificacion = table.Column<DateTime>(type: "datetime2(3)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlicuotasImpuesto", x => x.IdAlicuotaImpuesto);
                });

            migrationBuilder.CreateTable(
                name: "CajaCabecera",
                schema: "dbo",
                columns: table => new
                {
                    IdCaja = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FechaCaja = table.Column<DateTime>(type: "date", nullable: false),
                    IdUsuarioResponsable = table.Column<int>(type: "int", nullable: false),
                    IdEstadoCaja = table.Column<int>(type: "int", nullable: false),
                    SaldoInicial = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                    SaldoFinal = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    FechaApertura = table.Column<DateTime>(type: "datetime2(3)", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    FechaCierre = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                    Observaciones = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IdUsuarioCreacion = table.Column<int>(type: "int", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    IdUsuarioUltimaModificacion = table.Column<int>(type: "int", nullable: true),
                    FechaUltimaModificacion = table.Column<DateTime>(type: "datetime2(3)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CajaCabecera", x => x.IdCaja);
                });

            migrationBuilder.CreateTable(
                name: "CajaDetalle",
                schema: "dbo",
                columns: table => new
                {
                    IdCajaDetalle = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdCaja = table.Column<long>(type: "bigint", nullable: false),
                    IdTipoMovimientoCaja = table.Column<int>(type: "int", nullable: false),
                    IdMedioPago = table.Column<int>(type: "int", nullable: true),
                    IdMoneda = table.Column<int>(type: "int", nullable: true),
                    FechaMovimiento = table.Column<DateTime>(type: "datetime2(3)", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    Concepto = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Referencia = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Importe = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IdUsuarioCreacion = table.Column<int>(type: "int", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    IdUsuarioUltimaModificacion = table.Column<int>(type: "int", nullable: true),
                    FechaUltimaModificacion = table.Column<DateTime>(type: "datetime2(3)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CajaDetalle", x => x.IdCajaDetalle);
                    table.ForeignKey(
                        name: "FK_CajaDetalle_Cajas",
                        column: x => x.IdCaja,
                        principalSchema: "dbo",
                        principalTable: "CajaCabecera",
                        principalColumn: "IdCaja",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Categorias",
                schema: "dbo",
                columns: table => new
                {
                    IdCategoria = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Codigo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    FechaBaja = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                    IdUsuarioCreacion = table.Column<int>(type: "int", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    IdUsuarioUltimaModificacion = table.Column<int>(type: "int", nullable: true),
                    FechaUltimaModificacion = table.Column<DateTime>(type: "datetime2(3)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categorias", x => x.IdCategoria);
                });

            migrationBuilder.CreateTable(
                name: "ClienteContactos",
                schema: "dbo",
                columns: table => new
                {
                    IdCliente = table.Column<int>(type: "int", nullable: false),
                    IdContacto = table.Column<int>(type: "int", nullable: false),
                    EsPrincipal = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClienteContactos", x => new { x.IdCliente, x.IdContacto });
                });

            migrationBuilder.CreateTable(
                name: "Clientes",
                schema: "dbo",
                columns: table => new
                {
                    IdCliente = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Codigo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    RazonSocial = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NombreFantasia = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    CUIT = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: true),
                    IdCondicionPago = table.Column<int>(type: "int", nullable: true),
                    Observaciones = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Activo = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    FechaBaja = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                    IdUsuarioCreacion = table.Column<int>(type: "int", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    IdUsuarioUltimaModificacion = table.Column<int>(type: "int", nullable: true),
                    FechaUltimaModificacion = table.Column<DateTime>(type: "datetime2(3)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clientes", x => x.IdCliente);
                });

            migrationBuilder.CreateTable(
                name: "CondicionesIVA",
                schema: "dbo",
                columns: table => new
                {
                    IdCondicionIVA = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Codigo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    FechaBaja = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                    IdUsuarioCreacion = table.Column<int>(type: "int", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    IdUsuarioUltimaModificacion = table.Column<int>(type: "int", nullable: true),
                    FechaUltimaModificacion = table.Column<DateTime>(type: "datetime2(3)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CondicionesIVA", x => x.IdCondicionIVA);
                });

            migrationBuilder.CreateTable(
                name: "CondicionesPago",
                schema: "dbo",
                columns: table => new
                {
                    IdCondicionPago = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Codigo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Dias = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    Activo = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    FechaBaja = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                    IdUsuarioCreacion = table.Column<int>(type: "int", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    IdUsuarioUltimaModificacion = table.Column<int>(type: "int", nullable: true),
                    FechaUltimaModificacion = table.Column<DateTime>(type: "datetime2(3)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CondicionesPago", x => x.IdCondicionPago);
                });

            migrationBuilder.CreateTable(
                name: "Contactos",
                schema: "dbo",
                columns: table => new
                {
                    IdContacto = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Apellido = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Cargo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Telefono = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Celular = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Activo = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IdUsuarioCreacion = table.Column<int>(type: "int", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    IdUsuarioUltimaModificacion = table.Column<int>(type: "int", nullable: true),
                    FechaUltimaModificacion = table.Column<DateTime>(type: "datetime2(3)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contactos", x => x.IdContacto);
                });

            migrationBuilder.CreateTable(
                name: "Depositos",
                schema: "dbo",
                columns: table => new
                {
                    IdDeposito = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Codigo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Ubicacion = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Activo = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IdUsuarioCreacion = table.Column<int>(type: "int", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    IdUsuarioUltimaModificacion = table.Column<int>(type: "int", nullable: true),
                    FechaUltimaModificacion = table.Column<DateTime>(type: "datetime2(3)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Depositos", x => x.IdDeposito);
                });

            migrationBuilder.CreateTable(
                name: "Domicilios",
                schema: "dbo",
                columns: table => new
                {
                    IdDomicilio = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TipoReferencia = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    IdReferencia = table.Column<int>(type: "int", nullable: false),
                    Domicilio = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Ciudad = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Localidad = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IdProvincia = table.Column<int>(type: "int", nullable: false),
                    CodigoPostal = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Activo = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    FechaBaja = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                    IdUsuarioCreacion = table.Column<int>(type: "int", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    IdUsuarioUltimaModificacion = table.Column<int>(type: "int", nullable: true),
                    FechaUltimaModificacion = table.Column<DateTime>(type: "datetime2(3)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Domicilios", x => x.IdDomicilio);
                    table.CheckConstraint("CK_Domicilios_IdReferencia", "IdReferencia > 0");
                    table.CheckConstraint("CK_Domicilios_TipoReferencia", "TipoReferencia IN ('C','P')");
                });

            migrationBuilder.CreateTable(
                name: "Emails",
                schema: "dbo",
                columns: table => new
                {
                    IdEmail = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TipoReferencia = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    IdReferencia = table.Column<int>(type: "int", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(254)", maxLength: 254, nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    FechaBaja = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                    IdUsuarioCreacion = table.Column<int>(type: "int", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    IdUsuarioUltimaModificacion = table.Column<int>(type: "int", nullable: true),
                    FechaUltimaModificacion = table.Column<DateTime>(type: "datetime2(3)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Emails", x => x.IdEmail);
                    table.CheckConstraint("CK_Emails_IdReferencia", "IdReferencia > 0");
                    table.CheckConstraint("CK_Emails_TipoReferencia", "TipoReferencia IN ('C','P')");
                });

            migrationBuilder.CreateTable(
                name: "EstadosCaja",
                schema: "dbo",
                columns: table => new
                {
                    IdEstadoCaja = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Codigo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IdUsuarioCreacion = table.Column<int>(type: "int", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    IdUsuarioUltimaModificacion = table.Column<int>(type: "int", nullable: true),
                    FechaUltimaModificacion = table.Column<DateTime>(type: "datetime2(3)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstadosCaja", x => x.IdEstadoCaja);
                });

            migrationBuilder.CreateTable(
                name: "EstadosCompra",
                schema: "dbo",
                columns: table => new
                {
                    IdEstadoCompra = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdUsuarioCreacion = table.Column<int>(type: "int", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    IdUsuarioUltimaModificacion = table.Column<int>(type: "int", nullable: true),
                    FechaUltimaModificacion = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                    Codigo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    FechaBaja = table.Column<DateTime>(type: "datetime2(3)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstadosCompra", x => x.IdEstadoCompra);
                });

            migrationBuilder.CreateTable(
                name: "EstadosFactura",
                schema: "dbo",
                columns: table => new
                {
                    IdEstadoFactura = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdUsuarioCreacion = table.Column<int>(type: "int", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    IdUsuarioUltimaModificacion = table.Column<int>(type: "int", nullable: true),
                    FechaUltimaModificacion = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                    Codigo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    FechaBaja = table.Column<DateTime>(type: "datetime2(3)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstadosFactura", x => x.IdEstadoFactura);
                });

            migrationBuilder.CreateTable(
                name: "EstadosPedido",
                schema: "dbo",
                columns: table => new
                {
                    IdEstadoPedido = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdUsuarioCreacion = table.Column<int>(type: "int", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    IdUsuarioUltimaModificacion = table.Column<int>(type: "int", nullable: true),
                    FechaUltimaModificacion = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                    Codigo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    FechaBaja = table.Column<DateTime>(type: "datetime2(3)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstadosPedido", x => x.IdEstadoPedido);
                });

            migrationBuilder.CreateTable(
                name: "EstadosProceso",
                schema: "dbo",
                columns: table => new
                {
                    IdEstadoProceso = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdUsuarioCreacion = table.Column<int>(type: "int", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    IdUsuarioUltimaModificacion = table.Column<int>(type: "int", nullable: true),
                    FechaUltimaModificacion = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                    Codigo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    FechaBaja = table.Column<DateTime>(type: "datetime2(3)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstadosProceso", x => x.IdEstadoProceso);
                });

            migrationBuilder.CreateTable(
                name: "Facturas",
                schema: "dbo",
                columns: table => new
                {
                    IdFactura = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdCliente = table.Column<int>(type: "int", nullable: true),
                    NombreCliente = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    IdTipoComprobante = table.Column<int>(type: "int", nullable: false),
                    NumeroComprobante = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IdCondicionPago = table.Column<int>(type: "int", nullable: false),
                    Subtotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ImporteIva = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Total = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Observaciones = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false),
                    FechaBaja = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IdUsuarioCreacion = table.Column<int>(type: "int", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IdUsuarioUltimaModificacion = table.Column<int>(type: "int", nullable: true),
                    FechaUltimaModificacion = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Facturas", x => x.IdFactura);
                    table.ForeignKey(
                        name: "FK_Facturas_Clientes_IdCliente",
                        column: x => x.IdCliente,
                        principalSchema: "dbo",
                        principalTable: "Clientes",
                        principalColumn: "IdCliente",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Facturas_CondicionesPago_IdCondicionPago",
                        column: x => x.IdCondicionPago,
                        principalSchema: "dbo",
                        principalTable: "CondicionesPago",
                        principalColumn: "IdCondicionPago",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FacturasDetalle",
                schema: "dbo",
                columns: table => new
                {
                    IdFacturaDetalle = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdFactura = table.Column<long>(type: "bigint", nullable: false),
                    IdProducto = table.Column<int>(type: "int", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Cantidad = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    PrecioUnitario = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    PorcentajeIva = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Subtotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ImporteIva = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FacturasDetalle", x => x.IdFacturaDetalle);
                    table.ForeignKey(
                        name: "FK_FacturasDetalle_Facturas_IdFactura",
                        column: x => x.IdFactura,
                        principalSchema: "dbo",
                        principalTable: "Facturas",
                        principalColumn: "IdFactura",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ListaPreciosDetalle",
                schema: "dbo",
                columns: table => new
                {
                    IdListaPrecioDetalle = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdListaPrecio = table.Column<int>(type: "int", nullable: false),
                    IdProducto = table.Column<int>(type: "int", nullable: false),
                    Precio = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    IdUsuarioCreacion = table.Column<int>(type: "int", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    IdUsuarioUltimaModificacion = table.Column<int>(type: "int", nullable: true),
                    FechaUltimaModificacion = table.Column<DateTime>(type: "datetime2(3)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ListaPreciosDetalle", x => x.IdListaPrecioDetalle);
                });

            migrationBuilder.CreateTable(
                name: "ListasPrecios",
                schema: "dbo",
                columns: table => new
                {
                    IdListaPrecio = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Codigo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FechaDesde = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                    FechaHasta = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                    Activo = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    FechaBaja = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                    IdUsuarioCreacion = table.Column<int>(type: "int", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    IdUsuarioUltimaModificacion = table.Column<int>(type: "int", nullable: true),
                    FechaUltimaModificacion = table.Column<DateTime>(type: "datetime2(3)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ListasPrecios", x => x.IdListaPrecio);
                });

            migrationBuilder.CreateTable(
                name: "MediosPago",
                schema: "dbo",
                columns: table => new
                {
                    IdMedioPago = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Codigo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    EsEfectivo = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    RequiereReferencia = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IdUsuarioCreacion = table.Column<int>(type: "int", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    IdUsuarioUltimaModificacion = table.Column<int>(type: "int", nullable: true),
                    FechaUltimaModificacion = table.Column<DateTime>(type: "datetime2(3)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MediosPago", x => x.IdMedioPago);
                });

            migrationBuilder.CreateTable(
                name: "PagosClientes",
                schema: "dbo",
                columns: table => new
                {
                    IdPagoCliente = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdCliente = table.Column<int>(type: "int", nullable: false),
                    IdFactura = table.Column<long>(type: "bigint", nullable: true),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Monto = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IdMedioPago = table.Column<int>(type: "int", nullable: false),
                    NumeroCheque = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Banco = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    FechaVencimientoCheque = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Observaciones = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false),
                    IdUsuarioCreacion = table.Column<int>(type: "int", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IdUsuarioUltimaModificacion = table.Column<int>(type: "int", nullable: true),
                    FechaUltimaModificacion = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PagosClientes", x => x.IdPagoCliente);
                    table.ForeignKey(
                        name: "FK_PagosClientes_Clientes_IdCliente",
                        column: x => x.IdCliente,
                        principalSchema: "dbo",
                        principalTable: "Clientes",
                        principalColumn: "IdCliente",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PagosClientes_Facturas_IdFactura",
                        column: x => x.IdFactura,
                        principalSchema: "dbo",
                        principalTable: "Facturas",
                        principalColumn: "IdFactura",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_PagosClientes_MediosPago_IdMedioPago",
                        column: x => x.IdMedioPago,
                        principalSchema: "dbo",
                        principalTable: "MediosPago",
                        principalColumn: "IdMedioPago",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ValoresCartera",
                schema: "dbo",
                columns: table => new
                {
                    IdValorCartera = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdPagoCliente = table.Column<long>(type: "bigint", nullable: false),
                    IdCliente = table.Column<int>(type: "int", nullable: false),
                    TipoValor = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Numero = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Banco = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    FechaEmision = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FechaVencimiento = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Importe = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Estado = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Observaciones = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Activo = table.Column<bool>(type: "bit", nullable: false),
                    IdUsuarioCreacion = table.Column<int>(type: "int", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IdUsuarioUltimaModificacion = table.Column<int>(type: "int", nullable: true),
                    FechaUltimaModificacion = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ValoresCartera", x => x.IdValorCartera);
                    table.ForeignKey(
                        name: "FK_ValoresCartera_Clientes_IdCliente",
                        column: x => x.IdCliente,
                        principalSchema: "dbo",
                        principalTable: "Clientes",
                        principalColumn: "IdCliente",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ValoresCartera_PagosClientes_IdPagoCliente",
                        column: x => x.IdPagoCliente,
                        principalSchema: "dbo",
                        principalTable: "PagosClientes",
                        principalColumn: "IdPagoCliente",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Monedas",
                schema: "dbo",
                columns: table => new
                {
                    IdMoneda = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Codigo = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Simbolo = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    CantidadDecimales = table.Column<byte>(type: "tinyint", nullable: false, defaultValue: (byte)2),
                    Activo = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IdUsuarioCreacion = table.Column<int>(type: "int", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    IdUsuarioUltimaModificacion = table.Column<int>(type: "int", nullable: true),
                    FechaUltimaModificacion = table.Column<DateTime>(type: "datetime2(3)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Monedas", x => x.IdMoneda);
                });

            migrationBuilder.CreateTable(
                name: "MotivosAnulacion",
                schema: "dbo",
                columns: table => new
                {
                    IdMotivoAnulacion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdUsuarioCreacion = table.Column<int>(type: "int", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    IdUsuarioUltimaModificacion = table.Column<int>(type: "int", nullable: true),
                    FechaUltimaModificacion = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                    Codigo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    FechaBaja = table.Column<DateTime>(type: "datetime2(3)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MotivosAnulacion", x => x.IdMotivoAnulacion);
                });

            migrationBuilder.CreateTable(
                name: "MovimientosStock",
                schema: "dbo",
                columns: table => new
                {
                    IdMovimientoStock = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdDeposito = table.Column<int>(type: "int", nullable: false),
                    IdTipoMovimientoStock = table.Column<int>(type: "int", nullable: false),
                    FechaMovimiento = table.Column<DateTime>(type: "datetime2(3)", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    IdUsuarioResponsable = table.Column<int>(type: "int", nullable: false),
                    Referencia = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Observaciones = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IdUsuarioCreacion = table.Column<int>(type: "int", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    IdUsuarioUltimaModificacion = table.Column<int>(type: "int", nullable: true),
                    FechaUltimaModificacion = table.Column<DateTime>(type: "datetime2(3)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovimientosStock", x => x.IdMovimientoStock);
                    table.ForeignKey(
                        name: "FK_MovimientosStock_Depositos",
                        column: x => x.IdDeposito,
                        principalSchema: "dbo",
                        principalTable: "Depositos",
                        principalColumn: "IdDeposito",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MovimientoStockDetalle",
                schema: "dbo",
                columns: table => new
                {
                    IdMovimientoStockDetalle = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdMovimientoStock = table.Column<long>(type: "bigint", nullable: false),
                    IdProducto = table.Column<int>(type: "int", nullable: false),
                    Cantidad = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    IdUsuarioCreacion = table.Column<int>(type: "int", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    IdUsuarioUltimaModificacion = table.Column<int>(type: "int", nullable: true),
                    FechaUltimaModificacion = table.Column<DateTime>(type: "datetime2(3)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovimientoStockDetalle", x => x.IdMovimientoStockDetalle);
                    table.ForeignKey(
                        name: "FK_MovimientoStockDetalle_Movimiento",
                        column: x => x.IdMovimientoStock,
                        principalSchema: "dbo",
                        principalTable: "MovimientosStock",
                        principalColumn: "IdMovimientoStock",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PagosProveedores",
                schema: "dbo",
                columns: table => new
                {
                    IdPagoProveedor = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdProveedor = table.Column<int>(type: "int", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Monto = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IdMedioPago = table.Column<int>(type: "int", nullable: false),
                    NumeroCheque = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Banco = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    FechaVencimientoCheque = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Concepto = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Observaciones = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false),
                    IdUsuarioCreacion = table.Column<int>(type: "int", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IdUsuarioUltimaModificacion = table.Column<int>(type: "int", nullable: true),
                    FechaUltimaModificacion = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PagosProveedores", x => x.IdPagoProveedor);
                    table.ForeignKey(
                        name: "FK_PagosProveedores_MediosPago_IdMedioPago",
                        column: x => x.IdMedioPago,
                        principalSchema: "dbo",
                        principalTable: "MediosPago",
                        principalColumn: "IdMedioPago",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Productos",
                schema: "dbo",
                columns: table => new
                {
                    IdProducto = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Codigo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    IdCategoria = table.Column<int>(type: "int", nullable: false),
                    IdSubCategoria = table.Column<int>(type: "int", nullable: true),
                    CodigoBarras = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Costo = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
                    StockMinimo = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
                    StockMaximo = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
                    Observaciones = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Activo = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    FechaBaja = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                    IdUsuarioCreacion = table.Column<int>(type: "int", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    IdUsuarioUltimaModificacion = table.Column<int>(type: "int", nullable: true),
                    FechaUltimaModificacion = table.Column<DateTime>(type: "datetime2(3)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Productos", x => x.IdProducto);
                    table.ForeignKey(
                        name: "FK_Productos_Categorias",
                        column: x => x.IdCategoria,
                        principalSchema: "dbo",
                        principalTable: "Categorias",
                        principalColumn: "IdCategoria",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Stock",
                schema: "dbo",
                columns: table => new
                {
                    IdStock = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdDeposito = table.Column<int>(type: "int", nullable: false),
                    IdProducto = table.Column<int>(type: "int", nullable: false),
                    Cantidad = table.Column<decimal>(type: "decimal(18,4)", nullable: false, defaultValue: 0m),
                    FechaUltimoMovimiento = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stock", x => x.IdStock);
                    table.ForeignKey(
                        name: "FK_Stock_Depositos",
                        column: x => x.IdDeposito,
                        principalSchema: "dbo",
                        principalTable: "Depositos",
                        principalColumn: "IdDeposito",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Stock_Productos",
                        column: x => x.IdProducto,
                        principalSchema: "dbo",
                        principalTable: "Productos",
                        principalColumn: "IdProducto",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Proveedores",
                schema: "dbo",
                columns: table => new
                {
                    IdProveedor = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Codigo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    RazonSocial = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NombreFantasia = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    CUIT = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: true),
                    IdCondicionPago = table.Column<int>(type: "int", nullable: true),
                    Observaciones = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Activo = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    FechaBaja = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                    IdUsuarioCreacion = table.Column<int>(type: "int", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    IdUsuarioUltimaModificacion = table.Column<int>(type: "int", nullable: true),
                    FechaUltimaModificacion = table.Column<DateTime>(type: "datetime2(3)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Proveedores", x => x.IdProveedor);
                    table.ForeignKey(
                        name: "FK_Proveedores_CondicionesPago",
                        column: x => x.IdCondicionPago,
                        principalSchema: "dbo",
                        principalTable: "CondicionesPago",
                        principalColumn: "IdCondicionPago",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Provincias",
                schema: "dbo",
                columns: table => new
                {
                    IdProvincia = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CodigoAFIP = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    IdUsuarioCreacion = table.Column<int>(type: "int", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    IdUsuarioUltimaModificacion = table.Column<int>(type: "int", nullable: true),
                    FechaUltimaModificacion = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                    Codigo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    FechaBaja = table.Column<DateTime>(type: "datetime2(3)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Provincias", x => x.IdProvincia);
                });

            migrationBuilder.CreateTable(
                name: "SubCategorias",
                schema: "dbo",
                columns: table => new
                {
                    IdSubCategoria = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdCategoria = table.Column<int>(type: "int", nullable: false),
                    Codigo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    FechaBaja = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                    IdUsuarioCreacion = table.Column<int>(type: "int", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    IdUsuarioUltimaModificacion = table.Column<int>(type: "int", nullable: true),
                    FechaUltimaModificacion = table.Column<DateTime>(type: "datetime2(3)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubCategorias", x => x.IdSubCategoria);
                    table.ForeignKey(
                        name: "FK_SubCategorias_Categorias",
                        column: x => x.IdCategoria,
                        principalSchema: "dbo",
                        principalTable: "Categorias",
                        principalColumn: "IdCategoria",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Telefonos",
                schema: "dbo",
                columns: table => new
                {
                    IdTelefono = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TipoReferencia = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    IdReferencia = table.Column<int>(type: "int", nullable: false),
                    NumeroTelefono = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TipoTelefono = table.Column<string>(type: "nvarchar(7)", maxLength: 7, nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    FechaBaja = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                    IdUsuarioCreacion = table.Column<int>(type: "int", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    IdUsuarioUltimaModificacion = table.Column<int>(type: "int", nullable: true),
                    FechaUltimaModificacion = table.Column<DateTime>(type: "datetime2(3)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Telefonos", x => x.IdTelefono);
                    table.CheckConstraint("CK_Telefonos_IdReferencia", "IdReferencia > 0");
                    table.CheckConstraint("CK_Telefonos_TipoReferencia", "TipoReferencia IN ('C','P')");
                });

            migrationBuilder.CreateTable(
                name: "TiposComprobante",
                schema: "dbo",
                columns: table => new
                {
                    IdTipoComprobante = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CodigoAFIP = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    IdUsuarioCreacion = table.Column<int>(type: "int", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    IdUsuarioUltimaModificacion = table.Column<int>(type: "int", nullable: true),
                    FechaUltimaModificacion = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                    Codigo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    FechaBaja = table.Column<DateTime>(type: "datetime2(3)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TiposComprobante", x => x.IdTipoComprobante);
                });

            migrationBuilder.CreateTable(
                name: "TiposImpuesto",
                schema: "dbo",
                columns: table => new
                {
                    IdTipoImpuesto = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Codigo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    FechaBaja = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                    IdUsuarioCreacion = table.Column<int>(type: "int", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    IdUsuarioUltimaModificacion = table.Column<int>(type: "int", nullable: true),
                    FechaUltimaModificacion = table.Column<DateTime>(type: "datetime2(3)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TiposImpuesto", x => x.IdTipoImpuesto);
                });

            migrationBuilder.CreateTable(
                name: "TiposMovimientoCaja",
                schema: "dbo",
                columns: table => new
                {
                    IdTipoMovimientoCaja = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Codigo = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Signo = table.Column<short>(type: "smallint", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IdUsuarioCreacion = table.Column<int>(type: "int", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    IdUsuarioUltimaModificacion = table.Column<int>(type: "int", nullable: true),
                    FechaUltimaModificacion = table.Column<DateTime>(type: "datetime2(3)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TiposMovimientoCaja", x => x.IdTipoMovimientoCaja);
                });

            migrationBuilder.CreateTable(
                name: "TiposMovimientoStock",
                schema: "dbo",
                columns: table => new
                {
                    IdTipoMovimientoStock = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Codigo = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Signo = table.Column<short>(type: "smallint", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IdUsuarioCreacion = table.Column<int>(type: "int", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    IdUsuarioUltimaModificacion = table.Column<int>(type: "int", nullable: true),
                    FechaUltimaModificacion = table.Column<DateTime>(type: "datetime2(3)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TiposMovimientoStock", x => x.IdTipoMovimientoStock);
                });

            migrationBuilder.CreateTable(
                name: "TiposUsuario",
                schema: "dbo",
                columns: table => new
                {
                    IdTipoUsuario = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descripcion = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    FechaBaja = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                    IdUsuarioCreacion = table.Column<int>(type: "int", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    IdUsuarioUltimaModificacion = table.Column<int>(type: "int", nullable: true),
                    FechaUltimaModificacion = table.Column<DateTime>(type: "datetime2(3)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TiposUsuario", x => x.IdTipoUsuario);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                schema: "dbo",
                columns: table => new
                {
                    IdUsuario = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Apellido = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IdTipoUsuario = table.Column<int>(type: "int", nullable: false),
                    Usuario = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ClaveHash = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Bloqueado = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    FechaBaja = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                    IdUsuarioCreacion = table.Column<int>(type: "int", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    IdUsuarioUltimaModificacion = table.Column<int>(type: "int", nullable: true),
                    FechaUltimaModificacion = table.Column<DateTime>(type: "datetime2(3)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.IdUsuario);
                    table.ForeignKey(
                        name: "FK_Usuarios_TiposUsuario",
                        column: x => x.IdTipoUsuario,
                        principalSchema: "dbo",
                        principalTable: "TiposUsuario",
                        principalColumn: "IdTipoUsuario",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Usuarios_UsuarioCreacion",
                        column: x => x.IdUsuarioCreacion,
                        principalSchema: "dbo",
                        principalTable: "Usuarios",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Usuarios_UsuarioModificacion",
                        column: x => x.IdUsuarioUltimaModificacion,
                        principalSchema: "dbo",
                        principalTable: "Usuarios",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AlicuotasImpuesto_IdTipoImpuesto",
                schema: "dbo",
                table: "AlicuotasImpuesto",
                column: "IdTipoImpuesto");

            migrationBuilder.CreateIndex(
                name: "IX_AlicuotasImpuesto_IdUsuarioCreacion",
                schema: "dbo",
                table: "AlicuotasImpuesto",
                column: "IdUsuarioCreacion");

            migrationBuilder.CreateIndex(
                name: "IX_AlicuotasImpuesto_IdUsuarioUltimaModificacion",
                schema: "dbo",
                table: "AlicuotasImpuesto",
                column: "IdUsuarioUltimaModificacion");

            migrationBuilder.CreateIndex(
                name: "UQ_AlicuotasImpuesto_Codigo",
                schema: "dbo",
                table: "AlicuotasImpuesto",
                column: "Codigo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CajaCabecera_IdEstadoCaja",
                schema: "dbo",
                table: "CajaCabecera",
                column: "IdEstadoCaja");

            migrationBuilder.CreateIndex(
                name: "IX_CajaCabecera_IdUsuarioCreacion",
                schema: "dbo",
                table: "CajaCabecera",
                column: "IdUsuarioCreacion");

            migrationBuilder.CreateIndex(
                name: "IX_CajaCabecera_IdUsuarioResponsable",
                schema: "dbo",
                table: "CajaCabecera",
                column: "IdUsuarioResponsable");

            migrationBuilder.CreateIndex(
                name: "IX_CajaCabecera_IdUsuarioUltimaModificacion",
                schema: "dbo",
                table: "CajaCabecera",
                column: "IdUsuarioUltimaModificacion");

            migrationBuilder.CreateIndex(
                name: "IX_CajaDetalle_IdCaja",
                schema: "dbo",
                table: "CajaDetalle",
                column: "IdCaja");

            migrationBuilder.CreateIndex(
                name: "IX_CajaDetalle_IdMedioPago",
                schema: "dbo",
                table: "CajaDetalle",
                column: "IdMedioPago");

            migrationBuilder.CreateIndex(
                name: "IX_CajaDetalle_IdMoneda",
                schema: "dbo",
                table: "CajaDetalle",
                column: "IdMoneda");

            migrationBuilder.CreateIndex(
                name: "IX_CajaDetalle_IdTipoMovimientoCaja",
                schema: "dbo",
                table: "CajaDetalle",
                column: "IdTipoMovimientoCaja");

            migrationBuilder.CreateIndex(
                name: "IX_CajaDetalle_IdUsuarioCreacion",
                schema: "dbo",
                table: "CajaDetalle",
                column: "IdUsuarioCreacion");

            migrationBuilder.CreateIndex(
                name: "IX_CajaDetalle_IdUsuarioUltimaModificacion",
                schema: "dbo",
                table: "CajaDetalle",
                column: "IdUsuarioUltimaModificacion");

            migrationBuilder.CreateIndex(
                name: "IX_Categorias_IdUsuarioCreacion",
                schema: "dbo",
                table: "Categorias",
                column: "IdUsuarioCreacion");

            migrationBuilder.CreateIndex(
                name: "IX_Categorias_IdUsuarioUltimaModificacion",
                schema: "dbo",
                table: "Categorias",
                column: "IdUsuarioUltimaModificacion");

            migrationBuilder.CreateIndex(
                name: "UQ_Categorias_Codigo",
                schema: "dbo",
                table: "Categorias",
                column: "Codigo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClienteContactos_IdContacto",
                schema: "dbo",
                table: "ClienteContactos",
                column: "IdContacto");

            migrationBuilder.CreateIndex(
                name: "IX_Clientes_IdCondicionPago",
                schema: "dbo",
                table: "Clientes",
                column: "IdCondicionPago");

            migrationBuilder.CreateIndex(
                name: "IX_Clientes_IdUsuarioCreacion",
                schema: "dbo",
                table: "Clientes",
                column: "IdUsuarioCreacion");

            migrationBuilder.CreateIndex(
                name: "IX_Clientes_IdUsuarioUltimaModificacion",
                schema: "dbo",
                table: "Clientes",
                column: "IdUsuarioUltimaModificacion");

            migrationBuilder.CreateIndex(
                name: "UQ_Clientes_Codigo",
                schema: "dbo",
                table: "Clientes",
                column: "Codigo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CondicionesIVA_IdUsuarioCreacion",
                schema: "dbo",
                table: "CondicionesIVA",
                column: "IdUsuarioCreacion");

            migrationBuilder.CreateIndex(
                name: "IX_CondicionesIVA_IdUsuarioUltimaModificacion",
                schema: "dbo",
                table: "CondicionesIVA",
                column: "IdUsuarioUltimaModificacion");

            migrationBuilder.CreateIndex(
                name: "UQ_CondicionesIVA_Codigo",
                schema: "dbo",
                table: "CondicionesIVA",
                column: "Codigo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CondicionesPago_IdUsuarioCreacion",
                schema: "dbo",
                table: "CondicionesPago",
                column: "IdUsuarioCreacion");

            migrationBuilder.CreateIndex(
                name: "IX_CondicionesPago_IdUsuarioUltimaModificacion",
                schema: "dbo",
                table: "CondicionesPago",
                column: "IdUsuarioUltimaModificacion");

            migrationBuilder.CreateIndex(
                name: "UQ_CondicionesPago_Codigo",
                schema: "dbo",
                table: "CondicionesPago",
                column: "Codigo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Contactos_IdUsuarioCreacion",
                schema: "dbo",
                table: "Contactos",
                column: "IdUsuarioCreacion");

            migrationBuilder.CreateIndex(
                name: "IX_Contactos_IdUsuarioUltimaModificacion",
                schema: "dbo",
                table: "Contactos",
                column: "IdUsuarioUltimaModificacion");

            migrationBuilder.CreateIndex(
                name: "IX_Depositos_IdUsuarioCreacion",
                schema: "dbo",
                table: "Depositos",
                column: "IdUsuarioCreacion");

            migrationBuilder.CreateIndex(
                name: "IX_Depositos_IdUsuarioUltimaModificacion",
                schema: "dbo",
                table: "Depositos",
                column: "IdUsuarioUltimaModificacion");

            migrationBuilder.CreateIndex(
                name: "UQ_Depositos_Codigo",
                schema: "dbo",
                table: "Depositos",
                column: "Codigo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Domicilios_IdProvincia",
                schema: "dbo",
                table: "Domicilios",
                column: "IdProvincia");

            migrationBuilder.CreateIndex(
                name: "IX_Domicilios_IdUsuarioCreacion",
                schema: "dbo",
                table: "Domicilios",
                column: "IdUsuarioCreacion");

            migrationBuilder.CreateIndex(
                name: "IX_Domicilios_IdUsuarioUltimaModificacion",
                schema: "dbo",
                table: "Domicilios",
                column: "IdUsuarioUltimaModificacion");

            migrationBuilder.CreateIndex(
                name: "IX_Domicilios_TipoReferencia_IdReferencia",
                schema: "dbo",
                table: "Domicilios",
                columns: new[] { "TipoReferencia", "IdReferencia" });

            migrationBuilder.CreateIndex(
                name: "IX_Emails_IdUsuarioCreacion",
                schema: "dbo",
                table: "Emails",
                column: "IdUsuarioCreacion");

            migrationBuilder.CreateIndex(
                name: "IX_Emails_IdUsuarioUltimaModificacion",
                schema: "dbo",
                table: "Emails",
                column: "IdUsuarioUltimaModificacion");

            migrationBuilder.CreateIndex(
                name: "IX_Emails_TipoReferencia_IdReferencia",
                schema: "dbo",
                table: "Emails",
                columns: new[] { "TipoReferencia", "IdReferencia" });

            migrationBuilder.CreateIndex(
                name: "IX_EstadosCaja_IdUsuarioCreacion",
                schema: "dbo",
                table: "EstadosCaja",
                column: "IdUsuarioCreacion");

            migrationBuilder.CreateIndex(
                name: "IX_EstadosCaja_IdUsuarioUltimaModificacion",
                schema: "dbo",
                table: "EstadosCaja",
                column: "IdUsuarioUltimaModificacion");

            migrationBuilder.CreateIndex(
                name: "UQ_EstadosCaja_Codigo",
                schema: "dbo",
                table: "EstadosCaja",
                column: "Codigo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EstadosCompra_IdUsuarioCreacion",
                schema: "dbo",
                table: "EstadosCompra",
                column: "IdUsuarioCreacion");

            migrationBuilder.CreateIndex(
                name: "IX_EstadosCompra_IdUsuarioUltimaModificacion",
                schema: "dbo",
                table: "EstadosCompra",
                column: "IdUsuarioUltimaModificacion");

            migrationBuilder.CreateIndex(
                name: "UQ_EstadosCompra_Codigo",
                schema: "dbo",
                table: "EstadosCompra",
                column: "Codigo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EstadosFactura_IdUsuarioCreacion",
                schema: "dbo",
                table: "EstadosFactura",
                column: "IdUsuarioCreacion");

            migrationBuilder.CreateIndex(
                name: "IX_EstadosFactura_IdUsuarioUltimaModificacion",
                schema: "dbo",
                table: "EstadosFactura",
                column: "IdUsuarioUltimaModificacion");

            migrationBuilder.CreateIndex(
                name: "UQ_EstadosFactura_Codigo",
                schema: "dbo",
                table: "EstadosFactura",
                column: "Codigo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EstadosPedido_IdUsuarioCreacion",
                schema: "dbo",
                table: "EstadosPedido",
                column: "IdUsuarioCreacion");

            migrationBuilder.CreateIndex(
                name: "IX_EstadosPedido_IdUsuarioUltimaModificacion",
                schema: "dbo",
                table: "EstadosPedido",
                column: "IdUsuarioUltimaModificacion");

            migrationBuilder.CreateIndex(
                name: "UQ_EstadosPedido_Codigo",
                schema: "dbo",
                table: "EstadosPedido",
                column: "Codigo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EstadosProceso_IdUsuarioCreacion",
                schema: "dbo",
                table: "EstadosProceso",
                column: "IdUsuarioCreacion");

            migrationBuilder.CreateIndex(
                name: "IX_EstadosProceso_IdUsuarioUltimaModificacion",
                schema: "dbo",
                table: "EstadosProceso",
                column: "IdUsuarioUltimaModificacion");

            migrationBuilder.CreateIndex(
                name: "UQ_EstadosProceso_Codigo",
                schema: "dbo",
                table: "EstadosProceso",
                column: "Codigo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Facturas_IdCliente",
                schema: "dbo",
                table: "Facturas",
                column: "IdCliente");

            migrationBuilder.CreateIndex(
                name: "IX_Facturas_IdCondicionPago",
                schema: "dbo",
                table: "Facturas",
                column: "IdCondicionPago");

            migrationBuilder.CreateIndex(
                name: "IX_Facturas_IdTipoComprobante",
                schema: "dbo",
                table: "Facturas",
                column: "IdTipoComprobante");

            migrationBuilder.CreateIndex(
                name: "IX_FacturasDetalle_IdFactura",
                schema: "dbo",
                table: "FacturasDetalle",
                column: "IdFactura");

            migrationBuilder.CreateIndex(
                name: "IX_FacturasDetalle_IdProducto",
                schema: "dbo",
                table: "FacturasDetalle",
                column: "IdProducto");

            migrationBuilder.CreateIndex(
                name: "IX_ListaPreciosDetalle_IdProducto",
                schema: "dbo",
                table: "ListaPreciosDetalle",
                column: "IdProducto");

            migrationBuilder.CreateIndex(
                name: "IX_ListaPreciosDetalle_IdUsuarioCreacion",
                schema: "dbo",
                table: "ListaPreciosDetalle",
                column: "IdUsuarioCreacion");

            migrationBuilder.CreateIndex(
                name: "IX_ListaPreciosDetalle_IdUsuarioUltimaModificacion",
                schema: "dbo",
                table: "ListaPreciosDetalle",
                column: "IdUsuarioUltimaModificacion");

            migrationBuilder.CreateIndex(
                name: "UQ_ListaPreciosDetalle",
                schema: "dbo",
                table: "ListaPreciosDetalle",
                columns: new[] { "IdListaPrecio", "IdProducto" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ListasPrecios_IdUsuarioCreacion",
                schema: "dbo",
                table: "ListasPrecios",
                column: "IdUsuarioCreacion");

            migrationBuilder.CreateIndex(
                name: "IX_ListasPrecios_IdUsuarioUltimaModificacion",
                schema: "dbo",
                table: "ListasPrecios",
                column: "IdUsuarioUltimaModificacion");

            migrationBuilder.CreateIndex(
                name: "UQ_ListasPrecios_Codigo",
                schema: "dbo",
                table: "ListasPrecios",
                column: "Codigo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MediosPago_IdUsuarioCreacion",
                schema: "dbo",
                table: "MediosPago",
                column: "IdUsuarioCreacion");

            migrationBuilder.CreateIndex(
                name: "IX_MediosPago_IdUsuarioUltimaModificacion",
                schema: "dbo",
                table: "MediosPago",
                column: "IdUsuarioUltimaModificacion");

            migrationBuilder.CreateIndex(
                name: "UQ_MediosPago_Codigo",
                schema: "dbo",
                table: "MediosPago",
                column: "Codigo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Monedas_IdUsuarioCreacion",
                schema: "dbo",
                table: "Monedas",
                column: "IdUsuarioCreacion");

            migrationBuilder.CreateIndex(
                name: "IX_Monedas_IdUsuarioUltimaModificacion",
                schema: "dbo",
                table: "Monedas",
                column: "IdUsuarioUltimaModificacion");

            migrationBuilder.CreateIndex(
                name: "UQ_Monedas_Codigo",
                schema: "dbo",
                table: "Monedas",
                column: "Codigo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MotivosAnulacion_IdUsuarioCreacion",
                schema: "dbo",
                table: "MotivosAnulacion",
                column: "IdUsuarioCreacion");

            migrationBuilder.CreateIndex(
                name: "IX_MotivosAnulacion_IdUsuarioUltimaModificacion",
                schema: "dbo",
                table: "MotivosAnulacion",
                column: "IdUsuarioUltimaModificacion");

            migrationBuilder.CreateIndex(
                name: "UQ_MotivosAnulacion_Codigo",
                schema: "dbo",
                table: "MotivosAnulacion",
                column: "Codigo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MovimientosStock_IdDeposito",
                schema: "dbo",
                table: "MovimientosStock",
                column: "IdDeposito");

            migrationBuilder.CreateIndex(
                name: "IX_MovimientosStock_IdTipoMovimientoStock",
                schema: "dbo",
                table: "MovimientosStock",
                column: "IdTipoMovimientoStock");

            migrationBuilder.CreateIndex(
                name: "IX_MovimientosStock_IdUsuarioCreacion",
                schema: "dbo",
                table: "MovimientosStock",
                column: "IdUsuarioCreacion");

            migrationBuilder.CreateIndex(
                name: "IX_MovimientosStock_IdUsuarioResponsable",
                schema: "dbo",
                table: "MovimientosStock",
                column: "IdUsuarioResponsable");

            migrationBuilder.CreateIndex(
                name: "IX_MovimientosStock_IdUsuarioUltimaModificacion",
                schema: "dbo",
                table: "MovimientosStock",
                column: "IdUsuarioUltimaModificacion");

            migrationBuilder.CreateIndex(
                name: "IX_MovimientoStockDetalle_IdMovimientoStock",
                schema: "dbo",
                table: "MovimientoStockDetalle",
                column: "IdMovimientoStock");

            migrationBuilder.CreateIndex(
                name: "IX_MovimientoStockDetalle_IdProducto",
                schema: "dbo",
                table: "MovimientoStockDetalle",
                column: "IdProducto");

            migrationBuilder.CreateIndex(
                name: "IX_MovimientoStockDetalle_IdUsuarioCreacion",
                schema: "dbo",
                table: "MovimientoStockDetalle",
                column: "IdUsuarioCreacion");

            migrationBuilder.CreateIndex(
                name: "IX_MovimientoStockDetalle_IdUsuarioUltimaModificacion",
                schema: "dbo",
                table: "MovimientoStockDetalle",
                column: "IdUsuarioUltimaModificacion");

            migrationBuilder.CreateIndex(
                name: "IX_PagosClientes_IdCliente",
                schema: "dbo",
                table: "PagosClientes",
                column: "IdCliente");

            migrationBuilder.CreateIndex(
                name: "IX_PagosClientes_IdFactura",
                schema: "dbo",
                table: "PagosClientes",
                column: "IdFactura");

            migrationBuilder.CreateIndex(
                name: "IX_PagosClientes_IdMedioPago",
                schema: "dbo",
                table: "PagosClientes",
                column: "IdMedioPago");

            migrationBuilder.CreateIndex(
                name: "IX_PagosProveedores_IdMedioPago",
                schema: "dbo",
                table: "PagosProveedores",
                column: "IdMedioPago");

            migrationBuilder.CreateIndex(
                name: "IX_PagosProveedores_IdProveedor",
                schema: "dbo",
                table: "PagosProveedores",
                column: "IdProveedor");

            migrationBuilder.CreateIndex(
                name: "IX_Productos_IdCategoria",
                schema: "dbo",
                table: "Productos",
                column: "IdCategoria");

            migrationBuilder.CreateIndex(
                name: "IX_Productos_IdSubCategoria",
                schema: "dbo",
                table: "Productos",
                column: "IdSubCategoria");

            migrationBuilder.CreateIndex(
                name: "IX_Productos_IdUsuarioCreacion",
                schema: "dbo",
                table: "Productos",
                column: "IdUsuarioCreacion");

            migrationBuilder.CreateIndex(
                name: "IX_Productos_IdUsuarioUltimaModificacion",
                schema: "dbo",
                table: "Productos",
                column: "IdUsuarioUltimaModificacion");

            migrationBuilder.CreateIndex(
                name: "UQ_Productos_Codigo",
                schema: "dbo",
                table: "Productos",
                column: "Codigo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Proveedores_IdCondicionPago",
                schema: "dbo",
                table: "Proveedores",
                column: "IdCondicionPago");

            migrationBuilder.CreateIndex(
                name: "IX_Proveedores_IdUsuarioCreacion",
                schema: "dbo",
                table: "Proveedores",
                column: "IdUsuarioCreacion");

            migrationBuilder.CreateIndex(
                name: "IX_Proveedores_IdUsuarioUltimaModificacion",
                schema: "dbo",
                table: "Proveedores",
                column: "IdUsuarioUltimaModificacion");

            migrationBuilder.CreateIndex(
                name: "UQ_Proveedores_Codigo",
                schema: "dbo",
                table: "Proveedores",
                column: "Codigo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Provincias_IdUsuarioCreacion",
                schema: "dbo",
                table: "Provincias",
                column: "IdUsuarioCreacion");

            migrationBuilder.CreateIndex(
                name: "IX_Provincias_IdUsuarioUltimaModificacion",
                schema: "dbo",
                table: "Provincias",
                column: "IdUsuarioUltimaModificacion");

            migrationBuilder.CreateIndex(
                name: "UQ_Provincias_Codigo",
                schema: "dbo",
                table: "Provincias",
                column: "Codigo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Stock_IdProducto",
                schema: "dbo",
                table: "Stock",
                column: "IdProducto");

            migrationBuilder.CreateIndex(
                name: "UQ_Stock_DepositoProducto",
                schema: "dbo",
                table: "Stock",
                columns: new[] { "IdDeposito", "IdProducto" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SubCategorias_IdCategoria",
                schema: "dbo",
                table: "SubCategorias",
                column: "IdCategoria");

            migrationBuilder.CreateIndex(
                name: "IX_SubCategorias_IdUsuarioCreacion",
                schema: "dbo",
                table: "SubCategorias",
                column: "IdUsuarioCreacion");

            migrationBuilder.CreateIndex(
                name: "IX_SubCategorias_IdUsuarioUltimaModificacion",
                schema: "dbo",
                table: "SubCategorias",
                column: "IdUsuarioUltimaModificacion");

            migrationBuilder.CreateIndex(
                name: "IX_Telefonos_IdUsuarioCreacion",
                schema: "dbo",
                table: "Telefonos",
                column: "IdUsuarioCreacion");

            migrationBuilder.CreateIndex(
                name: "IX_Telefonos_IdUsuarioUltimaModificacion",
                schema: "dbo",
                table: "Telefonos",
                column: "IdUsuarioUltimaModificacion");

            migrationBuilder.CreateIndex(
                name: "IX_Telefonos_TipoReferencia_IdReferencia",
                schema: "dbo",
                table: "Telefonos",
                columns: new[] { "TipoReferencia", "IdReferencia" });

            migrationBuilder.CreateIndex(
                name: "IX_TiposComprobante_IdUsuarioCreacion",
                schema: "dbo",
                table: "TiposComprobante",
                column: "IdUsuarioCreacion");

            migrationBuilder.CreateIndex(
                name: "IX_TiposComprobante_IdUsuarioUltimaModificacion",
                schema: "dbo",
                table: "TiposComprobante",
                column: "IdUsuarioUltimaModificacion");

            migrationBuilder.CreateIndex(
                name: "UQ_TiposComprobante_Codigo",
                schema: "dbo",
                table: "TiposComprobante",
                column: "Codigo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TiposImpuesto_IdUsuarioCreacion",
                schema: "dbo",
                table: "TiposImpuesto",
                column: "IdUsuarioCreacion");

            migrationBuilder.CreateIndex(
                name: "IX_TiposImpuesto_IdUsuarioUltimaModificacion",
                schema: "dbo",
                table: "TiposImpuesto",
                column: "IdUsuarioUltimaModificacion");

            migrationBuilder.CreateIndex(
                name: "UQ_TiposImpuesto_Codigo",
                schema: "dbo",
                table: "TiposImpuesto",
                column: "Codigo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TiposMovimientoCaja_IdUsuarioCreacion",
                schema: "dbo",
                table: "TiposMovimientoCaja",
                column: "IdUsuarioCreacion");

            migrationBuilder.CreateIndex(
                name: "IX_TiposMovimientoCaja_IdUsuarioUltimaModificacion",
                schema: "dbo",
                table: "TiposMovimientoCaja",
                column: "IdUsuarioUltimaModificacion");

            migrationBuilder.CreateIndex(
                name: "UQ_TiposMovimientoCaja_Codigo",
                schema: "dbo",
                table: "TiposMovimientoCaja",
                column: "Codigo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TiposMovimientoStock_IdUsuarioCreacion",
                schema: "dbo",
                table: "TiposMovimientoStock",
                column: "IdUsuarioCreacion");

            migrationBuilder.CreateIndex(
                name: "IX_TiposMovimientoStock_IdUsuarioUltimaModificacion",
                schema: "dbo",
                table: "TiposMovimientoStock",
                column: "IdUsuarioUltimaModificacion");

            migrationBuilder.CreateIndex(
                name: "UQ_TiposMovimientoStock_Codigo",
                schema: "dbo",
                table: "TiposMovimientoStock",
                column: "Codigo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TiposUsuario_IdUsuarioCreacion",
                schema: "dbo",
                table: "TiposUsuario",
                column: "IdUsuarioCreacion");

            migrationBuilder.CreateIndex(
                name: "IX_TiposUsuario_IdUsuarioUltimaModificacion",
                schema: "dbo",
                table: "TiposUsuario",
                column: "IdUsuarioUltimaModificacion");

            migrationBuilder.CreateIndex(
                name: "UQ_TiposUsuario_Descripcion",
                schema: "dbo",
                table: "TiposUsuario",
                column: "Descripcion",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_IdTipoUsuario",
                schema: "dbo",
                table: "Usuarios",
                column: "IdTipoUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_IdUsuarioCreacion",
                schema: "dbo",
                table: "Usuarios",
                column: "IdUsuarioCreacion");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_IdUsuarioUltimaModificacion",
                schema: "dbo",
                table: "Usuarios",
                column: "IdUsuarioUltimaModificacion");

            migrationBuilder.CreateIndex(
                name: "UQ_Usuarios_Usuario",
                schema: "dbo",
                table: "Usuarios",
                column: "Usuario",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ValoresCartera_IdCliente",
                schema: "dbo",
                table: "ValoresCartera",
                column: "IdCliente");

            migrationBuilder.CreateIndex(
                name: "IX_ValoresCartera_IdPagoCliente",
                schema: "dbo",
                table: "ValoresCartera",
                column: "IdPagoCliente",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AlicuotasImpuesto_TiposImpuesto",
                schema: "dbo",
                table: "AlicuotasImpuesto",
                column: "IdTipoImpuesto",
                principalSchema: "dbo",
                principalTable: "TiposImpuesto",
                principalColumn: "IdTipoImpuesto",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AlicuotasImpuesto_UsuariosCreacion",
                schema: "dbo",
                table: "AlicuotasImpuesto",
                column: "IdUsuarioCreacion",
                principalSchema: "dbo",
                principalTable: "Usuarios",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AlicuotasImpuesto_UsuariosModificacion",
                schema: "dbo",
                table: "AlicuotasImpuesto",
                column: "IdUsuarioUltimaModificacion",
                principalSchema: "dbo",
                principalTable: "Usuarios",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CajaCabecera_Estado",
                schema: "dbo",
                table: "CajaCabecera",
                column: "IdEstadoCaja",
                principalSchema: "dbo",
                principalTable: "EstadosCaja",
                principalColumn: "IdEstadoCaja",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CajaCabecera_Responsable",
                schema: "dbo",
                table: "CajaCabecera",
                column: "IdUsuarioResponsable",
                principalSchema: "dbo",
                principalTable: "Usuarios",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CajaCabecera_UsuarioCreacion",
                schema: "dbo",
                table: "CajaCabecera",
                column: "IdUsuarioCreacion",
                principalSchema: "dbo",
                principalTable: "Usuarios",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CajaCabecera_UsuarioModificacion",
                schema: "dbo",
                table: "CajaCabecera",
                column: "IdUsuarioUltimaModificacion",
                principalSchema: "dbo",
                principalTable: "Usuarios",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CajaDetalle_MedioPago",
                schema: "dbo",
                table: "CajaDetalle",
                column: "IdMedioPago",
                principalSchema: "dbo",
                principalTable: "MediosPago",
                principalColumn: "IdMedioPago",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CajaDetalle_Moneda",
                schema: "dbo",
                table: "CajaDetalle",
                column: "IdMoneda",
                principalSchema: "dbo",
                principalTable: "Monedas",
                principalColumn: "IdMoneda",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CajaDetalle_TipoMovimiento",
                schema: "dbo",
                table: "CajaDetalle",
                column: "IdTipoMovimientoCaja",
                principalSchema: "dbo",
                principalTable: "TiposMovimientoCaja",
                principalColumn: "IdTipoMovimientoCaja",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CajaDetalle_UsuarioCreacion",
                schema: "dbo",
                table: "CajaDetalle",
                column: "IdUsuarioCreacion",
                principalSchema: "dbo",
                principalTable: "Usuarios",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CajaDetalle_UsuarioModificacion",
                schema: "dbo",
                table: "CajaDetalle",
                column: "IdUsuarioUltimaModificacion",
                principalSchema: "dbo",
                principalTable: "Usuarios",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Categorias_UsuariosCreacion",
                schema: "dbo",
                table: "Categorias",
                column: "IdUsuarioCreacion",
                principalSchema: "dbo",
                principalTable: "Usuarios",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Categorias_UsuariosModificacion",
                schema: "dbo",
                table: "Categorias",
                column: "IdUsuarioUltimaModificacion",
                principalSchema: "dbo",
                principalTable: "Usuarios",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ClienteContactos_Clientes",
                schema: "dbo",
                table: "ClienteContactos",
                column: "IdCliente",
                principalSchema: "dbo",
                principalTable: "Clientes",
                principalColumn: "IdCliente",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ClienteContactos_Contactos",
                schema: "dbo",
                table: "ClienteContactos",
                column: "IdContacto",
                principalSchema: "dbo",
                principalTable: "Contactos",
                principalColumn: "IdContacto",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Clientes_CondicionesPago",
                schema: "dbo",
                table: "Clientes",
                column: "IdCondicionPago",
                principalSchema: "dbo",
                principalTable: "CondicionesPago",
                principalColumn: "IdCondicionPago",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Clientes_UsuariosCreacion",
                schema: "dbo",
                table: "Clientes",
                column: "IdUsuarioCreacion",
                principalSchema: "dbo",
                principalTable: "Usuarios",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Clientes_UsuariosModificacion",
                schema: "dbo",
                table: "Clientes",
                column: "IdUsuarioUltimaModificacion",
                principalSchema: "dbo",
                principalTable: "Usuarios",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CondicionesIVA_UsuariosCreacion",
                schema: "dbo",
                table: "CondicionesIVA",
                column: "IdUsuarioCreacion",
                principalSchema: "dbo",
                principalTable: "Usuarios",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CondicionesIVA_UsuariosModificacion",
                schema: "dbo",
                table: "CondicionesIVA",
                column: "IdUsuarioUltimaModificacion",
                principalSchema: "dbo",
                principalTable: "Usuarios",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CondicionesPago_UsuariosCreacion",
                schema: "dbo",
                table: "CondicionesPago",
                column: "IdUsuarioCreacion",
                principalSchema: "dbo",
                principalTable: "Usuarios",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CondicionesPago_UsuariosModificacion",
                schema: "dbo",
                table: "CondicionesPago",
                column: "IdUsuarioUltimaModificacion",
                principalSchema: "dbo",
                principalTable: "Usuarios",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Contactos_UsuariosCreacion",
                schema: "dbo",
                table: "Contactos",
                column: "IdUsuarioCreacion",
                principalSchema: "dbo",
                principalTable: "Usuarios",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Contactos_UsuariosModificacion",
                schema: "dbo",
                table: "Contactos",
                column: "IdUsuarioUltimaModificacion",
                principalSchema: "dbo",
                principalTable: "Usuarios",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Depositos_UsuariosCreacion",
                schema: "dbo",
                table: "Depositos",
                column: "IdUsuarioCreacion",
                principalSchema: "dbo",
                principalTable: "Usuarios",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Depositos_UsuariosModificacion",
                schema: "dbo",
                table: "Depositos",
                column: "IdUsuarioUltimaModificacion",
                principalSchema: "dbo",
                principalTable: "Usuarios",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Domicilios_Provincias",
                schema: "dbo",
                table: "Domicilios",
                column: "IdProvincia",
                principalSchema: "dbo",
                principalTable: "Provincias",
                principalColumn: "IdProvincia",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Domicilios_UsuariosCreacion",
                schema: "dbo",
                table: "Domicilios",
                column: "IdUsuarioCreacion",
                principalSchema: "dbo",
                principalTable: "Usuarios",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Domicilios_UsuariosModificacion",
                schema: "dbo",
                table: "Domicilios",
                column: "IdUsuarioUltimaModificacion",
                principalSchema: "dbo",
                principalTable: "Usuarios",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Emails_UsuariosCreacion",
                schema: "dbo",
                table: "Emails",
                column: "IdUsuarioCreacion",
                principalSchema: "dbo",
                principalTable: "Usuarios",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Emails_UsuariosModificacion",
                schema: "dbo",
                table: "Emails",
                column: "IdUsuarioUltimaModificacion",
                principalSchema: "dbo",
                principalTable: "Usuarios",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_EstadosCaja_UsuariosCreacion",
                schema: "dbo",
                table: "EstadosCaja",
                column: "IdUsuarioCreacion",
                principalSchema: "dbo",
                principalTable: "Usuarios",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_EstadosCaja_UsuariosModificacion",
                schema: "dbo",
                table: "EstadosCaja",
                column: "IdUsuarioUltimaModificacion",
                principalSchema: "dbo",
                principalTable: "Usuarios",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_EstadosCompra_UsuariosCreacion",
                schema: "dbo",
                table: "EstadosCompra",
                column: "IdUsuarioCreacion",
                principalSchema: "dbo",
                principalTable: "Usuarios",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_EstadosCompra_UsuariosModificacion",
                schema: "dbo",
                table: "EstadosCompra",
                column: "IdUsuarioUltimaModificacion",
                principalSchema: "dbo",
                principalTable: "Usuarios",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_EstadosFactura_UsuariosCreacion",
                schema: "dbo",
                table: "EstadosFactura",
                column: "IdUsuarioCreacion",
                principalSchema: "dbo",
                principalTable: "Usuarios",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_EstadosFactura_UsuariosModificacion",
                schema: "dbo",
                table: "EstadosFactura",
                column: "IdUsuarioUltimaModificacion",
                principalSchema: "dbo",
                principalTable: "Usuarios",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_EstadosPedido_UsuariosCreacion",
                schema: "dbo",
                table: "EstadosPedido",
                column: "IdUsuarioCreacion",
                principalSchema: "dbo",
                principalTable: "Usuarios",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_EstadosPedido_UsuariosModificacion",
                schema: "dbo",
                table: "EstadosPedido",
                column: "IdUsuarioUltimaModificacion",
                principalSchema: "dbo",
                principalTable: "Usuarios",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_EstadosProceso_UsuariosCreacion",
                schema: "dbo",
                table: "EstadosProceso",
                column: "IdUsuarioCreacion",
                principalSchema: "dbo",
                principalTable: "Usuarios",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_EstadosProceso_UsuariosModificacion",
                schema: "dbo",
                table: "EstadosProceso",
                column: "IdUsuarioUltimaModificacion",
                principalSchema: "dbo",
                principalTable: "Usuarios",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Facturas_TiposComprobante_IdTipoComprobante",
                schema: "dbo",
                table: "Facturas",
                column: "IdTipoComprobante",
                principalSchema: "dbo",
                principalTable: "TiposComprobante",
                principalColumn: "IdTipoComprobante",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FacturasDetalle_Productos_IdProducto",
                schema: "dbo",
                table: "FacturasDetalle",
                column: "IdProducto",
                principalSchema: "dbo",
                principalTable: "Productos",
                principalColumn: "IdProducto",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ListaPreciosDetalle_ListasPrecios",
                schema: "dbo",
                table: "ListaPreciosDetalle",
                column: "IdListaPrecio",
                principalSchema: "dbo",
                principalTable: "ListasPrecios",
                principalColumn: "IdListaPrecio",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ListaPreciosDetalle_Productos",
                schema: "dbo",
                table: "ListaPreciosDetalle",
                column: "IdProducto",
                principalSchema: "dbo",
                principalTable: "Productos",
                principalColumn: "IdProducto",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ListaPreciosDetalle_UsuariosCreacion",
                schema: "dbo",
                table: "ListaPreciosDetalle",
                column: "IdUsuarioCreacion",
                principalSchema: "dbo",
                principalTable: "Usuarios",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ListaPreciosDetalle_UsuariosModificacion",
                schema: "dbo",
                table: "ListaPreciosDetalle",
                column: "IdUsuarioUltimaModificacion",
                principalSchema: "dbo",
                principalTable: "Usuarios",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ListasPrecios_UsuariosCreacion",
                schema: "dbo",
                table: "ListasPrecios",
                column: "IdUsuarioCreacion",
                principalSchema: "dbo",
                principalTable: "Usuarios",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ListasPrecios_UsuariosModificacion",
                schema: "dbo",
                table: "ListasPrecios",
                column: "IdUsuarioUltimaModificacion",
                principalSchema: "dbo",
                principalTable: "Usuarios",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MediosPago_UsuariosCreacion",
                schema: "dbo",
                table: "MediosPago",
                column: "IdUsuarioCreacion",
                principalSchema: "dbo",
                principalTable: "Usuarios",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MediosPago_UsuariosModificacion",
                schema: "dbo",
                table: "MediosPago",
                column: "IdUsuarioUltimaModificacion",
                principalSchema: "dbo",
                principalTable: "Usuarios",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Monedas_UsuariosCreacion",
                schema: "dbo",
                table: "Monedas",
                column: "IdUsuarioCreacion",
                principalSchema: "dbo",
                principalTable: "Usuarios",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Monedas_UsuariosModificacion",
                schema: "dbo",
                table: "Monedas",
                column: "IdUsuarioUltimaModificacion",
                principalSchema: "dbo",
                principalTable: "Usuarios",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MotivosAnulacion_UsuariosCreacion",
                schema: "dbo",
                table: "MotivosAnulacion",
                column: "IdUsuarioCreacion",
                principalSchema: "dbo",
                principalTable: "Usuarios",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MotivosAnulacion_UsuariosModificacion",
                schema: "dbo",
                table: "MotivosAnulacion",
                column: "IdUsuarioUltimaModificacion",
                principalSchema: "dbo",
                principalTable: "Usuarios",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MovimientosStock_Responsable",
                schema: "dbo",
                table: "MovimientosStock",
                column: "IdUsuarioResponsable",
                principalSchema: "dbo",
                principalTable: "Usuarios",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MovimientosStock_UsuarioCreacion",
                schema: "dbo",
                table: "MovimientosStock",
                column: "IdUsuarioCreacion",
                principalSchema: "dbo",
                principalTable: "Usuarios",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MovimientosStock_UsuarioModificacion",
                schema: "dbo",
                table: "MovimientosStock",
                column: "IdUsuarioUltimaModificacion",
                principalSchema: "dbo",
                principalTable: "Usuarios",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MovimientosStock_Tipo",
                schema: "dbo",
                table: "MovimientosStock",
                column: "IdTipoMovimientoStock",
                principalSchema: "dbo",
                principalTable: "TiposMovimientoStock",
                principalColumn: "IdTipoMovimientoStock",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MovimientoStockDetalle_Producto",
                schema: "dbo",
                table: "MovimientoStockDetalle",
                column: "IdProducto",
                principalSchema: "dbo",
                principalTable: "Productos",
                principalColumn: "IdProducto",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MovimientoStockDetalle_UsuarioCreacion",
                schema: "dbo",
                table: "MovimientoStockDetalle",
                column: "IdUsuarioCreacion",
                principalSchema: "dbo",
                principalTable: "Usuarios",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MovimientoStockDetalle_UsuarioModificacion",
                schema: "dbo",
                table: "MovimientoStockDetalle",
                column: "IdUsuarioUltimaModificacion",
                principalSchema: "dbo",
                principalTable: "Usuarios",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PagosProveedores_Proveedores_IdProveedor",
                schema: "dbo",
                table: "PagosProveedores",
                column: "IdProveedor",
                principalSchema: "dbo",
                principalTable: "Proveedores",
                principalColumn: "IdProveedor",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Productos_SubCategorias",
                schema: "dbo",
                table: "Productos",
                column: "IdSubCategoria",
                principalSchema: "dbo",
                principalTable: "SubCategorias",
                principalColumn: "IdSubCategoria",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Productos_UsuariosCreacion",
                schema: "dbo",
                table: "Productos",
                column: "IdUsuarioCreacion",
                principalSchema: "dbo",
                principalTable: "Usuarios",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Productos_UsuariosModificacion",
                schema: "dbo",
                table: "Productos",
                column: "IdUsuarioUltimaModificacion",
                principalSchema: "dbo",
                principalTable: "Usuarios",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Proveedores_UsuariosCreacion",
                schema: "dbo",
                table: "Proveedores",
                column: "IdUsuarioCreacion",
                principalSchema: "dbo",
                principalTable: "Usuarios",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Proveedores_UsuariosModificacion",
                schema: "dbo",
                table: "Proveedores",
                column: "IdUsuarioUltimaModificacion",
                principalSchema: "dbo",
                principalTable: "Usuarios",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Provincias_UsuariosCreacion",
                schema: "dbo",
                table: "Provincias",
                column: "IdUsuarioCreacion",
                principalSchema: "dbo",
                principalTable: "Usuarios",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Provincias_UsuariosModificacion",
                schema: "dbo",
                table: "Provincias",
                column: "IdUsuarioUltimaModificacion",
                principalSchema: "dbo",
                principalTable: "Usuarios",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SubCategorias_UsuariosCreacion",
                schema: "dbo",
                table: "SubCategorias",
                column: "IdUsuarioCreacion",
                principalSchema: "dbo",
                principalTable: "Usuarios",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SubCategorias_UsuariosModificacion",
                schema: "dbo",
                table: "SubCategorias",
                column: "IdUsuarioUltimaModificacion",
                principalSchema: "dbo",
                principalTable: "Usuarios",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Telefonos_UsuariosCreacion",
                schema: "dbo",
                table: "Telefonos",
                column: "IdUsuarioCreacion",
                principalSchema: "dbo",
                principalTable: "Usuarios",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Telefonos_UsuariosModificacion",
                schema: "dbo",
                table: "Telefonos",
                column: "IdUsuarioUltimaModificacion",
                principalSchema: "dbo",
                principalTable: "Usuarios",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TiposComprobante_UsuariosCreacion",
                schema: "dbo",
                table: "TiposComprobante",
                column: "IdUsuarioCreacion",
                principalSchema: "dbo",
                principalTable: "Usuarios",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TiposComprobante_UsuariosModificacion",
                schema: "dbo",
                table: "TiposComprobante",
                column: "IdUsuarioUltimaModificacion",
                principalSchema: "dbo",
                principalTable: "Usuarios",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TiposImpuesto_UsuariosCreacion",
                schema: "dbo",
                table: "TiposImpuesto",
                column: "IdUsuarioCreacion",
                principalSchema: "dbo",
                principalTable: "Usuarios",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TiposImpuesto_UsuariosModificacion",
                schema: "dbo",
                table: "TiposImpuesto",
                column: "IdUsuarioUltimaModificacion",
                principalSchema: "dbo",
                principalTable: "Usuarios",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TiposMovimientoCaja_UsuariosCreacion",
                schema: "dbo",
                table: "TiposMovimientoCaja",
                column: "IdUsuarioCreacion",
                principalSchema: "dbo",
                principalTable: "Usuarios",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TiposMovimientoCaja_UsuariosModificacion",
                schema: "dbo",
                table: "TiposMovimientoCaja",
                column: "IdUsuarioUltimaModificacion",
                principalSchema: "dbo",
                principalTable: "Usuarios",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TiposMovimientoStock_UsuariosCreacion",
                schema: "dbo",
                table: "TiposMovimientoStock",
                column: "IdUsuarioCreacion",
                principalSchema: "dbo",
                principalTable: "Usuarios",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TiposMovimientoStock_UsuariosModificacion",
                schema: "dbo",
                table: "TiposMovimientoStock",
                column: "IdUsuarioUltimaModificacion",
                principalSchema: "dbo",
                principalTable: "Usuarios",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TiposUsuario_UsuarioCreacion",
                schema: "dbo",
                table: "TiposUsuario",
                column: "IdUsuarioCreacion",
                principalSchema: "dbo",
                principalTable: "Usuarios",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TiposUsuario_UsuarioModificacion",
                schema: "dbo",
                table: "TiposUsuario",
                column: "IdUsuarioUltimaModificacion",
                principalSchema: "dbo",
                principalTable: "Usuarios",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TiposUsuario_UsuarioCreacion",
                schema: "dbo",
                table: "TiposUsuario");

            migrationBuilder.DropForeignKey(
                name: "FK_TiposUsuario_UsuarioModificacion",
                schema: "dbo",
                table: "TiposUsuario");

            migrationBuilder.DropTable(
                name: "AlicuotasImpuesto",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "CajaDetalle",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "ClienteContactos",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "CondicionesIVA",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Domicilios",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Emails",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "EstadosCompra",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "EstadosFactura",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "EstadosPedido",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "EstadosProceso",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FacturasDetalle",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "ListaPreciosDetalle",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "MotivosAnulacion",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "MovimientoStockDetalle",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "PagosProveedores",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Stock",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Telefonos",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "ValoresCartera",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "TiposImpuesto",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "CajaCabecera",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Monedas",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "TiposMovimientoCaja",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Contactos",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Provincias",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "ListasPrecios",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "MovimientosStock",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Proveedores",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Productos",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "PagosClientes",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "EstadosCaja",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Depositos",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "TiposMovimientoStock",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "SubCategorias",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Facturas",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "MediosPago",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Categorias",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Clientes",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "TiposComprobante",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "CondicionesPago",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Usuarios",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "TiposUsuario",
                schema: "dbo");
        }
    }
}
