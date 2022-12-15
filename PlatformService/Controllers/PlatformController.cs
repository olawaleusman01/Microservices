using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.AsynDataService;
using PlatformService.Data;
using PlatformService.Dtos;
using PlatformService.Http;
using PlatformService.Models;

namespace PlatformService.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlatformController : ControllerBase
    {
        private readonly IPlatformRepo _repo;
        private IMapper _mapper;
        private readonly ICommandDataClient _commandDataClient;
        private readonly IPlatformMessageBusClient _msgBusClient;

        public PlatformController(IPlatformRepo repo,
                                  IMapper mapper,
                                  ICommandDataClient commandDataClient,
                                  IPlatformMessageBusClient msgBusClient)
        {
            _repo = repo;
            _mapper = mapper;
            _commandDataClient = commandDataClient;
            _msgBusClient = msgBusClient;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _repo.GetPlatforms();

            return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(result));
        }

        [HttpGet("{id}", Name = "GetPlatformById")]
        public async Task<IActionResult> GetPlatformById(int id)
        {
            var result = await _repo.GetPlatformById(id);
            if (result != null)
            {
                return Ok(_mapper.Map<PlatformReadDto>(result));
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<ActionResult<PlatformReadDto>> Post(PlatformCreateDto model)
        {
            var platform = _mapper.Map<Platform>(model);
            _repo.CreatePlatform(platform);
            _repo.SaveChanges();

            var platformRead = _mapper.Map<PlatformReadDto>(platform);

            try
            {
                await _commandDataClient.SendPlatformToCommand(platformRead);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"-->Could not send synchronously--{ex.Message}");
            }

            try
            {
                var platformPublish = _mapper.Map<PlatformPublishDto>(platformRead);
                platformPublish.Event = "Platform_Published";
                _msgBusClient.PublishNewPlatform(platformPublish);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"-->Could not send asynchronously--{ex.Message}");
            }
            return CreatedAtRoute(nameof(GetPlatformById), new { Id = platform.Id }, platformRead);
        }
    }
}