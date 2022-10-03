using Microsoft.EntityFrameworkCore;
using Project_API.Models;

namespace Project_API.Repositories
{
    public class DishRepository : IDishRepository
    {
        private readonly MyDbContext _context;
        public DishRepository(MyDbContext context)
        {
            this._context = context;
        }

        public async Task AddNewDish(Dish dish)
        {
            var checkRestaurantId = await _context.Restaurants.FirstOrDefaultAsync(x => x.RestaurantId == dish.RestaurantId);
            if (checkRestaurantId == null)
            {
                throw new Exception($"The given restaurant ID: \"{dish.RestaurantId}\" does not exist!");
            }
            await _context.Dishes.AddAsync(dish);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteDishById(int id)
        {
            var existingDish = await _context.Dishes.FirstOrDefaultAsync(x => x.DishId == id);
            if(existingDish == null)
            {
                throw new Exception($"The dish with ID: \"{id}\" does not exist!");
            }
            else
            {
                _context.Remove(existingDish);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Dish>> GetAll()
        {
            return await _context.Dishes.IgnoreAutoIncludes().ToListAsync();
        }

        public async Task<Dish> GetDishById(int id)
        {
            var result = await _context.Dishes.FirstOrDefaultAsync(x => x.DishId == id);
            if(result == null)
            {
                return null;
            }
            return result;
        }

        public async Task<IEnumerable<Dish>> GetDishesByName(string name)
        {
            var result = await _context.Dishes.Where(x => x.DishName == name).ToListAsync();

            if(result.Count == 0)
            {
                return null;
            }
            return result;
        }

        public async Task UpdateDishById(Dish dish, int id)
        {
            var existingDish = await _context.Dishes.FirstOrDefaultAsync(x => x.DishId == id);

            if (existingDish == null)
            {
                throw new Exception($"The dish with ID: \"{id}\" does not exist!");
            }
            else if (existingDish != null)
            {
                var checkRestaurantId = await _context.Restaurants.FirstOrDefaultAsync(x => x.RestaurantId == dish.RestaurantId);
                if(checkRestaurantId == null)
                {
                    throw new Exception($"The given restaurant ID: \"{dish.RestaurantId}\" does not exist!");
                }
                existingDish.DishName = dish.DishName;
                existingDish.DishDescription = dish.DishDescription;
                existingDish.Price = dish.Price;
                existingDish.RestaurantId = dish.RestaurantId;
                existingDish.Require18 = dish.Require18;

                await _context.SaveChangesAsync();

            }

        }

    }
}
