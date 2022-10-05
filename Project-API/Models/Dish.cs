using System;
using System.Collections.Generic;

namespace Project_API.Models
{
    public partial class Dish
    {
        public Dish()
        {
            OrderDishes = new HashSet<OrderDish>();
        }

        public int DishId { get; set; }
        public string? DishName { get; set; }
        public string DishDescription { get; set; } = null!;
        public int RestaurantId { get; set; }
        public decimal Price { get; set; }
        public bool? Require18 { get; set; }

        public virtual Restaurant Restaurant { get; set; } = null!;
        public virtual ICollection<OrderDish> OrderDishes { get; set; }
    }
}
