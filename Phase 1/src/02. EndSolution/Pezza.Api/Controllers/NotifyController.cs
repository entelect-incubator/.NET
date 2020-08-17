namespace Pezza.Api.Controllers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Pezza.Api.Controllers.CleanArchitecture.WebUI.Controllers;
    using Pezza.Api.Helpers;
    using Pezza.Common.Entities;
    using Pezza.Common.Models.SearchModels;
    using Pezza.Core.Notify.Commands;
    using Pezza.Core.Notify.Queries;

    [ApiController]
    [Route("[controller]")]
    public class NotifyController : ApiController
    {
        [HttpGet("{id}")]
        public async Task<ActionResult> Get(int id)
        {
            var result = await this.Mediator.Send(new GetNotifyQuery { Id = id });

            return ResponseHelper.ResponseOutcome<Notify>(result, this);
        }

        [HttpGet()]
        public async Task<ActionResult> Search(NotifySearchModel searchModel)
        {
            var result = await this.Mediator.Send(new GetNotifiesQuery { NotifySearchModel = searchModel });

            return ResponseHelper.ResponseOutcome<Notify>(result, this);
        }

        [HttpPost]
        public async Task<ActionResult<Notify>> Create(CreateNotifyCommand command)
        {
            var result = await this.Mediator.Send(command);

            return ResponseHelper.ResponseOutcome<Notify>(result, this);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, UpdateNotifyCommand command)
        {
            if (id != command.Id)
            {
                return this.ValidationProblem();
            }

            var result = await this.Mediator.Send(command);

            return ResponseHelper.ResponseOutcome<Notify>(result, this);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await this.Mediator.Send(new DeleteNotifyCommand { Id = id });

            return ResponseHelper.ResponseOutcome(result, this);
        }
    }
}
