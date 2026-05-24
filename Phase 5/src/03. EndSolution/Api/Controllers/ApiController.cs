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

	protected ICommandMediator CmdMediator => this.cmdMediator ??= this.HttpContext.RequestServices.GetRequiredService<ICommandMediator>();

	protected IQueryMediator QryMediator => this.qryMediator ??= this.HttpContext.RequestServices.GetRequiredService<IQueryMediator>();
}