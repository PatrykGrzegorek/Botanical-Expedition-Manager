using System;
using System.Collections.Generic;

#nullable disable

namespace WebApplication.Models
{
    public partial class TypeOfReference
    {
        public TypeOfReference()
        {
            References = new HashSet<Reference>();
        }

        public int TypeOfReferenceId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Reference> References { get; set; }
    }
}
