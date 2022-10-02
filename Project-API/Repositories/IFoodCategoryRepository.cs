using Project_API.Models;

namespace Project_API.Repositories
{
    public interface IFoodCategoryRepository
    {
        Task<IEnumerable<FoodCategory>> GetAllAsync();
        Task<FoodCategory> GetByNameAsync(string categoryName);
        Task AddAsync(FoodCategory category);
        Task<FoodCategory> DeleteAsync(string name);
        Task<FoodCategory> UpdateAsync(string name, FoodCategory category);

    }
}
