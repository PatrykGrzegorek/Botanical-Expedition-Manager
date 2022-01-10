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

            List<ViewSpecies> vw_species = new List<ViewSpecies>();
            for (var i = 0; i < species.Count(); i++)
            {
                int idFamily = ctx.Families.Where(d => d.FamilyId == ctx.Genus.Where(d => d.GenusId == species[i].TaxonomicTree).Select(s => s.GenusId).FirstOrDefault()).Select(s => s.FamilyId).FirstOrDefault();
                int idOrder = ctx.Orders.Where(d => d.OrderId == idFamily).Select(s => s.OrderId).FirstOrDefault();
                int idClas = ctx.Classes.Where(d => d.ClassId == idOrder).Select(s => s.ClassId).FirstOrDefault();
                int idDivision = ctx.Divisions.Where(d => d.DivisionId == idClas).Select(s => s.DivisionId).FirstOrDefault();
                int idKingdome = ctx.Kingdoms.Where(d => d.KingdomId == idDivision).Select(s => s.KingdomId).FirstOrDefault();

                vw_species.Add(new ViewSpecies(species[i].Id, species[i].LatinName, species[i].FullName, species[i].IsEndemic, species[i].IsAutochthonous, species[i].IsWeed, species[i].IsInvasive,
                    ctx.Genus.Where(d => d.GenusId == species[i].TaxonomicTree).Select(s => s.Name).FirstOrDefault(),
                    ctx.Orders.Where(d => d.OrderId == idOrder).Select(s => s.Name).FirstOrDefault(),
                    ctx.Classes.Where(d => d.ClassId == idClas).Select(s => s.Name).FirstOrDefault(),
                    ctx.Families.Where(d => d.FamilyId == idFamily).Select(s => s.Name).FirstOrDefault(),
                    ctx.Kingdoms.Where(d => d.KingdomId == idKingdome).Select(s => s.Name).FirstOrDefault(),
                    ctx.Divisions.Where(d => d.DivisionId == idDivision).Select(s => s.Name).FirstOrDefault()));
            }

            var model = new SpeciesViewModel
            {
                Species = vw_species,
                PagingInfo = pagingInfo
            };
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await PrepareDropDownListsGenus();
            return View();
        }

        private async Task PrepareDropDownListsGenus()
        {
            var genu = await ctx.Genus.OrderBy(d => d.Name)
            .Select(d => new { d.Name, d.GenusId })
            .ToListAsync();
            ViewBag.genu = new SelectList(genu,
            "GenusId", "Name");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Species species)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    species.FullName = species.LatinName + " " + ctx.Genus.Where(d => d.GenusId == species.TaxonomicTree).Select(s => s.Name).FirstOrDefault();
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
                    await PrepareDropDownListsGenus();
                    return View(species);
                }
            }
            else
            {
                await PrepareDropDownListsGenus();
                return View(species);
            }

        }

        [HttpGet]
        public IActionResult CreateKingdom()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateKingdom(Kingdom kingdom)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    ctx.Add(kingdom);
                    ctx.SaveChanges();

                    TempData[Constants.Message] = $"kingdom {kingdom.Name} is added.";
                    TempData[Constants.ErrorOccurred] = false;
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception exc)
                {
                    ModelState.AddModelError(string.Empty, exc.CompleteExceptionMessage());
                    return View(kingdom);
                }
            }
            else
            {
                return View(kingdom);
            }
        }

        [HttpGet]
        public async Task<IActionResult> CreateDivision()
        {
            await PrepareDropDownListsKingdom();
            return View();
        }

        private async Task PrepareDropDownListsKingdom()
        {
            var kingdom = await ctx.Kingdoms.OrderBy(d => d.Name)
            .Select(d => new { d.Name, d.KingdomId })
            .ToListAsync();
            ViewBag.kingdom = new SelectList(kingdom,
            "KingdomId", "Name");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateDivision(Division division)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    ctx.Add(division);
                    await ctx.SaveChangesAsync();

                    TempData[Constants.Message] = $"division {division.Name} is added.";
                    TempData[Constants.ErrorOccurred] = false;
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception exc)
                {
                    ModelState.AddModelError(string.Empty, exc.CompleteExceptionMessage());
                    await PrepareDropDownListsKingdom();
                    return View(division);
                }
            }
            else
            {
                await PrepareDropDownListsKingdom();
                return View(division);
            }
        }

        [HttpGet]
        public async Task<IActionResult> CreateClass()
        {
            await PrepareDropDownListsDivision();
            return View();
        }

        private async Task PrepareDropDownListsDivision()
        {
            var division = await ctx.Divisions.OrderBy(d => d.Name)
            .Select(d => new { d.Name, d.DivisionId })
            .ToListAsync();
            ViewBag.division = new SelectList(division,
            "DivisionId", "Name");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateClass(Class clas)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    ctx.Add(clas);
                    await ctx.SaveChangesAsync();

                    TempData[Constants.Message] = $"clas {clas.Name} is added.";
                    TempData[Constants.ErrorOccurred] = false;
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception exc)
                {
                    ModelState.AddModelError(string.Empty, exc.CompleteExceptionMessage());
                    await PrepareDropDownListsDivision();
                    return View(clas);
                }
            }
            else
            {
                await PrepareDropDownListsDivision();
                return View(clas);
            }
        }

        [HttpGet]
        public async Task<IActionResult> CreateOrder()
        {
            await PrepareDropDownListsClass();
            return View();
        }

        private async Task PrepareDropDownListsClass()
        {
            var clas = await ctx.Classes.OrderBy(d => d.Name)
            .Select(d => new { d.Name, d.ClassId })
            .ToListAsync();
            ViewBag.clas = new SelectList(clas,
            "ClassId", "Name");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateOrder(Order order)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    ctx.Add(order);
                    await ctx.SaveChangesAsync();

                    TempData[Constants.Message] = $"order {order.Name} is added.";
                    TempData[Constants.ErrorOccurred] = false;
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception exc)
                {
                    ModelState.AddModelError(string.Empty, exc.CompleteExceptionMessage());
                    await PrepareDropDownListsClass();
                    return View(order);
                }
            }
            else
            {
                await PrepareDropDownListsClass();
                return View(order);
            }
        }

        [HttpGet]
        public async Task<IActionResult> CreateFamily()
        {
            await PrepareDropDownListsOrder();
            return View();
        }

        private async Task PrepareDropDownListsOrder()
        {
            var order = await ctx.Orders.OrderBy(d => d.Name)
            .Select(d => new { d.Name, d.OrderId })
            .ToListAsync();
            ViewBag.order = new SelectList(order,
            "OrderId", "Name");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateFamily(Family family)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    ctx.Add(family);
                    await ctx.SaveChangesAsync();

                    TempData[Constants.Message] = $"family {family.Name} is added.";
                    TempData[Constants.ErrorOccurred] = false;
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception exc)
                {
                    ModelState.AddModelError(string.Empty, exc.CompleteExceptionMessage());
                    await PrepareDropDownListsOrder();
                    return View(family);
                }
            }
            else
            {
                await PrepareDropDownListsOrder();
                return View(family);
            }
        }

        [HttpGet]
        public async Task<IActionResult> CreateGenus()
        {
            await PrepareDropDownListsFamily();
            return View();
        }

        private async Task PrepareDropDownListsFamily()
        {
            var family = await ctx.Families.OrderBy(d => d.Name)
            .Select(d => new { d.Name, d.FamilyId })
            .ToListAsync();
            ViewBag.family = new SelectList(family,
            "FamilyId", "Name");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateGenus(Genu genu)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    ctx.Add(genu);
                    await ctx.SaveChangesAsync();

                    TempData[Constants.Message] = $"genus {genu.Name} is added.";
                    TempData[Constants.ErrorOccurred] = false;
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception exc)
                {
                    ModelState.AddModelError(string.Empty, exc.CompleteExceptionMessage());
                    await PrepareDropDownListsFamily();
                    return View(genu);
                }
            }
            else
            {
                await PrepareDropDownListsFamily();
                return View(genu);
            }
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
                    d => d.LatinName,  d => d.IsEndemic, d => d.IsAutochthonous, d => d.IsWeed, d => d.IsInvasive
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
