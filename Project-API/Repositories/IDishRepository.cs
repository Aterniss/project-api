using Project_API.Models;

namespace Project_API.Repositories
{
    public interface IDishRepository
    {
        public Task<IEnumerable<Dish>> GetAll();
        public Task<Dish> GetDishById(int id);
        public Task<IEnumerable<Dish>> GetDishesByName(string name);
        public Task UpdateDishById(Dish dish, int id);
        public Task DeleteDishById(int id);
        public Task AddNewDish(Dish dish);
    }
}
