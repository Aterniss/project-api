using Project_API.Models;
using Project_API.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_Tests.Repository_tests
{
    internal class RiderRepositoryTests : FakeDatabase
    {
        RiderRepository _rider;

        [OneTimeSetUp]
        public void Setup()
        {
            _context = new MyDbContext(dbContextOptions);
            _context.Database.EnsureCreated();

            SeedDatabase();

            _rider = new RiderRepository(_context);
        }
        [Test, Order(1)]
        public void AddRider_WhenZoneExist_NoExceptions()
        {
            var newRider = new Rider()
            {
                RiderName = "new rider name",
                ZoneId = 1
            };
            var result = _rider.AddRider(newRider);
            Assert.DoesNotThrowAsync(() => result);
        }
        [Test, Order(2)]
        public void AddRider_WhenZoneDoesNotExist_ThrowException()
        {
            var newRider = new Rider()
            {
                RiderName = "new rider name2",
                ZoneId = 9999
            };
            var result = _rider.AddRider(newRider);
            Assert.That(() => result, Throws.Exception.With.Message.EqualTo($"Zone with ID: \"{newRider.ZoneId}\" does not exist!"));
        }
        [Test, Order(3)]
        public void DeleteRider_WhenRiderDoesNotExist_ThrowException()
        {
            int id = 999;
            var result = _rider.DeleteRider(id);
            Assert.That(() => result, Throws.Exception.With.Message.EqualTo($"Rider with ID: \"{id}\" has not been found!"));
        }
        [Test, Order(4)]
        public void DeleteRider_WhenRiderExist_NoException()
        {
            int id = 999;
            var result = _rider.DeleteRider(id);
            Assert.That(() => result, Throws.Exception.With.Message.EqualTo($"Rider with ID: \"{id}\" has not been found!"));
        }
        [Test, Order(5)]
        public void GetAll_WithoutException_Test()
        {
            var result = _rider.GetAll();
            Assert.That(result.Result, Is.Not.Null);
            Assert.DoesNotThrowAsync(() => result);
        }
        [Test, Order(6)]
        public void GetById_WhenRiderExist_ReturnRider()
        {
            int id = 1;
            var result = _rider.GetById(id);
            Assert.That(result.Result, Is.Not.Null);
            Assert.DoesNotThrowAsync(() => result);
        }
        [Test, Order(7)]
        public void GetById_WhenRiderDoesNotExist_ReturnNull()
        {
            int id = 999;
            var result = _rider.GetById(id);
            Assert.That(result.Result, Is.Null);
            Assert.DoesNotThrowAsync(() => result);
        }
        [Test, Order(8)]
        public void UpdateRider_WhenRiderDoesNotExist_ThrowException()
        {
            int id = 99;
            var rider = new Rider()
            {
                RiderName = "Test rider",
                ZoneId = 1
            };
            var result = _rider.UpdateRider(rider, id);
            Assert.That(() => result, Throws.Exception.With.Message.EqualTo($"Rider with ID: \"{id}\" does not exist!"));
        }
        [Test, Order(9)]
        public void UpdateRider_WhenZoneNotExist_ThrowException()
        {
            int id = 2;
            var rider = new Rider()
            {
                RiderName = "Test rider",
                ZoneId = 999
            };
            var result = _rider.UpdateRider(rider, id);
            Assert.That(() => result, Throws.Exception.With.Message.EqualTo($"Zone with ID: \"{rider.ZoneId}\" does not exist!"));
        }
        [Test, Order(10)]
        public void UpdateRider_WithoutException_Test()
        {
            int id = 6;
            var rider = new Rider()
            {
                RiderName = "Test rider",
                ZoneId = 1
            };
            var result = _rider.UpdateRider(rider, id);
            Assert.DoesNotThrowAsync(() => result);
        }








        [OneTimeTearDown]
        public void CleanUp()
        {
            _context.Database.EnsureDeleted();
        }
    }
}
