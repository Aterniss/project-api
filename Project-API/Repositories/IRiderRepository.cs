using Project_API.Models;

namespace Project_API.Repositories
{
    public interface IRiderRepository
    {
        Task<IEnumerable<Rider>> GetAll();
        Task<IEnumerable<Rider>> GetAllId(int restaurantId);
        Task<Rider> GetById(int id);
        Task AddRider(Rider rider);
        Task DeleteRider(int id);
        Task UpdateRider(Rider rider, int id);
    }
}
