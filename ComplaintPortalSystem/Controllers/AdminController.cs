using ComplaintPortalSystem.Models;
using ComplaintPortalSystem.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ComplaintPortalSystem.Controllers
{
    public class AdminController : Controller
    {
        ComplaintPortalDBEntities dbContext = new ComplaintPortalDBEntities();
        
        [CustomAuthorize(UserRole.ADMIN)]
        public ActionResult Index()
        {
            return View(dbContext.AccountHolders.ToList());
        }

        [CustomAuthorize(UserRole.ADMIN)]
        public ActionResult RegisterIndex()
        {
            return View();
        }

        [CustomAuthorize(UserRole.ADMIN)]
        public ActionResult DataMigrateIndex()
        {
            return View();
        }

        [CustomAuthorize(UserRole.ADMIN)]
        public ActionResult Details(int id)
        {

            var adm = dbContext.AccountHolders.ToList().Where(s => s.ID == id).FirstOrDefault();
            return View(adm);
        }

        [CustomAuthorize(UserRole.ADMIN)]
        public ActionResult Create()
        {
            return View();
        }

        [CustomAuthorize(UserRole.ADMIN)]
        [HttpPost]
        public ActionResult Create(AccountHolder admin)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var accountHolder = dbContext.AccountHolders.ToList().Where(s => s.Email == admin.Email).FirstOrDefault();
                    if(accountHolder == null)
                    {
                        if (admin.Name == null || admin.Email == null || admin.Password == null)
                        {
                            ModelState.AddModelError("Create Error", "Create Error, Please fulfill all the details");
                            return View(admin);
                        }
                        else
                        {
                            admin.Role = UserRole.ADMIN.ToString();
                            dbContext.AccountHolders.Add(admin);
                            dbContext.SaveChanges();
                            return RedirectToAction("Index");

                        }  
                    }
                    else
                    {
                        ModelState.AddModelError("Create Error", "Create Error, Email account already exist");
                        return View(accountHolder);
                    }                
                }
                else
                {
                    return View(admin);
                }
            }
            catch (Exception e)
            {
                return View(admin);
            }
        }

        [CustomAuthorize(UserRole.ADMIN)]
        public ActionResult Edit(int id)
        {
            var adm = dbContext.AccountHolders.ToList().Where(s => s.ID == id).FirstOrDefault();
            return View(adm);
        }

        [CustomAuthorize(UserRole.ADMIN)]
        [HttpPost]
        public ActionResult Edit(int id, AccountHolder admin)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var accountHolder = dbContext.AccountHolders.ToList().Where(s => s.Email == admin.Email && s.ID != id).FirstOrDefault();
                    var adm = dbContext.AccountHolders.ToList().Where(s => s.ID == id).FirstOrDefault();
                    if (accountHolder == null)
                    {
                        if (admin.Name == null || admin.Email == null || admin.Password == null)
                        {
                            ModelState.AddModelError("Edit Error", "Edit Error, Please fulfill all the details");
                            return View(admin);
                        }
                        else
                        {
                            adm.Email = admin.Email;
                            adm.Name = admin.Name;
                            adm.Password = admin.Password;
                            dbContext.SaveChanges();
                            return RedirectToAction("Index");
                        }   
                    }
                    else
                    {
                        ModelState.AddModelError("Edit Error", "Edit Error, Email account already exist");
                        return View(accountHolder);
                    }
                }
                else
                {
                    return View(admin);
                }               
            }
            catch
            {
                return View();
            }
        }

        [CustomAuthorize(UserRole.ADMIN)]
        public ActionResult Delete(int id)
        {
            var adm = dbContext.AccountHolders.ToList().Where(s => s.ID == id).FirstOrDefault();
            return View(adm);
        }

        [CustomAuthorize(UserRole.ADMIN)]
        [HttpPost]
        public ActionResult Delete(int id, AccountHolder admin)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var adm = dbContext.AccountHolders.ToList().Where(s => s.ID == id).FirstOrDefault();
                    dbContext.AccountHolders.Remove(adm);
                    dbContext.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(admin);
                }
            }
            catch(Exception e)
            {
                return View();
            }
        }
    }
}
