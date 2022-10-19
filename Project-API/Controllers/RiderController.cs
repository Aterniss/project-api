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
        private readonly ILogger<RiderController> _logger;
        public RiderController(IRiderRepository rider, IMapper mapper, ILogger<RiderController> logger)
        {
            this._rider = rider;
            this.mapper = mapper;
            this._logger = logger;
        }
        [HttpGet()]
        public async Task<IActionResult> GetAllRiders()
        {
            try
            {
                _logger.LogInformation(returnLogMessage("Rider", "GetAllRiders"));
                var result = await _rider.GetAll();
                var resultDTO = mapper.Map<List<RiderDTO>>(result);
                return Ok(resultDTO);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest("Sorry, we could not load the riders!");
            }
        }
        [HttpGet("{riderId}")]
        public async Task<IActionResult> GetById(int riderId)
        {
            _logger.LogInformation(returnLogMessage("Rider", "GetById"));
            var result = await _rider.GetById(riderId);
            if(result == null)
            {
                var msg = $"Rider with ID: \"{riderId}\" has not been found!";
                _logger.LogWarning(msg);
                return NotFound(msg);
            }
            else
            {
                var resultDTO = mapper.Map<RiderDTO>(result);
                return Ok(resultDTO);
            }
        }
        [HttpPost()]
        public async Task<IActionResult> AddRider([FromBody]RiderRequestModel request)
        {
            _logger.LogInformation(returnLogMessage("Rider", "AddRider"));
            if (request.RiderName == "" || request.ZoneId <= 0)
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
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("{riderId}")]
        public async Task<IActionResult> DeleteRiderById(int riderId)
        {
            _logger.LogInformation(returnLogMessage("Rider", "DeleteRiderById"));
            try
            {
                await _rider.DeleteRider(riderId);
                return Ok("Succesfully deleted!");
            }
            catch(Exception ex)
            {
                _logger.LogWarning(ex.Message);
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("{riderId}")]
        public async Task<IActionResult> UpdateRiderById([FromBody] RiderRequestModel request, int riderId)
        {
            _logger.LogInformation(returnLogMessage("Rider", "UpdateRiderById"));
            if (request.RiderName == "" || request.ZoneId == 0)
            {
                var msg = "The given fields are required!";
                _logger.LogError(msg);
                return BadRequest(msg);
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
