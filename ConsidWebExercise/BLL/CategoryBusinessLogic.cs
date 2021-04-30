using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ConsidWebExercise.Models;
using ConsidWebExercise.Repos;
using System.Linq;
namespace ConsidWebExercise.BLL
{
    public class CategoryBusinessLogic
    {
        private readonly CategoryRepository _categoryRepo;
        private readonly LibraryItemBusinessLogic _libraryBLL;

        public CategoryBusinessLogic(CategoryRepository categoryRepo, LibraryItemBusinessLogic libraryBLL)
        {
            _categoryRepo = categoryRepo;
            _libraryBLL = libraryBLL;
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            try
            {
                return await _categoryRepo.GetCategoriesAsync();
            } catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<Category> GetCategoryById(int? id)
        {
            try
            {
                return await _categoryRepo.GetCategoryById(id);
            } catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task UpdateCategory(Category category)
        {
            try
            {
                await _categoryRepo.UpdateCategory(category);
            } catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task AddNewCategory(Category category)
        {
            if(category == null)
            {
                throw new ArgumentException("Cannot add empty category");
            }
            else
            {
                await _categoryRepo.AddCategory(category);
            }
        }

        public async Task RemoveCategory(Category category)
        {
            IEnumerable<LibraryItem> itemsUsingCategory = await _libraryBLL.GetLibraryItemsWithCategory(category.Id);
            //If the category is null or used in any of the library items do not remove, else remove and return true
            //potentially use try and catch instead 
            if(category == null)
            {
                throw new ArgumentException("Cannot remove category with this id");
            }
            else if(itemsUsingCategory.Count() > 0)
            {
                //Here should send info that the category is used within a item
                throw new ArgumentException("Category is used in one or multiple LibraryItems and cannot be removed");
            }
            else
            {
                await _categoryRepo.RemoveCategory(category);
            }
        }
    }
}
