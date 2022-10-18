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
        private readonly ILogger<RestaurantController> _logger;
        public RestaurantController(IRestaurantRepository restaurant, IMapper mapper, ILogger<RestaurantController> logger)
        {
            this._restaurant = restaurant;
            this.mapper = mapper;
            this._logger = logger;
        }
        [HttpGet()]
        public async Task<IActionResult> GetAllRestaurants()
        {
            try
            {
                _logger.LogInformation(returnLogMessage("Restaurant", "GetAllRestaurants"));
                var result = await _restaurant.GetAll();
                var restaurantsDTO = mapper.Map<List<RestaurantDTO>>(result);
                return Ok(restaurantsDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest("Sorry, we could not load the restaurants!");
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            _logger.LogInformation(returnLogMessage("Restaurant", "GetById"));
            var result = await _restaurant.GetById(id);
            if(result == null)
            {
                _logger.LogWarning("not found!");
                return NotFound($"Restaurant with ID: \"{id}\" was not founded!");
            }
            var resultDTO = mapper.Map<RestaurantDTO>(result);
            return Ok(resultDTO);
        }
        [HttpPost()]
        public async Task<IActionResult> AddRestaurant([FromBody]RestaurantRequestModel newRestaurant)
        {
            _logger.LogInformation(returnLogMessage("Restaurant", "AddRestaurant"));
            if (newRestaurant.ZoneId == 0)
            {
                var msg = "The given field: \"zone id\" is required!";
                _logger.LogError(msg);
                return BadRequest(msg);
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
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteById(int id)
        {
            _logger.LogInformation(returnLogMessage("Restaurant", "DeleteById"));
            try
            {
                await _restaurant.DeleteRestaurantById(id);
                return Ok($"Restaurant is deleted succesfully!");
            }
            catch(Exception ex)
            {
                _logger.LogWarning(ex.Message);
                return NotFound(ex.Message);
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRestaurant([FromBody]RestaurantRequestModel restaurant, int id)
        {
            _logger.LogInformation(returnLogMessage("Restaurant", "UpdateRestaurant"));
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
                _logger.LogWarning(ex.Message);
                return BadRequest(ex.Message);
            }
        }
        private string returnLogMessage(string controllerClassName, string nameMethod)
        {
            return $"Controller: {controllerClassName}Controller: Request: {nameMethod}()";
        }
     
    }
}

