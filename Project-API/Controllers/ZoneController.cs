using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Project_API.DTO;
using Project_API.DTO.RequestModels;
using Project_API.Models;
using Project_API.Repositories;

namespace Project_API.Controllers
{
    [ApiController]
    [Route("Zones")]
    public class ZoneController : Controller
    {
        private readonly IZoneRepository _zone;
        private readonly IMapper mapper;
        public ZoneController(IZoneRepository zone, IMapper mapper)
        {
            this._zone = zone;
            this.mapper = mapper;
        }

        [HttpGet("get-all-zones")]
        public async Task<IActionResult> GetAllZones()
        {
            var result = await _zone.GetAll();
            var resultDTO = mapper.Map<List<ZoneDTO>>(result);
            return Ok(resultDTO);
        }
        [HttpGet("get-zone-by-id/{zoneId}")]
        public async Task<IActionResult> GetById(int zoneId)
        {
            var result = await _zone.GetById(zoneId);
            if(result == null)
            {
                return NotFound($"Zone with ID: \"{zoneId}\" has not been found!");
            }
            else
            {
                var resultDTO = mapper.Map<ZoneDTO>(result);
                return Ok(resultDTO);
            }
        }
        [HttpPost("add-new-zone")]
        public async Task<IActionResult> AddZone([FromBody]ZoneRequestModel request)
        {
            if(request.ZoneName == "")
            {
                return BadRequest("This field is required~!");
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
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("update-zone-by-id/{zoneId}")]
        public async Task<IActionResult> UpdateZone([FromBody]ZoneRequestModel request, int zoneId)
        {
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
                return NotFound(ex.Message);
            }
        }
        [HttpDelete("delete-zone-by-id/{zoneId}")]
        public async Task<IActionResult> DeleteZone(int zoneId)
        {
            try
            {
                await _zone.DeleteZone(zoneId);
                return Ok("Succesfully deleted!");
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
