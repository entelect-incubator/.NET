namespace Scheduler.Jobs;

using System.Threading.Tasks;

public interface IEmailJob
{
	Task SendAsync(CancellationToken cancellationToken = default);
}

public sealed class EmailJob : IEmailJob
{
	public Task SendAsync(CancellationToken cancellationToken = default)
	{
		return Task.CompletedTask;
	}
}