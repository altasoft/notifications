using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace AltaSoft.Notifications.Service
{
    public static class Extensions
    {
        public static string GetQueryString(this object obj, bool encodeValues = false)
        {
            var properties = from p in obj.GetType().GetProperties()
                             where p.GetValue(obj, null) != null
                             select p.Name + "=" + (encodeValues ? HttpUtility.UrlEncode(p.GetValue(obj, null).ToString()) : p.GetValue(obj, null).ToString());

            return String.Join("&", properties.ToArray());
        }
    }
}
