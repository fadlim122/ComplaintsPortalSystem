using ComplaintPortalSystem.Common;
using ComplaintPortalSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ComplaintPortalSystem.Controllers
{
    public class ViewTicketController : Controller
    {
        // GET: ViewTicket
        ComplaintPortalDBEntities dbContext = new ComplaintPortalDBEntities();

        [CustomAuthorize(UserRole.STAFF, UserRole.STUDENT)]
        public ActionResult Index()
        {
            // error here
            return View(dbContext.Complaints.ToList().Where(s => s.ComplaintOwnerID == UserSession.UserId));
        }

        [CustomAuthorize(UserRole.STAFF, UserRole.STUDENT)]
        public ActionResult Details(int id)
        {
            var ticket = dbContext.Complaints.ToList().Where(s => s.ID == id).FirstOrDefault();
            return View(ticket);
        }
    }
}