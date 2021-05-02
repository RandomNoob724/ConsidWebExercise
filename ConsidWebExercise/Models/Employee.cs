using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ConsidWebExercise.Models
{
    public class Employee
    {
        //Defining the coeficients for employee status
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
        [Required]
        public bool IsCEO { get; set; }
        [Required]
        public bool IsManager { get; set; }
        public int? ManagerId { get; set; }

        //This function is used to calculate the salary depending on the rank and the employee status
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

        //Validation for every different scenario for adding and updating an employee
        //If error happens throw a list with them and display all of them to the user in the UI
        public void Validate(IEnumerable<Employee> employees)
        {
            try
            {
                var errorList = new List<Exception>();
                var CEO = employees.Where(employee => employee.IsCEO);
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
                if (!IsCEO && !IsManager && ManagerId == CEO.First().Id)
                {
                    errorList.Add(new ArgumentException("CEO cannot be manager for a regular employee"));
                }
                if (IsManager || IsCEO && employees.Where(employee => employee.ManagerId == Id).Count() > 0)
                {
                    errorList.Add(new ArgumentException("Cannot change employee status for employee that is currently managing another employee"));
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
