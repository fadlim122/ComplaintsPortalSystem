using ComplaintPortalSystem.Common;
using ComplaintPortalSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ComplaintPortalSystem.Controllers
{
    public class SupervisorAssignmentController : Controller
    {
        ComplaintPortalDBEntities dbContext = new ComplaintPortalDBEntities();

        [CustomAuthorize(UserRole.SUPERVISOR)]
        public ActionResult Index()
        {
            return View(dbContext.SupervisorAssignments.ToList().Where(s => s.SupervisorID == UserSession.UserId && s.Complaint.Status != ComplaintStatus.CLOSED));
        }

        public ActionResult ViewCloseTickets()
        {
            return View(dbContext.SupervisorAssignments.ToList().Where(s => s.SupervisorID == UserSession.UserId && s.Complaint.Status == ComplaintStatus.CLOSED));
        }

        [CustomAuthorize(UserRole.SUPERVISOR)]
        public new ActionResult Response(int id)
        {
            var complaint = dbContext.SupervisorAssignments.ToList().Where(s => s.ComplaintID == id && s.SupervisorID == UserSession.UserId).FirstOrDefault();
            return View(complaint);
        }

        [CustomAuthorize(UserRole.SUPERVISOR)]
        [HttpPost]
        public new ActionResult Response(int id, SupervisorAssignment supervisor)
        {
            try     
            {
                if (ModelState.IsValid)
                {
                    var complaint = dbContext.SupervisorAssignments.ToList().Where(s => s.ComplaintID == id && s.SupervisorID == UserSession.UserId).FirstOrDefault();

                    complaint.Remark = supervisor.Remark;
                    complaint.Status = supervisor.Status;
                    complaint.ResponseDate = DateTime.Now;

                    dbContext.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(supervisor);
                }

            }
            catch (Exception e)
            {
                return View();
            }
        }

        [CustomAuthorize(UserRole.SUPERVISOR)]
        public ActionResult ApplyLeave(int id)
        {
            var handler = dbContext.Supervisors.Where(s => s.ID == id).FirstOrDefault();
            return View(handler);
        }

        [CustomAuthorize(UserRole.SUPERVISOR)]
        [HttpPost]
        public ActionResult ApplyLeave(int id, Supervisor supervisor)
        {
            try
            {
                if (ModelState.IsValid)
                {            
                    var ss = dbContext.Supervisors.ToList().Where(s => s.ID == id).FirstOrDefault();

                    if (supervisor.StartDate.HasValue && supervisor.EndDate.HasValue)
                    {
                        if(supervisor.StartDate > supervisor.EndDate)
                        {
                            ModelState.AddModelError("Date error", "Date error, Start date cannot be earlier than End date");
                            return View(supervisor);
                        }
                        else
                        {
                                ss.StartDate = supervisor.StartDate;
                                ss.EndDate = supervisor.EndDate;
                                dbContext.SaveChanges();
                                ViewBag.Message = "Apply Leave Successfully";
                                return View(supervisor);
                        }
                        
                    }
                    else
                    {
                        ModelState.AddModelError("Date error", "Date error, Cannot be empty, you should input the Date value");
                        return View(supervisor);
                    }
                }
                else
                {
                    return View(supervisor);
                }
            }
            catch (Exception e)
            {
                return View(supervisor);
            }
        }

    }
}
