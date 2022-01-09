using System;
using System.Collections.Generic;

#nullable disable

namespace WebApplication.Models
{
    public partial class Species
    {
        public Species()
        {
            Herbaria = new HashSet<Herbarium>();
            Observations = new HashSet<Observation>();
            References = new HashSet<Reference>();
        }

        public int Id { get; set; }
        public string LatinName { get; set; }
        public string FullName { get; set; }
        public int TaxonomicTree { get; set; }
        public string IsEndemic { get; set; }
        public string IsAutochthonous { get; set; }
        public string IsWeed { get; set; }
        public string IsInvasive { get; set; }
        public int? CommonNameId { get; set; }

        public virtual CommonName CommonName { get; set; }
        public virtual Genu TaxonomicTreeNavigation { get; set; }
        public virtual ICollection<Herbarium> Herbaria { get; set; }
        public virtual ICollection<Observation> Observations { get; set; }
        public virtual ICollection<Reference> References { get; set; }
    }
}
