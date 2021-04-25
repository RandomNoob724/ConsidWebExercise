using ConsidWebExercise.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsidWebExercise.ViewModels
{
    public class CreateLibraryItemViewModel
    {
        public LibraryItem LibraryItem { get; set; }
        public List<Category> Categories { get; set; }
    }
}
