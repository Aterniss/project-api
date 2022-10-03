using AutoMapper;
using Project_API.DTO;
using Project_API.DTO.RequestModels;
using Project_API.Models;

namespace Project_API.Profiles
{
    public class DishProfile : Profile
    {
        public DishProfile()
        {
            CreateMap<Dish, DishDTO>().ReverseMap();
            CreateMap<Restaurant, RestaurantDTO>().ReverseMap();
            CreateMap<Dish, DishRequestModel>().ReverseMap();
        }
    }
}
