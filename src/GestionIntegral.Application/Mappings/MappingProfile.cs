using AutoMapper;
using GestionIntegral.Application.DTOs.TiposUsuario;
using GestionIntegral.Application.DTOs.Usuarios;
using GestionIntegral.Domain.Entities;

namespace GestionIntegral.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<TipoUsuario, TipoUsuarioResponseDto>();

        CreateMap<Usuario, UsuarioResponseDto>()
            .ForMember(dest => dest.TipoUsuarioDescripcion,
                       opt => opt.MapFrom(src => src.TipoUsuario.Descripcion));
    }
}
