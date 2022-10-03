using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Project_API.Models
{
    public partial class Order
    {
        public int OrderId { get; set; }
        public string OrderStatus { get; set; } = null!;
        [Timestamp]
        public byte[] CreatedAt { get; set; } = null!;

        public int RiderId { get; set; }
        public int IdUser { get; set; }

        public virtual User IdUserNavigation { get; set; } = null!;
        public virtual Rider Rider { get; set; } = null!;
    }
}
