using ConsidWebExercise.Data;
using ConsidWebExercise.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConsidWebExercise.BLL;
using System;

namespace ConsidWebExercise.Controllers
{
    public class LibraryItemController : Controller
    {
        private readonly LibraryItemBusinessLogic _libraryItemBll;
        private readonly CategoryBusinessLogic _categoryBll;

        public LibraryItemController(LibraryItemBusinessLogic libraryItemBll, CategoryBusinessLogic categoryBll)
        {
            _libraryItemBll = libraryItemBll;
            _categoryBll = categoryBll;
        }

        // GET: /LibraryItem/Index?sortingId
        // Default sorting will be Category but in view the user will be able to change this depending on preference
        public async Task<IActionResult> Index(string? sortingId)
        {
            IEnumerable<LibraryItem> listOfLibraryItems = await _libraryItemBll.GetLibraryItemsSortedBy(sortingId);
            IEnumerable<Category> categories = await _categoryBll.GetAllCategoriesAsync();
            var viewModel = new ListLibraryItemViewModel
            {
                libraryItems = listOfLibraryItems,
                categories = categories
            };
            return View(viewModel);
        }

        // GET: /LibraryItem/Edit/id
        public async Task<IActionResult> Edit(int? id)
        {
            if(id.HasValue)
            {
                try
                {
                    IEnumerable<Category> categories = await _categoryBll.GetAllCategoriesAsync();
                    var viewModel = new CreateLibraryItemViewModel
                    {
                        LibraryItem = await _libraryItemBll.GetLibraryItemById(id),
                        Categories = categories.ToList()
                    };
                    return View(viewModel);
                } catch(Exception e)
                {
                    ModelState.AddModelError("", e.Message);
                }
            }
            return RedirectToAction("Index");
        }

        // POST: /LibraryItem/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CreateLibraryItemViewModel obj)
        {
            IEnumerable<Category> categories = await _categoryBll.GetAllCategoriesAsync();
            obj.Categories = categories.ToList();
            if (ModelState.IsValid)
            {
                try
                {
                    await _libraryItemBll.UpdateLibraryItem(obj.LibraryItem);
                    return RedirectToAction("Index");
                }
                catch (AggregateException e)
                {
                    foreach (Exception exception in e.InnerExceptions)
                    {
                        ModelState.AddModelError(" ", exception.Message);
                    }
                    return View(obj);
                }
            }
            return View(obj);
        }

        // GET: /LibraryItem/Create
        public async Task<IActionResult> Create()
        {
            IEnumerable<Category> categories = await _categoryBll.GetAllCategoriesAsync();
            var viewModel = new CreateLibraryItemViewModel 
            { 
                Categories = categories.ToList()
            };
            return View(viewModel);
        }

        // POST: /LibraryItem/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateLibraryItemViewModel obj)
        {
            IEnumerable<Category> categories = await _categoryBll.GetAllCategoriesAsync();
            obj.Categories = categories.ToList();
            if (ModelState.IsValid)
            {
                try
                {
                    await _libraryItemBll.AddLibraryItem(obj.LibraryItem);
                    return RedirectToAction("Index");
                } catch(AggregateException ex)
                {
                    foreach(Exception exception in ex.InnerExceptions)
                    {
                        ModelState.AddModelError(" ", exception.Message);
                    }
                    return View(obj);
                }
            }
            return View(obj);
        }


        // POST: /LibraryItem/Delete/id
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? Id)
        {
            if (Id.HasValue)
            {
                try
                {
                    await _libraryItemBll.RemoveLibraryItem(Id);
                } catch(ArgumentException e)
                {
                    ModelState.AddModelError(" ", e.Message);
                }
            }
            return RedirectToAction("Index");
        }
    }
}
