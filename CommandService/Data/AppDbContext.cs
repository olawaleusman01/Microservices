using Microsoft.EntityFrameworkCore;
using CommandService.Models;

namespace CommandService.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Platform> Platform { get; set; }
        public DbSet<Command> Command { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
            .Entity<Platform>()
            .HasMany(p => p.Commands)
            .WithOne(d => d.Platform!)
            .HasForeignKey(p => p.PlatformId)
            ;

            //     modelBuilder
            //    .Entity<Platform>()
            //    .Property(e => e.Id).ValueGeneratedOnAdd();

            modelBuilder
            .Entity<Command>()
            .HasOne(p => p.Platform)
            .WithMany(d => d.Commands)
            .HasForeignKey(p => p.PlatformId);
        }

    }
}