using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Project_API.DTO;
using Project_API.DTO.RequestModels;
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
        [HttpGet("order-dish/order-id/{orderId}")]
        public async Task<IActionResult> GetByOrderId(int orderId)
        {
            var result = await _orderDish.GetByOrderId(orderId);
            if(result == null)
            {
                return NotFound($"Order ID: \"{orderId}\" has not been found!");
            }
            else
            {
                List<int> dishes = new List<int>();
                foreach(var item in result)
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
        [HttpGet("order-dish/dish-id/{dishId}")]
        public async Task<IActionResult> GetByDishId(int dishId)
        {
            var result = await _orderDish.GetByDishId(dishId);
            if (result == null)
            {
                return NotFound($"Dish ID: \"{dishId}\" has not been found!");
            }
            else
            {
                List<int> orders = new List<int>();
                foreach (var item in result)
                {
                    orders.Add(item.OrderId);
                }
                //var resultDTO = mapper.Map<List<OrderDishDTO>>(result);
                var resultDTO = new DishOrdersList()
                {
                    OrderId = orders,
                    DishId = dishId
                };
                return Ok(resultDTO);
            }
        }


    }
}
