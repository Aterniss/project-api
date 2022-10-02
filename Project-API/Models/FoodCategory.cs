using System;
using System.Collections.Generic;

namespace Project_API.Models
{
    public partial class FoodCategory
    {
        public FoodCategory()
        {
            Restaurants = new HashSet<Restaurant>();
        }

        public string CategoryName { get; set; } = null!;
        public string CategoryDescription { get; set; } = null!;

        public virtual ICollection<Restaurant> Restaurants { get; set; }
    }
}
