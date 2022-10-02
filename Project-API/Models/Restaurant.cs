using System;
using System.Collections.Generic;

namespace Project_API.Models
{
    public partial class Restaurant
    {
        public Restaurant()
        {
            Dishes = new HashSet<Dish>();
        }

        public int RestaurantId { get; set; }
        public string? RestaurantName { get; set; }
        public string? CategoryName { get; set; }
        public string? RestaurantAddress { get; set; }
        public int ZoneId { get; set; }

        public virtual FoodCategory? CategoryNameNavigation { get; set; }
        public virtual Zone Zone { get; set; } = null!;
        public virtual ICollection<Dish> Dishes { get; set; }
    }
}
