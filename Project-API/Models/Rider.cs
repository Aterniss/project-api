using System;
using System.Collections.Generic;

namespace Project_API.Models
{
    public partial class Rider
    {
        public Rider()
        {
            Orders = new HashSet<Order>();
        }

        public int RiderId { get; set; }
        public string RiderName { get; set; } = null!;
        public int ZoneId { get; set; }

        public virtual Zone Zone { get; set; } = null!;
        public virtual ICollection<Order> Orders { get; set; }
    }
}
