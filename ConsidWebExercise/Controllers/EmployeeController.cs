using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using ConsidWebExercise.Repos;
using ConsidWebExercise.Models;
using ConsidWebExercise.BLL;
using ConsidWebExercise.ViewModels;

namespace ConsidWebExercise.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly EmployeeBusinessLogic _employeeBusinessLogic;

        public EmployeeController(EmployeeBusinessLogic employeeBusinessLogic)
        {
            _employeeBusinessLogic = employeeBusinessLogic;
        }

        // GET: /Employee/Index
        public async Task<IActionResult> Index()
        {
            return View(await _employeeBusinessLogic.GetAllEmployeesAsync());
        }

        // GET: /Employee/Create
        public async Task<IActionResult> Create()
        {
            var viewModel = new CreateEmployeeViewModel
            {
                managers = await _employeeBusinessLogic.GetManagers()
            };
            return View(viewModel);
        }

        // POST: /Employee/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateEmployeeViewModel employeeModel)
        {
            if (ModelState.IsValid)
            {
                if(await _employeeBusinessLogic.CreateNewEmployee(employeeModel.employeeToAdd))
                {
                    return RedirectToAction("Index");
                }
            }
            return View(employeeModel);
        }

        // GET: /Employee/Edit/id
        public async Task<IActionResult> Edit(int? Id)
        {
            if (Id.HasValue)
            {
                Employee employeeToEdit = await _employeeBusinessLogic.GetEmployeeById(Id);
                if(employeeToEdit != null)
                {
                    return View(employeeToEdit);
                }
            }
            return RedirectToAction("Index");
        }

        // POST: /Employee/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Employee employee)
        {
            if (ModelState.IsValid)
            {
                await _employeeBusinessLogic.UpdateEmployeeInfo(employee);
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
                if(await _employeeBusinessLogic.RemoveEmployee(Id))
                {
                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction("Index");
        }
    }
}
