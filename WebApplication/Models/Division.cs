using System;
using System.Collections.Generic;

#nullable disable

namespace WebApplication.Models
{
    public partial class Division
    {
        public Division()
        {
            Classes = new HashSet<Class>();
        }

        public int DivisionId { get; set; }
        public string Name { get; set; }
        public int KingdomId { get; set; }

        public virtual Kingdom Kingdom { get; set; }
        public virtual ICollection<Class> Classes { get; set; }
    }
}
