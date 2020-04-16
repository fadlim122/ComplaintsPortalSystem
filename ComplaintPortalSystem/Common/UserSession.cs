using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ComplaintPortalSystem.Common
{
    public class UserSession
    {
        public static void CreateUserSession(int userId, string userName, string email, string role)
        {
            UserId = userId;
            UserName = userName;
            Email = email;
            Role = role;
        }
        public static int UserId
        {
            get { return (int)HttpContext.Current.Session["UserId"]; }
            set { HttpContext.Current.Session["UserId"] = value; }
        }

        public static string UserName
        {
            get { return (string)HttpContext.Current.Session["UserName"]; }
            set { HttpContext.Current.Session["UserName"] = value; }
        }

        public static string Email
        {
            get { return (string)HttpContext.Current.Session["Email"]; }
            set { HttpContext.Current.Session["Email"] = value; }
        }

        public static string Role
        {
            get { return (string)HttpContext.Current.Session["Role"]; }
            set { HttpContext.Current.Session["Role"] = value; }
        }

    }
}