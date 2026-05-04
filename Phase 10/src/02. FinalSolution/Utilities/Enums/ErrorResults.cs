namespace Utilities.Enums;

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