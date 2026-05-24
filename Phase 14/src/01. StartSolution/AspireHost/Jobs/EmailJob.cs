namespace Scheduler.Jobs;

using System.Threading.Tasks;
using Common.DTO;
using Core.Email;

public interface IEmailJob
{
	Task SendAsync(CancellationToken cancellationToken = default);
}

public sealed class EmailJob : IEmailJob
{
	public async Task SendAsync(CancellationToken cancellationToken = default)
	{
		var emailService = new EmailService
		{
			HtmlContent = "<p>This is a scheduled test email.</p>",
			Customer = new CustomerDTO { Email = "fakeemail@test.com", Name = "Pezza User" }
		};

		await emailService.SendEmail();
	}
}