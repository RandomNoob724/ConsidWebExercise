using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ConsidWebExercise.BLL;
using ConsidWebExercise.Models;
using ConsidWebExercise.Repos;
namespace ConsidWebExercise.BLL
{
    public class LibraryItemBusinessLogic
    {
        private readonly LibraryitemRepository _libraryItemRepo;

        public LibraryItemBusinessLogic(LibraryitemRepository libraryItemRepo)
        {
            _libraryItemRepo = libraryItemRepo;
        }

        public async Task<IEnumerable<LibraryItem>> GetLibraryItemsAsync()
        {
            try
            {
                return await _libraryItemRepo.GetLibraryItemsAsync();
            } catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<LibraryItem> GetLibraryItemById(int? id)
        {
            try
            {
                return await _libraryItemRepo.GetLibraryItemsById(id);
            } catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        //Here we get all of the library items with a specific type of categoryId
        public async Task<IEnumerable<LibraryItem>> GetLibraryItemsWithCategory(int? id)
        {
            try
            {
                return await _libraryItemRepo.GetLibraryItemsWithCategoryId(id);
            } catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task AddLibraryItem(LibraryItem libraryItem)
        {
            try
            {
                if(libraryItem == null)
                {
                    throw new Exception("Cannot remove empty library item");
                }
                else
                {
                    await _libraryItemRepo.AddLibraryItem(libraryItem);
                }
            } catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task RemoveLibraryItem(LibraryItem libraryItem)
        {
            try
            {
                await _libraryItemRepo.RemoveLibraryItem(libraryItem);
            } catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
