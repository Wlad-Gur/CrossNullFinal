using MailKit.Net.Smtp;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CrossNull.Logic.Services
{
    class EmailService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            SmtpClient smtpClient = new SmtpClient();
            smtpClient.Connect("smtp.gmail.com", 465, true);
            smtpClient.Authenticate(new NetworkCredential("123qwert88888888@gmail.com", "!123qwert"));
            //{ Host = "smtp.gmail.com",
            //    EnableSsl = true,
            //    Port = 465,
            //    Credentials = new NetworkCredential("123qwert88888888@gmail.com", "!123qwert"),
            //    UseDefaultCredentials = false,
            //    DeliveryMethod = SmtpDeliveryMethod.Network,
            //    DeliveryFormat= SmtpDeliveryFormat.International
            //};
            //MailMessage mailMessage = new MailMessage("123qwert88888888@gmail.com", message.Destination,
            //    "Reset password", "Hello ");
            //smtpClient.Send(mailMessage);
            return Task.CompletedTask;
        }
    }
}
