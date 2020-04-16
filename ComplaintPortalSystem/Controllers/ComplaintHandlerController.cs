using ComplaintPortalSystem.Common;
using ComplaintPortalSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ComplaintPortalSystem.Controllers
{
    public class ComplaintHandlerController : Controller
    {
        ComplaintPortalDBEntities dbContext = new ComplaintPortalDBEntities();
        // GET: ComplaintHandler

        [CustomAuthorize(UserRole.ADMIN)]
        public ActionResult Index()
        {
            return View(dbContext.ComplaintHandlers.ToList());
        }

        // GET: ComplaintHandler/Details/5
        [CustomAuthorize(UserRole.ADMIN)]
        public ActionResult Details(int id)
        {
            var ch = dbContext.ComplaintHandlers.ToList().Where(s => s.ID == id).FirstOrDefault();
            return View(ch);
        }

        // GET: ComplaintHandler/Create
        [CustomAuthorize(UserRole.ADMIN)]
        public ActionResult Create()
        {
            ViewData["DepartmentList"] = dbContext.Departments.ToList();
            ViewData["SupervisorList"] = dbContext.AccountHolders.Where(s => s.Role == UserRole.SUPERVISOR.ToString()).ToList();
            return View();
        }

        // POST: ComplaintHandler/Create
        [CustomAuthorize(UserRole.ADMIN)]
        [HttpPost]
        public ActionResult Create(ComplaintHandler complainthandler)
        {
            ViewData["DepartmentList"] = dbContext.Departments.ToList();
            ViewData["SupervisorList"] = dbContext.AccountHolders.Where(s => s.Role == UserRole.SUPERVISOR.ToString()).ToList();

            try
            {
                if (ModelState.IsValid)
                {
                    // error here
                    var accountHolder = dbContext.AccountHolders.ToList().Where(s => s.Email == complainthandler.AccountHolder.Email).FirstOrDefault();
                    if (accountHolder == null)
                    {

                        if (complainthandler.AccountHolder.Name == null || complainthandler.AccountHolder.Email == null || complainthandler.AccountHolder.Password == null)
                        {
                            ModelState.AddModelError("Create Error", "Create Error, Please fulfill all the details");
                            return View(complainthandler);
                        }
                        else
                        {
                            var supervisor = dbContext.Supervisors.ToList().Where(r => r.DepartmentID == complainthandler.DepartmentID).FirstOrDefault();
                            //var supervisorID = complainthandler.Department.Supervisors.ToList().Where(r => r.DepartmentID == complainthandler.DepartmentID).FirstOrDefault();
                            using (System.Data.Entity.DbContextTransaction trans = dbContext.Database.BeginTransaction())
                            {
                                complainthandler.AccountHolder.Role = UserRole.COMPLAINT_HANDLER.ToString();
                                dbContext.AccountHolders.Add(complainthandler.AccountHolder);
                                dbContext.SaveChanges();
                                complainthandler.ID = complainthandler.AccountHolder.ID;
                                complainthandler.SupervisorID = supervisor.ID;
                                dbContext.ComplaintHandlers.Add(complainthandler);
                                dbContext.SaveChanges();
                                trans.Commit();
                            }
                            return RedirectToAction("Index");
                        }
                            
                    }
                    else
                    {
                        ModelState.AddModelError("Create Error", "Create Error, Email account already exist");
                        return View(complainthandler);
                    }
                }
                else
                {
                    return View(complainthandler);
                }
                // TODO: Add insert logic here
            }
            catch (Exception e)
            {
                return View(complainthandler);
            }
        }

        // GET: ComplaintHandler/Edit/5
        [CustomAuthorize(UserRole.ADMIN)]
        public ActionResult Edit(int id)
        {
            ViewData["DepartmentList"] = dbContext.Departments.ToList();
            ViewData["SupervisorList"] = dbContext.AccountHolders.Where(s => s.Role == UserRole.SUPERVISOR.ToString()).ToList();
            var ch = dbContext.ComplaintHandlers.ToList().Where(s => s.ID == id).FirstOrDefault();
            return View(ch);
        }

        // POST: ComplaintHandler/Edit/5
        [CustomAuthorize(UserRole.ADMIN)]
        [HttpPost]
        public ActionResult Edit(int id, ComplaintHandler complinthandler)
        {
            try
            {
                ViewData["DepartmentList"] = dbContext.Departments.ToList();
                ViewData["SupervisorList"] = dbContext.AccountHolders.Where(s => s.Role == UserRole.SUPERVISOR.ToString()).ToList();
                // TODO: Add update logic here
                if (ModelState.IsValid)
                {
                    var accountHolder = dbContext.AccountHolders.ToList().Where(s => s.Email == complinthandler.AccountHolder.Email && s.ID != id).FirstOrDefault();
                    if (accountHolder == null)
                    {
                        
                        if (complinthandler.AccountHolder.Name == null || complinthandler.AccountHolder.Email == null || complinthandler.AccountHolder.Password == null)
                        {
                            ModelState.AddModelError("Edit Error", "Edit Error, Please fulfill all the details");
                            return View(complinthandler);
                        }
                        else
                        {
                            var supervisor = dbContext.Supervisors.ToList().Where(r => r.DepartmentID == complinthandler.DepartmentID).FirstOrDefault();
                            using (System.Data.Entity.DbContextTransaction trans = dbContext.Database.BeginTransaction())
                            {
                                var ch = dbContext.ComplaintHandlers.ToList().Where(s => s.ID == id).FirstOrDefault();
                                ch.AccountHolder.Name = complinthandler.AccountHolder.Name;
                                ch.AccountHolder.Email = complinthandler.AccountHolder.Email;
                                ch.AccountHolder.Password = complinthandler.AccountHolder.Password;
                                ch.DepartmentID = complinthandler.DepartmentID;
                                ch.SupervisorID = supervisor.ID;
                                dbContext.SaveChanges();
                                trans.Commit();
                            }

                            return RedirectToAction("Index");
                        }
                            
                    }
                    else
                    {
                        ModelState.AddModelError("Create Error", "Edit Error, Email account already exist");
                        return View(complinthandler);
                    }
                }
                else
                {
                    return View(complinthandler);
                }
            }
            catch
            {
                return View();
            }
        }

        // GET: ComplaintHandler/Delete/5
        [CustomAuthorize(UserRole.ADMIN)]
        public ActionResult Delete(int id)
        {
            var ch = dbContext.ComplaintHandlers.ToList().Where(s => s.ID == id).FirstOrDefault();
            return View(ch);
        }

        // POST: ComplaintHandler/Delete/5
        [CustomAuthorize(UserRole.ADMIN)]
        [HttpPost]
        public ActionResult Delete(int id, ComplaintHandler complainthandler)
        {
            try
            {
                // TODO: Add delete logic here
                if (ModelState.IsValid)
                {
                    using (System.Data.Entity.DbContextTransaction trans = dbContext.Database.BeginTransaction())
                    {
                        var ch = dbContext.ComplaintHandlers.ToList().Where(s => s.ID == id).FirstOrDefault();
                        dbContext.AccountHolders.Remove(ch.AccountHolder);
                        dbContext.ComplaintHandlers.Remove(ch);
                        dbContext.SaveChanges();
                        trans.Commit();
                    }
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(complainthandler);
                }
            }
            catch (Exception e)
            {
                return View();
            }
        }
    }
}
