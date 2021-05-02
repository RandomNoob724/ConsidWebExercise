using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ConsidWebExercise.Models
{
    public class ListLibraryItemViewModel
    {
        public IEnumerable<LibraryItem> libraryItems{ get; set; }
        public IEnumerable<Category> categories { get; set; }

        public async Task<string> GetAcronym(LibraryItem item)
        {
            string acronym = "";
            await Task.Run(() =>
            {
                MatchCollection matches = Regex.Matches(item.Title, @"\b[a-zA-Z0-9]");
                foreach (var match in matches)
                {
                    acronym += match.ToString();
                }
            });
            return acronym;
        }
    }
}
