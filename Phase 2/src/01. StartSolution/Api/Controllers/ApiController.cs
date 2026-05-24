namespace Api.Controllers;

using Core;

/// <summary>
/// Base API controller providing access to the custom Dispatcher.
/// All controllers should inherit from this to use the Dispatcher for commands and queries.
/// </summary>
[ApiController]
[Route("[controller]")]
[Produces("application/json")]
public abstract class ApiController : ControllerBase
{
	private Dispatcher? dispatcher;

	/// <summary>
	/// Gets the custom dispatcher for sending commands and queries.
	/// </summary>
	protected Dispatcher Dispatcher => this.dispatcher ??= this.HttpContext.RequestServices.GetRequiredService<Dispatcher>();
}
