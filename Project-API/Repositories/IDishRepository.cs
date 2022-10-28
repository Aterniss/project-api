using Project_API.Models;

namespace Project_API.Repositories
{
    public interface IDishRepository
    {
        Task<IEnumerable<Dish>> GetAll();
        Task<IEnumerable<Dish>> GetAllForRestaurant(int restaurantId);
        Task<Dish> GetDishById(int id);
        Task<IEnumerable<Dish>> GetDishesByName(string name);
        Task UpdateDishById(Dish dish, int id);
        Task DeleteDishById(int id);
        Task AddNewDish(Dish dish);
    }
}
