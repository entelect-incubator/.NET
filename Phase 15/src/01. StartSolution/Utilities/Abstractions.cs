using Utilities.CQRS;

public interface ICommand<TResponse> : Utilities.CQRS.ICommand<TResponse>
{
}

public interface IQuery<TResponse> : Utilities.CQRS.IQuery<TResponse>
{
}

public interface INotification : Utilities.CQRS.INotification
{
}

public interface ICommandHandler<TCommand, TResponse> : Utilities.CQRS.ICommandHandler<TCommand, TResponse>
    where TCommand : ICommand<TResponse>
{
}

public interface IQueryHandler<TQuery, TResponse> : Utilities.CQRS.IQueryHandler<TQuery, TResponse>
    where TQuery : IQuery<TResponse>
{
}

public interface INotificationHandler<TNotification> : Utilities.CQRS.INotificationHandler<TNotification>
    where TNotification : INotification
{
}

public class Result
{
    public bool Succeeded { get; set; }

    public List<string> Errors { get; set; } = [];

    public static Result Success()
        => new() { Succeeded = true };

    public static Result Failure(string error)
        => new() { Succeeded = false, Errors = [error] };
}

public class Result<T> : Result
{
    public T? Data { get; set; }

    public int? Count { get; set; }

    public static Result<T> Success(T data, int? count = null)
        => new() { Succeeded = true, Data = data, Count = count };

    public static new Result<T> Failure(string error)
        => new() { Succeeded = false, Errors = [error] };
}