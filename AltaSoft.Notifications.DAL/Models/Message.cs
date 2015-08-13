using AltaSoft.Notifications.DAL.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AltaSoft.Notifications.DAL
{
    /// <summary>
    /// Queue to process send requests
    /// </summary>
    public class Message : ModelBase
    {
        public int? UserId { get; set; }
        public User User { get; set; }

        public int ProviderId { get; set; }
        public Provider Provider { get; set; }

        public int ApplicationId { get; set; }
        public Application Application { get; set; }

        public string To { get; set; }

        public string Subject { get; set; }

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
    }
}
