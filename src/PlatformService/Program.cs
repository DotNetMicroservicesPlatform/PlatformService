
using Microsoft.AspNetCore.Builder;
using PlatformCommon.Data;
using PlatformCommon.MassTransit;
using PlatformService.Data;
using PlatformService.Data.Extensions;
using PlatformService.SyncDataServices.Grpc;
using PlatformService.SyncDataServices.Http;

namespace PlatformService;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddAppDbContext<AppDbContext>(builder.Configuration)
                        .AddRepositories();        

        builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        builder.Services.AddHttpClient<ICommandDataClient, HttpCommandDataClient>();

        builder.Services.AddMassTransitWithRabbitMq();

        builder.Services.AddGrpc();

        builder.Services.AddControllers(opt=>{
             opt.SuppressAsyncSuffixInActionNames = false;
        });
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();

        app.MapGrpcService<GrpcPlatformService>();

        app.MapGet("/protos/platforms.proto", async context => {
            await context.Response.WriteAsync(File.ReadAllText("Protos/platforms.proto"));
        });
       
        app.PrepPopulation(builder.Environment);

        app.Run();
    }
}
