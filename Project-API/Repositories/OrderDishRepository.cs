using Microsoft.EntityFrameworkCore;
using Project_API.Models;

namespace Project_API.Repositories
{
    public class OrderDishRepository : IOrderDishRepository
    {
        private readonly MyDbContext _context;
        public OrderDishRepository(MyDbContext context)
        {
            this._context = context;
        }

        public async Task<IEnumerable<OrderDish>> GetAll()
        {
            return await _context.OrderDishes.ToListAsync();
        }
    }
}
