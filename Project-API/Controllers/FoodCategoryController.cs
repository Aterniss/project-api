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
        private readonly ILogger<DishController> _logger;
        public FoodCategoryController(IFoodCategoryRepository foodCategory, IMapper mapper, ILogger<DishController> logger)
        {
            this._foodCategory = foodCategory;
            this.mapper = mapper;
            this._logger = logger;
        }

        [HttpGet()]
        public async Task<IActionResult> GetAllCategories()
        {
            _logger.LogInformation(returnLogMessage("FoodCategory","GetAllCategories"));
            var categories = await _foodCategory.GetAllAsync();
            var categoriesDTO = mapper.Map<List<FoodCategoryDTO>>(categories);
            return Ok(categoriesDTO);
        }
        [HttpGet("{name}")]
        public async Task<IActionResult> GetByName(string name)
        {
            _logger.LogInformation(returnLogMessage("FoodCategory","GetByName"));
            var category = await _foodCategory.GetByNameAsync(name);
            var categoryDTO = mapper.Map<FoodCategoryDTO>(category);
            if (categoryDTO == null)
            {
                return NotFound($"Category with name: \"{name}\" is not founded!");
            }
            return Ok(categoryDTO);

        }
        [HttpPost()]
        public async Task<IActionResult> AddCategoryAsync([FromBody] FoodCategoryDTO addNewCategory)
        {
            _logger.LogInformation(returnLogMessage("FoodCategory", "AddCategoryAsync"));
            if (addNewCategory.CategoryName == "" || addNewCategory.CategoryDescription == "")
            {
                return BadRequest("The given fields cannot be empty!");
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
                return BadRequest($"Category: \"{addNewCategory.CategoryName}\" already exist!");
            }
        }
        [HttpDelete("{name}")]
        public async Task<IActionResult> DeleteCategoryAsync(string name)
        {
            _logger.LogInformation(returnLogMessage("FoodCategory", "DeleteCategoryAsync"));
            try
            {
                await _foodCategory.DeleteAsync(name);
                return Ok($"Category: \"{name}\" has been deleted!");
            }
            catch(Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
        [HttpPut("{name}")]
        public async Task<IActionResult> UpdateCategoryAsync([FromBody] FoodCategoryRequestModel newCategory, string name)
        {
            _logger.LogInformation(returnLogMessage("FoodCategory", "UpdateCategoryAsync"));
            if (newCategory.CategoryDescription == "")
            {
                return BadRequest("The given field cannot be empty!");
            }

            var category = new FoodCategory()
            {
                CategoryName = name,
                CategoryDescription = newCategory.CategoryDescription
            };
            var response = await _foodCategory.UpdateAsync(name, category);
            if (response == null)
            {
                return NotFound($"Category: \"{name}\" was not founded!");
            }
            return Ok($"Category: \"{name}\" has been updated");
        }
        private string returnLogMessage(string controllerClassName, string nameMethod)
        {
            return $"Controller: {controllerClassName}Controller: Request: {nameMethod}()";
        }

    }
}
