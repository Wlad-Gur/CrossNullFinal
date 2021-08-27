using MailKit.Net.Smtp;
using Microsoft.AspNet.Identity;
using MimeKit;
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
            smtpClient.Connect("localhost", 1025, false);
            //smtpClient.Authenticate(new NetworkCredential("123qwert88888888@gmail.com", "!123qwert"));
            //{ Host = "smtp.gmail.com",
            //    EnableSsl = true,
            //    Port = 465,
            //    Credentials = new NetworkCredential("123qwert88888888@gmail.com", "!123qwert"),
            //    UseDefaultCredentials = false,
            //    DeliveryMethod = SmtpDeliveryMethod.Network,
            //    DeliveryFormat= SmtpDeliveryFormat.International
            //};

            MimeMessage mailMessage = new MimeMessage(
                new[] { new MailboxAddress("123qwert88888888@gmail.com") },
                new[] { new MailboxAddress(message.Destination) },
                "Reset password", new TextPart(MimeKit.Text.TextFormat.Html) { Text = message.Body });
            smtpClient.Send(mailMessage);
            return Task.CompletedTask;
        }
    }
}
