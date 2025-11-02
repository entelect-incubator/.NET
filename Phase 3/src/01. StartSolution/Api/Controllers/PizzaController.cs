namespace Api.Controllers;

using Core.Pizza.Commands;
using Core.Pizza.Queries;

[ApiController]
[Route("[controller]")]
public sealed class PizzaController : ApiController
{
	/// <summary>
	/// Get Pizza by Id.
	/// </summary>
	/// <param name="query"></param>
	/// <param name="id">Pizza Id</param>
	/// <param name="cancellationToken"></param>
	/// <returns>ActionResult</returns>
	[HttpGet("{id}")]
	[ProducesResponseType(200)]
	[ProducesResponseType(404)]
	public async Task<ActionResult> Get([FromServices] IGetPizzaQuery query, int id, CancellationToken cancellationToken)
		=> ResponseHelper.ResponseOutcome(await query.ExecuteAsync(id, cancellationToken), this);

	/// <summary>
	/// Get all Pizzas.
	/// </summary>
	/// <param name="query"></param>
	/// <param name="cancellationToken"></param>
	/// <returns>ActionResult</returns>
	[HttpPost("Search")]
	[ProducesResponseType(200)]
	public async Task<ActionResult> Search([FromServices] IGetPizzasQuery query, CancellationToken cancellationToken)
		=> ResponseHelper.ResponseOutcome(await query.ExecuteAsync(cancellationToken), this);

	/// <summary>
	/// Create Pizza.
	/// </summary>
	/// <remarks>
	/// Sample request:
	///
	///     POST api/Pizza
	///     {
	///       "name": "Hawaiian",
	///       "description": "Hawaiian pizza is a pizza originating in Canada, and is traditionally topped with pineapple, tomato sauce, cheese, and either ham or bacon.",
	///       "price": "99"
	///     }
	/// </remarks>
	/// <param name="command"></param>
	/// <param name="model">Pizza Model</param>
	/// <param name="cancellationToken"></param>
	/// <returns>ActionResult</returns>
	[HttpPost]
	[ProducesResponseType(200)]
	[ProducesResponseType(400)]
	public async Task<ActionResult<Pizza>> Create([FromServices] ICreatePizzaCommand command, [FromBody] CreatePizzaModel model, CancellationToken cancellationToken)
		=> ResponseHelper.ResponseOutcome(await command.ExecuteAsync(model, cancellationToken), this);


	/// <summary>
	/// Update Pizza.
	/// </summary>
	/// <remarks>
	/// Sample request:
	///
	///     PUT api/Pizza/1
	///     {
	///       "price": "119"
	///     }
	/// </remarks>
	/// <param name="command"></param>
	/// <param name="id"></param>
	/// <param name="model">Pizza Model</param>
	/// <param name="cancellationToken"></param>
	/// <returns>ActionResult</returns>
	[HttpPut("{id}")]
	[ProducesResponseType(200)]
	[ProducesResponseType(400)]
	public async Task<ActionResult> Update([FromServices] IUpdatePizzaCommand command, int id, [FromBody] UpdatePizzaModel model, CancellationToken cancellationToken)
		=> ResponseHelper.ResponseOutcome(await command.ExecuteAsync(id, model, cancellationToken), this);

	/// <summary>
	/// Delete Pizza by Id.
	/// </summary>
	/// <param name="command"></param>
	/// <param name="id">Pizza Id</param>
	/// <param name="cancellationToken"></param>
	/// <returns>ActionResult</returns>
	[HttpDelete("{id}")]
	[ProducesResponseType(200)]
	[ProducesResponseType(400)]
	public async Task<ActionResult> Delete([FromServices] IDeletePizzaCommand command, int id, CancellationToken cancellationToken)
		=> ResponseHelper.ResponseOutcome(await command.ExecuteAsync(id, cancellationToken), this);
}
