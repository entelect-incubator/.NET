namespace Api.Controllers;

using Core;
using Microsoft.AspNetCore.Mvc;

/// <summary>
/// Base API controller with dispatcher support for CQRS operations.
/// </summary>
[ApiController]
[Route("[controller]")]
public abstract class ApiController(Dispatcher dispatcher) : ControllerBase
{
	/// <summary>
	/// Gets the dispatcher for executing commands and queries.
	/// </summary>
	protected Dispatcher Dispatcher { get; } = dispatcher;
}
