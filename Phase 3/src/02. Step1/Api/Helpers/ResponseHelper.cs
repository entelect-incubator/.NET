namespace Api.Helpers;

using Common.Models.Results;

/// <summary>
/// Helper class for standardized API response handling.
/// Converts business results to HTTP responses with appropriate status codes.
/// </summary>
public static class ResponseHelper
{
	/// <summary>
	/// Converts a generic result to appropriate HTTP response.
	/// Returns NotFound if data is null, BadRequest if failed, otherwise Ok.
	/// </summary>
	public static ActionResult ResponseOutcome<T>(Result<T> result, ApiController controller)
		=> result.Data is null
			? controller.NotFound(Result.Failure($"{typeof(T).Name.Replace("Model", string.Empty)} not found"))
			: result.HasError
				? controller.BadRequest(result)
				: controller.Ok(result);

	/// <summary>
	/// Converts a list result to appropriate HTTP response.
	/// Returns BadRequest if failed, otherwise Ok.
	/// </summary>
	public static ActionResult ResponseOutcome<T>(Result<List<T>> result, ApiController controller)
		=> result.HasError ? controller.BadRequest(result) : controller.Ok(result);

	/// <summary>
	/// Converts a non-generic result to appropriate HTTP response.
	/// Returns BadRequest if failed, otherwise Ok.
	/// </summary>
	public static ActionResult ResponseOutcome(Result result, ApiController controller)
		=> result.HasError ? controller.BadRequest(result) : controller.Ok(result);
}
