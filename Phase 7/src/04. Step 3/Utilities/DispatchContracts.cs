namespace Dispatch;

public interface IPipelineBehavior<TRequest, TResult>
{
	Task<TResult> Handle(TRequest request, Func<Task<TResult>> next, CancellationToken ct);
}
