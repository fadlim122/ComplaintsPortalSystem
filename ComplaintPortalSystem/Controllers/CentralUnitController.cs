using ComplaintPortalSystem.Common;
using ComplaintPortalSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ComplaintPortalSystem.Controllers
{
    public class CentralUnitController : Controller
    {
        ComplaintPortalDBEntities dbContext = new ComplaintPortalDBEntities();
        // GET: CentralUnit
        [CustomAuthorize(UserRole.ADMIN)]
        public ActionResult Index()
        {
            return View(dbContext.AccountHolders.ToList());
        }

        // GET: CentralUnit/Details/5
        [CustomAuthorize(UserRole.ADMIN)]
        public ActionResult Details(int id)
        {
            var cu = dbContext.AccountHolders.ToList().Where(s => s.ID == id).FirstOrDefault();
            return View(cu);
        }

        // GET: CentralUnit/Create
        [CustomAuthorize(UserRole.ADMIN)]
        public ActionResult Create()
        {
            return View();
        }

        // POST: CentralUnit/Create
        [CustomAuthorize(UserRole.ADMIN)]
        [HttpPost]
        public ActionResult Create(AccountHolder centralunit)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var accountHolder = dbContext.AccountHolders.ToList().Where(s => s.Email == centralunit.Email).FirstOrDefault();
                    if (accountHolder == null)
                    {

                        if (centralunit.Name == null || centralunit.Email == null || centralunit.Password == null)
                        {
                            ModelState.AddModelError("Create Error", "Create Error, Please fulfill all the details");
                            return View(centralunit);
                        }
                        else
                        {
                            centralunit.Role = UserRole.CENTRAL_UNIT.ToString();
                            dbContext.AccountHolders.Add(centralunit);
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
                    return View(centralunit);
                }
                // TODO: Add insert logic here
            }
            catch (Exception e)
            {
                return View(centralunit);
            }
        }

        // GET: CentralUnit/Edit/5
        [CustomAuthorize(UserRole.ADMIN)]
        public ActionResult Edit(int id)
        {
            var cu = dbContext.AccountHolders.ToList().Where(s => s.ID == id).FirstOrDefault();
            return View(cu);
        }

        // POST: CentralUnit/Edit/5
        [CustomAuthorize(UserRole.ADMIN)]
        [HttpPost]
        public ActionResult Edit(int id, AccountHolder centralunit)
        {
            try
            {
                // TODO: Add update logic here
                if (ModelState.IsValid)
                {
                    var accountHolder = dbContext.AccountHolders.ToList().Where(s => s.Email == centralunit.Email && s.ID != id).FirstOrDefault();
                    if (accountHolder == null)
                    {
                        if (centralunit.Name == null || centralunit.Email == null || centralunit.Password == null)
                        {
                            ModelState.AddModelError("Edit Error", "Edit Error, Please fulfill all the details");
                            return View(centralunit);
                        }
                        else
                        {
                            var cu = dbContext.AccountHolders.ToList().Where(s => s.ID == id).FirstOrDefault();
                            cu.Email = centralunit.Email;
                            cu.Name = centralunit.Name;
                            cu.Password = centralunit.Password;
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
                    return View(centralunit);
                }

            }
            catch
            {
                return View();
            }
        }

        // GET: CentralUnit/Delete/5
        [CustomAuthorize(UserRole.ADMIN)]
        public ActionResult Delete(int id)
        {
            var cu = dbContext.AccountHolders.ToList().Where(s => s.ID == id).FirstOrDefault();
            return View(cu);
        }

        // POST: CentralUnit/Delete/5
        [CustomAuthorize(UserRole.ADMIN)]
        [HttpPost]
        public ActionResult Delete(int id, AccountHolder centralunit)
        {
            try
            {
                // TODO: Add delete logic here
                if (ModelState.IsValid)
                {
                    var cu = dbContext.AccountHolders.ToList().Where(s => s.ID == id).FirstOrDefault();
                    dbContext.AccountHolders.Remove(cu);
                    dbContext.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(centralunit);
                }
            }
            catch (Exception e)
            {
                return View();
            }
        }
    }
}
