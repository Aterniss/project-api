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
        private readonly ILogger<UserController> _logger;
        public UserController(IUserRepository userRepository, IMapper mapper, ILogger<UserController> logger)
        {
            this._user = userRepository;
            this.mapper = mapper;
            this._logger = logger;
        }

        [HttpGet()]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                _logger.LogInformation(returnLogMessage("User", "GetAllUsers"));
                var result = await _user.GetAll();
                var resultDTO = mapper.Map<List<UserDTO>>(result);
                return Ok(resultDTO);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest("Sorry, we could not load the users!");
            }
        }
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetById(int userId)
        {
            if(userId <= 0)
            {
                return BadRequest("ID must be greater than 0!");
            }
            try
            {
                _logger.LogInformation(returnLogMessage("Rider", "GetById"));
                var result = await _user.GetById(userId);
                if (result == null)
                {
                    var msg = $"User with ID: \"{userId}\" has not been found!";
                    _logger.LogWarning(msg);
                    return NotFound(msg);
                }
                var resultDTO = mapper.Map<UserDTO>(result);
                return Ok(resultDTO);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest("Something went wrong!");
            }
        }
        [HttpGet("get-by-name/{userName}")]
        public async Task<IActionResult> GetByName(string userName)
        {
            try
            {
                _logger.LogInformation(returnLogMessage("Rider", "GetByName"));
                var result = await _user.GetByName(userName);
                if (result == null)
                {
                    var msg = $"User with fullname: \"{userName}\" has not been found!";
                    _logger.LogWarning(msg);
                    return BadRequest(msg);
                }
                var resultDTO = mapper.Map<List<UserDTO>>(result);
                return Ok(resultDTO);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest("Something went wrong!");
            }
        }
        [HttpPost()]
        public async Task<IActionResult> AddUser([FromBody]UserRequestModel request)
        {
            if (request.FullName == "" || request.UserAddress == "")
            {
                var msg = $"The given fields: \"Your name\" and \"Your address\" are required!";
                _logger.LogWarning(msg);
                return BadRequest(msg);
            }
            else
            {
                try
                {
                    _logger.LogInformation(returnLogMessage("Rider", "AddUser"));
                    if (request.FullName == "" || request.UserAddress == "")
                    {
                        var msg = $"The given fields: \"Your name\" and \"Your address\" are required!";
                        _logger.LogWarning(msg);
                        return BadRequest(msg);
                    }
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
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    return BadRequest("sorry you could not add new user!");
                }
            }
            
        }
        [HttpPut("{userId}")]
        public async Task<IActionResult> UpdateUser([FromBody]UserRequestModel request, int userId)
        {
            _logger.LogInformation(returnLogMessage("Rider", "UpdateUser"));
            if (request.FullName == "" || request.UserAddress == "")
            {
                var msg = $"The given fields: \"Your name\" and \"Your address\" are required!";
                _logger.LogWarning(msg);
                return BadRequest(msg);
            }
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
                _logger.LogError(ex.Message);
                return NotFound(ex.Message);
            }
        }
        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeteleUser(int userId)
        {
            _logger.LogInformation(returnLogMessage("Rider", "DeleteUser"));
            try
            {
                await _user.DeleteUser(userId);
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
