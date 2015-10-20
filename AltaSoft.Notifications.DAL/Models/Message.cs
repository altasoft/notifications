using AltaSoft.Notifications.DAL.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AltaSoft.Notifications.DAL
{
    /// <summary>
    /// Queue to process send requests
    /// </summary>
    public class Message : ModelBase, IProviderInfo
    {
        public int? UserId { get; set; }
        public User User { get; set; }

        public int ProviderId { get; set; }
        public Provider Provider { get; set; }

        public int ApplicationId { get; set; }
        public Application Application { get; set; }

        [StringLength(50)]
        public string To { get; set; }

        [StringLength(200)]
        public string Subject { get; set; }

        [StringLength(2048)]
        public string Content { get; set; }

        public int RetryCount { get; set; }

        public string ErrorMessage { get; set; }

        public string ErrorDetails { get; set; }

        public Guid GroupId { get; set; }

        public MessageStates State { get; set; }

        public MessagePriority Priority { get; set; }

        /// <summary>
        /// If null, processes immediately, otherwise waits for ProcessDate
        /// </summary>
        public DateTime? ProcessDate { get; set; }

        /// <summary>
        /// Duration in MS
        /// </summary>
        public int ProcessingDuration { get; set; }

        /// <summary>
        /// for SMS Service, to send it immediately
        /// </summary>
        public bool? ForceSendingNow { get; set; }

        public bool IsTest { get; set; }
    }
}
