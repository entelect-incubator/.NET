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

            return ResponseHelper.ResponseOutcome<Product>(result, this);
        }

        /// <summary>
        /// Get all Products.
        /// </summary>
        /// <param name="searchModel">ProductDataDTO.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [Route("Search")]
        public async Task<ActionResult> Search(ProductDataDTO searchModel)
        {
            var result = await this.Mediator.Send(new GetProductsQuery
            {
                SearchModel = searchModel
            });

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
        /// <param name="data">ProductDataDTO.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<Product>> Create(ProductDataDTO data)
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

            return ResponseHelper.ResponseOutcome<Product>(result, this);
        }

        /// <summary>
        /// Update Product.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///     PUT api/Product/1
        ///     {
        ///       "special": true
        ///     }.
        /// </remarks>
        /// <param name="id">int.</param>
        /// <param name="data">ProductDataDTO.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> Update(int id, ProductDataDTO data)
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
                Id = id,
                Data = data
            });

            return ResponseHelper.ResponseOutcome<Product>(result, this);
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
