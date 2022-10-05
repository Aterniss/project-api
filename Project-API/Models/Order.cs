using System;
using System.Collections.Generic;

namespace Project_API.Models
{
    public partial class Order
    {
        public Order()
        {
            OrderDishes = new HashSet<OrderDish>();
        }

        public int OrderId { get; set; }
        public string OrderStatus { get; set; } = null!;
        public byte[] CreatedAt { get; set; } = null!;
        public int RiderId { get; set; }
        public int IdUser { get; set; }

        public virtual User IdUserNavigation { get; set; } = null!;
        public virtual Rider Rider { get; set; } = null!;
        public virtual ICollection<OrderDish> OrderDishes { get; set; }
    }
}
