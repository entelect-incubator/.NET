namespace Pezza.BackEnd.Controllers
{
    using System;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using Microsoft.AspNetCore.Mvc;
    using Pezza.Common;
    using Pezza.Common.Models;
    using Pezza.Portal.Helpers;

    public abstract class BaseController : Controller
    {
        public readonly HttpClient client;

        public readonly IHttpClientFactory clientFactory;

        public BaseController(IHttpClientFactory clientFactory)
        {
            this.clientFactory = clientFactory;
            this.client = clientFactory.CreateClient();
            this.client.BaseAddress = new Uri(AppSettings.ApiUrl);
            this.client.DefaultRequestHeaders.Accept.Clear();
            this.client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public ActionResult Validate<T>(Result<T> result, ApiCallHelper<T> apiCallHelper, object model)
        {
            if (!result.Succeeded)
            {
                if (apiCallHelper.ValidationErrors.Any())
                {
                    foreach (var validation in apiCallHelper.ValidationErrors)
                    {
                        ModelState.AddModelError(validation.Property, validation.Error);
                    }
                }

                return this.View(model);
            }

            if (!ModelState.IsValid)
            {
                return this.View(model);
            }

            return this.RedirectToAction("Index");
        }
    }
}
