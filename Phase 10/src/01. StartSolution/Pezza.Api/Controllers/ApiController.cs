namespace Api.Controllers;


using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

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