using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ConsidWebExercise.Models;
namespace ConsidWebExercise.Models
{
    public class CreateEmployeeViewModel
    {
        public IEnumerable<Employee> managers { get; set; }
        public Employee employeeToAdd { get; set; }
        [Range(1, 10)]
        public int rank { get; set; }
    }
}
