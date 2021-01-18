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

    public class ProductController : BaseController
    {
        public async Task<ActionResult> Index()
        {
            var data = new StringContent("", Encoding.UTF8, "application/json");
            var response = await this.client.PostAsync(@$"{AppSettings.ApiUrl}Product\Search", data);

            var responseData = await response.Content.ReadAsStringAsync();
            var entities = JsonConvert.DeserializeObject<List<ProductDTO>>(responseData).ToList();
            for (var i = 0; i < entities.Count; i++)
            {
                entities[i].PictureUrl = $"{AppSettings.ApiUrl}Picture?file={entities[i].PictureUrl}&folder=Product";
            }
            return this.View(entities);
        }

        public async Task<ActionResult> Details(int id)
        {
            var entity = new ProductDTO();
            var responseMessage = await this.client.GetAsync(@$"{AppSettings.ApiUrl}Product\{id}");
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = await responseMessage.Content.ReadAsStringAsync();
                entity = JsonConvert.DeserializeObject<ProductDTO>(responseData);
            }

            return this.View(entity);
        }

        public ActionResult Create() => this.View(new ProductModel());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ProductModel product)
        {
            if (this.ModelState.IsValid)
            {
                if (product.Image?.Length > 0)
                {
                    using var ms = new MemoryStream();
                    product.Image.CopyTo(ms);
                    var fileBytes = ms.ToArray();
                    product.ImageData = $"data:{MimeTypeMap.GetMimeType(Path.GetExtension(product.Image.FileName))};base64,{Convert.ToBase64String(fileBytes)}";
                }

                var json = JsonConvert.SerializeObject(product);
                var data = new StringContent(json, Encoding.UTF8, "application/json");

                var responseMessage = await this.client.PostAsync(@$"{AppSettings.ApiUrl}Product", data);
                if (responseMessage.IsSuccessStatusCode)
                {
                    var responseData = await responseMessage.Content.ReadAsStringAsync();
                    var response = JsonConvert.DeserializeObject<ProductDTO>(responseData);
                }

                return this.RedirectToAction("Index");
            }
            else
            {
                return this.View(product);
            }
        }

        [Route("Product/Edit/{id?}")]
        public async Task<ActionResult> Edit(int id)
        {
            var entity = new ProductDTO();
            var response = await this.client.GetAsync(@$"{AppSettings.ApiUrl}Product\{id}");
            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();
                entity = JsonConvert.DeserializeObject<ProductDTO>(responseData);

                return this.View(new ProductModel
                {
                    Id = id,
                    Name = entity.Name,
                    Description = entity.Description,
                    PictureUrl = $"{AppSettings.ApiUrl}Picture?file={entity.PictureUrl}&folder=Product",
                    Price = entity.Price,
                    Special = entity.Special,
                    OfferEndDate = entity.OfferEndDate,
                    OfferPrice = entity.OfferPrice,
                    IsActive = entity.IsActive
                });
            }
            return this.View(new ProductModel());
        }

        [HttpPost]
        [Route("Product/Edit/{id?}")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(ProductModel Product)
        {
            if (this.ModelState.IsValid)
            {
                if (Product.Image?.Length > 0)
                {
                    using var ms = new MemoryStream();
                    Product.Image.CopyTo(ms);
                    var fileBytes = ms.ToArray();
                    Product.ImageData = $"data:{MimeTypeMap.GetMimeType(Path.GetExtension(Product.Image.FileName))};base64,{Convert.ToBase64String(fileBytes)}";
                }

                var json = JsonConvert.SerializeObject(new ProductDTO
                {
                    Name = Product.Name,
                    Description = Product.Description,
                    Price = Product.Price,
                    Special = Product.Special,
                    OfferEndDate = Product.OfferEndDate,
                    OfferPrice = Product.OfferPrice,
                    IsActive = Product.IsActive,
                    ImageData = Product.ImageData
                });
                var data = new StringContent(json, Encoding.UTF8, "application/json");

                var responseMessage = await this.client.PutAsync(@$"{AppSettings.ApiUrl}Product/{Product.Id}", data);
                if (responseMessage.IsSuccessStatusCode)
                {
                    var responseData = await responseMessage.Content.ReadAsStringAsync();
                    var response = JsonConvert.DeserializeObject<ProductDTO>(responseData);
                }

                return this.RedirectToAction("Index");
            }
            else
            {
                return this.View(Product);
            }
        }

        [HttpPost]
        [Route("Product/Delete/{id?}")]
        public async Task<JsonResult> Delete(int id)
        {
            if (id == 0)
            {
                return this.Json(false);
            }

            if (this.ModelState.IsValid)
            {
                var responseMessage = await this.client.DeleteAsync(@$"{AppSettings.ApiUrl}Product\{id}");
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
