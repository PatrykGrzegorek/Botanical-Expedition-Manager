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
using System.Collections.Generic;

namespace WebApplication.Controllers
{
    public class ObservationController : Controller
    {
        private readonly RPPP19Context ctx;
        private readonly AppSettings appData;
        public ObservationController(RPPP19Context ctx,
        IOptionsSnapshot<AppSettings> options)
        {
            this.ctx = ctx;
            appData = options.Value;
        }

        public async Task<IActionResult> Index(int page = 1, int sort = 1,
        bool ascending = true)
        {
            int pagesize = appData.PageSize;
            var query = ctx.Observations.AsNoTracking();
            int count = await query.CountAsync();
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

            query = query.ApplySort(sort, ascending);
            var observation = await query
                            .Skip((page - 1) * pagesize)
                            .Take(pagesize)
                            .ToListAsync();

            List<ViewObservation> vw_observation = new List<ViewObservation>();

            for (var i = 0; i < observation.Count(); i++)
            {
                vw_observation.Add(new ViewObservation(observation[i].ObservationId, observation[i].Date, observation[i].ExpeditionId, ctx.Expeditions
                  .Where(d => d.ExpeditionId == observation[i].ExpeditionId)
                  .Select(s => s.Name)
                  .FirstOrDefault()));
                
            }
            var model = new ObservationViewModel
            {
                Observation = vw_observation,
                PagingInfo = pagingInfo
            };
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await PrepareDropDownLists();
            return View();
        }
        private async Task PrepareDropDownLists()
        {
            var expedition = await ctx.Expeditions.OrderBy(d => d.Name)
            .Select(d => new { d.Name, d.ExpeditionId })
            .ToListAsync();
            ViewBag.expedition = new SelectList(expedition,
            "ExpeditionId", "Name");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Observation observation)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    ctx.Add(observation);
                    await ctx.SaveChangesAsync();

                    TempData[Constants.Message] = $"Observation {observation.ObservationId} is added.";
                    TempData[Constants.ErrorOccurred] = false;
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception exc)
                {
                    ModelState.AddModelError(string.Empty, exc.CompleteExceptionMessage());
                    await PrepareDropDownLists();
                    return View(observation);
                }
            }
            else
            {
                await PrepareDropDownLists();
                return View(observation);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int IdObservation, int page = 1, int sort = 1, bool ascending = true)
        {
            var observation = ctx.Observations.Find(IdObservation);
            if (observation != null)
            {
                try
                {
                    int name = observation.ObservationId;
                    ctx.Remove(observation);
                    ctx.SaveChanges();
                    TempData[Constants.Message] = $"Observation {name} was deleted";
                    TempData[Constants.ErrorOccurred] = false;
                }
                catch (Exception exc)
                {
                    TempData[Constants.Message] = "Error removing observation: " + exc.CompleteExceptionMessage();
                    TempData[Constants.ErrorOccurred] = true;
                }
            }
            else
            {
                TempData[Constants.Message] = "There is no observation with the id: " + IdObservation;
                TempData[Constants.ErrorOccurred] = true;
            }
            return RedirectToAction(nameof(Index), new { page = page, sort = sort, ascending = ascending });
        }

        [HttpGet]
        public IActionResult Edit(int id, int page = 1, int sort = 1, bool ascending = true)
        {
            var observation = ctx.Observations.AsNoTracking()
            .Where(d => d.ObservationId == id)
            .SingleOrDefault();
            if (observation == null)
                return NotFound("There is no observation with id: " + id);
            else
            {
                ViewBag.Page = page; ViewBag.Sort = sort;
                ViewBag.Ascending = ascending;
                return View(observation);
            }
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, int page = 1, int sort = 1, bool ascending = true)
        {

            try
            {
                Observation observation = await ctx.Observations
                                  .Where(d => d.ObservationId == id)
                                  .FirstOrDefaultAsync();
                if (observation == null)
                {
                    return NotFound("Invalid observation id: " + id);
                }

                if (await TryUpdateModelAsync<Observation>(observation, "",
                    d => d.Date
                ))
                {
                    ViewBag.Page = page;
                    ViewBag.Sort = sort;
                    ViewBag.Ascending = ascending;
                    try
                    {
                        await ctx.SaveChangesAsync();
                        TempData[Constants.Message] = "Observation updated.";
                        TempData[Constants.ErrorOccurred] = false;
                        return RedirectToAction(nameof(Index), new { page = page, sort = sort, ascending = ascending });
                    }
                    catch (Exception exc)
                    {
                        ModelState.AddModelError(string.Empty, exc.CompleteExceptionMessage());
                        return View(observation);
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Observation data cannot be linked to a form.");
                    return View(observation);
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
