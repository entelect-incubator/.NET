namespace Pezza.BackEnd.Controllers
{
    using System;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using Microsoft.AspNetCore.Mvc;
    using Pezza.Common;

    public abstract class BaseController : Controller
    {
        public readonly HttpClient client;

        public BaseController()
        {
            this.client = new HttpClient
            {
                BaseAddress = new Uri(AppSettings.ApiUrl)
            };
            this.client.DefaultRequestHeaders.Accept.Clear();
            this.client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
    }
}
