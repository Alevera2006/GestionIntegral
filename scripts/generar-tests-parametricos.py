import os
import re
from pathlib import Path

SERVICES_DIR = Path("src/GestionIntegral.Application/Services")
OUTPUT_DIR = Path("src/GestionIntegral.Application.Tests/Services")

EXCLUDED = {
    "AuthService",
    "UsuarioService",
    "TipoUsuarioService",
    "CajaCabeceraService",
    "CajaDetalleService",
    "ClienteService",
    "ClienteContactoService",
    "ContactoService",
    "DomicilioService",
    "EmailService",
    "MovimientoStockService",
    "MovimientoStockDetalleService",
    "ProductoService",
    "ProveedorService",
    "StockService",
    "TelefonoService",
}


def pascal_to_kebab(name: str) -> str:
    return re.sub(r"(?<!^)(?=[A-Z])", "-", name).lower()


def discover_service_info(path: Path):
    text = path.read_text(encoding="utf-8")

    class_match = re.search(r"public class (\w+)Service : I(\w+)Service", text)
    if not class_match:
        return None
    service_name = class_match.group(1)
    if f"{service_name}Service" in EXCLUDED:
        return None

    dto_ns_match = re.search(r"using GestionIntegral\.Application\.DTOs\.([\w]+);", text)
    dto_ns = dto_ns_match.group(1) if dto_ns_match else service_name + "s"

    entity_match = re.search(r"new (\w+)\s*\{", text)
    entity_name = entity_match.group(1) if entity_match else service_name

    response_dto = f"{service_name}ResponseDto"
    crear_dto = f"Crear{service_name}RequestDto"
    actualizar_dto = f"Actualizar{service_name}RequestDto"

    uow_prop_match = re.search(r"_unitOfWork\.(\w+)\.(?:GetAllAsync|GetByIdAsync|ExisteCodigoAsync|GetByCodigoAsync|AddAsync|UpdateAsync)", text)
    uow_prop = uow_prop_match.group(1) if uow_prop_match else service_name + "s"

    id_prop_match = re.search(r"request\.(Id\w+)(?:\s*==|\s*,|\s*\})", text)
    id_prop = id_prop_match.group(1) if id_prop_match else f"Id{service_name}"

    has_existe = "ExisteCodigoAsync" in text
    has_get_by_codigo = "GetByCodigoAsync" in text
    has_actualizar = "ActualizarAsync" in text

    extra_crear_props = []
    crear_block_match = re.search(rf"new {entity_name}\s*\{{(.*?)\}};", text, re.DOTALL)
    if crear_block_match:
        block = crear_block_match.group(1)
        for m in re.finditer(r"(\w+) = request\.(\w+)", block):
            prop = m.group(2)
            if prop not in ("Codigo", "Descripcion", "Activo", "IdUsuarioCreacion", "FechaCreacion"):
                extra_crear_props.append(prop)

    return {
        "service_name": service_name,
        "dto_ns": dto_ns,
        "entity_name": entity_name,
        "response_dto": response_dto,
        "crear_dto": crear_dto,
        "actualizar_dto": actualizar_dto,
        "uow_prop": uow_prop,
        "id_prop": id_prop,
        "has_existe": has_existe,
        "has_get_by_codigo": has_get_by_codigo,
        "has_actualizar": has_actualizar,
        "extra_crear_props": extra_crear_props,
    }


def render_test(info):
    sn = info["service_name"]
    en = info["entity_name"]
    ns = info["dto_ns"]
    rd = info["response_dto"]
    cd = info["crear_dto"]
    ad = info["actualizar_dto"]
    up = info["uow_prop"]
    ip = info["id_prop"]

    extra_inicializacion = ""
    for prop in info["extra_crear_props"]:
        if prop in ("Dias", "CantidadDecimales"):
            extra_inicializacion += f"\n            request.{prop} = 30;"
        elif prop in ("Porcentaje"):
            extra_inicializacion += f"\n            request.{prop} = 21;"
        elif prop in ("EsEfectivo", "RequiereReferencia"):
            extra_inicializacion += f"\n            request.{prop} = true;"
        elif prop in ("IdTipoImpuesto", "IdCategoria"):
            extra_inicializacion += f"\n            request.{prop} = 1;"
        elif prop == "CodigoAFIP":
            extra_inicializacion += f"\n            request.{prop} = \"123\";"
        elif prop in ("FechaDesde", "FechaHasta"):
            extra_inicializacion += f"\n            request.{prop} = DateTime.UtcNow;"
        elif prop == "Simbolo":
            extra_inicializacion += f"\n            request.{prop} = \"$\";"
        else:
            extra_inicializacion += f"\n            request.{prop} = \"valor\";"

    content = f'''using AutoMapper;
using FluentAssertions;
using GestionIntegral.Application.DTOs;
using GestionIntegral.Application.DTOs.{ns};
using GestionIntegral.Application.Mappings;
using GestionIntegral.Application.Services;
using GestionIntegral.Domain.Common.Exceptions;
using GestionIntegral.Domain.Entities;
using GestionIntegral.Domain.Interfaces.Repositories;
using GestionIntegral.Domain.Interfaces.Services;
using Moq;

namespace GestionIntegral.Application.Tests.Services;

public class {sn}ServiceTests
{{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly IMapper _mapper;
    private readonly Mock<ICurrentUserService> _currentUserServiceMock;
    private readonly {sn}Service _service;

    public {sn}ServiceTests()
    {{
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapper = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>()).CreateMapper();
        _currentUserServiceMock = new Mock<ICurrentUserService>();
        _service = new {sn}Service(_unitOfWorkMock.Object, _mapper, _currentUserServiceMock.Object);
    }}

    [Fact]
    public async Task ObtenerTodosAsync_RetornaListaMapeada()
    {{
        var entidades = new List<{en}> {{ new() {{ Id{sn} = 1, Codigo = "COD1", Descripcion = "Desc 1" }} }};
        _unitOfWorkMock.Setup(u => u.{up}.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(entidades);

        var result = await _service.ObtenerTodosAsync();

        result.Exitoso.Should().BeTrue();
        result.Datos.Should().HaveCount(1);
    }}

    [Fact]
    public async Task ObtenerPorIdAsync_CuandoExiste_RetornaDto()
    {{
        var entidad = new {en} {{ Id{sn} = 1, Codigo = "COD1", Descripcion = "Desc 1" }};
        _unitOfWorkMock.Setup(u => u.{up}.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(entidad);

        var result = await _service.ObtenerPorIdAsync(1);

        result.Exitoso.Should().BeTrue();
        result.Datos!.Codigo.Should().Be("COD1");
    }}

    [Fact]
    public async Task ObtenerPorIdAsync_CuandoNoExiste_LanzaNotFoundException()
    {{
        _unitOfWorkMock.Setup(u => u.{up}.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(({en}?)null);

        Func<Task> act = async () => await _service.ObtenerPorIdAsync(99);

        await act.Should().ThrowAsync<NotFoundException>();
    }}
'''

    if info["has_existe"]:
        content += f'''
    [Fact]
    public async Task CrearAsync_CuandoNoExiste_CreaCorrectamente()
    {{
        var request = new {cd} {{ Codigo = "COD1", Descripcion = "Desc 1" }};{extra_inicializacion}
        _unitOfWorkMock.Setup(u => u.{up}.ExisteCodigoAsync(request.Codigo, It.IsAny<CancellationToken>())).ReturnsAsync(false);
        _unitOfWorkMock.Setup(u => u.{up}.AddAsync(It.IsAny<{en}>(), It.IsAny<CancellationToken>()))
            .Returns<{en}, CancellationToken>((e, _) => {{ e.Id{sn} = 10; return Task.FromResult(e); }});
        _unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
        _currentUserServiceMock.Setup(c => c.UserId).Returns(5);

        var result = await _service.CrearAsync(request);

        result.Exitoso.Should().BeTrue();
        result.Datos!.Codigo.Should().Be("COD1");
        result.Datos.Id{sn}.Should().Be(10);
    }}

    [Fact]
    public async Task CrearAsync_CuandoExisteCodigo_LanzaBusinessException()
    {{
        var request = new {cd} {{ Codigo = "COD1", Descripcion = "Desc 1" }};
        _unitOfWorkMock.Setup(u => u.{up}.ExisteCodigoAsync(request.Codigo, It.IsAny<CancellationToken>())).ReturnsAsync(true);

        Func<Task> act = async () => await _service.CrearAsync(request);

        await act.Should().ThrowAsync<BusinessException>();
    }}
'''
    else:
        content += f'''
    [Fact]
    public async Task CrearAsync_CuandoNoExiste_CreaCorrectamente()
    {{
        var request = new {cd} {{ Codigo = "COD1", Descripcion = "Desc 1" }};{extra_inicializacion}
        _unitOfWorkMock.Setup(u => u.{up}.AddAsync(It.IsAny<{en}>(), It.IsAny<CancellationToken>()))
            .Returns<{en}, CancellationToken>((e, _) => {{ e.Id{sn} = 10; return Task.FromResult(e); }});
        _unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
        _currentUserServiceMock.Setup(c => c.UserId).Returns(5);

        var result = await _service.CrearAsync(request);

        result.Exitoso.Should().BeTrue();
        result.Datos!.Codigo.Should().Be("COD1");
    }}
'''

    if info["has_actualizar"]:
        content += f'''
    [Fact]
    public async Task ActualizarAsync_CuandoExiste_ActualizaCorrectamente()
    {{
        var entidad = new {en} {{ {ip} = 1, Codigo = "COD1", Descripcion = "Desc 1", Activo = true }};
        var request = new {ad} {{ {ip} = 1, Codigo = "COD2", Descripcion = "Desc 2", Activo = true }};
        _unitOfWorkMock.Setup(u => u.{up}.GetByIdAsync(request.{ip}, It.IsAny<CancellationToken>())).ReturnsAsync(entidad);
'''
        if info["has_get_by_codigo"]:
            content += f'''        _unitOfWorkMock.Setup(u => u.{up}.GetByCodigoAsync(request.Codigo, It.IsAny<CancellationToken>())).ReturnsAsync(({en}?)null);
'''
        content += f'''        _unitOfWorkMock.Setup(u => u.{up}.UpdateAsync(It.IsAny<{en}>(), It.IsAny<CancellationToken>()))
            .Returns<{en}, CancellationToken>((e, _) => Task.FromResult(e));
        _unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
        _currentUserServiceMock.Setup(c => c.UserId).Returns(5);

        var result = await _service.ActualizarAsync(request);

        result.Exitoso.Should().BeTrue();
        result.Datos!.Codigo.Should().Be("COD2");
    }}

    [Fact]
    public async Task ActualizarAsync_CuandoNoExiste_LanzaNotFoundException()
    {{
        var request = new {ad} {{ {ip} = 99, Codigo = "COD1", Descripcion = "Desc 1", Activo = true }};
        _unitOfWorkMock.Setup(u => u.{up}.GetByIdAsync(request.{ip}, It.IsAny<CancellationToken>())).ReturnsAsync(({en}?)null);

        Func<Task> act = async () => await _service.ActualizarAsync(request);

        await act.Should().ThrowAsync<NotFoundException>();
    }}
'''

    content += f'''
    [Fact]
    public async Task EliminarAsync_CuandoExisteYActivo_DaDeBajaCorrectamente()
    {{
        var entidad = new {en} {{ Id{sn} = 1, Codigo = "COD1", Descripcion = "Desc 1", Activo = true }};
        _unitOfWorkMock.Setup(u => u.{up}.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(entidad);
        _unitOfWorkMock.Setup(u => u.{up}.UpdateAsync(It.IsAny<{en}>(), It.IsAny<CancellationToken>()))
            .Returns<{en}, CancellationToken>((e, _) => Task.FromResult(e));
        _unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
        _currentUserServiceMock.Setup(c => c.UserId).Returns(5);

        var result = await _service.EliminarAsync(1);

        result.Exitoso.Should().BeTrue();
        result.Datos.Should().BeTrue();
        entidad.Activo.Should().BeFalse();
    }}

    [Fact]
    public async Task EliminarAsync_CuandoNoExiste_LanzaNotFoundException()
    {{
        _unitOfWorkMock.Setup(u => u.{up}.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(({en}?)null);

        Func<Task> act = async () => await _service.EliminarAsync(99);

        await act.Should().ThrowAsync<NotFoundException>();
    }}
}}
'''
    return content


def main():
    OUTPUT_DIR.mkdir(parents=True, exist_ok=True)
    generated = []
    for path in sorted(SERVICES_DIR.glob("*.cs")):
        info = discover_service_info(path)
        if not info:
            continue
        content = render_test(info)
        out_path = OUTPUT_DIR / f"{info['service_name']}ServiceTests.cs"
        out_path.write_text(content, encoding="utf-8")
        generated.append(out_path.name)

    print(f"Generated {len(generated)} test files:")
    for g in generated:
        print(f"  - {g}")


if __name__ == "__main__":
    main()
