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
            return await _db.LibraryItems.ToListAsync();
        }

        public async Task<LibraryItem> GetLibraryItemsById(int? Id)
        {
            return await _db.LibraryItems.FindAsync(Id);
        }

        public async Task<IEnumerable<LibraryItem>> GetLibraryItemsSortedBy(string sortingId)
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
        }

        public async Task<IEnumerable<LibraryItem>> GetLibraryItemsWithCategoryId(int? id)
        {
            return await _db.LibraryItems.Where(item => item.CategoryId == id).ToListAsync();
        }

        public async Task<IEnumerable<LibraryItem>> GetBarrowableItems()
        {
            return await _db.LibraryItems.Where(item => item.IsBorrowable == true).ToListAsync();
        }

        public async Task UpdateLibraryItem(LibraryItem libraryItem)
        {
            _db.Update(libraryItem);
            await _db.SaveChangesAsync();
        }

        public async Task RemoveLibraryItem(LibraryItem libraryItem)
        {
            _db.Remove(libraryItem);
            await _db.SaveChangesAsync();
        }

        public async Task AddLibraryItem(LibraryItem libraryItem)
        {
            await _db.AddAsync(libraryItem);
            await _db.SaveChangesAsync();
        }
    }
}
