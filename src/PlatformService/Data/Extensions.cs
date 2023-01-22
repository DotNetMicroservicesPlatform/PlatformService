namespace PlatformService.Data.Extensions;

public static class Extensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IPlatformRepository, PlaformRepository>();
        return services;
    }
}