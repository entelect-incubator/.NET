namespace Utilities.Results;

using System.ComponentModel;

public class ErrorResult : Result
{
	public ErrorResult()
		=> (this.IsError, this.IsValidationError) = (true, false);

	[DefaultValue(true)]
	public bool IsError { get; set; }

	[DefaultValue(false)]
	public bool IsValidationError { get; set; }
}

public class ValidationErrorResult : Result
{
	public ValidationErrorResult()
		=> (this.IsError, this.IsValidationError) = (false, true);

	[DefaultValue(false)]
	public bool IsError { get; set; }

	[DefaultValue(true)]
	public bool IsValidationError { get; set; }
}

public class NotFoundErrorResult : Result
{
	public NotFoundErrorResult()
		=> this.IsNotFound = true;

	[DefaultValue(true)]
	public bool IsNotFound { get; set; }
}