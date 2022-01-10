using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Models;

namespace WebApplication.ViewModels
{
    public class ObservationViewModel
    {
        public IEnumerable<ViewObservation> Observation { get; set; }
        public PagingInfo PagingInfo { get; set; }

    }
}
