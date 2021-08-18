namespace Pezza.Website.Pages
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using Pezza.Common;
    using Pezza.Common.DTO;
    using Pezza.Website.Helpers;

    public class IndexModel : PageModel
    {
        private readonly ApiCallHelper<ProductDTO> apiCallHelperProducts;
        private readonly ApiCallHelper<RestaurantDTO> apiCallHelperRestaurants;

        public readonly HttpClient client;

        public readonly IHttpClientFactory clientFactory;

        public IndexModel(IHttpClientFactory clientFactory)
        {
            this.clientFactory = clientFactory;
            this.client = clientFactory.CreateClient();
            this.client.BaseAddress = new Uri(AppSettings.ApiUrl);
            this.client.DefaultRequestHeaders.Accept.Clear();
            this.client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            this.apiCallHelperProducts = new ApiCallHelper<ProductDTO>(this.clientFactory);
            this.apiCallHelperProducts.ControllerName = "Product";
            this.apiCallHelperRestaurants = new ApiCallHelper<RestaurantDTO>(this.clientFactory);
            this.apiCallHelperRestaurants.ControllerName = "Restaurant";
        }

        public async Task OnGetAsync()
        {
            var taskLoadProducts = this.LoadProducts();
            var taskLoadRestaurants = this.LoadRestaurants();

            await Task.WhenAll(taskLoadProducts, taskLoadRestaurants);

            TempData["ProductData"] = await taskLoadProducts;
            TempData["RestaurantData"] = await taskLoadRestaurants;

        }

        public async Task<List<ProductDTO>> LoadProducts()
        {
            var json = JsonConvert.SerializeObject(new ProductDTO
            {
                OrderBy = "Name asc",
                PagingArgs = Common.Models.PagingArgs.NoPaging
            });
            var result = await this.apiCallHelperProducts.GetListAsync(json);
            for (var i = 0; i < result.Data.Count; i++)
            {
                var picture = result.Data[i].PictureUrl;
                result.Data[i].PictureUrl = $"{AppSettings.ApiUrl}Picture?file={picture}&folder={this.apiCallHelperProducts.ControllerName}";
            }

            return result.Data;
        }

        public async Task<List<RestaurantDTO>> LoadRestaurants()
        {
            var json = JsonConvert.SerializeObject(new ProductDTO
            {
                OrderBy = "Name asc",
                PagingArgs = Common.Models.PagingArgs.NoPaging
            });
            var result = await this.apiCallHelperRestaurants.GetListAsync(json);
            for (var i = 0; i < result.Data.Count; i++)
            {
                var picture = result.Data[i].PictureUrl;
                result.Data[i].PictureUrl = $"{AppSettings.ApiUrl}Picture?file={picture}&folder={this.apiCallHelperRestaurants.ControllerName}";
            }

            return result.Data;
        }
    }
}
