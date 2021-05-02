using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ConsidWebExercise.Models;
using ConsidWebExercise.BLL;
using System;
using System.Collections.Generic;

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
            employeeModel.managers = await _employeeBusinessLogic.GetManagers();
            if (ModelState.IsValid)
            {
                try
                {
                    await _employeeBusinessLogic.CreateNewEmployee(employeeModel.employeeToAdd, employeeModel.rank);
                    return RedirectToAction("Index");
                } catch(AggregateException e)
                {
                    foreach(Exception exception in e.InnerExceptions)
                    {
                        ModelState.AddModelError(" ", exception.Message);
                    }
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
                try
                {
                    Employee employeeToEdit = await _employeeBusinessLogic.GetEmployeeById(Id);
                    IEnumerable<Employee> managers = await _employeeBusinessLogic.GetManagers();
                    var viewModel = new CreateEmployeeViewModel
                    {
                        employeeToAdd = employeeToEdit,
                        managers = managers,
                    };
                    return View(viewModel);
                } catch(Exception e)
                {
                    ModelState.AddModelError("",e.Message);
                }
            }
            return RedirectToAction("Index");
        }

        // POST: /Employee/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CreateEmployeeViewModel employeeModel)
        {
            IEnumerable<Employee> managers = await _employeeBusinessLogic.GetManagers();
            employeeModel.managers = managers;
            if (ModelState.IsValid)
            {
                try
                {
                    await _employeeBusinessLogic.UpdateEmployeeInfo(employeeModel.employeeToAdd, employeeModel.rank);
                    return RedirectToAction("Index");
                }
                catch (AggregateException e)
                {
                    foreach (Exception exception in e.InnerExceptions)
                    {
                        ModelState.AddModelError(" ", exception.Message);
                    }
                }
            }
            return View(employeeModel);
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
            } catch(AggregateException ex)
            {
                Employee employee = await _employeeBusinessLogic.GetEmployeeById(Id);
                foreach(Exception exception in ex.InnerExceptions)
                {
                    ModelState.AddModelError("", exception.Message);
                }
                return View(employee);
            }
        }
    }
}
