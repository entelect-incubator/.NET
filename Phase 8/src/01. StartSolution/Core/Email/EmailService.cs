namespace Core.Email;

using System.Text.RegularExpressions;
using Common.Models.Todos;
using FluentEmail.Core;
using HtmlAgilityPack;

public class EmailService
{
	public string HtmlContent { get; set; }

	public string ToEmail { get; set; }

	public TodoModel Model { get; set; }

	public async Task<Result> SendEmail()
	{
		var doc = new HtmlDocument();
		doc.LoadHtml(this.HtmlContent);
		var plainText = doc.DocumentNode.SelectSingleNode("//body").InnerText;
		plainText = Regex.Replace(plainText, @"\s+", " ").Trim();

		var email = await Email
			.From("todos@elist.com", "EList")
			.To(this.ToEmail)
			.Subject("Todo item(s) is about to expire")
			.Body(this.HtmlContent)
			.PlaintextAlternativeBody(plainText)
			.SendAsync();

		return email.Successful ? Result.Success() : Result.Failure("Email could not send");
	}
}