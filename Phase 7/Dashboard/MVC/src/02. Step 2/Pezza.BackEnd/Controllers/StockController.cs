namespace Pezza.Portal.Controllers;

using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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

    public ActionResult Index()
    {
        return this.View(new Models.PagingModel
        {
            Limit = 10,
            Page = 1
        });
    }

    public async Task<JsonResult> List(int limit, int page, string orderBy = "Name asc")
    {
        var json = JsonConvert.SerializeObject(new StockDTO
        {
            OrderBy = orderBy,
            PagingArgs = new Common.Models.PagingArgs
            {
                Limit = limit,
                Offset = (page - 1) * limit,
                UsePaging = true
            }
        });
        var result = await this.apiCallHelper.GetListAsync(json);
        return this.Json(result);
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
        return Validate(result, this.apiCallHelper, stock);
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
        return Validate(result, this.apiCallHelper, stock);
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
