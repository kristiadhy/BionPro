namespace Application.Exceptions.Abstractions;

public abstract class UnauthorizedException(string message) : Exception(message);