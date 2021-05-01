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
            try
            {
                return await _employeeRepo.GetEmployeesAsync();
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

        public async Task UpdateEmployeeInfo(Employee employee)
        {
            try
            {
                await ValidateEmployee(employee);
                await _employeeRepo.UpdateEmployee(employee);
            } catch(AggregateException e)
            {
                throw e;
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
                throw new Exception(e.Message);
            }
        }

        public async Task CreateNewEmployee(Employee employee)
        {
            try
            {
                await ValidateEmployee(employee);
                CalculateSalary(employee);
                await _employeeRepo.AddEmployee(employee);
            } catch(AggregateException e)
            {
                throw e;
            }
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

        private async Task ValidateEmployee(Employee employee)
        {
            try
            {
                var errorList = new List<Exception>();
                IEnumerable<Employee> CEO = await _employeeRepo.GetCEO();
                if (employee.IsCEO)
                {
                    if (CEO.Count() > 0)
                    {
                        errorList.Add(new ArgumentException("Cannot assign more than one CEO"));
                    }
                }
                if (employee.IsCEO && employee.IsManager)
                {
                    errorList.Add(new ArgumentException("Cannot assing both CEO and Manager role to a single employee"));
                }
                if (!employee.IsCEO && !employee.IsManager && employee.ManagerId == null)
                {
                    errorList.Add(new ArgumentException("When creating a new employee you need to assign a manager"));
                }
                if (errorList.Count() > 0)
                {
                    throw new AggregateException(errorList);
                }
                CalculateSalary(employee);
            }
            catch (AggregateException e)
            {
                throw e;
            }
        }
    }
}
