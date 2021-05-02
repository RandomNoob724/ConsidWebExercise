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
                await Validate(category);
                await _categoryRepo.UpdateCategory(category);
            } catch(AggregateException e)
            {
                throw;
            }
        }

        public async Task AddNewCategory(Category category)
        {
            try
            {
                await Validate(category);
                await _categoryRepo.AddCategory(category);
            } catch(AggregateException e)
            {
                throw;
            }
        }

        public async Task RemoveCategory(Category category)
        {
            try
            {
                IEnumerable<LibraryItem> itemsUsingCategory = await _libraryBLL.GetLibraryItemsWithCategory(category.Id);
                //If the category is null or used in any of the library items do not remove, else remove and return true
                //potentially use try and catch instead 
                if (category == null)
                {
                    throw new ArgumentException("Cannot remove category with this id");
                }
                else if (itemsUsingCategory.Count() > 0)
                {
                    //Here should send info that the category is used within a item
                    throw new ArgumentException("Category is used in one or multiple LibraryItems and cannot be removed");
                }
                else
                {
                    await _categoryRepo.RemoveCategory(category);
                }
            } catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        private async Task Validate(Category category)
        {
            var errorList = new List<Exception>();
            var categoryWithName = await _categoryRepo.GetCategoryByName(category.CategoryName);
            if (category == null)
            {
                errorList.Add(new ArgumentException("Cannot add empty category"));
            }
            if (categoryWithName.Count() > 0)
            {
                errorList.Add(new ArgumentException("Cannot add multiple categories with same name"));
            }
            if (errorList.Count() > 0)
            {
                throw new AggregateException(errorList);
            }
        }
    }
}
