using Microsoft.AspNetCore.Mvc;
using RajeshMVC2.Models;
using RajeshMVC2.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RajeshMVC2.Controllers
{
    public class EmployeeController : Controller
    {
        public ApplicationDbContext DbContext { get; }

        public EmployeeController(ApplicationDbContext dbContext)
        {
            DbContext = dbContext;
        }
        public IActionResult EmployeeList()
        {
            // var emps = DbContext.Employees.ToList();
            var emps = (from e in DbContext.Employees
                        join
                        d in DbContext.Departments
                        on e.Dept_Id equals d.Id
                        select new EmployeeVM()
                        {
                            Id = e.Id,
                            Name = e.Name,
                            Email = e.Email,
                            Gender = e.Gender,
                            Salary = e.Salary,
                            Mobile = e.Mobile,
                            DName = d.Name
                        }).ToList();
            return View(emps);
        }
        public IActionResult CreateEmployee()
        {
            ViewBag.dept = DbContext.Departments.ToList();
            return View();
        }
       [HttpPost]
        public IActionResult CreateEmployee(Employee employee)
        {

            Employee em = new Employee();
            em.Name = Request.Form["Name"];
            em.Email = Request.Form["Email"];
            em.Mobile = Request.Form["Mobile"];
            em.Gender = Request.Form["Gender"];
            var salary = Request.Form["Salary"];
            var dept = Request.Form["Dept_Id"];

            if (!string.IsNullOrEmpty(salary))
            {
                em.Salary = Convert.ToDecimal(salary);
            }
            if (!string.IsNullOrEmpty(dept))
            {
                em.Dept_Id = Convert.ToInt32(dept);
            }

            DbContext.Employees.Add(em);
            DbContext.SaveChanges();
            return RedirectToAction("EmployeeList");
        }

        public IActionResult DeleteEmployee(int id)
        {
            var emp = DbContext.Employees.SingleOrDefault(e => e.Id == id);
            DbContext.Employees.Remove(emp);
            DbContext.SaveChanges();
            return RedirectToAction("EmployeeList");
        }
        public IActionResult EditEmployee(int id)
        {
            var emp = DbContext.Employees.SingleOrDefault(e => e.Id == id);
            return View(emp);
        }
        [HttpPost]
        public IActionResult EditEmployee(Employee emp)
        {
            DbContext.Employees.Update(emp);
            DbContext.SaveChanges();
            return RedirectToAction("EmployeeList");
        }
    }
}
