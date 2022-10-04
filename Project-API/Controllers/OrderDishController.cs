using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Project_API.DTO;
using Project_API.Repositories;

namespace Project_API.Controllers
{
    [ApiController]
    [Route("order-dish")]
    public class OrderDishController : Controller
    {
        private readonly IOrderDishRepository _orderDish;
        private readonly IMapper mapper;
        public OrderDishController(IOrderDishRepository orderDish, IMapper mapper)
        {
            this._orderDish = orderDish;
            this.mapper = mapper;
        }
      


    }
}
