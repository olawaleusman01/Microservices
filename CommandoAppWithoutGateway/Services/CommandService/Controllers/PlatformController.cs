using AutoMapper;
using CommandService.Dtos;
using Microsoft.AspNetCore.Mvc;
using PlatformService.Data;

namespace CommandService.Controllers
{
    [Route("api/c/[Controller]")]
    [ApiController]
    public class PlatformController : ControllerBase
    {
        private readonly ICommandRepo _repository;
        private readonly IMapper _mapper;

        public PlatformController(ICommandRepo repository,
        IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult> GetPlatforms()
        {
            Console.WriteLine("--> Getting Platforms from CommandService");
            var platforms = await _repository.GetAllPlatforms();
            return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(platforms));
        }

        [HttpPost]
        public ActionResult TestInboundConnection()
        {
            Console.WriteLine("--> Inbound Post # Command Service");

            return Ok("Inbound Test ok from Command Service");
        }
    }
}