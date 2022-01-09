using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Models;

namespace WebApplication.ViewModels
{
    public class HerbariumoViewModel
    {
        public int HerbariumId { get; set; }
        public int CollectionId { get; set; }
        public DateTime? YearOfCollection { get; set; }
        public int InventoryNumber { get; set; }
        public int? PersonCollectorId { get; set; }
        public int? PersonDetermineId { get; set; }
        public int PiecesOfPlantsId { get; set; }
        public int SpiecesId { get; set; }
    }
}
