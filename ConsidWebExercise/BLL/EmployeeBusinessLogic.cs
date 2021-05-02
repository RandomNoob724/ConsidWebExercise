using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConsidWebExercise.Models;
using ConsidWebExercise.Repos;

namespace ConsidWebExercise.BLL
{
    public class EmployeeBusinessLogic
    {

        private readonly EmployeeRepository _employeeRepo;

        public EmployeeBusinessLogic(EmployeeRepository employeeRepo)
        {
            _employeeRepo = employeeRepo;
        }

        //Fetches all of the employees from the employee repository
        public async Task<IEnumerable<Employee>> GetAllEmployeesAsync()
        {
            try
            {
                IEnumerable<Employee> employees = await _employeeRepo.GetEmployeesAsync();
                return employees.OrderByDescending(employee => employee.IsCEO).ThenByDescending(employee => employee.IsManager);
            } catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        //Gets employee depending on ID from the employee repository
        public async Task<Employee> GetEmployeeById(int? id)
        {
            try
            {
                return await _employeeRepo.GetEmployeeById(id);
            } catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        //Get managers from the employee repository
        public async Task<IEnumerable<Employee>> GetManagers()
        {
            try
            {
                return await _employeeRepo.GetManagers();
            } catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        //Update employee does a check to validate the updated employee object in order to see if it breaks any of the set rules
        public async Task UpdateEmployeeInfo(Employee employee, int rank)
        {
            try
            {
                employee.Validate(await GetAllEmployeesAsync());
                employee.CalculateSalary(rank);
                await _employeeRepo.UpdateEmployee(employee);
            } catch(AggregateException e)
            {
                throw;
            }
        }

        //Called when removing employees, have special check for if they are a manager and if they are currently managing an employee
        //If they are managing another employee error will be thrown
        public async Task RemoveEmployee(int? employeeId)
        {
            try
            {
                List<Exception> errorList = new List<Exception>();
                Employee employeeToRemove = await GetEmployeeById(employeeId);
                IEnumerable<Employee> employees = await GetAllEmployeesAsync();
                if (employeeToRemove == null)
                {
                    errorList.Add(new ArgumentException("Cannot perform action on empty object"));
                }
                if (employeeToRemove.IsManager || employeeToRemove.IsCEO && employees.Where(employee => employee.ManagerId == employeeToRemove.Id).Count() > 0)
                {
                    errorList.Add(new ArgumentException("Cannot remove employee that is currently managing another employee"));
                }
                if(errorList.Count() > 0)
                {
                    throw new AggregateException(errorList);
                }
                await _employeeRepo.RemoveEmployee(employeeToRemove);
            } catch(AggregateException e)
            {
                throw;
            }
        }

        //When creating a new employee the new employee model will be validated in order to know if it breaks any of the set rules in the system
        public async Task CreateNewEmployee(Employee employee, int rank)
        {
            try
            {
                employee.Validate(await GetAllEmployeesAsync());
                employee.CalculateSalary(rank);
                await _employeeRepo.AddEmployee(employee);
            } catch(AggregateException e)
            {
                throw;
            }
        }
    }
}
