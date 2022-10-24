using Microsoft.EntityFrameworkCore;
using Project_API.Models;

namespace Project_API.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly MyDbContext _context;

        public AccountRepository(MyDbContext context)
        {
            this._context = context;
        }
        public async Task Add(Account account)
        {
            await _context.AddAsync(account);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var account = await _context.Accounts.FirstOrDefaultAsync(x => x.Id == id);
            if(account == null)
            {
                throw new Exception($"The account with ID: \"{id}\" does not exist!");
            }
            else
            {
                _context.Accounts.Remove(account);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Account>> GetAll()
        {
            return await _context.Accounts
                .Include(x => x.Restaurant)
                .Include(x => x.IdUsersNavigation)
                .ToListAsync();
        }

        public async Task<Account> GetById(int id)
        {
            var result = await _context.Accounts.FirstOrDefaultAsync(x => x.Id == id);
            if(result == null)
            {
                return null;
            }
            return result;
        }

        public async Task<Account> Login(string username, string password)
        {
            var result = await _context.Accounts.Where(x => x.UserName == username && x.UserPassword == password).FirstOrDefaultAsync();
            if(result == null)
            {
                return null;
            }
            else
            {
                return result;
            }


        }

        public async Task Update(Account account, int id)
        {
            var result = await _context.Accounts.FirstOrDefaultAsync(x => x.Id == id);
            if(result == null)
            {
                throw new Exception($"The account with ID: \"{id}\" does not exist!");
            }
            else
            {
                result.UserPassword = account.UserPassword;
                result.Role = account.Role;
                result.TelNumber = account.TelNumber;
                result.UserName = account.UserName;
                result.RestaurantId = account.RestaurantId;
                result.IdUsers = account.IdUsers;
                result.EmailAddress = account.EmailAddress;

                await _context.SaveChangesAsync();
            }
        }


    }
}
