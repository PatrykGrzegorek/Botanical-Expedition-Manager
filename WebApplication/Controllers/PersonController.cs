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
    public class PersonController : Controller
    {
        private readonly RPPP19Context ctx;
        private readonly AppSettings appData;
        public PersonController(RPPP19Context ctx,
        IOptionsSnapshot<AppSettings> options)
        {
            this.ctx = ctx;
            appData = options.Value;
        }

        public IActionResult Index(int page = 1, int sort = 1,
        bool ascending = true)
        {
            int pagesize = appData.PageSize;
            var query = ctx.People
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

            query = query.ApplySort(sort, ascending);            var person = query
                            .Skip((page - 1) * pagesize)
                            .Take(pagesize)
                            .ToList();
            var model = new PersonViewModel
            {
                Person = person,
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
        public IActionResult Create(Person person)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    ctx.Add(person);
                    ctx.SaveChanges();
                    TempData[Constants.Message] =
                    $"Person {person.Name} is added.";
                    TempData[Constants.ErrorOccurred] = false;
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception exc)
                {
                    ModelState.AddModelError(string.Empty,
                    exc.CompleteExceptionMessage());
                    return View(person);
                }
            }
            else
                return View(person);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int IdPerson, int page = 1, int sort = 1, bool ascending = true)
        {
            var person = await ctx.People.FindAsync(IdPerson);
            
            if (person != null)
            {
                try
                {
                    string name = person.Name;
                    ctx.Remove(person);
                    ctx.SaveChanges();
                    TempData[Constants.Message] = $"Person {name} was deleted";
                    TempData[Constants.ErrorOccurred] = false;
                }
                catch (Exception exc)
                {
                    TempData[Constants.Message] = "Error removing person: " + exc.CompleteExceptionMessage();
                    TempData[Constants.ErrorOccurred] = true;
                }
            }
            else
            {
                TempData[Constants.Message] = "There is no person with the id: " + IdPerson.ToString();
                TempData[Constants.ErrorOccurred] = true;
            }
            return RedirectToAction(nameof(Index), new { page = page, sort = sort, ascending = ascending });
        }

        [HttpGet]
        public IActionResult Edit(int id, int page = 1, int sort = 1, bool ascending = true)
        {
            var person = ctx.People.AsNoTracking()
            .Where(d => d.PersonId == id)
            .SingleOrDefault();
            if (person == null)
                return NotFound("There is no person with id: " + id);
            else
            {
                ViewBag.Page = page; ViewBag.Sort = sort;
                ViewBag.Ascending = ascending;
                return View(person);
            }
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, int page = 1, int sort = 1, bool ascending = true)
        {

            try
            {
                Person person = await ctx.People
                                  .Where(d => d.PersonId == id)
                                  .FirstOrDefaultAsync();
                if (person == null)
                {
                    return NotFound("Invalid person id: " + id);
                }

                if (await TryUpdateModelAsync<Person>(person, "",
                    d => d.Name, d => d.LastName, d => d.Title
                ))
                {
                    ViewBag.Page = page;
                    ViewBag.Sort = sort;
                    ViewBag.Ascending = ascending;
                    try
                    {
                        await ctx.SaveChangesAsync();
                        TempData[Constants.Message] = "Person updated.";
                        TempData[Constants.ErrorOccurred] = false;
                        return RedirectToAction(nameof(Index), new { page = page, sort = sort, ascending = ascending });
                    }
                    catch (Exception exc)
                    {
                        ModelState.AddModelError(string.Empty, exc.CompleteExceptionMessage());
                        return View(person);
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Person data cannot be linked to a form.");
                    return View(person);
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
