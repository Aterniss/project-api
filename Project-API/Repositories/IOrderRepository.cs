using Project_API.DTO.RequestModels;
using Project_API.Models;

namespace Project_API.Repositories
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetAll();
        Task<Order> GetById(int orderId);
        Task AddNewOrder(OrderRequestModel order);
        Task DeleteOrder(int orderId);
        Task UpdateOrder(Order order, int orderId);
    }
}
