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
            return await _db.Employees.ToListAsync();
        }

        public async Task<Employee> GetEmployeeById(int? id)
        {
            return await _db.Employees.FindAsync(id);
        }

        public async Task AddEmployee(Employee employee)
        {
            await _db.Employees.AddAsync(employee);
            await _db.SaveChangesAsync();
        }

        public async Task RemoveEmployee(Employee employee)
        {
            _db.Remove(employee);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateEmployee(Employee employee)
        {
            _db.Update(employee);
            await _db.SaveChangesAsync();
        }

        public async Task<IEnumerable<Employee>> GetCEO()
        {
            return await _db.Employees.Where(employee => employee.IsCEO == true).ToListAsync();
        }

        public async Task<IEnumerable<Employee>> GetManagers()
        {
            return await _db.Employees.Where(employee => employee.IsManager == true).ToListAsync();
        }
    }
}
