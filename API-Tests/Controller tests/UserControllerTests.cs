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
    internal class UserControllerTests
    {
        private UserController _controller;
        private IMapper _mapper;
        private UserRepository repo;
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

            repo = new UserRepository(_context);
            var profile = new UserProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(profile));
            _mapper = new Mapper(configuration);


            _controller = new UserController(repo, _mapper, new NullLogger<UserController>());
            database.SeedDatabase(_context);
        }
        [Test, Order(1)]
        public void HTTPGET_GetAllUsers_WithoutException_ReturnOk()
        {
            var result = _controller.GetAllUsers();
            Assert.DoesNotThrowAsync(() => result);
            Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
            Assert.That(result, Is.Not.Null);
        }
        [Test, Order(2)]
        public void HTTPGET_GetById_WhenUserDoesNotExist_ReturnNotFound()
        {
            int id = 999;
            var result = _controller.GetById(id);
            Assert.DoesNotThrowAsync(() => result);
            Assert.That(result.Result, Is.TypeOf<NotFoundObjectResult>());
            var expectedMsg = $"User with ID: \"{id}\" has not been found!";
            var msg = (result.Result as NotFoundObjectResult).Value as string;
            Assert.That(msg, Is.EqualTo(expectedMsg));
        }
        [Test, Order(3)]
        public void HTTPGET_GetById_WithoutException_ReturnOk()
        {
            int id = 2;
            var result = _controller.GetById(id);
            Assert.DoesNotThrowAsync(() => result);
            Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
            Assert.That(result.Id, Is.EqualTo(id));
        }
        [Test, Order(4)]
        public void HTTPGET_GetByName_WithoutException_ReturnOk()
        {
            string name = "User name 3";
            var result = _controller.GetByName(name);
            Assert.DoesNotThrowAsync(() => result);
            Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
        }
        [Test, Order(5)]
        public void HTTPGET_GetByName_WhenNameDoesNotExist_ReturnBadRequest()
        {
            string name = "wrong name!!!!";
            var result = _controller.GetByName(name);
            Assert.DoesNotThrowAsync(() => result);
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());
            var expectedMsg = $"User with fullname: \"{name}\" has not been found!";
            var msg = (result.Result as BadRequestObjectResult).Value as string;
            Assert.That(msg, Is.EqualTo(expectedMsg));
        }
        [Test, Order(6)]
        public void HTTPPOST_AddUser_WhenFieldsAreEmpty_ReturnBadRequest()
        {
            var request = new UserRequestModel()
            {
                FullName = "",
                IsOver18 = false,
                UserAddress = ""
            };
            var result =_controller.AddUser(request);
            Assert.DoesNotThrowAsync(() => result);
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());
            var expectedMsg = $"The given fields: \"Your name\" and \"Your address\" are required!";
            var msg = (result.Result as BadRequestObjectResult).Value as string;
            Assert.That(msg, Is.EqualTo(expectedMsg));
        }
        [Test, Order(7)]
        public void HTTPPOST_AddUser_WithoutException_ReturnOk()
        {
            var request = new UserRequestModel()
            {
                FullName = "Full sample name",
                IsOver18 = false,
                UserAddress = "Address 155"
            };
            var result = _controller.AddUser(request);
            Assert.DoesNotThrowAsync(() => result);
            Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
            var expectedMsg = "Succesfully added!";
            var msg = (result.Result as OkObjectResult).Value as string;
            Assert.That(msg, Is.EqualTo(expectedMsg));
        }
        [Test, Order(8)]
        public void HTTPPUT_UpdateUser_WhenFieldsAreEmpty_ReturnBadRequest()
        {
            int id = 3;
            var request = new UserRequestModel()
            {
                FullName = "",
                IsOver18 = false,
                UserAddress = ""
            };
            var result = _controller.UpdateUser(request, id);
            Assert.DoesNotThrowAsync(() => result);
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());
            var expectedMsg = $"The given fields: \"Your name\" and \"Your address\" are required!";
            var msg = (result.Result as BadRequestObjectResult).Value as string;
            Assert.That(msg, Is.EqualTo(expectedMsg));
        }
        [Test, Order(9)]
        public void HTTPPUT_UpdateUser_WithoutException_ReturnOk()
        {
            int id = 2;
            var request = new UserRequestModel()
            {
                FullName = "Tony Stark",
                IsOver18 = false,
                UserAddress = "Text..."
            };
            var result = _controller.UpdateUser(request, id);
            Assert.DoesNotThrowAsync(() => result);
            Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
            var expectedMsg = "Succesfully updated!";
            var msg = (result.Result as OkObjectResult).Value as string;
            Assert.That(msg, Is.EqualTo(expectedMsg));
        }
        [Test, Order(10)]
        public void HTTPDELETE_DeteleUser_WhenIdDoesNotExist_ReturnBadRequest()
        {
            int id = 99999;
            var result = _controller.DeteleUser(id);
            Assert.DoesNotThrowAsync(() => result);
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());
            var expectedMsg = $"User with ID: \"{id}\" does not exist!";
            var msg = (result.Result as BadRequestObjectResult).Value as string;
            Assert.That(msg, Is.EqualTo(expectedMsg));
        }
        [Test, Order(11)]
        public void HTTPDELETE_DeteleUser_WithoutException_ReturnOk()
        {
            int id = 5;
            var result = _controller.DeteleUser(id);
            Assert.DoesNotThrowAsync(() => result);
            Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
            var expectedMsg = "Succesfully deleted!";
            var msg = (result.Result as OkObjectResult).Value as string;
            Assert.That(msg, Is.EqualTo(expectedMsg));
        }









        [OneTimeTearDown]
        public void CleanUp()
        {
            _context.Database.EnsureDeleted();
        }
    }
}
