using ConsidWebExercise.Data;
using ConsidWebExercise.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConsidWebExercise.Repos;

namespace ConsidWebExercise.Controllers
{
    public class CategoryController : Controller
    {
        private readonly CategoryRepository _categoryRepo;
        private readonly LibraryitemRepository _libraryItemRepo;

        public CategoryController(CategoryRepository categoryRepo, LibraryitemRepository libraryitemRepo)
        {
            _categoryRepo = categoryRepo;
            _libraryItemRepo = libraryitemRepo;
        }

        // GET: Category/Index
        public async Task<IActionResult> Index()
        {
            IEnumerable<Category> categoryList = await _categoryRepo.GetCategoriesAsync();
            return View(categoryList);
        }

        // GET: Category/Edit
        public async Task<IActionResult> Edit(int? id)
        {
            Category categoryObj = await _categoryRepo.GetCategoryById(id);
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
                await _categoryRepo.AddCategory(categoryObj);
                return Redirect("Index");
            }
            return View(categoryObj);
        }

        // POST: Category/Delete/id
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            Category category = await _categoryRepo.GetCategoryById(id);
            IEnumerable<LibraryItem> itemsWithCategory = await _libraryItemRepo.GetLibraryItemsWithCategoryId(id);
            if (itemsWithCategory.Count() == 0 && category != null)
            {
                await _categoryRepo.RemoveCategory(category);
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
                await _categoryRepo.UpdateCategory(categoryObj);
                return RedirectToAction("Index");
            }
            return View(categoryObj);
        }
    }
}
