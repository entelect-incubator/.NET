namespace Api.Helpers;

using Api.Controllers;

public static class ResponseHelper
{
	public static ActionResult ResponseOutcome(Result result, ApiController controller)
		=> !result.Succeeded ? controller.BadRequest(result) : controller.Ok(result);

	public static ActionResult ResponseOutcome<T>(Result<T> result, ApiController controller)
	{
		if (result.Data is null)
		{
			return controller.NotFound(Result.Failure($"{typeof(T).Name.Replace("Model", string.Empty)} not found"));
		}

		return !result.Succeeded ? controller.BadRequest(result) : controller.Ok(result);
	}
}