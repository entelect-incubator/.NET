namespace Pezza.Api.Controllers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Pezza.Api.Helpers;
    using Pezza.Common.DTO;
    using Pezza.Common.Entities;
    using Pezza.Core.Product.Commands;
    using Pezza.Core.Product.Queries;

    [ApiController]
    public class ProductController : ApiController
    {
        /// <summary>
        /// Get Product by Id.
        /// </summary>
        /// <param name="id">int.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> Get(int id)
        {
            var result = await this.Mediator.Send(new GetProductQuery { Id = id });
            return ResponseHelper.ResponseOutcome<ProductDTO>(result, this);
        }

        /// <summary>
        /// Get all Products.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [Route("Search")]
        public async Task<ActionResult> Search()
        {
            var result = await this.Mediator.Send(new GetProductsQuery());
            return ResponseHelper.ResponseOutcome<ProductDTO>(result, this);
        }

        /// <summary>
        /// Create Product.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///     POST api/Product
        ///     {
        ///       "name": "Tomatoes",
        ///       "description": "",
        ///       "pictureUrl": "Base64",
        ///       "price": "50.00",
        ///       "special": false,
        ///       "isActive": true
        ///     }.
        /// </remarks>
        /// <param name="data">ProductDTO.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<Product>> Create(ProductDTO data)
        {
            if (!string.IsNullOrEmpty(data.ImageData))
            {
                var imageResult = await MediaHelper.UploadMediaAsync("product", data.ImageData);
                if (imageResult != null)
                {
                    data.PictureUrl = imageResult.Data.Path;
                }
            }

            var result = await this.Mediator.Send(new CreateProductCommand
            {
                Data = data
            });

            return ResponseHelper.ResponseOutcome<ProductDTO>(result, this);
        }

        /// <summary>
        /// Update Product.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///     PUT api/Product
        ///     {
        ///       "id": 1,
        ///       "special": true
        ///     }.
        /// </remarks>
        /// <param name="data">ProductDTO.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [HttpPut]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> Update(ProductDTO data)
        {
            if (!string.IsNullOrEmpty(data.ImageData))
            {
                var imageResult = await MediaHelper.UploadMediaAsync("product", data.ImageData);
                if (imageResult != null)
                {
                    data.PictureUrl = imageResult.Data.Path;
                }
            }

            var result = await this.Mediator.Send(new UpdateProductCommand
            {
                Data = data
            });

            return ResponseHelper.ResponseOutcome<ProductDTO>(result, this);
        }

        /// <summary>
        /// Remove Product by Id.
        /// </summary>
        /// <param name="id">int.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await this.Mediator.Send(new DeleteProductCommand { Id = id });
            return ResponseHelper.ResponseOutcome(result, this);
        }
    }
}
