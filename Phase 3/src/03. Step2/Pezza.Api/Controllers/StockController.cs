namespace Api.Controllers;

using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Api.Helpers;
using Common.DTO;
using Common.Entities;
using Common.Models;
using Core.Stock.Commands;
using Core.Stock.Queries;

[ApiController]
public class StockController : ApiController
{
    /// <summary>
    /// Get Stock by Id.
    /// </summary>
    /// <param name="id">id.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    /// <response code="200">Get a pizza</response>
    /// <response code="400">Error getting a pizza</response>
    /// <response code="404">Stock not found</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Result<StockDTO>), 200)]
    [ProducesResponseType(typeof(ErrorResult), 400)]
    [ProducesResponseType(typeof(ErrorResult), 404)]
    public async Task<ActionResult> Get(int id)
    {
        var result = await this.Mediator.Send(new GetStockQuery { Id = id });
        return ResponseHelper.ResponseOutcome(result, this);
    }

    /// <summary>
    /// Get all Stock.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    /// <response code="200">Stock Search</response>
    /// <response code="400">Error searching for pizza</response>
    [HttpPost]
    [ProducesResponseType(typeof(ListResult<StockDTO>), 200)]
    [ProducesResponseType(typeof(ErrorResult), 400)]
    [Route("Search")]
    public async Task<ActionResult> Search(StockDTO dto)
    {
        var result = await this.Mediator.Send(new GetStocksQuery
        {
            Data = dto
        });
        return ResponseHelper.ResponseOutcome(result, this);
    }

    /// <summary>
    /// Create Stock.
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///     POST api/Stock
    ///     {
    ///       "name": "Tomatoes",
    ///       "unitOfMeasure": "Kg",
    ///       "valueOfMeasure": "1",
    ///       "quantity": "50"
    ///       "comment": ""
    ///     }.
    /// </remarks>
    /// <param name="data">StockDTO.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    /// <response code="200">Stock created</response>
    /// <response code="400">Error creating a pizza</response>
    [HttpPost]
    [ProducesResponseType(typeof(Result<StockDTO>), 200)]
    [ProducesResponseType(typeof(ErrorResult), 400)]
    public async Task<ActionResult<Stock>> Create(StockDTO data)
    {
        var result = await this.Mediator.Send(new CreateStockCommand
        {
            Data = data
        });

        return ResponseHelper.ResponseOutcome(result, this);
    }

    /// <summary>
    /// Update Stock.
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     PUT api/Stock
    ///     {
    ///       "id": 1
    ///       "quantity": 30
    ///     }.
    /// </remarks>
    /// <param name="data">StockDTO.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    /// <response code="200">Stock updated</response>
    /// <response code="400">Error updating a pizza</response>
    /// <response code="404">Stock not found</response>
    [HttpPut]
    [ProducesResponseType(typeof(Result<StockDTO>), 200)]
    [ProducesResponseType(typeof(ErrorResult), 400)]
    [ProducesResponseType(typeof(Result), 404)]
    public async Task<ActionResult> Update(StockDTO data)
    {
        var result = await this.Mediator.Send(new UpdateStockCommand
        {
            Data = data
        });

        return ResponseHelper.ResponseOutcome(result, this);
    }

    /// <summary>
    /// Remove Stock by Id.
    /// </summary>
    /// <param name="id">int.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    /// <response code="200">Stock deleted</response>
    /// <response code="400">Error deleting a pizza</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(Result), 200)]
    [ProducesResponseType(typeof(ErrorResult), 400)]
    public async Task<ActionResult> Delete(int id)
    {
        var result = await this.Mediator.Send(new DeleteStockCommand { Id = id });
        return ResponseHelper.ResponseOutcome(result, this);
    }
}
