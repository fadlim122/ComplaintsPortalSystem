using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ComplaintPortalSystem.Models;
using System.Net;
using System.Net.Mail;
using ComplaintPortalSystem.Common;
using System.Configuration;

namespace ComplaintPortalSystem.Controllers
{
    public class EmailController : Controller
    {
        ComplaintPortalDBEntities dbContext = new ComplaintPortalDBEntities();

        [CustomAuthorize(UserRole.CENTRAL_UNIT, UserRole.COMPLAINT_HANDLER)]
        public ActionResult Index()
        {
            return View();
        }

        [CustomAuthorize(UserRole.CENTRAL_UNIT, UserRole.COMPLAINT_HANDLER)]
        [HttpPost]
        public ActionResult Index(ComplaintPortalSystem.Models.Email model)
        {
            // central unit send email to complainant

            //var centralUnit = dbContext.AccountHolders.ToList().Where(r => r.ID == UserRole.CENTRAL_UNIT);
            //MailMessage mm = new MailMessage(, model.To);
            string sender = ConfigurationManager.AppSettings["Email.Sender"];
            string password = ConfigurationManager.AppSettings["Email.Password"];

            MailMessage mm = new MailMessage(sender, model.To);
            mm.Subject = model.Subject;
            mm.Body = model.Body;
            mm.IsBodyHtml = false;

            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.EnableSsl = true;

            NetworkCredential nc = new NetworkCredential(sender, password);
            smtp.UseDefaultCredentials = true;
            smtp.Credentials = nc;
            smtp.Send(mm);
            if(UserSession.Role == UserRole.COMPLAINT_HANDLER.ToString())
            {
                ViewBag.Message = "Email Has Been Sent Successfully!";
            }
            else
            {
                ViewBag.Message = "Email Has Been Sent Successfully! Please remember to close the case if the issue has been resolved.";
            }
            return View();
        }
    }
}