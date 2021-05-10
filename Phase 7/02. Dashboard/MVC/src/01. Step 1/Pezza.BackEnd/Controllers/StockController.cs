namespace Pezza.BackEnd.Controllers
{
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json;
    using Pezza.Common;
    using Pezza.Common.DTO;
    using Pezza.Portal.Helpers;

    public class StockController : BaseController
    {
        private readonly ApiCallHelper<StockDTO> apiCallHelper;

        public StockController(IHttpClientFactory clientFactory)
            : base(clientFactory)
        {
            this.apiCallHelper = new ApiCallHelper<StockDTO>(this.clientFactory);
            this.apiCallHelper.ControllerName = "Stock";
        }

        public async Task<ActionResult> Index()
        {
            var json = JsonConvert.SerializeObject(new StockDTO
            {
                PagingArgs = Common.Models.PagingArgs.NoPaging
            });
            var entities = await this.apiCallHelper.GetListAsync(json);
            return this.View(entities);
        }

        public async Task<ActionResult> Details(int id)
        {
            var entity = await this.apiCallHelper.GetAsync(id);
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
            if (!this.ModelState.IsValid)
            {
                return this.View(stock);
            }

            var result = await this.apiCallHelper.Create(stock);
            return Validate<StockDTO>(result, this.apiCallHelper, stock);
        }

        [Route("Stock/Edit/{id?}")]
        public async Task<ActionResult> Edit(int id)
        {
            var entity = await this.apiCallHelper.GetAsync(id);
            return this.View(entity);
        }

        [HttpPost]
        [Route("Stock/Edit/{id?}")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, StockDTO stock)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(stock);
            }

            stock.Id = id;
            var result = await this.apiCallHelper.Edit(stock);
            return Validate<StockDTO>(result, this.apiCallHelper, stock);
        }

        [HttpPost]
        [Route("Stock/Delete/{id?}")]
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
