using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Gestion_Centre_Formations.Helpers
{
    public class RazorViewRenderer
    {
        public static string RenderViewToString(ControllerContext context, string viewName, object model)
        {
            // Find the view
            var viewEngineResult = ViewEngines.Engines.FindView(context, viewName, null);
            if (viewEngineResult.View == null)
            {
                throw new FileNotFoundException($"View '{viewName}' not found.");
            }

            var view = viewEngineResult.View;

            // Assign the model to ViewData
            context.Controller.ViewData.Model = model;

            // Render the view
            using (var sw = new StringWriter())
            {
                var viewContext = new ViewContext(
                    context,
                    view,
                    context.Controller.ViewData,
                    context.Controller.TempData,
                    sw
                );

                view.Render(viewContext, sw);
                return sw.ToString();
            }
        }
    }
}