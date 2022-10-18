using AutoMapper;
using Project_API.DTO;
using Project_API.Models;

namespace Project_API.Profiles
{
    public class RiderProfile : Profile
    {
        public RiderProfile()
        {
            CreateMap<Rider, RiderDTO>().ReverseMap();
            CreateMap<Zone, ZoneDTO>().ReverseMap();
        }
    }
}
