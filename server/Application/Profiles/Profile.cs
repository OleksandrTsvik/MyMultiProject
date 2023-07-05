using Application.Photos;
using Domain;

namespace Application.Profiles;

public class Profile
{
    public string UserName { get; set; }
    public string Email { get; set; }
    public DateTime RegistrationDate { get; set; }
    public string Image { get; set; }

    public bool Following { get; set; }
    public int FollowersCount { get; set; }
    public int FollowingCount { get; set; }

    public ICollection<PhotoDto> Photos { get; set; }

    public Profile()
    {
        Photos = new List<PhotoDto>();
    }
}