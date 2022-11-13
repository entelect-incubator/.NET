namespace Pezza.Common.Models;

using System.Collections.Generic;
using System.Linq;

public class Result
{
    public Result()
    {
    }

    internal Result(bool succeeded, string error)
    {
        this.Succeeded = succeeded;

        this.Errors = new List<object>
        {
            error
        };
    }

    internal Result(bool succeeded, List<object> errors)
    {
        this.Succeeded = succeeded;
        this.Errors = errors;
    }

    internal Result(bool succeeded, List<string> errors)
    {
        this.Succeeded = succeeded;
        this.Errors = errors.ToList<object>();
    }

    public bool Succeeded { get; set; }

    public List<object> Errors { get; set; }

    public static Result Success() => new(true, new List<object> { });

    public static Result Failure(List<object> errors) => new(false, errors);

    public static Result Failure(List<string> errors) => new(false, errors);

    public static Result Failure(string error) => new(false, error);
}

public class Result<T>
{
    internal Result(bool succeeded, string error)
    {
        this.Succeeded = succeeded;
        this.Errors = new List<object>
        {
            error
        };
    }

    internal Result(bool succeeded, List<object> errors)
    {
        this.Succeeded = succeeded;
        this.Errors = errors;
    }

    internal Result(bool succeeded, T data, List<object> errors)
    {
        this.Succeeded = succeeded;
        this.Errors = errors;
        this.Data = data;
    }

    public bool Succeeded { get; set; }

    public T Data { get; set; }

    public List<object> Errors { get; set; }

    public static Result<T> Success(T data) => new(true, data, new List<object> { });

    public static Result<T> Failure(string error) => new(false, error);

    public static Result<T> Failure(List<object> errors) => new(false, errors);
}

public class ListResult<T>
{
    internal ListResult(bool succeeded, string error)
    {
        this.Succeeded = succeeded;
        this.Errors = new List<object>
        {
            error
        };
    }

    internal ListResult(bool succeeded, List<object> errors)
    {
        this.Succeeded = succeeded;
        this.Errors = errors;
    }

    internal ListResult(bool succeeded, List<T> data, int count, List<object> errors)
    {
        this.Succeeded = succeeded;
        this.Errors = errors;
        this.Data = data;
        this.Count = count;
    }

    internal ListResult(bool succeeded, IEnumerable<T> data, int count, List<object> errors)
    {
        this.Succeeded = succeeded;
        this.Errors = errors;
        this.Data = data.ToList();
        this.Count = count;
    }

    public bool Succeeded { get; set; }

    public List<T> Data { get; set; }

    public List<object> Errors { get; set; }

    public int Count { get; set; }

    public static ListResult<T> Success(List<T> data, int count) => new(true, data, count, new List<object> { });

    public static ListResult<T> Success(IEnumerable<T> data, int count) => new(true, data, count, new List<object> { });

    public static ListResult<T> Failure(string error) => new(false, error);

    public static ListResult<T> Failure(List<object> errors) => new(false, errors);
}

public class ListOutcome<T>
{
    public List<T> Data { get; set; }

    public int Count { get; set; }

    public List<string> Errors { get; set; }
}
