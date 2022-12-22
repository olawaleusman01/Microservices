using CommandService.AsynDataService;
using CommandService.Data;
using CommandService.EventProcessing;
using CommandService.SyncDataService.Grpc;
using Microsoft.EntityFrameworkCore;
using PlatformService.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

if (builder.Environment.IsDevelopment())
{
    Console.WriteLine($"--> Using InMem Db");
    builder.Services.AddDbContext<AppDbContext>(opt =>
                                                opt.UseInMemoryDatabase("CommandDb"));
}
else
{
    Console.WriteLine($"--> Using SQLServer Db");
    builder.Services.AddDbContext<AppDbContext>(opt =>
                                                    opt.UseSqlServer(builder.Configuration.GetConnectionString("CommandCon")));
}


builder.Services.AddControllers();
builder.Services.AddHostedService<MessageBusSubscriber>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddSingleton<IEventProcessor, EventProcessor>();
builder.Services.AddSingleton<IPlatformDataClient, PlatformDataClient>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ICommandRepo, CommandRepo>();

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

PrepDb.PrepPopulation(app, app.Environment.IsProduction());
app.Run();
