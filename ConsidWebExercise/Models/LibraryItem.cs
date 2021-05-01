using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ConsidWebExercise.Models
{
    public class LibraryItem
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }
        [Required]
        public string Title { get; set; }
        public string Author { get; set; }
        public int? Pages { get; set; }
        public int? RunTimeMinutes { get; set; }
        public bool IsBorrowable { get; set; }
        public string Borrower { get; set; }
        public DateTime? BorrowDate { get; set; }
        [Required]
        public string Type { get; set; }
    }
}
