using PlatformService.Dtos;

namespace PlatformService.AsynDataService
{
    public interface IMessageBusClient
    {
        void SendMessage(string message);
    }
}