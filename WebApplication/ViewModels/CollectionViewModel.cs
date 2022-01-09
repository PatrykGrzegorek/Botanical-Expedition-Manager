using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Models;

namespace WebApplication.ViewModels
{
    public class CollectionViewModel
    {
        public IEnumerable<Collection> Collection { get; set; }
        public PagingInfo PagingInfo { get; set; }

    }
}
