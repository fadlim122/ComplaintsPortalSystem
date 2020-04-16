using ComplaintPortalSystem.Common;
using ComplaintPortalSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ComplaintPortalSystem.Controllers
{
    public class CategoryController : Controller
    {
        ComplaintPortalDBEntities dbContext = new ComplaintPortalDBEntities();
        // GET: Category

        [CustomAuthorize(UserRole.ADMIN)]
        public ActionResult Index()
        {
            return View(dbContext.Categories.ToList());
        }

        // GET: Category/Details/5
        [CustomAuthorize(UserRole.ADMIN)]
        public ActionResult Details(int id)
        {
            var ctgry = dbContext.Categories.ToList().Where(s => s.ID == id).FirstOrDefault();
            return View(ctgry);
        }

        // GET: Category/Create
        [CustomAuthorize(UserRole.ADMIN)]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Category/Create
        [CustomAuthorize(UserRole.ADMIN)]
        [HttpPost]
        public ActionResult Create(Category category)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var create = dbContext.Categories.ToList().Where(s => s.CategoryDescription == category.CategoryDescription).FirstOrDefault();
                    if (create == null)
                    {
                        if(category.CategoryDescription == null)
                        {
                            ModelState.AddModelError("Create Error", "Create Error, Please fulfill all the details");
                            return View(category);
                        }
                        else
                        {
                            dbContext.Categories.Add(category);
                            dbContext.SaveChanges();
                            return RedirectToAction("Index");
                        }
                        
                    }
                    else
                    {
                        ModelState.AddModelError("Create Error", "Create Error, Category already exist");
                        return View(create);
                    }
                }
                else
                {
                    return View(category);
                }
                // TODO: Add insert logic here
            }
            catch (Exception e)
            {
                return View(category);
            }
        }

        // GET: Category/Edit/5
        [CustomAuthorize(UserRole.ADMIN)]
        public ActionResult Edit(int id)
        {
            var ctgry = dbContext.Categories.ToList().Where(s => s.ID == id).FirstOrDefault();
            return View(ctgry);
        }

        // POST: Category/Edit/5
        [CustomAuthorize(UserRole.ADMIN)]
        [HttpPost]
        public ActionResult Edit(int id, Category category)
        {
            try
            {
                // TODO: Add update logic here
                if (ModelState.IsValid)
                {
                    var create = dbContext.Categories.ToList().Where(s => s.CategoryDescription == category.CategoryDescription).FirstOrDefault();
                    if (create == null)
                    {
                        if (category.CategoryDescription == null)
                        {
                            ModelState.AddModelError("Edit Error", "Edit Error, Please fulfill all the details");
                            return View(category);
                        }
                        else
                        {
                            using (System.Data.Entity.DbContextTransaction trans = dbContext.Database.BeginTransaction())
                            {
                                var ctgry = dbContext.Categories.ToList().Where(s => s.ID == id).FirstOrDefault();
                                ctgry.CategoryDescription = category.CategoryDescription;
                                dbContext.SaveChanges();
                                trans.Commit();
                            }
                            return RedirectToAction("Index");
                        }
                            
                    }
                    else
                    {
                        ModelState.AddModelError("Edit Error", "Edit Error, Category already exist");
                        return View(create);
                    }
                }
                else
                {
                    return View(category);
                }

            }
            catch
            {
                return View();
            }
        }

        // GET: Category/Delete/5
        [CustomAuthorize(UserRole.ADMIN)]
        public ActionResult Delete(int id)
        {
            var ctgry = dbContext.Categories.ToList().Where(s => s.ID == id).FirstOrDefault();
            return View(ctgry);
        }

        // POST: Category/Delete/5
        [CustomAuthorize(UserRole.ADMIN)]
        [HttpPost]
        public ActionResult Delete(int id, Category category)
        {
            try
            {
                // TODO: Add delete logic here
                if (ModelState.IsValid)
                {
                    var complaintWithSelectedCategory = dbContext.Complaints.ToList().Where(s => s.CategoryID == id).FirstOrDefault();     
                    var ctgry = dbContext.Categories.ToList().Where(s => s.ID == id).FirstOrDefault();
                    if (complaintWithSelectedCategory == null)
                    {
                        dbContext.Categories.Remove(ctgry);
                        dbContext.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("Delete Error", "Category cannot be deleted because it's used by other complaints");
                        return View(ctgry);
                    }
                        
                }
                else
                {
                    return View(category);
                }
            }
            catch (Exception e)
            {
                return View();
            }
        }
    }
}
