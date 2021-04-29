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
        const decimal EMPLOYEE_COEFICIENT = 1.125M;
        const decimal MANAGER_COEFICIENT = 1.725M;
        const decimal CEO_COEFICIENT = 2.725M;

        private readonly EmployeeRepository _employeeRepo;

        public EmployeeBusinessLogic(EmployeeRepository employeeRepo)
        {
            _employeeRepo = employeeRepo;
        }

        public async Task<IEnumerable<Employee>> GetAllEmployeesAsync()
        {
            return await _employeeRepo.GetEmployeesAsync();
        }

        public async Task<Employee> GetEmployeeById(int? id)
        {
            return await _employeeRepo.GetEmployeeById(id);
        }

        public async Task<IEnumerable<Employee>> GetManagers()
        {
            return await _employeeRepo.GetManagers();
        }

        public async Task UpdateEmployeeInfo(Employee employee)
        {
            await _employeeRepo.UpdateEmployee(employee);
        }

        public async Task<bool> RemoveEmployee(int? employeeId)
        {
            Employee employeeToRemove = await GetEmployeeById(employeeId);
            if (employeeToRemove == null)
            {
                return false;
            }
            else
            {
                await _employeeRepo.RemoveEmployee(employeeToRemove);
            }
            return true;
        }

        public async Task<bool> CreateNewEmployee(Employee employee)
        {
            //Instead of returning false return something more useful for the user to see
            IEnumerable<Employee> CEO = await _employeeRepo.GetCEO();
            if (employee.IsCEO)
            {
                if(CEO.Count() > 0)
                {
                    return false;
                }
            }
            if (employee.IsCEO && employee.IsManager)
            {
                return false;
            }
            if (!employee.IsCEO && !employee.IsManager && employee.ManagerId == null)
            {
                return false;
            }
            CalculateSalary(employee);
            await _employeeRepo.AddEmployee(employee);
            return true;
        }

        private void CalculateSalary(Employee employee)
        {
            if (employee.IsCEO)
            {
                employee.Salary *= CEO_COEFICIENT;
            }
            else if (employee.IsManager)
            {
                employee.Salary *= MANAGER_COEFICIENT;
            }
            else
            {
                employee.Salary *= EMPLOYEE_COEFICIENT;
            }
        }
    }
}
