using System;
using System.Web;
using System.Web.Mvc;

namespace Gestion_Centre_Formations.CustomAttributes
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        private readonly string _role;

        public CustomAuthorizeAttribute(string role)
        {
            _role = role;
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext.Session["UserType"] == null)
            {
                return false; 
            }

            return httpContext.Session["UserType"].ToString() == _role;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.Session["UserType"] == null)
            {
                filterContext.Result = new RedirectResult("~/Authentification/Login");
            }
            else
            {
                filterContext.Result = new RedirectResult("~/Error/Error401");
            }
        }
    }
}