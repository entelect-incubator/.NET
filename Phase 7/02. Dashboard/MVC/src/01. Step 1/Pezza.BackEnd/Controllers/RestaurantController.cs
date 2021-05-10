namespace Pezza.BackEnd.Controllers
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json;
    using Pezza.Common;
    using Pezza.Common.DTO;
    using Pezza.Common.Entities;
    using Pezza.Portal.Helpers;
    using Pezza.Portal.Models;

    public class RestaurantController : BaseController
    {
        private readonly ApiCallHelper<RestaurantDTO> apiCallHelper;

        public RestaurantController(IHttpClientFactory clientFactory)
            : base(clientFactory)
        {
            this.apiCallHelper = new ApiCallHelper<RestaurantDTO>(this.clientFactory);
            this.apiCallHelper.ControllerName = "Restaurant";
        }

        public async Task<ActionResult> Index()
        {
            var json = JsonConvert.SerializeObject(new RestaurantDTO
            {
                PagingArgs = Common.Models.PagingArgs.NoPaging
            });
            var entities = await this.apiCallHelper.GetListAsync(json);
            for (var i = 0; i < entities.Count; i++)
            {
                var picture = entities[i].PictureUrl;
                entities[i].PictureUrl = $"{AppSettings.ApiUrl}Picture?file={picture}&folder=restaurant";
            }
            return this.View(entities);
        }

        public async Task<ActionResult> Details(int id)
        {
            var entity = await this.apiCallHelper.GetAsync(id);
            return this.View(entity);
        }

        public ActionResult Create() => this.View(new RestaurantModel());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(RestaurantModel restaurant)
        {
            {
                if(string.IsNullOrEmpty(restaurant.Description))
                {
                    restaurant.Description = string.Empty;
                }
                restaurant.IsActive = true;
                restaurant.DateCreated = DateTime.Now;

                if (restaurant.Image?.Length > 0)
                {
                    using var ms = new MemoryStream();
                    restaurant.Image.CopyTo(ms);
                    var fileBytes = ms.ToArray();
                    restaurant.ImageData = $"data:{MimeTypeMap.GetMimeType(Path.GetExtension(restaurant.Image.FileName))};base64,{Convert.ToBase64String(fileBytes)}";
                }
                else
                {
                    ModelState.AddModelError("Image", "Please select a photo of the restaurant");
                }

                var result = await this.apiCallHelper.Create(restaurant);
                return Validate<RestaurantDTO>(result, this.apiCallHelper, restaurant);
            }
        }

        [Route("Restaurant/Edit/{id?}")]
        public async Task<ActionResult> Edit(int id)
        {
            var entity = await this.apiCallHelper.GetAsync(id);

            return this.View(new RestaurantModel
            {
                Id = id,
                Name = entity.Name,
                PictureUrl = $"{AppSettings.ApiUrl}Picture?file={entity.PictureUrl}&folder=restaurant",
                Description = entity.Description,
                Address = new AddressBase
                {
                    Address = entity.Address?.Address,
                    City = entity.Address?.City,
                    Province = entity.Address?.Province,
                    ZipCode = entity.Address?.ZipCode
                },
                IsActive = entity.IsActive
            });
        }

        [HttpPost]
        [Route("Restaurant/Edit/{id?}")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, RestaurantModel restaurant)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(restaurant);
            }

            if (restaurant.Image?.Length > 0)
            {
                using var ms = new MemoryStream();
                restaurant.Image.CopyTo(ms);
                var fileBytes = ms.ToArray();
                restaurant.ImageData = $"data:{MimeTypeMap.GetMimeType(Path.GetExtension(restaurant.Image.FileName))};base64,{Convert.ToBase64String(fileBytes)}";
            }
            else
            {
                restaurant.PictureUrl = null;
                ModelState.AddModelError("Image", "Please select a photo of the restaurant");
            }

            restaurant.Id = id;
            var result = await this.apiCallHelper.Edit(restaurant);
            return Validate<RestaurantDTO>(result, this.apiCallHelper, restaurant);
        }

        [HttpPost]
        [Route("Restaurant/Delete/{id?}")]
        public async Task<JsonResult> Delete(int id)
        {
            if (id == 0)
            {
                return this.Json(false);
            }

            if (!this.ModelState.IsValid)
            {
                return this.Json(false);
            }

            var result = await this.apiCallHelper.Delete(id);
            return this.Json(result);
        }
    }
}
