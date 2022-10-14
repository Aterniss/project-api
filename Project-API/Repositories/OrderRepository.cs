using Microsoft.EntityFrameworkCore;
using Project_API.DTO.RequestModels;
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

        public async Task AddNewOrder(OrderRequestModel order)
        {
            var checkDishes = await _context.Dishes.Select(x => x.DishId).ToListAsync();
            var dishesList = order.Dishes;
            bool flag = true;
            foreach(var item in dishesList)
            {
                if (!checkDishes.Contains(item))
                {
                    flag = false;
                }
            }
            if(flag == false)
            {
                throw new Exception($"The dishes does not exist!");
            }
            else
            {
                await CheckUserIdAndRiderId(order.IdUser, order.RiderId);
                var newOrder = new Order()
                {
                    OrderStatus = order.OrderStatus,
                    CreatedAt = DateTime.Now,
                    RiderId = order.RiderId,
                    IdUser = order.IdUser
                };
                _context.Orders.Add(newOrder);
                await _context.SaveChangesAsync();
                foreach (var dish in dishesList)
                {
                    var orderDishes = new OrderDish()
                    {
                        OrderId = newOrder.OrderId,
                        DishId = dish
                    };
                    _context.OrderDishes.Add(orderDishes);
                }
                await _context.SaveChangesAsync();
            }




            //await CheckUserIdAndRiderId(order.IdUser, order.RiderId);
            //var newOrder = new Order()
            //{
            //    OrderStatus = order.OrderStatus,
            //    CreatedAt = DateTime.Now,
            //    RiderId = order.RiderId,
            //    IdUser = order.IdUser
            //};
            //_context.Orders.Add(newOrder);
            //await _context.SaveChangesAsync();

            
            //try
            //{
            //    foreach (var dish in dishesList)
            //    {
            //        var orderDishes = new OrderDish()
            //        {
            //            OrderId = newOrder.OrderId,
            //            DishId = dish
            //        };
            //        _context.OrderDishes.Add(orderDishes);
            //    }
            //    await _context.SaveChangesAsync();
            //}
            //catch
            //{
            //    throw new Exception($"The dishes does not exist!");
            //}

            

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
