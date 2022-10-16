using AutoMapper;
using Project_API.Controllers;
using Project_API.Models;
using Project_API.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_Tests.Controller_tests
{
    internal class DishControllerTests : FakeDatabase
    {
        DishRepository _dishRepository;
        DishController dishController;
        IMapper mapper;

        [OneTimeSetUp]
        public void Setup()
        {
            _context = new MyDbContext(dbContextOptions);
            _context.Database.EnsureCreated();

            SeedDatabase();

            _dishRepository = new DishRepository(_context);
            dishController = new DishController(_dishRepository, mapper);
        }





















        [OneTimeTearDown]
        public void CleanUp()
        {
            _context.Database.EnsureDeleted();
        }


    }
}
