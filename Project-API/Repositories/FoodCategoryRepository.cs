using Microsoft.EntityFrameworkCore;
using Project_API.Models;

namespace Project_API.Repositories
{
    public class FoodCategoryRepository : IFoodCategoryRepository
    {

        private readonly MyDbContext _context;

        public FoodCategoryRepository(MyDbContext dbContext)
        {
            this._context = dbContext;
        }

        public async Task AddAsync(FoodCategory category)
        {
            await _context.AddAsync(category);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string name)
        {
            var response = await _context.FoodCategories.FirstOrDefaultAsync(x => x.CategoryName == name);
            if (response == null)
            {
                throw new Exception($"Category: \"{name}\" is not founded!");
            }
            else
            {
                _context.FoodCategories.Remove(response);
                await _context.SaveChangesAsync();
            }

           
        }

        public async Task<IEnumerable<FoodCategory>> GetAllAsync()
        {
            return await _context.FoodCategories.ToListAsync();

        }

        public async Task<FoodCategory> GetByNameAsync(string categoryName)
        {
            var response = await _context.FoodCategories.FirstOrDefaultAsync(n => n.CategoryName == categoryName);
#pragma warning disable CS8603 // Possible null reference return.
            return response;
#pragma warning restore CS8603 // Possible null reference return.
        }

        public async Task<FoodCategory> UpdateAsync(string name, FoodCategory category)
        {
            var response = await _context.FoodCategories.FirstOrDefaultAsync(n => n.CategoryName == name);
            if (response == null)
            {
                return null; // when category is not founded!
            }

            response.CategoryName = category.CategoryName;
            response.CategoryDescription = category.CategoryDescription;
            await _context.SaveChangesAsync();
            return response; // 2 - when everything is ok.

        }
    }
}