using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConsidWebExercise.Data;
using ConsidWebExercise.Models;

namespace ConsidWebExercise.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly ApplicationDbContext _db;
        public EmployeeController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            IEnumerable<Employee> employees = _db.Employees;
            return View(employees);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Employee newEmployee)
        {
            if (ModelState.IsValid)
            {
                _db.Employees.Add(newEmployee);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(newEmployee);
        }

        public IActionResult Edit(int? Id)
        {
            if (Id.HasValue)
            {
                Employee employeeToEdit = _db.Employees.Find(Id);
                if(employeeToEdit != null)
                {
                    return View(employeeToEdit);
                }
            }
            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Employee employee)
        {
            if (ModelState.IsValid)
            {
                _db.Employees.Update(employee);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(employee);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int? Id)
        {
            if (Id.HasValue)
            {
                Employee employeeToRemove = _db.Employees.Find(Id);
                if(employeeToRemove == null)
                {
                    return NotFound();
                }
                _db.Employees.Remove(employeeToRemove);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return NotFound();
        }
    }
}
