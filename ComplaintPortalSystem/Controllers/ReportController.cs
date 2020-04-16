using DevExpress.XtraReports.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ComplaintPortalSystem.Reports;
using ComplaintPortalSystem.Common;

namespace ComplaintPortalSystem.Controllers
{
    public class ReportController : Controller
    {
        // GET: Report
        public ActionResult Index()
        {
            return View();
        }

        [CustomAuthorize(UserRole.SUPERVISOR)]
        public ActionResult GetReportComplaintReceivedOvertime()
        {
            return View();
        }
    }
}