namespace Common.Models.Results;

using Common.Enums;

public abstract class ResultBase
{
	public ErrorResults ErrorResult { get; set; } = ErrorResults.None;

	public string? Message { get; set; }

	public List<string> Errors { get; set; } = [];

	public Dictionary<string, List<string>> ValidationErrors { get; set; } = [];

	public void AddError(string error)
	{
		this.ErrorResult = ErrorResults.GeneralError;
		this.Errors.Add(error);
	}

	public void AddValidationError(string name, List<string> errors)
	{
		this.ErrorResult = ErrorResults.ValidationError;
		if (!this.ValidationErrors.TryGetValue(name, out var existing))
		{
			this.ValidationErrors[name] = errors;
		}
		else
		{
			existing.AddRange(errors);
		}
	}

	public bool HasError => this.ErrorResult != ErrorResults.None;
}
