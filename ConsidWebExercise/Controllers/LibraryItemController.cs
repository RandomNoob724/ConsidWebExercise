using ConsidWebExercise.Data;
using ConsidWebExercise.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConsidWebExercise.ViewModels;
using ConsidWebExercise.Repos;

namespace ConsidWebExercise.Controllers
{
    public class LibraryItemController : Controller
    {
        private readonly LibraryitemRepository _libraryRepo;
        private readonly CategoryRepository _categoryRepository;

        public LibraryItemController(LibraryitemRepository libraryRepo, CategoryRepository categoryRepository)
        {
            _libraryRepo = libraryRepo;
            _categoryRepository = categoryRepository;
        }

        // GET: /LibraryItem/Index
        public async Task<IActionResult> Index(string? sortingId)
        {
            IEnumerable<LibraryItem> listOfLibraryItems = await _libraryRepo.GetLibraryItemsSortedBy(sortingId);
            IEnumerable<Category> categories = await _categoryRepository.GetCategoriesAsync();
            var viewModel = new ListLibraryItemViewModel
            {
                libraryItems = listOfLibraryItems,
                categories = categories,
            };
            return View(viewModel);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if(id.HasValue)
            {
                IEnumerable<Category> categories = await _categoryRepository.GetCategoriesAsync();
                var viewModel = new CreateLibraryItemViewModel
                {
                    LibraryItem = await _libraryRepo.GetLibraryItemsById(id),
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
                await _libraryRepo.UpdateLibraryItem(obj.LibraryItem);
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        public async Task<IActionResult> Create()
        {
            IEnumerable<Category> categories = await _categoryRepository.GetCategoriesAsync();
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
                await _libraryRepo.AddLibraryItem(itemToAdd);
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
                LibraryItem item = await _libraryRepo.GetLibraryItemsById(Id);
                if(item != null)
                {
                    await _libraryRepo.RemoveLibraryItem(item);
                }
            }
            return RedirectToAction("Index");
        }
    }
}
