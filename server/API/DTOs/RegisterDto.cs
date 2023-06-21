using System.ComponentModel.DataAnnotations;

namespace API.DTOs;

public class RegisterDto
{
    [Required]
    [RegularExpression("^\\S{2,20}$", ErrorMessage = "Username must contain between 2 and 20 characters and no spaces")]
    public string UserName { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [RegularExpression("^(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])\\S{6,}$", ErrorMessage = "Password must be complex")]
    public string Password { get; set; }
}