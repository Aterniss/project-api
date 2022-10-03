using Project_API.Models;

namespace Project_API.Repositories
{
    public interface IRestaurantRepository
    {
        Task<IEnumerable<Restaurant>> GetAll();
        Task<Restaurant> GetById(int id);
        Task AddRestaurant(Restaurant newRestaurant);
        Task DeleteRestaurantById(int id);
        Task DeleteRestaurantByName(string restaurantName);
        Task UpdateRestaurant(int id, Restaurant updateRestaurant);
    }
}
