using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RajeshMVC2.Models;
using RajeshMVC2.Controllers;
namespace RajeshMVC2.Controllers
{

    public class HomeController : Controller
    {
        public ApplicationDbContext DbContext { get; }

        public HomeController(ApplicationDbContext dbContext)
        {
            DbContext = dbContext;
        }
        public IActionResult Index()
        {
            //string message = "Welcome To ViewBag";
            //ViewBag.msg = message;
            //List<string> Students = new List<string> { "Abhay","Akash","Rajesh","Aryan","Hrithik"};
            //ViewBag.students = Students;

            var employees = DbContext.Employees.ToList();
            return View(employees);
        }
        public IActionResult About()
        {
            return View();
        }
        public IActionResult Contact()
        {
            return View();
        }
        public IActionResult Blog()
        {
            return View();
        }
        public IActionResult Services()
        {
            return View();
        }
    }
}
