namespace GestionIntegral.Domain.Common.Exceptions;

public class NotFoundException : DomainException
{
    public NotFoundException(string entityName, object key)
        : base($"Entidad '{entityName}' con clave '{key}' no encontrada.") { }
}
