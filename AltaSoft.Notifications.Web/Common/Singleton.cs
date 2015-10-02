using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AltaSoft.Notifications.Web.Common
{
    public class Singleton<T> where T : class, new()
    {
        private static object lockingObject = new object();
        private static T singleTonObject;
        protected Singleton()
        {
        }

        public static T Current
        {
            get
            {
                return InstanceCreation();
            }
        }
        public static T InstanceCreation()
        {
            if (singleTonObject == null)
            {
                lock (lockingObject)
                {
                    if (singleTonObject == null)
                    {
                        singleTonObject = new T();
                    }
                }
            }
            return singleTonObject;
        }
    }
}