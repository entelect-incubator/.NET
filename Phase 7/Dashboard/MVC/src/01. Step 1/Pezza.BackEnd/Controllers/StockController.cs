namespace BackEnd.Controllers;

using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Common;
using Common.DTO;
using Portal.Helpers;

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
        return this.View(entities.Data);
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
    public async Task<ActionResult> Create(StockDTO pizza)
    {
        if (!this.ModelState.IsValid)
        {
            return this.View(pizza);
        }

        var result = await this.apiCallHelper.Create(pizza);
        return Validate<StockDTO>(result, this.apiCallHelper, pizza);
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
    public async Task<ActionResult> Edit(int id, StockDTO pizza)
    {
        if (!this.ModelState.IsValid)
        {
            return this.View(pizza);
        }

        pizza.Id = id;
        var result = await this.apiCallHelper.Edit(pizza);
        return Validate<StockDTO>(result, this.apiCallHelper, pizza);
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
