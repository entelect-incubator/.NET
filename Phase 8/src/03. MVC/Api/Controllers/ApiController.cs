namespace Api.Controllers;

using Common.CQRS;
using Microsoft.Extensions.DependencyInjection;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
public abstract class ApiController : ControllerBase
{
	private Dispatcher? dispatcher;

	protected Dispatcher Dispatcher => this.dispatcher ??= this.HttpContext.RequestServices.GetRequiredService<Dispatcher>();
}