namespace Scheduler.Jobs;

using System.Threading.Tasks;
using Common.CQRS;
using Core.Todos.Commands;

public interface IEmailJob
{
	Task SendAsync(CancellationToken cancellationToken = default);
}

public sealed class EmailJob(Dispatcher dispatcher) : IEmailJob
{
	public async Task SendAsync(CancellationToken cancellationToken = default)
		=> await dispatcher.Send(new ExpiredTodoCommand() { ToEmail = "fakeemail@test.com" }, cancellationToken);
}