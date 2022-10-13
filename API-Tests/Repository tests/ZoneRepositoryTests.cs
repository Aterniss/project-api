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













        [OneTimeTearDown]
        public void CleanUp()
        {
            _context.Database.EnsureDeleted();
        }
    }
}


