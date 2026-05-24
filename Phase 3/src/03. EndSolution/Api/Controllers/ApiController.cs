namespace Api.Controllers;

using Utilities.CQRS;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
public abstract class ApiController : ControllerBase
{
	[FromServices]
	public Dispatcher Dispatcher { get; set; } = default!;
}