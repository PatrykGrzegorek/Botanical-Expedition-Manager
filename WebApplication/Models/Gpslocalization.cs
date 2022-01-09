using System;
using System.Collections.Generic;

#nullable disable

namespace WebApplication.Models
{
    public partial class Gpslocalization
    {
        public Gpslocalization()
        {
            Observations = new HashSet<Observation>();
        }

        public int GpslocalizationId { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double MarginOfError { get; set; }

        public virtual ICollection<Observation> Observations { get; set; }
    }
}
