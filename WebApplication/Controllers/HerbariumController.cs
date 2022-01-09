using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using WebApplication.Extensions;
using WebApplication.Extensions.Selectors;
using WebApplication.Models;
using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using WebApplication.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApplication.Controllers
{
    public class HerbariumController : Controller
    {
        private readonly RPPP19Context ctx;
        private readonly AppSettings appData;
        public HerbariumController(RPPP19Context ctx,
        IOptionsSnapshot<AppSettings> options)
        {
            this.ctx = ctx;
            appData = options.Value;
        }

        public async Task<IActionResult> Index(string filter, int page = 1, int sort = 1, bool ascending = true)
        {
            int pagesize = appData.PageSize;
            var query = ctx.Herbaria.AsNoTracking();

            //#region Apply filter
            //HerbariumFilter df = HerbariumFilter.FromString(filter);
            //if (!df.IsEmpty())
            //{
            //    if (df.CollectionId.HasValue)
            //    {
            //        df.CollectionName = await ctx.vw_Collection
            //                                  .Where(p => p.CollectionId == df.CollectionId)
            //                                  .Select(vp => vp.InventoryNumber.ToString())
            //                                  .FirstOrDefaultAsync();
            //    }
            //    query = df.Apply(query);
            //}
            //#endregion

            int count = await query.CountAsync();

            var pagingInfo = new PagingInfo
            {
                CurrentPage = page,
                Sort = sort,
                Ascending = ascending,
                ItemsPerPage = pagesize,
                TotalItems = count
            };
            if (count > 0 && (page < 1 || page > pagingInfo.TotalPages))
            {
                return RedirectToAction(nameof(Index), new { page = 1, sort, ascending, filter });
            }

            query = query.ApplySort(sort, ascending);
            var herbarium = await query
                            .Skip((page - 1) * pagesize)
                            .Take(pagesize)
                            .ToListAsync();

            //var model = new HerbariumViewModel
            //{
            //    Herbarium = herbarium,
            //    PagingInfo = pagingInfo,
            //    Filter = df
            //};
            //return View(model);
            return View();
        }

        [HttpPost]
        public IActionResult Filter(HerbariumFilter filter)
        {
            return RedirectToAction(nameof(Index), new { filter = filter.ToString() });
        }

        //public async Task<IActionResult> Show(int id, int? position, string filter, int page = 1, int sort = 1, bool ascending = true, string viewName = nameof(Show))
        //{
        //    var Herbarium = await ctx.Herbarium
        //                            .Where(d => d.IdDokumenta == id)
        //                            .Select(d => new HerbariumViewModel
        //                            {
        //                                BrDokumenta = d.BrDokumenta,
        //                                DatDokumenta = d.DatDokumenta,
        //                                IdDokumenta = d.IdDokumenta,
        //                                IdPartnera = d.IdPartnera,
        //                                IdPrethDokumenta = d.IdPrethDokumenta,
        //                                IznosDokumenta = d.IznosDokumenta,
        //                                PostoPorez = d.PostoPorez,
        //                                VrDokumenta = d.VrDokumenta
        //                            })
        //                            .FirstOrDefaultAsync();
        //    if (dokument == null)
        //    {
        //        return NotFound($"Dokument {id} ne postoji");
        //    }
        //    else
        //    {
        //        dokument.NazPartnera = await ctx.vw_Partner
        //                                        .Where(p => p.IdPartnera == dokument.IdPartnera)
        //                                        .Select(p => p.Naziv)
        //                                        .FirstOrDefaultAsync();

        //        if (dokument.IdPrethDokumenta.HasValue)
        //        {
        //            dokument.NazPrethodnogDokumenta = await ctx.vw_Dokumenti
        //                                                       .Where(d => d.IdDokumenta == dokument.IdPrethDokumenta)
        //                                                       .Select(d => d.IdDokumenta + " " + d.NazPartnera + " " + d.IznosDokumenta)
        //                                                       .FirstOrDefaultAsync();
        //        }
        //        //učitavanje stavki
        //        var stavke = await ctx.Stavka
        //                              .Where(s => s.IdDokumenta == dokument.IdDokumenta)
        //                              .OrderBy(s => s.IdStavke)
        //                              .Select(s => new StavkaViewModel
        //                              {
        //                                  IdStavke = s.IdStavke,
        //                                  JedCijArtikla = s.JedCijArtikla,
        //                                  KolArtikla = s.KolArtikla,
        //                                  NazArtikla = s.SifArtiklaNavigation.NazArtikla,
        //                                  PostoRabat = s.PostoRabat,
        //                                  SifArtikla = s.SifArtikla
        //                              })
        //                              .ToListAsync();
        //        dokument.Stavke = stavke;

        //        if (position.HasValue)
        //        {
        //            page = 1 + position.Value / appData.PageSize;
        //            await SetPreviousAndNext(position.Value, filter, sort, ascending);
        //        }

        //        ViewBag.Page = page;
        //        ViewBag.Sort = sort;
        //        ViewBag.Ascending = ascending;
        //        ViewBag.Filter = filter;
        //        ViewBag.Position = position;

        //        return View(viewName, dokument);
        //    }
        //}

        //private async Task SetPreviousAndNext(int position, string filter, int sort, bool ascending)
        //{
        //    var query = ctx.vw_Dokumenti.AsQueryable();

        //    DokumentFilter df = new DokumentFilter();
        //    if (!string.IsNullOrWhiteSpace(filter))
        //    {
        //        df = DokumentFilter.FromString(filter);
        //        if (!df.IsEmpty())
        //        {
        //            query = df.Apply(query);
        //        }
        //    }

        //    query = query.ApplySort(sort, ascending);
        //    if (position > 0)
        //    {
        //        ViewBag.Previous = await query.Skip(position - 1).Select(d => d.IdDokumenta).FirstAsync();
        //    }
        //    if (position < await query.CountAsync() - 1)
        //    {
        //        ViewBag.Next = await query.Skip(position + 1).Select(d => d.IdDokumenta).FirstAsync();
        //    }
        //}

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await PrepareDropDownLists();
            return View();
        }
        private async Task PrepareDropDownLists()
        {
            var collection = await ctx.Collections.OrderBy(d => d.Name)
            .Select(d => new { d.Name, d.CollectionId })
            .ToListAsync();
            ViewBag.collection = new SelectList(collection,
            "CollectionId", "Name");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Herbarium herbarium)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    ctx.Add(herbarium);
                    ctx.SaveChanges();
                    TempData[Constants.Message] =
                    $"Herbarium {herbarium.InventoryNumber} is added.";
                    TempData[Constants.ErrorOccurred] = false;
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception exc)
                {
                    ModelState.AddModelError(string.Empty,
                    exc.CompleteExceptionMessage());
                    return View(herbarium);
                }
            }
            else
                return View(herbarium);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int IdHerbarium, int page = 1, int sort = 1, bool ascending = true)
        {
            var herbarium = ctx.Herbaria.Find(IdHerbarium);
            if (herbarium != null)
            {
                try
                {
                    int name = herbarium.InventoryNumber;
                    ctx.Remove(herbarium);
                    ctx.SaveChanges();
                    TempData[Constants.Message] = $"Herbarium {name} was deleted";
                    TempData[Constants.ErrorOccurred] = false;
                }
                catch (Exception exc)
                {
                    TempData[Constants.Message] = "Error removing herbarium: " + exc.CompleteExceptionMessage();
                    TempData[Constants.ErrorOccurred] = true;
                }
            }
            else
            {
                TempData[Constants.Message] = "There is no herbarium with the id: " + IdHerbarium;
                TempData[Constants.ErrorOccurred] = true;
            }
            return RedirectToAction(nameof(Index), new { page = page, sort = sort, ascending = ascending });
        }

        [HttpGet]
        public IActionResult Edit(int id, int page = 1, int sort = 1, bool ascending = true)
        {
            var herbarium = ctx.Herbaria.AsNoTracking()
            .Where(d => d.HerbariumId == id)
            .SingleOrDefault();
            if (herbarium == null)
                return NotFound("There is no herbarium with id: " + id);
            else
            {
                ViewBag.Page = page; ViewBag.Sort = sort;
                ViewBag.Ascending = ascending;
                return View(herbarium);
            }
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, int page = 1, int sort = 1, bool ascending = true)
        {

            try
            {
                Herbarium herbarium = await ctx.Herbaria
                                  .Where(d => d.HerbariumId == id)
                                  .FirstOrDefaultAsync();
                if (herbarium == null)
                {
                    return NotFound("Invalid herbarium id: " + id);
                }

                if (await TryUpdateModelAsync<Herbarium>(herbarium, "",
                    d => d.InventoryNumber
                ))
                {
                    ViewBag.Page = page;
                    ViewBag.Sort = sort;
                    ViewBag.Ascending = ascending;
                    try
                    {
                        await ctx.SaveChangesAsync();
                        TempData[Constants.Message] = "Herbarium updated.";
                        TempData[Constants.ErrorOccurred] = false;
                        return RedirectToAction(nameof(Index), new { page = page, sort = sort, ascending = ascending });
                    }
                    catch (Exception exc)
                    {
                        ModelState.AddModelError(string.Empty, exc.CompleteExceptionMessage());
                        return View(herbarium);
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Herbarium data cannot be linked to a form.");
                    return View(herbarium);
                }
            }
            catch (Exception exc)
            {
                TempData[Constants.Message] = exc.CompleteExceptionMessage();
                TempData[Constants.ErrorOccurred] = true;
                return RedirectToAction(nameof(Edit), id);
            }
        }
    }
}
