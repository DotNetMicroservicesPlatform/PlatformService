using Microsoft.EntityFrameworkCore;
using PlatformService.Models;

namespace PlatformService.Data.Extensions;

public static class PrepDb
{
    public static IApplicationBuilder PrepPopulation(this IApplicationBuilder appBuilder, IWebHostEnvironment environment)
    {
        using (var serviceScpoe = appBuilder.ApplicationServices.CreateScope())
        {
            SeedData(serviceScpoe.ServiceProvider.GetService<AppDbContext>(), environment);
        }
        return appBuilder;
    }

    private static void SeedData(AppDbContext context, IWebHostEnvironment environment)
    {
        if(environment.IsProduction()){
             Console.WriteLine("--> Applying Migrations...");
            context.Database.Migrate();
        }
        if (context.Platforms.Any())
        {
            Console.WriteLine("--> We already have data");
            return;
        }
        Console.WriteLine("--> Seeding data...");
        context.Platforms.AddRange(
                new Platform() { Name = "DotNet", Publisher = "Microsoft", Cost = "Free" },
                new Platform() { Name = "Sql Server Express", Publisher = "Microsoft", Cost = "Free" },
                new Platform() { Name = "Kubernetes", Publisher = "Cloud Native Computing Foundation", Cost = "Free" }
        );
        context.SaveChanges();
    }
}