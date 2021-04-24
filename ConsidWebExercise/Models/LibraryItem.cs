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
        [ForeignKey("CategoryId")]
        public Category Category { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Author { get; set; }
        public int? Pages { get; set; }
        public int? RunTimeMinutes { get; set; }
        [Required]
        public bool IsBorrowable { get; set; }
        [Required]
        public string Barrower { get; set; }
        public DateTime? BarrowDate { get; set; }
        [Required]
        public string Type { get; set; }
    }
}
