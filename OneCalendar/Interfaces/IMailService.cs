using OneCalendar.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OneCalendar.Interfaces
{
    public interface IMailService
    {
        MailSent SendEmail(string userEmail);
    }
}
