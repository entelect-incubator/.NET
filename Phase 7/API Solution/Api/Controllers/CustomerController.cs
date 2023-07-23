namespace Api.Controllers;

using Core.Customer.Commands;
using Core.Customer.Queries;

public class CustomerController : ApiController
{
	/// <summary>
	/// Get Customer by Id.
	/// </summary>
	/// <param name="id">int.</param>
	/// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
	/// <response code="200">Get a customer</response>
	/// <response code="400">Error getting a customer</response>
	/// <response code="404">Customer not found</response>
	[HttpGet("{id}")]
	[ProducesResponseType(typeof(Result<CustomerModel>), 200)]
	[ProducesResponseType(typeof(ErrorResult), 400)]
	[ProducesResponseType(typeof(ErrorResult), 404)]
	public async Task<ActionResult> GetCustomer(int id)
	{
		var result = await this.Mediator.Send(new GetCustomerQuery { Id = id });
		return ResponseHelper.ResponseOutcome(result, this);
	}

	/// <summary>
	/// Get all Customers.
	/// </summary>
	/// <returns>A <see cref="Task"/> repres
	/// enting the asynchronous operation.</returns>
	/// <response code="200">Customer Search</response>
	/// <response code="400">Error searching for customers</response>
	[HttpPost]
	[ProducesResponseType(typeof(ListResult<CustomerModel>), 200)]
	[ProducesResponseType(typeof(ErrorResult), 400)]
	[Route("Search")]
	public async Task<ActionResult> Search(SearchCustomerModel data)
	{
		var result = await this.Mediator.Send(new GetCustomersQuery()
		{
			Data = data
		});
		return ResponseHelper.ResponseOutcome(result, this);
	}

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
	/// <param name="model">CustomerModel.</param>
	/// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
	/// <response code="200">Customer created</response>
	/// <response code="400">Error creating a customer</response>
	[HttpPost]
	[ProducesResponseType(typeof(Result<CustomerModel>), 200)]
	[ProducesResponseType(typeof(ErrorResult), 400)]
	public async Task<ActionResult<CustomerModel>> Create(CreateCustomerModel model)
	{
		var result = await this.Mediator.Send(new CreateCustomerCommand
		{
			Data = model
		});

		return ResponseHelper.ResponseOutcome(result, this);
	}

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
	/// <param name="model">CustomerModel.</param>
	/// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
	/// <response code="200">Customer updated</response>
	/// <response code="400">Error updating a customer</response>
	/// <response code="404">Customer not found</response>
	[HttpPut]
	[ProducesResponseType(typeof(Result<CustomerModel>), 200)]
	[ProducesResponseType(typeof(ErrorResult), 400)]
	[ProducesResponseType(typeof(Result), 404)]
	public async Task<ActionResult> Update(UpdateCustomerModel model)
	{
		var result = await this.Mediator.Send(new UpdateCustomerCommand
		{
			Data = model
		});

		return ResponseHelper.ResponseOutcome(result, this);
	}

	/// <summary>
	/// Remove Customer by Id.
	/// </summary>
	/// <param name="id">int.</param>
	/// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
	/// <response code="200">Customer deleted</response>
	/// <response code="400">Error deleting a customer</response>
	[HttpDelete("{id}")]
	[ProducesResponseType(typeof(Result), 200)]
	[ProducesResponseType(typeof(ErrorResult), 400)]
	public async Task<ActionResult> Delete(int id)
	{
		var result = await this.Mediator.Send(new DeleteCustomerCommand { Id = id });
		return ResponseHelper.ResponseOutcome(result, this);
	}
}
