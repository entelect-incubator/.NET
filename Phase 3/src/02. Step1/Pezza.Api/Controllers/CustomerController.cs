namespace Api.Controllers;

using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Api.Helpers;
using Common.DTO;
using Common.Models;
using Core.Customer.Commands;
using Core.Customer.Queries;

[ApiController]
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
    [ProducesResponseType(typeof(Result<CustomerDTO>), 200)]
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
    [ProducesResponseType(typeof(ListResult<CustomerDTO>), 200)]
    [ProducesResponseType(typeof(Result), 400)]
    [Route("Search")]
    public async Task<ActionResult> Search()
    {
        var result = await this.Mediator.Send(new GetCustomersQuery());
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
    ///       "address": "1 Tree Street",
    ///       "city": "Pretoria",
    ///       "province": "Gautenf",
    ///       "PostalCode": "0181",
    ///       "phone": "0721230000",
    ///       "email": "person.a@gmail.com"
    ///       "contactPerson": "Person B 0723210000"
    ///     }.
    /// </remarks>
    /// <param name="customer">CustomerDTO.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    /// <response code="200">Customer created</response>
    /// <response code="400">Error creating a customer</response>
    [HttpPost]
    [ProducesResponseType(typeof(Result<CustomerDTO>), 200)]
    [ProducesResponseType(typeof(Result), 400)]
    public async Task<ActionResult<CustomerDTO>> Create(CustomerDTO customer)
    {
        var result = await this.Mediator.Send(new CreateCustomerCommand
        {
            Data = customer
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
    ///       "name": "Person A",
    ///       "address": "1 Tree Street",
    ///       "city": "Pretoria",
    ///       "province": "Gautenf",
    ///       "PostalCode": "0181",
    ///       "phone": "0721230000",
    ///       "email": "person.a@gmail.com"
    ///       "contactPerson": "Person B 0723210000"
    ///     }.
    /// </remarks>
    /// <param name="customer">CustomerDTO.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    /// <response code="200">Customer updated</response>
    /// <response code="400">Error updating a customer</response>
    /// <response code="404">Customer not found</response>
    [HttpPut]
    [ProducesResponseType(typeof(Result<CustomerDTO>), 200)]
    [ProducesResponseType(typeof(Result), 400)]
    [ProducesResponseType(typeof(Result), 404)]
    public async Task<ActionResult> Update(CustomerDTO customer)
    {
        var result = await this.Mediator.Send(new UpdateCustomerCommand
        {
            Data = customer
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
    [ProducesResponseType(typeof(Result), 400)]
    public async Task<ActionResult> Delete(int id)
    {
        var result = await this.Mediator.Send(new DeleteCustomerCommand { Id = id });
        return ResponseHelper.ResponseOutcome(result, this);
    }
}
