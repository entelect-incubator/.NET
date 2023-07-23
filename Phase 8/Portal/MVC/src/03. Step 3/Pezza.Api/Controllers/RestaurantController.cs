namespace Api.Controllers;

using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Api.Helpers;
using Common.DTO;
using Common.Entities;
using Common.Models;
using Core.Restaurant.Commands;
using Core.Restaurant.Queries;

[ApiController]
public class RestaurantController : ApiController
{
    /// <summary>
    /// Get Restaurant by Id.
    /// </summary>
    /// <param name="id">Id.</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    /// <response code="200">Get a restaurant.</response>
    /// <response code="400">Error getting a restaurant.</response>
    /// <response code="404">Restaurant not found.</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Result<RestaurantDTO>), 200)]
    [ProducesResponseType(typeof(ErrorResult), 400)]
    [ProducesResponseType(typeof(ErrorResult), 404)]
    public async Task<ActionResult> Get(int id, CancellationToken cancellationToken = default)
    {
        var result = await this.Mediator.Send(new GetRestaurantQuery { Id = id }, cancellationToken);
        return ResponseHelper.ResponseOutcome(result, this);
    }

    /// <summary>
    /// Get all Restaurants.
    /// </summary>
    /// <param name="dto">RestaurantDTO.</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    /// <response code="200">Restaurant Search.</response>
    /// <response code="400">Error searching for restaurants.</response>
    [HttpPost]
    [ProducesResponseType(typeof(ListResult<RestaurantDTO>), 200)]
    [ProducesResponseType(typeof(ErrorResult), 400)]
    [Route("Search")]
    public async Task<ActionResult> Search(RestaurantDTO dto, CancellationToken cancellationToken = default)
    {
        var result = await this.Mediator.Send(
            new GetRestaurantsQuery()
            {
                Data = dto,
            }, cancellationToken);
        return ResponseHelper.ResponseOutcome(result, this);
    }

    /// <summary>
    /// Create Stock.
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST api/Restaurant
    ///     {
    ///       "name": "Restaurant 1",
    ///       "description": "",
    ///       "pictureUrl": "base64",
    ///       "isActive": true
    ///       "Address": {
    ///         "city": "Pretoria",
    ///         "province": "Gauteng",
    ///         "PostalCode": "0000"
    ///       }
    ///     }.
    /// </remarks>
    /// <param name="data">RestaurantDTO.</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    /// <response code="200">Restaurant created.</response>
    /// <response code="400">Error creating a restaurant.</response>
    [HttpPost]
    [ProducesResponseType(typeof(Result<RestaurantDTO>), 200)]
    [ProducesResponseType(typeof(ErrorResult), 400)]
    public async Task<ActionResult<Restaurant>> Create(RestaurantDTO data, CancellationToken cancellationToken = default)
    {
        if (!string.IsNullOrEmpty(data.ImageData))
        {
            var imageResult = await MediaHelper.UploadMediaAsync("restaurant", data.ImageData);
            if (imageResult != null)
            {
                data.PictureUrl = imageResult.Data.Path;
            }
        }

        var result = await this.Mediator.Send(
            new CreateRestaurantCommand
            {
                Data = data,
            }, cancellationToken);

        return ResponseHelper.ResponseOutcome(result, this);
    }

    /// <summary>
    /// Update Restaurant.
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     PUT api/Restaurant
    ///     {
    ///       "id": 1,
    ///       "name": "New Restaurant"
    ///     }.
    /// </remarks>
    /// <param name="data">RestaurantDTO.</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    /// <response code="200">Restaurant updated.</response>
    /// <response code="400">Error updating a restaurant.</response>
    /// <response code="404">Restaurant not found.</response>
    [HttpPut]
    [ProducesResponseType(typeof(Result<RestaurantDTO>), 200)]
    [ProducesResponseType(typeof(ErrorResult), 400)]
    [ProducesResponseType(typeof(Result), 404)]
    public async Task<ActionResult> Update(RestaurantDTO data, CancellationToken cancellationToken = default)
    {
        if (!string.IsNullOrEmpty(data.ImageData))
        {
            var imageResult = await MediaHelper.UploadMediaAsync("restaurant", data.ImageData);
            if (imageResult != null)
            {
                data.PictureUrl = imageResult.Data.Path;
            }
        }

        var result = await this.Mediator.Send(
            new UpdateRestaurantCommand
            {
                Data = data,
            }, cancellationToken);

        return ResponseHelper.ResponseOutcome(result, this);
    }

    /// <summary>
    /// Remove Restaurant by Id.
    /// </summary>
    /// <param name="id">int.</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    /// <response code="200">Restaurant deleted.</response>
    /// <response code="400">Error deleting a restaurant.</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(Result), 200)]
    [ProducesResponseType(typeof(ErrorResult), 400)]
    public async Task<ActionResult> Delete(int id, CancellationToken cancellationToken = default)
    {
        var result = await this.Mediator.Send(new DeleteRestaurantCommand { Id = id }, cancellationToken);
        return ResponseHelper.ResponseOutcome(result, this);
    }
}
