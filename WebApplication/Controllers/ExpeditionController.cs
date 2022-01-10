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

namespace WebApplication.Controllers
{
    public class ExpeditionController : Controller
    {
        private readonly RPPP19Context ctx;
        private readonly AppSettings appData;
        public ExpeditionController(RPPP19Context ctx,
        IOptionsSnapshot<AppSettings> options)
        {
            this.ctx = ctx;
            appData = options.Value;
        }

        public IActionResult Index(int page = 1, int sort = 1,
        bool ascending = true)
        {
            int pagesize = appData.PageSize;
            var query = ctx.Expeditions
            .AsNoTracking();
            int count = query.Count();
            if (count == 0)
            {
                TempData[Constants.Message] = "No data in the database";
                TempData[Constants.ErrorOccurred] = false;
                return RedirectToAction(nameof(Create));
            }

            var pagingInfo = new PagingInfo
            {
                CurrentPage = page,
                Sort = sort,
                Ascending = ascending,
                ItemsPerPage = pagesize,
                TotalItems = count
            };
            if (page < 1 || page > pagingInfo.TotalPages)
            {
                return RedirectToAction(nameof(Index),
                new { page = pagingInfo.TotalPages, sort, ascending });
            }

            query = query.ApplySort(sort, ascending);            var expedition = query
                            .Skip((page - 1) * pagesize)
                            .Take(pagesize)
                            .ToList();
            var model = new ExpeditionViewModel
            {
                Expedition = expedition,
                PagingInfo = pagingInfo
            };
            return View(model);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Expedition expedition)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    ctx.Add(expedition);
                    ctx.SaveChanges();
                    TempData[Constants.Message] =
                    $"Expedition {expedition.Name} is added.";
                    TempData[Constants.ErrorOccurred] = false;
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception exc)
                {
                    ModelState.AddModelError(string.Empty,
                    exc.CompleteExceptionMessage());
                    return View(expedition);
                }
            }
            else
                return View(expedition);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int IdExpedition, int page = 1, int sort = 1, bool ascending = true)
        {
            var expedition = await ctx.Expeditions.FindAsync(IdExpedition);
            
            if (expedition != null)
            {
                try
                {
                    string name = expedition.Name;
                    ctx.Remove(expedition);
                    ctx.SaveChanges();
                    TempData[Constants.Message] = $"Expedition {name} was deleted";
                    TempData[Constants.ErrorOccurred] = false;
                }
                catch (Exception exc)
                {
                    TempData[Constants.Message] = "Error removing expedition: " + exc.CompleteExceptionMessage();
                    TempData[Constants.ErrorOccurred] = true;
                }
            }
            else
            {
                TempData[Constants.Message] = "There is no expedition with the id: " + IdExpedition.ToString();
                TempData[Constants.ErrorOccurred] = true;
            }
            return RedirectToAction(nameof(Index), new { page = page, sort = sort, ascending = ascending });
        }

        [HttpGet]
        public IActionResult Edit(int id, int page = 1, int sort = 1, bool ascending = true)
        {
            var expedition = ctx.Expeditions.AsNoTracking()
            .Where(d => d.ExpeditionId == id)
            .SingleOrDefault();
            if (expedition == null)
                return NotFound("There is no expedition with id: " + id);
            else
            {
                ViewBag.Page = page; ViewBag.Sort = sort;
                ViewBag.Ascending = ascending;
                return View(expedition);
            }
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, int page = 1, int sort = 1, bool ascending = true)
        {

            try
            {
                Expedition expedition = await ctx.Expeditions
                                  .Where(d => d.ExpeditionId == id)
                                  .FirstOrDefaultAsync();
                if (expedition == null)
                {
                    return NotFound("Invalid expedition id: " + id);
                }

                if (await TryUpdateModelAsync<Expedition>(expedition, "",
                    d => d.Name, d => d.Discription
                ))
                {
                    ViewBag.Page = page;
                    ViewBag.Sort = sort;
                    ViewBag.Ascending = ascending;
                    try
                    {
                        await ctx.SaveChangesAsync();
                        TempData[Constants.Message] = "Expedition updated.";
                        TempData[Constants.ErrorOccurred] = false;
                        return RedirectToAction(nameof(Index), new { page = page, sort = sort, ascending = ascending });
                    }
                    catch (Exception exc)
                    {
                        ModelState.AddModelError(string.Empty, exc.CompleteExceptionMessage());
                        return View(expedition);
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Expedition data cannot be linked to a form.");
                    return View(expedition);
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
