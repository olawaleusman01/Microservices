using CommandService.Models;

namespace PlatformService.Data
{
    public interface ICommandRepo
    {
        bool SaveChanges();
        //Platforms
        Task<IEnumerable<Platform>> GetAllPlatforms();
        bool PlatfromExists(int platformId);
        bool ExternalPlatfromExists(int externalPlatformId);
        void CreatePlatform(Platform model);

        //Commands
        Task<IEnumerable<Command>> GetCommandsForPlatform(int platformId);
        Task<Command> GetCommandById(int platformId, int commandId);
        void CreateCommand(int platformId, Command command);
    }
}