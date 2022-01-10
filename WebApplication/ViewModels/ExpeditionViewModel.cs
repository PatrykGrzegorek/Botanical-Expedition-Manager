using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Models;

namespace WebApplication.ViewModels
{
    public class ExpeditionViewModel
    {
        public IEnumerable<Expedition> Expedition { get; set; }
        public PagingInfo PagingInfo { get; set; }

    }
}
