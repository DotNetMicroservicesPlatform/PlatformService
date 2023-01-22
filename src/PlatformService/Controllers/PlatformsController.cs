using AutoMapper;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using PlatformContracts.Dtos;
using PlatformService.Data;
using PlatformService.Dtos;
using PlatformService.Models;
using PlatformService.SyncDataServices.Http;

namespace PlatformService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PlatformsController : ControllerBase
{
    private readonly IPlatformRepository _platformRepo;
    private readonly IMapper _mapper;
    private readonly ILogger<PlatformsController> _logger;
    private readonly ICommandDataClient _commadDataClient;

    private readonly IPublishEndpoint _publishEndpoint;

    public PlatformsController(IPlatformRepository platformRepo, IMapper mapper, ILogger<PlatformsController> logger, ICommandDataClient commadDataClient, IPublishEndpoint publishEndpoint)
    {
        _platformRepo = platformRepo;
        _mapper = mapper;
        _logger = logger;
        _commadDataClient = commadDataClient;
        _publishEndpoint = publishEndpoint;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PlatformReadDto>>> GetAsync()
    {
        _logger.LogInformation("--> Getting Platforms...");

        var platformModels = await _platformRepo.GetAllAsync();

        var platformDtos = _mapper.Map<IEnumerable<PlatformReadDto>>(platformModels);

        _logger.LogInformation("Platforms returned");

        return Ok(platformDtos);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PlatformReadDto>> GetByIdAsync(int id)
    {
        _logger.LogInformation("--> Getting Platform...");

        var platformModel = await _platformRepo.GetAsync(id);
        if (platformModel == null)
        {
            _logger.LogWarning("Requested platform not found!");
            return NotFound();
        }

        var platformDto = _mapper.Map<PlatformReadDto>(platformModel);

        _logger.LogInformation("Platform returned");

        return Ok(platformDto);
    }

    [HttpPost]
    public async Task<ActionResult<PlatformReadDto>> PostAsync(PlatformCreateDto platformCreateDto)
    {
        _logger.LogInformation("--> Adding Platform...");

        var platformModel = _mapper.Map<Platform>(platformCreateDto);

        await _platformRepo.CreateAsync(platformModel);

        await _platformRepo.SaveChangesAsync();

        _logger.LogInformation("Platform created");

        var platformDto = _mapper.Map<PlatformReadDto>(platformModel);

        /* try
        {
             _logger.LogInformation("--> Sending platform info to CommandService...");             
            await _commadDataClient.SendPlatformToCommand(platformDto);
            _logger.LogInformation("--> Message sent to CommandService done successfully!");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "--> Could not send message to CommandService!");
        }
        */
        var platformCreatedContract=_mapper.Map<PlatformCreated>(platformDto);

        _logger.LogInformation("Published created Platform data");

        await _publishEndpoint.Publish(platformCreatedContract);

        return CreatedAtAction(nameof(GetByIdAsync), new { id = platformDto.Id }, platformDto);
    }

}