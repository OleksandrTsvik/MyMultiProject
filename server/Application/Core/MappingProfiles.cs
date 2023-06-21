using Application.Duties;
using Application.Profiles;
using AutoMapper;
using Domain;

namespace Application.Core;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Duty, Duty>();
        CreateMap<Duty, DutyDto>();
        
        CreateMap<AppUser, QueryUser>();
    }
}