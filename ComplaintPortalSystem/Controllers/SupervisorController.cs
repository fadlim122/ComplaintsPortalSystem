using ComplaintPortalSystem.Common;
using ComplaintPortalSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ComplaintPortalSystem.Controllers
{
    public class SupervisorController : Controller
    {
        ComplaintPortalDBEntities dbContext = new ComplaintPortalDBEntities();
        // GET: Supervisor

        [CustomAuthorize(UserRole.ADMIN)]
        public ActionResult Index()
        {
            return View(dbContext.Supervisors.ToList());
        }

        // GET: Supervisor/Details/5
        [CustomAuthorize(UserRole.ADMIN)]
        public ActionResult Details(int id)
        {
            var sv = dbContext.Supervisors.ToList().Where(s => s.ID == id).FirstOrDefault();
            return View(sv);
        }

        // GET: Supervisor/Create
        [CustomAuthorize(UserRole.ADMIN)]
        public ActionResult Create()
        {
            ViewData["DepartmentList"] = dbContext.Departments.ToList();
            return View();
        }

        // POST: Supervisor/Create
        [CustomAuthorize(UserRole.ADMIN)]
        [HttpPost]
        public ActionResult Create(Supervisor supervisor)
        {
            ViewData["DepartmentList"] = dbContext.Departments.ToList();
            try
            {
                if (ModelState.IsValid)
                {
                    if (dbContext.Supervisors.ToList().Where(s => s.DepartmentID == supervisor.DepartmentID).FirstOrDefault() == null)
                    {
                        var accountHolder = dbContext.AccountHolders.ToList().Where(s => s.Email == supervisor.AccountHolder.Email).FirstOrDefault();
                        if (accountHolder == null)
                        {
                            if (supervisor.AccountHolder.Name == null || supervisor.AccountHolder.Email == null || supervisor.AccountHolder.Password == null)
                            {
                                ModelState.AddModelError("Create Error", "Create Error, Please fulfill all the details");
                                return View(supervisor);
                            }
                            else
                            {
                                using (System.Data.Entity.DbContextTransaction trans = dbContext.Database.BeginTransaction())
                                {
                                    supervisor.AccountHolder.Role = UserRole.SUPERVISOR.ToString();
                                    dbContext.AccountHolders.Add(supervisor.AccountHolder);
                                    dbContext.SaveChanges();
                                    supervisor.ID = supervisor.AccountHolder.ID;
                                    dbContext.Supervisors.Add(supervisor);
                                    dbContext.SaveChanges();
                                    trans.Commit();
                                }

                                return RedirectToAction("Index");
                            }
                                
                        }
                        else
                        {
                            ModelState.AddModelError("Create Error", "Create Error, Email account already exist");
                            return View(supervisor);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("Create Error", "Create Error, Supervisor account already exist in current department, please select another one");
                        return View(supervisor);
                    }
                    
                }
                else
                {
                    return View(supervisor);
                }
                // TODO: Add insert logic here
            }
            catch (Exception e)
            {

                return View(supervisor);
            }
        }

        // GET: Supervisor/Edit/5
        [CustomAuthorize(UserRole.ADMIN)]
        public ActionResult Edit(int id)
        {
            ViewData["DepartmentList"] = dbContext.Departments.ToList();
            var sv = dbContext.Supervisors.ToList().Where(s => s.ID == id).FirstOrDefault();
            return View(sv);
        }

        // POST: Supervisor/Edit/5
        [CustomAuthorize(UserRole.ADMIN)]
        [HttpPost]
        public ActionResult Edit(int id, Supervisor supervisor)
        {
            try
            {
                ViewData["DepartmentList"] = dbContext.Departments.ToList();
                // TODO: Add update logic here
                if (ModelState.IsValid)
                {
                    var accountHolder = dbContext.AccountHolders.ToList().Where(s => s.Email == supervisor.AccountHolder.Email && s.ID != id).FirstOrDefault();
                    if (accountHolder == null)
                    {

                        if (dbContext.Supervisors.ToList().Where(s => s.DepartmentID == supervisor.DepartmentID && s.ID != id).FirstOrDefault() == null)
                        {
                            if (supervisor.AccountHolder.Name == null || supervisor.AccountHolder.Email == null || supervisor.AccountHolder.Password == null)
                            {
                                ModelState.AddModelError("Edit Error", "Edit Error, Please fulfill all the details");
                                return View(supervisor);
                            }
                            else
                            {
                                using (System.Data.Entity.DbContextTransaction trans = dbContext.Database.BeginTransaction())
                                {
                                    var sv = dbContext.Supervisors.ToList().Where(s => s.ID == id).FirstOrDefault();
                                    sv.AccountHolder.Name = supervisor.AccountHolder.Name;
                                    sv.AccountHolder.Email = supervisor.AccountHolder.Email;
                                    sv.AccountHolder.Password = supervisor.AccountHolder.Password;
                                    sv.DepartmentID = supervisor.DepartmentID;
                                    dbContext.SaveChanges();
                                    trans.Commit();
                                }

                                return RedirectToAction("Index");
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("Edit Error", "Edit Error, Supervisor account already exist in current department, please select another one");
                            return View(supervisor);
                        }   
                    }
                    else
                    {
                        ModelState.AddModelError("Edit Error", "Edit Error, Email account already exist");
                        return View(supervisor);
                    }
                }
                else
                {
                    return View(supervisor);
                }
            }
            catch
            {
                return View();
            }
        }

        // GET: Supervisor/Delete/5
        [CustomAuthorize(UserRole.ADMIN)]
        public ActionResult Delete(int id)
        {
            var sv = dbContext.Supervisors.ToList().Where(s => s.ID == id).FirstOrDefault();
            return View(sv);
        }

        // POST: Supervisor/Delete/5
        [CustomAuthorize(UserRole.ADMIN)]
        [HttpPost]
        public ActionResult Delete(int id, Supervisor supervisor)
        {
            try
            {
                // TODO: Add delete logic here
                if (ModelState.IsValid)
                {
                    using (System.Data.Entity.DbContextTransaction trans = dbContext.Database.BeginTransaction())
                    {
                        var sv = dbContext.Supervisors.ToList().Where(s => s.ID == id).FirstOrDefault();
                        dbContext.AccountHolders.Remove(sv.AccountHolder);
                        dbContext.Supervisors.Remove(sv);
                        dbContext.SaveChanges();
                        trans.Commit();
                    }
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(supervisor);
                }
            }
            catch (Exception e)
            {
                return View();
            }
        }
    }
}
