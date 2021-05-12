namespace Pezza.Api.Controllers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Pezza.Api.Helpers;
    using Pezza.Common.DTO;
    using Pezza.Common.Entities;
    using Pezza.Core.Stock.Commands;
    using Pezza.Core.Stock.Queries;

    [ApiController]
    [Authorize]
    public class StockController : ApiController
    {
        /// <summary>
        /// Get Stock by Id.
        /// </summary>
        /// <param name="id">id.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> Get(int id)
        {
            var result = await this.Mediator.Send(new GetStockQuery { Id = id });
            return ResponseHelper.ResponseOutcome<StockDTO>(result, this);
        }

        /// <summary>
        /// Get all Stock.
        /// </summary>
        /// <param name="searchModel">The search model.</param>
        /// <returns>
        /// A <see cref="Task" /> representing the asynchronous operation.
        /// </returns>
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [Route("Search")]
        public async Task<ActionResult> Search([FromBody] StockDTO searchModel)
        {
            var result = await this.Mediator.Send(new GetStocksQuery
            {
                SearchModel = searchModel ?? new StockDTO()
            });
            return ResponseHelper.ResponseOutcome<StockDTO>(result, this);
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
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<Stock>> Create(StockDTO data)
        {
            var result = await this.Mediator.Send(new CreateStockCommand
            {
                Data = data
            });

            return ResponseHelper.ResponseOutcome<StockDTO>(result, this);
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
        [HttpPut]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> Update(StockDTO data)
        {
            var result = await this.Mediator.Send(new UpdateStockCommand
            {
                Data = data
            });

            return ResponseHelper.ResponseOutcome<StockDTO>(result, this);
        }

        /// <summary>
        /// Remove Stock by Id.
        /// </summary>
        /// <param name="id">int.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await this.Mediator.Send(new DeleteStockCommand { Id = id });
            return ResponseHelper.ResponseOutcome(result, this);
        }
    }
}
