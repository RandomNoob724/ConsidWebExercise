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

        public async Task UpdateEmployeeInfo(Employee employee, int rank)
        {
            try
            {
                employee.Validate(await _employeeRepo.GetCEO());
                employee.CalculateSalary(rank);
                await _employeeRepo.UpdateEmployee(employee);
            } catch(AggregateException e)
            {
                throw;
            }
        }

        public async Task RemoveEmployee(int? employeeId)
        {
            try
            {
                Employee employeeToRemove = await GetEmployeeById(employeeId);
                if (employeeToRemove == null)
                {
                    throw new Exception("Cannot remove employee with no data");
                }
                else
                {
                    await _employeeRepo.RemoveEmployee(employeeToRemove);
                }
            } catch(Exception e)
            {
                throw;
            }
        }

        public async Task CreateNewEmployee(Employee employee, int rank)
        {
            try
            {
                employee.Validate(await _employeeRepo.GetCEO());
                employee.CalculateSalary(rank);
                await _employeeRepo.AddEmployee(employee);
            } catch(AggregateException e)
            {
                throw;
            }
        }
    }
}
