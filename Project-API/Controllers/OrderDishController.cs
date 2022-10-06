using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Project_API.DTO;
using Project_API.DTO.RequestModels;
using Project_API.Models;
using Project_API.Repositories;

namespace Project_API.Controllers
{
    [ApiController]
    [Route("order-dish")]
    public class OrderDishController : Controller
    {
        private readonly IOrderDishRepository _orderDish;
        private readonly IMapper mapper;
        public OrderDishController(IOrderDishRepository orderDish, IMapper mapper)
        {
            this._orderDish = orderDish;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _orderDish.GetAll();
            var resultDTO = mapper.Map<List<OrderDishDTO>>(result);
            return Ok(resultDTO);
        }
        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetByOrderId(int orderId)
        {
            var result = await _orderDish.GetByOrderId(orderId);
            if (result == null)
            {
                return NotFound($"Order ID: \"{orderId}\" has not been found!");
            }
            else
            {
                List<int> dishes = new List<int>();
                foreach (var item in result)
                {
                    dishes.Add(item.DishId);
                }
                //var resultDTO = mapper.Map<List<OrderDishDTO>>(result);
                var resultDTO = new OrderDishesList()
                {
                    OrderId = orderId,
                    DishId = dishes
                };
                return Ok(resultDTO);
            }
        }
        [HttpPost]
        public async Task<IActionResult> AddNewOrderDishes([FromBody] OrderDishAddRequest request)
        {
            if (request.OrderId == 0 || request.DishId == 0)
            {
                return BadRequest("Something went wrong!");
            }
            try
            {
                var addNew = new OrderDish()
                {
                    DishId = request.DishId,
                    OrderId = request.OrderId
                };


                await _orderDish.AddOrderDishes(addNew);

                return Ok("Succesfully added!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [HttpDelete("{orderId}")]
        public async Task<IActionResult> DeleteById(int orderId)
        {
            try
            {
                await _orderDish.DeleteOrderDishes(orderId);
                return Ok("Succesfully deleted!");
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
