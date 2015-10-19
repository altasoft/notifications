using AltaSoft.Notifications.DAL.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Data.Entity;

namespace AltaSoft.Notifications.DAL
{
    public class ApplicationBusinessObject : BusinessObjectBase<Application>
    {
        public bool Check(int id, string secretKey)
        {
            return db.Applications.AsNoTracking().FirstOrDefault(x => x.Id == id && x.SecretKey == secretKey) != null;
        }


        public Application Authenticate(string username, string password)
        {
            return db.Applications.FirstOrDefault(x => x.Username == username && x.Password == password);
        }
    }
}
