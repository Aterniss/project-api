using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Project_API.DTO;
using Project_API.DTO.RequestModels;
using Project_API.Models;
using Project_API.Repositories;

namespace Project_API.Controllers
{
    [ApiController]
    [Route("zones")]
    public class ZoneController : Controller
    {
        private readonly IZoneRepository _zone;
        private readonly IMapper mapper;
        private readonly ILogger<ZoneController> _logger;
        public ZoneController(IZoneRepository zone, IMapper mapper, ILogger<ZoneController> logger)
        {
            this._zone = zone;
            this.mapper = mapper;
            this._logger = logger;
        }

        [HttpGet()]
        public async Task<IActionResult> GetAllZones()
        {
            try
            {
                _logger.LogInformation(returnLogMessage("Zone", "GetAllZones"));
                var result = await _zone.GetAll();
                var resultDTO = mapper.Map<List<ZoneDTO>>(result);
                return Ok(resultDTO);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest("Sorry, we could not load the zones!");
            }
        }
        [HttpGet("{zoneId}")]
        public async Task<IActionResult> GetById(int zoneId)
        {
            _logger.LogInformation(returnLogMessage("Zone", "GetById"));
            var result = await _zone.GetById(zoneId);
            if(result == null)
            {
                var msg = $"Zone with ID: \"{zoneId}\" has not been found!";
                _logger.LogWarning(msg);
                return NotFound(msg);
            }
            else
            {
                var resultDTO = mapper.Map<ZoneDTO>(result);
                return Ok(resultDTO);
            }
        }
        [HttpPost()]
        public async Task<IActionResult> AddZone([FromBody]ZoneRequestModel request)
        {
            _logger.LogInformation(returnLogMessage("Zone", "AddZone"));
            if (request.ZoneName == "")
            {
                var msg = "This field is required!";
                _logger.LogWarning(msg);
                return BadRequest(msg);
            }
            try
            {
                var newZone = new Zone()
                {
                    ZoneName = request.ZoneName
                };
                await _zone.AddZone(newZone);
                return Ok("Succesfully added!");
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("{zoneId}")]
        public async Task<IActionResult> UpdateZone([FromBody]ZoneRequestModel request, int zoneId)
        {
            _logger.LogInformation(returnLogMessage("Zone", "UpdateZone"));
            if (request.ZoneName == "")
            {
                var msg = "This field is required!";
                _logger.LogWarning(msg);
                return BadRequest(msg);
            }
            try
            {
                var updateZone = new ZoneDTO()
                {
                    ZoneName = request.ZoneName
                };
                var zone = mapper.Map<Zone>(updateZone);
                await _zone.UpdateZone(zone, zoneId);
                return Ok("Succesfully updated!");
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("{zoneId}")]
        public async Task<IActionResult> DeleteZone(int zoneId)
        {
            _logger.LogInformation(returnLogMessage("Zone", "DeleteZone"));
            try
            {
                await _zone.DeleteZone(zoneId);
                return Ok("Succesfully deleted!");
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
        }
        private string returnLogMessage(string controllerClassName, string nameMethod)
        {
            return $"Controller: {controllerClassName}Controller: Request: {nameMethod}()";
        }
    }
}
