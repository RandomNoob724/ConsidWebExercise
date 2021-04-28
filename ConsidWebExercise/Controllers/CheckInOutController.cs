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
    public class CheckInOutController : Controller
    {
        private readonly ApplicationDbContext _db;
        public CheckInOutController(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<LibraryItem> libraryItems = await _db.LibraryItems.Where(item => item.IsBorrowable == true).ToListAsync();
            IEnumerable<Category> categories = await _db.Categories.ToListAsync();
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
                LibraryItem item = await _db.LibraryItems.FindAsync(Id);
                item.BorrowDate = null;
                item.Borrower = null;
                _db.LibraryItems.Update(item);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return NotFound();
        }

        // GET: /checkinout/checkout
        public async Task<IActionResult> CheckOut(int? Id)
        {
            if (Id.HasValue)
            {
                LibraryItem item = await _db.LibraryItems.FindAsync(Id);
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
                LibraryItem item = await _db.LibraryItems.FindAsync(Id);
                item.BorrowDate = DateTime.Now;
                item.Borrower = borrower;

                _db.LibraryItems.Update(item);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return NotFound();
        }
    }
}
