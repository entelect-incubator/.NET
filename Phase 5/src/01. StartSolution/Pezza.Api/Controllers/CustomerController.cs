namespace Pezza.Api.Controllers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Pezza.Api.Helpers;
    using Pezza.Common.DTO;
    using Pezza.Core.Customer.Commands;
    using Pezza.Core.Customer.Queries;

    [ApiController]
    public class CustomerController : ApiController
    {
        /// <summary>
        /// Get Customer by Id.
        /// </summary>
        /// <param name="id">int.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> GetCustomer(int id)
        {
            var result = await this.Mediator.Send(new GetCustomerQuery { Id = id });

            return ResponseHelper.ResponseOutcome<CustomerDTO>(result, this);
        }

        /// <summary>
        /// Get all Customers.
        /// </summary>
        /// <param name="searchModel">CustomerDataDTO.</param>
        /// <returns>A <see cref="Task"/> repres
        /// enting the asynchronous operation.</returns>
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [Route("Search")]
        public async Task<ActionResult> Search(CustomerDataDTO searchModel)
        {
            var result = await this.Mediator.Send(new GetCustomersQuery
            {
                SearchModel = searchModel
            });

            return ResponseHelper.ResponseOutcome<CustomerDTO>(result, this);
        }

        /// <summary>
        /// Create Customer.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///     POST api/Customer
        ///     {
        ///       "name": "Person A",
        ///       "address": "1 Tree Street",
        ///       "city": "Pretoria",
        ///       "province": "Gautenf",
        ///       "zipCode": "0181",
        ///       "phone": "0721230000",
        ///       "email": "person.a@gmail.com"
        ///       "contactPerson": "Person B 0723210000"
        ///     }.
        /// </remarks>
        /// <param name="customer">CustomerDataDTO.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<CustomerDTO>> Create(CustomerDataDTO customer)
        {
            var result = await this.Mediator.Send(new CreateCustomerCommand
            {
                Data = customer
            });

            return ResponseHelper.ResponseOutcome<CustomerDTO>(result, this);
        }

        /// <summary>
        /// Update Customer.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///     PUT api/Customer/1
        ///     {
        ///       "name": "Person A",
        ///       "address": "1 Tree Street",
        ///       "city": "Pretoria",
        ///       "province": "Gautenf",
        ///       "zipCode": "0181",
        ///       "phone": "0721230000",
        ///       "email": "person.a@gmail.com"
        ///       "contactPerson": "Person B 0723210000"
        ///     }.
        /// </remarks>
        /// <param name="id">in.</param>
        /// <param name="customer">CustomerDataDTO.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> Update(int id, CustomerDataDTO customer)
        {
            var result = await this.Mediator.Send(new UpdateCustomerCommand
            {
                Id = id,
                Data = customer
            });

            return ResponseHelper.ResponseOutcome<CustomerDTO>(result, this);
        }

        /// <summary>
        /// Remove Customer by Id.
        /// </summary>
        /// <param name="id">int.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await this.Mediator.Send(new DeleteCustomerCommand { Id = id });

            return ResponseHelper.ResponseOutcome(result, this);
        }
    }
}
