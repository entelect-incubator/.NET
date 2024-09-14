﻿namespace Api.Helpers;

public static class ResponseHelper
{
    public static ActionResult ResponseOutcome<T>(Result<T> result, ApiController controller)
    {
        if (result.Data == null)
        {
            return controller.NotFound(Result.Failure($"{typeof(T).Name.Replace("Model", string.Empty)} not found"));
        }

        if (!result.Succeeded)
        {
            return controller.BadRequest(result);
        }

        return controller.Ok(result);
    }

    public static ActionResult ResponseOutcome<T>(ListResult<T> result, ApiController controller)
    {
        if (!result.Succeeded)
        {
            return controller.BadRequest(result);
        }

        return controller.Ok(result);
    }

    public static ActionResult ResponseOutcome(Result result, ApiController controller)
    {
        if (!result.Succeeded)
        {
            return controller.BadRequest(result);
        }

        return controller.Ok(result);
    }
}
