namespace Pezza.BackEnd.Controllers
{
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json;
    using Pezza.Common.DTO;
    using Pezza.Portal.Helpers;

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

        public async Task<JsonResult> List(int limit, int page, string orderBy = "Name asc")
        {
            var json = JsonConvert.SerializeObject(new CustomerDTO
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
        public async Task<ActionResult> Create(CustomerDTO customer)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(customer);
            }

            var result = await this.apiCallHelper.Create(customer);
            return Validate<CustomerDTO>(result, this.apiCallHelper, customer);
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
            return Validate<CustomerDTO>(result, this.apiCallHelper, customer);
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
