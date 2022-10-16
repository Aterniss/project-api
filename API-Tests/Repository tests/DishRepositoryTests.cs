using Microsoft.EntityFrameworkCore;
using Project_API.Models;
using Project_API.Repositories;

namespace API_Tests.Repository_tests
{
    public class DishRepositoryTests : FakeDatabase
    {

        DishRepository _dishRepository;

        [OneTimeSetUp]
        public void Setup()
        {
            _context = new MyDbContext(dbContextOptions);
            _context.Database.EnsureCreated();

            SeedDatabase();

            _dishRepository = new DishRepository(_context);
        }

        [Test, Order(1)]
        public void GetAll_ReturnSearchResult()
        {
            var result = _dishRepository.GetAll();
            Assert.That(result, Is.Not.Null);
            Assert.DoesNotThrowAsync(() => result);
        }
        [Test, Order(2)]
        public async Task GetDishById_WhenIdExist_ReturnCorrectObject()
        { 
            var testDish = new Dish()
            {
                DishId = 1,
                DishName = "Dish nr 1",
                DishDescription = "Dish description 1",
                RestaurantId = 1,
                Price = 4.99M,
                Require18 = true
            };
            int dishTestId = 1;
            var result = await _dishRepository.GetDishById(dishTestId);

            Assert.That(result.DishName, Is.EqualTo(testDish.DishName));
            Assert.That(result.DishId, Is.EqualTo(testDish.DishId));
            Assert.That(result.DishId, Is.EqualTo(testDish.DishId));
            Assert.That(result.DishDescription, Is.EqualTo(testDish.DishDescription));
            Assert.That(result.RestaurantId, Is.EqualTo(testDish.RestaurantId));
            Assert.That(result.Price, Is.EqualTo(testDish.Price));
            Assert.That(result.Require18, Is.EqualTo(testDish.Require18));
        }

        [Test, Order(3)]
        public async Task GetDishById_WhenIdDoesNotExist_ReturnNull()
        {
            var dishTestId = 999;
            var result = await _dishRepository.GetDishById(dishTestId);
            Assert.That(result, Is.Null);
        }
        [Test, Order(4)]
        public async Task GetDishesByName_WhenNameExist_ReturnListOfObjects()
        {
            var dishName = "Dish nr 1";
            var result = await _dishRepository.GetDishesByName(dishName);
            Assert.That(result, Is.Not.Null);
        }
        [Test, Order(5)]
        public async Task GetDishesByName_WhenNameDoesNotExist_ReturnNull()
        {
            var dishName = "Sample dish name";
            var result = await _dishRepository.GetDishesByName(dishName);
            Assert.That(result, Is.Null);
        }
        [Test, Order(6)]
        public void AddNewDish_WithException_Test()
        {
            var testDish = new Dish()
            {
                DishName = "Dish nr 4",
                DishDescription = "Dish description 4",
                RestaurantId = 971,
                Price = 4.99M,
                Require18 = true
            };

            Assert.That(() => _dishRepository.AddNewDish(testDish), Throws.Exception);
        }
        [Test, Order(7)]
        public void AddNewDish_WithoutException_Test()
        {
            var testDish = new Dish()
            {
                DishName = "Dish nr 4",
                DishDescription = "Dish description 4",
                RestaurantId = 1,
                Price = 4.99M,
                Require18 = true
            };

            var result = _dishRepository.AddNewDish(testDish);
            Assert.DoesNotThrowAsync(() => result);
        }


        [Test, Order(8)]
        public void DeleteDishById_WithException_Test()
        {
            var dishId = 961;
            Assert.That(() => _dishRepository.DeleteDishById(dishId), Throws.Exception.With.Message.EqualTo($"The dish with ID: \"{dishId}\" does not exist!"));
        }

        [Test, Order(9)]
        public void DeleteDishById_WithoutException_Test()
        {
            var id = 1;
            var result = _dishRepository.DeleteDishById(id);
            Assert.DoesNotThrowAsync(() => result);
        }
        [Test, Order(10)]
        public void UpdateDishById_WhenDishIdDoesNotExist_ThrowCorrectException()
        {
            int id = 92;
            var testDish = new Dish()
            {
                DishName = "Dish nr 10",
                DishDescription = "Dish description 10",
                RestaurantId = 2,
                Price = 4.99M,
                Require18 = true
            };
            Assert.That(() => _dishRepository.UpdateDishById(testDish, id), Throws.Exception.With.Message.EqualTo($"The dish with ID: \"{id}\" does not exist!"));
        }
        [Test, Order(11)]
        public void UpdateDishById_WhenRestaurantIdDoesNotExist_ThrowCorrectException()
        {
            int id = 2;
            var testDish = new Dish()
            {
                DishName = "Dish nr 10",
                DishDescription = "Dish description 10",
                RestaurantId = 92,
                Price = 4.99M,
                Require18 = true
            };
            Assert.That(() => _dishRepository.UpdateDishById(testDish, id), Throws.Exception.With.Message.EqualTo($"The given restaurant ID: \"{testDish.RestaurantId}\" does not exist!"));
        }
        [Test, Order(12)]
        public void UpdateDishById_WithoutException_Test()
        {
            int id = 2;
            var testDish = new Dish()
            {
                DishName = "Dish nr 10",
                DishDescription = "Dish description 10",
                RestaurantId = 2,
                Price = 4.99M,
                Require18 = true
            };
            var result = _dishRepository.UpdateDishById(testDish, id);
            Assert.DoesNotThrowAsync(() => result);
        }



        [OneTimeTearDown]
        public void CleanUp()
        {
            _context.Database.EnsureDeleted();
        }

        //private void SeedDatabase()
        //{
        //    var dishes = new List<Dish>
        //    {
        //        new Dish()
        //        {
        //            DishId =1,
        //            DishName = "Dish nr 1",
        //            DishDescription = "Dish description 1",
        //            RestaurantId = 1,
        //            Price = 4.99M,
        //            Require18 = true
        //        },
        //        new Dish()
        //        {
        //            DishId =2,
        //            DishName = "Dish nr 2",
        //            DishDescription = "Dish description 2",
        //            RestaurantId = 1,
        //            Price = 5.99M,
        //            Require18 = true
        //        },
        //        new Dish()
        //        {
        //            DishId =3,
        //            DishName = "Dish nr 3",
        //            DishDescription = "Dish description 3",
        //            RestaurantId = 2,
        //            Price = 7.99M,
        //            Require18 = false
        //        }
        //    };
        //    _context.Dishes.AddRange(dishes);

        //    var restaurants = new List<Restaurant>
        //    {
        //        new Restaurant()
        //        {
        //            RestaurantId = 1,
        //            RestaurantName = "Restaurant name 1",
        //            CategoryName = "category name 1",
        //            RestaurantAddress = "Address 1",
        //            ZoneId = 1
        //        },
        //        new Restaurant()
        //        {
        //            RestaurantId = 2,
        //            RestaurantName = "Restaurant name 2",
        //            CategoryName = "category name 1",
        //            RestaurantAddress = "Address 2",
        //            ZoneId = 1
        //        },
        //        new Restaurant()
        //        {
        //            RestaurantId = 3,
        //            RestaurantName = "Restaurant name 3",
        //            CategoryName = "category name 2",
        //            RestaurantAddress = "Address 3",
        //            ZoneId = 2
        //        },
        //        new Restaurant()
        //        {
        //            RestaurantId = 4,
        //            RestaurantName = "Restaurant name 4",
        //            CategoryName = "category name 2",
        //            RestaurantAddress = "Address 4",
        //            ZoneId = 3
        //        },


        //    };
        //    _context.Restaurants.AddRange(restaurants);

        //    var foodCategories = new List<FoodCategory>
        //    {
        //        new FoodCategory()
        //        {
        //            CategoryName = "category name 1",
        //            CategoryDescription = "description 1"
        //        },
        //        new FoodCategory()
        //        {
        //            CategoryName = "category name 2",
        //            CategoryDescription = "description 2"
        //        },
        //        new FoodCategory()
        //        {
        //            CategoryName = "category name 3",
        //            CategoryDescription = "description 3"
        //        },
        //    };
        //    _context.FoodCategories.AddRange(foodCategories);

        //    var zones = new List<Zone>
        //    {
        //        new Zone()
        //        {
        //            ZoneId = 1,
        //            ZoneName = "zone name 1"
        //        },
        //        new Zone()
        //        {
        //            ZoneId = 2,
        //            ZoneName = "zone name 2"
        //        },
        //        new Zone()
        //        {
        //            ZoneId = 3,
        //            ZoneName = "zone name 3"
        //        }
        //    };
        //    _context.Zones.AddRange(zones);

        //    var riders = new List<Rider>
        //    {
        //        new Rider()
        //        {
        //            RiderId = 1,
        //            RiderName = "rider name 1",
        //            ZoneId = 1
        //        },
        //        new Rider()
        //        {
        //            RiderId = 2,
        //            RiderName = "rider name 2",
        //            ZoneId = 2
        //        },
        //        new Rider()
        //        {
        //            RiderId = 3,
        //            RiderName = "rider name 3",
        //            ZoneId = 2
        //        },
        //        new Rider()
        //        {
        //            RiderId = 4,
        //            RiderName = "rider name 4",
        //            ZoneId = 3
        //        }
        //    };
        //    _context.Riders.AddRange(riders);

        //    var orders = new List<Order>
        //    {
        //        new Order()
        //        {
        //            OrderId = 1,
        //            OrderStatus = "status 1",
        //            CreatedAt = DateTime.Now.AddDays(-15),
        //            RiderId = 1,
        //            IdUser = 1
        //        },
        //        new Order()
        //        {
        //            OrderId = 2,
        //            OrderStatus = "status 1",
        //            CreatedAt = DateTime.Now.AddDays(-12),
        //            RiderId = 1,
        //            IdUser = 2
        //        },
        //        new Order()
        //        {
        //            OrderId = 3,
        //            OrderStatus = "status 2",
        //            CreatedAt = DateTime.Now.AddDays(-11),
        //            RiderId = 2,
        //            IdUser = 2
        //        },
        //        new Order()
        //        {
        //            OrderId = 4,
        //            OrderStatus = "status 2",
        //            CreatedAt = DateTime.Now.AddDays(-6),
        //            RiderId = 3,
        //            IdUser = 3
        //        },
        //        new Order()
        //        {
        //            OrderId = 5,
        //            OrderStatus = "status 3",
        //            CreatedAt = DateTime.Now.AddDays(-5),
        //            RiderId = 2,
        //            IdUser = 1
        //        }
        //    };
        //    _context.Orders.AddRange(orders);

        //    var users = new List<User>
        //    {
        //        new User()
        //        {
        //            IdUser = 1,
        //            FullName = "User name 1",
        //            CreatedAt = DateTime.Now.AddDays(-10),
        //            LastUpdate = DateTime.Now.AddDays(-3),
        //            UserAddress = "Address 123",
        //            IsOver18 = false
        //        },
        //        new User()
        //        {
        //            IdUser = 2,
        //            FullName = "User name 2",
        //            CreatedAt = DateTime.Now.AddDays(-9),
        //            LastUpdate = DateTime.Now.AddDays(-4),
        //            UserAddress = "Address 121",
        //            IsOver18 = true
        //        },
        //        new User()
        //        {
        //            IdUser = 3,
        //            FullName = "User name 3",
        //            CreatedAt = DateTime.Now.AddDays(-10),
        //            LastUpdate = DateTime.Now.AddDays(-3),
        //            UserAddress = "Address 150",
        //            IsOver18 = true
        //        },
        //        new User()
        //        {
        //            IdUser = 4,
        //            FullName = "User name 4",
        //            CreatedAt = DateTime.Now.AddDays(-6),
        //            LastUpdate = DateTime.Now.AddDays(-2),
        //            UserAddress = "Address 173",
        //            IsOver18 = false
        //        }
        //    };
        //    _context.Users.AddRange(users);

        //    var orderDishes = new List<OrderDish>
        //    {
        //        new OrderDish()
        //        {
        //            Id = 1,
        //            OrderId = 1,
        //            DishId = 1
        //        },
        //        new OrderDish()
        //        {
        //            Id = 2,
        //            OrderId = 1,
        //            DishId = 3
        //        },
        //        new OrderDish()
        //        {
        //            Id = 3,
        //            OrderId = 2,
        //            DishId = 3
        //        },
        //        new OrderDish()
        //        {
        //            Id = 4,
        //            OrderId = 3,
        //            DishId = 1
        //        },
        //        new OrderDish()
        //        {
        //            Id = 5,
        //            OrderId = 3,
        //            DishId = 3
        //        },
        //        new OrderDish()
        //        {
        //            Id = 6,
        //            OrderId = 2,
        //            DishId = 2
        //        }
        //    };
        //    _context.OrderDishes.AddRange(orderDishes);

        //    _context.SaveChanges();

        //}

    }
}