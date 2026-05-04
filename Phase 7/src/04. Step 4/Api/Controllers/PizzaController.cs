namespace Api.Controllers;

using Common.Models.Pizza;
using Core;
using Core.Pizza.Commands;
using Core.Pizza.Queries;

[ApiController]
[Route("[controller]")]
public class PizzaController(Dispatcher dispatcher) : ApiController(dispatcher)
{
	/// <summary>
	/// Get Pizza by Id.
	/// </summary>
	/// <param name="id">Pizza Id</param>
	/// <returns>ActionResult</returns>
	[HttpGet("{id}")]
	[ProducesResponseType(200)]
	[ProducesResponseType(404)]
	public async Task<ActionResult> Get(int id)
	{
		var result = await this.Dispatcher.Query<GetPizzaQuery, Result<PizzaModel>>(new GetPizzaQuery { Id = id }, CancellationToken.None);
		return ResponseHelper.ResponseOutcome(result, this);
	}

	/// <summary>
	/// Get all Pizzas.
	/// </summary>
	/// <returns>ActionResult</returns>
	[HttpPost("Search")]
	[ProducesResponseType(200)]
	public async Task<ActionResult> Search(SearchPizzaModel data)
	{
		var result = await this.Dispatcher.Query<GetPizzasQuery, Result<IEnumerable<PizzaModel>>>(new GetPizzasQuery()
		{
			Data = data
		}, CancellationToken.None);
		return ResponseHelper.ResponseOutcome(result, this);
	}

	/// <summary>
	/// Create Pizza.
	/// </summary>
	/// <remarks>
	/// Sample request:
	///
	///     POST /Pizza
	///     {
	///       "name": "Hawaiian",
	///       "description": "Hawaiian pizza is a pizza originating in Canada, and is traditionally topped with pineapple, tomato sauce, cheese, and either ham or bacon.",
	///       "price": "99"
	///     }
	/// </remarks>
	/// <param name="model">Pizza Model</param>
	/// <returns>ActionResult</returns>
	[HttpPost]
	[ProducesResponseType(200)]
	[ProducesResponseType(400)]
	public async Task<ActionResult<Pizza>> Create([FromBody] CreatePizzaModel model)
	{
		var result = await this.Dispatcher.Send<CreatePizzaCommand, Result<PizzaModel>>(new CreatePizzaCommand
		{
			Data = model
		}, CancellationToken.None);

		return ResponseHelper.ResponseOutcome(result, this);
	}

	/// <summary>
	/// Update Pizza.
	/// </summary>
	/// <remarks>
	/// Sample request:
	///
	///     PUT /Pizza/1
	///     {
	///       "price": "119"
	///     }
	/// </remarks>
	/// <param name="model">Pizza Model</param>
	/// <returns>ActionResult</returns>
	[HttpPut]
	[ProducesResponseType(200)]
	[ProducesResponseType(400)]
	public async Task<ActionResult> Update([FromBody] UpdatePizzaModel model)
	{
		var result = await this.Dispatcher.Send<UpdatePizzaCommand, Result<PizzaModel>>(new UpdatePizzaCommand
		{
			Data = model
		}, CancellationToken.None);

		return ResponseHelper.ResponseOutcome(result, this);
	}

	/// <summary>
	/// Delete Pizza by Id.
	/// </summary>
	/// <param name="id">Pizza Id</param>
	/// <returns>ActionResult</returns>
	[HttpDelete("{id}")]
	[ProducesResponseType(200)]
	[ProducesResponseType(400)]
	public async Task<ActionResult> Delete(int id)
	{
		var result = await this.Dispatcher.Send<DeletePizzaCommand, Result>(new DeletePizzaCommand { Id = id }, CancellationToken.None);
		return ResponseHelper.ResponseOutcome(result, this);
	}
}
