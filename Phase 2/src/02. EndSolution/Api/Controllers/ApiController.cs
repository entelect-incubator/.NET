namespace Api.Controllers;

using MediatR;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
public abstract class ApiController : ControllerBase
{
    private IMediator mediator;

	protected IMediator Mediator => this.mediator ??= this.HttpContext.RequestServices.GetService<IMediator>();
}