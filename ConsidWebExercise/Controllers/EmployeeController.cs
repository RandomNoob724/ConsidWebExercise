using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using ConsidWebExercise.Repos;
using ConsidWebExercise.Models;

namespace ConsidWebExercise.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly EmployeeRepository _employeeRepo;
        public EmployeeController(EmployeeRepository employeeRepo)
        {
            _employeeRepo = employeeRepo;
        }

        // GET: /Employee/Index
        public async Task<IActionResult> Index()
        {
            IEnumerable<Employee> employees = await _employeeRepo.GetEmployeesAsync();
            return View(employees);
        }

        // GET: /Employee/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Employee/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Employee employeeToAdd)
        {
            if (ModelState.IsValid)
            {
                await _employeeRepo.AddEmployee(employeeToAdd);
                return RedirectToAction("Index");
            }
            return View(employeeToAdd);
        }

        // GET: /Employee/Edit/id
        public async Task<IActionResult> Edit(int? Id)
        {
            if (Id.HasValue)
            {
                Employee employeeToEdit = await _employeeRepo.GetEmployeeById(Id);
                if(employeeToEdit != null)
                {
                    return View(employeeToEdit);
                }
            }
            return NotFound();
        }

        // POST: /Employee/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Employee employee)
        {
            if (ModelState.IsValid)
            {
                await _employeeRepo.UpdateEmployee(employee);
                return RedirectToAction("Index");
            }
            return View(employee);
        }

        // POST: /Employee/Delete/id
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? Id)
        {
            if (Id.HasValue)
            {
                Employee employeeToRemove = await _employeeRepo.GetEmployeeById(Id);
                if(employeeToRemove == null)
                {
                    return NotFound();
                }
                await _employeeRepo.RemoveEmployee(employeeToRemove);
                return RedirectToAction("Index");
            }
            return NotFound();
        }
    }
}
