using Microsoft.AspNetCore.Identity;

namespace Domain;

public class AppUser : IdentityUser
{
    public DateTime RegistrationDate { get; set; }

    public ICollection<Duty> Duties { get; set; }
    public ICollection<Photo> Photos { get; set; }
    public ICollection<UserFollowing> Followings { get; set; }
    public ICollection<UserFollowing> Followers { get; set; }

    public AppUser()
    {
        Duties = new List<Duty>();
        Photos = new List<Photo>();
        Followings = new List<UserFollowing>();
        Followers = new List<UserFollowing>();
    }
}