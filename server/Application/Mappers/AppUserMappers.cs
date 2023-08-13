using Application.Profiles;
using Domain;

namespace Application.Mappers;

public static class AppUserMappers
{
    public static QueryUser ToQueryUser(this AppUser user)
    {
        return new QueryUser
        {
            Id = user.Id,
            UserName = user.UserName
        };
    }

    public static Profile ToProfile(this AppUser user, string currentUserName)
    {
        return new Profile
        {
            UserName = user.UserName,
            Email = user.Email,
            RegistrationDate = user.RegistrationDate,
            Image = user.Photos.FirstOrDefault(x => x.IsMain)?.Url,
            Following = user.Followers.Any(x => x.Observer.UserName == currentUserName),
            FollowersCount = user.Followers.Count,
            FollowingCount = user.Followings.Count,
            Photos = user.Photos.ToListPhotoDto()
        };
    }
}
