using Application.Exceptions.Abstractions;

namespace Application.Exceptions;

public sealed class RoleNotFoundException() : NotFoundException($"Default role not found") { }

public sealed class UserNotFoundException() : BadRequestException($"user not found") { }

public sealed class EmailConfirmationFailedException() : BadRequestException($"Email confirmation failed") { }
