namespace Api.Controllers;

using Core;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
public abstract class ApiController : ControllerBase
{
	private Dispatcher? dispatcher;

	protected Dispatcher Dispatcher => this.dispatcher ??= this.HttpContext.RequestServices.GetRequiredService<Dispatcher>();
}