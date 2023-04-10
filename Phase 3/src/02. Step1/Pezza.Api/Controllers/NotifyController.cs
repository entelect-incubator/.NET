namespace Api.Controllers;

using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Api.Helpers;
using Common.DTO;
using Common.Entities;
using Common.Models;
using Core.Notify.Commands;
using Core.Notify.Queries;

[ApiController]
public class NotifyController : ApiController
{
    /// <summary>
    /// Get Notify by Id.
    /// </summary>
    /// <param name="id">Primary Key.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    /// <response code="200">Get a notification</response>
    /// <response code="400">Error getting a notification</response>
    /// <response code="404">Notification not found</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Result<NotifyDTO>), 200)]
    [ProducesResponseType(typeof(ErrorResult), 400)]
    [ProducesResponseType(typeof(ErrorResult), 404)]
    public async Task<ActionResult> Get(int id)
    {
        var result = await this.Mediator.Send(new GetNotifyQuery { Id = id });

        return ResponseHelper.ResponseOutcome(result, this);
    }

    /// <summary>
    /// Get all Notifies.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    /// <response code="200">Notification Search</response>
    /// <response code="400">Error searching for notifications</response>
    [HttpPost]
    [ProducesResponseType(typeof(ListResult<NotifyDTO>), 200)]
    [ProducesResponseType(typeof(Result), 400)]
    [Route("Search")]
    public async Task<ActionResult> Search()
    {
        var result = await this.Mediator.Send(new GetNotifiesQuery());
        return ResponseHelper.ResponseOutcome(result, this);
    }

    /// <summary>
    /// Create Notify.
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///     POST api/Notify
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
    /// <param name="notify">Notification Data DTO.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    /// <response code="200">Notification created</response>
    /// <response code="400">Error creating a notification</response>
    [HttpPost]
    [ProducesResponseType(typeof(Result<NotifyDTO>), 200)]
    [ProducesResponseType(typeof(Result), 400)]
    [Route("Notify")]
    public async Task<ActionResult<Notify>> Create(NotifyDTO notify)
    {
        var result = await this.Mediator.Send(new CreateNotifyCommand
        {
            Data = notify
        });

        return ResponseHelper.ResponseOutcome(result, this);
    }

    /// <summary>
    /// Update Notify.
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///     PUT api/Notify
    ///     {
    ///       "customerId": "1",
    ///       "email": "person.a@gmail.com"
    ///     }.
    /// </remarks>
    /// <param name="notify">Notify Data DTO.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    /// <response code="200">Notification updated</response>
    /// <response code="400">Error updating a notification</response>
    /// <response code="404">Notification not found</response>
    [HttpPut]
    [ProducesResponseType(typeof(Result<NotifyDTO>), 200)]
    [ProducesResponseType(typeof(Result), 400)]
    [ProducesResponseType(typeof(Result), 404)]
    public async Task<ActionResult> Update(NotifyDTO notify)
    {
        var result = await this.Mediator.Send(new UpdateNotifyCommand
        {
            Data = notify
        });

        return ResponseHelper.ResponseOutcome(result, this);
    }

    /// <summary>
    /// Remove Notify by Id.
    /// </summary>
    /// <param name="id">Primary Key.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    /// <response code="200">Notification deleted</response>
    /// <response code="400">Error deleting a notification</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(Result), 200)]
    [ProducesResponseType(typeof(Result), 400)]
    public async Task<ActionResult> Delete(int id)
    {
        var result = await this.Mediator.Send(new DeleteNotifyCommand { Id = id });
        return ResponseHelper.ResponseOutcome(result, this);
    }
}
