using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ComplaintPortalSystem.Common
{
    public enum UserRole
    {
        ADMIN = 0,
        SUPERVISOR = 1,
        COMPLAINT_HANDLER =2,
        CENTRAL_UNIT = 3,
        STAFF = 4,
        STUDENT = 5,
        ANONYMOUS = 6,
        PUBLIC = 7
    }
}