using ConsidWebExercise.Data;
using ConsidWebExercise.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConsidWebExercise.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace ConsidWebExercise.Controllers
{
    public class LibraryItemController : Controller
    {
        private readonly ApplicationDbContext _db;

        public LibraryItemController(ApplicationDbContext db)
        {
            _db = db;
        }

        // GET: /LibraryItem/Index
        public async Task<IActionResult> Index(string? sortingId)
        {
            IEnumerable<LibraryItem> listOfLibraryItems = Enumerable.Empty<LibraryItem>();
            IEnumerable<Category> categories = await _db.Categories.ToListAsync();
            switch (sortingId)
            {
                case "Type":
                    listOfLibraryItems = await _db.LibraryItems.OrderBy(item => item.Type).ToListAsync();
                    break;
                case "Author":
                    listOfLibraryItems = await _db.LibraryItems.OrderBy(item => item.Author).ToListAsync();
                    break;
                case "Pages":
                    listOfLibraryItems = await _db.LibraryItems.OrderBy(item => item.Pages).ToListAsync();
                    break;
                case "RunTimeMinutes":
                    listOfLibraryItems = await _db.LibraryItems.OrderBy(item => item.RunTimeMinutes).ToListAsync();
                    break;
                case "Borrowable":
                    listOfLibraryItems = await _db.LibraryItems.OrderBy(item => item.IsBorrowable).ToListAsync();
                    break;
                case "Title":
                    listOfLibraryItems = await _db.LibraryItems.OrderBy(item => item.Title).ToListAsync();
                    break;
                default:
                    listOfLibraryItems = await _db.LibraryItems.OrderBy(item => item.Category.CategoryName).ToListAsync();
                    break;
            }
            var viewModel = new ListLibraryItemViewModel
            {
                libraryItems = listOfLibraryItems,
                categories = categories
            };
            return View(viewModel);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if(id.HasValue)
            {
                IEnumerable<Category> categories = await _db.Categories.ToListAsync();
                var viewModel = new CreateLibraryItemViewModel
                {
                    LibraryItem = await _db.LibraryItems.FindAsync(id),
                    Categories = categories.ToList()
                };
                return View(viewModel);
            }
            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CreateLibraryItemViewModel obj)
        {
            if(obj.LibraryItem != null)
            {
                _db.LibraryItems.Update(obj.LibraryItem);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        public async Task<IActionResult> Create()
        {
            IEnumerable<Category> categories = await _db.Categories.ToListAsync();
            var viewModel = new CreateLibraryItemViewModel 
            { 
                Categories = categories.ToList()
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateLibraryItemViewModel obj)
        {
            LibraryItem itemToAdd = obj.LibraryItem;
            if(itemToAdd != null)
            {
                await _db.AddAsync(itemToAdd);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? Id)
        {
            if (Id.HasValue)
            {
                LibraryItem item = await _db.LibraryItems.FindAsync(Id);
                _db.LibraryItems.Remove(item);
                await _db.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }
    }
}
