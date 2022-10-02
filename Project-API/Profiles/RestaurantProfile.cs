using AutoMapper;
using Project_API.DTO;
using Project_API.DTO.RequestModels;
using Project_API.Models;

namespace Project_API.Profiles
{
    public class RestaurantProfile : Profile
    {
        public RestaurantProfile()
        {
            CreateMap<Restaurant, RestaurantDTO>()
                .ReverseMap();
            CreateMap<FoodCategory, FoodCategoryDTO>()
         .ReverseMap();
            CreateMap<Zone, ZoneDTO>()
         .ReverseMap();
            CreateMap<Restaurant, RestaurantAddRequest>()
         .ReverseMap();
        }
    }
}
