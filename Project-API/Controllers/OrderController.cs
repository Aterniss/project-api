using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Project_API.DTO;
using Project_API.DTO.RequestModels;
using Project_API.Models;
using Project_API.Repositories;

namespace Project_API.Controllers
{
    [ApiController]
    [Route("Orders")]
    public class OrderController : Controller
    {
        private readonly IOrderRepository _order;
        private readonly IMapper mapper;
        public OrderController(IOrderRepository order, IMapper mapper)
        {
            this._order = order;
            this.mapper = mapper;
        }
        [HttpGet("get-all-orders")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _order.GetAll();
            var resultDTO = mapper.Map<List<OrderDTO>>(result);
            return Ok(resultDTO);
        }
        [HttpGet("get-order-by-id/{orderId}")]
        public async Task<IActionResult> GetById(int orderId)
        {
            var result = await _order.GetById(orderId);
            if(result == null)
            {
                return NotFound($"Order with ID: \"{orderId}\" has not been found.");
            }
            else
            {
                var resultDTO = mapper.Map<OrderDTO>(result);
                return Ok(resultDTO);
            }
        }

        [HttpPost("add-new-order")]
        public async Task<IActionResult> AddNewOrder([FromBody] OrderRequestModel orderRequest)
        {
            try
            {
                var newOrder = new Order()
                {
                    OrderStatus = orderRequest.OrderStatus,
                    RiderId = orderRequest.RiderId,
                    IdUser = orderRequest.IdUser
                };
                await _order.AddNewOrder(newOrder);
                return Ok("Succesfully added!");
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [HttpDelete("delete-order-by-id/{orderId}")]
        public async Task<IActionResult> DeleteOrder(int orderId)
        {
            try
            {
                await _order.DeleteOrder(orderId);
                return Ok("Succesfully deleted!");
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
          
        }
        [HttpPut("update-order-by-id/{orderId}")]
        public async Task<IActionResult> UpdateOrder([FromBody]OrderRequestModel request, int orderId)
        {
            try
            {
                var updateOrder = new Order()
                {
                    OrderStatus = request.OrderStatus,
                    RiderId = request.RiderId,
                    IdUser = request.IdUser
                };
                await _order.UpdateOrder(updateOrder, orderId);
                return Ok("Succesfully updated!");
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
