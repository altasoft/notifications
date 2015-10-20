using AltaSoft.Notifications.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AltaSoft.Notifications.Web.Common
{
    public class Helper
    {
        public static IEnumerable<SelectListItem> GetUsers()
        {
            using (var bo = new UserBusinessObject())
            {
                return bo.GetList(x => x.ApplicationId == UserContext.Current.Id).Select(x => new SelectListItem
                {
                    Text = x.FullName,
                    Value = x.Id.ToString()
                }).ToList();
            }
        }
        
        public static IEnumerable<SelectListItem> GetProviders()
        {
            using (var bo = new ProviderBusinessObject())
            {
                return bo.GetList().OrderBy(x => x.Name).Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList();
            }
        }

        public static IEnumerable<SelectListItem> GetGroups()
        {
            using (var bo = new GroupBusinessObject())
            {
                return bo.GetList(x => x.ApplicationId == UserContext.Current.Id).Select(x => new SelectListItem
                {
                    Text = x.Description,
                    Value = x.Key
                });
            }
        }


        public static string GetToByProvider(User user, int providerId)
        {
            switch (providerId)
            {
                case 1: return user.Email;
                case 2: return user.MobileNumber;
                case 3: return user.ExternalUserId.ToString();
                case 4: return user.Email;
                default: return String.Empty;
            }
        }
    }
}