using AutoMapper;
using PlatformService.Dtos;
using PlatformService.Models;

namespace PlatformService.Profiles
{
    public class PlatformProfiles : Profile
    {
        public PlatformProfiles()
        {
            //Source >> Target
            CreateMap<PlatformCreateDto, Platform>();
            CreateMap<Platform, PlatformReadDto>();
            CreateMap<PlatformReadDto, PlatformPublishDto>();
            CreateMap<Platform, GrpcPlatformModel>()
            .ForMember(dest => dest.PlatformId, opt => opt.MapFrom(src => src.Id));



        }

    }
}