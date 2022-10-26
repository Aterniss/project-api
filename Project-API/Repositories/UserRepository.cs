using Microsoft.EntityFrameworkCore;
using Project_API.Models;

namespace Project_API.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly MyDbContext _context;

        public UserRepository(MyDbContext context)
        {
            this._context = context;
        }

        public async Task AddUser(User newUser)
        {
            await _context.Users.AddAsync(newUser);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUser(int id)
        {
            var existUser = await _context.Users.FirstOrDefaultAsync(x => x.IdUser == id);
            if(existUser == null)
            {
                throw new Exception($"User with ID: \"{id}\" does not exist!");
            }
            else
            {
                var orders = await _context.Orders.Where(x => x.IdUser == id).ToListAsync();
                if (orders.Any())
                {
                    foreach(var order in orders)
                    {
                        var dishesTodelete = await _context.OrderDishes.Where(x => x.OrderId == order.OrderId).ToListAsync();
                        _context.RemoveRange(dishesTodelete);
                    }
                    _context.Orders.RemoveRange(orders);
                }
                _context.Users.Remove(existUser);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            return await _context.Users
                .ToListAsync();
        }

        public async Task<User> GetById(int id)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(x => x.IdUser == id);
            if(existingUser == null)
            {
                return null;
            }
            return existingUser;
        }

        public async Task<IEnumerable<User>> GetByName(string fullName)
        {
            var users = await _context.Users.Include(x => x.Orders).Where(x => x.FullName == fullName).ToListAsync();
            if(users.Count == 0)
            {
                return null;
            }
            return users;
        }

        public async Task UpdateUser(User user, int id)
        {
            var existUser = await _context.Users.FirstOrDefaultAsync(x => x.IdUser == id);
            if(existUser == null)
            {
                throw new Exception($"User with ID: \"{id}\" does not exist!");
            }
            else
            {
                existUser.FullName = user.FullName;
                existUser.UserAddress = user.UserAddress;
                existUser.LastUpdate = DateTime.Now;
                existUser.IsOver18 = user.IsOver18;
                await _context.SaveChangesAsync();
            }

        }

    }
}
