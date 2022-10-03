using Project_API.Models;

namespace Project_API.Repositories
{
    public interface IZoneRepository
    {
        Task<IEnumerable<Zone>> GetAll();
        Task<Zone> GetById(int id);
        Task AddZone(Zone zone);
        Task UpdateZone(Zone zone, int id);
        Task DeleteZone(int id);

    }
}
