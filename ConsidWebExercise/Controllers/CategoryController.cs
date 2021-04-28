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
        public async Task<IActionResult> Index()
        {
            IEnumerable<Category> categoryList = await _db.Categories.ToListAsync();
            return View(categoryList);
        }

        // GET: Category/Edit
        public async Task<IActionResult> Edit(int? id)
        {
            Category categoryObj = await _db.Categories.FindAsync(id);
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
        public async Task<IActionResult> Create(Category categoryObj)
        {
            if (ModelState.IsValid)
            {
                await _db.AddAsync(categoryObj);
                await _db.SaveChangesAsync();
                return Redirect("Index");
            }
            return View(categoryObj);
        }

        // POST: Category/Delete/id
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            // Add check if the category is refeered to in any of the library items
            Category category = await _db.Categories.FindAsync(id);
            if(_db.LibraryItems.Where(item => item.CategoryId == category.Id).FirstOrDefault() == null && category != null)
            {
                _db.Categories.Remove(category);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        // POST: Category/Edit/id
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Category categoryObj)
        {
            if (ModelState.IsValid)
            {
                _db.Categories.Update(categoryObj);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(categoryObj);
        }
    }
}
