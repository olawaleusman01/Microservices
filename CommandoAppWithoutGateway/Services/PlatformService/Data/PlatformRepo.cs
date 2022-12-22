using Microsoft.EntityFrameworkCore;
using PlatformService.Models;

namespace PlatformService.Data
{
    public class PlatformRepo : IPlatformRepo
    {
        private readonly AppDbContext _context;

        public PlatformRepo(AppDbContext context)
        {
            _context = context;
        }
        public void CreatePlatform(Platform model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            _context.Platform.Add(model);
        }

        public async Task<Platform> GetPlatformById(int id)
        {
            return await _context.Platform.FindAsync(id);
        }

        public async Task<IEnumerable<Platform>> GetPlatforms()
        {
            return await _context.Platform.ToListAsync();
        }

        public bool SaveChanges()
        {
            return _context.SaveChanges() > 0;
        }
    }
}