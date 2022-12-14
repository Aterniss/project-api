using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Project_API.DTO;
using Project_API.DTO.RequestModels;
using Project_API.Models;
using Project_API.Repositories;

namespace Project_API.Controllers
{
    [ApiController]
    [Route("food-category")]
    public class FoodCategoryController : Controller
    {
        private readonly IFoodCategoryRepository _foodCategory;
        private readonly IMapper mapper;
        private readonly ILogger<FoodCategoryController> _logger;
        public FoodCategoryController(IFoodCategoryRepository foodCategory, IMapper mapper, ILogger<FoodCategoryController> logger)
        {
            this._foodCategory = foodCategory;
            this.mapper = mapper;
            this._logger = logger;
        }

        [HttpGet()]
        public async Task<IActionResult> GetAllCategories()
        {
            try
            {
                _logger.LogInformation(ReturnLogMessage("FoodCategory", "GetAllCategories"));
                var categories = await _foodCategory.GetAllAsync();
                var categoriesDTO = mapper.Map<List<FoodCategoryDTO>>(categories);
                return Ok(categoriesDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest("Sorry, we could not load the categories!");
            }
        }
        [HttpGet("{name}")]
        public async Task<IActionResult> GetByName(string name)
        {
            try
            {
                _logger.LogInformation(ReturnLogMessage("FoodCategory", "GetByName"));
                var category = await _foodCategory.GetByNameAsync(name);
                var categoryDTO = mapper.Map<FoodCategoryDTO>(category);
                if (categoryDTO == null)
                {
                    var msg = $"Category with name: \"{name}\" is not founded!";
                    _logger.LogWarning(msg);
                    return NotFound(msg);
                }
                return Ok(categoryDTO);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest("Sorry, we could not load the category!");
            }

        }
        [HttpPost()]
        public async Task<IActionResult> AddCategoryAsync([FromBody] FoodCategoryDTO addNewCategory)
        {
            _logger.LogInformation(ReturnLogMessage("FoodCategory", "AddCategoryAsync"));
            if (addNewCategory.CategoryName == "" || addNewCategory.CategoryDescription == "")
            {
                var msg = "The given fields cannot be empty!";
                _logger.LogWarning(msg);
                return BadRequest(msg);
            }
            try
            {
                //Request DTO to domain model
                var category = new FoodCategory()
                {
                    CategoryName = addNewCategory.CategoryName,
                    CategoryDescription = addNewCategory.CategoryDescription
                };
                //pass details to repository
                await _foodCategory.AddAsync(category);
                return Ok("Category added!");
            }
            catch (Exception)
            {
                var msg = $"Category: \"{addNewCategory.CategoryName}\" already exist!";
                _logger.LogError(msg);
                return BadRequest(msg);
            }
        }
        [HttpDelete("{name}")]
        public async Task<IActionResult> DeleteCategoryAsync(string name)
        {
            _logger.LogInformation(ReturnLogMessage("FoodCategory", "DeleteCategoryAsync"));
            try
            {
                await _foodCategory.DeleteAsync(name);
                return Ok($"Category: \"{name}\" has been deleted!");
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return NotFound(ex.Message);
            }
        }
        [HttpPut("{name}")]
        public async Task<IActionResult> UpdateCategoryAsync([FromBody] FoodCategoryRequestModel newCategory, string name)
        {
            _logger.LogInformation(ReturnLogMessage("FoodCategory", "UpdateCategoryAsync"));
            if (newCategory.CategoryDescription == "")
            {
                var msg = "The given field cannot be empty!";
                _logger.LogError(msg);
                return BadRequest(msg);
            }

            var category = new FoodCategory()
            {
                CategoryName = name,
                CategoryDescription = newCategory.CategoryDescription
            };
            var response = await _foodCategory.UpdateAsync(name, category);
            if (response == null)
            {
                var msg = $"Category: \"{name}\" was not founded!";
                _logger.LogWarning(msg);
                return NotFound(msg);
            }
            return Ok($"Category: \"{name}\" has been updated");
        }
        private string ReturnLogMessage(string controllerClassName, string nameMethod)
        {
            return $"Controller: {controllerClassName}Controller: Request: {nameMethod}()";
        }

    }
}
