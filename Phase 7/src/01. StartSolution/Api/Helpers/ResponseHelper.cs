namespace Api.Helpers;

public static class ResponseHelper
{
	public static ActionResult ResponseOutcome<T>(Result<T> result, ApiController controller)
		=> result.Data == null
			? controller.NotFound(Result.Failure($"{typeof(T).Name.Replace("Model", string.Empty)} not found"))
			: !result.Succeeded ? controller.BadRequest(result) : controller.Ok(result);

	public static ActionResult ResponseOutcome<T>(Result<IEnumerable<T>> result, ApiController controller)
		=> !result.Succeeded ? controller.BadRequest(result) : controller.Ok(result);

	public static ActionResult ResponseOutcome(Result result, ApiController controller)
		=> !result.Succeeded ? controller.BadRequest(result) : controller.Ok(result);
}
