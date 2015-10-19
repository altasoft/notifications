using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AltaSoft.Notifications.Web.Common
{
    public class SecuredAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (UserContext.IsAuthenticated)
                return;

            base.OnAuthorization(filterContext);
        }
    }
}
