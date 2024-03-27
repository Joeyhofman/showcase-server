using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Domain.Interfaces.Services;
using Infrastructure.Services.Exceptions;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace Infrastructure.Services
{
    public class ContactEmailService : IContactEmailService
    {
        private readonly IConfiguration _configuration;

        public ContactEmailService(IConfiguration config)
        {
            _configuration = config;
        }

        public async Task SendMessage(string firstname, string lastname, string phonenumber, string email, string subject, string message)
        {

            var mineMessage = new MimeMessage();
            mineMessage.From.Add(new MailboxAddress("Contact formulier", email));
            mineMessage.To.Add(new MailboxAddress("Joey Hofman", "joeyhofman1@gmail.com"));
            mineMessage.Subject = subject;

            var bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = $"<p>{message}</p> <br><br><br> <h1>contact informatie</h1><p><strong>Voornaam</strong>: {firstname}<br> <strong>Achternaam</strong>: {lastname}<br> <strong>Telefoonnummer</strong>: {phonenumber}<br> <strong>email</strong>: {email}<br></p>";

            mineMessage.Body = bodyBuilder.ToMessageBody();

            var mailSettings = _configuration.GetSection("MailSettings");
            string server = mailSettings["Server"];
            int port = int.Parse(mailSettings["Port"]);
            string username = mailSettings["Username"];
            string password = mailSettings["Password"];

            MailKit.Net.Smtp.SmtpClient smtpClient = new MailKit.Net.Smtp.SmtpClient();

            try
            {
                smtpClient.Connect(server, port, false);
                smtpClient.Authenticate(username, password);
                await smtpClient.SendAsync(mineMessage);
            }
            catch (Exception e)
            {
                throw new CouldNotSendEmailException(e.Message);
            }
            finally
            {
                smtpClient.Disconnect(true);
                smtpClient.Dispose();
            }
        }
    }
}
