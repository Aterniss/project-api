using Project_API.Models;

namespace Project_API.Repositories
{
    public interface IRestaurantRepository
    {
        Task<IEnumerable<Restaurant>> GetAll();
        Task<Restaurant> GetById(int id);
        Task<Restaurant> GetByName(string restaurantName);
        Task AddRestaurant(Restaurant newRestaurant);
    }
}
