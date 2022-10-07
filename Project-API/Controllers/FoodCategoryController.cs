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

        public FoodCategoryController(IFoodCategoryRepository foodCategory, IMapper mapper)
        {
            this._foodCategory = foodCategory;
            this.mapper = mapper;
        }

        [HttpGet()]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await _foodCategory.GetAllAsync();
            var categoriesDTO = mapper.Map<List<FoodCategoryDTO>>(categories);
            return Ok(categoriesDTO);
        }
        [HttpGet("{name}")]
        public async Task<IActionResult> GetByName(string name)
        {
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
            if(addNewCategory.CategoryName == "" || addNewCategory.CategoryDescription == "")
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

            var response = await _foodCategory.DeleteAsync(name);
            if (response == null)
            {
                return NotFound($"Category: \"{name}\" is not founded!");
            }
            return Ok($"Category: \"{name}\" has been deleted!");


        }
        [HttpPut("{name}")]
        public async Task<IActionResult> UpdateCategoryAsync([FromBody] FoodCategoryRequestModel newCategory, string name)
        {
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


    }
}
