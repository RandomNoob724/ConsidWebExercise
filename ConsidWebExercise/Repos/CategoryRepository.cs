using System;
using Microsoft.EntityFrameworkCore;
using ConsidWebExercise.Data;
using System.Threading.Tasks;
using System.Collections.Generic;
using ConsidWebExercise.Models;
using System.Linq;

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
            try
            {
                return await _db.Categories.ToListAsync();
            } catch(DbUpdateException ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<Category>> GetCategoryByName(string? name)
        {
            try
            {
                return await _db.Categories.Where(category => category.CategoryName.ToLower() == name.ToLower()).ToListAsync();
            } catch(Exception ex)
            {
                throw;
            }
        }
        public async Task<Category> GetCategoryById(int? id)
        {
            try
            {
                return await _db.Categories.FindAsync(id);
            } catch(DbUpdateException ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task AddCategory(Category category)
        {
            try
            {
                await _db.AddAsync(category);
                await _db.SaveChangesAsync();
            } catch(DbUpdateException ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task UpdateCategory(Category category)
        {
            try
            {
                _db.Update(category);
                await _db.SaveChangesAsync();
            } catch(DbUpdateException ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task RemoveCategory(Category category)
        {
            try
            {
                _db.Remove(category);
                await _db.SaveChangesAsync();
            } catch(DbUpdateException ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
