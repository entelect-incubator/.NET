namespace Pezza.Api.Controllers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Pezza.Api.Helpers;
    using Pezza.Common.DTO;
    using Pezza.Common.Entities;
    using Pezza.Common.Models;
    using Pezza.Core.Order.Commands;
    using Pezza.Core.Order.Queries;

    [ApiController]
    [Authorize]
    public class OrderController : ApiController
    {
        /// <summary>
        /// Get Order by Id.
        /// </summary>
        /// <param name="id">int.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <response code="200">Get a product</response>
        /// <response code="400">Error getting a product</response>
        /// <response code="404">Product not found</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Result<OrderDTO>), 200)]
        [ProducesResponseType(typeof(ErrorResult), 400)]
        [ProducesResponseType(typeof(ErrorResult), 404)]
        public async Task<ActionResult> Get(int id)
        {
            var result = await this.Mediator.Send(new GetOrderQuery { Id = id });
            return ResponseHelper.ResponseOutcome(result, this);
        }

        /// <summary>
        /// Get all Orders.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <response code="200">Order Search</response>
        /// <response code="400">Error searching for orders</response>
        [HttpPost]
        [ProducesResponseType(typeof(ListResult<OrderDTO>), 200)]
        [ProducesResponseType(typeof(Result), 400)]
        [Route("Search")]
        public async Task<ActionResult> Search(OrderDTO dto)
        {
            var result = await this.Mediator.Send(new GetOrdersQuery
            {
                dto = dto
            });
            return ResponseHelper.ResponseOutcome(result, this);
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
        /// <response code="200">Order created</response>
        /// <response code="400">Error creating a order</response>
        [HttpPost]
        [ProducesResponseType(typeof(Result<OrderDTO>), 200)]
        [ProducesResponseType(typeof(Result), 400)]
        public async Task<ActionResult<Order>> Create(OrderDTO data)
        {
            data.Customer = null;
            var result = await this.Mediator.Send(new CreateOrderCommand
            {
                Data = data
            });

            return ResponseHelper.ResponseOutcome(result, this);
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
        /// <response code="200">Order updated</response>
        /// <response code="400">Error updating a order</response>
        /// <response code="404">Order not found</response>
        [HttpPut]
        [ProducesResponseType(typeof(Result<OrderDTO>), 200)]
        [ProducesResponseType(typeof(Result), 400)]
        [ProducesResponseType(typeof(Result), 404)]
        public async Task<ActionResult> Update(OrderDTO data)
        {
            var result = await this.Mediator.Send(new UpdateOrderCommand
            {
                Data = data
            });

            return ResponseHelper.ResponseOutcome(result, this);
        }

        /// <summary>
        /// Remove Order by Id.
        /// </summary>
        /// <param name="id">int.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <response code="200">Order deleted</response>
        /// <response code="400">Error deleting a order</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(Result), 200)]
        [ProducesResponseType(typeof(Result), 400)]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await this.Mediator.Send(new DeleteOrderCommand { Id = id });
            return ResponseHelper.ResponseOutcome(result, this);
        }
    }
}
