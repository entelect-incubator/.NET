namespace Pezza.Api.Controllers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Pezza.Api.Controllers.CleanArchitecture.WebUI.Controllers;
    using Pezza.Api.Helpers;
    using Pezza.Common.Entities;
    using Pezza.Common.Models.SearchModels;
    using Pezza.Core.Restaurant.Commands;
    using Pezza.Core.Restaurant.Queries;

    [ApiController]
    [Route("[controller]")]
    public class RestaurantController : ApiController
    {
        [HttpGet("{id}")]
        public async Task<ActionResult> Get(int id)
        {
            var result = await this.Mediator.Send(new GetRestaurantQuery { Id = id });

            return ResponseHelper.ResponseOutcome<Restaurant>(result, this);
        }

        [HttpGet()]
        public async Task<ActionResult> Search(RestaurantSearchModel searchModel)
        {
            var result = await this.Mediator.Send(new GetRestaurantsQuery { RestaurantSearchModel = searchModel });

            return ResponseHelper.ResponseOutcome<Restaurant>(result, this);
        }

        [HttpPost]
        public async Task<ActionResult<Restaurant>> Create(CreateRestaurantCommand command)
        {
            var imageResult = await MediaHelper.UploadMediaAsync("restaurant", command.ImageData);
            if (imageResult != null)
            {
                command.Restaurant.PictureUrl = imageResult.Data.RelativePath;
            }

            var result = await this.Mediator.Send(command);

            return ResponseHelper.ResponseOutcome<Restaurant>(result, this);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, UpdateRestaurantCommand command)
        {
            if (id != command.Id)
            {
                return this.ValidationProblem();
            }

            var imageResult = await MediaHelper.UploadMediaAsync("restaurant", command.ImageData);
            if (imageResult != null)
            {
                command.PictureUrl = imageResult.Data.RelativePath;
            }

            var result = await this.Mediator.Send(command);

            return ResponseHelper.ResponseOutcome<Restaurant>(result, this);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await this.Mediator.Send(new DeleteRestaurantCommand { Id = id });

            return ResponseHelper.ResponseOutcome(result, this);
        }
    }
}
