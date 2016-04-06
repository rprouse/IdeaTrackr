using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdeaTracker.Web.Services
{
    public class AuthMessageSenderOptions
    {
        public string FromName { get; set; }
        public string FromEmail { get; set; }

        public string SmtpHost { get; set; }
        public int SmtpPort { get; set; }
        public string SmtpUser { get; set; }
        public string SmtpPassword { get; set; }
    }
}
