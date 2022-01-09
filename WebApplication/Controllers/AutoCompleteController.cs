using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using WebApplication.Models;
using WebApplication.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Controllers
{
    public class AutoCompleteController : Controller
    {
        private readonly RPPP19Context ctx;
        private readonly AppSettings appData;

        public AutoCompleteController(RPPP19Context ctx, IOptionsSnapshot<AppSettings> options)
        {
            this.ctx = ctx;
            appData = options.Value;
        }
    }
}
