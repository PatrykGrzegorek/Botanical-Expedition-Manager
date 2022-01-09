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
    public class SpeciesController : Controller
    {
        private readonly RPPP19Context ctx;
        private readonly AppSettings appData;
        public SpeciesController(RPPP19Context ctx,
        IOptionsSnapshot<AppSettings> options)
        {
            this.ctx = ctx;
            appData = options.Value;
        }

        public IActionResult Index(int page = 1, int sort = 1,
        bool ascending = true)
        {
            int pagesize = appData.PageSize;
            var query = ctx.Species
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

            query = query.ApplySort(sort, ascending);
            var species = query
                            .Skip((page - 1) * pagesize)
                            .Take(pagesize)
                            .ToList();
            var model = new SpeciesViewModel
            {
                Species = species,
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
        public IActionResult Create(Species species)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    species.FullName = species.LatinName + " Test";
                    ctx.Add(species);
                    ctx.SaveChanges();
                    TempData[Constants.Message] =
                    $"Species {species.FullName} is added.";
                    TempData[Constants.ErrorOccurred] = false;
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception exc)
                {
                    ModelState.AddModelError(string.Empty,
                    exc.CompleteExceptionMessage());
                    return View(species);
                }
            }
            else
                return View(species);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int IdSpecies, int page = 1, int sort = 1, bool ascending = true)
        {
            var species = ctx.Species.Find(IdSpecies);
            if (species != null)
            {
                try
                {
                    string name = species.LatinName;
                    ctx.Remove(species);
                    ctx.SaveChanges();
                    TempData[Constants.Message] = $"Species {name} was deleted";
                    TempData[Constants.ErrorOccurred] = false;
                }
                catch (Exception exc)
                {
                    TempData[Constants.Message] = "Error removing species: " + exc.CompleteExceptionMessage();
                    TempData[Constants.ErrorOccurred] = true;
                }
            }
            else
            {
                TempData[Constants.Message] = "There is no species with the id: " + IdSpecies;
                TempData[Constants.ErrorOccurred] = true;
            }
            return RedirectToAction(nameof(Index), new { page = page, sort = sort, ascending = ascending });
        }

        [HttpGet]
        public IActionResult Edit(int id, int page = 1, int sort = 1, bool ascending = true)
        {
            var species = ctx.Species.AsNoTracking()
            .Where(d => d.Id == id)
            .SingleOrDefault();
            if (species == null)
                return NotFound("There is no species with id: " + id);
            else
            {
                ViewBag.Page = page; ViewBag.Sort = sort;
                ViewBag.Ascending = ascending;
                return View(species);
            }
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, int page = 1, int sort = 1, bool ascending = true)
        {

            try
            {
                Species species = await ctx.Species
                                  .Where(d => d.Id == id)
                                  .FirstOrDefaultAsync();
                if (species == null)
                {
                    return NotFound("Invalid species id: " + id);
                }

                if (await TryUpdateModelAsync<Species>(species, "",
                    d => d.LatinName, d => d.TaxonomicTree, d => d.IsEndemic, d => d.IsAutochthonous, d => d.IsWeed, d => d.IsInvasive
                ))
                {
                    ViewBag.Page = page;
                    ViewBag.Sort = sort;
                    ViewBag.Ascending = ascending;
                    try
                    {
                        await ctx.SaveChangesAsync();
                        TempData[Constants.Message] = "Species updated.";
                        TempData[Constants.ErrorOccurred] = false;
                        return RedirectToAction(nameof(Index), new { page = page, sort = sort, ascending = ascending });
                    }
                    catch (Exception exc)
                    {
                        ModelState.AddModelError(string.Empty, exc.CompleteExceptionMessage());
                        return View(species);
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Species data cannot be linked to a form.");
                    return View(species);
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
