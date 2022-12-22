using AutoMapper;
using Grpc.Core;
using PlatformService.Data;

namespace PlatformService.Grpc
{
    public class GrpcPlatformService : GrpcPlatform.GrpcPlatformBase
    {
        private readonly IPlatformRepo _platformRepo;
        private IMapper _mapper;

        public GrpcPlatformService(IPlatformRepo platformRepo,
        IMapper mapper)
        {
            _platformRepo = platformRepo;
            _mapper = mapper;

        }

        public override Task<PlatformResponse> GetAllPlatforms(GetAllRequest request, ServerCallContext context)
        {
            var response = new PlatformResponse();
            var platforms = _platformRepo.GetPlatforms().Result;

            foreach (var plat in platforms)
            {
                response.Platform.Add(_mapper.Map<GrpcPlatformModel>(plat));
            }

            return Task.FromResult(response);
        }
    }
}