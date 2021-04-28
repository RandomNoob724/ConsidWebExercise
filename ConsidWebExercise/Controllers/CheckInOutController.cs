using ConsidWebExercise.Data;
using ConsidWebExercise.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConsidWebExercise.ViewModels;
using Microsoft.EntityFrameworkCore;
using ConsidWebExercise.Repos;

namespace ConsidWebExercise.Controllers
{
    public class CheckInOutController : Controller
    {
        private readonly LibraryitemRepository _libraryRepo;
        private readonly CategoryRepository _categoryRepo;
        public CheckInOutController(LibraryitemRepository libraryRepo, CategoryRepository categoryRepo)
        {
            _libraryRepo = libraryRepo;
            _categoryRepo = categoryRepo;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<LibraryItem> libraryItems = await _libraryRepo.GetBarrowableItems();
            IEnumerable<Category> categories = await _categoryRepo.GetCategoriesAsync();
            var viewModel = new ListLibraryItemViewModel
            {
                libraryItems = libraryItems,
                categories = categories
            };
            return View(viewModel);
        }

        // POST: /checkinout/checkin/id
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CheckIn(int? Id)
        {
            if (Id.HasValue)
            {
                LibraryItem item = await _libraryRepo.GetLibraryItemsById(Id);
                item.BorrowDate = null;
                item.Borrower = null;
                await _libraryRepo.UpdateLibraryItem(item);
                return RedirectToAction("Index");
            }
            return NotFound();
        }

        // GET: /checkinout/checkout
        public async Task<IActionResult> CheckOut(int? Id)
        {
            if (Id.HasValue)
            {
                LibraryItem item = await _libraryRepo.GetLibraryItemsById(Id);
                return View(item);
            }
            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CheckOutPost(int? Id, string borrower)
        {
            if(Id.HasValue && borrower != null)
            {
                LibraryItem item = await _libraryRepo.GetLibraryItemsById(Id);
                item.BorrowDate = DateTime.Now;
                item.Borrower = borrower;

                await _libraryRepo.UpdateLibraryItem(item);
                return RedirectToAction("Index");
            }
            return NotFound();
        }
    }
}
