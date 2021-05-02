using System;
using System.ComponentModel.DataAnnotations;

namespace ConsidWebExercise.Models
{
    public class CheckOutViewModel
    {
        public int Id { get; set; }
        [Required]
        public string name { get; set; }
    }
}
