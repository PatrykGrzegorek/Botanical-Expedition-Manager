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
    public class CollectionController : Controller
    {
        private readonly RPPP19Context ctx;
        private readonly AppSettings appData;
        public CollectionController(RPPP19Context ctx,
        IOptionsSnapshot<AppSettings> options)
        {
            this.ctx = ctx;
            appData = options.Value;
        }

        public async Task<IActionResult> Index(int page = 1, int sort = 1,
        bool ascending = true)
        {
            int pagesize = appData.PageSize;
            var query = ctx.Collections.AsNoTracking();
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
            var collection = await query
                            .Skip((page - 1) * pagesize)
                            .Take(pagesize)
                            .ToListAsync();

            List<ViewCollection> vw_collection = new List<ViewCollection>();

            for (var i = 0; i < collection.Count(); i++)
            {
                vw_collection.Add(new ViewCollection(collection[i].CollectionId, collection[i].Name, collection[i].MuseumId, ctx.Museums
                  .Where(d => d.MuseumId == collection[i].MuseumId)
                  .Select(s => s.Name)
                  .FirstOrDefault()));
                
            }
            var model = new CollectionViewModel
            {
                Collection = vw_collection,
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
            var museum = await ctx.Museums.OrderBy(d => d.Name)
            .Select(d => new { d.Name, d.MuseumId })
            .ToListAsync();
            ViewBag.museum = new SelectList(museum,
            "MuseumId", "Name");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Collection collection)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    ctx.Add(collection);
                    await ctx.SaveChangesAsync();

                    TempData[Constants.Message] = $"Collection {collection.Name} is added.";
                    TempData[Constants.ErrorOccurred] = false;
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception exc)
                {
                    ModelState.AddModelError(string.Empty, exc.CompleteExceptionMessage());
                    await PrepareDropDownLists();
                    return View(collection);
                }
            }
            else
            {
                await PrepareDropDownLists();
                return View(collection);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int IdCollection, int page = 1, int sort = 1, bool ascending = true)
        {
            var collection = ctx.Collections.Find(IdCollection);
            if (collection != null)
            {
                try
                {
                    string name = collection.Name;
                    ctx.Remove(collection);
                    ctx.SaveChanges();
                    TempData[Constants.Message] = $"Collection {name} was deleted";
                    TempData[Constants.ErrorOccurred] = false;
                }
                catch (Exception exc)
                {
                    TempData[Constants.Message] = "Error removing collection: " + exc.CompleteExceptionMessage();
                    TempData[Constants.ErrorOccurred] = true;
                }
            }
            else
            {
                TempData[Constants.Message] = "There is no collection with the id: " + IdCollection;
                TempData[Constants.ErrorOccurred] = true;
            }
            return RedirectToAction(nameof(Index), new { page = page, sort = sort, ascending = ascending });
        }

        [HttpGet]
        public IActionResult Edit(int id, int page = 1, int sort = 1, bool ascending = true)
        {
            var collection = ctx.Collections.AsNoTracking()
            .Where(d => d.CollectionId == id)
            .SingleOrDefault();
            if (collection == null)
                return NotFound("There is no collection with id: " + id);
            else
            {
                ViewBag.Page = page; ViewBag.Sort = sort;
                ViewBag.Ascending = ascending;
                return View(collection);
            }
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, int page = 1, int sort = 1, bool ascending = true)
        {

            try
            {
                Collection collection = await ctx.Collections
                                  .Where(d => d.CollectionId == id)
                                  .FirstOrDefaultAsync();
                if (collection == null)
                {
                    return NotFound("Invalid collection id: " + id);
                }

                if (await TryUpdateModelAsync<Collection>(collection, "",
                    d => d.Name
                ))
                {
                    ViewBag.Page = page;
                    ViewBag.Sort = sort;
                    ViewBag.Ascending = ascending;
                    try
                    {
                        await ctx.SaveChangesAsync();
                        TempData[Constants.Message] = "Collection updated.";
                        TempData[Constants.ErrorOccurred] = false;
                        return RedirectToAction(nameof(Index), new { page = page, sort = sort, ascending = ascending });
                    }
                    catch (Exception exc)
                    {
                        ModelState.AddModelError(string.Empty, exc.CompleteExceptionMessage());
                        return View(collection);
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Collection data cannot be linked to a form.");
                    return View(collection);
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
