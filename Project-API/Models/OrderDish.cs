using System;
using System.Collections.Generic;

namespace Project_API.Models
{
    public partial class OrderDish
    {
        public int OrderId { get; set; }
        public int DishId { get; set; }

        public virtual Dish Dish { get; set; } = null!;
        public virtual Order Order { get; set; } = null!;
    }
}
