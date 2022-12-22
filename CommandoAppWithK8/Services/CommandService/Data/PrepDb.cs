using CommandService.Models;
using CommandService.SyncDataService.Grpc;
using Microsoft.EntityFrameworkCore;
using PlatformService.Data;

namespace CommandService.Data
{
    public static class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder app, bool isProd)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var grpcClient = serviceScope.ServiceProvider.GetService<IPlatformDataClient>();
                var platforms = grpcClient.ReturnAllPlatforms();
                var commandRepo = serviceScope.ServiceProvider.GetService<ICommandRepo>();
                SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>(), commandRepo, platforms, isProd);
            }
        }

        public static void SeedData(AppDbContext context, ICommandRepo commandRepo, IEnumerable<Platform> platforms, bool isProd)
        {
            if (isProd)
            {
                Console.WriteLine("--> Attempting to apply migrations---");
                try
                {
                    context.Database.Migrate();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"--> Could not run migrations---{ex.Message}");
                }
            }
            Console.WriteLine("-->Seeding New Platforms<---");
            foreach (var platform in platforms)
            {
                if (!commandRepo.ExternalPlatfromExists(platform.ExternalId))
                {
                    commandRepo.CreatePlatform(platform);
                }
                commandRepo.SaveChanges();
            }
        }
    }
}