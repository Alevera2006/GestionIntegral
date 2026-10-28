using GestionIntegral.Domain.Entities;

namespace GestionIntegral.Domain.Interfaces.Services;

public interface ITokenService
{
    string GenerarToken(Usuario usuario);
}
