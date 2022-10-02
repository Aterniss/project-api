using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Project_API.DTO;
using Project_API.Repositories;

namespace Project_API.Controllers
{
    [ApiController]
    [Route("Restaurants")]
    public class RestaurantController : Controller
    {
        private readonly IRestaurantRepository _restaurant;
        private readonly IMapper mapper;
        public RestaurantController(IRestaurantRepository restaurant, IMapper mapper)
        {
            this._restaurant = restaurant;
            this.mapper = mapper;
        }
        [HttpGet("get-all-restaurants")]
        public async Task<IActionResult> GetAllRestaurants()
        {
            var result = await _restaurant.GetAll();
            var restaurantsDTO = mapper.Map<List<RestaurantDTO>>(result);
            return Ok(restaurantsDTO);
        }

    }
}

