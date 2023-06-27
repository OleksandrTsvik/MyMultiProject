using Domain;

namespace Application.Profiles;

public class Profile
{
    public string UserName { get; set; }
    public string Email { get; set; }
    public DateTime RegistrationDate { get; set; }
    public string Image { get; set; }

    public ICollection<Photo> Photos { get; set; }

    public Profile()
    {
        Photos = new List<Photo>();
    }
}