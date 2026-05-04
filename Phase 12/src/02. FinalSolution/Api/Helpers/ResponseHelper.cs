namespace Api.Helpers;

using Api.Controllers;
using Microsoft.AspNetCore.Mvc;

public static class ResponseHelper
{
    public static ActionResult ResponseOutcome<T>(Result<T> result, ApiController controller)
    {
        return result.Data == null
            ? (ActionResult)controller.NotFound()
            : !result.Succeeded ? controller.BadRequest(result.Errors) : controller.Ok(result.Data);
    }

    public static ActionResult ResponseOutcome<T>(Result<IEnumerable<T>> result, ApiController controller)
    {
        return !result.Succeeded ? controller.BadRequest(result.Errors) : controller.Ok(result.Data);
    }

    public static ActionResult ResponseOutcome(Result result, ApiController controller)
    {
        return !result.Succeeded ? controller.BadRequest(result.Errors) : controller.Ok(result.Succeeded);
    }
}
