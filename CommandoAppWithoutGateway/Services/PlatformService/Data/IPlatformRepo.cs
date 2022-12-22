using PlatformService.Models;

namespace PlatformService.Data
{
    public interface IPlatformRepo
    {
        bool SaveChanges();
        Task<IEnumerable<Platform>> GetPlatforms();
        Task<Platform> GetPlatformById(int Id);
        void CreatePlatform(Platform model);
    }
}