using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Project_API.DTO;
using Project_API.DTO.RequestModels;
using Project_API.Models;
using Project_API.Repositories;

namespace Project_API.Controllers
{
    [ApiController]
    [Route("dishes")]
    public class DishController : Controller
    {
        private readonly IDishRepository _dish;
        private readonly IMapper mapper;
        private readonly ILogger<DishController> _logger; 
        public DishController(IDishRepository dish, IMapper mapper, ILogger<DishController> logger)
        {
            this._dish = dish;
            this.mapper = mapper;
            this._logger = logger;
        }
        [HttpGet()]
        public async Task<IActionResult> GetAllDishes()
        {
            _logger.LogInformation(returnLogMessage("Dish", "GetAllDishes"));
            try
            {
                var result = await _dish.GetAll();
                var resultDTO = mapper.Map<List<DishDTO>>(result);
                return Ok(resultDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest("Sorry, we could not load the dishes!");
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            _logger.LogInformation(returnLogMessage("Dish", "GetById"));
            var result = await _dish.GetDishById(id);
            if(result == null)
            {
                var msg = $"The dish with id: {id} was not found!";
                _logger.LogWarning(msg);
                return NotFound(msg);
            }
            else
            {
                var resultDTO = mapper.Map<DishDTO>(result);
                return Ok(resultDTO);
            }
        }
        [HttpGet("dishes-name/{dishName}")]
        public async Task<IActionResult> GetByName(string dishName)
        {
            _logger.LogInformation(returnLogMessage("Dish","GetByName"));
            var result = await _dish.GetDishesByName(dishName);
            if(result == null)
            {
                var msg = $"The dish: \"{dishName}\" was not found!";
                _logger.LogWarning(msg);
                return NotFound(msg);
            }
            else
            {
                var resultDTO = mapper.Map<List<DishDTO>>(result);
                return Ok(resultDTO);
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDish([FromBody]DishRequestModel request, int id)
        {
            _logger.LogInformation(returnLogMessage("Dish", "UpdateDish"));
            if (request.DishDescription == "" || request.RestaurantId == 0 || request.Price == 0)
            {
                return BadRequest($"Fields: \"Dish_description\", \"restaurant id\" and \"price\" are required!");
            }
            try
            {
                var dishDTO = new DishDTO()
                {
                    DishId = id,
                    DishName = request.DishName,
                    DishDescription = request.DishDescription,
                    Price = request.Price,
                    RestaurantId = request.RestaurantId,
                    Require18 = request.Require18
                };
                var updateDish = mapper.Map<Dish>(dishDTO);
                await _dish.UpdateDishById(updateDish, id);
                return Ok("Successfully updated!");
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("{dishId}")]
        public async Task<IActionResult> DeleteDish(int dishId)
        {
            _logger.LogInformation(returnLogMessage("Dish", "DeleteDish"));
            try
            {
                await _dish.DeleteDishById(dishId);
                return Ok("Deleted succesfully!");
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
        }
        [HttpPost()]
        public async Task<IActionResult> AddNewDish([FromBody]DishRequestModel request)
        {
            _logger.LogInformation(returnLogMessage("Dish", "AddNewDish"));
            if (request.DishDescription =="" || request.RestaurantId == 0 || request.Price == 0)
            {
                return BadRequest($"Fields: \"Dish_description\", \"restaurant id\" and \"price\" are required!");
            }
            try
            {
     
                    var newDish = mapper.Map<Dish>(request);
                    await _dish.AddNewDish(newDish);
                    return Ok("Succesfully added!");
                
            }
            catch(Exception ex)
            {
                _logger?.LogError(ex.Message);
                return BadRequest(ex.Message);
            }


        }
        private string returnLogMessage(string controllerClassName, string nameMethod)
        {
            return $"Controller: {controllerClassName}Controller: Request: {nameMethod}()";
        }
    }
}
