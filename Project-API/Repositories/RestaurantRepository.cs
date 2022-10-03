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
            await CheckZoneAndCategoryName(newRestaurant.CategoryName, newRestaurant.ZoneId);
            _dbContext.Add(newRestaurant);
            await _dbContext.SaveChangesAsync();

        }

        public async Task DeleteRestaurantById(int id)
        {
            var result = await _dbContext.Restaurants.FirstOrDefaultAsync(x => x.RestaurantId == id);
            if(result != null)
            {
               _dbContext.Restaurants.Remove(result);
               await _dbContext.SaveChangesAsync();
            }
            else
            {
                throw new Exception($"Restaurant with ID: \"{id}\" does not exist!");
            }
            
        }

        public async Task DeleteRestaurantByName(string restaurantName)
        {
            var result = await _dbContext.Restaurants.FirstOrDefaultAsync(x => x.RestaurantName == restaurantName);
            if (result != null)
            {
                _dbContext.Restaurants.Remove(result);
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                throw new Exception($"Restaurant with name: \"{restaurantName}\" does not exist!");
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

        public async Task UpdateRestaurant(int id, Restaurant updateRestaurant)
        {
            var result = await _dbContext.Restaurants.FirstOrDefaultAsync(x => x.RestaurantId == id);
            if(result == null)
            {
                throw new Exception($"Restaurant with ID: \"{id}\" does not exist!");
            }
            else
            {
                await CheckZoneAndCategoryName(updateRestaurant.CategoryName, updateRestaurant.ZoneId);
                result.RestaurantName = updateRestaurant.RestaurantName;
                result.RestaurantAddress = updateRestaurant.RestaurantAddress;
                result.CategoryName = updateRestaurant.CategoryName;
                result.ZoneId = updateRestaurant.ZoneId;

                await _dbContext.SaveChangesAsync();
            }
        }

        //check method
        public async Task CheckZoneAndCategoryName(string categoryName, int zoneId)
        {
            var zone = await _dbContext.Zones.Select(x => x.ZoneId == zoneId).FirstOrDefaultAsync();
            var category = await _dbContext.FoodCategories.Select(x => x.CategoryName == categoryName).FirstOrDefaultAsync();
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

        }

        public Task<Restaurant> GetByName(string restaurantName)
        {
            throw new NotImplementedException();
        }
    }
}
