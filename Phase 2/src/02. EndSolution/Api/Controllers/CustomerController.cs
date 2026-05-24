namespace Api.Controllers;

using Common.Models.Customer;
using Common.Models.Results;
using Core.Customer.Commands;
using Core.Customer.Queries;

public sealed class CustomerController : ApiController
{
	/// <summary>
	/// Get Customer by Id.
	/// </summary>
	/// <param name="query"></param>
	/// <param name="id">int.</param>
	/// <param name="cancellationToken"></param>
	/// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
	/// <response code="200">Get a customer</response>
	/// <response code="400">Error getting a customer</response>
	/// <response code="404">Customer not found</response>
	[HttpGet("{id}")]
	[ProducesResponseType(typeof(Result<CustomerModel>), 200)]
	[ProducesResponseType(typeof(ErrorResult), 400)]
	[ProducesResponseType(typeof(ErrorResult), 404)]
	public async Task<ActionResult> GetCustomer([FromServices] IGetCustomerQuery query, int id, CancellationToken cancellationToken)
		=> ResponseHelper.ResponseOutcome(await query.ExecuteAsync(id, cancellationToken), this);

	/// <summary>
	/// Get all Customers.
	/// </summary>
	/// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
	/// <param name="query"></param>
	/// <param name="cancellationToken"></param>
	/// <response code="200">Customer Search</response>
	/// <response code="400">Error searching for customers</response>
	[HttpPost]
	[ProducesResponseType(typeof(Result<IEnumerable<CustomerModel>>), 200)]
	[ProducesResponseType(typeof(ErrorResult), 400)]
	[Route("Search")]
	public async Task<ActionResult> Search([FromServices] IGetCustomersQuery query, CancellationToken cancellationToken)
		=> ResponseHelper.ResponseOutcome(await query.ExecuteAsync(cancellationToken), this);

	/// <summary>
	/// Create Customer.
	/// </summary>
	/// <remarks>
	/// Sample request:
	///     POST api/Customer
	///     {
	///       "name": "Person A",
	///       "address": "1 Tree Street, Pretoria, Gauteng",
	///       "email": "person.a@gmail.com"
	///       "cellphone": "0721230000"
	///     }.
	/// </remarks>
	/// <param name="command"></param>
	/// <param name="model">CustomerModel.</param>
	/// <param name="cancellationToken"></param>
	/// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
	/// <response code="200">Customer created</response>
	/// <response code="400">Error creating a customer</response>
	[HttpPost]
	[ProducesResponseType(typeof(Result<CustomerModel>), 200)]
	[ProducesResponseType(typeof(ErrorResult), 400)]
	public async Task<ActionResult<CustomerModel>> Create([FromServices] ICreateCustomerCommand command, CreateCustomerModel model, CancellationToken cancellationToken)
		=> ResponseHelper.ResponseOutcome(await command.ExecuteAsync(model, cancellationToken), this);

	/// <summary>
	/// Update Customer.
	/// </summary>
	/// <remarks>
	/// Sample request:
	///     PUT api/Customer
	///     {
	///       "id": 1,
	///       "email": "person.a@gmail.com"
	///     }.
	/// </remarks>
	/// <param name="command"></param>
	/// <param name="id"></param>
	/// <param name="model">CustomerModel.</param>
	/// <param name="cancellationToken"></param>
	/// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
	/// <response code="200">Customer updated</response>
	/// <response code="400">Error updating a customer</response>
	/// <response code="404">Customer not found</response>
	[HttpPut("{id}")]
	[ProducesResponseType(typeof(Result<CustomerModel>), 200)]
	[ProducesResponseType(typeof(ErrorResult), 400)]
	[ProducesResponseType(typeof(Result), 404)]
	public async Task<ActionResult> Update([FromServices] IUpdateCustomerCommand command, int id, UpdateCustomerModel model, CancellationToken cancellationToken)
		=> ResponseHelper.ResponseOutcome(await command.ExecuteAsync(id, model, cancellationToken), this);

	/// <summary>
	/// Remove Customer by Id.
	/// </summary>
	/// <param name="command"></param>
	/// <param name="id">int.</param>
	/// <param name="cancellationToken"></param>
	/// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
	/// <response code="200">Customer deleted</response>
	/// <response code="400">Error deleting a customer</response>
	[HttpDelete("{id}")]
	[ProducesResponseType(typeof(Result), 200)]
	[ProducesResponseType(typeof(ErrorResult), 400)]
	public async Task<ActionResult> Delete([FromServices] IDeleteCustomerCommand command, int id, CancellationToken cancellationToken)
		=> ResponseHelper.ResponseOutcome(await command.ExecuteAsync(id, cancellationToken), this);
}
