namespace Api.Controllers;

using Microsoft.AspNetCore.Mvc;
using Utilities.CQRS;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
public abstract class ApiController : ControllerBase
{
    private Dispatcher? mediator;

    protected Dispatcher Mediator => this.mediator ??= HttpContext.RequestServices.GetRequiredService<Dispatcher>();
}
