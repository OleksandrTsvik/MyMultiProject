using Application.Duties;
using Application.Profiles;
using Domain;

namespace Application.Core;

public class MappingProfiles : AutoMapper.Profile
{
    public MappingProfiles()
    {
        CreateMap<Duty, Duty>();
        CreateMap<Duty, DutyDto>();

        CreateMap<AppUser, QueryUser>();

        CreateMap<AppUser, Profile>()
            .ForMember(profile => profile.Image,
                option => option.MapFrom(user => user.Photos.FirstOrDefault(x => x.IsMain).Url));
    }
}