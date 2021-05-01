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

        //Validating the employee model according to the instructions given.
        /*private async Task ValidateEmployee(Employee employee)
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
                if (employee.IsManager && employee.ManagerId == employee.Id)
                {
                    errorList.Add(new ArgumentException("Manager can not be their own manager"));
                }
                if(employee.IsManager == false && employee.ManagerId == CEO.First().Id)
                {
                    errorList.Add(new ArgumentException("CEO are not allowed to be manager for employees"));
                }
                if(employee.IsCEO && employee.ManagerId != null)
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
        }*/
    }
}
