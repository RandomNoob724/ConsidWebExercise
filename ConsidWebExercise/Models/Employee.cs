using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ConsidWebExercise.Models
{
    public class Employee
    {
        private const decimal EMPLOYEE_COEFICIENT = 1.125M;
        private const decimal MANAGER_COEFICIENT = 1.725M;
        private const decimal CEO_COEFICIENT = 2.725M;

        [Key]
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public decimal Salary { get; set; }
        public bool IsCEO { get; set; }
        public bool IsManager { get; set; }
        public int? ManagerId { get; set; }

        public void CalculateSalary(int rank)
        {
            if (IsCEO)
            {
                Salary = rank * CEO_COEFICIENT;
            }
            else if (IsManager)
            {
                Salary = rank * MANAGER_COEFICIENT;
            }
            else
            {
                Salary = rank * EMPLOYEE_COEFICIENT;
            }
        }

        public void Validate(IEnumerable<Employee> CEO)
        {
            try
            {
                var errorList = new List<Exception>();
                if (IsCEO)
                {
                    if (CEO.Count() > 0)
                    {
                        errorList.Add(new ArgumentException("Cannot assign more than one CEO"));
                        if (IsManager == false && ManagerId == CEO.First().Id)
                        {
                            errorList.Add(new ArgumentException("CEO are not allowed to be manager for employees"));
                        }
                    }
                }
                if (IsCEO && IsManager)
                {
                    errorList.Add(new ArgumentException("Cannot assing both CEO and Manager role to a single employee"));
                }
                if (!IsCEO && !IsManager && ManagerId == null)
                {
                    errorList.Add(new ArgumentException("When creating a new employee you need to assign a manager"));
                }
                if (IsManager && ManagerId == Id)
                {
                    errorList.Add(new ArgumentException("Manager can not be their own manager"));
                }
                if (IsCEO && ManagerId != null)
                {
                    errorList.Add(new ArgumentException("CEO are not allowed to have a manager"));
                }
                if (errorList.Count() > 0)
                {
                    throw new AggregateException(errorList);
                }
            }
            catch (AggregateException e)
            {
                throw;
            }
        }
    }
}
