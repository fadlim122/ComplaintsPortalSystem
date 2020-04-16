using ComplaintPortalSystem.Common;
using ComplaintPortalSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ComplaintPortalSystem.Controllers
{
    public class ExternalAgencyController : Controller
    {
        ComplaintPortalDBEntities dbContext = new ComplaintPortalDBEntities();
        // GET: ExternalAgency

        [CustomAuthorize(UserRole.ADMIN, UserRole.COMPLAINT_HANDLER)]
        public ActionResult Index()
        {
            return View(dbContext.ExternalAgencies.ToList());
        }

        [CustomAuthorize(UserRole.ADMIN, UserRole.COMPLAINT_HANDLER)]
        // GET: ExternalAgency/Details/5
        public ActionResult Details(int id)
        {
            var externalAgency = dbContext.ExternalAgencies.ToList().Where(s => s.ID == id).FirstOrDefault();
            return View(externalAgency);
        }

        [CustomAuthorize(UserRole.ADMIN, UserRole.COMPLAINT_HANDLER)]
        // GET: ExternalAgency/Create
        public ActionResult Create()
        {
            return View();
        }

        [CustomAuthorize(UserRole.ADMIN, UserRole.COMPLAINT_HANDLER)]
        // POST: ExternalAgency/Create
        [HttpPost]
        public ActionResult Create(ExternalAgency externalAgency)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var external = dbContext.ExternalAgencies.ToList().Where(s => s.CompanyName == externalAgency.CompanyName).FirstOrDefault();
                    if (external == null)
                    {

                        if (externalAgency.CompanyName == null || externalAgency.ContactNumber == 0 || externalAgency.ContactPerson == null || externalAgency.Email == null)
                        {
                            ModelState.AddModelError("Create Error", "Create Error, Please fulfill all the details");
                            return View(externalAgency);
                        }
                        else
                        {
                            dbContext.ExternalAgencies.Add(externalAgency);
                            dbContext.SaveChanges();
                            return RedirectToAction("Index");
                        }   
                    }
                    else
                    {
                        ModelState.AddModelError("Create Error", "Create Error, Agency already exist");
                        return View(external);
                    }
                }
                else
                {
                    return View(externalAgency);
                }
                // TODO: Add insert logic here
            }
            catch (Exception e)
            {
                return View(externalAgency);
            }
        }

        [CustomAuthorize(UserRole.ADMIN, UserRole.COMPLAINT_HANDLER)]
        // GET: ExternalAgency/Edit/5
        public ActionResult Edit(int id)
        {
            var external = dbContext.ExternalAgencies.ToList().Where(s => s.ID == id).FirstOrDefault();
            return View(external);
        }

        [CustomAuthorize(UserRole.ADMIN, UserRole.COMPLAINT_HANDLER)]
        // POST: ExternalAgency/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, ExternalAgency externalAgency)
        {
            try
            {
                var ex = dbContext.ExternalAgencies.ToList().Where(s => s.ID == id).FirstOrDefault();
                // TODO: Add update logic here
                if (ModelState.IsValid)
                {
                    var external = dbContext.ExternalAgencies.ToList().Where(s => s.CompanyName == externalAgency.CompanyName && s.ID != id).FirstOrDefault();
                    if (external == null)
                    {

                        if (externalAgency.CompanyName == null || externalAgency.ContactNumber == 0 || externalAgency.ContactPerson == null || externalAgency.Email == null)
                        {
                            ModelState.AddModelError("Edit Error", "Edit Error, Please fulfill all the details");
                            return View(externalAgency);
                        }
                        else
                        {
                            using (System.Data.Entity.DbContextTransaction trans = dbContext.Database.BeginTransaction())
                            {

                                ex.CompanyName = externalAgency.CompanyName;
                                ex.ContactPerson = externalAgency.ContactPerson;
                                ex.ContactNumber = externalAgency.ContactNumber;
                                ex.Email = externalAgency.Email;
                                dbContext.SaveChanges();
                                trans.Commit();
                            }
                            return RedirectToAction("Index");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("Edit Error", "Edit Error, Agency already exist");
                        return View(external);
                    }
                }
                else
                {
                    return View(externalAgency);
                }

            }
            catch
            {
                return View();
            }
        }

        [CustomAuthorize(UserRole.ADMIN, UserRole.COMPLAINT_HANDLER)]
        // GET: ExternalAgency/Delete/5
        public ActionResult Delete(int id)
        {
            var external = dbContext.ExternalAgencies.ToList().Where(s => s.ID == id).FirstOrDefault();
            return View(external);
        }

        [CustomAuthorize(UserRole.ADMIN, UserRole.COMPLAINT_HANDLER)]
        // POST: ExternalAgency/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, ExternalAgency externalAgency)
        {
            try
            {
                // TODO: Add delete logic here
                if (ModelState.IsValid)
                {
                    var ex = dbContext.ExternalAgencies.ToList().Where(s => s.ID == id).FirstOrDefault();
                    dbContext.ExternalAgencies.Remove(ex);
                    dbContext.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(externalAgency);
                }
            }
            catch (Exception e)
            {
                return View();
            }
        }
    }
}
