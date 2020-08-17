namespace Pezza.Api.Controllers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Pezza.Api.Controllers.CleanArchitecture.WebUI.Controllers;
    using Pezza.Api.Helpers;
    using Pezza.Common.Entities;
    using Pezza.Common.Models.SearchModels;
    using Pezza.Core.Stock.Commands;
    using Pezza.Core.Stock.Queries;

    [ApiController]
    [Route("[controller]")]
    public class StockController : ApiController
    {
        [HttpGet("{id}")]
        public async Task<ActionResult> Get(int id)
        {
            var result = await this.Mediator.Send(new GetStockQuery { Id = id });

            return ResponseHelper.ResponseOutcome<Stock>(result, this);
        }

        [HttpGet()]
        public async Task<ActionResult> Search(StockSearchModel searchModel)
        {
            var result = await this.Mediator.Send(new GetStocksQuery { StockSearchModel = searchModel });

            return ResponseHelper.ResponseOutcome<Stock>(result, this);
        }

        [HttpPost]
        public async Task<ActionResult<Stock>> Create(CreateStockCommand command)
        {
            var result = await this.Mediator.Send(command);

            return ResponseHelper.ResponseOutcome<Stock>(result, this);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, UpdateStockCommand command)
        {
            if (id != command.Id)
            {
                return this.ValidationProblem();
            }

            var result = await this.Mediator.Send(command);

            return ResponseHelper.ResponseOutcome<Stock>(result, this);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await this.Mediator.Send(new DeleteStockCommand { Id = id });

            return ResponseHelper.ResponseOutcome(result, this);
        }
    }
}
