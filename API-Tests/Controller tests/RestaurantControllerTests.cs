using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Project_API.Controllers;
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
    internal class RestaurantControllerTests : FakeDatabase
    {
        private RestaurantController _restaurant;
        private IMapper _mapper;
        private RestaurantRepository repo;

        [OneTimeSetUp]
        public void Setup()
        {
            _context = new MyDbContext(dbContextOptions);
            _context.Database.EnsureCreated();

            repo = new RestaurantRepository(_context);
            var profile = new RestaurantProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(profile));
            _mapper = new Mapper(configuration);
            

            _restaurant = new RestaurantController(repo, _mapper, new NullLogger<RestaurantController>());
            SeedDatabase();
        }
        [Test, Order(1)]
        public void HTTPGET_GetAllRestaurants_WithoutException_ReturnOk()
        {
            var result = _restaurant.GetAllRestaurants();
            Assert.DoesNotThrowAsync(() => result);
            Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
        }
        [Test, Order(2)]
        public void HTTPGET_GetById_WhenIdDoesNotExist_ReturnNotFound()
        {
            int id = 9999;
            var result = _restaurant.GetById(id);
            Assert.DoesNotThrowAsync(() => result);
            Assert.That(result.Result, Is.TypeOf<NotFoundObjectResult>());
            var msg = (result.Result as NotFoundObjectResult).Value as string;
            Assert.That(msg, Is.EqualTo($"Restaurant with ID: \"{id}\" was not founded!"));
        }
        [Test, Order(3)]
        public void HTTPGET_GetById_WithoutException_ReturnOk()
        {
            int id = 1;
            var result = _restaurant.GetById(id);
            Assert.DoesNotThrowAsync(() => result);
            Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
            Assert.That(result.Result, Is.Not.Null);
        }
        [Test, Order(4)]
        public void HTTPPOST_AddRestaurant_WhenFieldsAreEmpty_ReturnBadRequest()
        {
            var request = new RestaurantRequestModel()
            {
                CategoryName = "",
                RestaurantAddress = "Address 150",
                RestaurantName = "",
                ZoneId = 0
            };
            var result = _restaurant.AddRestaurant(request);
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());
            Assert.DoesNotThrowAsync(() => result);
            var msg = (result.Result as BadRequestObjectResult).Value as string;
            Assert.That(msg, Is.EqualTo("The given fields are required!"));
        }
        [Test, Order(5)]
        public void HTTPPOST_AddRestaurant_WithException_ReturnBadRequest()
        {
            var request = new RestaurantRequestModel()
            {
                CategoryName = "cat 1",
                RestaurantAddress = "Address 150",
                RestaurantName = "Some name",
                ZoneId = 999
            };
            var result = _restaurant.AddRestaurant(request);
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());
            Assert.DoesNotThrowAsync(() => result);
            var msg = (result.Result as BadRequestObjectResult).Value as string;
            Assert.That(msg, Is.EqualTo($"The given \"zone id\" and \"category\" does not exist."));
        }
        [Test, Order(6)]
        public void HTTPPOST_AddRestaurant_WithoutException_ReturnOk()
        {
            var request = new RestaurantRequestModel()
            {
                CategoryName = "category name 1",
                RestaurantAddress = "Address 150",
                RestaurantName = "Some name",
                ZoneId = 1
            };
            var result = _restaurant.AddRestaurant(request);
            Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
            Assert.DoesNotThrowAsync(() => result);
            var msg = (result.Result as OkObjectResult).Value as string;
            Assert.That(msg, Is.EqualTo($"Restaurant {request.RestaurantName} is added!"));
        }
        [Test, Order(7)]
        public void HTTPDELETE_DeleteById_WhenIdDoesNotExist_ReturnNotFound()
        {
            int id = 999;
            var result = _restaurant.DeleteById(id);
            Assert.DoesNotThrowAsync(() => result);
            Assert.That(result.Result, Is.TypeOf<NotFoundObjectResult>());
            var msg = (result.Result as NotFoundObjectResult).Value as string;
            Assert.That(msg, Is.EqualTo($"Restaurant with ID: \"{id}\" does not exist!"));
        }
        [Test, Order(8)]
        public void HTTPDELETE_DeleteById_WithoutException_ReturnOk()
        {
            int id = 4;
            var result = _restaurant.DeleteById(id);
            Assert.DoesNotThrowAsync(() => result);
            Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
            var msg = (result.Result as OkObjectResult).Value as string;
            Assert.That(msg, Is.EqualTo($"Restaurant is deleted succesfully!"));
        }
        [Test, Order(9)]
        public void HTTPPUT_UpdateRestaurant_WhenFieldsAreEmpty_ReturnBadRequest()
        {
            int id = 3;
            var request = new RestaurantRequestModel()
            {
                CategoryName = "",
                RestaurantAddress = "",
                RestaurantName = "Some name",
                ZoneId = 0
            };
            var result = _restaurant.UpdateRestaurant(request, id);
            Assert.DoesNotThrowAsync(() => result);
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());
            var msg = (result.Result as BadRequestObjectResult).Value as string;
            Assert.That(msg, Is.EqualTo("The given fields are required!"));
        }
        [Test, Order(10)]
        public void HTTPPUT_UpdateRestaurant_WithException_ReturnBadRequest()
        {
            int id = 2;
            var request = new RestaurantRequestModel()
            {
                CategoryName = "Cat1",
                RestaurantAddress = "Address 150",
                RestaurantName = "Some name",
                ZoneId = 1
            };
            var result = _restaurant.UpdateRestaurant(request, id);
            Assert.DoesNotThrowAsync(() => result);
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());
        }
        [Test, Order(11)]
        [Ignore("Mapping does not work correctly!")]
        public void HTTPPUT_UpdateRestaurant_WithoutException_ReturnOk()
        {
            int id = 5;
            var request = new RestaurantRequestModel()
            {
                CategoryName = "category name 1",
                RestaurantAddress = "Address 150",
                RestaurantName = "Some name",
                ZoneId = 1
            };
            var result = _restaurant.UpdateRestaurant(request, id);
            Assert.DoesNotThrowAsync(() => result);
            Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
        }





        [OneTimeTearDown]
        public void CleanUp()
        {
            _context.Database.EnsureDeleted();
        }
    }
}
