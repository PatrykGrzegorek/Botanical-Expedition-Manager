using System;
using System.Collections.Generic;

#nullable disable

namespace WebApplication.Models
{
    public partial class Person
    {
        public Person()
        {
            HerbariumPersonCollectors = new HashSet<Herbarium>();
            HerbariumPersonDetermines = new HashSet<Herbarium>();
            Observations = new HashSet<Observation>();
            References = new HashSet<Reference>();
        }

        public int PersonId { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Title { get; set; }

        public virtual ICollection<Herbarium> HerbariumPersonCollectors { get; set; }
        public virtual ICollection<Herbarium> HerbariumPersonDetermines { get; set; }
        public virtual ICollection<Observation> Observations { get; set; }
        public virtual ICollection<Reference> References { get; set; }
    }
}
