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
    public class MuseumController : Controller
    {
        private readonly RPPP19Context ctx;
        private readonly AppSettings appData;
        public MuseumController(RPPP19Context ctx,
        IOptionsSnapshot<AppSettings> options)
        {
            this.ctx = ctx;
            appData = options.Value;
        }

        public IActionResult Index(int page = 1, int sort = 1,
        bool ascending = true)
        {
            int pagesize = appData.PageSize;
            var query = ctx.Museums
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

            query = query.ApplySort(sort, ascending);            var museum = query
                            .Skip((page - 1) * pagesize)
                            .Take(pagesize)
                            .ToList();
            var model = new MuseumViewModel
            {
                Museum = museum,
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
        public IActionResult Create(Museum museum)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    ctx.Add(museum);
                    ctx.SaveChanges();
                    TempData[Constants.Message] =
                    $"Museum {museum.Name} is added.";
                    TempData[Constants.ErrorOccurred] = false;
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception exc)
                {
                    ModelState.AddModelError(string.Empty,
                    exc.CompleteExceptionMessage());
                    return View(museum);
                }
            }
            else
                return View(museum);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int IdMuseum, int page = 1, int sort = 1, bool ascending = true)
        {
            Console.WriteLine(IdMuseum.ToString());
            var museum = await ctx.Museums.FindAsync(IdMuseum);
            
            if (museum != null)
            {
                try
                {
                    string name = museum.Name;
                    ctx.Remove(museum);
                    ctx.SaveChanges();
                    TempData[Constants.Message] = $"Museum {name} was deleted";
                    TempData[Constants.ErrorOccurred] = false;
                }
                catch (Exception exc)
                {
                    TempData[Constants.Message] = "Error removing museum: " + exc.CompleteExceptionMessage();
                    TempData[Constants.ErrorOccurred] = true;
                }
            }
            else
            {
                TempData[Constants.Message] = "There is no museum with the id: " + IdMuseum.ToString();
                TempData[Constants.ErrorOccurred] = true;
            }
            return RedirectToAction(nameof(Index), new { page = page, sort = sort, ascending = ascending });
        }

        [HttpGet]
        public IActionResult Edit(int id, int page = 1, int sort = 1, bool ascending = true)
        {
            var museum = ctx.Museums.AsNoTracking()
            .Where(d => d.MuseumId == id)
            .SingleOrDefault();
            if (museum == null)
                return NotFound("There is no museum with id: " + id);
            else
            {
                ViewBag.Page = page; ViewBag.Sort = sort;
                ViewBag.Ascending = ascending;
                return View(museum);
            }
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, int page = 1, int sort = 1, bool ascending = true)
        {

            try
            {
                Museum museum = await ctx.Museums
                                  .Where(d => d.MuseumId == id)
                                  .FirstOrDefaultAsync();
                if (museum == null)
                {
                    return NotFound("Invalid museum id: " + id);
                }

                if (await TryUpdateModelAsync<Museum>(museum, "",
                    d => d.Name, d => d.Country, d => d.City, d => d.StreetNumber, d => d.StreetName, d => d.PostalCode
                ))
                {
                    ViewBag.Page = page;
                    ViewBag.Sort = sort;
                    ViewBag.Ascending = ascending;
                    try
                    {
                        await ctx.SaveChangesAsync();
                        TempData[Constants.Message] = "Museum updated.";
                        TempData[Constants.ErrorOccurred] = false;
                        return RedirectToAction(nameof(Index), new { page = page, sort = sort, ascending = ascending });
                    }
                    catch (Exception exc)
                    {
                        ModelState.AddModelError(string.Empty, exc.CompleteExceptionMessage());
                        return View(museum);
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Museum data cannot be linked to a form.");
                    return View(museum);
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
