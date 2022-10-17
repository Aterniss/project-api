using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Project_API.DTO;
using Project_API.DTO.RequestModels;
using Project_API.Models;
using Project_API.Repositories;

namespace Project_API.Controllers
{
    [ApiController]
    [Route("orders")]
    public class OrderController : Controller
    {
        private readonly IOrderRepository _order;
        private readonly IMapper mapper;
        private readonly ILogger<DishController> _logger;
        public OrderController(IOrderRepository order, IMapper mapper, ILogger<DishController> logger)
        {
            this._order = order;
            this.mapper = mapper;
            this._logger = logger;
        }
        [HttpGet()]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                _logger.LogInformation(returnLogMessage("Order", "GetAll"));
                var result = await _order.GetAll();
                var resultDTO = mapper.Map<List<OrderDTO>>(result);
                return Ok(resultDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest("Sorry, we could not load the orders!");
            }

        }
        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetById(int orderId)
        {
            _logger.LogInformation(returnLogMessage("Order", "GetById"));
            var result = await _order.GetById(orderId);
            if(result == null)
            {
                var msg = $"Order with ID: \"{orderId}\" has not been found.";
                _logger.LogWarning(msg);
                return NotFound(msg);
            }
            else
            {
                var resultDTO = mapper.Map<OrderDTO>(result);
                return Ok(resultDTO);
            }
        }
        [HttpPost()]
        public async Task<IActionResult> AddNewOrder([FromBody] OrderRequestModel orderRequest)
        {
            _logger.LogInformation(returnLogMessage("Order", "AddNewOrder"));
            if (orderRequest.RiderId == 0 || orderRequest.IdUser == 0 || orderRequest.OrderStatus == "")
            {
                var msg = "The given fields are required!";
                _logger.LogWarning(msg);
                return BadRequest(msg);
            }
            try
            {
                await _order.AddNewOrder(orderRequest);
                return Ok("Succesfully added!");
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }

        }
        [HttpDelete("{orderId}")]
        public async Task<IActionResult> DeleteOrder(int orderId)
        {
            _logger.LogInformation(returnLogMessage("Order", "DeleteOrder"));
            try
            {
                await _order.DeleteOrder(orderId);
                return Ok("Succesfully deleted!");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return NotFound(ex.Message);
            }
          
        }
        [HttpPut("{orderId}")]
        public async Task<IActionResult> UpdateOrder([FromBody]OrderUpdateRequest request, int orderId)
        {
            _logger.LogInformation(returnLogMessage("Order", "UpdateOrder"));
            if (request.RiderId == 0 || request.IdUser == 0 || request.OrderStatus == "")
            {
                return BadRequest("The given fields are required!");
            }
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
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
        }
        private string returnLogMessage(string controllerClassName,string nameMethod)
        {
            return $"Controller: {controllerClassName}Controller: Request: {nameMethod}()";
        }
    }
}
