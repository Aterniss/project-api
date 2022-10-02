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

        public async Task<IEnumerable<Restaurant>> GetAll()
        {
            return await _dbContext.Restaurants.IgnoreAutoIncludes().ToListAsync();
        }
    }
}
