using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Project_API.Controllers;
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
            var response = _order.GetAll();
            Assert.DoesNotThrowAsync(() => response);
            Assert.That(response, Is.Not.Null);
           // Assert.That(response.Result, Is.TypeOf<OkObjectResult>());
        }

        //rest I will do later







        [OneTimeTearDown]
        public void CleanUp()
        {
            _context.Database.EnsureDeleted();
        }
    }
}
