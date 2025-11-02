namespace Api.Controllers;

using LiteBus.Commands.Abstractions;
using LiteBus.Queries.Abstractions;

/// <summary>
/// Base API controller providing access to LiteBus command and query mediators.
/// All controllers should inherit from this to use CmdMediator and QryMediator.
/// </summary>
[ApiController]
[Route("[controller]")]
[Produces("application/json")]
public abstract class ApiController : ControllerBase
{
	private ICommandMediator? cmdMediator;
	private IQueryMediator? qryMediator;

	/// <summary>
	/// Gets the command mediator for dispatching commands.
	/// </summary>
	protected ICommandMediator CmdMediator => cmdMediator ??= HttpContext.RequestServices.GetRequiredService<ICommandMediator>();

	/// <summary>
	/// Gets the query mediator for dispatching queries.
	/// </summary>
	protected IQueryMediator QryMediator => qryMediator ??= HttpContext.RequestServices.GetRequiredService<IQueryMediator>();
}
