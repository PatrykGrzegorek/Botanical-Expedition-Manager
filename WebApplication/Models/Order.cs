using System;
using System.Collections.Generic;

#nullable disable

namespace WebApplication.Models
{
    public partial class Order
    {
        public Order()
        {
            Families = new HashSet<Family>();
        }

        public int OrderId { get; set; }
        public string Name { get; set; }
        public int ClassId { get; set; }

        public virtual Class Class { get; set; }
        public virtual ICollection<Family> Families { get; set; }
    }
}
