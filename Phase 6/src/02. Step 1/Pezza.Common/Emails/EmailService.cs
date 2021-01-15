namespace Pezza.Common.Emails
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Mail;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    public class EmailService
    {
        private readonly SmtpSettings smtpSettings;

        public async Task<bool> SingleGuestArrivalAsync(IList<string> emailList, string subject, string message, string rootFolder)
        {
            try
            {
                var mail = new MailMessage
                {
                    From = new MailAddress(this.smtpSettings.FromAddress),
                    Subject = subject,
                    Body = message
                };

                foreach (var email in emailList)
                {
                    mail.To.Add(email);
                }

                var doc = new HtmlDocument();
                doc.LoadHtml(message);
                var plainText = doc.DocumentNode.SelectSingleNode("//body").InnerText;
                plainText = Regex.Replace(plainText, @"\s+", " ").Trim();
                var plainView = AlternateView.CreateAlternateViewFromString(plainText, null, "text/plain");
                var htmlView = AlternateView.CreateAlternateViewFromString(message, null, "text/html");

                mail.AlternateViews.Add(plainView);
                mail.AlternateViews.Add(htmlView);
                mail.IsBodyHtml = true;
                var smtp = new SmtpClient(this.smtpSettings.Server, this.smtpSettings.Port)
                {
                    EnableSsl = this.smtpSettings.EnableSSL,
                    Credentials = new NetworkCredential(this.smtpSettings.Username, this.smtpSettings.Password)
                };

                await smtp.SendMailAsync(mail);

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
}
