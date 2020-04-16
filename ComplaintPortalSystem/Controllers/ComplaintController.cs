using ComplaintPortalSystem.Common;
using ComplaintPortalSystem.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace ComplaintPortalSystem.Controllers
{
    public class ComplaintController : Controller
    {
        // GET: Complaint
        ComplaintPortalDBEntities dbContext = new ComplaintPortalDBEntities();

        [CustomAuthorize(UserRole.CENTRAL_UNIT)]
        public ActionResult Index()
        {
            return View(dbContext.Complaints.ToList().Where(s => s.Status != ComplaintStatus.CLOSED));
        }

        [CustomAuthorize(UserRole.CENTRAL_UNIT)]
        public ActionResult ClosedTickets()
        {
            return View(dbContext.Complaints.ToList().Where(s=> s.Status == ComplaintStatus.CLOSED));
        }

        // GET: Complaint/Details/5
        [CustomAuthorize(UserRole.CENTRAL_UNIT)]
        public ActionResult Details(int id)
        {
            var complaint = dbContext.Complaints.ToList().Where(s => s.ID == id).FirstOrDefault();
            return View(complaint);
        }

        [CustomAuthorize(UserRole.CENTRAL_UNIT)]
        public ActionResult DeleteSupervisorAssignment(int supervisorID, int complaintID)
        {
            var c = dbContext.Complaints.ToList().Where(s => s.ID == complaintID).FirstOrDefault();
            try
            {

                // TODO: Add delete logic here
                if (ModelState.IsValid)
                {

                    var supervisorAssignment = dbContext.SupervisorAssignments.ToList().Where(s => s.ComplaintID == complaintID && s.SupervisorID == supervisorID).FirstOrDefault();
                    dbContext.SupervisorAssignments.Remove(supervisorAssignment);
                    dbContext.SaveChanges();
                    //return View(c);
                    return Redirect("Allocate/" + complaintID);
                    //return RedirectToAction("Allocate", "Complaint");//, new { supervisorId = supervisorID, complaintId = complaintID });
                }
                else
                {
                    return View(c);
                }
            }
            catch (Exception e)
            {
                return View(c);
            }
        }

        [CustomAuthorize(UserRole.CENTRAL_UNIT)]
        public ActionResult DeleteComplaintHandlerAssignment(int handlerID, int complaintID)
        {
            var c = dbContext.Complaints.ToList().Where(s => s.ID == complaintID).FirstOrDefault();
            try
            {

                // TODO: Add delete logic here
                if (ModelState.IsValid)
                {

                    var handlerAssignment = dbContext.HandlerAssignments.ToList().Where(s => s.ComplaintID == complaintID && s.HandlerID == handlerID).FirstOrDefault();
                    dbContext.HandlerAssignments.Remove(handlerAssignment);
                    dbContext.SaveChanges();
                    //return View(c);
                    return Redirect("Allocate/" + complaintID);
                    //return RedirectToAction("Allocate", "Complaint");//, new { supervisorId = supervisorID, complaintId = complaintID });
                }
                else
                {
                    return View(c);
                }
            }
            catch (Exception e)
            {
                return View(c);
            }
        }

        [CustomAuthorize(UserRole.CENTRAL_UNIT)]
        public ActionResult Allocate(int id)
        {
            ViewData["IsRedflagList"] = dbContext.Complaints.ToList();
            ViewData["DepartmentList"] = dbContext.Departments.ToList();
            ViewData["ComplaintHandlerList"] = dbContext.AccountHolders.Where(s => s.Role == UserRole.COMPLAINT_HANDLER.ToString()).ToList();
            ViewData["SupervisorList"] = dbContext.AccountHolders.Where(s => s.Role == UserRole.SUPERVISOR.ToString()).ToList();
            var complaint = dbContext.Complaints.ToList().Where(s => s.ID == id).FirstOrDefault();


            List<SelectListItem> li = GetDepartmentList();

            ViewData["SupervisorDepartment"] = li;
            ViewData["HandlerDepartment"] = li;

            return View(complaint);
        }

        public List<SelectListItem> GetDepartmentList()
        {
            List<SelectListItem> li = new List<SelectListItem>();
            var deptList = dbContext.Departments.ToList();
            li.Add(new SelectListItem { Text = "Please choose department", Value = String.Empty });
            foreach (var item in deptList)
            {
                li.Add(new SelectListItem { Text = item.Name, Value = Convert.ToString(item.ID) });
            }

            return li;
        }

        public JsonResult GetSupervisor(int id, bool showAvailableSupervisorOnly)
        {

            List<SelectListItem> li = new List<SelectListItem>();
            if(showAvailableSupervisorOnly == true)
            {
                var supervisorAvailableList = dbContext.Supervisors.Where(m => m.DepartmentID == id && (DateTime.Now < m.StartDate || DateTime.Now > m.EndDate || m.StartDate == null || m.EndDate == null)).ToList();
                foreach (var item in supervisorAvailableList)
                {
                    li.Add(new SelectListItem { Text = item.AccountHolder.Name, Value = Convert.ToString(item.ID) });
                }
            }
            else
            {
                var supervisorList = dbContext.Supervisors.Where(m => m.DepartmentID == id).ToList();
                foreach (var item in supervisorList)
                {
                    li.Add(new SelectListItem { Text = item.AccountHolder.Name, Value = Convert.ToString(item.ID) });
                }
            }
            return Json(li);
        }

        public JsonResult GetComplaintHandler(int id, bool showAvailableHandlerOnly)
        {

            List<SelectListItem> li = new List<SelectListItem>();
            if (showAvailableHandlerOnly == true)
            {
                var handlerAvailableList = dbContext.ComplaintHandlers.Where(m => m.DepartmentID == id && (DateTime.Now < m.StartDate || DateTime.Now > m.EndDate || m.StartDate == null || m.EndDate == null)).ToList();
                foreach (var item in handlerAvailableList)
                {
                    li.Add(new SelectListItem { Text = item.AccountHolder.Name, Value = Convert.ToString(item.ID) });
                }
            }
            else
            {
                var handlerList = dbContext.ComplaintHandlers.Where(m => m.DepartmentID == id).ToList();
                foreach (var item in handlerList)
                {
                    li.Add(new SelectListItem { Text = item.AccountHolder.Name, Value = Convert.ToString(item.ID) });
                }
            }
            return Json(li);
        }


        [CustomAuthorize(UserRole.CENTRAL_UNIT)]
        [HttpPost]
        public ActionResult AllocateHandler(int id, FormCollection form)
        {
            var c = dbContext.Complaints.ToList().Where(s => s.ID == id).FirstOrDefault();
            List<SelectListItem> li = GetDepartmentList();

            ViewData["SupervisorDepartment"] = li;

            ViewData["HandlerDepartment"] = li;
            try
            {
                if (ModelState.IsValid)
                {
                    ViewData["IsRedflagList"] = dbContext.Complaints.ToList();
                    ViewData["DepartmentList"] = dbContext.Departments.ToList();
                    ViewData["ComplaintHandlerList"] = dbContext.AccountHolders.Where(s => s.Role == UserRole.COMPLAINT_HANDLER.ToString()).ToList();
                    ViewData["SupervisorList"] = dbContext.AccountHolders.Where(s => s.Role == UserRole.SUPERVISOR.ToString()).ToList();
                    using (System.Data.Entity.DbContextTransaction trans = dbContext.Database.BeginTransaction())
                    {
                        // error here, cannot add remark and status in
                        HandlerAssignment a = new HandlerAssignment();
                        a.ComplaintID = Convert.ToInt32(form["ID"].ToString());
                        a.HandlerID = Convert.ToInt32(form["ComplaintHander"].ToString());
                        if (dbContext.HandlerAssignments.ToList().Where(s => s.ComplaintID == a.ComplaintID && s.HandlerID == a.HandlerID).FirstOrDefault() == null)
                        {
                            // status not sure
                            if (c.Status == ComplaintStatus.REOPEN)
                            {
                                c.Status = ComplaintStatus.REOPEN;
                            }
                            else
                            {
                                c.Status = ComplaintStatus.PENDING;
                            }
                            //c.Status = ComplaintStatus.PENDING;
                            dbContext.HandlerAssignments.Add(a);
                            dbContext.SaveChanges();
                            trans.Commit();
                            return Redirect("Allocate/" + c.ID);
                            //return RedirectToAction("Allocate", "Complaint");
                        }
                        else
                        {
                            ModelState.AddModelError("Create Error", "Assign Error, Complaint handler already assigned, Please select another one");
                            return View("Allocate", c);
                        }

                    }
                    //return View(c);
                }
                else
                {
                    return Redirect("Allocate/" + c.ID);
                    //return RedirectToAction("Allocate", "Complaint");
                }
            }
            catch (Exception e)
            {
                return View(c);
            }
        }

        [CustomAuthorize(UserRole.CENTRAL_UNIT)]
        [HttpPost]
        public ActionResult Allocate(int id, FormCollection form)
        {
            var c = dbContext.Complaints.ToList().Where(s => s.ID == id).FirstOrDefault();
            List<SelectListItem> li = GetDepartmentList();

            ViewData["SupervisorDepartment"] = li;

            ViewData["HandlerDepartment"] = li;
            try
            {
                if (ModelState.IsValid)
                {
                    ViewData["IsRedflagList"] = dbContext.Complaints.ToList();
                    ViewData["DepartmentList"] = dbContext.Departments.ToList();
                    ViewData["ComplaintHandlerList"] = dbContext.AccountHolders.Where(s => s.Role == UserRole.COMPLAINT_HANDLER.ToString()).ToList();
                    ViewData["SupervisorList"] = dbContext.AccountHolders.Where(s => s.Role == UserRole.SUPERVISOR.ToString()).ToList();

                    using (System.Data.Entity.DbContextTransaction trans = dbContext.Database.BeginTransaction())
                    {
                        // error here, cannot add remark and status in
                        SupervisorAssignment a = new SupervisorAssignment();
                        a.ComplaintID = Convert.ToInt32(form["ID"].ToString());
                        a.SupervisorID = Convert.ToInt32(form["Supervisor"].ToString());
                        if (dbContext.SupervisorAssignments.ToList().Where(s => s.ComplaintID == a.ComplaintID && s.SupervisorID == a.SupervisorID).FirstOrDefault() == null)
                        {
                            // status not sure
                            if (c.Status == ComplaintStatus.REOPEN)
                            {
                                c.Status = ComplaintStatus.REOPEN;
                            }
                            else
                            {
                                c.Status = ComplaintStatus.PENDING;
                            }
                            dbContext.SupervisorAssignments.Add(a);
                            dbContext.SaveChanges();
                            trans.Commit();
                            return RedirectToAction("Allocate", "Complaint");
                        }
                        else
                        {
                            ModelState.AddModelError("Create Error", "Assign Error, supervisor already assigned, Please select another one");
                            return View(c);
                        }

                    }
                }
                else
                {
                    return RedirectToAction("Allocate", "Complaint");
                }
            }
            catch (Exception e)
            {
                return View(c);
            }
        }

        // GET: Complaint/Create

        public ActionResult Create()
        {
            ViewData["CategoryList"] = dbContext.Categories.ToList();
            return View();
        }

        // POST: Complaint/Create

        [HttpPost]
        public ActionResult Create(Complaint complaint, HttpPostedFileBase file)
        {
            ViewData["CategoryList"] = dbContext.Categories.ToList();

            try
            {
                if (ModelState.IsValid)
                {
                    using (System.Data.Entity.DbContextTransaction trans = dbContext.Database.BeginTransaction())
                    {
                        if (complaint.Title == null || complaint.Description == null)
                        {
                            ModelState.AddModelError("Create Error", "Complaint Create error, (title/description) cannot be empty.");
                            return View("Create", complaint);
                        }
                        else if ((complaint.PublicEmail != null && complaint.PublicName == null) || (complaint.PublicEmail == null && complaint.PublicName != null))
                        {
                            ModelState.AddModelError("Create Error", "Please provides us an (Email & Name) if you wish to received a response from us, " +
                                "OR keep it empty for (Email & Name) textbox if you wish to submit a complaint as Anonymous");
                            return View("Create", complaint);
                        }
                        else
                        {
                            complaint.Status = ComplaintStatus.OPEN;
                            complaint.DateSubmitted = DateTime.Now;
                            if (file != null)
                            {
                                file.SaveAs(HttpContext.Server.MapPath("~/Images/")
                                                                      + file.FileName);
                                complaint.Attachment = file.FileName;
                            }

                            if (HttpContext.User.Identity.IsAuthenticated && UserSession.Role != null)
                            {
                                complaint.ComplaintOwnerID = UserSession.UserId;
                            }

                            dbContext.Complaints.Add(complaint);
                            dbContext.SaveChanges();
                            trans.Commit();
                            // route to, you have submitted complaint successfully, get back button to home page
                            return RedirectToAction("Index", "Feedback");
                        }
                    }
                   
                }
                else
                {
                    return View(complaint);
                }
                // TODO: Add insert logic here
            }
            catch (Exception e)
            {

                return View(complaint);
            }
        }

        [CustomAuthorize(UserRole.CENTRAL_UNIT)]
        [HttpPost]
        public ActionResult UpdateRedFlag(int id, FormCollection form)
        {
            //if dropdown is "YES", check if complainthandler.count >0 and status <> REOPEN, inform user to remove complainhandler before it can set to YES
            //if dropdown is "NO", check if supervisor.count > 0, inform user to remove supervisor before it can change to NO

            ViewData["IsRedflagList"] = dbContext.Complaints.ToList();
            ViewData["DepartmentList"] = dbContext.Departments.ToList();
            ViewData["ComplaintHandlerList"] = dbContext.AccountHolders.Where(s => s.Role == UserRole.COMPLAINT_HANDLER.ToString()).ToList();
            ViewData["SupervisorList"] = dbContext.AccountHolders.Where(s => s.Role == UserRole.SUPERVISOR.ToString()).ToList();

            List<SelectListItem> li = GetDepartmentList();

            ViewData["SupervisorDepartment"] = li;
            ViewData["HandlerDepartment"] = li;
            string status = string.Empty;
            var message = "";

            var complaint = dbContext.Complaints.ToList().Where(s => s.ID == id).FirstOrDefault();
            if (complaint != null)
            {
                bool isRedFlag = (Convert.ToString(form["IsRedFlag"].ToString()) == "true" ? true : false);
                status = form["Status"].ToString();




                if (isRedFlag)
                {
                    IEnumerable<HandlerAssignment> complaintHandlerAssigned = dbContext.HandlerAssignments.ToList().Where(s => s.ComplaintID == id);
                    if (complaintHandlerAssigned.Count() > 0 && status != ComplaintStatus.REOPEN && status != ComplaintStatus.CLOSED)
                    {
                        ModelState.AddModelError("Update Error", "Update error, please delete all the complaint handlers assigned to this complaint before you change the Red Flag status to YES.");
                        return View("Allocate", complaint);
                    }
                    else
                    {
                        using (System.Data.Entity.DbContextTransaction trans = dbContext.Database.BeginTransaction())
                        {
                            if (status == ComplaintStatus.CLOSED && complaint.Status != ComplaintStatus.CLOSED)
                            {
                                complaint.DateClose = DateTime.Now;
                            }
                            if (status != ComplaintStatus.CLOSED && complaint.Status != ComplaintStatus.REOPEN)
                            {
                                complaint.DateClose = null;
                            }
                            complaint.Status = status;
                            complaint.CentralUnitID = UserSession.UserId;
                            complaint.IsRedFlag = isRedFlag;
                            dbContext.SaveChanges();
                            trans.Commit();
                            if (status == ComplaintStatus.CLOSED)
                            {
                                TriggerFeedbackEmail(complaint);
                                message = "Feedback form sent";
                            }
                            ViewBag.Message = message;
                            return View("Allocate", complaint);
                        }
                    }

                }
                else
                {
                    IEnumerable<SupervisorAssignment> supervisorAssigned = dbContext.SupervisorAssignments.ToList().Where(s => s.ComplaintID == id);
                    if (supervisorAssigned.Count() > 0 && status != ComplaintStatus.REOPEN && status != ComplaintStatus.CLOSED)
                    {
                        ModelState.AddModelError("Update Error", "Update error, please delete all the supervisors assigned to this complaint before you change the Red Flag status to NO.");
                        return View("Allocate", complaint);
                    }
                    else
                    {
                        using (System.Data.Entity.DbContextTransaction trans = dbContext.Database.BeginTransaction())
                        {
                            if (status == ComplaintStatus.CLOSED && complaint.Status != ComplaintStatus.CLOSED)
                            {
                                complaint.DateClose = DateTime.Now;
                            }
                            if (status != ComplaintStatus.CLOSED && complaint.Status != ComplaintStatus.REOPEN)
                            {
                                complaint.DateClose = null;
                            }
                            complaint.Status = status;
                            complaint.CentralUnitID = UserSession.UserId;
                            complaint.IsRedFlag = isRedFlag;
                            dbContext.SaveChanges();
                            trans.Commit();
                            if (status == ComplaintStatus.CLOSED)
                            {
                                TriggerFeedbackEmail(complaint);
                                message = "Feedback form sent";
                            }
                            ViewBag.Message = message;
                            return View("Allocate", complaint);
                        }
                    }
                }

            }
            
            return View("Allocate", complaint);
        }

        private bool TriggerFeedbackEmail(Complaint complaint)
        {
            try
            {
                string sender = ConfigurationManager.AppSettings["Email.Sender"];
                string password = ConfigurationManager.AppSettings["Email.Password"];
                string recipient;
                if (complaint.ComplaintOwnerID > 0)
                {
                    recipient = complaint.AccountHolder.Email;
                }
                else if (complaint.PublicEmail != null)
                {
                    recipient = complaint.PublicEmail;
                }else
                {
                    recipient = string.Empty;
                }

                if(recipient!= string.Empty)
                {
                    MailMessage mm = new MailMessage(sender, recipient);
                    mm.Subject = "Feedback for ComplaintPortal";
                    mm.Body = "Hi, You feedback is valuable to us. Please click URL below to do the survey. <br> <br>" + GetFeedbackLink(complaint);
                    mm.IsBodyHtml = true;

                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = "smtp.gmail.com";
                    smtp.Port = 587;
                    smtp.EnableSsl = true;

                    NetworkCredential nc = new NetworkCredential(sender, password);
                    smtp.UseDefaultCredentials = true;
                    smtp.Credentials = nc;
                    smtp.Send(mm);
                    return true;
                }
                return false; 
            }
            catch (Exception e)
            {
                return false;
            }
        }
        private string GetFeedbackLink(Complaint complaint)
        {
            string complaintId = Helper.Encrypt(complaint.ID.ToString());
            var verifyUrl = "/Feedback/FeedbackHandler/?id=" + HttpUtility.UrlEncode(complaintId);
            string url = System.Web.HttpContext.Current.Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);
            //string url = "https://localhost:44337/Feedback/FeedbackHandler/" + HttpUtility.UrlEncode(complaintId);
            return url;
        }
    }
}
