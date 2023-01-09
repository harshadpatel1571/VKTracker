using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace VKTracker.Common.Helper
{
    public class AuthorizeActionFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpSessionStateBase session = filterContext.HttpContext.Session;
            if (session["OrganizationId"] == null || Convert.ToInt32(session["OrganizationId"])== 0)
            {
                filterContext.Result =
                    new RedirectToRouteResult(
                        new RouteValueDictionary{{ "controller", "Account" },
                                { "action", "Logout" }

                        });
            }

            base.OnActionExecuting(filterContext);
        }
    }
}
