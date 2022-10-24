using System;
using System.Collections.Generic;

namespace Project_API.Models
{
    public partial class User
    {
        public User()
        {
            Accounts = new HashSet<Account>();
            Orders = new HashSet<Order>();
        }

        public int IdUser { get; set; }
        public string FullName { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime? LastUpdate { get; set; }
        public string UserAddress { get; set; } = null!;
        public bool? IsOver18 { get; set; }

        public virtual ICollection<Account> Accounts { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
