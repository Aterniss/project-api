using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Project_API.DTO;
using Project_API.DTO.RequestModels;
using Project_API.Models;
using Project_API.Repositories;

namespace Project_API.Controllers
{
    [ApiController]
    [Route("users")]
    public class UserController : Controller
    {
        private readonly IUserRepository _user;
        private readonly IMapper mapper;

        public UserController(IUserRepository userRepository, IMapper mapper)
        {
            this._user = userRepository;
            this.mapper = mapper;
        }

        [HttpGet("get-all-users")]
        public async Task<IActionResult> GetAllUsers()
        {
            var result = await _user.GetAll();
            var resultDTO = mapper.Map<List<UserDTO>>(result);
            return Ok(resultDTO);
        }
        [HttpGet("get-user-by-id/{userId}")]
        public async Task<IActionResult> GetById(int userId)
        {
            var result = await _user.GetById(userId);
            if(result == null)
            {
                return NotFound($"User with ID: \"{userId}\" has not been found!");
            }
            var resultDTO = mapper.Map<UserDTO>(result);
            return Ok(resultDTO);
        }
        [HttpGet("get-user-by-name/{userName}")]
        public async Task<IActionResult> GetByName(string userName)
        {
            var result = await _user.GetByName(userName);
            if(result == null)
            {
                return BadRequest($"User with fullname: \"{userName}\" has not been found!");
            }
            var resultDTO = mapper.Map<List<UserDTO>>(result);
            return Ok(resultDTO);
        }
        [HttpPost("add-user")]
        public async Task<IActionResult> AddUser([FromBody]UserRequestModel request)
        {
            var newUser = new User()
            {
                FullName = request.FullName,
                CreatedAt = DateTime.Now,
                LastUpdate = DateTime.Now,
                UserAddress = request.UserAddress,
                IsOver18 = request.IsOver18
            };
            await _user.AddUser(newUser);
            return Ok("Succesfully added!");
        }
        [HttpPut("update-user-by-id/{userId}")]
        public async Task<IActionResult> UpdateUser([FromBody]UserRequestModel request, int userId)
        {
            try
            {
                var user = new User()
                {
                    FullName = request.FullName,
                    LastUpdate = DateTime.Now,
                    UserAddress = request.UserAddress,
                    IsOver18 = request.IsOver18
                };
                await _user.UpdateUser(user, userId);
                return Ok("Succesfully updated!");
            }
            catch(Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
        [HttpDelete("delete-user-by-id/{userId}")]
        public async Task<IActionResult> DeteleUser(int userId)
        {
            try
            {
                await _user.DeleteUser(userId);
                return Ok("Succesfully deleted!");
            }
            catch(Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
