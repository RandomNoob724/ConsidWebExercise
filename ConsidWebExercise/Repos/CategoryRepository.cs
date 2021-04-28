using System;
using Microsoft.EntityFrameworkCore;
using ConsidWebExercise.Data;
using System.Threading.Tasks;
using System.Collections.Generic;
using ConsidWebExercise.Models;

namespace ConsidWebExercise.Repos
{
    public class CategoryRepository
    {
        private readonly ApplicationDbContext _db;
        public CategoryRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Category>> GetCategoriesAsync()
        {
            return await _db.Categories.ToListAsync();
        }

        public async Task<Category> GetCategoryById(int? id)
        {
            return await _db.Categories.FindAsync(id);
        }

        public async Task AddCategory(Category category)
        {
            await _db.AddAsync(category);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateCategory(Category category)
        {
            _db.Update(category);
            await _db.SaveChangesAsync();
        }

        public async Task RemoveCategory(Category category)
        {
            _db.Remove(category);
            await _db.SaveChangesAsync();
        }
    }
}
