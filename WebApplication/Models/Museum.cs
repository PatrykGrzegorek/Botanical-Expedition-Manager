using System;
using System.Collections.Generic;

#nullable disable

namespace WebApplication.Models
{
    public partial class Museum
    {
        public Museum()
        {
            Collections = new HashSet<Collection>();
        }

        public int MuseumId { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string StreetName { get; set; }
        public string StreetNumber { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Collection> Collections { get; set; }
    }
}
