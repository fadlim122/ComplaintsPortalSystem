using ComplaintPortalSystem.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ComplaintPortalSystem.Controllers
{
    public class DashboardEditorController : Controller
    {
        [CustomAuthorize(UserRole.ADMIN)]
        // GET: Dashboard
        public ActionResult DashboardDesigner()
        {
            return View("DashboardDesigner");
        }

        [CustomAuthorize(UserRole.SUPERVISOR)]
        public ActionResult DashboardViewer()
        {
            return View("DashboardViewer");
        }
    }
}