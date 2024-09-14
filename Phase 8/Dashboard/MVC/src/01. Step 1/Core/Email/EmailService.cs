namespace Core.Email;

using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Common.Models;
using FluentEmail.Core;
using HtmlAgilityPack;

public class EmailService
{
	public string HtmlContent { get; set; }

	public CustomerModel Customer { get; set; }

	public async Task<Result> SendEmail()
	{
		var doc = new HtmlDocument();
		doc.LoadHtml(this.HtmlContent);
		var plainText = doc.DocumentNode.SelectSingleNode("//body").InnerText;
		plainText = Regex.Replace(plainText, @"\s+", " ").Trim();

		var email = await Email
			.From("notify@pezza.com", "EList")
			.To(this.Customer?.Email, this.Customer?.Name)
			.Subject("Collect your order it while it's hot")
			.Body(this.HtmlContent)
			.PlaintextAlternativeBody(plainText)
			.SendAsync();

		return email.Successful ? Result.Success() : Result.Failure("Email could not send");
	}
}