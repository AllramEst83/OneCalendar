using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OneCalendar.Helpers.Settings
{
    public class AppSettings
    {
        public string AuthDbConnectionString { get; set; }
        public string CalenderConnectionString { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string Secret { get; set; }
        public string Email { get; set; }
        public string MailPassword { get; set; }
    }
}
