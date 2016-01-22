using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EmployeeManagementMVC.Models;
using System.Text;

namespace EmployeeManagementMVC.Controllers
{
    public class HomeController : Controller
    {
        private EmployeeEntities db = new EmployeeEntities();

        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Register()
        {
            ViewBag.DEPARTMENT_ID = new SelectList(db.DEPARTMENTS, "DEPARTMENT_ID", "DEPARTMENT_NAME");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register([Bind(Include = "FIRST_NAME,LAST_NAME,EMAIL,PHONE_NUMBER,HIRE_DATE,JOB_ID,SALARY,COMMISSION_PCT,DEPARTMENT_ID")] EMPLOYEE eMPLOYEE)
        {
            if (ModelState.IsValid)
            {
                db.EMPLOYEES.Add(eMPLOYEE);
                db.SaveChanges();
                eMPLOYEE.EMPLOYEE_ID = db.EMPLOYEES.Max(item => item.EMPLOYEE_ID);
                return PartialView("DisplayLoginID",eMPLOYEE) ;
            }

            ViewBag.DEPARTMENT_ID = new SelectList(db.DEPARTMENTS, "DEPARTMENT_ID", "DEPARTMENT_NAME");
            return View();
        }

    }
}