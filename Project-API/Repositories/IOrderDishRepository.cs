using Project_API.Models;

namespace Project_API.Repositories
{
    public interface IOrderDishRepository
    {
        Task<IEnumerable<OrderDish>> GetAll();
        Task<IEnumerable<OrderDish>> GetByOrderId(int orderId);
        Task AddOrderDishes(OrderDish orderDish);
        Task DeleteOrderDishes(int orderId);
    }
}
