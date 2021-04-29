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

        private async Task<List<string>> GetAcronym(IEnumerable<LibraryItem> items)
        {
            List<string> acronymsToReturn = new List<string>();
            await Task.Run(() =>
            {
                foreach (var item in items)
                {
                    string toAppend = "";
                    MatchCollection matches = Regex.Matches(item.Title, @"\b[a-zA-Z0-9]");
                    foreach(var match in matches)
                    {
                        toAppend += match.ToString();
                    }
                    acronymsToReturn.Add(toAppend);
                    toAppend = "";
                }
            });
            return acronymsToReturn;
        }

        // GET: /LibraryItem/Index?sortingId
        // Default sorting will be Category
        public async Task<IActionResult> Index(string? sortingId)
        {
            IEnumerable<LibraryItem> listOfLibraryItems = await _libraryRepo.GetLibraryItemsSortedBy(sortingId);
            IEnumerable<Category> categories = await _categoryRepository.GetCategoriesAsync();
            List<string> acronyms = await GetAcronym(listOfLibraryItems);
            var viewModel = new ListLibraryItemViewModel
            {
                libraryItems = listOfLibraryItems,
                categories = categories,
                acronyms = acronyms
            };
            return View(viewModel);
        }

        // GET: /LibraryItem/Edit/id
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

        // POST: /LibraryItem/Edit
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

        // GET: /LibraryItem/Create
        public async Task<IActionResult> Create()
        {
            IEnumerable<Category> categories = await _categoryRepository.GetCategoriesAsync();
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
            LibraryItem itemToAdd = obj.LibraryItem;
            if(itemToAdd != null)
            {
                await _libraryRepo.AddLibraryItem(itemToAdd);
                return RedirectToAction("Index");
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
