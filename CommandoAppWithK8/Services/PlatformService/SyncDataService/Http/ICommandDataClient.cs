using PlatformService.Dtos;

namespace PlatformService.Http
{
    public interface ICommandDataClient
    {
        Task SendPlatformToCommand(PlatformReadDto plat);
    }
}