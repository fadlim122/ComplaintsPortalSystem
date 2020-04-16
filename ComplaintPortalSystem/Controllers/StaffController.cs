using ComplaintPortalSystem.Common;
using ComplaintPortalSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ComplaintPortalSystem.Controllers
{
    public class StaffController : Controller
    {
        ComplaintPortalDBEntities dbContext = new ComplaintPortalDBEntities();
        // GET: Staff

        [CustomAuthorize(UserRole.ADMIN)]
        public ActionResult Index()
        {
            return View(dbContext.Staffs.ToList());
        }

        // GET: Staff/Details/5
        [CustomAuthorize(UserRole.ADMIN)]
        public ActionResult Details(int id)
        {
            var stf = dbContext.Staffs.ToList().Where(s => s.ID == id).FirstOrDefault();
            return View(stf);
        }

        // GET: Staff/Create
        [CustomAuthorize(UserRole.ADMIN)]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Staff/Create
        [CustomAuthorize(UserRole.ADMIN)]
        [HttpPost]
        public ActionResult Create(Staff staff)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // error here
                    var accountHolder = dbContext.AccountHolders.ToList().Where(s => s.Email == staff.AccountHolder.Email).FirstOrDefault();
                    if (accountHolder == null)
                    {
                        using (System.Data.Entity.DbContextTransaction trans = dbContext.Database.BeginTransaction())
                        {
                            if (staff.AccountHolder.Name == null || staff.AccountHolder.Email == null || staff.AccountHolder.Password == null || staff.JobDesignation == null)
                            {
                                ModelState.AddModelError("Create Error", "Create Error, Please fulfill all the details");
                                return View(staff);
                            }
                            else
                            {
                                staff.AccountHolder.Role = UserRole.STAFF.ToString();
                                dbContext.AccountHolders.Add(staff.AccountHolder);
                                dbContext.SaveChanges();
                                staff.ID = staff.AccountHolder.ID;
                                dbContext.Staffs.Add(staff);
                                dbContext.SaveChanges();
                                trans.Commit();
                            }
                                
                        }
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("Create Error", "Create Error, Email account already exist");
                        return View(staff);
                    }
                }
                else
                {
                    return View(staff);
                }
                // TODO: Add insert logic here
            }
            catch (Exception e)
            {
                return View(staff);
            }
        }

        // GET: Staff/Edit/5
        [CustomAuthorize(UserRole.ADMIN)]
        public ActionResult Edit(int id)
        {
            var stf = dbContext.Staffs.ToList().Where(s => s.ID == id).FirstOrDefault();
            return View(stf);
        }

        // POST: Staff/Edit/5
        [CustomAuthorize(UserRole.ADMIN)]
        [HttpPost]
        public ActionResult Edit(int id, Staff staff)
        {
            try
            {
                // TODO: Add update logic here
                if (ModelState.IsValid)
                {
                    var accountHolder = dbContext.AccountHolders.ToList().Where(s => s.Email == staff.AccountHolder.Email && s.ID != id).FirstOrDefault();
                    if (accountHolder == null)
                    {
                        using (System.Data.Entity.DbContextTransaction trans = dbContext.Database.BeginTransaction())
                        {
                            var stf = dbContext.Staffs.ToList().Where(s => s.ID == id).FirstOrDefault();
                            if(staff.AccountHolder.Name == null || staff.AccountHolder.Email == null || staff.AccountHolder.Password == null || staff.JobDesignation == null)
                            {
                                ModelState.AddModelError("Edit Error", "Edit Error, Please fulfill all the details");
                                return View(staff);
                            }
                            else
                            {
                                stf.AccountHolder.Name = staff.AccountHolder.Name;
                                stf.AccountHolder.Email = staff.AccountHolder.Email;
                                stf.AccountHolder.Password = staff.AccountHolder.Password;
                                stf.JobDesignation = staff.JobDesignation;
                                dbContext.SaveChanges();
                                trans.Commit();
                            }
                            
                        }

                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("Edit Error", "Edit Error, Email account already exist");
                        return View(staff);
                    }           
                }
                else
                {
                    return View(staff);
                }

            }
            catch
            {
                return View();
            }
        }

        // GET: Staff/Delete/5
        [CustomAuthorize(UserRole.ADMIN)]
        public ActionResult Delete(int id)
        {
            var stf = dbContext.Staffs.ToList().Where(s => s.ID == id).FirstOrDefault();
            return View(stf);
        }

        // POST: Staff/Delete/5
        [CustomAuthorize(UserRole.ADMIN)]
        [HttpPost]
        public ActionResult Delete(int id, Staff staff)
        {
            try
            {
                // TODO: Add delete logic here
                if (ModelState.IsValid)
                {
                    using (System.Data.Entity.DbContextTransaction trans = dbContext.Database.BeginTransaction())
                    {
                        var stf = dbContext.Staffs.ToList().Where(s => s.ID == id).FirstOrDefault();
                        dbContext.AccountHolders.Remove(stf.AccountHolder);
                        dbContext.Staffs.Remove(stf);
                        dbContext.SaveChanges();
                        trans.Commit();
                    }
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(staff);
                }
            }
            catch (Exception e)
            {
                return View();
            }
        }
    }
}
