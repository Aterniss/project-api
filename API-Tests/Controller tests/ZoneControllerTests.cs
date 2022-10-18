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
    internal class ZoneControllerTests : FakeDatabase
    {
        private ZoneController _controller;
        private IMapper _mapper;
        private ZoneRepository repo;


        [OneTimeSetUp]
        public void Setup()
        {
            _context = new MyDbContext(dbContextOptions);
            _context.Database.EnsureCreated();

            repo = new ZoneRepository(_context);
            var profile = new ZoneProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(profile));
            _mapper = new Mapper(configuration);


            _controller = new ZoneController(repo, _mapper, new NullLogger<ZoneController>());
            SeedDatabase();
        }
        [Test, Order(1)]
        public void HTTPGET_GetAllZones_WithoutException_ReturnOk()
        {
            var result = _controller.GetAllZones();
            Assert.DoesNotThrowAsync(() => result);
            Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
            Assert.That(result, Is.Not.Null);
        }
        [Test, Order(2)]
        public void HTTPGET_GetById_WhenIdNotExist_ReturnNotFound()
        {
            int id = 9999;
            var result = _controller.GetById(id);
            Assert.DoesNotThrowAsync(() => result);
            Assert.That(result.Result, Is.TypeOf<NotFoundObjectResult>());
            var expectedMsg = $"Zone with ID: \"{id}\" has not been found!";
            var msg = (result.Result as NotFoundObjectResult).Value as string;
            Assert.That(msg, Is.EqualTo(expectedMsg));
        }
        [Test, Order(3)]
        public void HTTPGET_GetById_WithoutException_ReturnOk()
        {
            int id = 3;
            var result = _controller.GetById(id);
            Assert.DoesNotThrowAsync(() => result);
            Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
        }
        [Test, Order(4)]
        public void HTTPPOST_AddZone_WhenFieldIsEmpty_ReturnBadRequest()
        {
            var request = new ZoneRequestModel()
            {
                ZoneName = ""
            };

            var result = _controller.AddZone(request);
            Assert.DoesNotThrowAsync(() => result);
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());
            var expectedMsg = "This field is required!";
            var msg = (result.Result as BadRequestObjectResult).Value as string;
            Assert.That(msg, Is.EqualTo(expectedMsg));
        }
        [Test, Order(5)]
        public void HTTPPOST_AddZone_WithException_ReturnBadRequest()
        {
            var request = new ZoneRequestModel()
            {
                ZoneName = "zone name 3"
            };

            var result = _controller.AddZone(request);
            Assert.DoesNotThrowAsync(() => result);
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());
            var expectedMsg = $"Zone with zone name: \"{request.ZoneName}\" already exist!";
            var msg = (result.Result as BadRequestObjectResult).Value as string;
            Assert.That(msg, Is.EqualTo(expectedMsg));
        }
        [Test, Order(6)]
        public void HTTPPOST_AddZone_WithoutException_ReturnOk()
        {
            var request = new ZoneRequestModel()
            {
                ZoneName = "zone name 999"
            };

            var result = _controller.AddZone(request);
            Assert.DoesNotThrowAsync(() => result);
            Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
            var expectedMsg = "Succesfully added!";
            var msg = (result.Result as OkObjectResult).Value as string;
            Assert.That(msg, Is.EqualTo(expectedMsg));
        }
        [Test, Order(7)]
        public void HTTPPUT_UpdateZone_WhenFieldIsEmpty_ReturnBadRequest()
        {
            var request = new ZoneRequestModel()
            {
                ZoneName = ""
            };
            int id = 4;
            var result = _controller.UpdateZone(request,id);
            Assert.DoesNotThrowAsync(() => result);
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());
            var expectedMsg = "This field is required!";
            var msg = (result.Result as BadRequestObjectResult).Value as string;
            Assert.That(msg, Is.EqualTo(expectedMsg));
        }
        [Test, Order(8)]
        public void HTTPPUT_UpdateZone_WhenNameExist_ReturnBadRequest()
        {
            var request = new ZoneRequestModel()
            {
                ZoneName = "zone name 3"
            };
            int id = 4;
            var result = _controller.UpdateZone(request, id);
            Assert.DoesNotThrowAsync(() => result);
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());
            var expectedMsg = $"Zone with name: \"{request.ZoneName}\" already exist!";
            var msg = (result.Result as BadRequestObjectResult).Value as string;
            Assert.That(msg, Is.EqualTo(expectedMsg));
        }
        [Test, Order(9)]
        public void HTTPPUT_UpdateZone_WhenIdNotExist_ReturnBadRequest()
        {
            var request = new ZoneRequestModel()
            {
                ZoneName = "zone name 99999"
            };
            int id = 9999;
            var result = _controller.UpdateZone(request, id);
            Assert.DoesNotThrowAsync(() => result);
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());
            var expectedMsg = $"Zone with ID: \"{id}\" does not exist!";
            var msg = (result.Result as BadRequestObjectResult).Value as string;
            Assert.That(msg, Is.EqualTo(expectedMsg));
        }
        [Test, Order(10)]
        public void HTTPPUT_UpdateZone_WithoutException_ReturnOk()
        {
            var request = new ZoneRequestModel()
            {
                ZoneName = "zone name 99999"
            };
            int id = 4;
            var result = _controller.UpdateZone(request, id);
            Assert.DoesNotThrowAsync(() => result);
            Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
            var expectedMsg = "Succesfully updated!";
            var msg = (result.Result as OkObjectResult).Value as string;
            Assert.That(msg, Is.EqualTo(expectedMsg));
        }
        [Test, Order(11)]
        public void HTTPDELETE_DeleteZone_WhenIdNotExist_ReturnBadRequest()
        {
            int id = 99999;
            var result = _controller.DeleteZone(id);
            Assert.DoesNotThrowAsync(() => result);
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());
            var expectedMsg = $"Zone with ID: \"{id}\" has not been found!";
            var msg = (result.Result as BadRequestObjectResult).Value as string;
            Assert.That(msg, Is.EqualTo(expectedMsg));
        }
        [Test, Order(12)]
        public void HTTPDELETE_DeleteZone_WhenZoneIsAssigned_ReturnBadRequest()
        {
            int id = 1;
            var result = _controller.DeleteZone(id);
            Assert.DoesNotThrowAsync(() => result);
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());
            var expectedMsg = $"You can not delete this Zone, because some restaurant has this zone asigned!";
            var msg = (result.Result as BadRequestObjectResult).Value as string;
            Assert.That(msg, Is.EqualTo(expectedMsg));
        }
        [Test, Order(13)]
        public void HTTPDELETE_DeleteZone_WithoutException_ReturnBadRequest()
        { 
            int id = 4;
            var result = _controller.DeleteZone(id);
            Assert.DoesNotThrowAsync(() => result);
            Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
            var expectedMsg = "Succesfully deleted!";
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
