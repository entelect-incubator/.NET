namespace Pezza.Api.Controllers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Pezza.Api.Controllers.CleanArchitecture.WebUI.Controllers;
    using Pezza.Api.Helpers;
    using Pezza.Common.Entities;
    using Pezza.Common.Models.SearchModels;
    using Pezza.Core.Order.Commands;
    using Pezza.Core.Order.Queries;

    [ApiController]
    [Route("[controller]")]
    public class OrderController : ApiController
    {
        [HttpGet("{id}")]
        public async Task<ActionResult> Get(int id)
        {
            var result = await this.Mediator.Send(new GetOrderQuery { Id = id });

            return ResponseHelper.ResponseOutcome<Order>(result, this);
        }

        [HttpGet()]
        public async Task<ActionResult> Search(OrderSearchModel searchModel)
        {
            var result = await this.Mediator.Send(new GetOrdersQuery { OrderSearchModel = searchModel });

            return ResponseHelper.ResponseOutcome<Order>(result, this);
        }

        [HttpPost]
        public async Task<ActionResult<Order>> Create(CreateOrderCommand command)
        {
            var result = await this.Mediator.Send(command);

            return ResponseHelper.ResponseOutcome<Order>(result, this);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await this.Mediator.Send(new DeleteOrderCommand { Id = id });

            return ResponseHelper.ResponseOutcome(result, this);
        }
    }
}
