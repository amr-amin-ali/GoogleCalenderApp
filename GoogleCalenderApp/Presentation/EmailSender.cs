using GoogleCalenderApp.Contracts;
using MimeKit;
//using MailKit.Net.Smtp;
//using System.Net.Mail;

namespace GoogleCalenderApp.Presentation
{
    public class EmailSender : INotifier
    {

        public async Task<bool> Notify(string message)
        {
            string fromEmail = "rachelle.mayer@ethereal.email";
            string fromPassword = "KJnEqaDRvZkupHT1s2";
            string toEmail = "rachelle.mayer@ethereal.email";

            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(fromEmail));
            email.To.Add(MailboxAddress.Parse(toEmail));
            email.Subject = "New event notification";
            email.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = message };
            var smtp = new MailKit.Net.Smtp.SmtpClient();
            try
            {
                smtp.Connect("smtp.ethereal.email", 587, MailKit.Security.SecureSocketOptions.StartTls);
                smtp.Authenticate(fromEmail, fromPassword);
                smtp.Send(email);
                smtp.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
