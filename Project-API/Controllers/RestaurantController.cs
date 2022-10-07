using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Project_API.DTO;
using Project_API.DTO.RequestModels;
using Project_API.Models;
using Project_API.Repositories;

namespace Project_API.Controllers
{
    [ApiController]
    [Route("restaurants")]
    public class RestaurantController : Controller
    {
        private readonly IRestaurantRepository _restaurant;
        private readonly IMapper mapper;
        public RestaurantController(IRestaurantRepository restaurant, IMapper mapper)
        {
            this._restaurant = restaurant;
            this.mapper = mapper;
        }
        [HttpGet()]
        public async Task<IActionResult> GetAllRestaurants()
        {
            var result = await _restaurant.GetAll();
            var restaurantsDTO = mapper.Map<List<RestaurantDTO>>(result);
            return Ok(restaurantsDTO);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _restaurant.GetById(id);
            if(result == null)
            {
                return NotFound($"Restaurant with ID: \"{id}\" was not founded!");
            }
            var resultDTO = mapper.Map<RestaurantDTO>(result);
            return Ok(resultDTO);
        }
        [HttpPost()]
        public async Task<IActionResult> AddRestaurant([FromBody]RestaurantRequestModel newRestaurant)
        {
            if(newRestaurant.ZoneId == 0)
            {
                return BadRequest("The given field: \"zone id\" is required!");
            }
            try
            { 
            var addRestaurant = new Restaurant()
            {
                RestaurantName = newRestaurant.RestaurantName,
                CategoryName = newRestaurant.CategoryName,
                RestaurantAddress = newRestaurant.RestaurantAddress,
                ZoneId = newRestaurant.ZoneId
            };
            await _restaurant.AddRestaurant(addRestaurant);
            return Ok($"Restaurant {newRestaurant.RestaurantName} is added!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteById(int id)
        {
            try
            {
                await _restaurant.DeleteRestaurantById(id);
                return Ok($"Restaurant is deleted succesfully!");
            }
            catch(Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
        [HttpDelete("name/{restaurantName}")]
        public async Task<IActionResult> DeleteByName(string restaurantName)
        {
            try
            {
                await _restaurant.DeleteRestaurantByName(restaurantName);
                return Ok($"Restaurant is deleted succesfully!");
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRestaurant([FromBody]RestaurantRequestModel restaurant, int id)
        {
            if (restaurant.ZoneId == 0)
            {
                return BadRequest("The given field: \"zone id\" is required!");
            }
            try
            {
                var updateRestaurant = mapper.Map<Restaurant>(restaurant);
                await _restaurant.UpdateRestaurant(id, updateRestaurant);
                return Ok("The restaurant has been updated!");
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //end
    }
}

