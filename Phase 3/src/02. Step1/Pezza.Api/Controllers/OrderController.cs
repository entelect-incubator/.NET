namespace Pezza.Api.Controllers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Pezza.Api.Helpers;
    using Pezza.Common.DTO;
    using Pezza.Common.Entities;
    using Pezza.Core.Order.Commands;
    using Pezza.Core.Order.Queries;

    [ApiController]
    public class OrderController : ApiController
    {
        /// <summary>
        /// Get Order by Id.
        /// </summary>
        /// <param name="id">int.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> Get(int id)
        {
            var result = await this.Mediator.Send(new GetOrderQuery { Id = id });
            return ResponseHelper.ResponseOutcome<OrderDTO>(result, this);
        }

        /// <summary>
        /// Get all Orders.
        /// </summary>
        /// <param name="searchModel">The search model.</param>
        /// <returns>
        /// A <see cref="Task" /> representing the asynchronous operation.
        /// </returns>
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [Route("Search")]
        public async Task<ActionResult> Search([FromBody] OrderDTO searchModel)
        {
            var result = await this.Mediator.Send(new GetOrdersQuery());
            return ResponseHelper.ResponseOutcome<OrderDTO>(result, this);
        }

        /// <summary>
        /// Create Order.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///     POST api/Order
        ///     {
        ///       "customerId": "1",
        ///       "restaurantId": "1",
        ///       "amount": "1.00"
        ///     }.
        /// </remarks>
        /// <param name="data">OrderDTO.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<Order>> Create(OrderDTO data)
        {
            data.Customer = null;
            var result = await this.Mediator.Send(new CreateOrderCommand
            {
                Data = data
            });

            return ResponseHelper.ResponseOutcome<OrderDTO>(result, this);
        }

        /// <summary>
        /// Update Order.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///     PUT api/Order
        ///     {
        ///       "id": 1,,
        ///       "completed": true
        ///     }.
        /// </remarks>
        /// <param name="data">OrderDTO.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [HttpPut]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> Update(OrderDTO data)
        {
            var result = await this.Mediator.Send(new UpdateOrderCommand
            {
                Data = data
            });

            return ResponseHelper.ResponseOutcome<OrderDTO>(result, this);
        }

        /// <summary>
        /// Remove Order by Id.
        /// </summary>
        /// <param name="id">int.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await this.Mediator.Send(new DeleteOrderCommand { Id = id });
            return ResponseHelper.ResponseOutcome(result, this);
        }
    }
}
