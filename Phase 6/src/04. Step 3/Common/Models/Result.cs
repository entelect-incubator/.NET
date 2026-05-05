namespace Common.Models;

using System.ComponentModel;

public class Result
{
	public bool Succeeded { get; set; }

	public List<string>? Errors { get; set; }

	public static Result Success() => new()
	{
		Succeeded = true
	};

	public static Result Failure(List<string> errors) => new()
	{
		Succeeded = false,
		Errors = errors
	};

	public static Result Failure(string error) => new()
	{
		Succeeded = false,
		Errors = [error]
	};
}

public class Result<T>
{
	public bool Succeeded { get; set; }

	public T? Data { get; set; }

	public int? Count { get; set; }

	public List<string>? Errors { get; set; }

	public static Result<T> Success(T data, int? count = 0) => new()
	{
		Data = data,
		Count = count,
		Succeeded = true
	};

	public static Result<T> Failure(string error) => new()
	{
		Errors = [error],
		Succeeded = false
	};

	public static Result<T> Failure(List<string> errors) => new()
	{
		Errors = errors,
		Succeeded = false
	};
}

public class ErrorResult : Result
{
	public ErrorResult() => this.Succeeded = false;

	[DefaultValue(false)]
	public new bool Succeeded { get; set; }
}