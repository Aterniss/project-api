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

            var checkZoneId = await _dbContext.Zones.FirstOrDefaultAsync(x => x.ZoneId == newRestaurant.ZoneId);
            var checkCategory = await _dbContext.FoodCategories.FirstOrDefaultAsync(x => x.CategoryName == newRestaurant.CategoryName);
            if (checkZoneId == null || checkCategory == null)
            {
                if (checkZoneId != null && checkCategory == null)
                {
                    throw new Exception($"The given \"category\" does not exist.");
                }
                else if (checkZoneId == null && checkCategory != null)
                {
                    throw new Exception($"The given \"zone id\" does not exist.");
                }
                else if (checkCategory == null && checkZoneId == null)
                {
                    throw new Exception($"The given \"zone id\" and \"category\" does not exist.");
                }
            }
            _dbContext.Add(newRestaurant);
            await _dbContext.SaveChangesAsync();

        }

        public async Task DeleteRestaurantById(int id)
        {
            var result = await _dbContext.Restaurants.FirstOrDefaultAsync(x => x.RestaurantId == id);
            if(result != null)
            {
               var dishes = await _dbContext.Dishes.Where(x => x.RestaurantId == id).ToListAsync();
               _dbContext.Dishes.RemoveRange(dishes);
               _dbContext.Restaurants.Remove(result);
               await _dbContext.SaveChangesAsync();
               
            }
            else
            {
                throw new Exception($"Restaurant with ID: \"{id}\" does not exist!");
            }
            
        }

        public async Task<IEnumerable<Restaurant>> GetAll()
        {
            return await _dbContext.Restaurants
                .Include(x => x.Zone)
                .Include(x => x.Dishes)
                .Include(x => x.CategoryNameNavigation)
                .ToListAsync();
        }

        public async Task<Restaurant> GetById(int id)
        {
            var result = await _dbContext.Restaurants
                .Include(x => x.Zone)
                .Include(x => x.Dishes)
                .Include(x => x.CategoryNameNavigation)
                .FirstOrDefaultAsync(x => x.RestaurantId == id);
            if(result == null)
            {
                return null;
            }
            return result;
        }

        public async Task UpdateRestaurant(int id, Restaurant updateRestaurant)
        {
            var result = await _dbContext.Restaurants.FirstOrDefaultAsync(x => x.RestaurantId == id);
            var checkZoneId = await _dbContext.Zones.FirstOrDefaultAsync(x => x.ZoneId == updateRestaurant.ZoneId);
            var checkCategory = await _dbContext.FoodCategories.FirstOrDefaultAsync(x => x.CategoryName == updateRestaurant.CategoryName);
            if (result == null)
            {
                throw new Exception($"Restaurant with ID: \"{id}\" does not exist!");
            }
            else if (result != null && (checkZoneId == null || checkCategory == null))
            {
                if (checkZoneId != null && checkCategory == null)
                {
                    throw new Exception($"The given \"category\" does not exist.");
                }
                else if (checkZoneId == null && checkCategory != null)
                {
                    throw new Exception($"The given \"zone id\" does not exist.");
                }
                else if (checkCategory == null && checkZoneId == null)
                {
                    throw new Exception($"The given \"zone id\" and \"category\" does not exist.");
                }
            }
            else
            {
                result.RestaurantName = updateRestaurant.RestaurantName;
                result.RestaurantAddress = updateRestaurant.RestaurantAddress;
                result.CategoryName = updateRestaurant.CategoryName;
                result.ZoneId = updateRestaurant.ZoneId;

                await _dbContext.SaveChangesAsync();
            }
        }

      
    }
}
