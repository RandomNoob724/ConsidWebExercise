using ConsidWebExercise.Data;
using ConsidWebExercise.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConsidWebExercise.ViewModels;

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
        public IActionResult Index()
        {
            IEnumerable<LibraryItem> listOfLibraryItems = _db.LibraryItems;
            IEnumerable<Category> categories = _db.Categories;
            var viewModel = new ListLibraryItemViewModel()
            {
                libraryItems = listOfLibraryItems,
                categories = categories
            };
            return View(viewModel);
        }

        public IActionResult Edit(int? id)
        {
            if(id.HasValue)
            {
                IEnumerable<Category> categories = _db.Categories;
                var viewModel = new CreateLibraryItemViewModel()
                {
                    LibraryItem = _db.LibraryItems.Find(id),
                    Categories = categories.ToList()
                };
                return View(viewModel);
            }
            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CreateLibraryItemViewModel obj)
        {
            if(obj != null)
            {
                _db.LibraryItems.Update(obj.LibraryItem);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        public IActionResult Create()
        {
            IEnumerable<Category> categories = _db.Categories;
            var viewModel = new CreateLibraryItemViewModel 
            { 
                Categories = categories.ToList()
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CreateLibraryItemViewModel obj)
        {
            LibraryItem itemToAdd = obj.LibraryItem;
            if(itemToAdd != null)
            {
                _db.Add(itemToAdd);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(obj);
        }
    }
}
