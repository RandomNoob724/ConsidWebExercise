using ConsidWebExercise.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsidWebExercise.ViewModels
{
    public class ListLibraryItemViewModel
    {
        public IEnumerable<LibraryItem> libraryItems{ get; set; }
        public IEnumerable<Category> categories { get; set; }
    }
}
