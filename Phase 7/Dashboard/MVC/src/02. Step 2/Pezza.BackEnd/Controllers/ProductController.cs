namespace Pezza.Portal.Controllers;

using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Pezza.Common;
using Pezza.Common.DTO;
using Pezza.Common.Models;
using Pezza.Portal.Helpers;
using Pezza.Portal.Models;

public class ProductController : BaseController
{
    private readonly ApiCallHelper<ProductDTO> apiCallHelper;

    public ProductController(IHttpClientFactory clientFactory)
        : base(clientFactory)
    {
        this.apiCallHelper = new ApiCallHelper<ProductDTO>(this.clientFactory);
        this.apiCallHelper.ControllerName = "Product";
    }

    public ActionResult Index()
    {
        return this.View(new PagingModel
        {
            Limit = 10,
            Page = 1
        });
    }

    public async Task<JsonResult> List(int limit, int page, string orderBy = "Name asc")
    {
        var json = JsonConvert.SerializeObject(new ProductDTO
        {
            OrderBy = orderBy,
            PagingArgs = new PagingArgs
            {
                Limit = limit,
                Offset = (page - 1) * limit,
                UsePaging = true
            }
        });
        var result = await this.apiCallHelper.GetListAsync(json);
        for (var i = 0; i < result.Data.Count; i++)
        {
            var picture = result.Data[i].PictureUrl;
            result.Data[i].PictureUrl = $"{AppSettings.ApiUrl}Picture?file={picture}&folder=Product";
        }
        return this.Json(result);
    }

    public async Task<ActionResult> Details(int id)
    {
        var entity = await this.apiCallHelper.GetAsync(id);
        return this.View(entity);
    }

    public ActionResult Create() => this.View(new ProductModel());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Create(ProductModel product)
    {
        if (!this.ModelState.IsValid)
        {
            return this.View(product);
        }

        if (string.IsNullOrEmpty(product.Description))
        {
            product.Description = string.Empty;
        }

        if (product.Image?.Length > 0)
        {
            using var ms = new MemoryStream();
            product.Image.CopyTo(ms);
            var fileBytes = ms.ToArray();
            product.ImageData = $"data:{MimeTypeMap.GetMimeType(Path.GetExtension(product.Image.FileName))};base64,{Convert.ToBase64String(fileBytes)}";
        }
        else
        {
            product.PictureUrl = null;
            ModelState.AddModelError("Image", "Please select a photo of the product");
        }

        var result = await this.apiCallHelper.Create(product);
        return Validate(result, this.apiCallHelper, product);
    }

    [Route("Product/Edit/{id?}")]
    public async Task<ActionResult> Edit(int id)
    {
        var entity = await this.apiCallHelper.GetAsync(id);
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

    [HttpPost]
    [Route("Product/Edit/{id?}")]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Edit(int id, ProductModel product)
    {
        if (!this.ModelState.IsValid)
        {
            return this.View(product);
        }

        if (product.Image?.Length > 0)
        {
            using var ms = new MemoryStream();
            product.Image.CopyTo(ms);
            var fileBytes = ms.ToArray();
            product.ImageData = $"data:{MimeTypeMap.GetMimeType(Path.GetExtension(product.Image.FileName))};base64,{Convert.ToBase64String(fileBytes)}";
        }
        else
        {
            product.PictureUrl = null;
            ModelState.AddModelError("Image", "Please select a photo of the product");
        }

        product.Id = id;
        var result = await this.apiCallHelper.Edit(product);
        return Validate(result, this.apiCallHelper, product);
    }

    [HttpPost]
    [Route("Product/Delete/{id?}")]
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
