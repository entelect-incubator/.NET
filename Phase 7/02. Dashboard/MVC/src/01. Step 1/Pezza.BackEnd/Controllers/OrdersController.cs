namespace Pezza.BackEnd.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Newtonsoft.Json;
    using Pezza.Common;
    using Pezza.Common.DTO;
    using Pezza.Portal.Helpers;
    using Pezza.Portal.Models;

    public class OrdersController : BaseController
    {
        private readonly ApiCallHelper<OrderDTO> apiCallHelper;

        public OrdersController(IHttpClientFactory clientFactory)
            : base(clientFactory)
        {
            this.apiCallHelper = new ApiCallHelper<OrderDTO>(this.clientFactory);
            this.apiCallHelper.ControllerName = "Order";
        }

        public async Task<ActionResult> Index()
        {
            var json = JsonConvert.SerializeObject(new OrderDTO
            {
                PagingArgs = Common.Models.PagingArgs.NoPaging
            });
            var entities = await this.apiCallHelper.GetListAsync(json);
            var entitiesByRestaurant = entities.OrderBy(o => o.Restaurant.Name).GroupBy(g => g.Restaurant.Name);
            var ordersByRestaurant = new Dictionary<string, List<OrderDTO>>();
            foreach (var restaurant in entitiesByRestaurant)
            {
                var tempEntities = new List<OrderDTO>();
                foreach (var order in restaurant)
                {
                    tempEntities.Add(order);
                }
                ordersByRestaurant.Add(restaurant.Key, tempEntities);
            }

            return this.View(ordersByRestaurant);
        }

        public async Task<IActionResult> OrderItem()
        {
            return this.PartialView("~/views/Orders/_Products.cshtml", new OrderItemModel
            {
                Products = await this.GetProducts()
            });
        }

        public async Task<ActionResult> Details(int id)
        {
            var entity = await this.apiCallHelper.GetAsync(id);
            return this.View(entity);
        }

        public async Task<ActionResult> Create()
        {
            return this.View(new OrderModel
            {
                Customers = await this.GetCustomers(),
                Restaurants = await this.GetRestaurants()
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(OrderDTO order)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(order);
            }

            var result = await this.apiCallHelper.Create(order);
            return this.RedirectToAction("Index");
        }

        private async Task<List<SelectListItem>> GetCustomers()
        {
            var json = JsonConvert.SerializeObject(new CustomerDTO
            {
                PagingArgs = Common.Models.PagingArgs.NoPaging
            });
            var entities = await new ApiCallHelper<CustomerDTO>(this.clientFactory).GetListAsync(json);
            return entities.Select(x =>
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
            var json = JsonConvert.SerializeObject(new RestaurantDTO
            {
                PagingArgs = Common.Models.PagingArgs.NoPaging
            });
            var entities = await new ApiCallHelper<RestaurantDTO>(this.clientFactory).GetListAsync(json);
            for (var i = 0; i < entities.Count; i++)
            {
                entities[i].PictureUrl = $"{AppSettings.ApiUrl}Picture?file={entities[i].PictureUrl}&folder=restaurant";
            }

            return entities.Select(x =>
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
            var json = JsonConvert.SerializeObject(new ProductDTO
            {
                PagingArgs = Common.Models.PagingArgs.NoPaging
            });
            var entities = await new ApiCallHelper<ProductDTO>(this.clientFactory).GetListAsync(json);
            for (var i = 0; i < entities.Count; i++)
            {
                entities[i].PictureUrl = $"{AppSettings.ApiUrl}Picture?file={entities[i].PictureUrl}&folder=Product";
            }

            return entities.Select(x =>
            {
                return new ProductModel
                {
                    Id = x.Id,
                    DateCreated = x.DateCreated,
                    Description = x.Description,
                    HasOffer = x.OfferEndDate.HasValue ? true : false,
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

        [Route("Order/Edit/{id?}")]
        public async Task<ActionResult> Edit(int id)
        {
            var entity = await this.apiCallHelper.GetAsync(id);
            return this.View(entity);
        }

        [HttpPost]
        [Route("Order/Edit/{id?}")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, OrderDTO order)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(order);
            }

            order.Id = id;
            var result = await this.apiCallHelper.Edit(order);
            return this.RedirectToAction("Index");
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
                Completed = true
            });
            return this.Json(result != null ? true : false);
        }
    }
}
