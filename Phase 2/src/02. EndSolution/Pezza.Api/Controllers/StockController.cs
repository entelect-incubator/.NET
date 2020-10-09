namespace Pezza.Api.Controllers
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Pezza.Common.DTO;
    using Pezza.Common.Entities;
    using Pezza.Core;

    [ApiController]
    [Route("api/[controller]")]
    public class StockController : ControllerBase
    {
        /// <summary>
        /// Get Stock by Id.
        /// </summary>
        /// <param name="id"></param> 
        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> Get(int id)
        {
            /*var search = await this.StockCore.GetAsync(id);
            if (search == null)
            {
                return this.NotFound();
            }*/

            return this.Ok();
        }

        /// <summary>
        /// Get all Stock.
        /// </summary>
        [HttpGet()]
        [ProducesResponseType(200)]
        public async Task<ActionResult> Search()
        {
            /*var result = await this.StockCore.GetAllAsync();*/

            return this.Ok();
        }

        /// <summary>
        /// Create Stock.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST api/Stock
        ///     {        
        ///       "name": "Tomatoes",
        ///       "UnitOfMeasure": "Kg",
        ///       "ValueOfMeasure": "1",
        ///       "Quantity": "50"
        ///     }
        /// </remarks>
        /// <param name="stock"></param> 
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<Stock>> Create([FromBody] StockDTO stock)
        {
            
            return this.Ok();
        }

        /// <summary>
        /// Update Stock.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     PUT api/Stock/1
        ///     {        
        ///       "Quantity": "30"
        ///     }
        /// </remarks>
        /// <param name="id"></param>
        /// <param name="stock"></param>
        [HttpPut("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> Update(int id, [FromBody] StockDTO stock)
        {
            

            return this.Ok();
        }

        /// <summary>
        /// Remove Stock by Id.
        /// </summary>
        /// <param name="id"></param> 
        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> Delete(int id)
        {
            
            return this.Ok();
        }
    }
}
