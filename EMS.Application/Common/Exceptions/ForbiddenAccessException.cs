namespace EMS.Application.Common.Exceptions;

public class ForbiddenAccessException : CustomException
{
    public ForbiddenAccessException(string message) : base(message, null, System.Net.HttpStatusCode.Forbidden) { }
}
