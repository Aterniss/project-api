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
    internal class OrderRepositoryTests : FakeDatabase
    {
        OrderRepository _order;

        [OneTimeSetUp]
        public void Setup()
        {
            _context = new MyDbContext(dbContextOptions);
            _context.Database.EnsureCreated();

            SeedDatabase();

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





        [OneTimeTearDown]
        public void CleanUp()
        {
            _context.Database.EnsureDeleted();
        }
    }
}
