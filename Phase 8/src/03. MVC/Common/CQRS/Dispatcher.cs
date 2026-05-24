using Microsoft.Extensions.DependencyInjection;

namespace Common.CQRS;

public interface IRequest<TResult>
{
}

public interface ICommand<TResult> : IRequest<TResult>
{
}

public interface IQuery<TResult> : IRequest<TResult>
{
}

public interface INotification
{
}

public delegate Task<TResponse> RequestHandlerDelegate<TResponse>(CancellationToken cancellationToken = default);

public interface ICommandHandler<TCommand, TResult>
    where TCommand : ICommand<TResult>
{
    Task<TResult> Handle(TCommand command, CancellationToken cancellationToken);
}

public interface IQueryHandler<TQuery, TResult>
    where TQuery : IQuery<TResult>
{
    Task<TResult> Handle(TQuery query, CancellationToken cancellationToken);
}

public interface INotificationHandler<TNotification>
    where TNotification : INotification
{
    Task Handle(TNotification notification, CancellationToken cancellationToken);
}

public interface IPipelineBehavior<TRequest, TResponse>
{
    Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken);
}

public class Dispatcher(IServiceProvider provider)
{
    private readonly IServiceProvider provider = provider;

    public Task<TResult> Send<TResult>(ICommand<TResult> command, CancellationToken cancellationToken = default)
        => this.Send((dynamic)command, cancellationToken);

    public Task<TResult> Send<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = default)
        => this.Query((dynamic)query, cancellationToken);

    public Task<TResult> Send<TCommand, TResult>(TCommand command, CancellationToken cancellationToken = default)
        where TCommand : ICommand<TResult>
    {
        var handler = this.provider.GetRequiredService<ICommandHandler<TCommand, TResult>>();
        return this.ExecutePipeline(command, handler.Handle, cancellationToken);
    }

    public Task<TResult> Query<TQuery, TResult>(TQuery query, CancellationToken cancellationToken = default)
        where TQuery : IQuery<TResult>
    {
        var handler = this.provider.GetRequiredService<IQueryHandler<TQuery, TResult>>();
        return this.ExecutePipeline(query, handler.Handle, cancellationToken);
    }

    public async Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = default)
        where TNotification : INotification
    {
        var handlers = this.provider.GetServices<INotificationHandler<TNotification>>();
        foreach (var handler in handlers)
        {
            await handler.Handle(notification, cancellationToken);
        }
    }

    public Task Publish(INotification notification, CancellationToken cancellationToken = default)
        => this.Publish((dynamic)notification, cancellationToken);

    private Task<TResult> ExecutePipeline<TRequest, TResult>(TRequest request, Func<TRequest, CancellationToken, Task<TResult>> handler, CancellationToken cancellationToken)
    {
        var behaviors = this.provider.GetServices<IPipelineBehavior<TRequest, TResult>>().Reverse().ToArray();

        RequestHandlerDelegate<TResult> next = ct => handler(request, ct);

        foreach (var behavior in behaviors)
        {
            var capturedBehavior = behavior;
            var capturedNext = next;
            next = ct => capturedBehavior.Handle(request, capturedNext, ct);
        }

        return next(cancellationToken);
    }
}
