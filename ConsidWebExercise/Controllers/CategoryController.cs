using ConsidWebExercise.Data;
using ConsidWebExercise.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConsidWebExercise.BLL;

namespace ConsidWebExercise.Controllers
{
    public class CategoryController : Controller
    {
        private readonly CategoryBusinessLogic _categoryBll;

        public CategoryController(CategoryBusinessLogic categoryBll)
        {
            _categoryBll = categoryBll;
        }

        // GET: Category/Index
        public async Task<IActionResult> Index()
        {
            IEnumerable<Category> categoryList = await _categoryBll.GetAllCategoriesAsync();
            return View(categoryList);
        }

        // GET: Category/Edit
        public async Task<IActionResult> Edit(int? id)
        {
            Category categoryObj = await _categoryBll.GetCategoryById(id);
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
                await _categoryBll.AddNewCategory(categoryObj);
                return Redirect("Index");
            }
            return View(categoryObj);
        }

        // POST: Category/Delete/id
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            Category category = await _categoryBll.GetCategoryById(id);
            await _categoryBll.RemoveCategory(category);
            return RedirectToAction("Index");
        }

        // POST: Category/Edit/id
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Category categoryObj)
        {
            if (ModelState.IsValid)
            {
                await _categoryBll.UpdateCategory(categoryObj);
                return RedirectToAction("Index");
            }
            return View(categoryObj);
        }
    }
}
