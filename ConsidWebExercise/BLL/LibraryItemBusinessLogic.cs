using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
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

        public async Task<IEnumerable<LibraryItem>> GetLibraryItemsSortedBy(string? sortingId)
        {
            try
            {
                return await _libraryItemRepo.GetLibraryItemsSortedBy(sortingId);
            } catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<LibraryItem> GetLibraryItemById(int? id)
        {
            try
            {
                LibraryItem itemToReturn = await _libraryItemRepo.GetLibraryItemsById(id);
                if(itemToReturn == null)
                {
                    throw new ArgumentException("There are no items with this id");
                } else
                {
                    return itemToReturn;
                }
            } catch(Exception e)
            {
                throw;
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

        //Logic behind adding new library items
        public async Task AddLibraryItem(LibraryItem libraryItem)
        {
            try
            {
                await ValidateLibraryItem(libraryItem);
                await _libraryItemRepo.AddLibraryItem(libraryItem);
            } catch(AggregateException e)
            {
                throw;
            }
        }

        public async Task RemoveLibraryItem(int? id)
        {
            try
            {
                LibraryItem libraryItem = await _libraryItemRepo.GetLibraryItemsById(id);
                if(libraryItem == null)
                {
                    throw new Exception("Cannot remove empty library Item");
                }
                else
                {
                    await _libraryItemRepo.RemoveLibraryItem(libraryItem);
                }
            } catch(ArgumentException e)
            {
                throw;
            }
        }

        public async Task<List<string>> GetAcronym(IEnumerable<LibraryItem> items)
        {
            List<string> acronymsToReturn = new List<string>();
            await Task.Run(() =>
            {
                foreach (var item in items)
                {
                    string toAppend = "";
                    MatchCollection matches = Regex.Matches(item.Title, @"\b[a-zA-Z0-9]");
                    foreach (var match in matches)
                    {
                        toAppend += match.ToString();
                    }
                    acronymsToReturn.Add(toAppend);
                    toAppend = "";
                }
            });
            return acronymsToReturn;
        }

        private async Task ValidateLibraryItem(LibraryItem libraryItem)
        {
            try
            {
                List<Exception> errorList = new List<Exception>();
                if (libraryItem == null)
                {
                    throw new Exception("Cannot add empty library item");
                }
                else
                {
                    if (libraryItem.Type == "Book")
                    {
                        await ValidateBook(libraryItem);
                    }
                    else if (libraryItem.Type == "Reference Book")
                    {
                        await ValidateReferenceBook(libraryItem);
                    }
                    else if (libraryItem.Type == "Audio Book")
                    {
                        await ValidateAudioBook(libraryItem);
                    }
                    else if (libraryItem.Type == "DVD")
                    {
                        await ValidateDVD(libraryItem);
                    }
                    else
                    {
                        errorList.Add(new ArgumentException("Invalid library item type"));
                    }

                    if(libraryItem.Type != "Reference Book")
                    {
                        libraryItem.IsBorrowable = true;
                    } else
                    {
                        libraryItem.IsBorrowable = false;
                    }

                    if (errorList.Count() > 0)
                    {
                        throw new AggregateException(errorList);
                    }
                }
            } catch(Exception e)
            {
                throw;
            }
            
        }

        private async Task ValidateBook(LibraryItem libraryItem)
        {
            List<Exception> errorList = new List<Exception>();
            if(libraryItem.Author == null || libraryItem.Author.Trim() == string.Empty)
            {
                errorList.Add(new ArgumentException("Author cannot be empty"));
            }
            if(libraryItem.Pages == null || libraryItem.Pages <= 0)
            {
                errorList.Add(new ArgumentException("Books need to have a positive number of pages"));
            }
            if(errorList.Count() > 0)
            {
                throw new AggregateException(errorList);
            }
        }

        private async Task ValidateDVD(LibraryItem libraryItem)
        {
            List<Exception> errorList = new List<Exception>();
            if(libraryItem.RunTimeMinutes == null || libraryItem.RunTimeMinutes <= 0)
            {
                errorList.Add(new ArgumentException("DVDs need to have a positive number of runtime minutes"));
            }

            if(errorList.Count() > 0)
            {
                throw new AggregateException(errorList);
            }
        }

        private async Task ValidateAudioBook(LibraryItem libraryItem)
        {
            List<Exception> errorList = new List<Exception>();
            if (libraryItem.RunTimeMinutes == null || libraryItem.RunTimeMinutes <= 0)
            {
                errorList.Add(new ArgumentException("Audio Books need to have a positive number of runtime minutes"));
            }
            if(errorList.Count() > 0)
            {
                throw new AggregateException(errorList);
            }
        }

        private async Task ValidateReferenceBook(LibraryItem libraryItem)
        {
            List<Exception> errorList = new List<Exception>();
            if (libraryItem.Author == null || libraryItem.Author.Trim() == string.Empty)
            {
                errorList.Add(new ArgumentException("Author cannot be empty"));
            }
            if (libraryItem.Pages == null || libraryItem.Pages <= 0)
            {
                errorList.Add(new ArgumentException("Reference Books need to have a positive number of pages"));
            }
            if (errorList.Count() > 0)
            {
                throw new AggregateException(errorList);
            }
        }
    }
}
