using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace MyBookkeeping.Filters
{
    public class CheckAuthorizeActionFilter : AuthorizeAttribute
    {
        public bool IsAuthorize { get; set; }

        public string AuthRole { get; set; }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);

            if (!this.IsAuthorize)
            {
                filterContext.HttpContext.Response.StatusCode = 403;

                //沒有權限就回到登入頁
                UrlHelper urlHelper = new UrlHelper(filterContext.RequestContext);
                filterContext.Result = new RedirectResult(urlHelper.Action("Login", "Home"));
                
            }
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            this.IsAuthorize = false;
            if (httpContext == null)
                throw new ArgumentNullException("httpContext");

            string[] users = Users.Split(',');

            if (!httpContext.User.Identity.IsAuthenticated)
                return false;

            //取得使用者的角色
            FormsIdentity id = httpContext.User.Identity as FormsIdentity;
            FormsAuthenticationTicket ticket = id.Ticket;
            string[] currentRoles = ticket.UserData.Split(',');

            string roles = AuthRole;

            foreach (string role in currentRoles)
            {
                if (roles.IndexOf(role) > -1)
                {
                    this.IsAuthorize = true;
                    return true;
                }

            }
            return false;
        }
    }
}