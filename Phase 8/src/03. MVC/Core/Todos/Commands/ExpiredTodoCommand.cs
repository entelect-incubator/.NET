namespace Core.Todos.Commands;

using Common.CQRS;
using Core.Todos.Events;

public class ExpiredTodoCommand : ICommand<Result>
{
	public required string ToEmail { get; set; }
}

public class ExpiredTodoCommandHandler(Dispatcher dispatcher) : ICommandHandler<ExpiredTodoCommand, Result>
{
	public async Task<Result> Handle(ExpiredTodoCommand request, CancellationToken cancellationToken)
	{
		await dispatcher.Publish(new EmailEvent { ToEmail = request.ToEmail }, cancellationToken);

		return Result.Success();
	}
}