namespace Scheduler.Jobs;

using System.Threading.Tasks;
using Core.Todos.Commands;
using MediatR;

public interface IEmailJob
{
	Task SendAsync(CancellationToken cancellationToken = default);
}

public sealed class EmailJob(IMediator mediator) : IEmailJob
{
	public async Task SendAsync(CancellationToken cancellationToken = default)
		=> await mediator.Send(new ExpiredTodoCommand() { ToEmail = "fakeemail@test.com" }, cancellationToken);
}