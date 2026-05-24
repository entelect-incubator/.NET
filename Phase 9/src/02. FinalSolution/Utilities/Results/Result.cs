namespace Utilities.Results;

using Utilities.Enums;

public class Result : ResultBase
{
    public static Result Success(string? message = null)
        => new() { Message = message };

    public static Result Failure(List<string>? errors = null, string? message = null)
        => new() { ErrorResult = ErrorResults.GeneralError, Errors = errors ?? [], Message = message };

    public static Result Failure(Exception exception)
        => Failure(exception?.Message, exception?.InnerException?.Message);

    public static Result Failure(string? error, string? message = null)
        => Failure(error is null ? [] : [error], message);

    public static Result ValidationFailure(Dictionary<string, List<string>>? validationErrors = null, string? message = null)
        => new() { ErrorResult = ErrorResults.ValidationError, ValidationErrors = validationErrors ?? [], Message = message };

    public static Result NotFound(string? message = null)
        => new() { ErrorResult = ErrorResults.NotFound, Message = message };
}

public class Result<T> : ResultBase
{
    public T? Data { get; set; }

    public int Count { get; set; }

    public static Result<T> Success(T data, int count = 0, string? message = null)
        => new() { Data = data, Count = count, Message = message };

    public static Result<T> Failure(List<string>? errors = null, string? message = null)
        => new() { ErrorResult = ErrorResults.GeneralError, Errors = errors ?? [], Message = message };

    public static Result<T> Failure(string error, string? message = null)
        => Failure([error], message);

    public static Result<T> Failure(List<string>? errors = null)
        => Failure(errors);

    public static Result<T> ValidationFailure(Dictionary<string, List<string>>? validationErrors = null, string? message = null, T? data = default)
        => new() { ErrorResult = ErrorResults.ValidationError, ValidationErrors = validationErrors ?? [], Message = message, Data = data };

    public static Result<T> NotFound(string? message = null)
        => new() { ErrorResult = ErrorResults.NotFound, Message = message };
}
