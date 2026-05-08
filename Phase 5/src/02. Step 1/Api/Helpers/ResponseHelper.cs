namespace Api.Helpers;

using Microsoft.AspNetCore.Mvc;

public static class ResponseHelper
{
	public static ActionResult ResponseOutcome<T>(Result<T> result, ControllerBase controller)
	{
		return result.Data == null
			? controller.NotFound(Result.Failure($"{typeof(T).Name.Replace("Model", string.Empty)} not found"))
			: !result.Succeeded ? controller.BadRequest(result) : controller.Ok(result);
	}

	public static ActionResult ResponseOutcome<T>(Result<IEnumerable<T>> result, ControllerBase controller)
	{
		return !result.Succeeded ? controller.BadRequest(result) : controller.Ok(result);
	}

	public static ActionResult ResponseOutcome<T>(ListResult<T> result, ControllerBase controller)
	{
		return !result.Succeeded ? controller.BadRequest(result) : controller.Ok(result);
	}

	public static ActionResult ResponseOutcome(Result result, ControllerBase controller)
	{
		return !result.Succeeded ? controller.BadRequest(result) : controller.Ok(result);
	}
}
