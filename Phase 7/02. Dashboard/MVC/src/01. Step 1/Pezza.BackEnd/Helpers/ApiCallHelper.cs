namespace Pezza.Portal.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;
    using Newtonsoft.Json;
    using Pezza.Common;

    public class ApiCallHelper<T>
    {
        private readonly IHttpClientFactory clientFactory;

        private readonly HttpClient client;

        public string ControllerName { get; set; }

        public ApiCallHelper(IHttpClientFactory clientFactory)
        {
            this.clientFactory = clientFactory;
            this.client = clientFactory.CreateClient();
            this.client.BaseAddress = new Uri(AppSettings.ApiUrl);
            this.client.DefaultRequestHeaders.Accept.Clear();
            this.client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<List<T>> GetListAsync(string jsonData)
        {
            var data = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var response = await this.client.PostAsync(@$"{AppSettings.ApiUrl}{ControllerName}\Search", data);

            var responseData = await response.Content.ReadAsStringAsync();
            var entities = JsonConvert.DeserializeObject<List<T>>(responseData);

            return entities;
        }

        public async Task<T> GetAsync(int id)
        {
            var responseMessage = await this.client.GetAsync(@$"{AppSettings.ApiUrl}{ControllerName}\{id}");
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = await responseMessage.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(responseData);
            }

            return default;
        }

        public async Task<T> Create(T entity)
        {
            var json = JsonConvert.SerializeObject(entity);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var responseMessage = await this.client.PostAsync(@$"{AppSettings.ApiUrl}{ControllerName}", data);
            if (responseMessage.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                var responseData = await responseMessage.Content.ReadAsStringAsync();
                var response = JsonConvert.DeserializeObject<T>(responseData);

                return response;
            }

            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = await responseMessage.Content.ReadAsStringAsync();
                var response = JsonConvert.DeserializeObject<T>(responseData);

                return response;
            }

            return default;
        }

        public async Task<T> Edit(T entity)
        {
            var json = JsonConvert.SerializeObject(entity);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var responseMessage = await this.client.PutAsync(@$"{AppSettings.ApiUrl}{ControllerName}", data);
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = await responseMessage.Content.ReadAsStringAsync();
                var response = JsonConvert.DeserializeObject<T>(responseData);
            }

            return default;
        }

        public async Task<bool> Delete(int id)
        {
            var responseMessage = await this.client.DeleteAsync(@$"{AppSettings.ApiUrl}{ControllerName}\{id}");
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = await responseMessage.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<bool>(responseData);
            }

            return false;
        }
    }
}
