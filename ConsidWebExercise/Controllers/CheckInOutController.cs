using ConsidWebExercise.BLL;
using ConsidWebExercise.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace ConsidWebExercise.Controllers
{
    public class CheckInOutController : Controller
    {
        private readonly CheckInOutBusinessLogic _checkInOutBusinessLogic;
        private readonly LibraryItemBusinessLogic _libraryItemBusinessLogic;
        
        public CheckInOutController(CheckInOutBusinessLogic checkInOutBusinessLogic, LibraryItemBusinessLogic libraryItemBusinessLogic)
        {
            _checkInOutBusinessLogic = checkInOutBusinessLogic;
            _libraryItemBusinessLogic = libraryItemBusinessLogic;
        }

        public async Task<IActionResult> Index()
        {
            var viewModel = await _checkInOutBusinessLogic.GetBarrowableItemsModel();
            return View(viewModel);
        }

        // GET: /checkinout/checkout
        public async Task<IActionResult> CheckOut(int? Id)
        {
            if (Id.HasValue)
            {
                LibraryItem itemToCheckOut = await _libraryItemBusinessLogic.GetLibraryItemById(Id);
                var viewModel = new CheckOutViewModel
                {
                    Id = itemToCheckOut.Id
                };
                return View(viewModel);
            }
            return NotFound();
        }

        // POST: /checkinout/checkin/id
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CheckIn(int? Id)
        {
            if (Id.HasValue)
            {
                try
                {
                    await _checkInOutBusinessLogic.CheckInItem(Id);
                    return RedirectToAction("Index");
                }
                catch (Exception e)
                {
                    throw;
                }
            }
            return RedirectToAction("Index");
        }

        // POST: /checkinout/checkout/id
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CheckOutPost(CheckOutViewModel inputModel)
        {
            if (ModelState.IsValid)
            {
                await _checkInOutBusinessLogic.CheckOutItem(inputModel);
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
    }
}
