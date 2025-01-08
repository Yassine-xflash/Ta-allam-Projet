using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Gestion_Centre_Formations
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            var exception = Server.GetLastError(); // Get the exception
            Server.ClearError(); // Clear the error to prevent ASP.NET from handling it

            // Log the exception (optional)
            // Logger.Log(exception);

            //// Redirect to the Error controller
            var httpException = exception as HttpException;
            if (httpException != null)
            {
                switch (httpException.GetHttpCode())
                {
                    case 401:
                        Response.Redirect("~/Error/Error401"); // Redirect to Error401 for unauthorized access
                        break;
                    case 404:
                        Response.Redirect("~/Error/Error404"); // Redirect to Error404 for not found
                        break;
                    case 500:
                        Response.Redirect("~/Error/Error500"); // Redirect to Error500 for server errors
                        break;
                    default:
                        Response.Redirect("~/Error/Index"); // Redirect to a generic error page for other errors
                        break;
                }
            }
            else
            {
                Response.Redirect("~/Error/Index"); // Redirect to a generic error page for non-HTTP exceptions
            }
        }
    }
}