﻿using AltaSoft.Notifications.DAL.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Data.Entity;

namespace AltaSoft.Notifications.DAL
{
    public class ProviderBusinessObject : BusinessObjectBase<Provider>
    {
        public Provider GetByKey(string key)
        {
            key = key.ToLower();

            return db.Providers.AsNoTracking().FirstOrDefault(x => x.Key.ToLower() == key);
        }
    }
}
