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
            var viewModel = await _checkInOutBusinessLogic.GetBorrowableItemsModel();
            return View(viewModel);
        }

        [HttpGet]
        // GET: /checkinout/checkout
        public async Task<IActionResult> CheckOut(int? Id)
        {
            //If the id provided in the request is not null fetch the item that the user want to checkout and look if it is available
            //if it is not borrowed return the CheckOut View with the viewmodel data
            if (Id.HasValue)
            {
                LibraryItem itemToCheckOut = await _libraryItemBusinessLogic.GetLibraryItemById(Id);
                if(itemToCheckOut.Borrower != null)
                {
                    return RedirectToAction("Index");
                } else
                {
                    var viewModel = new CheckOutViewModel
                    {
                        Id = itemToCheckOut.Id
                    };
                    return View(viewModel);
                }
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
        public async Task<IActionResult> CheckOut(CheckOutViewModel inputModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _checkInOutBusinessLogic.CheckOutItem(inputModel);
                    return RedirectToAction("Index");
                } catch(ArgumentException e)
                {
                    ModelState.AddModelError(" ", e.Message);
                    return View(inputModel);
                }   
            }
            return View(inputModel);
        }
    }
}
