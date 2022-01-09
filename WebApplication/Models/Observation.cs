using System;
using System.Collections.Generic;

#nullable disable

namespace WebApplication.Models
{
    public partial class Observation
    {
        public int ObservationId { get; set; }
        public int? GpslocalizationId { get; set; }
        public DateTime Date { get; set; }
        public int? HabitatId { get; set; }
        public int ExpeditionId { get; set; }
        public int? PersonId { get; set; }
        public int SpeciesId { get; set; }

        public virtual Expedition Expedition { get; set; }
        public virtual Gpslocalization Gpslocalization { get; set; }
        public virtual Habitat Habitat { get; set; }
        public virtual Person Person { get; set; }
        public virtual Species Species { get; set; }
    }
}
