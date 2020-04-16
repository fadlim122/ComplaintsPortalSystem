using ComplaintPortalSystem.Common;
using ComplaintPortalSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ComplaintPortalSystem.Controllers
{
    public class StudentController : Controller
    {
        ComplaintPortalDBEntities dbContext = new ComplaintPortalDBEntities();
        // GET: Student

        [CustomAuthorize(UserRole.ADMIN)]
        public ActionResult Index()
        {
            return View(dbContext.Students.ToList());
        }

        // GET: Student/Details/5
        [CustomAuthorize(UserRole.ADMIN)]
        public ActionResult Details(int id)
        {
            var std = dbContext.Students.ToList().Where(s => s.ID == id).FirstOrDefault();
            return View(std);
        }

        // GET: Student/Create
        [CustomAuthorize(UserRole.ADMIN)]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Student/Create
        [CustomAuthorize(UserRole.ADMIN)]
        [HttpPost]
        public ActionResult Create(Student student)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var accountHolder = dbContext.AccountHolders.ToList().Where(s => s.Email == student.AccountHolder.Email).FirstOrDefault();
                    if (accountHolder == null)
                    {

                        if (student.AccountHolder.Name == null || student.AccountHolder.Email == null || student.AccountHolder.Password == null || student.Major == null)
                        {
                            ModelState.AddModelError("Create Error", "Create Error, Please fulfill all the details");
                            return View(student);
                        }
                        else
                        {
                            using (System.Data.Entity.DbContextTransaction trans = dbContext.Database.BeginTransaction())
                            {
                                student.AccountHolder.Role = UserRole.STUDENT.ToString();
                                dbContext.AccountHolders.Add(student.AccountHolder);
                                dbContext.SaveChanges();
                                student.ID = student.AccountHolder.ID;
                                dbContext.Students.Add(student);
                                dbContext.SaveChanges();
                                trans.Commit();
                            }

                            return RedirectToAction("Index");
                        }  
                    }
                    else
                    {
                        ModelState.AddModelError("Create Error", "Create Error, Email account already exist");
                        return View(student);
                    }
                }
                else
                {
                    return View(student);
                }
                // TODO: Add insert logic here
            }
            catch (Exception e)
            {
                return View(student);
            }
        }

        // GET: Student/Edit/5
        [CustomAuthorize(UserRole.ADMIN)]
        public ActionResult Edit(int id)
        {
            var std = dbContext.Students.ToList().Where(s => s.ID == id).FirstOrDefault();
            return View(std);
        }

        // POST: Student/Edit/5
        [CustomAuthorize(UserRole.ADMIN)]
        [HttpPost]
        public ActionResult Edit(int id, Student student)
        {
            try
            {
                // TODO: Add update logic here
                if (ModelState.IsValid)
                {
                    var accountHolder = dbContext.AccountHolders.ToList().Where(s => s.Email == student.AccountHolder.Email && s.ID != id).FirstOrDefault();
                    if (accountHolder == null)
                    {

                        if (student.AccountHolder.Name == null || student.AccountHolder.Email == null || student.AccountHolder.Password == null || student.Major == null)
                        {
                            ModelState.AddModelError("Edit Error", "Edit Error, Please fulfill all the details");
                            return View(student);
                        }
                        else
                        {
                            using (System.Data.Entity.DbContextTransaction trans = dbContext.Database.BeginTransaction())
                            {
                                var std = dbContext.Students.ToList().Where(s => s.ID == id).FirstOrDefault();
                                std.AccountHolder.Name = student.AccountHolder.Name;
                                std.AccountHolder.Email = student.AccountHolder.Email;
                                std.AccountHolder.Password = student.AccountHolder.Password;
                                std.Major = student.Major;
                                dbContext.SaveChanges();
                                trans.Commit();
                            }

                            return RedirectToAction("Index");
                        }
                            
                    }
                    else
                    {
                        ModelState.AddModelError("Create Error", "Edit Error, Email already exist");
                        return View(student);
                    }     
                }
                else
                {
                    return View(student);
                }

            }
            catch
            {
                return View();
            }
        }

        // GET: Student/Delete/5
        [CustomAuthorize(UserRole.ADMIN)]
        public ActionResult Delete(int id)
        {
            var std = dbContext.Students.ToList().Where(s => s.ID == id).FirstOrDefault();
            return View(std);
        }

        // POST: Student/Delete/5
        [CustomAuthorize(UserRole.ADMIN)]
        [HttpPost]
        public ActionResult Delete(int id, Student student)
        {
            try
            {
                // TODO: Add delete logic here
                if (ModelState.IsValid)
                {
                    using (System.Data.Entity.DbContextTransaction trans = dbContext.Database.BeginTransaction())
                    {
                        var std = dbContext.Students.ToList().Where(s => s.ID == id).FirstOrDefault();
                        dbContext.AccountHolders.Remove(std.AccountHolder);
                        dbContext.Students.Remove(std);
                        dbContext.SaveChanges();
                        trans.Commit();
                    }
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(student);
                }
            }
            catch (Exception e)
            {
                return View();
            }
        }
    }
}
