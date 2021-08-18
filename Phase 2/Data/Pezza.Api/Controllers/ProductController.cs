namespace Pezza.Api.Controllers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Pezza.Api.Helpers;
    using Pezza.Common.DTO;
    using Pezza.Common.Entities;
    using Pezza.Common.Models;
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
        /// <response code="200">Get a product</response>
        /// <response code="400">Error getting a product</response>
        /// <response code="404">Product not found</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Result<ProductDTO>), 200)]
        [ProducesResponseType(typeof(ErrorResult), 400)]
        [ProducesResponseType(typeof(ErrorResult), 404)]
        public async Task<ActionResult> Get(int id)
        {
            var result = await this.Mediator.Send(new GetProductQuery { Id = id });
            return ResponseHelper.ResponseOutcome(result, this);
        }

        /// <summary>
        /// Get all Products.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <response code="200">Product Search</response>
        /// <response code="400">Error searching for products</response>
        [HttpPost]
        [ProducesResponseType(typeof(ListResult<ProductDTO>), 200)]
        [ProducesResponseType(typeof(Result), 400)]
        [Route("Search")]
        public async Task<ActionResult> Search()
        {
            var result = await this.Mediator.Send(new GetProductsQuery());
            return ResponseHelper.ResponseOutcome(result, this);
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
        /// <response code="200">Product created</response>
        /// <response code="400">Error creating a product</response>
        [HttpPost]
        [ProducesResponseType(typeof(Result<ProductDTO>), 200)]
        [ProducesResponseType(typeof(Result), 400)]
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

            return ResponseHelper.ResponseOutcome(result, this);
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
        /// <response code="200">Product updated</response>
        /// <response code="400">Error updating a product</response>
        /// <response code="404">Product not found</response>
        [HttpPut]
        [ProducesResponseType(typeof(Result<ProductDTO>), 200)]
        [ProducesResponseType(typeof(Result), 400)]
        [ProducesResponseType(typeof(Result), 404)]
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

            return ResponseHelper.ResponseOutcome(result, this);
        }

        /// <summary>
        /// Remove Product by Id.
        /// </summary>
        /// <param name="id">int.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <response code="200">Product deleted</response>
        /// <response code="400">Error deleting a product</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(Result), 200)]
        [ProducesResponseType(typeof(Result), 400)]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await this.Mediator.Send(new DeleteProductCommand { Id = id });
            return ResponseHelper.ResponseOutcome(result, this);
        }
    }
}
