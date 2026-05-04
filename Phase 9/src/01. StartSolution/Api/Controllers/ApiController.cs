namespace Api.Controllers;

using MediatR;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
public abstract class ApiController : ControllerBase
{
    private IMediator? mediator;

    protected IMediator Mediator => this.mediator ??= HttpContext.RequestServices.GetRequiredService<IMediator>();
}
