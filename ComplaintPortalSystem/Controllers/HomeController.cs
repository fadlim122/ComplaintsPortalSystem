using ComplaintPortalSystem.Common;
using ComplaintPortalSystem.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Security;

namespace ComplaintPortalSystem.Controllers
{
    public class HomeController : Controller
    {
       

        ComplaintPortalDBEntities dbContext = new ComplaintPortalDBEntities();
        public ActionResult Index()
        {
           
            if (User.Identity.IsAuthenticated && UserSession.Role != null)
            {
                var account = dbContext.AccountHolders.ToList().Where(s => s.ID == UserSession.UserId).FirstOrDefault();
                var loginCount = dbContext.AccountLogs.Where(s => s.AccountID == UserSession.UserId && s.Timestamp.Year == DateTime.Now.Year && s.Timestamp.Month == DateTime.Now.Month && s.Timestamp.Day == DateTime.Now.Day && s.Type == "LOGIN").ToList().Count();
 
                if (UserSession.Role == UserRole.COMPLAINT_HANDLER.ToString())
                {
                    if(loginCount == 1)
                    {
                        var obj = dbContext.GetCaseNumberByUser(account.ID);
                        foreach (GetCaseNumberByUser_Result item in obj)
                        {
                            ViewData["TOTAL_CLOSED_CASE"] = item.TOTAL_CLOSED_CASE;
                            ViewData["THIS_MONTH_CLOSED_CASE"] = item.THIS_MONTH_CLOSED_CASE;
                            ViewData["TOTAL_OPEN_CASE"] = item.TOTAL_OPEN_CASE;
                            ViewData["THIS_WEEK_CLOSED_CASE"] = item.THIS_WEEK_CLOSED_CASE;
                        }
                    }
                    ViewData["LoginCount"] = loginCount;
                }
                return View(account);

            }
            else
            {
                return View();
            }

        }

        public ActionResult About(int id)
        {
            var account = dbContext.AccountHolders.ToList().Where(s => s.ID == id).FirstOrDefault();
            return View(account);
        }

        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(String email, string password)
        {
            string path = (HttpContext.Server.MapPath("~/Log/Log.txt")); 
            //AccountHolder account = new AccountHolder();
            //var acc = dbContext.AccountHolders.ToList().Where(s)
            using (SqlConnection cs = DatabaseHelper.GetDatabaseConnection())
            {
                cs.Open();
                SqlCommand cmd = new SqlCommand();


                var accountHolder = dbContext.AccountHolders.ToList().Where(s => s.Email.ToLower() == email.ToLower() && s.Password == password).FirstOrDefault();
                if (accountHolder != null)
                {
                    cmd.Connection = cs;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "SetAccountLog";
                    cmd.Parameters.AddWithValue("@AccountID", accountHolder.ID);
                    cmd.Parameters.AddWithValue("@COMMAND", 0);
                    cmd.ExecuteNonQuery();
                    
                    FileInfo info = new FileInfo(path);
                    if (!info.Exists)
                    {
                        using (StreamWriter writer = info.CreateText())
                        {
                            writer.WriteLine(accountHolder.ID+", LOGIN, "+DateTime.Now);
                        }
                    }
                    else
                    {
                        using (StreamWriter writer = info.AppendText())
                        {
                            writer.WriteLine(accountHolder.ID + ", LOGIN, " + DateTime.Now);
                        }
                    }

                    FormsAuthentication.SetAuthCookie(accountHolder.ID.ToString(), false);
                    UserSession.CreateUserSession(accountHolder.ID, accountHolder.Name, accountHolder.Email, accountHolder.Role);
                    return Redirect("~/Home/Index");
                }
                else
                {
                    ModelState.AddModelError("Login Error", "Login Error, The account does not exist");
                    return View("Login");
                }
            }

            
        }

        [HttpGet]
        public ActionResult Logout()
        {
            string path = (HttpContext.Server.MapPath("~/Log/Log.txt"));
            using (SqlConnection cs = DatabaseHelper.GetDatabaseConnection())
            {   
                cs.Open();
                SqlCommand cmd = new SqlCommand();

                cmd.Connection = cs;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SetAccountLog";
                cmd.Parameters.AddWithValue("@AccountID", UserSession.UserId);
                cmd.Parameters.AddWithValue("@COMMAND", 1);
                cmd.ExecuteNonQuery();

                FileInfo info = new FileInfo(path);
                if (!info.Exists)
                {
                    using (StreamWriter writer = info.CreateText())
                    {
                        writer.WriteLine(UserSession.UserId + ", LOGOUT, " + DateTime.Now);

                    }
                }
                else
                {
                    using (StreamWriter writer = info.AppendText())
                    {
                        writer.WriteLine(UserSession.UserId + ", LOGOUT, " + DateTime.Now);
                    }
                }


                FormsAuthentication.SignOut();
                Session.Abandon();

                // clear authentication cookie
                HttpCookie cookie1 = new HttpCookie(FormsAuthentication.FormsCookieName, "");
                cookie1.Expires = DateTime.Now.AddYears(-1);
                Response.Cookies.Add(cookie1);

                return RedirectToAction("Login", "Home");

            }
        }


    }
}