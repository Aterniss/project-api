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
        public DishController(IDishRepository dish, IMapper mapper)
        {
            this._dish = dish;
            this.mapper = mapper;
        }
        [HttpGet()]
        public async Task<IActionResult> GetAllDishes()
        {
            var result = await _dish.GetAll();
            var resultDTO = mapper.Map<List<DishDTO>>(result);
            return Ok(resultDTO);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _dish.GetDishById(id);
            if(result == null)
            {
                return NotFound($"The dish was not found!");
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
            var result = await _dish.GetDishesByName(dishName);
            if(result == null)
            {
                return NotFound($"The dish: \"{dishName}\" was not found!");
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
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("{dishId}")]
        public async Task<IActionResult> DeleteDish(int dishId)
        {
            try
            {
                await _dish.DeleteDishById(dishId);
                return Ok("Deleted succesfully!");
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost()]
        public async Task<IActionResult> AddNewDish([FromBody]DishRequestModel request)
        {
            if(request.DishDescription =="" || request.RestaurantId == 0 || request.Price == 0)
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
                return BadRequest(ex.Message);
            }


        }
    }
}
