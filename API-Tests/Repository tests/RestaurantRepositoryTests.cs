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
    public class RestaurantRepositoryTests
    {
        RestaurantRepository _restaurant;
        protected static DbContextOptions<MyDbContext> dbContextOptions = new DbContextOptionsBuilder<MyDbContext>()
            .UseInMemoryDatabase(databaseName: "API-Tests")
            .Options;

        protected MyDbContext _context;
        [OneTimeSetUp]
        public void Setup()
        {
            var database = new FakeDatabase();
            _context = new MyDbContext(dbContextOptions);
            _context.Database.EnsureCreated();

            database.SeedDatabase(_context);

            _restaurant = new RestaurantRepository(_context);
        }
        [Test, Order(1)]
        public void GetAll_ReturnSearchResult()
        {
            var result = _restaurant.GetAll();
            Assert.That(result, Is.Not.Null);
            Assert.DoesNotThrowAsync(() => result);
        }
        
        [Test, Order(5)]
        public void AddRestaurant_WithoutException_Test()
        {
            var newRestaurant = new Restaurant()
            {
                RestaurantName = "new name 10",
                CategoryName = "category name 1",
                RestaurantAddress = "Address121",
                ZoneId = 1
            };
            var result = _restaurant.AddRestaurant(newRestaurant);
            Assert.DoesNotThrowAsync(() => result);
        }
        [Test, Order(6)]
        public void AddRestaurant_WithException_ThrowException()
        {
            var newRestaurant = new Restaurant()  //wrong category name
            {
                RestaurantName = "new name 10",
                CategoryName = "wrong category!",
                RestaurantAddress = "Address121",
                ZoneId = 1
            };
            var newRestaurant2 = new Restaurant()   //wrong zone id
            {
                RestaurantName = "new name 10",
                CategoryName = "category name 1",
                RestaurantAddress = "Address1212",
                ZoneId = 99
            };
            var newRestaurant3 = new Restaurant()   //wrong category name and zone id
            {
                RestaurantName = "new name 10",
                CategoryName = "wrong category!",
                RestaurantAddress = "Address121",
                ZoneId = 99
            };

            Assert.That(() => _restaurant.AddRestaurant(newRestaurant), Throws.Exception.With.Message.EqualTo($"The given \"category\" does not exist."));
            Assert.That(() => _restaurant.AddRestaurant(newRestaurant2), Throws.Exception.With.Message.EqualTo($"The given \"zone id\" does not exist."));
            Assert.That(() => _restaurant.AddRestaurant(newRestaurant3), Throws.Exception.With.Message.EqualTo($"The given \"zone id\" and \"category\" does not exist."));

        }
        [Test, Order(7)]
        public void DeleteRestaurantById_WithException_Test()
        {
            var id = 99;
            Assert.That(() => _restaurant.DeleteRestaurantById(id), Throws.Exception.With.Message);
        }
        [Test, Order(8)]
        public void DeleteRestaurantById_WithoutException_Test()
        {
            var id =3;
            Assert.DoesNotThrowAsync(() => _restaurant.DeleteRestaurantById(id));
        }
        [Test, Order(9)]
        public async Task GetById_WhenIdDoesNotExist_ReturnNull()
        {
            var id = 91;
            var result = await _restaurant.GetById(id);
            Assert.That(result, Is.Null);
        }
        [Test, Order(10)]
        public async Task GetById_WhenIdExist_ReturnNull()
        {
            var id = 4;
            var result = await _restaurant.GetById(id);
            Assert.That(result, Is.Not.Null);
        }
        [Test, Order(11)]
        public void UpdateRestaurant_WhenRestaurantDoesNotExist_ThrowCorrectException()
        {
            var id = 88;
            var newRestaurant = new Restaurant()
            {
                RestaurantName = "New restaurant 50",
                CategoryName = "category name 50",
                RestaurantAddress = "sample address 150",
                ZoneId = 1
            };
            Assert.That(() => _restaurant.UpdateRestaurant(id, newRestaurant), Throws.Exception.With.Message.EqualTo($"Restaurant with ID: \"{id}\" does not exist!"));
        }
        [Test, Order(12)]
        public void UpdateRestaurant_WhenRestaurantExistButThrowExceptionThroughCheckMethod_Test()
        {
            var id = 1;
            var newRestaurant = new Restaurant()    //wrong category name
            {
                RestaurantName = "New restaurant 50",
                CategoryName = "wrong category name!",
                RestaurantAddress = "sample address 150",
                ZoneId = 1
            };
            var newRestaurant2 = new Restaurant()   //wrong zone id
            {
                RestaurantName = "New restaurant 50",
                CategoryName = "category name 1",
                RestaurantAddress = "sample address 150",
                ZoneId = 999
            };
            var newRestaurant3 = new Restaurant()// wrong category name and zone id
            {
                RestaurantName = "New restaurant 50",
                CategoryName = "category name 50",
                RestaurantAddress = "sample address 150",
                ZoneId = 531
            };
            Assert.That(() => _restaurant.UpdateRestaurant(id, newRestaurant), Throws.Exception.With.Message.EqualTo($"The given \"category\" does not exist."));
            Assert.That(() => _restaurant.UpdateRestaurant(id, newRestaurant2), Throws.Exception.With.Message.EqualTo($"The given \"zone id\" does not exist."));
            Assert.That(() => _restaurant.UpdateRestaurant(id, newRestaurant3), Throws.Exception.With.Message.EqualTo($"The given \"zone id\" and \"category\" does not exist."));
        }







        [OneTimeTearDown]
        public void CleanUp()
        {
            _context.Database.EnsureDeleted();
        }

    }
}
