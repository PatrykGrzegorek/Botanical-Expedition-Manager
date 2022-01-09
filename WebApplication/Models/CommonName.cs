using System;
using System.Collections.Generic;

#nullable disable

namespace WebApplication.Models
{
    public partial class CommonName
    {
        public CommonName()
        {
            Species = new HashSet<Species>();
        }

        public int CommonNameId { get; set; }
        public int ReferenceId { get; set; }
        public string Name { get; set; }

        public virtual Reference Reference { get; set; }
        public virtual ICollection<Species> Species { get; set; }
    }
}
