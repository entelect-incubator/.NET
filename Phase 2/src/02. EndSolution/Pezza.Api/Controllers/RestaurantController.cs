namespace Pezza.Api.Controllers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Pezza.Api.Controllers.CleanArchitecture.WebUI.Controllers;
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
        /// <param name="id"></param> 
        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> Get(int id)
        {
            var result = await this.Mediator.Send(new GetRestaurantQuery { Id = id });

            return ResponseHelper.ResponseOutcome<Restaurant>(result, this);
        }

        /// <summary>
        /// Get all Restaurants.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [Route("Search")]
        public async Task<ActionResult> Search()
        {
            var result = await this.Mediator.Send(new GetRestaurantsQuery());

            return ResponseHelper.ResponseOutcome<Restaurant>(result, this);
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
        ///     }
        /// </remarks>
        /// <param name="data"></param> 
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<Restaurant>> Create(RestaurantDataDTO data)
        {
            var imageResult = await MediaHelper.UploadMediaAsync("restaurant", data.ImageData);
            if (imageResult != null)
            {
                data.PictureUrl = imageResult.Data.RelativePath;
            }

            var result = await this.Mediator.Send(new CreateRestaurantCommand
            {
                Data = data
            });

            return ResponseHelper.ResponseOutcome<Restaurant>(result, this);
        }

        /// <summary>
        /// Update Restaurant.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     PUT api/Restaurant/1
        ///     {        
        ///       "name": "New Restaurant"
        ///     }
        /// </remarks>
        /// <param name="id"></param>
        /// <param name="data"></param>
        [HttpPut("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> Update(int id, RestaurantDataDTO data)
        {
            var imageResult = await MediaHelper.UploadMediaAsync("restaurant", data.ImageData);
            if (imageResult != null)
            {
                data.PictureUrl = imageResult.Data.RelativePath;
            }

            var result = await this.Mediator.Send(new UpdateRestaurantCommand
            {
                Id = id,
                Data = data
            });

            return ResponseHelper.ResponseOutcome<Restaurant>(result, this);
        }

        /// <summary>
        /// Remove Restaurant by Id.
        /// </summary>
        /// <param name="id"></param> 
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
