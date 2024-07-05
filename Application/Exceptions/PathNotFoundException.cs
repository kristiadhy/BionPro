namespace Application.Exceptions;
public sealed class PathNotFoundException() : NotFoundException($"Path not found in the server") { }