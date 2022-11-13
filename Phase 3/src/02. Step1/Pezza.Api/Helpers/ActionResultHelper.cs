namespace Pezza.Api.Helpers;

using Microsoft.AspNetCore.Mvc;
using Pezza.Api.Controllers;
using Pezza.Common.Models;

public static class ResponseHelper
{
    public static ActionResult ResponseOutcome<T>(Result<T> result, ApiController controller)
    {
        if (result.Data == null)
        {
            return controller.NotFound();
        }

        if (!result.Succeeded)
        {
            return controller.BadRequest(result.Errors);
        }

        return controller.Ok(result.Data);
    }

    public static ActionResult ResponseOutcome<T>(ListResult<T> result, ApiController controller)
    {
        if (!result.Succeeded)
        {
            return controller.BadRequest(result.Errors);
        }

        return controller.Ok(result.Data);
    }

    public static ActionResult ResponseOutcome(Result result, ApiController controller)
    {
        if (!result.Succeeded)
        {
            return controller.BadRequest(result.Errors);
        }

        return controller.Ok(result.Succeeded);
    }
}
