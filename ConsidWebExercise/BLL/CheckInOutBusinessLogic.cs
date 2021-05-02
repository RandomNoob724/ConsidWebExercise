using System;
using ConsidWebExercise.Repos;
using ConsidWebExercise.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ConsidWebExercise.BLL
{
    public class CheckInOutBusinessLogic
    {
        private readonly LibraryItemBusinessLogic _libraryBll;
        private readonly CategoryBusinessLogic _categoryBll;

        public CheckInOutBusinessLogic(LibraryItemBusinessLogic libraryBll, CategoryBusinessLogic categoryBll)
        {
            _libraryBll = libraryBll;
            _categoryBll = categoryBll;
        }

        public async Task<ListLibraryItemViewModel> GetBorrowableItemsModel()
        {
            try
            {
                var model = new ListLibraryItemViewModel
                {
                    categories = await _categoryBll.GetAllCategoriesAsync(),
                    libraryItems = await _libraryBll.GetBorrowableItems()
                };
                return model;
            } catch(Exception e)
            {
                throw;
            }
            
        }

        public async Task CheckInItem(int? itemId)
        {
            try
            {
                LibraryItem libraryItem = await _libraryBll.GetLibraryItemById(itemId);
                libraryItem.Borrower = null;
                libraryItem.BorrowDate = null;
                await _libraryBll.UpdateLibraryItem(libraryItem);
            } catch(Exception e)
            {
                throw;
            }
        }

        public async Task CheckOutItem(CheckOutViewModel inputModel)
        {
            try
            {
                LibraryItem libraryItem = await _libraryBll.GetLibraryItemById(inputModel.Id);
                if(libraryItem.Borrower != null)
                {
                    throw new ArgumentException("Item cannot be borrowed by multiple people at once");
                }
                libraryItem.BorrowDate = DateTime.Now;
                libraryItem.Borrower = inputModel.name;
                await _libraryBll.UpdateLibraryItem(libraryItem);
            } catch(ArgumentException e)
            {
                throw;
            }
        }
    }
}
