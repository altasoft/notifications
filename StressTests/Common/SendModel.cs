using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StressTests.Common
{
    public class SendModel
    {
        public int ApplicationId { get; set; }
        public string ApplicationSecretKey { get; set; }

        public string ApplicationProductKey { get; set; }
        public int? ExternalUserId { get; set; }
        public List<int> ExternalUserIds { get; set; }
        public string To { get; set; }
        public string GroupKey { get; set; }

        public string ProviderKey { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public DateTime? ProcessDate { get; set; }
        public int Priority { get; set; }
        public bool IsTest { get; set; }
    }
}
