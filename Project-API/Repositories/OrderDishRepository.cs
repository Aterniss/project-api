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

        public async Task AddOrderDishes(OrderDish orderDish)
        {
            var checkOrderId = await _context.Orders.FirstOrDefaultAsync(x => x.OrderId == orderDish.OrderId);
            var checkDishId = await _context.Dishes.FirstOrDefaultAsync(x => x.DishId == orderDish.DishId);
            if(checkOrderId == null && checkDishId == null)
            {
                throw new Exception($"Order with ID: \"{orderDish.OrderId}\" does not exist!\nDish with ID: \"{orderDish.DishId}\" does not exist!");
            }
            else if(checkDishId == null && checkOrderId != null)
            {
                throw new Exception($"Dish with ID: \"{orderDish.DishId}\" does not exist!");
            }
            else if(checkOrderId == null && checkDishId != null)
            {
                throw new Exception($"Order with ID: \"{orderDish.OrderId}\" does not exist!");
            }
            else
            {
                await _context.OrderDishes.AddAsync(orderDish);
                await _context.SaveChangesAsync();
            }

        }

        public async Task DeleteOrderDishes(int orderId)
        {
            var result = await _context.OrderDishes.Where(x => x.OrderId == orderId).ToListAsync();
            if (result.Count == 0)
            {
                throw new Exception($"Order with ID: \"{orderId}\" does not exist!");
            }
            else
            {
                _context.OrderDishes.RemoveRange(result);
                var order = await _context.Orders.Where(x => x.OrderId == orderId).FirstOrDefaultAsync();
                if(order != null)
                {
                _context.Orders.Remove(order);
                }
                await _context.SaveChangesAsync();
            }
        }

        
    }
}
