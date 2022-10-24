using Project_API.Models;

namespace Project_API.Repositories
{
    public interface IAccountRepository
    {
        Task<IEnumerable<Account>> GetAll();
        Task<Account> GetById(int id);
        Task Add(Account account);
        Task Update(Account account, int id);
        Task Delete(int id);
        Task<Account> Login(string username, string password);

    }
}
