using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ComplaintPortalSystem.Common
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        public CustomAuthorizeAttribute(params object[] roles)
        {
            if (roles.Any(r => r.GetType().BaseType != typeof(Enum)))
                throw new ArgumentException("roles");

            this.Roles = string.Join(",", roles.Select(r => Enum.GetName(r.GetType(), r)));
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.User.Identity.IsAuthenticated && UserSession.Role !=null) //If user already login but his role is not allowed to access, show forbidden access
            {
                filterContext.Result = new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }
            else //otherwise, ask user to login
            {
                filterContext.Result = new RedirectResult("~/Home/Login");

            }
        }
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                if(UserSession.Role == null)
                {
                    return false;
                }
                else
                {
                    if (this.Roles.Contains(UserSession.Role))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }           
            }else
            {
                return false;
            }
            //return base.AuthorizeCore(httpContext);
        }
        

       
    }
}