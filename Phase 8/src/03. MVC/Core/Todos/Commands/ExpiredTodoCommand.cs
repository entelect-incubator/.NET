namespace Core.Todos.Commands;

using Core.Todos.Events;

public class ExpiredTodoCommand : IRequest<Result>
{
	public required string ToEmail { get; set; }
}

public class ExpiredTodoCommandHandler(IMediator mediator) : IRequestHandler<ExpiredTodoCommand, Result>
{
	public async Task<Result> Handle(ExpiredTodoCommand request, CancellationToken cancellationToken)
	{
		await mediator.Publish(new EmailEvent { ToEmail = request.ToEmail }, cancellationToken);

		return Result.Success();
	}
}