namespace Pezza.Api.Controllers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Pezza.Api.Helpers;
    using Pezza.Common.DTO;
    using Pezza.Common.Entities;
    using Pezza.Core.Notify.Commands;
    using Pezza.Core.Notify.Queries;

    [ApiController]
    public class NotifyController : ApiController
    {
        /// <summary>
        /// Get Notify by Id.
        /// </summary>
        /// <param name="id">Primary Key.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> Get(int id)
        {
            var result = await this.Mediator.Send(new GetNotifyQuery { Id = id });

            return ResponseHelper.ResponseOutcome<NotifyDTO>(result, this);
        }

        /// <summary>
        /// Get all Notifies.
        /// </summary>
        /// <param name="searchModel">The search model.</param>
        /// <returns>
        /// A <see cref="Task" /> representing the asynchronous operation.
        /// </returns>
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [Route("Search")]
        public async Task<ActionResult> Search([FromBody] NotifyDTO searchModel)
        {
            var result = await this.Mediator.Send(new GetNotifiesQuery
            {
                SearchModel = searchModel ?? new NotifyDTO()
            });
            return ResponseHelper.ResponseOutcome<NotifyDTO>(result, this);
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
        /// <param name="notify">Notify Data DTO.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [Route("Notify")]
        public async Task<ActionResult<Notify>> Create(NotifyDTO notify)
        {
            var result = await this.Mediator.Send(new CreateNotifyCommand
            {
                Data = notify
            });

            return ResponseHelper.ResponseOutcome<NotifyDTO>(result, this);
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
        [HttpPut]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> Update(NotifyDTO notify)
        {
            var result = await this.Mediator.Send(new UpdateNotifyCommand
            {
                Data = notify
            });

            return ResponseHelper.ResponseOutcome<NotifyDTO>(result, this);
        }

        /// <summary>
        /// Remove Notify by Id.
        /// </summary>
        /// <param name="id">Primary Key.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await this.Mediator.Send(new DeleteNotifyCommand { Id = id });
            return ResponseHelper.ResponseOutcome(result, this);
        }
    }
}
