using System.Text;
using System.Text.Json;
using PlatformService.Dtos;

namespace PlatformService.SyncDataServices.Http;

public class HttpCommandDataClient : ICommandDataClient
{
    private HttpClient _httpClient;
    private ILogger<HttpCommandDataClient> _logger;
    private IConfiguration _configuration;

    public HttpCommandDataClient(HttpClient httpClient, ILogger<HttpCommandDataClient> logger, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _logger = logger;
        _configuration = configuration;
    }

    public async Task SendPlatformToCommand(PlatformReadDto platform)
    {
        var commadServiceUrl=_configuration["CommandServiceUrl"];

        var httpContent = new StringContent(
            JsonSerializer.Serialize(platform),
            Encoding.UTF8,
            "application/json"
        );

        var response = await _httpClient.PostAsync($"{commadServiceUrl}", httpContent);

        if (response.IsSuccessStatusCode)
        {
            _logger.LogInformation("--> Send POST to CommandService was OK!");
        }
        else
        {
            _logger.LogError("--> Send POST to CommandService was NOT OK!");
        }
    }
}
