using Project_API.Models;

namespace Project_API.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        public Task Add(Account account)
        {
            throw new NotImplementedException();
        }

        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Account>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<Account> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task Update(Account account, int id)
        {
            throw new NotImplementedException();
        }
    }
}
