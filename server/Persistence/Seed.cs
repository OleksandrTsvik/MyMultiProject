using Domain;
using Microsoft.AspNetCore.Identity;

namespace Persistence;

public class Seed
{
    public static async Task SeedData(DataContext context, UserManager<AppUser> userManager)
    {
        if (!userManager.Users.Any())
        {
            List<AppUser> users = new List<AppUser>
            {
                new AppUser
                {
                    RegistrationDate = DateTime.Now,
                    UserName = "san4ik",
                    Email = "oleksandr.zwick@gmail.com"
                },
                new AppUser
                {
                    RegistrationDate = DateTime.Now,
                    UserName = "bob",
                    Email = "bob@test.com"
                }
            };

            foreach (AppUser user in users)
            {
                await userManager.CreateAsync(user, "Pa$$w0rd");
            }
        }

        await context.SaveChangesAsync();
    }
}