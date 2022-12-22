using AutoMapper;
using CommandService.Dtos;
using CommandService.Models;
using PlatformService;

namespace CommandService.Profiles
{
    public class CommandProfiles : Profile
    {
        public CommandProfiles()
        {
            //Source >> Target
            CreateMap<Platform, PlatformReadDto>();
            CreateMap<CommandCreateDto, Command>();
            CreateMap<Command, CommandReadDto>();
            CreateMap<PlatformPublishDto, Platform>()
            .ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Id, act => act.Ignore());
            CreateMap<GrpcPlatformModel, Platform>()
            .ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.PlatformId));




        }

    }
}