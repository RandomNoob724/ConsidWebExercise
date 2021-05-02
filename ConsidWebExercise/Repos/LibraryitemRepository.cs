using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConsidWebExercise.Data;
using ConsidWebExercise.Models;
using Microsoft.EntityFrameworkCore;

namespace ConsidWebExercise.Repos
{
    public class LibraryitemRepository
    {
        private readonly ApplicationDbContext _db;
        public LibraryitemRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<LibraryItem>> GetLibraryItemsAsync()
        {
            try
            {
                return await _db.LibraryItems.ToListAsync();
            } catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<LibraryItem> GetLibraryItemsById(int? Id)
        {
            try
            {
                return await _db.LibraryItems.FindAsync(Id);
            } catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<LibraryItem>> GetLibraryItemsSortedBy(string sortingId)
        {
            try
            {
                switch (sortingId)
                {
                    case "Type":
                        return await _db.LibraryItems.OrderBy(item => item.Type).ToListAsync();
                    case "Author":
                        return await _db.LibraryItems.OrderBy(item => item.Author).ToListAsync();
                    case "Pages":
                        return await _db.LibraryItems.OrderBy(item => item.Pages).ToListAsync();
                    case "RunTimeMinutes":
                        return await _db.LibraryItems.OrderBy(item => item.RunTimeMinutes).ToListAsync();
                    case "Borrowable":
                        return await _db.LibraryItems.OrderBy(item => item.IsBorrowable).ToListAsync();
                    case "Title":
                        return await _db.LibraryItems.OrderBy(item => item.Title).ToListAsync();
                    default:
                        return await _db.LibraryItems.OrderBy(item => item.Category.CategoryName).ToListAsync();
                }
            } catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
            
        }

        public async Task<IEnumerable<LibraryItem>> GetLibraryItemsWithCategoryId(int? id)
        {
            try
            {
                return await _db.LibraryItems.Where(item => item.CategoryId == id).ToListAsync();
            } catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<LibraryItem>> GetBarrowableItems()
        {
            try
            {
                return await _db.LibraryItems.Where(item => item.IsBorrowable == true).ToListAsync();
            } catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task UpdateLibraryItem(LibraryItem libraryItem)
        {
            try
            {
                _db.LibraryItems.Update(libraryItem);
                await _db.SaveChangesAsync();
            } catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task RemoveLibraryItem(LibraryItem libraryItem)
        {
            try
            {
                _db.Remove(libraryItem);
                await _db.SaveChangesAsync();
            } catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task AddLibraryItem(LibraryItem libraryItem)
        {
            try
            {
                await _db.AddAsync(libraryItem);
                await _db.SaveChangesAsync();
            } catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
