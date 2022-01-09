using System;
using System.Collections.Generic;

#nullable disable

namespace WebApplication.Models
{
    public partial class Herbarium
    {
        public int HerbariumId { get; set; }
        public int CollectionId { get; set; }
        public DateTime? YearOfCollection { get; set; }
        public int InventoryNumber { get; set; }
        public int? PersonCollectorId { get; set; }
        public int? PersonDetermineId { get; set; }
        public int PiecesOfPlantsId { get; set; }
        public int SpiecesId { get; set; }

        public virtual Collection Collection { get; set; }
        public virtual Person PersonCollector { get; set; }
        public virtual Person PersonDetermine { get; set; }
        public virtual PartOfPlant PiecesOfPlants { get; set; }
        public virtual Species Spieces { get; set; }
    }
}
