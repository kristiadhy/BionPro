namespace Application.Exceptions.Abstractions;

public abstract class BadRequestException(string message) : Exception(message);