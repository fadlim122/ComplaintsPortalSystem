using ComplaintPortalSystem.Common;
using ComplaintPortalSystem.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ComplaintPortalSystem.Controllers
{
    public class FeedbackController : Controller
    {
        ComplaintPortalDBEntities dbContext = new ComplaintPortalDBEntities();
        // GET: Feedback
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult FeedbackHandler (String id)
        {
            //id = HttpUtility.UrlDecode(id);
            id = Helper.Decrypt(id);
            int complaintID = Convert.ToInt32(id);
            var complaint = dbContext.Complaints.Where(s => s.ID == complaintID).FirstOrDefault();

            return Redirect("~/Feedback/Feedback/" + complaintID);
        }
        public ActionResult Feedback(int id)
        {
            var complaint = dbContext.Complaints.Where(s => s.ID == id).FirstOrDefault();
            return View(complaint);
        }


        [HttpPost]
        public ActionResult Feedback(int id, Complaint complaint)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var feedback = dbContext.Complaints.ToList().Where(s => s.ID == id).FirstOrDefault();
                    feedback.RatingEfficacy = complaint.RatingEfficacy;
                    feedback.RatingFeedback = complaint.RatingFeedback;
                    feedback.RatingFriendliness = complaint.RatingFriendliness;
                    feedback.RatingSpeed = complaint.RatingSpeed;
                    dbContext.SaveChanges();
                    return RedirectToAction("Index", "Feedback");
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
    }
}