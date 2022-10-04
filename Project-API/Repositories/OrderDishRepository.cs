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

        public async Task<IEnumerable<OrderDish>> GetByOrderId(int orderId)
        {
            var result = await _context.OrderDishes.Where(x => x.OrderId == orderId).ToListAsync();
            if(result.Count == 0)
            {
                return null;
            }
            return result;
        }

        public async Task<IEnumerable<OrderDish>> GetByDishId(int dishId)
        {
            var result = await _context.OrderDishes.Where(x => x.DishId == dishId).ToListAsync();
            if (result.Count == 0)
            {
                return null;
            }
            return result;
        }

    }
}
