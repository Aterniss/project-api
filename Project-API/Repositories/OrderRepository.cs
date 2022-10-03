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
            await CheckUserIdAndRiderId(order.IdUser, order.OrderId);
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
                _context.Orders.Remove(existOrder);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Order>> GetAll()
        {
            return await _context.Orders.ToListAsync();
            
        }

        public async Task<Order> GetById(int orderId)
        {
            var existOrder = await _context.Orders.FirstOrDefaultAsync(x => x.OrderId == orderId);
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
            var checkUserId = await _context.Users.Select(x => x.IdUser == userId).FirstOrDefaultAsync();
            var checkRiderId = await _context.Riders.Select(x => x.RiderId == riderId).FirstOrDefaultAsync();
            if (checkRiderId == false && checkUserId == false)
            {
                throw new Exception($"User with ID: \"{userId}\" does not exist!\n Rider with ID: \"{riderId}\" does not exist!");
            }
            else if (checkUserId == false && checkRiderId == true)
            {
                throw new Exception($"User with ID: \"{userId}\" does not exist!");
            }
            else if (checkUserId == true && checkRiderId == false)
            {
                throw new Exception($"Rider with ID: \"{riderId}\" does not exist!");
            } 
        }
    }
}
