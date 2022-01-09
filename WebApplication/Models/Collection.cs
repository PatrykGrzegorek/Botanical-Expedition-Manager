using System;
using System.Collections.Generic;

#nullable disable

namespace WebApplication.Models
{
    public partial class Collection
    {
        public Collection()
        {
            Herbaria = new HashSet<Herbarium>();
        }

        public int CollectionId { get; set; }
        public string Name { get; set; }
        public int? MuseumId { get; set; }

        public virtual Museum Museum { get; set; }
        public virtual ICollection<Herbarium> Herbaria { get; set; }
    }
}
