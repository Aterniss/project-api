using Microsoft.EntityFrameworkCore;
using Project_API.Models;
using Project_API.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_Tests.Repository_tests
{
    internal class FoodCategoryRepositoryTests
    {
        FoodCategoryRepository _category;
        protected static DbContextOptions<MyDbContext> dbContextOptions = new DbContextOptionsBuilder<MyDbContext>()
            .UseInMemoryDatabase(databaseName: "API-Tests")
            .Options;

        protected MyDbContext _context;
        [OneTimeSetUp]
        public void Setup()
        {
            var db = new FakeDatabase();
            _context = new MyDbContext(dbContextOptions);
            _context.Database.EnsureCreated();

            db.SeedDatabase(_context);

            _category = new FoodCategoryRepository(_context);
        }

        [Test, Order(1)]
        public void AddAsync_WithoutException_Test()
        {
            var newCategory = new FoodCategory()
            {
                CategoryName = "category",
                CategoryDescription = "sample text..."
            };
            var result = _category.AddAsync(newCategory);

            Assert.DoesNotThrowAsync(() => result);
        }
        [Test, Order(8)]
        public void AddAsync_WhenCategoryNameExist_ThrowException()
        {
            var newCategory = new FoodCategory()
            {
                CategoryName = "already exist",
                CategoryDescription = "sample text..."
            };
            Assert.That(() => _category.AddAsync(newCategory), Throws.Exception);
        }
        [Test, Order(3)]
        public void DeleteAsync_WhenCategoryDoesNotExist_ThrowException()
        {

            var categoryName = "category name 51";

            Assert.That(() => _category.DeleteAsync(categoryName), Throws.Exception.With.Message.EqualTo($"Category: \"{categoryName}\" is not founded!"));
        }
        [Test, Order(2)]
        public void DeleteAsync_WhenCategoryExist_NoException()
        {
            var name = "to delete";
            var result = _category.DeleteAsync(name);
            Assert.DoesNotThrowAsync(() => result);

        }
        [Test, Order(5)]
        public void GetAllAsync_WithoutException_Test()
        {
            var result = _category.GetAllAsync();
            Assert.That(result, Is.Not.Null);
            Assert.DoesNotThrowAsync(() => result);
        }
        [Test, Order(6)]
        public void GetByNameAsync_WhenCategoryExist_ReturnResult()
        {
            var name = "category name 1";
            var result = _category.GetByNameAsync(name);
            Assert.That(result.Result, Is.Not.Null);
            Assert.DoesNotThrowAsync(() => result);
        }
        [Test, Order(7)]
        public void GetByNameAsync_WhenCategoryDoesNotExist_ThrowException()
        {
            var name = "does not exist!";
            var result = _category.GetByNameAsync(name);
            Assert.That(result.Result, Is.Null);
        }
        [Test, Order(4)]
        public void UpdateAsync_WhenCategoryExist_ReturnResult()
        {
            var name = "update";
            var updatedCategory = new FoodCategory()
            {
                CategoryName = name,
                CategoryDescription = "text..."
            };
            var result = _category.UpdateAsync(name, updatedCategory);
            Assert.That(result.Result, Is.Not.Null);
            Assert.DoesNotThrowAsync(() => result);
        }
        [Test, Order(9)]
        public void UpdateAsync_WhenCategoryDoesNotExist_ReturnNull()
        {
            var name = "does not exist!";
            var updatedCategory = new FoodCategory()
            {
                CategoryName = name,
                CategoryDescription = "text..."
            };
            var result = _category.UpdateAsync(name, updatedCategory);
            Assert.That(result.Result, Is.Null);
            Assert.DoesNotThrowAsync(() => result);
        }



        [OneTimeTearDown]
        public void CleanUp()
        {
            _context.Database.EnsureDeleted();
        }
    }
}
