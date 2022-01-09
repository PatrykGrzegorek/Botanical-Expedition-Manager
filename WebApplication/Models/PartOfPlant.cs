using System;
using System.Collections.Generic;

#nullable disable

namespace WebApplication.Models
{
    public partial class PartOfPlant
    {
        public PartOfPlant()
        {
            Herbaria = new HashSet<Herbarium>();
        }

        public int PlantId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Herbarium> Herbaria { get; set; }
    }
}
