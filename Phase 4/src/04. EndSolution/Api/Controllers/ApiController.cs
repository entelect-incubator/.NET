namespace Api.Controllers;

using Dispatch;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
public abstract class ApiController : ControllerBase
{
private Dispatcher? dispatcher;

protected Dispatcher Dispatcher => dispatcher ??= HttpContext.RequestServices.GetRequiredService<Dispatcher>();
}
