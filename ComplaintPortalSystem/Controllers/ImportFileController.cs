using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ComplaintPortalSystem.Common;
using ComplaintPortalSystem.Models;

namespace ComplaintPortalSystem.Controllers
{
    public class ImportFileController : Controller
    {
        ComplaintPortalDBEntities dbContext = new ComplaintPortalDBEntities();
        // GET: ImportFile

        [CustomAuthorize(UserRole.ADMIN)]
        public ActionResult Index()
        {
            return View();
        }

        public bool CheckIsEmpty(string input)
        {
            if(string.IsNullOrEmpty(input) == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        [CustomAuthorize(UserRole.ADMIN)]
        [HttpPost]
        public ActionResult Index(HttpPostedFileBase file)
        {
            string filePath = string.Empty;
            string errorResult = string.Empty;
            int rollback = 0;
            if (file != null)
            {
                string path = Server.MapPath("~/Uploads/");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                filePath = path + Path.GetFileName(file.FileName);
                file.SaveAs(filePath);

                var excel = new ExcelPackage(file.InputStream);
                var dt = excel.ToDataTable();
                foreach (DataRow row in dt.Rows)
                {
                    using (System.Data.Entity.DbContextTransaction trans = dbContext.Database.BeginTransaction())
                    {
                        try
                        {
                            string name = Convert.ToString(row["Name"]);
                            string email = Convert.ToString(row["Email"]);
                            string password = Convert.ToString(row["Password"]);
                            string role = Convert.ToString(row["Role"]);

                            var accountHolder = dbContext.AccountHolders.ToList().Where(s => s.Email == email).FirstOrDefault();
                            AccountHolder account = new AccountHolder();

                            if (accountHolder == null)
                            {
                                if (CheckIsEmpty(name) != true && CheckIsEmpty(email) != true && CheckIsEmpty(password) != true && CheckIsEmpty(role) != true)
                                {
                                    account.Name = name;
                                    account.Email = email;
                                    account.Password = password;
                                    account.Role = role;
                                    dbContext.AccountHolders.Add(account);
                                    dbContext.SaveChanges();

                                    if (role == UserRole.STUDENT.ToString())
                                    {
                                        Student student = new Student();
                                        string major = Convert.ToString(row["Major"]);
                                        if (CheckIsEmpty(major) != true)
                                        {
                                            student.Major = major;
                                            student.ID = account.ID;
                                            dbContext.Students.Add(student);
                                            dbContext.SaveChanges();
                                            trans.Commit();
                                        }
                                        else
                                        {
                                            rollback++;
                                            trans.Rollback();
                                        }

                                    }
                                    else if (role == UserRole.STAFF.ToString())
                                    {
                                        Staff staff = new Staff();
                                        string jobDesignation = Convert.ToString(row["JobDesignation"]);
                                        if (CheckIsEmpty(jobDesignation) != true)
                                        {
                                            staff.JobDesignation = jobDesignation;
                                            staff.ID = account.ID;
                                            dbContext.Staffs.Add(staff);
                                            dbContext.SaveChanges();
                                            trans.Commit();
                                        }
                                        else
                                        {
                                            rollback++;
                                            trans.Rollback();
                                        }

                                    }
                                    else if (role == UserRole.SUPERVISOR.ToString())
                                    {
                                        Supervisor supervisor = new Supervisor();
                                        int departmentID = Convert.ToInt32(row["DepartmentID"]);
                                        supervisor.DepartmentID = departmentID;
                                        supervisor.ID = account.ID;
                                        dbContext.Supervisors.Add(supervisor);
                                        dbContext.SaveChanges();
                                        trans.Commit();
                                    }
                                    else if (role == UserRole.COMPLAINT_HANDLER.ToString())
                                    {
                                        ComplaintHandler handler = new ComplaintHandler();
                                        int departmentID = Convert.ToInt32(row["DepartmentID"]);
                                        int supervisorID = Convert.ToInt32(row["SupervisorID"]);
                                        handler.DepartmentID = departmentID;
                                        handler.SupervisorID = supervisorID;
                                        handler.ID = account.ID;
                                        dbContext.ComplaintHandlers.Add(handler);
                                        dbContext.SaveChanges();
                                        trans.Commit();
                                    }
                                    else
                                    {
                                        trans.Commit();
                                    }
                                }
                                else
                                {
                                    rollback++;
                                    trans.Rollback();
                                }



                            }
                            else
                            {
                                errorResult = errorResult + " Duplicate email (" + email + ") , ";
                            }
                        }
                        catch (Exception)
                        {
                            trans.Rollback();
                        }

                    }                 
                }
                string rollbackMsg = string.Empty;
                if (rollback > 0)
                {
                    rollbackMsg = ", and there're some data rollback because of insufficient data.";
                }

                ViewBag.Success = "File Imported and excel data saved into database" + rollbackMsg + " " + errorResult;
                return View();
            }
            else
            {
                ViewBag.Success = "Please choose the file to be imported it into database";
                return View();
            }
            
        }

        [CustomAuthorize(UserRole.ADMIN)]
        public ActionResult ImportDepartment()
        {
            return View();
        }

        [HttpPost]
        [CustomAuthorize(UserRole.ADMIN)]
        public ActionResult ImportDepartment(HttpPostedFileBase file)
        {
            string filePath = string.Empty;
            string errorResult = string.Empty;
            int rollback = 0;
            if (file != null)
            {
                string path = Server.MapPath("~/Uploads/");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                filePath = path + Path.GetFileName(file.FileName);
                file.SaveAs(filePath);


                var excel = new ExcelPackage(file.InputStream);
                var dt = excel.ToDataTable();

                foreach (DataRow row in dt.Rows)
                {
                    using (System.Data.Entity.DbContextTransaction trans = dbContext.Database.BeginTransaction())
                    {
                        try
                        {
                            string name = Convert.ToString(row["Name"]);
                            string HODName = Convert.ToString(row["HODName"]);
                            string HODEmail = Convert.ToString(row["HODEmail"]);

                            var department = dbContext.Departments.ToList().Where(s => s.Name == name).FirstOrDefault();
                            Department dmt = new Department();
                            if (department == null)
                            {
                                if (CheckIsEmpty(name) != true && CheckIsEmpty(HODName) != true && CheckIsEmpty(HODEmail) != true)
                                {
                                    dmt.Name = name;
                                    dmt.HODName = HODName;
                                    dmt.HODEmail = HODEmail;
                                    dbContext.Departments.Add(dmt);
                                    dbContext.SaveChanges();
                                    trans.Commit();
                                }
                                else
                                {
                                    rollback++;
                                    trans.Rollback();
                                }


                            }
                            else
                            {
                                errorResult = errorResult + " Duplicate name (" + name + ") , ";
                            }
                        }
                        catch (Exception)
                        {
                            trans.Rollback();
                        }
                    } 
                }
                string rollbackMsg = string.Empty;
                if (rollback > 0)
                {
                    rollbackMsg = ", and there're some data rollback because of insufficient data.";
                }
                //Environment.NewLine
                ViewBag.Success = "File Imported and excel data saved into database" + rollbackMsg + " " + errorResult;
                return View();
            }
            else
            {
                ViewBag.Success = "Please choose the file to be imported it into database";
                return View();
            }
            
        }

        [CustomAuthorize(UserRole.ADMIN)]
        public ActionResult ImportCategory()
        {
            return View();
        }

        [CustomAuthorize(UserRole.ADMIN)]
        [HttpPost]
        public ActionResult ImportCategory(HttpPostedFileBase file)
        {
            string filePath = string.Empty;
            string errorResult = string.Empty;
            int rollback = 0;

            if (file != null)
            {
                string path = Server.MapPath("~/Uploads/");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                filePath = path + Path.GetFileName(file.FileName);
                file.SaveAs(filePath);


                var excel = new ExcelPackage(file.InputStream);
                var dt = excel.ToDataTable();

                foreach (DataRow row in dt.Rows)
                {

                    using (System.Data.Entity.DbContextTransaction trans = dbContext.Database.BeginTransaction())
                    {
                        try
                        {
                            string CategoryDescription = Convert.ToString(row["CategoryDescription"]);

                            var category = dbContext.Categories.ToList().Where(s => s.CategoryDescription == CategoryDescription).FirstOrDefault();
                            Category ctgry = new Category();
                            if (category == null)
                            {

                                if (CheckIsEmpty(CategoryDescription) != true)
                                {
                                    ctgry.CategoryDescription = CategoryDescription;
                                    dbContext.Categories.Add(ctgry);
                                    dbContext.SaveChanges();
                                    trans.Commit();
                                }
                                else
                                {
                                    rollback++;
                                    trans.Rollback();
                                }


                            }
                            else
                            {
                                errorResult = errorResult + " Duplicate name (" + CategoryDescription + ") , ";
                            }
                        }
                        catch (Exception)
                        {
                            trans.Rollback();
                        }

                    }

                    
                }
                string rollbackMsg = string.Empty;
                if (rollback > 0)
                {
                    rollbackMsg = ", and there're some data rollback because of insufficient data.";
                }
                ViewBag.Success = "File Imported and excel data saved into database" + rollbackMsg + " " + errorResult;
                return View();
            }
            else
            {
                ViewBag.Success = "Please choose the file to be imported it into database";
                return View();
            }
            
        }
    }
}