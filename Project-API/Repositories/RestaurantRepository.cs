using Microsoft.EntityFrameworkCore;
using Project_API.Models;

namespace Project_API.Repositories
{
    public class RestaurantRepository : IRestaurantRepository
    {
        private readonly MyDbContext _dbContext;
        public RestaurantRepository(MyDbContext context)
        {
            this._dbContext = context;
        }

        public async Task AddRestaurant(Restaurant newRestaurant)
        {
            var zone = await _dbContext.Zones.Select(x => x.ZoneId == newRestaurant.ZoneId).FirstOrDefaultAsync();
            var category = await _dbContext.FoodCategories.Select(x => x.CategoryName == newRestaurant.CategoryName).FirstOrDefaultAsync();
            if (zone == true && category == false)
            {
                throw new Exception($"The given \"category\" does not exist.");
            } 
            else if (zone == false && category == true)
            {
                throw new Exception($"The given \"zone id\" does not exist.");
            } 
            else if (category == false && zone == false)
            {
                throw new Exception($"The given \"zone id\" and \"category\" does not exist.");
            }
            else
            {
                _dbContext.Add(newRestaurant);
                await _dbContext.SaveChangesAsync();
            }
 
        }

        public async Task<IEnumerable<Restaurant>> GetAll()
        {
            return await _dbContext.Restaurants.IgnoreAutoIncludes().ToListAsync();
        }

        public async Task<Restaurant> GetById(int id)
        {
            var result = await _dbContext.Restaurants.FirstOrDefaultAsync(x => x.RestaurantId == id);
            if(result == null)
            {
                return null;
            }
            return result;
        }

        public async Task<Restaurant> GetByName(string restaurantName)
        {
            var result = await _dbContext.Restaurants.FirstOrDefaultAsync(x => x.RestaurantName == restaurantName);
            if (result == null)
            {
                return null;
            }
            return result;
        }
    }
}
