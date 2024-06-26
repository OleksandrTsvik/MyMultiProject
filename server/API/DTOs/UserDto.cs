namespace API.DTOs;

public class UserDto
{
    public string UserName { get; set; }
    public string Email { get; set; }
    public DateTime RegistrationDate { get; set; }

    public string Image { get; set; }
    public string Token { get; set; }


    public int CountNotCompletedDuties { get; set; }
    public int CountCompletedDuties { get; set; }
}