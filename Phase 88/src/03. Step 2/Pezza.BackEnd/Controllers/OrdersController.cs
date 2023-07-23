using BackEnd.Controllers;

namespace Portal.Controllers;

using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Common;
using Common.DTO;
using Portal.Helpers;
using Portal.Models;

public class OrdersController : BaseController
{
    private readonly ApiCallHelper<OrderDTO> apiCallHelper;

    public OrdersController(IHttpClientFactory clientFactory)
        : base(clientFactory)
    {
        this.apiCallHelper = new ApiCallHelper<OrderDTO>(this.clientFactory)
        {
            ControllerName = "Order"
        };
    }

    public ActionResult Index() => this.View(new PagingModel
    {
        Limit = 10,
        Page = 1
    });

    [HttpPost]
    public async Task<JsonResult> List([FromBody] SearchModel<OrderDTO> searchmodel)
    {
        var entity = searchmodel.SearchData;
        entity.OrderBy = searchmodel.OrderBy;
        entity.PagingArgs = new Common.Models.PagingArgs
        {
            Limit = searchmodel.Limit,
            Offset = (searchmodel.Page - 1) * searchmodel.Limit,
            UsePaging = true
        };
        var result = await this.apiCallHelper.GetListAsync(entity);
        return this.Json(result);
    }

    public async Task<IActionResult> OrderItem() => this.PartialView("~/views/Orders/_Products.cshtml", new OrderItemModel
    {
        Products = await this.GetProducts()
    });

    public async Task<ActionResult> Details(int id)
    {
        var entity = await this.apiCallHelper.GetAsync(id);
        return this.View(entity);
    }

    public async Task<ActionResult> Create() => this.View(new OrderModel
    {
        Customers = await this.GetCustomers(),
        Restaurants = await this.GetRestaurants()
    });

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Create(OrderDTO order)
    {
        if (!this.ModelState.IsValid)
        {
            return this.View(order);
        }

        var result = await this.apiCallHelper.Create(order);
        return this.Validate(result, this.apiCallHelper, order);
    }

    private async Task<List<SelectListItem>> GetCustomers()
    {
        var dto = new CustomerDTO
        {
            PagingArgs = Common.Models.PagingArgs.NoPaging
        };
        var apiHelper = new ApiCallHelper<CustomerDTO>(this.clientFactory)
        {
            ControllerName = "Customer"
        };
        var entities = await apiHelper.GetListAsync(dto);
        return entities.Data?.Select(x =>
        {
            return new SelectListItem
            {
                Value = $"{x.Id}",
                Text = $"{x.Name} {x.Phone}"
            };
        }).ToList();
    }

    private async Task<List<SelectListItem>> GetRestaurants()
    {
        var dto = new RestaurantDTO
        {
            PagingArgs = Common.Models.PagingArgs.NoPaging
        };
        var apiHelper = new ApiCallHelper<RestaurantDTO>(this.clientFactory)
        {
            ControllerName = "Restaurant"
        };
        var entities = await apiHelper.GetListAsync(dto);
        for (var i = 0; i < entities.Data.Count; i++)
        {
            entities.Data[i].PictureUrl = $"{AppSettings.ApiUrl}Picture?file={entities.Data[i].PictureUrl}&folder=restaurant";
        }

        return entities.Data.Select(x =>
        {
            return new SelectListItem
            {
                Value = $"{x.Id}",
                Text = $"{x.Name}"
            };
        }).ToList();
    }

    private async Task<List<ProductModel>> GetProducts()
    {
        var dto = new ProductDTO
        {
            PagingArgs = Common.Models.PagingArgs.NoPaging
        };
        var apiHelper = new ApiCallHelper<ProductDTO>(this.clientFactory)
        {
            ControllerName = "Product"
        };
        var entities = await apiHelper.GetListAsync(dto);
        if (entities.Data.Any())
        {
            for (var i = 0; i < entities.Data.Count; i++)
            {
                entities.Data[i].PictureUrl = $"{AppSettings.ApiUrl}Picture?file={entities.Data[i].PictureUrl}&folder=Product";
            }
        }

        return entities.Data.Select(x =>
        {
            return new ProductModel
            {
                Id = x.Id,
                DateCreated = x.DateCreated,
                Description = x.Description,
                HasOffer = x.OfferEndDate.HasValue,
                IsActive = x.IsActive,
                OfferEndDate = x.OfferEndDate,
                OfferPrice = x.OfferPrice,
                Name = x.Name,
                Price = x.Price,
                PictureUrl = x.PictureUrl,
                Special = x.Special
            };
        }).ToList();
    }


    [HttpPost]
    [Route("Order/Delete/{id?}")]
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

    [HttpPost]
    [Route("Order/Complete/{id?}")]
    public async Task<JsonResult> Complete(int id)
    {
        if (id == 0)
        {
            return this.Json(false);
        }

        if (!this.ModelState.IsValid)
        {
            return this.Json(false);
        }

        var result = await this.apiCallHelper.Edit(new OrderDTO
        {
            Id = id,
            DateCreated = null,
            Completed = true
        });

        return !result.Succeeded ? this.Json(false) : this.Json(result.Data?.Id > 0);
    }
}
