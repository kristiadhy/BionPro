using System.ComponentModel.DataAnnotations;

namespace Domain.DTO;
public class UserAuthenticationDTO
{
  //public UserAuthenticationDTO(string userName, string password)
  //{
  //    UserName = userName;
  //    Password = password;
  //}

  [Required(ErrorMessage = "User name is required")]
  public string? UserName { get; set; }
  [Required(ErrorMessage = "Password name is required")]
  public string? Password { get; set; }
}
