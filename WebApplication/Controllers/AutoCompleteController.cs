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

        //public async Task<IEnumerable<IdLabel>> Herbarium(string term)
        //{
        //    var query = ctx.Herbarium
        //                    .Select(m => new IdLabel
        //                    {
        //                        Id = m.HerbariumId,
        //                        Label = m.InventoryNumber.ToString()
        //                    })
        //                    .Where(l => l.Label.Contains(term));

        //    var list = await query.OrderBy(l => l.Label)
        //                          .ThenBy(l => l.Id)
        //                          .Take(appData.AutoCompleteCount)
        //                          .ToListAsync();
        //    return list;
        //}

        //public async Task<IEnumerable<IdLabel>> Collection(string term)
        //{
        //    var query = ctx.Collection
        //                    .Select(p => new IdLabel
        //                    {
        //                        Id = p.CollectionId,
        //                        Label = p.Name
        //                    })
        //                    .Where(l => l.Label.Contains(term));

        //    var list = await query.OrderBy(l => l.Label)
        //                          .ThenBy(l => l.Id)
        //                          .Take(appData.AutoCompleteCount)
        //                          .ToListAsync();
        //    return list;
        //}
    }
}
