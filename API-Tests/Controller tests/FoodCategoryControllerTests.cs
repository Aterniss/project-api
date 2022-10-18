using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Project_API.Controllers;
using Project_API.DTO;
using Project_API.DTO.RequestModels;
using Project_API.Models;
using Project_API.Profiles;
using Project_API.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_Tests.Controller_tests
{
    internal class FoodCategoryControllerTests : FakeDatabase
    {
        private FoodCategoryController _foodCategory;
        private Mapper mapper;
        FoodCategoryRepository repo;

        [OneTimeSetUp]
        public void Setup()
        {
            _context = new MyDbContext(dbContextOptions);
            _context.Database.EnsureCreated();

            SeedDatabase();
            repo = new FoodCategoryRepository(_context);
            var config = new MapperConfiguration(cfg => {
                cfg.AddProfile<FoodCategoryProfile>();
            });
            mapper = new Mapper(config);
            _foodCategory = new FoodCategoryController(repo, mapper, new NullLogger<FoodCategoryController>());
        }
        [Test, Order(1)]
        public void HTTPGET_GetAllCategories_WithoutException_ReturnOk()
        {
            var response = _foodCategory.GetAllCategories();
            Assert.DoesNotThrowAsync(() => response);
            Assert.That(response.Result, Is.TypeOf<OkObjectResult>());
        }
        [Test, Order(6)]
        public void HTTPGET_GetByName_WhenCategoryDoesNotExist_ReturnNotFound()
        {
            string categoryName = "sample category!";
            var response = _foodCategory.GetByName(categoryName);
            Assert.DoesNotThrowAsync(() => response);
            Assert.That(response.Result, Is.TypeOf<NotFoundObjectResult>());
        }
        [Test, Order(9)]
        public void HTTPGET_GetByName_WhenCategorytExist_ReturnOk()
        {
            string categoryName = "category name 2";
            var response = _foodCategory.GetByName(categoryName);
            Assert.DoesNotThrowAsync(() => response);
            Assert.That(response.Result, Is.TypeOf<OkObjectResult>());

        }
        [Test, Order(12)]
        public void HTTPPOST_AddCategoryAsync_WhenFieldsAreEmpty_ReturnBadRequest()
        {
            var newCategory = new FoodCategoryDTO()
            {
                CategoryName = "",
                CategoryDescription = ""
            };
            var response = _foodCategory.AddCategoryAsync(newCategory);
            Assert.That(response.Result, Is.TypeOf<BadRequestObjectResult>());
            Assert.DoesNotThrowAsync(() => response);
            var msg = (response.Result as BadRequestObjectResult).Value as string;
            Assert.That(msg, Is.EqualTo("The given fields cannot be empty!"));
        }
        [Test, Order(5)]
        public void HTTPPOST_AddCategoryAsync_WhenCategoryExist_ReturnBadRequest()
        {
            var newCategory = new FoodCategoryDTO()
            {
                CategoryName = "category name 2",
                CategoryDescription = "sample text"
            };
            var response = _foodCategory.AddCategoryAsync(newCategory);
            Assert.That(response.Result, Is.TypeOf<BadRequestObjectResult>());
            Assert.DoesNotThrowAsync(() => response);
            var msg = (response.Result as BadRequestObjectResult).Value as string;
            Assert.That(msg, Is.EqualTo($"Category: \"{newCategory.CategoryName}\" already exist!"));
        }
        [Test, Order(2)]
        public void HTTPPOST_AddCategoryAsync_WithoutException_ReturnOk()
        {
            var newCategory = new FoodCategoryDTO()
            {
                CategoryName = "new category name 30",
                CategoryDescription = "sample text"
            };
            var response = _foodCategory.AddCategoryAsync(newCategory);
            Assert.That(response.Result, Is.TypeOf<OkObjectResult>());
            Assert.DoesNotThrowAsync(() => response);
            var msg = (response.Result as OkObjectResult).Value as string;
            Assert.That(msg, Is.EqualTo("Category added!"));
        }
        [Test, Order(7)]
        public void HTTPDELETE_DeleteCategoryAsync_WhenNameDoesNotExist_ReturnNotFound()
        {
            string name = "does not exist!";
            var result = _foodCategory.DeleteCategoryAsync(name);
            Assert.DoesNotThrowAsync(() => result);
            Assert.That(result.Result, Is.TypeOf<NotFoundObjectResult>());
            var msg = (result.Result as NotFoundObjectResult).Value as string;
            Assert.That(msg, Is.EqualTo($"Category: \"{name}\" is not founded!"));
        }
        [Test, Order(8)]
        public void HTTPDELETE_DeleteCategoryAsync_WhenRestaurantHasThisCategory_ReturnBadRequest()
        {
            string name = "category name 1";
            var result = _foodCategory.DeleteCategoryAsync(name);
            Assert.DoesNotThrowAsync(() => result);
            Assert.That(result.Result, Is.TypeOf<NotFoundObjectResult>());
            var msg = (result.Result as NotFoundObjectResult).Value as string;
            Assert.That(msg, Is.EqualTo($"You can not delete this \"Category name\", because some restaurant has this category asigned!"));
        }
        [Test, Order(3)]
        public void HTTPDELETE_DeleteCategoryAsync_WithoutException_ReturnOk()
        {
            string name = "to delete";
            var result = _foodCategory.DeleteCategoryAsync(name);
            Assert.DoesNotThrowAsync(() => result);
            Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
            var msg = (result.Result as OkObjectResult).Value as string;
            Assert.That(msg, Is.EqualTo($"Category: \"{name}\" has been deleted!"));
        }
        [Test, Order(10)]
        public void HTTPPUT_UpdateCategoryAsync_WhenFieldIsEmpty_ReturnBadRequest()
        {
            string name = "update";
            var updateCategory = new FoodCategoryRequestModel()
            {
                CategoryDescription = ""
            };

            var result = _foodCategory.UpdateCategoryAsync(updateCategory, name);
            Assert.DoesNotThrowAsync(() => result);
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());
            var msg = (result.Result as BadRequestObjectResult).Value as string;
            Assert.That(msg, Is.EqualTo("The given field cannot be empty!"));
        }
        [Test, Order(11)]
        public void HTTPPUT_UpdateCategoryAsync_WhenCategoryDoesNotExist_ReturnBadRequest()
        {
            string name = "does not exist!";
            var updateCategory = new FoodCategoryRequestModel()
            {
                CategoryDescription = "text"
            };

            var result = _foodCategory.UpdateCategoryAsync(updateCategory, name);
            Assert.DoesNotThrowAsync(() => result);
            Assert.That(result.Result, Is.TypeOf<NotFoundObjectResult>());
            var msg = (result.Result as NotFoundObjectResult).Value as string;
            Assert.That(msg, Is.EqualTo($"Category: \"{name}\" was not founded!"));
        }
        [Test, Order(4)]
        public void HTTPPUT_UpdateCategoryAsync_WithoutException_ReturnOk()
        {
            string name = "update me";
            var updateCategory = new FoodCategoryRequestModel()
            {
                CategoryDescription = "text"
            };

            var result = _foodCategory.UpdateCategoryAsync(updateCategory, name);
            Assert.DoesNotThrowAsync(() => result);
            Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
            var msg = (result.Result as OkObjectResult).Value as string;
            Assert.That(msg, Is.EqualTo($"Category: \"{name}\" has been updated"));
        }
        [OneTimeTearDown]
        public void CleanUp()
        {
            _context.Database.EnsureDeleted();
        }

    }
}
