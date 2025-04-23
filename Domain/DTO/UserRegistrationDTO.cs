namespace Domain.DTO;
public record UserRegistrationDTO
{
  public required string UserName { get; init; }
  public required string Password { get; init; }
  public required string Email { get; init; }
  public string? ClientUri { get; set; }
  public ICollection<string>? Roles { get; init; }
}