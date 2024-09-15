namespace Core.Todos.Commands;

using Core.Todos.Events;

public class ExpiredTodoCommand : IRequest<Result>
{
	public required string ToEmail { get; set; }

	public required Guid SessionId { get; set; }
}

public class ExpiredTodoCommandHandler(IMediator mediator) : IRequestHandler<ExpiredTodoCommand, Result>
{
	public async Task<Result> Handle(ExpiredTodoCommand request, CancellationToken cancellationToken)
	{
		await mediator.Publish(new EmailEvent { ToEmail = request.ToEmail, SessionId = request.SessionId }, cancellationToken);

		return Result.Success();
	}
}