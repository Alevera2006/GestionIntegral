namespace GestionIntegral.Domain.Common.Exceptions;

public class BusinessException : DomainException
{
    public BusinessException(string message) : base(message) { }
}
