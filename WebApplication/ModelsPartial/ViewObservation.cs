using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Models;

namespace WebApplication.Models
{
    
    public class ViewObservation
    {
        public ViewObservation(int observationId, DateTime date, int expeditionId, string expeditionName)
        {
            ObservationId = observationId;
            Date = date;
            ExpeditionId = expeditionId;
            ExpeditionName = expeditionName;
        }

        public int ObservationId { get; set; }
        public DateTime Date { get; set; }
        public int ExpeditionId { get; set; }

        public string ExpeditionName { get; set; }
    }
}
