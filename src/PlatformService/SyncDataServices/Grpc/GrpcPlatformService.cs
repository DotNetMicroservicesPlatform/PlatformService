using AutoMapper;
using Grpc.Core;
using PlatformService.Data;

namespace PlatformService.SyncDataServices.Grpc;

public class GrpcPlatformService : GrpcPlatform.GrpcPlatformBase
{
    private readonly IPlatformRepository _platformRepo;
    private readonly IMapper _mapper;

    public GrpcPlatformService(IPlatformRepository platformRepo, IMapper mapper)
    {
        _platformRepo = platformRepo;
        _mapper = mapper;
    }

    public override async Task<PlatformsResponse> GetAllPlatforms(GetAllRequest request, ServerCallContext context){
        var response = new PlatformsResponse();

        var platformEntities=await _platformRepo.GetAllAsync();

        var grpcModels= _mapper.Map<IEnumerable<GrpcPlatformModel>>(platformEntities);
        
        response.Platforms.AddRange(grpcModels);

        return response;
    }
}