namespace Pezza.Api.Controllers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Pezza.Api.Helpers;
    using Pezza.Common.DTO;
    using Pezza.Common.Entities;
    using Pezza.Core.Restaurant.Commands;
    using Pezza.Core.Restaurant.Queries;

    [ApiController]
    public class RestaurantController : ApiController
    {
        /// <summary>
        /// Get Restaurant by Id.
        /// </summary>
        /// <param name="id">int.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> Get(int id)
        {
            var result = await this.Mediator.Send(new GetRestaurantQuery { Id = id });
            return ResponseHelper.ResponseOutcome<RestaurantDTO>(result, this);
        }

        /// <summary>
        /// Get all Restaurants.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [Route("Search")]
        public async Task<ActionResult> Search(RestaurantDTO searchModel)
        {
            var result = await this.Mediator.Send(new GetRestaurantsQuery
            {
                SearchModel = searchModel ?? new RestaurantDTO()
            });
            return ResponseHelper.ResponseOutcome<RestaurantDTO>(result, this);
        }

        /// <summary>
        /// Create Stock.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST api/Restaurant
        ///     {
        ///       "name": "Restaurant 1",
        ///       "description": "",
        ///       "pictureUrl": "base64",
        ///       "isActive": true
        ///       "Address": {
        ///         "city": "Pretoria",
        ///         "province": "Gauteng",
        ///         "ZipCode": "0000"
        ///       }
        ///     }.
        /// </remarks>
        /// <param name="data">RestaurantDTO.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<Restaurant>> Create(RestaurantDTO data)
        {
            if (!string.IsNullOrEmpty(data.ImageData))
            {
                var imageResult = await MediaHelper.UploadMediaAsync("restaurant", data.ImageData);
                if (imageResult != null)
                {
                    data.PictureUrl = imageResult.Data.Path;
                }
            }

            var result = await this.Mediator.Send(new CreateRestaurantCommand
            {
                Data = data
            });

            return ResponseHelper.ResponseOutcome<RestaurantDTO>(result, this);
        }

        /// <summary>
        /// Update Restaurant.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT api/Restaurant
        ///     {
        ///       "id": 1,
        ///       "name": "New Restaurant"
        ///     }.
        /// </remarks>
        /// <param name="data">RestaurantDTO.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [HttpPut]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> Update(RestaurantDTO data)
        {
            if (!string.IsNullOrEmpty(data.ImageData))
            {
                var imageResult = await MediaHelper.UploadMediaAsync("restaurant", data.ImageData);
                if (imageResult != null)
                {
                    data.PictureUrl = imageResult.Data.Path;
                }
            }

            var result = await this.Mediator.Send(new UpdateRestaurantCommand
            {
                Data = data
            });

            return ResponseHelper.ResponseOutcome<RestaurantDTO>(result, this);
        }

        /// <summary>
        /// Remove Restaurant by Id.
        /// </summary>
        /// <param name="id">int.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await this.Mediator.Send(new DeleteRestaurantCommand { Id = id });
            return ResponseHelper.ResponseOutcome(result, this);
        }
    }
}
