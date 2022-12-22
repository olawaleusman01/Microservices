using System.Text.Json;
using AutoMapper;
using CommandService.Dtos;
using CommandService.Models;
using PlatformService.Data;
using PlatformService.Dtos;

namespace CommandService.EventProcessing
{
    public class EventProcessor : IEventProcessor
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IMapper _mapper;

        public EventProcessor(IServiceScopeFactory scopeFactory,
        IMapper mapper)
        {
            _scopeFactory = scopeFactory;
            _mapper = mapper;
        }
        public void ProcessEvent(string message)
        {
            var eventType = DetermineEvent(message);

            switch (eventType)
            {
                case EventType.PlatformPublished:
                    AddPlatform(message);
                    break;
                default:
                    Console.WriteLine("--> Could not detect platform publish event");
                    break;
            }
        }

        private EventType DetermineEvent(string notificationMessage)
        {
            Console.WriteLine("--> Determining Event Type");
            var eventType = JsonSerializer.Deserialize<GenericEventDto>(notificationMessage);

            switch (eventType.Event)
            {
                case "Platform_Published":
                    Console.WriteLine("--> Platform Published event detected");
                    return EventType.PlatformPublished;
                default:
                    Console.WriteLine("--> Could not detect platform publish event");
                    return EventType.Undetermined;
            }

        }

        private void AddPlatform(string platformPublishedMsg)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var repo = scope.ServiceProvider.GetRequiredService<ICommandRepo>();
                var platformPublishedDto = JsonSerializer.Deserialize<PlatformPublishDto>(platformPublishedMsg);

                try
                {
                    var plat = _mapper.Map<Platform>(platformPublishedDto);
                    Console.WriteLine($"--> Platform added...{plat.Id}-{plat.ExternalId}");
                    // plat.Id = 0;
                    //Console.WriteLine($"--> Platform added...{plat.Id}-{plat.ExternalId}");

                    if (!repo.ExternalPlatfromExists(plat.ExternalId))
                    {
                        repo.CreatePlatform(plat);
                        repo.SaveChanges();
                        Console.WriteLine($"--> Platform added...");

                    }
                    else
                    {
                        Console.WriteLine($"--> Platform already exists...");

                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"--> Could not add platform to db! {ex.Message}");

                }
            }
        }
    }

    enum EventType
    {
        PlatformPublished,
        Undetermined
    }
}