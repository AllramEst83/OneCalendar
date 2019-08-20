using Microsoft.Extensions.Options;
using OneCalendar.Helpers.Settings;
using OneCalendar.Interfaces;
using OneCalendar.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace OneCalendar.Services
{
    public class MailService : IMailService
    {
        public MailService(IOptions<AppSettings> appsettings)
        {
            Appsettings = appsettings.Value;
        }

        public AppSettings Appsettings { get; }

        public MailSent SendEmail(string userEmail)
        {
            MailSent mailSent;
            //För att avaktivera Less Secure App:
            //https://myaccount.google.com/u/0/lesssecureapps
            SmtpClient mail = new SmtpClient()
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(Appsettings.Email, Appsettings.MailPassword)
            };

            using (MailMessage message = new MailMessage(Appsettings.Email, userEmail)
            {
                Subject = "Test återsällning",
                Body = $"Teståterställning av lösenord för {userEmail}",
                IsBodyHtml = true
            })
            {
                //message.Attachments.Add(new Attachment("C:\\Users\\kaywi\\Downloads\\IMG_20190807_211308.jpg"));
                try
                {
                    mail.Send(message);
                }
                catch (Exception ex)
                {
                    mailSent = new MailSent()
                    {
                        IsSent = false,
                        SentMessage = ex.InnerException.ToString()
                    };

                    return mailSent;
                }

                mailSent = new MailSent()
                {
                    IsSent = true, 
                    SentMessage =$"Mail with a new password for username '{userEmail}' has successfully been sent"
                };

                return mailSent;
            }
        }
    }
}
