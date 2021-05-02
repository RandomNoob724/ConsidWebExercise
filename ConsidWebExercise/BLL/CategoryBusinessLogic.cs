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

        //Updated the category object but first does a check in order to know if the updated object breaks the validation
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

        //Add new category object but first does a check in order to know if the updated object breaks the validation
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

        //Removes a already existing category but also does a check in order to know that it does not break the validation rules
        public async Task RemoveCategory(Category category)
        {
            try
            {
                IEnumerable<LibraryItem> itemsUsingCategory = await _libraryBLL.GetLibraryItemsWithCategory(category.Id);
                List<Exception> errorList = new List<Exception>();
                //If the category is null or used in any of the library items do not remove, else remove and return true
                //potentially use try and catch instead 
                if (category == null)
                {
                    errorList.Add(new ArgumentException("Cannot remove category with this id"));
                }
                else if (itemsUsingCategory.Count() > 0)
                {
                    //Here should send info that the category is used within a item
                    errorList.Add(new ArgumentException("Category is used in one or multiple LibraryItems and cannot be removed"));
                }
                if(errorList.Count() > 0)
                {
                    throw new AggregateException(errorList);
                }
                else
                {
                    await _categoryRepo.RemoveCategory(category);
                }
            } catch(AggregateException e)
            {
                throw;
            }
        }

        //Validates the category object before performing any other action, if error is found a exception will be thrown
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
