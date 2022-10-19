using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
    internal class RiderControllerTests
    {
        private RiderController _controller;
        private IMapper _mapper;
        private RiderRepository repo;
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

            repo = new RiderRepository(_context);
            var profile = new RiderProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(profile));
            _mapper = new Mapper(configuration);


            _controller = new RiderController(repo, _mapper, new NullLogger<RiderController>());
            database.SeedDatabase(_context);
        }
        [Test, Order(1)]
        public void HTTPGET_GetAllRiders_WithoutException_ReturnOk()
        {
            var result = _controller.GetAllRiders();
            Assert.DoesNotThrowAsync(() => result);
            Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
        }
        [Test, Order(2)]
        public void HTTPGET_GetById_WhenIdExist_ReturnOk()
        {
            int id = 1;
            var result = _controller.GetById(id);
            Assert.DoesNotThrowAsync(() => result);
            Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
            Assert.That(result.Id, Is.EqualTo(id));
        }
        [Test, Order(3)]
        public void HTTPGET_GetById_WhenIdDoesNotExist_ReturnNotFound()
        {
            int id = 999;
            var result = _controller.GetById(id);
            Assert.DoesNotThrowAsync(() => result);
            Assert.That(result.Result, Is.TypeOf<NotFoundObjectResult>());
        }
        [Test, Order(4)]
        public void HTTPPOST_AddRider_WhenFieldsAreEmpty_ReturnBadRequest()
        {
            var request = new RiderRequestModel()
            {
                RiderName = "",
                ZoneId = 0
            };
            var result = _controller.AddRider(request);
            Assert.DoesNotThrowAsync(() => result);
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());
            var expectedMessage = "The given fields are required!";
            var msg = (result.Result as BadRequestObjectResult).Value as string;
            Assert.That(msg, Is.EqualTo(expectedMessage));
        }
        [Test, Order(5)]
        public void HTTPPOST_AddRider_WithSpecificException_ReturnBadRequest()
        {
            var request = new RiderRequestModel()
            {
                RiderName = "Rider new 2",
                ZoneId = 999
            };
            var result = _controller.AddRider(request);
            Assert.DoesNotThrowAsync(() => result);
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());
            var expectedMessage = $"Zone with ID: \"{request.ZoneId}\" does not exist!";
            var msg = (result.Result as BadRequestObjectResult).Value as string;
            Assert.That(msg, Is.EqualTo(expectedMessage));
        }
        [Test, Order(6)]
        public void HTTPPOST_AddRider_WithoutException_ReturnOk()
        {
            var request = new RiderRequestModel()
            {
                RiderName = "Rider new 3",
                ZoneId = 1
            };
            var result = _controller.AddRider(request);
            Assert.DoesNotThrowAsync(() => result);
            Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
            var expectedMessage = "Succesfully added!";
            var msg = (result.Result as OkObjectResult).Value as string;
            Assert.That(msg, Is.EqualTo(expectedMessage));
        }
        [Test, Order(7)]
        public void HTTPDELETE_DeleteRiderById_WhenIdDoesNotExist_ReturnBadRequest()
        {
            int id = 9999;
            var result = _controller.DeleteRiderById(id);
            Assert.DoesNotThrowAsync(() => result);
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());
            var expectedMessage = $"Rider with ID: \"{id}\" has not been found!";
            var msg = (result.Result as BadRequestObjectResult).Value as string;
            Assert.That(msg, Is.EqualTo(expectedMessage));
        }
        [Test, Order(8)]
        public void HTTPDELETE_DeleteRiderById_WithoutException_ReturnOk()
        {
            int id = 5;
            var result = _controller.DeleteRiderById(id);
            Assert.DoesNotThrowAsync(() => result);
            Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
            var expectedMessage = "Succesfully deleted!";
            var msg = (result.Result as OkObjectResult).Value as string;
            Assert.That(msg, Is.EqualTo(expectedMessage));
        }
        [Test, Order(9)]
        public void HTTPPUT_UpdateRiderById_WhenFieldsAreEmpty_ReturnBadRequest()
        {
            int id = 6;
            var request = new RiderRequestModel()
            {
                RiderName = "",
                ZoneId = 1
            };
            var result = _controller.UpdateRiderById(request, id);
            Assert.DoesNotThrowAsync(() => result);
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());
            var expectedMessage = "The given fields are required!";
            var msg = (result.Result as BadRequestObjectResult).Value as string;
            Assert.That(msg, Is.EqualTo(expectedMessage));
        }
        [Test, Order(10)]
        public void HTTPPUT_UpdateRiderById_WhenZoneDoesNotExist_ReturnBadRequest()
        {
            int id = 6;
            var request = new RiderRequestModel()
            {
                RiderName = "Rider name",
                ZoneId = 999
            };
            var result = _controller.UpdateRiderById(request, id);
            Assert.DoesNotThrowAsync(() => result);
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());
            var expectedMessage = $"Zone with ID: \"{request.ZoneId}\" does not exist!";
            var msg = (result.Result as BadRequestObjectResult).Value as string;
            Assert.That(msg, Is.EqualTo(expectedMessage));
        }
        [Test, Order(11)]
        public void HTTPPUT_UpdateRiderById_WhenRiderDoesNotExist_ReturnBadRequest()
        {
            int id = 999;
            var request = new RiderRequestModel()
            {
                RiderName = "Rider name",
                ZoneId = 1
            };
            var result = _controller.UpdateRiderById(request, id);
            Assert.DoesNotThrowAsync(() => result);
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());
            var expectedMessage = $"Rider with ID: \"{id}\" does not exist!";
            var msg = (result.Result as BadRequestObjectResult).Value as string;
            Assert.That(msg, Is.EqualTo(expectedMessage));
        }
        [Test, Order(12)]
        public void HTTPPUT_UpdateRiderById_WithoutException_ReturnOk()
        {
            int id = 6;
            var request = new RiderRequestModel()
            {
                RiderName = "Rider name",
                ZoneId = 1
            };
            var result = _controller.UpdateRiderById(request, id);
            Assert.DoesNotThrowAsync(() => result);
            Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
            var expectedMessage = "Succesfully updated!";
            var msg = (result.Result as OkObjectResult).Value as string;
            Assert.That(msg, Is.EqualTo(expectedMessage));
        }

        [OneTimeTearDown]
        public void CleanUp()
        {
            _context.Database.EnsureDeleted();
        }
    }
}
