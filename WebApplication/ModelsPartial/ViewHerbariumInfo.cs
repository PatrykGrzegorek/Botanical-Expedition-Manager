using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication.Models
{
  public class ViewHerbariumInfo
  {
        public int HerbariumId { get; set; }
        [NotMapped]
        public int CollectionId { get; set; }
        public DateTime? YearOfCollection { get; set; }
        public int InventoryNumber { get; set; }
        public int? PersonCollectorId { get; set; }
        [NotMapped]
        public int? PersonDetermineId { get; set; }
        [NotMapped]
        public int PiecesOfPlantsId { get; set; }
        [NotMapped]
        public int SpiecesId { get; set; }
  }
}
