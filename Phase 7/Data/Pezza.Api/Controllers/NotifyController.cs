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
        /// <param name="id"></param> 
        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> Get(int id)
        {
            var result = await this.Mediator.Send(new GetNotifyQuery { Id = id });

            return ResponseHelper.ResponseOutcome<Notify>(result, this);
        }

        /// <summary>
        /// Get all Notifys.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(ListResult<NotifyDTO>), 200)]
        [ProducesResponseType(typeof(ErrorResult), 400)]
        [Route("Search")]
        public async Task<ActionResult> Search(NotifyDTO dto)
        {
            var result = await this.Mediator.Send(new GetNotifiesQuery
            {
                dto = dto
            });
            return ResponseHelper.ResponseOutcome(result, this);
        }

        /// <summary>
        /// Create Notify.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
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
        ///     }
        /// </remarks>
        /// <param name="notify"></param> 
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [Route("Notify")]
        public async Task<ActionResult<Notify>> Create(NotifyDataDTO notify)
        {
            var result = await this.Mediator.Send(new CreateNotifyCommand
            {
                Data = notify
            });

            return ResponseHelper.ResponseOutcome<Notify>(result, this);
        }

        /// <summary>
        /// Update Notify.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     PUT api/Notify/1
        ///     {        
        ///       "customerId": "1",
        ///       "email": "person.a@gmail.com"
        ///     }
        /// </remarks>
        /// <param name="id"></param>
        /// <param name="notify"></param>
        [HttpPut("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> Update(int id, NotifyDataDTO notify)
        {
            var result = await this.Mediator.Send(new UpdateNotifyCommand
            {
                Id = id,
                Data = notify
            });

            return ResponseHelper.ResponseOutcome<Notify>(result, this);
        }

        /// <summary>
        /// Remove Notify by Id.
        /// </summary>
        /// <param name="id"></param> 
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
