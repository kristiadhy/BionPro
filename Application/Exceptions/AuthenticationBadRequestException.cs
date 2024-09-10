namespace Application.Exceptions;

public sealed class RoleNotFoundException() : Exception($"Default role not found") { }
