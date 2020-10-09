namespace Pezza.Api.Controllers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Pezza.Api.Controllers.CleanArchitecture.WebUI.Controllers;
    using Pezza.Api.Helpers;
    using Pezza.Common.Entities;
    using Pezza.Common.Models.SearchModels;
    using Pezza.Core.Product.Commands;
    using Pezza.Core.Product.Queries;

    [ApiController]
    [Route("[controller]")]
    public class ProductController : ApiController
    {
        [HttpGet("{id}")]
        public async Task<ActionResult> Get(int id)
        {
            var result = await this.Mediator.Send(new GetProductQuery { Id = id });

            return ResponseHelper.ResponseOutcome<Product>(result, this);
        }

        [HttpGet()]
        public async Task<ActionResult> Search(ProductSearchModel searchModel)
        {
            var result = await this.Mediator.Send(new GetProductsQuery { ProductSearchModel = searchModel });

            return ResponseHelper.ResponseOutcome<Product>(result, this);
        }

        [HttpPost]
        public async Task<ActionResult<Product>> Create(CreateProductCommand command)
        {            
            var imageResult = await MediaHelper.UploadMediaAsync("product", command.ImageData);
            if(imageResult != null)
            {
                command.Product.PictureUrl = imageResult.Data.RelativePath;
            }

            var result = await this.Mediator.Send(command);

            return ResponseHelper.ResponseOutcome<Product>(result, this);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, UpdateProductCommand command)
        {
            if (id != command.Id)
            {
                return this.ValidationProblem();
            }

            var imageResult = await MediaHelper.UploadMediaAsync("product", command.ImageData);
            if (imageResult != null)
            {
                command.PictureUrl = imageResult.Data.RelativePath;
            }

            var result = await this.Mediator.Send(command);

            return ResponseHelper.ResponseOutcome<Product>(result, this);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await this.Mediator.Send(new DeleteProductCommand { Id = id });

            return ResponseHelper.ResponseOutcome(result, this);
        }
    }
}
