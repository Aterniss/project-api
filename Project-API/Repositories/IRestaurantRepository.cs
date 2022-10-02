using Project_API.Models;

namespace Project_API.Repositories
{
    public interface IRestaurantRepository
    {
        Task<IEnumerable<Restaurant>> GetAll();
    }
}
