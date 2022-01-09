using System;
using System.Collections.Generic;

#nullable disable

namespace WebApplication.Models
{
    public partial class Class
    {
        public Class()
        {
            Orders = new HashSet<Order>();
        }

        public int ClassId { get; set; }
        public string Name { get; set; }
        public int DivisionId { get; set; }

        public virtual Division Division { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
