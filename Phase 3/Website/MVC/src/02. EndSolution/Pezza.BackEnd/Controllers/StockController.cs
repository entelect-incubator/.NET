using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Pezza.Common;
using Pezza.Common.Entities;

namespace Pezza.BackEnd.Controllers
{
    public class StockController : BaseController
    {
        // GET: StockController
        public async Task<ActionResult> Index()
        {
            var entities = new List<Stock>();
            var data = new StringContent("", Encoding.UTF8, "application/json");
            var response = await this.client.PostAsync(@$"{AppSettings.ApiUrl}Stock\Search", data);

            var responseData = await response.Content.ReadAsStringAsync();
            entities = JsonConvert.DeserializeObject<List<Stock>>(responseData);

            return this.View(entities);
        }

        // GET: StockController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var entity = new Stock();
            var responseMessage = await this.client.GetAsync(@$"{AppSettings.ApiUrl}Stock\{id}");
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = await responseMessage.Content.ReadAsStringAsync();
                entity = JsonConvert.DeserializeObject<Stock>(responseData);
            }

            return this.View(entity);
        }

        // GET: StockController/Create
        public ActionResult Create()
        {
            return this.View();
        }

        // POST: StockController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return this.RedirectToAction(nameof(Index));
            }
            catch
            {
                return this.View();
            }
        }

        // GET: StockController/Edit/5
        public ActionResult Edit(int id)
        {
            return this.View();
        }

        // POST: StockController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return this.RedirectToAction(nameof(Index));
            }
            catch
            {
                return this.View();
            }
        }

        // GET: StockController/Delete/5
        public ActionResult Delete(int id)
        {
            return this.View();
        }

        // POST: StockController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return this.RedirectToAction(nameof(Index));
            }
            catch
            {
                return this.View();
            }
        }
    }
}
