using Microsoft.EntityFrameworkCore;
using Project_API.Models;

namespace API_Tests
{
    public class Tests
    {
        private static DbContextOptions<MyDbContext> dbContextOptions = new DbContextOptionsBuilder<MyDbContext>()
            .UseInMemoryDatabase(databaseName: "API-Tests")
            .Options;

        MyDbContext _context;



        [OneTimeSetUp]
        public void Setup()
        {
            _context = new MyDbContext(dbContextOptions);
            _context.Database.EnsureCreated();

            SeedDatabase();
        }


        [OneTimeTearDown]
        public void CleanUp()
        {
            _context.Database.EnsureDeleted();
        }

        private void SeedDatabase()
        {
            var dishes = new List<Dish>
            {
                new Dish()
                {
                    DishId =1,
                    DishName = "Dish nr 1",
                    DishDescription = "Dish description 1",
                    RestaurantId = 1,
                    Price = 4.99M,
                    Require18 = true
                },
                new Dish()
                {
                    DishId =2,
                    DishName = "Dish nr 2",
                    DishDescription = "Dish description 2",
                    RestaurantId = 1,
                    Price = 5.99M,
                    Require18 = true
                },
                new Dish()
                {
                    DishId =3,
                    DishName = "Dish nr 3",
                    DishDescription = "Dish description 3",
                    RestaurantId = 2,
                    Price = 7.99M,
                    Require18 = false
                }
            };
            _context.Dishes.AddRange(dishes);

            var restaurants = new List<Restaurant>
            {
                new Restaurant()
                {
                    RestaurantId = 1,
                    RestaurantName = "Restaurant name 1",
                    CategoryName = "category name 1",
                    RestaurantAddress = "Address 1",
                    ZoneId = 1
                },
                new Restaurant()
                {
                    RestaurantId = 2,
                    RestaurantName = "Restaurant name 2",
                    CategoryName = "category name 1",
                    RestaurantAddress = "Address 2",
                    ZoneId = 1
                },
                new Restaurant()
                {
                    RestaurantId = 3,
                    RestaurantName = "Restaurant name 3",
                    CategoryName = "category name 2",
                    RestaurantAddress = "Address 3",
                    ZoneId = 2
                },
                new Restaurant()
                {
                    RestaurantId = 4,
                    RestaurantName = "Restaurant name 4",
                    CategoryName = "category name 2",
                    RestaurantAddress = "Address 4",
                    ZoneId = 3
                },


            };
            _context.Restaurants.AddRange(restaurants);

            var foodCategories = new List<FoodCategory>
            {
                new FoodCategory()
                {
                    CategoryName = "category name 1",
                    CategoryDescription = "description 1"
                },
                new FoodCategory()
                {
                    CategoryName = "category name 2",
                    CategoryDescription = "description 2"
                },
                new FoodCategory()
                {
                    CategoryName = "category name 3",
                    CategoryDescription = "description 3"
                },
            };
            _context.FoodCategories.AddRange(foodCategories);

            var zones = new List<Zone>
            {
                new Zone()
                {
                    ZoneId = 1,
                    ZoneName = "zone name 1"
                },
                new Zone()
                {
                    ZoneId = 2,
                    ZoneName = "zone name 2"
                },
                new Zone()
                {
                    ZoneId = 3,
                    ZoneName = "zone name 3"
                }
            };


        }
    }
}