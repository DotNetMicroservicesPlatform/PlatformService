using AutoMapper;
using PlatformContracts.Dtos;
using PlatformService.Dtos;
using PlatformService.Models;

namespace PlatformService.MappingProfiles;

public class PlatformProfile : Profile
{
    public PlatformProfile()
    {
        // Source -> Target
        CreateMap<Platform, PlatformReadDto>();        
        CreateMap<PlatformCreateDto, Platform>();
        CreateMap<PlatformReadDto, PlatformCreated>();
        CreateMap<Platform, GrpcPlatformModel>()
                .ForMember(dest => dest.PlatformId, opt=> opt.MapFrom(src=>src.Id));
    }
}