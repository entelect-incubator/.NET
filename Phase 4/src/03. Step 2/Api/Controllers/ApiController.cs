namespace Api.Controllers;

using Utilities.CQRS;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
public abstract class ApiController : ControllerBase
{
	private Dispatcher mediator;

	protected Dispatcher Mediator => this.mediator ??= this.HttpContext.RequestServices.GetService<Dispatcher>()!;
}