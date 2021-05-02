using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConsidWebExercise.Data;
using ConsidWebExercise.Models;
using Microsoft.EntityFrameworkCore;

namespace ConsidWebExercise.Repos
{
    public class EmployeeRepository
    {

        private readonly ApplicationDbContext _db;
        public EmployeeRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Employee>> GetEmployeesAsync()
        {
            try
            {
                return await _db.Employees.ToListAsync();
            } catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Employee> GetEmployeeById(int? id)
        {
            try
            {
                return await _db.Employees.FindAsync(id);
            } catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task AddEmployee(Employee employee)
        {
            try
            {
                await _db.Employees.AddAsync(employee);
                await _db.SaveChangesAsync();
            } catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task RemoveEmployee(Employee employee)
        {
            try
            {
                _db.Remove(employee);
                await _db.SaveChangesAsync();
            } catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task UpdateEmployee(Employee employee)
        {
            try
            {
                _db.Employees.Update(employee);
                await _db.SaveChangesAsync();
            } catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<Employee>> GetCEO()
        {
            try
            {
                return await _db.Employees.Where(employee => employee.IsCEO == true).ToListAsync();
            } catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<Employee>> GetManagers()
        {
            try
            {
                return await _db.Employees.Where(employee => employee.IsManager == true).ToListAsync();
            } catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        
    }
}
