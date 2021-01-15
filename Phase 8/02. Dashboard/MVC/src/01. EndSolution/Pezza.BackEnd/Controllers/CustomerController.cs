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
    using Pezza.Common.Entities;
    using Pezza.Common.Mapping;

    public class CustomerController : BaseController
    {
        public async Task<ActionResult> Index()
        {
            var data = new StringContent("", Encoding.UTF8, "application/json");
            var response = await this.client.PostAsync(@$"{AppSettings.ApiUrl}Customer\Search", data);

            var responseData = await response.Content.ReadAsStringAsync();
            var entities = JsonConvert.DeserializeObject<List<Customer>>(responseData);

            return this.View(entities);
        }

        public IActionResult CreatePartial()
        {
            return this.PartialView("~/views/Customer/_Create.cshtml", new CustomerDataDTO());
        }

        public async Task<ActionResult> Details(int id)
        {
            var entity = new Customer();
            var responseMessage = await this.client.GetAsync(@$"{AppSettings.ApiUrl}Customer\{id}");
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = await responseMessage.Content.ReadAsStringAsync();
                entity = JsonConvert.DeserializeObject<Customer>(responseData);
            }

            return this.View(entity);
        }

        public ActionResult Create()
        {
            return this.View(new CustomerDataDTO());
        }

        [HttpPost]
        public async Task<ActionResult> Create(CustomerDataDTO Customer)
        {
            if (this.ModelState.IsValid)
            {
                var json = JsonConvert.SerializeObject(Customer);
                var data = new StringContent(json, Encoding.UTF8, "application/json");

                var responseMessage = await this.client.PostAsync(@$"{AppSettings.ApiUrl}Customer", data);
                if (responseMessage.IsSuccessStatusCode)
                {
                    var responseData = await responseMessage.Content.ReadAsStringAsync();
                    var response = JsonConvert.DeserializeObject<CustomerDTO>(responseData);
                }

                return this.RedirectToAction("Index");
            }
            else
            {
                return this.View(Customer);
            }
        }

        [Route("Customer/Edit/{id?}")]
        public async Task<ActionResult> Edit(int id)
        {
            var entity = new CustomerDTO();
            var response = await this.client.GetAsync(@$"{AppSettings.ApiUrl}Customer\{id}");
            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();
                entity = JsonConvert.DeserializeObject<CustomerDTO>(responseData);
            }
            return this.View(new CustomerDataDTO
            {
                Address = new AddressBase
                {
                    Address = entity.Address,
                    City = entity.City,
                    Province = entity.Province,
                    ZipCode = entity.ZipCode
                },
                ContactPerson = entity.ContactPerson,
                Email = entity.Email,
                Name = entity.Name,
                Phone = entity.Phone
            });
        }

        [HttpPost]
        [Route("Customer/Edit/{id?}")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, CustomerDataDTO customer)
        {
            if (this.ModelState.IsValid)
            {
                var json = JsonConvert.SerializeObject(customer);
                var data = new StringContent(json, Encoding.UTF8, "application/json");

                var responseMessage = await this.client.PutAsync(@$"{AppSettings.ApiUrl}Customer\{id}", data);
                if (responseMessage.IsSuccessStatusCode)
                {
                    var responseData = await responseMessage.Content.ReadAsStringAsync();
                    var response = JsonConvert.DeserializeObject<CustomerDTO>(responseData);
                }

                return this.RedirectToAction("Index");
            }
            else
            {
                return this.View(customer);
            }
        }

        [HttpPost]
        [Route("Customer/Delete/{id?}")]
        public async Task<JsonResult> Delete(int id)
        {
            if (id == 0)
            {
                return this.Json(false);
            }

            if (this.ModelState.IsValid)
            {
                var responseMessage = await this.client.DeleteAsync(@$"{AppSettings.ApiUrl}Customer\{id}");
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
