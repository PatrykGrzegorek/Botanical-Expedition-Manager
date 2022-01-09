using System;
using System.Collections.Generic;

#nullable disable

namespace WebApplication.Models
{
    public partial class Family
    {
        public Family()
        {
            Genus = new HashSet<Genu>();
        }

        public int FamilyId { get; set; }
        public string Name { get; set; }
        public int OrderId { get; set; }

        public virtual Order Order { get; set; }
        public virtual ICollection<Genu> Genus { get; set; }
    }
}
