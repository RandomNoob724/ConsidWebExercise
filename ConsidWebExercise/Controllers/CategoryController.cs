using ConsidWebExercise.Data;
using ConsidWebExercise.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsidWebExercise.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _db;

        public CategoryController(ApplicationDbContext db)
        {
            _db = db;
        }

        // GET: Category/Index
        public IActionResult Index()
        {
            IEnumerable<Category> categoryList = _db.Categories;
            return View(categoryList);
        }

        // GET: Category/Edit
        public IActionResult Edit(int? id)
        {
            Category categoryObj = _db.Categories.Find(id);
            return View(categoryObj);
        }

        // GET: Category/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Category/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category categoryObj)
        {
            if (ModelState.IsValid)
            {
                _db.Add(categoryObj);
                _db.SaveChanges();
                return Redirect("Index");
            }
            return View(categoryObj);
        }

        // POST: Category/Delete/id
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int? id)
        {
            // Add check if the category is refeered to in any of the library items
            Category category = _db.Categories.Find(id);
            var libraryItemsWithCategory = _db.LibraryItems.Where(item => item.Category == category).ToList();
            if(libraryItemsWithCategory.Count() > 0)
            {
                return RedirectToAction("Index");
            }
            if (category != null)
            {
                _db.Categories.Remove(category);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        // POST: Category/Edit/id
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category categoryObj)
        {
            if (ModelState.IsValid)
            {
                _db.Categories.Update(categoryObj);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(categoryObj);
        }
    }
}
