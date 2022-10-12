﻿using AutoMapper;
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
        public OrderController(IOrderRepository order, IMapper mapper)
        {
            this._order = order;
            this.mapper = mapper;
        }
        [HttpGet()]
        public async Task<IActionResult> GetAll()
        {
            var result = await _order.GetAll();
            var resultDTO = mapper.Map<List<OrderDTO>>(result);
            return Ok(resultDTO);
        }
        [HttpGet("{orderId}")]
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
        [HttpPost()]
        public async Task<IActionResult> AddNewOrder([FromBody] OrderRequestModel orderRequest)
        {
            if(orderRequest.RiderId == 0 || orderRequest.IdUser == 0 || orderRequest.OrderStatus == "")
            {
                return BadRequest("The given fields are required!");
            }
            try
            {
                var newOrder = new Order()
                {
                    OrderStatus = orderRequest.OrderStatus,
                    CreatedAt = DateTime.Now,
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
        [HttpDelete("{orderId}")]
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
        [HttpPut("{orderId}")]
        public async Task<IActionResult> UpdateOrder([FromBody]OrderRequestModel request, int orderId)
        {
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
                return BadRequest(ex.Message);
            }
        }
    }
}