using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConsidWebExercise.Data;
using ConsidWebExercise.Models;
using Microsoft.EntityFrameworkCore;

namespace ConsidWebExercise.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly ApplicationDbContext _db;
        public EmployeeController(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Employee> employees = await _db.Employees.ToListAsync();
            return View(employees);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Employee newEmployee)
        {
            if (ModelState.IsValid)
            {
                await _db.Employees.AddAsync(newEmployee);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(newEmployee);
        }

        public async Task<IActionResult> Edit(int? Id)
        {
            if (Id.HasValue)
            {
                Employee employeeToEdit = await _db.Employees.FindAsync(Id);
                if(employeeToEdit != null)
                {
                    return View(employeeToEdit);
                }
            }
            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Employee employee)
        {
            if (ModelState.IsValid)
            {
                _db.Employees.Update(employee);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(employee);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? Id)
        {
            if (Id.HasValue)
            {
                Employee employeeToRemove = await _db.Employees.FindAsync(Id);
                if(employeeToRemove == null)
                {
                    return NotFound();
                }
                _db.Employees.Remove(employeeToRemove);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return NotFound();
        }
    }
}
