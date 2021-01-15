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
    using Pezza.Common.Entities;
    using Pezza.Common.Mapping;
    using Pezza.Portal.Models;

    public class OrdersController : BaseController
    {
        public async Task<ActionResult> Index()
        {
            var data = new StringContent("", Encoding.UTF8, "application/json");
            var response = await this.client.PostAsync(@$"{AppSettings.ApiUrl}Order\Search", data);

            var responseData = await response.Content.ReadAsStringAsync();
            var entities = JsonConvert.DeserializeObject<List<OrderDTO>>(responseData);
            var entitiesByRestaurant = entities.OrderBy(o => o.Restaurant.Name).GroupBy(g => g.Restaurant.Name);
            var ordersByRestaurant = new Dictionary<string, List<OrderDTO>>();
            foreach(var restaurant in entitiesByRestaurant)
            {
                var tempEntities = new List<OrderDTO>();
                foreach(var order in restaurant)
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
            var entity = new Order();
            var responseMessage = await this.client.GetAsync(@$"{AppSettings.ApiUrl}Order\{id}");
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = await responseMessage.Content.ReadAsStringAsync();
                entity = JsonConvert.DeserializeObject<Order>(responseData);
            }

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

        private async Task<List<SelectListItem>> GetCustomers()
        {
            var data = new StringContent("", Encoding.UTF8, "application/json");
            var response = await this.client.PostAsync(@$"{AppSettings.ApiUrl}Customer\Search", data);

            var responseData = await response.Content.ReadAsStringAsync();
            var entities = JsonConvert.DeserializeObject<List<Customer>>(responseData);
            return entities.Select(x => {
                return new SelectListItem
                {
                    Value = $"{x.Id}",
                    Text = $"{x.Name} {x.Phone}"
                };
            }).ToList();
        }

        private async Task<List<SelectListItem>> GetRestaurants()
        {
            var data = new StringContent("", Encoding.UTF8, "application/json");
            var response = await this.client.PostAsync(@$"{AppSettings.ApiUrl}Restaurant\Search", data);

            var responseData = await response.Content.ReadAsStringAsync();
            var entities = JsonConvert.DeserializeObject<List<Restaurant>>(responseData).ToList();
            for (var i = 0; i < entities.Count; i++)
            {
                entities[i].PictureUrl = $"{AppSettings.ApiUrl}Picture?file={entities[i].PictureUrl}&folder=restaurant";
            }

            return entities.Select(x => {
                return new SelectListItem
                {
                    Value = $"{x.Id}",
                    Text = $"{x.Name}"
                };
            }).ToList();
        }

        private async Task<List<Product>> GetProducts()
        {
            var data = new StringContent("", Encoding.UTF8, "application/json");
            var response = await this.client.PostAsync(@$"{AppSettings.ApiUrl}Product\Search", data);

            var responseData = await response.Content.ReadAsStringAsync();
            var entities = JsonConvert.DeserializeObject<List<Product>>(responseData).ToList();
            for (var i = 0; i < entities.Count; i++)
            {
                entities[i].PictureUrl = $"{AppSettings.ApiUrl}Picture?file={entities[i].PictureUrl}&folder=Product";
            }

            return entities;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(OrderDataDTO Order)
        {
            if (this.ModelState.IsValid)
            {
                var json = JsonConvert.SerializeObject(Order);
                var data = new StringContent(json, Encoding.UTF8, "application/json");

                var responseMessage = await this.client.PostAsync(@$"{AppSettings.ApiUrl}Order", data);
                if (responseMessage.IsSuccessStatusCode)
                {
                    var responseData = await responseMessage.Content.ReadAsStringAsync();
                    var response = JsonConvert.DeserializeObject<OrderDTO>(responseData);
                }

                return this.RedirectToAction("Index");
            }
            else
            {
                return this.View(Order);
            }
        }

        [Route("Order/Edit/{id?}")]
        public async Task<ActionResult> Edit(int id)
        {
            var entity = new OrderDTO();
            var response = await this.client.GetAsync(@$"{AppSettings.ApiUrl}Order\{id}");
            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();
                entity = JsonConvert.DeserializeObject<OrderDTO>(responseData);
            }
            return this.View(new OrderDataDTO
            {
               
            });
        }

        [HttpPost]
        [Route("Order/Edit/{id?}")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, OrderDataDTO Order)
        {
            if (this.ModelState.IsValid)
            {
                var json = JsonConvert.SerializeObject(Order);
                var data = new StringContent(json, Encoding.UTF8, "application/json");

                var responseMessage = await this.client.PutAsync(@$"{AppSettings.ApiUrl}Order\{id}", data);
                if (responseMessage.IsSuccessStatusCode)
                {
                    var responseData = await responseMessage.Content.ReadAsStringAsync();
                    var response = JsonConvert.DeserializeObject<OrderDTO>(responseData);
                }

                return this.RedirectToAction("Index");
            }
            else
            {
                return this.View(Order);
            }
        }

        [HttpPost]
        [Route("Order/Delete/{id?}")]
        public async Task<JsonResult> Delete(int id)
        {
            if (id == 0)
            {
                return this.Json(false);
            }

            if (this.ModelState.IsValid)
            {
                var responseMessage = await this.client.DeleteAsync(@$"{AppSettings.ApiUrl}Order\{id}");
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

        [HttpPost]
        [Route("Order/Complete/{id?}")]
        public async Task<JsonResult> Complete(int id)
        {
            if (id == 0)
            {
                return this.Json(false);
            }

            if (this.ModelState.IsValid)
            {
                var json = JsonConvert.SerializeObject(new OrderDataDTO
                {
                    Completed = true
                });
                var data = new StringContent(json, Encoding.UTF8, "application/json");

                var responseMessage = await this.client.PutAsync(@$"{AppSettings.ApiUrl}Order\{id}", data);
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
