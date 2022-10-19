using Microsoft.EntityFrameworkCore;
using Project_API.DTO.RequestModels;
using Project_API.Models;
using Project_API.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_Tests.Repository_tests
{
    internal class OrderRepositoryTests
    {
        OrderRepository _order;
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

            _order = new OrderRepository(_context);
        }

        [Test, Order(1)]
        public void AddNewOrder_TestCheckMethodFirtstWay_ThrowCorrectException()
        {
            var order = new OrderRequestModel()
            {
                OrderStatus = "done",
                RiderId = 1,
                IdUser = 91, // does not exist!
                Dishes = new List<int>
                {
                    1,2
                }
               
            };
            var result = _order.AddNewOrder(order);
            Assert.That(() => result, Throws.Exception.With.Message.EqualTo($"User with ID: \"{order.IdUser}\" does not exist!"));
        }
        [Test, Order(2)]
        public void AddNewOrder_TestCheckMethodSecondWay_ThrowCorrectException()
        {
            var order = new OrderRequestModel()
            {
                OrderStatus = "done",
                RiderId = 99,   // does not exist!
                IdUser = 99,    // does not exist!
                Dishes = new List<int>
                {
                    1,2
                }

            };
            var result = _order.AddNewOrder(order);
            Assert.That(() => result, Throws.Exception.With.Message.EqualTo($"User with ID: \"{order.IdUser}\" does not exist!\n Rider with ID: \"{order.RiderId}\" does not exist!"));
        }
        [Test, Order(3)]
        public void AddNewOrder_TestCheckMethodThirdWay_ThrowCorrectException()
        {
            var order = new OrderRequestModel()
            {
                OrderStatus = "done",
                RiderId = 99,  
                IdUser = 1,    // does not exist!
                Dishes = new List<int>
                {
                    1,2
                }

            };
            var result = _order.AddNewOrder(order);
            Assert.That(() => result, Throws.Exception.With.Message.EqualTo($"Rider with ID: \"{order.RiderId}\" does not exist!"));
        }
        [Test, Order(4)]
        public void AddNewOrder_WhenDishesDoesNotExist_ThrowException()
        {
            var order = new OrderRequestModel()
            {
                OrderStatus = "done",
                RiderId = 1,
                IdUser = 1,    // does not exist!
                Dishes = new List<int>
                {
                    2, 99
                }

            };
            var result = _order.AddNewOrder(order);
            Assert.That(() => result, Throws.Exception.With.Message.EqualTo($"The dishes does not exist!"));
        }
        [Test, Order(5)]
        public void AddNewOrder_WithoutException_Test()
        {
            var order = new OrderRequestModel()
            {
                OrderStatus = "done",
                RiderId = 1,
                IdUser = 1,    // does not exist!
                Dishes = new List<int>
                {
                    1, 2
                }

            };
            var result = _order.AddNewOrder(order);
            Assert.DoesNotThrowAsync(() => result);
        }
        [Test, Order(6)]
        public void DeleteOrder_WhenOrderDoesNotExist_ThrowException()
        {
            int id = 999;
            var result = _order.DeleteOrder(id);
            Assert.That(() => result, Throws.Exception.With.Message.EqualTo($"Order with ID: \"{id}\" does not exist!"));
        }
        [Test, Order(7)]
        public void DeleteOrder_WithoutException_Test()
        {
            int id = 5;
            var result = _order.DeleteOrder(id);
            Assert.DoesNotThrowAsync(() => result);
        }
        [Test, Order(8)]
        public void GetAll_WhenCalled_NoException()
        {
            var result = _order.GetAll();
            Assert.DoesNotThrowAsync(() => result);
        }
        [Test, Order(9)]
        public void GetById_WhenOrderExist_ReturnOrder()
        {
            int id = 1;
            var result = _order.GetById(id);
            Assert.That(result.Result, Is.Not.Null);
            Assert.DoesNotThrowAsync(() => result);
        }
        [Test, Order(10)]
        public void GetById_WhenOrderDoesNotExist_ReturnNull()
        {
            int id = 99;
            var result = _order.GetById(id);
            Assert.That(result.Result, Is.EqualTo(null));
        }
        [Test, Order(11)]
        public void UpdateOrder_WhenOrderExist_NoException()
        {
            int id = 3;
            var order = new Order()
            {
                OrderStatus = "done",
                CreatedAt = DateTime.Now.AddDays(-1),
                RiderId = 1,
                IdUser = 1
            };
            var result = _order.UpdateOrder(order, id);
            Assert.DoesNotThrowAsync(() => result);
        }
        [Test, Order(12)]
        public void UpdateOrder_WhenIdIsWrong_ThrowException()
        {
            int id = 99;
            var order = new Order()
            {
                OrderStatus = "done",
                CreatedAt = DateTime.Now.AddDays(-1),
                RiderId = 1,
                IdUser = 1
            };
            var result = _order.UpdateOrder(order, id);
            Assert.That(() => result, Throws.Exception.With.Message.EqualTo($"Order with ID: \"{id}\" does not exist!"));
            
        }
        [Test, Order(13)]
        public void CheckUserIdAndRiderId_WhenCalled_TestCorrectException()
        {
            int correctUser = 1;
            int wrongUser = 999;
            int correctRider = 1;
            int wrongRider = 999;

            var result1 = _order.CheckUserIdAndRiderId(correctUser, correctRider);
            var result2 = _order.CheckUserIdAndRiderId(wrongUser, correctRider);
            var result3 = _order.CheckUserIdAndRiderId(wrongUser, wrongRider);
            var result4 = _order.CheckUserIdAndRiderId(correctUser, wrongRider);
            Assert.DoesNotThrowAsync(() => result1);
            Assert.That(() => result2, Throws.Exception.With.Message.EqualTo($"User with ID: \"{wrongUser}\" does not exist!"));
            Assert.That(() => result3, Throws.Exception.With.Message.EqualTo($"User with ID: \"{wrongUser}\" does not exist!\n Rider with ID: \"{wrongRider}\" does not exist!"));
            Assert.That(() => result4, Throws.Exception.With.Message.EqualTo($"Rider with ID: \"{wrongRider}\" does not exist!"));
        }








        [OneTimeTearDown]
        public void CleanUp()
        {
            _context.Database.EnsureDeleted();
        }
    }
}
