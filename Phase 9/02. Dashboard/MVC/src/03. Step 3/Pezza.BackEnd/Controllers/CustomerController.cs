namespace Pezza.BackEnd.Controllers
{
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json;
    using Pezza.Common.DTO;
    using Pezza.Portal.Helpers;
    using Pezza.Portal.Models;

    public class CustomerController : BaseController
    {
        private readonly ApiCallHelper<CustomerDTO> apiCallHelper;

        public CustomerController(IHttpClientFactory clientFactory)
            : base(clientFactory)
        {
            this.apiCallHelper = new ApiCallHelper<CustomerDTO>(this.clientFactory);
            this.apiCallHelper.ControllerName = "Customer";
        }

        public ActionResult Index()
        {
            return this.View(new Portal.Models.PagingModel
            {
                Limit = 10,
                Page = 1
            });
        }

        [HttpPost]
        public async Task<JsonResult> List([FromBody] SearchModel<CustomerDTO> searchmodel)
        {
            var entity = searchmodel.SearchData;
            entity.OrderBy = searchmodel.OrderBy;
            entity.PagingArgs = new Common.Models.PagingArgs
            {
                Limit = searchmodel.Limit,
                Offset = (searchmodel.Page - 1) * searchmodel.Limit,
                UsePaging = true
            };
            var json = JsonConvert.SerializeObject(entity);
            var result = await this.apiCallHelper.GetListAsync(json);
            return this.Json(result);
        }

        public IActionResult CreatePartial()
        {
            return this.PartialView("~/views/Customer/_Create.cshtml", new CustomerDTO());
        }

        public async Task<ActionResult> Details(int id)
        {
            var entity = await this.apiCallHelper.GetAsync(id);
            return this.View(entity);
        }

        public ActionResult Create()
        {
            return this.View(new CustomerDTO());
        }

        [HttpPost]
        public async Task<ActionResult> Create(CustomerDTO Customer)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(Customer);
            }

            var result = await this.apiCallHelper.Create(Customer);
            return this.RedirectToAction("Index");
        }

        [Route("Customer/Edit/{id?}")]
        public async Task<ActionResult> Edit(int id)
        {
            var entity = await this.apiCallHelper.GetAsync(id);
            return this.View(entity);
        }

        [HttpPost]
        [Route("Customer/Edit/{id?}")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, CustomerDTO customer)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(customer);
            }

            customer.Id = id;
            var result = await this.apiCallHelper.Edit(customer);
            return this.RedirectToAction("Index");
        }

        [HttpPost]
        [Route("Customer/Delete/{id?}")]
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
