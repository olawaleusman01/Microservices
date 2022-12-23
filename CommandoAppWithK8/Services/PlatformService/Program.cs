using Microsoft.EntityFrameworkCore;
using PlatformService.AsynDataService;
using PlatformService.Data;
using PlatformService.Grpc;
using PlatformService.Http;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
if (builder.Environment.IsDevelopment())
{
    Console.WriteLine($"--> Using InMem Db");
    builder.Services.AddDbContext<AppDbContext>(opt =>
                                                opt.UseInMemoryDatabase("PlatformDb"));
}
else
{
    Console.WriteLine($"--> Using SQLServer Db");
    var connString = builder.Configuration["ConnectionStrings:PlatformCon"];


    //Console.WriteLine($"-->ConnectionString={builder.Configuration["ConnectionStrings:PlatformCon"]}");

    builder.Services.AddDbContext<AppDbContext>(opt =>
           opt.UseSqlServer(connString));
}

builder.Services.AddScoped<IPlatformRepo, PlatformRepo>();
builder.Services.AddHttpClient<ICommandDataClient, HttpCommandDataClient>();
builder.Services.AddScoped<IMessageBusClient, RabbitMessageBusClient>();
builder.Services.AddScoped<IPlatformMessageBusClient, PlatformMessageBusClient>();


builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddGrpc();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build()
                 .SetupMiddleware();

// // Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI();
// }

// app.UseHttpsRedirection();

// app.UseAuthorization();

// app.MapControllers();
Console.WriteLine($"--> CommandService Endpoint {builder.Configuration["CommandServiceBaseUrl"]}");
// Console.WriteLine($"--> Connection string {builder.Configuration.GetConnectionString("PlatformCon")}");

app.Run();


public static class RegisterStartupMiddlewares
{
    public static WebApplication SetupMiddleware(this WebApplication app)
    {
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
        app.MapGet("protos/platforms.proto", async context =>
        {
            await context.Response.WriteAsync(File.ReadAllText("Protos/platforms.proto"));
        });


        PrepDb.PrepPopulation(app, app.Environment.IsProduction());

        return app;
    }
}