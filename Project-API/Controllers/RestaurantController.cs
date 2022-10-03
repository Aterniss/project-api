﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Project_API.DTO;
using Project_API.DTO.RequestModels;
using Project_API.Models;
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
        [HttpGet("get-restaurant-by-id/{id}")]
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
        [HttpPost("add-restaurant")]
        public async Task<IActionResult> AddRestaurant([FromBody]RestaurantRequestModel newRestaurant)
        {
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
        [HttpDelete("delete-restaurant-by-id/{restaurantId}")]
        public async Task<IActionResult> DeleteById(int restaurantId)
        {
            try
            {
                await _restaurant.DeleteRestaurantById(restaurantId);
                return Ok($"Restaurant is deleted succesfully!");
            }
            catch(Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
        [HttpDelete("delete-restaurant-by-name/{restaurantName}")]
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
        [HttpPut("update-restaurant/{id}")]
        public async Task<IActionResult> UpdateRestaurant([FromBody]RestaurantRequestModel restaurant, int id)
        {
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

