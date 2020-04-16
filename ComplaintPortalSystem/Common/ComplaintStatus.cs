using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ComplaintPortalSystem.Common
{
    public class ComplaintStatus
    {
        public static string OPEN = "OPEN";
        public static string PENDING = "PENDING";
        public static string PENDING_EXTERNAL = "PENDING_EXTERNAL";
        public static string CLOSED = "CLOSED";
        public static string REOPEN = "REOPEN";
    }

    public class InvestigtionStatus
    {
        public static string PENDING = "PENDING";
        public static string IN_PROGRESS = "IN_PROGRESS";
        public static string COMPLETED = "COMPLETED";
    }
}