using CommandService.Data;
using CommandService.Models;
using Microsoft.EntityFrameworkCore;

namespace PlatformService.Data
{
    public class CommandRepo : ICommandRepo
    {
        private readonly AppDbContext _context;

        public CommandRepo(AppDbContext context)
        {
            _context = context;
        }

        public void CreateCommand(int platformId, Command command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }
            command.PlatformId = platformId;
            _context.Command.Add(command);
        }

        public void CreatePlatform(Platform platform)
        {
            if (platform == null)
            {
                throw new ArgumentNullException(nameof(platform));
            }
            _context.Platform.Add(platform);
        }

        public async Task<IEnumerable<Platform>> GetAllPlatforms()
        {
            return await _context.Platform.ToListAsync();
        }

        public async Task<Command> GetCommandById(int platformId, int commandId)
        {
            return await _context.Command
                   .FirstOrDefaultAsync(d => d.PlatformId == platformId && d.Id == commandId);
        }

        public async Task<IEnumerable<Command>> GetCommandsForPlatform(int platformId)
        {
            return await _context.Command
                    .Where(d => d.PlatformId == platformId).ToListAsync();
        }


        public bool PlatfromExists(int platformId)
        {
            return _context.Platform.Any(d => d.Id == platformId);
        }

        public bool ExternalPlatfromExists(int externalPlatformId)
        {
            return _context.Platform.Any(d => d.ExternalId == externalPlatformId);
        }

        public bool SaveChanges()
        {
            return _context.SaveChanges() > 0;
        }
    }
}