namespace Core.Email;

using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Common.DTO;
using Common.Models;
using SendGrid;
using SendGrid.Helpers.Mail;

public class EmailService
{
    public string HtmlContent { get; set; }

    public CustomerDTO Customer { get; set; }

    public async Task<Result> SendEmail()
    {
        var doc = new HtmlDocument();
        doc.LoadHtml(this.HtmlContent);
        var plainText = doc.DocumentNode.SelectSingleNode("//body").InnerText;
        plainText = Regex.Replace(plainText, @"\s+", " ").Trim();

        var apiKey = "YOUR-API-KEY";
        var client = new SendGridClient(apiKey);
        var from = new EmailAddress("notify@pezza.com", "EList");
        var subject = $"Collect your order it while it's hot";
        var to = new EmailAddress(this.Customer?.Email, this.Customer?.Name);
        var plainTextContent = plainText;
        var htmlContent = this.HtmlContent;
        var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
        var response = await client.SendEmailAsync(msg);

        return response.StatusCode != System.Net.HttpStatusCode.OK ? Result.Failure("Email could not send") : Result.Success();
    }
}