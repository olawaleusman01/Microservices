using AutoMapper;
using CommandService.Dtos;
using CommandService.Models;
using Microsoft.AspNetCore.Mvc;
using PlatformService.Data;

namespace CommandService.Controllers
{
    [Route("api/c/platform/{platformId}/[Controller]")]
    [ApiController]
    public class CommandController : ControllerBase
    {
        private readonly ICommandRepo _repository;
        private readonly IMapper _mapper;

        public CommandController(ICommandRepo repository,
        IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult> GetCommandsForPlatform(int platformId)
        {
            Console.WriteLine("--> Getting GetCommands For Platform from CommandService");
            var platforms = await _repository.GetCommandsForPlatform(platformId);
            return Ok(_mapper.Map<IEnumerable<CommandReadDto>>(platforms));
        }

        [HttpGet("{commandId}", Name = "GetCommandForPlatform")]
        public async Task<ActionResult> GetCommandForPlatform(int platformId, int commandId)
        {
            Console.WriteLine($"--> Getting GetCommand For Platform from CommandService {platformId} / {commandId}");

            if (!_repository.PlatfromExists(platformId))
            {
                return NotFound();
            }
            var platforms = await _repository.GetCommandById(platformId, commandId);

            if (platforms == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<CommandReadDto>(platforms));
        }


        [HttpPost]
        public ActionResult CreateCommandsForPlatform(int platformId, CommandCreateDto model)
        {
            Console.WriteLine($"--> Hit Create Commad For Platform from CommandService {platformId}");

            if (!_repository.PlatfromExists(platformId))
            {
                return NotFound();
            }
            var commandObj = _mapper.Map<Command>(model);
            _repository.CreateCommand(platformId, commandObj);
            _repository.SaveChanges();

            var commandReadDto = _mapper.Map<CommandReadDto>(commandObj);
            return CreatedAtRoute(nameof(GetCommandForPlatform), new { platformId = platformId, commandId = commandObj.Id }, commandReadDto);
        }


    }
}