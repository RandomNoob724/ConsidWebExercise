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
            try
            {
                IEnumerable<Category> categoryList = await _categoryBll.GetAllCategoriesAsync();
                return View(categoryList);
            } catch(Exception ex)
            {
                ModelState.AddModelError(" ", ex.Message);
                return View(ModelState);
            }
        }

        // GET: Category/Edit
        public async Task<IActionResult> Edit(int? id)
        {
            try
            {
                Category categoryObj = await _categoryBll.GetCategoryById(id);
                return View(categoryObj);
            } catch(Exception ex)
            {
                ModelState.AddModelError(" ", ex.Message);
                return View(ModelState);
            }
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
                try
                {
                    await _categoryBll.AddNewCategory(categoryObj);
                    return Redirect("Index");
                } catch(Exception e)
                {
                    ModelState.AddModelError(" ", e.Message);
                }
            }
            return View(categoryObj);
        }

        // POST: Category/Delete/id
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                Category category = await _categoryBll.GetCategoryById(id);
                await _categoryBll.RemoveCategory(category);
                return RedirectToAction("Index");
            } catch(Exception ex)
            {
                ModelState.AddModelError(" ", ex.Message);
                return RedirectToAction("Index", ModelState);
            }
        }

        // POST: Category/Edit/id
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Category categoryObj)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _categoryBll.UpdateCategory(categoryObj);
                    return RedirectToAction("Index");
                } catch(Exception ex)
                {
                    ModelState.AddModelError(" ", ex.Message);
                    return View(ModelState);
                }
            }
            return View(categoryObj);
        }
    }
}
