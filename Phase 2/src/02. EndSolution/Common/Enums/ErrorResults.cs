namespace Common.Enums;

public enum ErrorResults
{
	[Description("None")]
	None,

	[Description("Validation Error")]
	ValidationError,

	[Description("Not Found")]
	NotFound,

	[Description("Error")]
	GeneralError
}