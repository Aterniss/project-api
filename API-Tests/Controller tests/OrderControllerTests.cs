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
    internal class OrderControllerTests : FakeDatabase
    {
        private OrderController _order;
        private IMapper _mapper;
        OrderRepository repo;
        [OneTimeSetUp]
        public void Setup()
        {
            _context = new MyDbContext(dbContextOptions);
            _context.Database.EnsureCreated();

            repo = new OrderRepository(_context);
            var profile = new OrderProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(profile));
            _mapper = new Mapper(configuration);
            _order = new OrderController(repo, _mapper, new NullLogger<OrderController>());
            SeedDatabase();
        }
        [Test, Order(1)]
        public void HTTPGET_GetAll_WithoutException_ReturnOk()
        {
            var result = _order.GetAll();
            Assert.DoesNotThrowAsync(() => result);
            Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
        }
        [Test, Order(2)]
        public void HTTPGET_GetById_WithoutException_ReturnOk()
        {
            int id = 1;
            var result = _order.GetById(id);
            Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
            Assert.DoesNotThrowAsync(() => result);
            Assert.That(result.Result, Is.Not.Null);
        }
        [Test, Order(3)]
        public void HTTPGET_GetById_WhenIdDoesNotExist_ReturnNotFound()
        {
            int id = 999;
            var result = _order.GetById(id);
            Assert.That(result.Result, Is.TypeOf<NotFoundObjectResult>());
            Assert.DoesNotThrowAsync(() => result);
            //var expectedMsg = $"Order with ID: \"{id}\" does not exist!";
            //var msg = (result.Result as NotFoundObjectResult).Value as string;
            //Assert.That(msg, Is.EqualTo(expectedMsg));

        }
        [Test, Order(4)]
        public void HTTPPOST_AddNewOrder_WhenFieldsAreEmpty_ReturnBadRequest()
        {
            var request = new OrderRequestModel()
            {
                IdUser = 0,
                OrderStatus = "",
                RiderId = 0,
                Dishes = new List<int>() { 1, 2}
            };

            var result = _order.AddNewOrder(request);
            Assert.DoesNotThrowAsync(() => result);
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());
            var expectedMsg = "The given fields are required!";
            var msg = (result.Result as BadRequestObjectResult).Value as string;
            Assert.That(msg, Is.EqualTo(expectedMsg));
        }
        [Test, Order(5)]
        public void HTTPPOST_AddNewOrder_WhenUserIdIsWrong_ReturnBadRequest()
        {
            var request = new OrderRequestModel()
            {
                IdUser = 9999,
                OrderStatus = "done",
                RiderId = 1,
                Dishes = new List<int>() { 1, 2 }
            };

            var result = _order.AddNewOrder(request);
            Assert.DoesNotThrowAsync(() => result);
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());
            var expectedMsg = $"User with ID: \"{request.IdUser}\" does not exist!";
            var msg = (result.Result as BadRequestObjectResult).Value as string;
            Assert.That(msg, Is.EqualTo(expectedMsg));
        }
        [Test, Order(6)]
        public void HTTPPOST_AddNewOrder_WhenRiderIdIsWrong_ReturnBadRequest()
        {
            var request = new OrderRequestModel()
            {
                IdUser = 2,
                OrderStatus = "done",
                RiderId = 999,
                Dishes = new List<int>() { 1, 2 }
            };

            var result = _order.AddNewOrder(request);
            Assert.DoesNotThrowAsync(() => result);
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());
            var expectedMsg = $"Rider with ID: \"{request.RiderId}\" does not exist!";
            var msg = (result.Result as BadRequestObjectResult).Value as string;
            Assert.That(msg, Is.EqualTo(expectedMsg));
        }
        [Test, Order(7)]
        public void HTTPPOST_AddNewOrder_WhenRiderIdAndUserIdAreWrong_ReturnBadRequest()
        {
            var request = new OrderRequestModel()
            {
                IdUser = 999,
                OrderStatus = "done",
                RiderId = 999,
                Dishes = new List<int>() { 1, 2 }
            };

            var result = _order.AddNewOrder(request);
            Assert.DoesNotThrowAsync(() => result);
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());
            var expectedMsg = $"User with ID: \"{request.IdUser}\" does not exist!\n Rider with ID: \"{request.RiderId}\" does not exist!";
            var msg = (result.Result as BadRequestObjectResult).Value as string;
            Assert.That(msg, Is.EqualTo(expectedMsg));
        }
        [Test, Order(8)]
        public void HTTPPOST_AddNewOrder_WhenDishesDoesNotExist_ReturnBadRequest()
        {
            var request = new OrderRequestModel()
            {
                IdUser = 1,
                OrderStatus = "done",
                RiderId = 2,
                Dishes = new List<int>() { 99, 122 }
            };

            var result = _order.AddNewOrder(request);
            Assert.DoesNotThrowAsync(() => result);
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());
            var expectedMsg = $"The dishes does not exist!";
            var msg = (result.Result as BadRequestObjectResult).Value as string;
            Assert.That(msg, Is.EqualTo(expectedMsg));
        }
        [Test, Order(9)]
        public void HTTPPOST_AddNewOrder_WithoutException_ReturnOk()
        {
            var request = new OrderRequestModel()
            {
                IdUser = 2,
                OrderStatus = "done",
                RiderId = 1,
                Dishes = new List<int>() { 1, 2 }
            };

            var result = _order.AddNewOrder(request);
            Assert.DoesNotThrowAsync(() => result);
            Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
            var expectedMsg = "Succesfully added!";
            var msg = (result.Result as OkObjectResult).Value as string;
            Assert.That(msg, Is.EqualTo(expectedMsg));
        }
        [Test, Order(10)]
        public void HTTPDELETE_DeleteOrder_WhenIdDoesNotExist_ReturnBadRequest()
        {
            int id = 99999;
            var result = _order.DeleteOrder(id);
            Assert.DoesNotThrowAsync(() => result);
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());
            var expectedMsg = $"Order with ID: \"{id}\" does not exist!";
            var msg = (result.Result as BadRequestObjectResult).Value as string;
            Assert.That(msg, Is.EqualTo(expectedMsg));
        }
        [Test, Order(11)]
        public void HTTPDELETE_DeleteOrder_WithoutException_ReturnOk()
        {
            int id = 3;
            var result = _order.DeleteOrder(id);
            Assert.DoesNotThrowAsync(() => result);
            Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
            var expectedMsg = "Succesfully deleted!";
            var msg = (result.Result as OkObjectResult).Value as string;
            Assert.That(msg, Is.EqualTo(expectedMsg));
        }
        [Test, Order(12)]
        public void HTTPPUT_UpdateOrder_WhenFieldsAreEmpty_ReturnBadRequest()
        {
            int orderId = 4;
            var request = new OrderUpdateRequest()
            {
                IdUser = 2,
                OrderStatus = "",
                RiderId = 0
            };
            var result = _order.UpdateOrder(request, orderId);
            Assert.DoesNotThrowAsync(() => result);
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());
            var expectedMsg = "The given fields are required!";
            var msg = (result.Result as BadRequestObjectResult).Value as string;
            Assert.That(msg, Is.EqualTo(expectedMsg));
        }
        [Test, Order(12)]
        public void HTTPPUT_UpdateOrder_WhenOrderIdIsWrong_ReturnBadRequest()
        {
            int orderId = 999;
            var request = new OrderUpdateRequest()
            {
                IdUser = 2,
                OrderStatus = "done",
                RiderId = 1
            };
            var result = _order.UpdateOrder(request, orderId);
            Assert.DoesNotThrowAsync(() => result);
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());
            var expectedMsg = $"Order with ID: \"{orderId}\" does not exist!";
            var msg = (result.Result as BadRequestObjectResult).Value as string;
            Assert.That(msg, Is.EqualTo(expectedMsg));
        }
        [Test, Order(13)]
        public void HTTPPUT_UpdateOrder_WithoutException_ReturnOk()
        {
            int orderId = 5;
            var request = new OrderUpdateRequest()
            {
                IdUser = 2,
                OrderStatus = "done",
                RiderId = 1
            };
            var result = _order.UpdateOrder(request, orderId);
            Assert.DoesNotThrowAsync(() => result);
            Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
            var expectedMsg = "Succesfully updated!";
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
