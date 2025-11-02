namespace Api.Controllers;

using LiteBus.Commands.Abstractions;
using LiteBus.Queries.Abstractions;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
public abstract class ApiController : ControllerBase
{
	private ICommandMediator cmdMediator;
	private IQueryMediator qryMediator;

	protected ICommandMediator CmdMediator => cmdMediator ??= HttpContext.RequestServices.GetRequiredService<ICommandMediator>();

	protected IQueryMediator QryMediator => qryMediator ??= HttpContext.RequestServices.GetRequiredService<IQueryMediator>();
}