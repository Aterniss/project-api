using Project_API.Models;
using Project_API.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_Tests.Repository_tests
{
    internal class ZoneRepositoryTests : FakeDatabase
    {
        ZoneRepository _zone;

        [OneTimeSetUp]
        public void Setup()
        {
            _context = new MyDbContext(dbContextOptions);
            _context.Database.EnsureCreated();

            SeedDatabase();

            _zone = new ZoneRepository(_context);
        }

        [Test, Order(1)]
        public void AddZone_WhenZoneNameExist_ThrowException()
        {
            var zone = new Zone()
            {
                ZoneName = "Already exist"
            };
            var result = _zone.AddZone(zone);
            Assert.That(() => result, Throws.Exception.With.Message.EqualTo($"Zone with zone name: \"{zone.ZoneName}\" already exist!"));
        }
        [Test, Order(2)]
        public void AddZone_WhenSuccesfullyAdded_Test()
        {
            var zone = new Zone()
            {
                ZoneName = "New Zone"
            };
            var result = _zone.AddZone(zone);
            Assert.DoesNotThrowAsync(() => result);
        }
        [Test, Order(3)]
        public void DeleteZone_IfZoneExistButIsInRelation_ThrowException()
        {
            var id = 1;
            var result = _zone.DeleteZone(id);
            Assert.That(() => result, Throws.Exception.With.Message.EqualTo($"You can not delete this Zone, because some restaurant has this zone asigned!"));
        }
        [Test, Order(4)]
        public void DeleteZone_IfZoneIdNotExist_ThrowException()
        {
            var id = 999;
            var result = _zone.DeleteZone(id);
            Assert.That(() => result, Throws.Exception.With.Message.EqualTo($"Zone with ID: \"{id}\" has not been found!"));
        }
        [Test, Order(5)]
        public void DeleteZone_WithoutException_Test()
        {
            var id = 5;
            var result = _zone.DeleteZone(id);
            Assert.DoesNotThrowAsync(() => result);
        }
        [Test, Order(6)]
        public void GetAll_WithoutException_Test()
        {
            var result = _zone.GetAll();
            Assert.DoesNotThrowAsync(() => result);
        }
        [Test, Order(7)]
        public void GetById_WhenZoneExist_ReturnZone()
        {
            int id = 1;
            var result = _zone.GetById(id);
            Assert.DoesNotThrowAsync(() => result);
            Assert.That(result.Result, Is.Not.Null);
        }
        [Test, Order(8)]
        public void GetById_WhenZoneNotExist_ReturnNull()
        {
            int id = 9999;
            var result = _zone.GetById(id);
            Assert.DoesNotThrowAsync(() => result);
            Assert.That(result.Result, Is.Null);
        }
        [Test, Order(9)]
        public void UpdateZone_WithoutException_Test()
        {
            int id = 3;
            var updateZone = new Zone()
            {
                ZoneId = 3,
                ZoneName = "SampleName"
            };
            var result = _zone.UpdateZone(updateZone, id);
            Assert.DoesNotThrowAsync(() => result);
        }
        [Test, Order(10)]
        public void UpdateZone_WhenZoneNameExist_ThrowException()
        {
            int id = 1;
            var updateZone = new Zone()
            {
                ZoneId = 1,
                ZoneName = "Update test"
            };
            var result = _zone.UpdateZone(updateZone, id);
            Assert.That(() => result, Throws.Exception.With.Message.EqualTo($"Zone with name: \"{updateZone.ZoneName}\" already exist!"));
        }
        [Test, Order(11)]
        public void UpdateZone_WhenZoneNotExist_ThrowException()
        {
            int id = 999;
            var updateZone = new Zone()
            {
                ZoneId = 999,
                ZoneName = "Update test"
            };
            var result = _zone.UpdateZone(updateZone, id);
            Assert.That(() => result, Throws.Exception.With.Message.EqualTo($"Zone with ID: \"{id}\" does not exist!"));
        }


        [OneTimeTearDown]
        public void CleanUp()
        {
            _context.Database.EnsureDeleted();
        }
    }
}


