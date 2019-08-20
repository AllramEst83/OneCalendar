using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OneCalendar.Models
{
    public class MailSent
    {
        public bool IsSent { get; set; } = false;
        public string SentMessage { get; set; } = string.Empty;
    }
}
