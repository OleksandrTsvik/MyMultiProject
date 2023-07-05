using Application.Duties;
using Application.Photos;
using Application.Profiles;
using Domain;

namespace Application.Core;

public class MappingProfiles : AutoMapper.Profile
{
    public MappingProfiles()
    {
        string currentUserName = null;

        CreateMap<Duty, Duty>();
        CreateMap<Duty, DutyDto>();

        CreateMap<Photo, PhotoDto>();

        CreateMap<AppUser, QueryUser>();

        CreateMap<AppUser, Profile>()
            .ForMember(profile => profile.Image,
                option => option.MapFrom(user => user.Photos.FirstOrDefault(x => x.IsMain).Url))
            .ForMember(profile => profile.FollowersCount,
                option => option.MapFrom(user => user.Followers.Count))
            .ForMember(profile => profile.FollowingCount,
                option => option.MapFrom(user => user.Followings.Count))
            .ForMember(profile => profile.Following,
                option => option.MapFrom(user => user.Followers.Any(x => x.Observer.UserName == currentUserName)));
    }
}