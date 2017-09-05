using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;

namespace MyBookkeeping
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            //讓 ASP.NET 知道該使用者擁有什麼角色
            if (Request.IsAuthenticated)
            {
                FormsIdentity id = (FormsIdentity)User.Identity;                
                FormsAuthenticationTicket ticket = id.Ticket;
                string[] roles = ticket.UserData.Split(new char[] { ',' });
                Context.User = new GenericPrincipal(Context.User.Identity, roles);
            }

        }
    }
}
