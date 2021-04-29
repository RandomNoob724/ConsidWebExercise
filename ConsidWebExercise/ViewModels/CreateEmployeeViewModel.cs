﻿using System;
using System.Collections.Generic;
using ConsidWebExercise.Models;
namespace ConsidWebExercise.ViewModels
{
    public class CreateEmployeeViewModel
    {
        public IEnumerable<Employee> managers { get; set; }
        public Employee employeeToAdd { get; set; }
        public int rank { get; set; }
    }
}
