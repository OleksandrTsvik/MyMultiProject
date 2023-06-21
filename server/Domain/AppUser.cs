using Microsoft.AspNetCore.Identity;

namespace Domain;

public class AppUser : IdentityUser
{
    public DateTime RegistrationDate { get; set; }
    public ICollection<Duty> Duties { get; set; }

    public AppUser()
    {
        Duties = new List<Duty>();
    }
}