using System;
using System.Collections.Generic;

#nullable disable

namespace WebApplication.Models
{
    public partial class Reference
    {
        public Reference()
        {
            CommonNames = new HashSet<CommonName>();
        }

        public int ReferenceId { get; set; }
        public int SpeciesId { get; set; }
        public int TypeOfReferenceId { get; set; }
        public int? AuthorId { get; set; }
        public string Title { get; set; }
        public DateTime? Year { get; set; }
        public string Description { get; set; }

        public virtual Person Author { get; set; }
        public virtual Species Species { get; set; }
        public virtual TypeOfReference TypeOfReference { get; set; }
        public virtual ICollection<CommonName> CommonNames { get; set; }
    }
}
