using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EmployeeManagementMVC.Models;

namespace EmployeeManagementMVC.Controllers
{
    public class EMPLOYEEController : Controller
    {
        private EmployeeEntities db = new EmployeeEntities();


        //Go to Login page
        public ActionResult Login()
        {
            return View();
        }

        //post to login to index page
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login( int MANAGER_ID)
        {
            Session["ID"] = MANAGER_ID;
            int Login_Id = Convert.ToInt32(Session["ID"]);
            return RedirectToAction("Index",new {id = Login_Id});
        }

        // GET: EMPLOYEE
        public ActionResult Index(int? ID)
        {
            if(ID == null)
            {
                return Content("You must enter a Login Id");
            }
            var eMPLOYEES = db.EMPLOYEES.Include(e => e.DEPARTMENT).Include(e => e.EMPLOYEE1).Where(e => e.MANAGER_ID==ID);
            //var eMPLOYEES = db.EMPLOYEES.Where(e => e.MANAGER_ID==MANAGER_ID);
            return View(eMPLOYEES.ToList());
        }

        // GET: EMPLOYEE/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EMPLOYEE eMPLOYEE = db.EMPLOYEES.Find(id);
            if (eMPLOYEE == null)
            {
                return HttpNotFound();
            }
            return View(eMPLOYEE);
        }

        // GET: EMPLOYEE/Create
        public ActionResult Create()
        {
            ViewBag.DEPARTMENT_ID = new SelectList(db.DEPARTMENTS, "DEPARTMENT_ID", "DEPARTMENT_NAME");
            return View();
        }

        // POST: EMPLOYEE/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "FIRST_NAME,LAST_NAME,EMAIL,PHONE_NUMBER,HIRE_DATE,JOB_ID,SALARY,COMMISSION_PCT,DEPARTMENT_ID")] EMPLOYEE eMPLOYEE)
        {
            int Login_Id = Convert.ToInt32(Session["ID"]);
            if (ModelState.IsValid)
            {
                eMPLOYEE.MANAGER_ID = Login_Id;
                db.EMPLOYEES.Add(eMPLOYEE);
                db.SaveChanges();
                return RedirectToAction("Index", new {id = Login_Id });
            }

            ViewBag.DEPARTMENT_ID = new SelectList(db.DEPARTMENTS, "DEPARTMENT_ID", "DEPARTMENT_NAME", eMPLOYEE.DEPARTMENT_ID);
            ViewBag.MANAGER_ID = new SelectList(db.EMPLOYEES, "EMPLOYEE_ID", "FIRST_NAME", eMPLOYEE.MANAGER_ID);
            return View(eMPLOYEE);
        }

        // GET: EMPLOYEE/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EMPLOYEE eMPLOYEE = db.EMPLOYEES.Find(id);
            if (eMPLOYEE == null)
            {
                return HttpNotFound();
            }
            ViewBag.DEPARTMENT_ID = new SelectList(db.DEPARTMENTS, "DEPARTMENT_ID", "DEPARTMENT_NAME", eMPLOYEE.DEPARTMENT_ID);
            ViewBag.MANAGER_ID = new SelectList(db.EMPLOYEES, "EMPLOYEE_ID", "FIRST_NAME", eMPLOYEE.MANAGER_ID);
            return View(eMPLOYEE);
        }

        // POST: EMPLOYEE/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EMPLOYEE_ID,FIRST_NAME,LAST_NAME,EMAIL,PHONE_NUMBER,HIRE_DATE,JOB_ID,SALARY,COMMISSION_PCT,MANAGER_ID,DEPARTMENT_ID")] EMPLOYEE eMPLOYEE)
        {
            int Login_Id = Convert.ToInt32(Session["ID"]);
            if (ModelState.IsValid)
            {
                db.Entry(eMPLOYEE).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { id = Login_Id});
            }
            ViewBag.DEPARTMENT_ID = new SelectList(db.DEPARTMENTS, "DEPARTMENT_ID", "DEPARTMENT_NAME", eMPLOYEE.DEPARTMENT_ID);
            ViewBag.MANAGER_ID = new SelectList(db.EMPLOYEES, "EMPLOYEE_ID", "FIRST_NAME", eMPLOYEE.MANAGER_ID);
            return View(eMPLOYEE);
        }

        // GET: EMPLOYEE/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EMPLOYEE eMPLOYEE = db.EMPLOYEES.Find(id);
            if (eMPLOYEE == null)
            {
                return HttpNotFound();
            }
            return View(eMPLOYEE);
        }

        // POST: EMPLOYEE/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            int Login_Id = Convert.ToInt32(Session["ID"]);
            EMPLOYEE eMPLOYEE = db.EMPLOYEES.Find(id);
            db.EMPLOYEES.Remove(eMPLOYEE);
            db.SaveChanges();
            return RedirectToAction("Index",new { id = Login_Id});
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
