using System;
using System.Collections.Generic;

namespace Project_API.Models
{
    public partial class Zone
    {
        public Zone()
        {
            Restaurants = new HashSet<Restaurant>();
            Riders = new HashSet<Rider>();
        }

        public int ZoneId { get; set; }
        public string? ZoneName { get; set; }

        public virtual ICollection<Restaurant> Restaurants { get; set; }
        public virtual ICollection<Rider> Riders { get; set; }
    }
}
