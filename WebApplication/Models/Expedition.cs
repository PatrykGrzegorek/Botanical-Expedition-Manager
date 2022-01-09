using System;
using System.Collections.Generic;

#nullable disable

namespace WebApplication.Models
{
    public partial class Expedition
    {
        public Expedition()
        {
            Observations = new HashSet<Observation>();
        }

        public int ExpeditionId { get; set; }
        public string Name { get; set; }
        public string Discription { get; set; }

        public virtual ICollection<Observation> Observations { get; set; }
    }
}
