using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Models;

namespace WebApplication.ViewModels
{
    public class HerbariumViewModel
    {
        public IEnumerable<ViewHerbariumInfo> Herbarium { get; set; }
        public PagingInfo PagingInfo { get; set; }

        public HerbariumFilter Filter { get; set; }

    }
}
