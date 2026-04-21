using System.Net;

namespace OrderTrackingApplication.Application.Exceptions;

public abstract class ApplicationException(HttpStatusCode statusCode, string message) : Exception(message)
{
    public HttpStatusCode ApplicationErrorCode { get; } = statusCode;
}
