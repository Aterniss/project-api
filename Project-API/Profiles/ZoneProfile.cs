using AutoMapper;
using Project_API.DTO;
using Project_API.Models;

namespace Project_API.Profiles
{
    public class ZoneProfile : Profile
    {
        public ZoneProfile()
        {
            CreateMap<Zone, ZoneDTO>().ReverseMap();
        }
    }
}
