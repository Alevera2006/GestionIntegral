using GestionIntegral.WebClient.Models;
using GestionIntegral.WebClient.Models.Parametros;
using GestionIntegral.WebClient.Services;
using Microsoft.AspNetCore.Mvc;

namespace GestionIntegral.WebClient.Controllers;

public class ParametrosController : BaseController
{
    public ParametrosController(IApiClient apiClient) : base(apiClient)
    {
    }

    public IActionResult Index(string? grupo)
    {
        var tarjetas = new List<TarjetaParametroViewModel>
        {
            // Configuración general
            new() { Key = "provincias", Titulo = "Provincias", Icono = "fa-map-marker-alt", ApiUrl = "api/provincias", RequiereCodigoAFIP = true },
            new() { Key = "tiposComprobante", Titulo = "Tipos de Comprobante", Icono = "fa-file-invoice", Grupo = "ventas", ApiUrl = "api/tiposcomprobante", RequiereCodigoAFIP = true },
            new() { Key = "motivosAnulacion", Titulo = "Motivos de Anulación", Icono = "fa-ban", Grupo = "ventas", ApiUrl = "api/motivosanulacion", RequiereCodigoAFIP = false },
            new() { Key = "estadosPedido", Titulo = "Estados de Pedido", Icono = "fa-truck", Grupo = "ventas", ApiUrl = "api/estadospedido", RequiereCodigoAFIP = false },
            new() { Key = "estadosFactura", Titulo = "Estados de Factura", Icono = "fa-file-invoice-dollar", Grupo = "ventas", ApiUrl = "api/estadosfactura", RequiereCodigoAFIP = false },
            new() { Key = "estadosCompra", Titulo = "Estados de Compra", Icono = "fa-shopping-cart", Grupo = "compras", ApiUrl = "api/estadoscompra", RequiereCodigoAFIP = false },
            new() { Key = "estadosProceso", Titulo = "Estados de Proceso", Icono = "fa-cogs", Grupo = "compras", ApiUrl = "api/estadosproceso", RequiereCodigoAFIP = false },
            new() { Key = "condicionesPago", Titulo = "Condiciones de Pago", Icono = "fa-money-check-alt", Grupo = "finanzas", ApiUrl = "api/condicionespago", RequiereCodigoAFIP = false },
            new() { Key = "condicionesIVA", Titulo = "Condiciones de IVA", Icono = "fa-percent", Grupo = "finanzas", ApiUrl = "api/condicionesiva", RequiereCodigoAFIP = false },
            new() { Key = "tiposImpuesto", Titulo = "Tipos de Impuesto", Icono = "fa-receipt", Grupo = "finanzas", ApiUrl = "api/tiposimpuesto", RequiereCodigoAFIP = false },
            new() { Key = "alicuotasImpuesto", Titulo = "Alícuotas de Impuesto", Icono = "fa-calculator", Grupo = "finanzas", ApiUrl = "api/alicuotasimpuesto", RequiereCodigoAFIP = false },
            // Financieros
            new() { Key = "monedas", Titulo = "Monedas", Icono = "fa-coins", Grupo = "finanzas", ApiUrl = "api/monedas", RequiereCodigoAFIP = false },
            new() { Key = "mediosPago", Titulo = "Medios de Pago", Icono = "fa-credit-card", Grupo = "finanzas", ApiUrl = "api/mediospago", RequiereCodigoAFIP = false },
            new() { Key = "tiposMovimientoCaja", Titulo = "Tipos de Movimiento de Caja", Icono = "fa-cash-register", Grupo = "finanzas", ApiUrl = "api/tiposmovimientocaja", RequiereCodigoAFIP = false },
            new() { Key = "estadosCaja", Titulo = "Estados de Caja", Icono = "fa-university", Grupo = "finanzas", ApiUrl = "api/estadoscaja", RequiereCodigoAFIP = false },
            // Stock/Depósitos
            new() { Key = "depositos", Titulo = "Depósitos", Icono = "fa-warehouse", Grupo = "stock", ApiUrl = "api/depositos", RequiereCodigoAFIP = false },
            new() { Key = "tiposMovimientoStock", Titulo = "Tipos de Movimiento de Stock", Icono = "fa-dolly", Grupo = "stock", ApiUrl = "api/tiposmovimientostock", RequiereCodigoAFIP = false },
            // Productos
            new() { Key = "categorias", Titulo = "Categorías", Icono = "fa-tags", ApiUrl = "api/categorias", RequiereCodigoAFIP = false },
            new() { Key = "subCategorias", Titulo = "Subcategorías", Icono = "fa-tag", ApiUrl = "api/subcategorias", RequiereCodigoAFIP = false },
            new() { Key = "listasPrecios", Titulo = "Listas de Precios", Icono = "fa-dollar-sign", Grupo = "ventas", ApiUrl = "api/listasprecios", RequiereCodigoAFIP = false }
        };

        ViewBag.GrupoInicial = grupo ?? "parametricos";
        return View(tarjetas);
    }

    [HttpGet]
    public async Task<IActionResult> Listar(string apiUrl, CancellationToken cancellationToken)
    {
        var response = await ApiClient.GetAsync<BaseResponseDto<IEnumerable<object>>>(apiUrl, cancellationToken);
        return Json(response);
    }

    [HttpPost]
    public async Task<IActionResult> Crear(string apiUrl, [FromBody] ParametroFormDto dto, CancellationToken cancellationToken)
    {
        if (apiUrl.Equals("api/subcategorias", StringComparison.OrdinalIgnoreCase) && dto.IdCategoria is null or <= 0)
            return BadRequest(BaseResponseDto<object>.Error("Seleccione una categoría válida."));

        if (apiUrl.Equals("api/alicuotasimpuesto", StringComparison.OrdinalIgnoreCase) && dto.IdTipoImpuesto is null or <= 0)
            return BadRequest(BaseResponseDto<object>.Error("Seleccione un tipo de impuesto válido."));

        if (apiUrl.Equals("api/tiposmovimientocaja", StringComparison.OrdinalIgnoreCase) || apiUrl.Equals("api/tiposmovimientostock", StringComparison.OrdinalIgnoreCase))
        {
            if (dto.Signo is null || (dto.Signo != 1 && dto.Signo != -1))
                return BadRequest(BaseResponseDto<object>.Error("Seleccione un signo válido: Positivo o Negativo."));
        }

        var request = ConstruirRequest(apiUrl, dto, esCreacion: true);
        var response = await ApiClient.PostAsync<object, BaseResponseDto<object>>(apiUrl, request, cancellationToken);
        return Json(response);
    }

    [HttpPut]
    public async Task<IActionResult> Actualizar(string apiUrl, [FromBody] ParametroFormDto dto, CancellationToken cancellationToken)
    {
        if (dto.Id is null or <= 0)
            return BadRequest(BaseResponseDto<object>.Error("El identificador no es válido."));

        if (apiUrl.Equals("api/subcategorias", StringComparison.OrdinalIgnoreCase) && dto.IdCategoria is null or <= 0)
            return BadRequest(BaseResponseDto<object>.Error("Seleccione una categoría válida."));

        if (apiUrl.Equals("api/alicuotasimpuesto", StringComparison.OrdinalIgnoreCase) && dto.IdTipoImpuesto is null or <= 0)
            return BadRequest(BaseResponseDto<object>.Error("Seleccione un tipo de impuesto válido."));

        if (apiUrl.Equals("api/tiposmovimientocaja", StringComparison.OrdinalIgnoreCase) || apiUrl.Equals("api/tiposmovimientostock", StringComparison.OrdinalIgnoreCase))
        {
            if (dto.Signo is null || (dto.Signo != 1 && dto.Signo != -1))
                return BadRequest(BaseResponseDto<object>.Error("Seleccione un signo válido: Positivo o Negativo."));
        }

        var request = ConstruirRequest(apiUrl, dto, esCreacion: false);
        var response = await ApiClient.PutAsync<object, BaseResponseDto<object>>(apiUrl, request, cancellationToken);
        return Json(response);
    }

    [HttpDelete]
    public async Task<IActionResult> Eliminar(string apiUrl, int id, CancellationToken cancellationToken)
    {
        if (id <= 0)
            return BadRequest(BaseResponseDto<bool>.Error("El identificador debe ser mayor que cero."));

        try
        {
            var ok = await ApiClient.DeleteAsync($"{apiUrl}/{id}", cancellationToken);
            return Json(ok
                ? BaseResponseDto<bool>.Ok(true)
                : BaseResponseDto<bool>.Error("No se pudo eliminar el registro."));
        }
        catch (Exception ex)
        {
            return Json(BaseResponseDto<bool>.Error($"Error al eliminar: {ex.Message}"));
        }
    }

    private static object ConstruirRequest(string apiUrl, ParametroFormDto dto, bool esCreacion)
    {
        var idValue = dto.Id ?? 0;

        return apiUrl.ToLowerInvariant() switch
        {
            "api/provincias" => esCreacion
                ? new { dto.Codigo, dto.Descripcion, CodigoAFIP = dto.CodigoAFIP }
                : new { IdProvincia = idValue, dto.Codigo, dto.Descripcion, CodigoAFIP = dto.CodigoAFIP, dto.Activo },
            "api/tiposcomprobante" => esCreacion
                ? new { dto.Codigo, dto.Descripcion, CodigoAFIP = dto.CodigoAFIP }
                : new { IdTipoComprobante = idValue, dto.Codigo, dto.Descripcion, CodigoAFIP = dto.CodigoAFIP, dto.Activo },
            "api/motivosanulacion" => esCreacion
                ? new { dto.Codigo, dto.Descripcion }
                : new { IdMotivoAnulacion = idValue, dto.Codigo, dto.Descripcion, dto.Activo },
            "api/estadospedido" => esCreacion
                ? new { dto.Codigo, dto.Descripcion }
                : new { IdEstadoPedido = idValue, dto.Codigo, dto.Descripcion, dto.Activo },
            "api/estadosfactura" => esCreacion
                ? new { dto.Codigo, dto.Descripcion }
                : new { IdEstadoFactura = idValue, dto.Codigo, dto.Descripcion, dto.Activo },
            "api/estadoscompra" => esCreacion
                ? new { dto.Codigo, dto.Descripcion }
                : new { IdEstadoCompra = idValue, dto.Codigo, dto.Descripcion, dto.Activo },
            "api/estadosproceso" => esCreacion
                ? new { dto.Codigo, dto.Descripcion }
                : new { IdEstadoProceso = idValue, dto.Codigo, dto.Descripcion, dto.Activo },
            "api/condicionespago" => esCreacion
                ? new { dto.Codigo, dto.Descripcion, Dias = 0 }
                : new { IdCondicionPago = idValue, dto.Codigo, dto.Descripcion, Dias = 0, dto.Activo },
            "api/condicionesiva" => esCreacion
                ? new { dto.Codigo, dto.Descripcion }
                : new { IdCondicionIVA = idValue, dto.Codigo, dto.Descripcion, dto.Activo },
            "api/tiposimpuesto" => esCreacion
                ? new { dto.Codigo, dto.Descripcion }
                : new { IdTipoImpuesto = idValue, dto.Codigo, dto.Descripcion, dto.Activo },
            "api/alicuotasimpuesto" => esCreacion
                ? new { dto.IdTipoImpuesto, dto.Codigo, dto.Descripcion, Porcentaje = dto.Porcentaje ?? 0m, FechaDesde = (DateTime?)null, FechaHasta = (DateTime?)null }
                : new { IdAlicuotaImpuesto = idValue, dto.IdTipoImpuesto, dto.Codigo, dto.Descripcion, Porcentaje = dto.Porcentaje ?? 0m, FechaDesde = (DateTime?)null, FechaHasta = (DateTime?)null, dto.Activo },
            "api/monedas" => esCreacion
                ? new { dto.Codigo, dto.Descripcion, Simbolo = (string?)null, CantidadDecimales = (byte)2 }
                : new { IdMoneda = idValue, dto.Codigo, dto.Descripcion, Simbolo = (string?)null, CantidadDecimales = (byte)2, dto.Activo },
            "api/mediospago" => esCreacion
                ? new { dto.Codigo, dto.Descripcion, EsEfectivo = false, RequiereReferencia = false }
                : new { IdMedioPago = idValue, dto.Codigo, dto.Descripcion, EsEfectivo = false, RequiereReferencia = false, dto.Activo },
            "api/tiposmovimientocaja" => esCreacion
                ? new { dto.Codigo, dto.Descripcion, Signo = dto.Signo }
                : new { IdTipoMovimientoCaja = idValue, dto.Codigo, dto.Descripcion, Signo = dto.Signo, dto.Activo },
            "api/estadoscaja" => esCreacion
                ? new { dto.Codigo, dto.Descripcion }
                : new { IdEstadoCaja = idValue, dto.Codigo, dto.Descripcion, dto.Activo },
            "api/depositos" => esCreacion
                ? new { dto.Codigo, dto.Descripcion, Ubicacion = (string?)null }
                : new { IdDeposito = idValue, dto.Codigo, dto.Descripcion, Ubicacion = (string?)null, dto.Activo },
            "api/tiposmovimientostock" => esCreacion
                ? new { dto.Codigo, dto.Descripcion, Signo = dto.Signo }
                : new { IdTipoMovimientoStock = idValue, dto.Codigo, dto.Descripcion, Signo = dto.Signo, dto.Activo },
            "api/categorias" => esCreacion
                ? new { dto.Codigo, dto.Descripcion }
                : new { IdCategoria = idValue, dto.Codigo, dto.Descripcion, dto.Activo },
            "api/subcategorias" => esCreacion
                ? new { dto.IdCategoria, dto.Codigo, dto.Descripcion }
                : new { IdSubCategoria = idValue, dto.IdCategoria, dto.Codigo, dto.Descripcion, dto.Activo },
            "api/listasprecios" => esCreacion
                ? new { dto.Codigo, dto.Descripcion, FechaDesde = (DateTime?)null, FechaHasta = (DateTime?)null }
                : new { IdListaPrecio = idValue, dto.Codigo, dto.Descripcion, FechaDesde = (DateTime?)null, FechaHasta = (DateTime?)null, dto.Activo },
            _ => dto
        };
    }
}
