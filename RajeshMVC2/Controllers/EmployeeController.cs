using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RajeshMVC2.Models;
using RajeshMVC2.ViewModel;
using System;
using System.Collections.Generic;
using System.Globalization;
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
            ViewBag.dept = DbContext.Departments.ToList();
            Employee em = new Employee();
            em.Name = Request.Form["Name"];
            em.Email = Request.Form["Email"];
            em.Mobile = Request.Form["Mobile"];
            em.Gender = Request.Form["Gender"];
            var salary = Request.Form["Salary"];
            var dept = Request.Form["Dept_Id"];

            if (!ModelState.IsValid)
            {
             
                return View(em);
            }
            if (DbContext.Employees.Any(e => e.Email == em.Email)) {
                ModelState.AddModelError("Email", "This Email is already registerd please try another Email Address!");
                return View(em);
            }
            if (!string.IsNullOrEmpty(salary))
            {
                em.Salary = Convert.ToDecimal(salary);
            }
            if (!string.IsNullOrEmpty(dept))
            {
                em.Dept_Id = Convert.ToInt32(dept);
            }

            var file = Request.Form.Files["Image"];
            string err = string.Empty;
            em.Image = UploadFile(file,out err);
            if (!string.IsNullOrEmpty(err))
            {
                ViewBag.FileError = err;
                ViewBag.dept = DbContext.Departments.ToList();
                return View(employee);
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
            ViewBag.dept = DbContext.Departments.ToList();
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
        public string UploadFile(IFormFile formFile, out string ErrorMessage)
        {
            ErrorMessage = null;
            string fullpath = string.Empty;
            if (formFile != null)
            {
                string ext = Path.GetExtension(formFile.FileName);
                if (ext.ToLower() == ".jpg" || ext.ToLower() == ".jpeg" || ext.ToLower() == ".png" || ext.ToLower() == ".gif")
                {
                    long fsize = formFile.Length;
                    if (fsize / 1024 < 100)
                    {

                        string p = Environment.WebRootPath;
                        //    string NewFilePath = Guid.NewGuid().ToString() + ext;
                        string timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff",
                                                CultureInfo.InvariantCulture);
                        timestamp = timestamp.Replace('-', '_');
                        timestamp = timestamp.Replace(':', '_');
                        timestamp = timestamp.Replace('.', '_');

                        string NewFilePath = timestamp + ext;
                        string path = Path.Combine(p, "Images", NewFilePath);
                        FileStream fs = new FileStream(path, FileMode.CreateNew);
                        formFile.CopyTo(fs);
                       fullpath=Path.Combine("Images", NewFilePath);
                        return fullpath;
                    }
                    else
                    {
                        ErrorMessage = "Please select file size max 100kb !!";
                    }
                }
                else
                {
                    ErrorMessage = "Please select Image jpf/jpeg file only!!";
                }
            }
            else
            {
                ErrorMessage = "Please Upload File!";
            }
            return fullpath;

        }
        
        public IActionResult DownloadFile(string FileName)
        {
            string p = Environment.WebRootPath;
            string fullPath = Path.Combine(p, FileName);
            string NFileName = FileName.Split("\\")[1];
            byte[] fileBytes = System.IO.File.ReadAllBytes(fullPath);

            return File(fileBytes,"application/force-download",NFileName);

        }
    }
}
