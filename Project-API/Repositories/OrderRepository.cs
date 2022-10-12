using Microsoft.EntityFrameworkCore;
using Project_API.Models;

namespace Project_API.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly MyDbContext _context;
        public OrderRepository(MyDbContext context)
        {
            this._context = context;
        }

        public async Task AddNewOrder(Order order)
        {
            await CheckUserIdAndRiderId(order.IdUser, order.RiderId);
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            

        }

        public async Task DeleteOrder(int orderId)
        {
            var existOrder = await _context.Orders.FirstOrDefaultAsync(x => x.OrderId == orderId);
            if(existOrder == null)
            {
                throw new Exception($"Order with ID: \"{orderId}\" does not exist!");
            }
            else
            {
                var orderDish = await _context.OrderDishes.Where(x => x.OrderId == orderId).ToListAsync();
                _context.Orders.Remove(existOrder);
                _context.OrderDishes.RemoveRange(orderDish);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Order>> GetAll()
        {
            return await _context.Orders
                .Include(x => x.Rider).ThenInclude(x => x.Zone)
                .Include(x => x.IdUserNavigation)
                .Include(x => x.OrderDishes).ThenInclude(x => x.Dish)
                .ToListAsync();
            
        }

        public async Task<Order> GetById(int orderId)
        {
            var existOrder = await _context.Orders
                .Include(x => x.Rider).ThenInclude(x => x.Zone)
                .Include(x => x.IdUserNavigation)
                .Include(x => x.OrderDishes).ThenInclude(x => x.Dish)
                .FirstOrDefaultAsync(x => x.OrderId == orderId);
            if(existOrder == null)
            {
                return null;
            }
            return existOrder;
        }

        public async Task UpdateOrder(Order order, int orderId)
        {
            var existOrder = await _context.Orders.FirstOrDefaultAsync(x => x.OrderId==orderId);
            if( existOrder == null)
            {
                throw new Exception($"Order with ID: \"{orderId}\" does not exist!");
            }
            else
            {
                await CheckUserIdAndRiderId(order.IdUser, order.RiderId);
                existOrder.OrderStatus = order.OrderStatus;
                existOrder.RiderId = order.RiderId;
                existOrder.IdUser = order.IdUser;
                await _context.SaveChangesAsync();
            }
        }

        public async Task CheckUserIdAndRiderId(int userId, int riderId)
        {
            var checkUserId = await _context.Users.FirstOrDefaultAsync(x => x.IdUser == userId);
            var checkRiderId = await _context.Riders.FirstOrDefaultAsync(x => x.RiderId == riderId);
            if (checkRiderId == null && checkUserId == null)
            {
                throw new Exception($"User with ID: \"{userId}\" does not exist!\n Rider with ID: \"{riderId}\" does not exist!");
            }
            else if (checkUserId == null && checkRiderId != null)
            {
                throw new Exception($"User with ID: \"{userId}\" does not exist!");
            }
            else if (checkUserId != null && checkRiderId == null)
            {
                throw new Exception($"Rider with ID: \"{riderId}\" does not exist!");
            }
            else
            {

            }
        }
    }
}
