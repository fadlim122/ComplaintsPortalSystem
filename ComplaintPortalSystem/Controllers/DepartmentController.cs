using ComplaintPortalSystem.Common;
using ComplaintPortalSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ComplaintPortalSystem.Controllers
{
    public class DepartmentController : Controller
    {
        ComplaintPortalDBEntities dbContext = new ComplaintPortalDBEntities();
        // GET: Department
        [CustomAuthorize(UserRole.ADMIN)]
        public ActionResult Index()
        {
            return View(dbContext.Departments.ToList());
        }

        // GET: Department/Details/5
        [CustomAuthorize(UserRole.ADMIN)]
        public ActionResult Details(int id)
        {
            var dm = dbContext.Departments.ToList().Where(s => s.ID == id).FirstOrDefault();
            return View(dm);
        }

        // GET: Department/Create
        [CustomAuthorize(UserRole.ADMIN)]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Department/Create
        [CustomAuthorize(UserRole.ADMIN)]
        [HttpPost]
        public ActionResult Create(Department department)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var accountHolder = dbContext.Departments.ToList().Where(s => s.Name == department.Name).FirstOrDefault();
                    if (accountHolder == null)
                    {
                        if (department.Name == null || department.HODEmail == null || department.HODName == null)
                        {
                            ModelState.AddModelError("Create Error", "Create Error, Please fulfill all the details");
                            return View(department);
                        }
                        else
                        {
                            dbContext.Departments.Add(department);
                            dbContext.SaveChanges();
                            return RedirectToAction("Index");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("Create Error", "Create Error, Department account already exist");
                        return View(accountHolder);
                    }
                }
                else
                {
                    return View(department);
                }
                // TODO: Add insert logic here
            }
            catch (Exception e)
            {
                return View(department);
            }
        }

        // GET: Department/Edit/5
        [CustomAuthorize(UserRole.ADMIN)]
        public ActionResult Edit(int id)
        {
            var dm = dbContext.Departments.ToList().Where(s => s.ID == id).FirstOrDefault();
            return View(dm);
        }

        // POST: Department/Edit/5
        [CustomAuthorize(UserRole.ADMIN)]
        [HttpPost]
        public ActionResult Edit(int id, Department department)
        {
            try
            {
                // TODO: Add update logic here
                if (ModelState.IsValid)
                {
                    var accountHolder = dbContext.Departments.ToList().Where(s => s.Name == department.Name && s.ID != id).FirstOrDefault();
                    if (accountHolder == null)
                    {

                        if (department.Name == null || department.HODEmail == null || department.HODName == null)
                        {
                            ModelState.AddModelError("Edit Error", "Edit Error, Please fulfill all the details");
                            return View(department);
                        }
                        else
                        {
                            using (System.Data.Entity.DbContextTransaction trans = dbContext.Database.BeginTransaction())
                            {
                                var dm = dbContext.Departments.ToList().Where(s => s.ID == id).FirstOrDefault();
                                dm.Name = department.Name;
                                dm.HODEmail = department.HODEmail;
                                dm.HODName = department.HODName;
                                dbContext.SaveChanges();
                                trans.Commit();
                            }
                            return RedirectToAction("Index");
                        }
                            
                    }
                    else
                    {
                        ModelState.AddModelError("Edit Error", "Edit Error, Department account already exist");
                        return View(accountHolder);
                    }   
                }
                else
                {
                    return View(department);
                }

            }
            catch
            {
                return View();
            }
        }

        // GET: Department/Delete/5
        [CustomAuthorize(UserRole.ADMIN)]
        public ActionResult Delete(int id)
        {
            var dm = dbContext.Departments.ToList().Where(s => s.ID == id).FirstOrDefault();
            return View(dm);      
        }

        // POST: Department/Delete/5
        [CustomAuthorize(UserRole.ADMIN)]
        [HttpPost]
        public ActionResult Delete(int id, Department department)
        {
            try
            {
                // TODO: Add delete logic here
                if (ModelState.IsValid)
                {
                    //make sure the to be deleted department is not used by any other tables.
                    var complaintHandlerWithSelectedDepartment = dbContext.ComplaintHandlers.ToList().Where(s => s.DepartmentID == id).FirstOrDefault();
                    var supervisorWithSelectedDepartment = dbContext.Supervisors.ToList().Where(s => s.DepartmentID == id).FirstOrDefault();
                    var dm = dbContext.Departments.ToList().Where(s => s.ID == id).FirstOrDefault();

                    if (complaintHandlerWithSelectedDepartment == null && supervisorWithSelectedDepartment == null)
                    {
                        dbContext.Departments.Remove(dm);
                        dbContext.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("Delete Error", "Department cannot be deleted because it's used by other tables");
                        return View(dm);
                    }
                    
                }
                else
                {
                    return View(department);
                }
            }
            catch (Exception e)
            {
                return View();
            }
        }
    }
}
