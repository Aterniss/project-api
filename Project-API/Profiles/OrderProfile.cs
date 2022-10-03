using AutoMapper;
using Project_API.DTO;
using Project_API.Models;

namespace Project_API.Profiles
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<Order, OrderDTO>().ReverseMap();
            // CreateMap<Rider, RiderDTO>().ReverseMap();
            CreateMap<User, UserDTO>().ReverseMap();
        }
    }
}
