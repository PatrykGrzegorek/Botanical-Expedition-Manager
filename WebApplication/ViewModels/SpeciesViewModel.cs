using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Models;

namespace WebApplication.ViewModels
{
    public class SpeciesViewModel
    {
        public IEnumerable<ViewSpecies> Species { get; set; }
        public PagingInfo PagingInfo { get; set; }

    }
}
