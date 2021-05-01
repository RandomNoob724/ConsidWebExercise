using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ConsidWebExercise.Models;
using ConsidWebExercise.BLL;
using System;

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
            try
            {
                return View(await _employeeBusinessLogic.GetAllEmployeesAsync());
            } catch (Exception ex)
            {
                ModelState.AddModelError(" ", ex.Message);
                return View(ModelState);
            }
        }

        // GET: /Employee/Create
        public async Task<IActionResult> Create()
        {
            try
            {
                var viewModel = new CreateEmployeeViewModel
                {
                    managers = await _employeeBusinessLogic.GetManagers()
                };
                return View(viewModel);
            } catch(Exception ex)
            {
                ModelState.AddModelError(" ", ex.Message);
                return View(ModelState);
            }
        }

        // POST: /Employee/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateEmployeeViewModel employeeModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _employeeBusinessLogic.CreateNewEmployee(employeeModel.employeeToAdd);
                    return RedirectToAction("Index");
                } catch(AggregateException e)
                {
                    foreach(Exception exception in e.InnerExceptions)
                    {
                        ModelState.AddModelError(" ", exception.Message);
                    }
                    employeeModel.managers = await _employeeBusinessLogic.GetManagers();
                    return View(employeeModel);
                }
            }
            else
            {
                return View(employeeModel);
            }
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
                try
                {
                    await _employeeBusinessLogic.UpdateEmployeeInfo(employee);
                    return RedirectToAction("Index");
                } catch(AggregateException e)
                {
                    foreach(Exception exception in e.InnerExceptions)
                    {
                        ModelState.AddModelError(" ", exception.Message);
                    }
                    return View(employee);
                }
            }
            return View(employee);
        }

        // POST: /Employee/Delete/id
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? Id)
        {
            try
            {
                await _employeeBusinessLogic.RemoveEmployee(Id);
                return RedirectToAction("Index");
            } catch(Exception ex)
            {
                ModelState.AddModelError(" ", ex.Message);
                return RedirectToAction("Index", ModelState);
            }
        }
    }
}
