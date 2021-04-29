using ConsidWebExercise.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ConsidWebExercise.ViewModels
{
    public class ListLibraryItemViewModel
    {
        public IEnumerable<LibraryItem> libraryItems{ get; set; }
        public IEnumerable<Category> categories { get; set; }
        public List<string> acronyms { get; set; }
    }
}
