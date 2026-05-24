namespace Api.Controllers;

using Common.Models.Order;
using Core;
using Core.Order.Commands;

[ApiController]
[Route("[controller]")]
public class OrderController(Dispatcher dispatcher) : ApiController(dispatcher)
{
	/// <summary>
	/// Order Pizza.
	/// </summary>
	/// <remarks>
	/// Sample request:
	///
	///     POST /Order
	///     {
	///       "customerId": 1,
	///       "pizzaIds": [1, 2, 3, 4, 5]
	///     }
	/// </remarks>
	/// <param name="model">Create Order Model</param>
	/// <returns>ActionResult</returns>
	[HttpPost]
	[ProducesResponseType(200)]
	[ProducesResponseType(400)]
	public async Task<ActionResult<OrderModel>> Create([FromBody] OrderModel model)
	{
		var result = await this.Dispatcher.Send<OrderCommand, Result>(
			new OrderCommand
			{
				Data = model
			},
			CancellationToken.None);

		return ResponseHelper.ResponseOutcome(result, this);
	}
}
