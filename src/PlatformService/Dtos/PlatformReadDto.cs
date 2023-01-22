namespace PlatformService.Dtos;
public class PlatformReadDto
{
    public required int Id { get; set; }
    
    public required string Name { get; set; }
    
    public required string Publisher { get; set; }
    
    public required string Cost { get; set; }
}