using AutoMapper;
using Project_API.DTO;
using Project_API.Models;

namespace Project_API.Profiles
{
    public class RoleProfile : Profile
    {
        public RoleProfile()
        {
            CreateMap<Role, RoleDTO>().ReverseMap();
        }
    }
}
