using System.Text.Json;
using PlatformService.Dtos;

namespace PlatformService.AsynDataService
{
    public class PlatformMessageBusClient : IPlatformMessageBusClient
    {
        private readonly IMessageBusClient _msgBusClient;

        public PlatformMessageBusClient(IMessageBusClient msgBusClient)
        {
            _msgBusClient = msgBusClient;
        }
        public void PublishNewPlatform(PlatformPublishDto platformPublishDto)
        {
            var message = JsonSerializer.Serialize(platformPublishDto);
            _msgBusClient.SendMessage(message);
        }
    }
}