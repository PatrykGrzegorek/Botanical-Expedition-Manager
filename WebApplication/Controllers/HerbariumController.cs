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

        public async Task<IActionResult> Index(int page = 1, int sort = 1,
        bool ascending = true)
        {
            int pagesize = appData.PageSize;
            var query = ctx.Herbaria.AsNoTracking();
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
            var herbarium = await query
                            .Skip((page - 1) * pagesize)
                            .Take(pagesize)
                            .ToListAsync();

            List<ViewHerbarium> vw_herbarium = new List<ViewHerbarium>();

            for (var i = 0; i < herbarium.Count(); i++)
            {
                vw_herbarium.Add(new ViewHerbarium(herbarium[i].HerbariumId, herbarium[i].CollectionId, herbarium[i].YearOfCollection, herbarium[i].InventoryNumber, ctx.Collections
                  .Where(d => d.CollectionId == herbarium[i].CollectionId)
                  .Select(s => s.Name)
                  .FirstOrDefault(),
                  ctx.PartOfPlants.Where(d => d.PlantId == herbarium[i].PiecesOfPlantsId).Select(s => s.Name).FirstOrDefault(),
                  ctx.Species.Where(d => d.Id == herbarium[i].SpiecesId).Select(s => s.FullName).FirstOrDefault()));
                
            }
            var model = new HerbariumViewModel
            {
                Herbarium = vw_herbarium,
                PagingInfo = pagingInfo
            };
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await PrepareDropDownCollection();
            await PrepareDropDownListsPartOfPlant();
            await PrepareDropDownListsSpecies();
            return View();
        }
        private async Task PrepareDropDownCollection()
        {
            var collection = await ctx.Collections.OrderBy(d => d.Name)
            .Select(d => new { d.Name, d.CollectionId })
            .ToListAsync();
            ViewBag.collection = new SelectList(collection,
            "CollectionId", "Name");
        }

        private async Task PrepareDropDownListsPartOfPlant()
        {
            var part = await ctx.PartOfPlants.OrderBy(d => d.Name)
            .Select(d => new { d.Name, d.PlantId })
            .ToListAsync();
            ViewBag.partofplant = new SelectList(part,
            "PlantId", "Name");
        }

        private async Task PrepareDropDownListsSpecies()
        {
            var species = await ctx.Species.OrderBy(d => d.FullName)
            .Select(d => new { d.FullName, d.Id })
            .ToListAsync();
            ViewBag.species = new SelectList(species,
            "Id", "FullName");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Herbarium herbarium)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    ctx.Add(herbarium);
                    await ctx.SaveChangesAsync();

                    TempData[Constants.Message] = $"Herbarium {herbarium.InventoryNumber} is added.";
                    TempData[Constants.ErrorOccurred] = false;
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception exc)
                {
                    ModelState.AddModelError(string.Empty, exc.CompleteExceptionMessage());
                    await PrepareDropDownCollection();
                    await PrepareDropDownListsPartOfPlant();
                    await PrepareDropDownListsSpecies();
                    return View(herbarium);
                }
            }
            else
            {
                await PrepareDropDownCollection();
                await PrepareDropDownListsPartOfPlant();
                await PrepareDropDownListsSpecies();
                return View(herbarium);
            }
        }

        [HttpGet]
        public IActionResult CreatePartOfPlant()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreatePartOfPlant(PartOfPlant partOfPlant)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    ctx.Add(partOfPlant);
                    ctx.SaveChanges();

                    TempData[Constants.Message] = $"partOfPlant {partOfPlant.Name} is added.";
                    TempData[Constants.ErrorOccurred] = false;
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception exc)
                {
                    ModelState.AddModelError(string.Empty, exc.CompleteExceptionMessage());
                    return View(partOfPlant);
                }
            }
            else
            {
                return View(partOfPlant);
            }
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
