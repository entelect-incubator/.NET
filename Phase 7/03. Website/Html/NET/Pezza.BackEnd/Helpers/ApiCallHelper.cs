namespace Pezza.Portal.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;
    using Pezza.Common;
    using System.Text.Json;
    using Pezza.Common.Models;

    public class ApiCallHelper<T>
    {
        private readonly IHttpClientFactory clientFactory;

        private readonly HttpClient client;

        private readonly JsonSerializerOptions jsonSerializerOptions;

        public string ControllerName { get; set; }

        public ApiCallHelper(IHttpClientFactory clientFactory)
        {
            this.clientFactory = clientFactory;
            this.client = clientFactory.CreateClient();
            this.client.BaseAddress = new Uri(AppSettings.ApiUrl);
            this.client.DefaultRequestHeaders.Accept.Clear();
            this.client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            this.jsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                IgnoreNullValues = true,
                MaxDepth = 20
            };
        }

        public async Task<ListOutcome<T>> GetListAsync(string jsonData)
        {
            var data = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var response = await this.client.PostAsync(@$"{AppSettings.ApiUrl}{ControllerName}\Search", data);

            var responseData = await response.Content.ReadAsStringAsync();

            var entities = JsonSerializer.Deserialize<ListOutcome<T>>(responseData, this.jsonSerializerOptions);
            return entities;
        }

        public async Task<T> GetAsync(int id)
        {
            var responseMessage = await this.client.GetAsync(@$"{AppSettings.ApiUrl}{ControllerName}\{id}");
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = await responseMessage.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<T>(responseData, this.jsonSerializerOptions);
            }

            return default;
        }

        public async Task<T> Create(T entity)
        {
            var json = JsonSerializer.Serialize(entity);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var responseMessage = await this.client.PostAsync(@$"{AppSettings.ApiUrl}{ControllerName}", data);
            if (responseMessage.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                var responseData = await responseMessage.Content.ReadAsStringAsync();
                var response = JsonSerializer.Deserialize<T>(responseData, this.jsonSerializerOptions);

                return response;
            }

            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = await responseMessage.Content.ReadAsStringAsync();
                var response = JsonSerializer.Deserialize<T>(responseData, this.jsonSerializerOptions);

                return response;
            }

            return default;
        }

        public async Task<T> Edit(T entity)
        {
            var json = JsonSerializer.Serialize(entity);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var responseMessage = await this.client.PutAsync(@$"{AppSettings.ApiUrl}{ControllerName}", data);
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = await responseMessage.Content.ReadAsStringAsync();
                var response = JsonSerializer.Deserialize<T>(responseData, this.jsonSerializerOptions);
                return response;
            }

            return default;
        }

        public async Task<bool> Delete(int id)
        {
            var responseMessage = await this.client.DeleteAsync(@$"{AppSettings.ApiUrl}{ControllerName}\{id}");
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = await responseMessage.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<bool>(responseData, this.jsonSerializerOptions);
            }

            return false;
        }
    }
}
