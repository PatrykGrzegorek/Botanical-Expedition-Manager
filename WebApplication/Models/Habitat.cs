using System;
using System.Collections.Generic;

#nullable disable

namespace WebApplication.Models
{
    public partial class Habitat
    {
        public Habitat()
        {
            Observations = new HashSet<Observation>();
        }

        public int HabitatId { get; set; }
        public string Name { get; set; }
        public string NationalClassification { get; set; }

        public virtual ICollection<Observation> Observations { get; set; }
    }
}
