using AltaSoft.Notifications.DAL.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AltaSoft.Notifications.DAL
{
    /// <summary>
    /// Notification providers, SendGrid, SMS, SignalR, etc.
    /// </summary>
    public class Provider : ModelBase
    {
        [Index("IX_Key", IsUnique = true), StringLength(20)]
        /// <summary>
        /// Will be identified by this field
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Display Name
        /// </summary>
        public string Name { get; set; }

        public string WebUrl { get; set; }
    }
}
