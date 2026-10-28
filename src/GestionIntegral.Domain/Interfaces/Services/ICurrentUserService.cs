namespace GestionIntegral.Domain.Interfaces.Services;

public interface ICurrentUserService
{
    int? UserId { get; }
    string? UserName { get; }
    bool IsAuthenticated { get; }
}
