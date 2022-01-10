using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Models;

namespace WebApplication.ViewModels
{
    public class PersonViewModel
    {
        public IEnumerable<Person> Person { get; set; }
        public PagingInfo PagingInfo { get; set; }

    }
}
