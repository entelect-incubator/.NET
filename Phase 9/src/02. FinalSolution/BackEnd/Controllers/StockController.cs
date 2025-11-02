using BackEnd.Controllers;

namespace Portal.Controllers;

using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Common.DTO;
using Portal.Helpers;

public class StockController : BaseController
{
    private readonly ApiCallHelper<PizzaModel> apiCallHelper;

    public StockController(IHttpClientFactory clientFactory)
        : base(clientFactory)
    {
        this.apiCallHelper = new ApiCallHelper<PizzaModel>(this.clientFactory)
        {
            ControllerName = "Stock"
        };
    }

    public ActionResult Index() => this.View(new Models.PagingModel
    {
        Limit = 10,
        Page = 1
    });

    public async Task<JsonResult> List(int limit, int page, string orderBy = "Name asc")
    {
        var dto = new PizzaModel
        {
            OrderBy = orderBy,
            PagingArgs = new Common.Models.PagingArgs
            {
                Limit = limit,
                Offset = (page - 1) * limit,
                UsePaging = true
            }
        };
        var result = await this.apiCallHelper.GetListAsync(dto);
        return this.Json(result);
    }

    public async Task<ActionResult> Details(int id)
    {
        var entity = await this.apiCallHelper.GetAsync(id);
        return this.View(entity);
    }

    public ActionResult Create() => this.View(new PizzaModel());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Create(PizzaModel pizza)
    {
        if (!this.ModelState.IsValid)
        {
            return this.View(pizza);
        }

        var result = await this.apiCallHelper.Create(pizza);
        return this.Validate(result, this.apiCallHelper, pizza);
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
    public async Task<ActionResult> Edit(int id, PizzaModel pizza)
    {
        if (!this.ModelState.IsValid)
        {
            return this.View(pizza);
        }

        pizza.Id = id;
        var result = await this.apiCallHelper.Edit(pizza);
        return this.Validate(result, this.apiCallHelper, pizza);
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
