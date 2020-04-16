using ComplaintPortalSystem.Common;
using ComplaintPortalSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ComplaintPortalSystem.Controllers
{
    public class HandlerAssignmentController : Controller
    {
        // GET: HandlerAssignment
        ComplaintPortalDBEntities dbContext = new ComplaintPortalDBEntities();
        [CustomAuthorize(UserRole.COMPLAINT_HANDLER)]
        public ActionResult Index()
        {
            return View(dbContext.HandlerAssignments.ToList().Where(s => s.HandlerID == UserSession.UserId && s.Complaint.Status != ComplaintStatus.CLOSED));
        }

        public ActionResult ViewCloseTickets()
        {
            return View(dbContext.HandlerAssignments.ToList().Where(s => s.HandlerID == UserSession.UserId && s.Complaint.Status == ComplaintStatus.CLOSED));
        }

        [CustomAuthorize(UserRole.COMPLAINT_HANDLER)]
        public new ActionResult Response(int id)
        {
            ViewData["ExternalAgencyList"] = dbContext.ExternalAgencies.ToList();
            var complaint = dbContext.HandlerAssignments.ToList().Where(s => s.ComplaintID == id && s.HandlerID == UserSession.UserId).FirstOrDefault();
            return View(complaint);
        }

        [CustomAuthorize(UserRole.COMPLAINT_HANDLER)]
        [HttpPost]
        public new ActionResult Response(int id, HandlerAssignment complaintHandler)
        {
            ViewData["ExternalAgencyList"] = dbContext.ExternalAgencies.ToList();
            try
            {
                // TODO: Add update logic here
                if (ModelState.IsValid)
                {
                    var complaint = dbContext.HandlerAssignments.ToList().Where(s => s.ComplaintID == id && s.HandlerID == UserSession.UserId).FirstOrDefault();

                    complaint.Remark = complaintHandler.Remark;
                    complaint.Status = complaintHandler.Status;
                    complaint.Complaint.ExternalAgencyID = complaintHandler.Complaint.ExternalAgency.ID;
                    complaint.ResponseDate = DateTime.Now;


                    dbContext.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(complaintHandler);
                }

            }
            catch (Exception e)
            {
                return View();
            }
        }

        [CustomAuthorize(UserRole.COMPLAINT_HANDLER)]
        public ActionResult ApplyLeave(int id)
        {
            var handler = dbContext.ComplaintHandlers.Where(s => s.ID == id).FirstOrDefault();
            return View(handler);
        }

        [CustomAuthorize(UserRole.COMPLAINT_HANDLER)]
        [HttpPost]
        public ActionResult ApplyLeave(int id, ComplaintHandler complaintHandler)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var handler = dbContext.ComplaintHandlers.ToList().Where(s => s.ID == id).FirstOrDefault();
                    if (complaintHandler.StartDate.HasValue && complaintHandler.EndDate.HasValue)
                    {
                        if (complaintHandler.StartDate > complaintHandler.EndDate)
                        {
                            ModelState.AddModelError("Date error", "Date error, Start date cannot be earlier than End date");
                            return View(complaintHandler);
                        }
                        else
                        {
                            handler.StartDate = complaintHandler.StartDate;
                            handler.EndDate = complaintHandler.EndDate;
                            dbContext.SaveChanges();
                            ViewBag.Message = "Apply Leave Successfully";
                            return View(complaintHandler);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("Date error", "Date error, Cannot be empty, you should input the Date value");
                        return View(complaintHandler);
                    }
                    
                }
                else
                {
                    return View(complaintHandler);
                }
            }
            catch (Exception e)
            {
                return View(complaintHandler);
            }
        }

    }
}
