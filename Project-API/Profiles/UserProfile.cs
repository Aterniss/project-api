using AutoMapper;
using Project_API.DTO;
using Project_API.DTO.RequestModels;
using Project_API.Models;

namespace Project_API.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<User, UserRequestModel>().ReverseMap();
        }
    }
}
