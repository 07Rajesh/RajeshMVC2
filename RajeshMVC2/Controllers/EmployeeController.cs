using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RajeshMVC2.Models;
using RajeshMVC2.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace RajeshMVC2.Controllers
{
    public class EmployeeController : Controller
    {
        public ApplicationDbContext DbContext { get; }
        public IHostingEnvironment Environment { get; }

        public EmployeeController(ApplicationDbContext dbContext, IHostingEnvironment environment)
        {
            DbContext = dbContext;
            Environment = environment;
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
                            DName = d.Name,
                            Image=e.Image
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

            var file = Request.Form.Files["Image"];
            em.Image = UploadFile(file);


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
        public string UploadFile(IFormFile formFile)
        {
            string p = Environment.WebRootPath;
            string path = Path.Combine(p, "Images", formFile.FileName);
            FileStream fs = new FileStream(path,FileMode.CreateNew);
            formFile.CopyTo(fs);
            return Path.Combine("Images", formFile.FileName);
        }
    }
}
