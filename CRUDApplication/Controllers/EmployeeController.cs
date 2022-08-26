using CRUDApplication.Data;
using CRUDApplication.Models;
using CRUDApplication.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CRUDApplication.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly CRUDAppDbContext _appDbContext;

        public EmployeeController(CRUDAppDbContext dbContext)
        {
            _appDbContext = dbContext;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var employees = _appDbContext.Employees.ToList();

            return View(employees);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(EmployeeViewModel employeeModel)
        {
            var employee = new Employee
            {
                Id = Guid.NewGuid(),
                Name = employeeModel.Name,
                Email = employeeModel.Email,
                Salary = employeeModel.Salary,
                DateOfBirth = employeeModel.DateOfBirth.Date,
                Department = employeeModel.Department
            };

            await _appDbContext.Employees.AddAsync(employee);
            await _appDbContext.SaveChangesAsync();

            return RedirectToAction("Add");
        }

        [HttpGet]
        public async Task<IActionResult> View(Guid id)
        {
            var employee = await _appDbContext.Employees.FirstOrDefaultAsync(x => x.Id == id);

            if (employee is not null)
            {
                var viewModel = new EmployeeUpdateViewModel()
                {
                    Id = Guid.NewGuid(),
                    Name = employee.Name,
                    Email = employee.Email,
                    Salary = employee.Salary,
                    DateOfBirth = employee.DateOfBirth.Date,
                    Department = employee.Department
                };

                return await Task.Run(() => View("View", viewModel));
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> View(EmployeeUpdateViewModel employeeModel)
        {
            var employee = await _appDbContext.Employees.FindAsync(employeeModel.Id);

            if (employee is not null)
            {
                employee.Name = employeeModel.Name;
                employee.Email = employeeModel.Email;
                employee.Salary = employeeModel.Salary;
                employee.DateOfBirth = employeeModel.DateOfBirth.Date;
                employee.Department = employeeModel.Department;

                await _appDbContext.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(EmployeeUpdateViewModel viewModel)
        {
            var employee = await _appDbContext.Employees.FindAsync(viewModel.Id);

            if (employee is not null)
            {
                _appDbContext.Employees.Remove(employee);
                await _appDbContext.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }
    }
}
