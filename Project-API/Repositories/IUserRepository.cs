using Project_API.Models;

namespace Project_API.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAll();
        Task<User> GetById(int id);
        Task<IEnumerable<User>> GetByName(string fullName);
        Task AddUser(User newUser);
        Task UpdateUser(User user, int id);
        Task DeleteUser(int id);
    }
}
