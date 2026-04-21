using System.Net;

namespace OrderTrackingApplication.Application.Exceptions;

public class NotFoundException(long id) : ApplicationException(HttpStatusCode.NotFound, $"Entity with id {id} was not found");
