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
    public class CheckInOutController : Controller
    {
        private readonly ApplicationDbContext _db;
        public CheckInOutController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            IEnumerable<LibraryItem> libraryItems = _db.LibraryItems.Where(item => item.IsBorrowable == true);
            IEnumerable<Category> categories = _db.Categories;
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
        public IActionResult CheckIn(int? Id)
        {
            if (Id.HasValue)
            {
                LibraryItem item = _db.LibraryItems.Find(Id);
                item.BarrowDate = null;
                item.Barrower = null;
                _db.LibraryItems.Update(item);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return NotFound();
        }

        // GET: /checkinout/checkout
        public IActionResult CheckOut(int? Id)
        {
            if (Id.HasValue)
            {
                LibraryItem item = _db.LibraryItems.Find(Id);
                return View(item);
            }
            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CheckOutPost(int? Id, string barrower)
        {
            if(Id.HasValue && barrower != null)
            {
                LibraryItem item = _db.LibraryItems.Find(Id);
                item.BarrowDate = DateTime.Now;
                item.Barrower = barrower;

                _db.LibraryItems.Update(item);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return NotFound();
        }
    }
}
