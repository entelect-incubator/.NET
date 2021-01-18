namespace Pezza.BackEnd.Controllers
{
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json;
    using Pezza.Common;
    using Pezza.Common.DTO;
    using Pezza.Common.Entities;
    using Pezza.Common.Models;

    public class StockController : BaseController
    {
        public async Task<ActionResult> Index()
        {
            var data = new StringContent("", Encoding.UTF8, "application/json");
            var response = await this.client.PostAsync(@$"{AppSettings.ApiUrl}Stock\Search", data);

            var responseData = await response.Content.ReadAsStringAsync();
            var entities = JsonConvert.DeserializeObject<List<StockDTO>>(responseData);

            return this.View(entities);
        }

        public async Task<ActionResult> Details(int id)
        {
            var entity = new StockDTO();
            var responseMessage = await this.client.GetAsync(@$"{AppSettings.ApiUrl}Stock\{id}");
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = await responseMessage.Content.ReadAsStringAsync();
                entity = JsonConvert.DeserializeObject<StockDTO>(responseData);
            }

            return this.View(entity);
        }

        public ActionResult Create()
        {
            return this.View(new StockDTO());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(StockDTO stock)
        {
            if (this.ModelState.IsValid)
            {
                var json = JsonConvert.SerializeObject(stock);
                var data = new StringContent(json, Encoding.UTF8, "application/json");

                var responseMessage = await this.client.PostAsync(@$"{AppSettings.ApiUrl}Stock", data);
                if (responseMessage.IsSuccessStatusCode)
                {
                    var responseData = await responseMessage.Content.ReadAsStringAsync();
                    var response = JsonConvert.DeserializeObject<StockDTO>(responseData);
                }

                return this.RedirectToAction("Index");
            }
            else
            {
                return this.View(stock);
            }
        }

        [Route("Stock/Edit/{id?}")]
        public async Task<ActionResult> Edit(int id)
        {
            var entity = new StockDTO();
            var response = await this.client.GetAsync(@$"{AppSettings.ApiUrl}Stock\{id}");
            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();
                entity = JsonConvert.DeserializeObject<StockDTO>(responseData);
            }
            return this.View(entity);
        }

        [HttpPost]
        [Route("Stock/Edit/{id?}")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, StockDTO stock)
        {
            if (this.ModelState.IsValid)
            {
                var json = JsonConvert.SerializeObject(new StockDTO
                {
                    Name = stock.Name,
                    Quantity = stock.Quantity,
                    UnitOfMeasure = stock.UnitOfMeasure,
                    ValueOfMeasure = stock.ValueOfMeasure,
                    ExpiryDate = stock.ExpiryDate,
                    Comment = stock.Comment
                });
                var data = new StringContent(json, Encoding.UTF8, "application/json");

                var responseMessage = await this.client.PutAsync(@$"{AppSettings.ApiUrl}Stock\{id}", data);
                if (responseMessage.IsSuccessStatusCode)
                {
                    var responseData = await responseMessage.Content.ReadAsStringAsync();
                    var response = JsonConvert.DeserializeObject<StockDTO>(responseData);
                }

                return this.RedirectToAction("Index");
            }
            else
            {
                return this.View(stock);
            }
        }

        [HttpPost]
        [Route("Stock/Delete/{id?}")]
        public async Task<JsonResult> Delete(int id)
        {
            if (id == 0)
            {
                return this.Json(false);
            }

            if (this.ModelState.IsValid)
            {
                var responseMessage = await this.client.DeleteAsync(@$"{AppSettings.ApiUrl}Stock\{id}");
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
