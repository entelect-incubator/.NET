namespace Pezza.Api.Controllers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Pezza.Api.Controllers.CleanArchitecture.WebUI.Controllers;
    using Pezza.Api.Helpers;
    using Pezza.Common.Entities;
    using Pezza.Common.Models.SearchModels;
    using Pezza.Core.Customer.Commands;
    using Pezza.Core.Customer.Queries;

    [ApiController]
    [Route("[controller]")]
    public class CustomerController : ApiController
    {
        [HttpGet("{id}")]
        public async Task<ActionResult> Get(int id)
        {
            var result = await this.Mediator.Send(new GetCustomerQuery { Id = id });

            return ResponseHelper.ResponseOutcome<Customer>(result, this);
        }

        [HttpGet()]
        public async Task<ActionResult> Search(CustomerSearchModel searchModel)
        {
            var result = await this.Mediator.Send(new GetCustomersQuery { CustomerSearchModel = searchModel });

            return ResponseHelper.ResponseOutcome<Customer>(result, this);
        }

        [HttpPost]
        public async Task<ActionResult<Customer>> Create(CreateCustomerCommand command)
        {
            var result = await this.Mediator.Send(command);

            return ResponseHelper.ResponseOutcome<Customer>(result, this);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, UpdateCustomerCommand command)
        {
            if (id != command.Id)
            {
                return this.ValidationProblem();
            }

            var result = await this.Mediator.Send(command);

            return ResponseHelper.ResponseOutcome<Customer>(result, this);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await this.Mediator.Send(new DeleteCustomerCommand { Id = id });

            return ResponseHelper.ResponseOutcome(result, this);
        }
    }
}
