using AutoMapper;
using Project_API.DTO;
using Project_API.Models;

namespace Project_API.Profiles
{
    public class FoodCategoryProfile : Profile
    {
        public FoodCategoryProfile()
        {
            CreateMap<FoodCategory, FoodCategoryDTO>()
                .ReverseMap();
        }
    }
}
