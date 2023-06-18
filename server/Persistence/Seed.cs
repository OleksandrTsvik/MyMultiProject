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

        if (context.Duties.Any())
        {
            return;
        }

        List<Duty> duties = new List<Duty>
        {
            new Duty
            {
                Position = 1,
                Title = "Test #1",
                Description = "Test task #1",
                IsCompleted = false,
                DateCreation = DateTime.Now,
                BackgroundColor = "#000",
                FontColor = "#fff"
            },
            new Duty
            {
                Position = 2,
                Title = "Test #2",
                Description = "Test task #2",
                IsCompleted = false,
                DateCreation = DateTime.Now
            }
        };

        await context.Duties.AddRangeAsync(duties);
        await context.SaveChangesAsync();
    }
}