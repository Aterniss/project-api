using AutoMapper;
using Project_API.DTO;
using Project_API.Models;

namespace Project_API.Profiles
{
    public class OrderDishProfile : Profile
    {
        public OrderDishProfile()
        {
            CreateMap<OrderDish, OrderDishDTO>().ReverseMap();
            CreateMap<Order, OrderDTO>().ReverseMap();
            CreateMap<Dish, DishDTO>().ReverseMap();
          
        }
    }
}
