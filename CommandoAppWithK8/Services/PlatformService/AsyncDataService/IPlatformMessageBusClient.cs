using PlatformService.Dtos;

namespace PlatformService.AsynDataService
{
    public interface IPlatformMessageBusClient
    {
        void PublishNewPlatform(PlatformPublishDto platformPublishDto);
    }
}