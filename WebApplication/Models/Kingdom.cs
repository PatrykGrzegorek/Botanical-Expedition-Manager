using System;
using System.Collections.Generic;

#nullable disable

namespace WebApplication.Models
{
    public partial class Kingdom
    {
        public Kingdom()
        {
            Divisions = new HashSet<Division>();
        }

        public int KingdomId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Division> Divisions { get; set; }
    }
}
