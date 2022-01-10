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
    public class ReferenceController : Controller
    {
        private readonly RPPP19Context ctx;
        private readonly AppSettings appData;
        public ReferenceController(RPPP19Context ctx,
        IOptionsSnapshot<AppSettings> options)
        {
            this.ctx = ctx;
            appData = options.Value;
        }

        public async Task<IActionResult> Index(int page = 1, int sort = 1,
        bool ascending = true)
        {
            int pagesize = appData.PageSize;
            var query = ctx.References.AsNoTracking();
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
            var reference = await query
                            .Skip((page - 1) * pagesize)
                            .Take(pagesize)
                            .ToListAsync();

            List<ViewReference> vw_reference = new List<ViewReference>();
            for (var i = 0; i < reference.Count(); i++)
            {
                vw_reference.Add(new ViewReference(reference[i].ReferenceId, reference[i].SpeciesId,
                   ctx.Species.Where(d => d.Id == reference[i].SpeciesId).Select(s => s.FullName).FirstOrDefault(),
                   reference[i].TypeOfReferenceId,
                   ctx.TypeOfReferences.Where(d => d.TypeOfReferenceId == reference[i].TypeOfReferenceId).Select(s => s.Name).FirstOrDefault(),
                   reference[i].AuthorId,
                   ctx.People.Where(d => d.PersonId == reference[i].AuthorId).Select(s => s.Title).FirstOrDefault(),
                   ctx.People.Where(d => d.PersonId == reference[i].AuthorId).Select(s => s.Name).FirstOrDefault(),
                   ctx.People.Where(d => d.PersonId == reference[i].AuthorId).Select(s => s.LastName).FirstOrDefault(),
                   reference[i].Title, reference[i].Year, reference[i].Description));

        }
            var model = new ReferenceViewModel
            {
                Reference = vw_reference,
                PagingInfo = pagingInfo
            };
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await PrepareDropDownListsSpecies();
            await PrepareDropDownListsTypeOfReference();
            await PrepareDropDownListsAuthor();
            return View();
        }
        private async Task PrepareDropDownListsSpecies()
        {
            var species = await ctx.Species.OrderBy(d => d.FullName)
            .Select(d => new { d.FullName, d.Id })
            .ToListAsync();
            ViewBag.species = new SelectList(species,
            "Id", "FullName");
        }
        private async Task PrepareDropDownListsTypeOfReference()
        {
            var typeofreference = await ctx.TypeOfReferences.OrderBy(d => d.Name)
            .Select(d => new { d.Name, d.TypeOfReferenceId })
            .ToListAsync();
            ViewBag.typeofreference = new SelectList(typeofreference,
            "TypeOfReferenceId", "Name");
        }
        private async Task PrepareDropDownListsAuthor()
        {
            var person = await ctx.People.OrderBy(d => d.Name)
            .Select(d => new { d.LastName, d.PersonId })
            .ToListAsync();
            ViewBag.person = new SelectList(person,
            "PersonId", "LastName");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Reference reference)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    ctx.Add(reference);
                    await ctx.SaveChangesAsync();

                    TempData[Constants.Message] = $"Reference {reference.Title} is added.";
                    TempData[Constants.ErrorOccurred] = false;
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception exc)
                {
                    ModelState.AddModelError(string.Empty, exc.CompleteExceptionMessage());
                    await PrepareDropDownListsSpecies();
                    await PrepareDropDownListsTypeOfReference();
                    await PrepareDropDownListsAuthor();
                    return View(reference);
                }
            }
            else
            {
                await PrepareDropDownListsSpecies();
                await PrepareDropDownListsTypeOfReference();
                await PrepareDropDownListsAuthor();
                return View(reference);
            }
        }

        [HttpGet]
        public IActionResult CreateTypeOfReference()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateTypeOfReference(TypeOfReference typeOfReference)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    ctx.Add(typeOfReference);
                    ctx.SaveChanges();
                    TempData[Constants.Message] =
                    $"Museum {typeOfReference.Name} is added.";
                    TempData[Constants.ErrorOccurred] = false;
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception exc)
                {
                    ModelState.AddModelError(string.Empty,
                    exc.CompleteExceptionMessage());
                    return View(typeOfReference);
                }
            }
            else
                return View(typeOfReference);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int IdReference, int page = 1, int sort = 1, bool ascending = true)
        {
            var reference = ctx.References.Find(IdReference);
            if (reference != null)
            {
                try
                {
                    string name = reference.Title;
                    ctx.Remove(reference);
                    ctx.SaveChanges();
                    TempData[Constants.Message] = $"Reference {name} was deleted";
                    TempData[Constants.ErrorOccurred] = false;
                }
                catch (Exception exc)
                {
                    TempData[Constants.Message] = "Error removing reference: " + exc.CompleteExceptionMessage();
                    TempData[Constants.ErrorOccurred] = true;
                }
            }
            else
            {
                TempData[Constants.Message] = "There is no reference with the id: " + IdReference;
                TempData[Constants.ErrorOccurred] = true;
            }
            return RedirectToAction(nameof(Index), new { page = page, sort = sort, ascending = ascending });
        }

        [HttpGet]
        public IActionResult Edit(int id, int page = 1, int sort = 1, bool ascending = true)
        {
            var reference = ctx.References.AsNoTracking()
            .Where(d => d.ReferenceId == id)
            .SingleOrDefault();
            if (reference == null)
                return NotFound("There is no reference with id: " + id);
            else
            {
                ViewBag.Page = page; ViewBag.Sort = sort;
                ViewBag.Ascending = ascending;
                return View(reference);
            }
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, int page = 1, int sort = 1, bool ascending = true)
        {

            try
            {
                Reference reference = await ctx.References
                                  .Where(d => d.ReferenceId == id)
                                  .FirstOrDefaultAsync();
                if (reference == null)
                {
                    return NotFound("Invalid reference id: " + id);
                }

                if (await TryUpdateModelAsync<Reference>(reference, "",
                    d => d.Title, d => d.Year
                ))
                {
                    ViewBag.Page = page;
                    ViewBag.Sort = sort;
                    ViewBag.Ascending = ascending;
                    try
                    {
                        await ctx.SaveChangesAsync();
                        TempData[Constants.Message] = "Reference updated.";
                        TempData[Constants.ErrorOccurred] = false;
                        return RedirectToAction(nameof(Index), new { page = page, sort = sort, ascending = ascending });
                    }
                    catch (Exception exc)
                    {
                        ModelState.AddModelError(string.Empty, exc.CompleteExceptionMessage());
                        return View(reference);
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Reference data cannot be linked to a form.");
                    return View(reference);
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
