using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Project_API.DTO;
using Project_API.DTO.RequestModels;
using Project_API.Models;
using Project_API.Repositories;

namespace Project_API.Controllers
{
    [ApiController]
    [Route("accounts")]
    public class AccountController : Controller
    {
        private readonly IAccountRepository _account;
        private readonly IMapper mapper;
        private readonly ILogger<AccountController> _logger;

        public AccountController(IAccountRepository dish, IMapper mapper, ILogger<AccountController> logger)
        {
            this._account = dish;
            this.mapper = mapper;
            this._logger = logger;
        }

        [HttpGet()]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation(ReturnLogMessage("Account", "GetAll"));
            try
            {
                var result = await _account.GetAll();
                var resultDTO = mapper.Map<List<AccountDTO>>(result);
                return Ok(result);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            _logger.LogInformation(ReturnLogMessage("Account", "GetById"));
            var result = await _account.GetById(id);
            if (result == null)
            {
                var msg = $"The account with Id: \"{id}\" was not found!";
                _logger.LogWarning(msg);
                return NotFound(msg);
            }
            else
            {
                var resultDTO = mapper.Map<AccountDTO>(result);
                return Ok(resultDTO);
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccount(int id)
        {
            _logger.LogInformation(ReturnLogMessage("Account", "DeleteAccount"));
            try
            {
                await _account.Delete(id);
                return Ok("Deleted succesfully!");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
        }
        [HttpPost()]
        public async Task<IActionResult> AddAccount([FromBody] AccountRequestModel request)
        {
            _logger.LogInformation(ReturnLogMessage("Account", "AddAccount"));
            if (request.EmailAddress == "" || request.UserName == "" || request.UserPassword == "")
            {
                var msg = $"Fields: \"User name\", \"Password\" and \"e-mail\" are required!";
                _logger.LogWarning(msg);
                return BadRequest(msg);
            }
            try
            {
                var newAccount = new Account()
                {
                    UserName = request.UserName,
                    UserPassword = request.UserPassword,
                    EmailAddress = request.EmailAddress,
                    TelNumber = request.TelNumber = null,
                    Role = 1, // basic user!
                    RestaurantId = null,
                    IdUsers = null
                };
                await _account.Add(newAccount);
                return Ok("Succesfully added!");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return NotFound(ex.Message);
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAccount([FromBody] AccountAdminRequest request, int id)
        {
            _logger.LogInformation(ReturnLogMessage("Account", "UpdateAccount"));
            if (request.EmailAddress == "" || request.UserName == "" || request.UserPassword == "")
            {
                var msg = $"Fields: \"User name\", \"Password\" and \"e-mail\" are required!";
                _logger.LogWarning(msg);
                return BadRequest(msg);
            }
            try
            {
                var newAccount = new Account()
                {
                    UserName = request.UserName,
                    UserPassword = request.UserPassword,
                    EmailAddress = request.EmailAddress,
                    TelNumber = request.TelNumber = null,
                    Role = request.Role = 1,
                    RestaurantId = request.RestaurantId = null,
                    IdUsers = request.IdUsers = null
                };
                await _account.Add(newAccount);
                return Ok("Succesfully added!");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return NotFound(ex.Message);
            }
        }





        private string ReturnLogMessage(string controllerClassName, string nameMethod)
        {
            return $"Controller: {controllerClassName}Controller: Request: {nameMethod}()";
        }
    }
}
