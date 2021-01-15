namespace Pezza.BackEnd.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json;
    using Pezza.Common;
    using Pezza.Common.DTO;
    using Pezza.Common.Entities;
    using Pezza.Portal.Models;

    public class RestaurantController : BaseController
    {
        public async Task<ActionResult> Index()
        {
            var data = new StringContent("", Encoding.UTF8, "application/json");
            var response = await this.client.PostAsync(@$"{AppSettings.ApiUrl}Restaurant\Search", data);

            var responseData = await response.Content.ReadAsStringAsync();
            var entities = JsonConvert.DeserializeObject<List<Restaurant>>(responseData).ToList();
            for (var i = 0; i < entities.Count; i++)
            {
                entities[i].PictureUrl = $"{AppSettings.ApiUrl}Picture?file={entities[i].PictureUrl}&folder=restaurant";
            }
            return this.View(entities);
        }

        public async Task<ActionResult> Details(int id)
        {
            var entity = new Restaurant();
            var responseMessage = await this.client.GetAsync(@$"{AppSettings.ApiUrl}Restaurant\{id}");
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = await responseMessage.Content.ReadAsStringAsync();
                entity = JsonConvert.DeserializeObject<Restaurant>(responseData);
            }

            return this.View(entity);
        }

        public ActionResult Create() => this.View(new RestaurantModel());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(RestaurantModel restaurant)
        {
            if (this.ModelState.IsValid)
            {
                if (restaurant.Image?.Length > 0)
                {
                    using var ms = new MemoryStream();
                    restaurant.Image.CopyTo(ms);
                    var fileBytes = ms.ToArray();
                    restaurant.ImageData = $"data:{MimeTypeMap.GetMimeType(Path.GetExtension(restaurant.Image.FileName))};base64,{Convert.ToBase64String(fileBytes)}";
                }

                var json = JsonConvert.SerializeObject(restaurant);
                var data = new StringContent(json, Encoding.UTF8, "application/json");

                var responseMessage = await this.client.PostAsync(@$"{AppSettings.ApiUrl}Restaurant", data);
                if (responseMessage.IsSuccessStatusCode)
                {
                    var responseData = await responseMessage.Content.ReadAsStringAsync();
                    var response = JsonConvert.DeserializeObject<Restaurant>(responseData);
                }

                return this.RedirectToAction("Index");
            }
            else
            {
                return this.View(restaurant);
            }
        }

        [Route("Restaurant/Edit/{id?}")]
        public async Task<ActionResult> Edit(int id)
        {
            var entity = new Restaurant();
            var response = await this.client.GetAsync(@$"{AppSettings.ApiUrl}Restaurant\{id}");
            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();
                entity = JsonConvert.DeserializeObject<Restaurant>(responseData);

                return this.View(new RestaurantModel
                {
                    Id = id,
                    Name = entity.Name,
                    PictureUrl = $"{AppSettings.ApiUrl}Picture?file={entity.PictureUrl}&folder=restaurant",
                    Description = entity.Description,
                    Address = new AddressBase
                    {
                        Address = entity.Address,
                        City = entity.City,
                        Province = entity.Province,
                        ZipCode = entity.PostalCode
                    },
                    IsActive = entity.IsActive
                });
            }
            return this.View(new RestaurantModel());
        }

        [HttpPost]
        [Route("Restaurant/Edit/{id?}")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(RestaurantModel restaurant)
        {
            if (this.ModelState.IsValid)
            {
                if (restaurant.Image?.Length > 0)
                {
                    using var ms = new MemoryStream();
                    restaurant.Image.CopyTo(ms);
                    var fileBytes = ms.ToArray();
                    restaurant.ImageData = $"data:{MimeTypeMap.GetMimeType(Path.GetExtension(restaurant.Image.FileName))};base64,{Convert.ToBase64String(fileBytes)}";
                }

                var json = JsonConvert.SerializeObject(new RestaurantDataDTO
                {
                    Name = restaurant.Name,
                    Description = restaurant.Description,
                    Address = restaurant.Address,
                    IsActive = restaurant.IsActive,
                    ImageData = restaurant.ImageData
                });
                var data = new StringContent(json, Encoding.UTF8, "application/json");

                var responseMessage = await this.client.PutAsync(@$"{AppSettings.ApiUrl}Restaurant/{restaurant.Id}", data);
                if (responseMessage.IsSuccessStatusCode)
                {
                    var responseData = await responseMessage.Content.ReadAsStringAsync();
                    var response = JsonConvert.DeserializeObject<Restaurant>(responseData);
                }

                return this.RedirectToAction("Index");
            }
            else
            {
                return this.View(restaurant);
            }
        }

        [HttpPost]
        [Route("Restaurant/Delete/{id?}")]
        public async Task<JsonResult> Delete(int id)
        {
            if (id == 0)
            {
                return this.Json(false);
            }

            if (this.ModelState.IsValid)
            {
                var responseMessage = await this.client.DeleteAsync(@$"{AppSettings.ApiUrl}Restaurant\{id}");
                if (responseMessage.IsSuccessStatusCode)
                {
                    var responseData = await responseMessage.Content.ReadAsStringAsync();
                    var response = JsonConvert.DeserializeObject<bool>(responseData);

                    return this.Json(response);
                }

                return this.Json(false);
            }
            else
            {
                return this.Json(false);
            }
        }
    }
}
