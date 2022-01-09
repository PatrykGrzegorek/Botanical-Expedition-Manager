using System;
using System.Collections.Generic;

#nullable disable

namespace WebApplication.Models
{
    public partial class Genu
    {
        public Genu()
        {
            Species = new HashSet<Species>();
        }

        public int GenusId { get; set; }
        public string Name { get; set; }
        public int FamilyId { get; set; }

        public virtual Family Family { get; set; }
        public virtual ICollection<Species> Species { get; set; }
    }
}
