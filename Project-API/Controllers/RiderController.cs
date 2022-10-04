using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Project_API.DTO;
using Project_API.DTO.RequestModels;
using Project_API.Models;
using Project_API.Repositories;

namespace Project_API.Controllers
{
    [ApiController]
    [Route("riders")]
    public class RiderController : Controller
    {
        private readonly IRiderRepository _rider;
        private readonly IMapper mapper;
        public RiderController(IRiderRepository rider, IMapper mapper)
        {
            this._rider = rider;
            this.mapper = mapper;
        }
        [HttpGet("get-all-riders")]
        public async Task<IActionResult> GetAllRiders()
        {
            var result = await _rider.GetAll();
            var resultDTO = mapper.Map<List<RiderDTO>>(result);
            return Ok(resultDTO);
        }
        [HttpGet("get-rider-by-id/{riderId}")]
        public async Task<IActionResult> GetById(int riderId)
        {
            var result = await _rider.GetById(riderId);
            if(result == null)
            {
                return NotFound($"Rider with ID: \"{riderId}\" has not been found!");
            }
            else
            {
                var resultDTO = mapper.Map<RiderDTO>(result);
                return Ok(resultDTO);
            }
        }
        [HttpPost("add-new-rider")]
        public async Task<IActionResult> AddRider([FromBody]RiderRequestModel request)
        {
            if (request.RiderName == "" || request.ZoneId == 0)
            {
                return BadRequest("The given fields are required!");
            }
            try
            {
                var newRider = new Rider()
                {
                    RiderName = request.RiderName,
                    ZoneId = request.ZoneId
                };
                await _rider.AddRider(newRider);
                return Ok("Succesfully added!");
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("delete-rider-by-id/{riderId}")]
        public async Task<IActionResult> DeleteRiderById(int riderId)
        {
            try
            {
                await _rider.DeleteRider(riderId);
                return Ok("Succesfully deleted!");
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("update-rider-by-id/{riderId}")]
        public async Task<IActionResult> UpdateRiderById([FromBody] RiderRequestModel request, int riderId)
        {
            if (request.RiderName == "" || request.ZoneId == 0)
            {
                return BadRequest("The given fields are required!");
            }
            try
            {
                var updateRider = new Rider()
                {
                    RiderName = request.RiderName,
                    ZoneId = request.ZoneId
                };
                await _rider.UpdateRider(updateRider, riderId);
                return Ok("Succesfully updated!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
