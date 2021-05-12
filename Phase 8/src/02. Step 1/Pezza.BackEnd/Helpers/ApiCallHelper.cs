namespace Pezza.Portal.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Net.Http.Json;
    using System.Text;
    using System.Text.Json;
    using System.Threading.Tasks;
    using Newtonsoft.Json.Linq;
    using Pezza.Common;
    using Pezza.Common.Models;

    public class ApiCallHelper<T>
    {
        private readonly IHttpClientFactory clientFactory;

        private readonly HttpClient client;

        public List<ValidationError> ValidationErrors;

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

        private async Task<string> SetBearerToken()
        {
            var json = JsonSerializer.Serialize(new AuthModel
            {
                ApiKey = "940312b1cd649122b2f29fc2a68e47dbfaf12aca"
            });
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var responseMessage = await this.client.PostAsync(@$"{AppSettings.ApiUrl}Auth/Authorise", data);
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = await responseMessage.Content.ReadAsStringAsync();
                var response = JsonSerializer.Deserialize<TokenModel>(responseData, this.jsonSerializerOptions);
                return response.Token;
            }

            return string.Empty;
        }

        public async Task<ListOutcome<T>> GetListAsync(T entity)
        {
            this.client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await this.SetBearerToken());

            var json = JsonSerializer.Serialize(entity);
            var response = await this.client.PostAsync(@$"{AppSettings.ApiUrl}{ControllerName}\Search", new StringContent(json, Encoding.UTF8, "application/json"));
            var responseData = await response.Content.ReadAsStringAsync();

            var entities = JsonSerializer.Deserialize<ListOutcome<T>>(responseData, this.jsonSerializerOptions);
            return entities;
        }

        public async Task<T> GetAsync(int id)
        {
            this.client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await this.SetBearerToken());

            var responseMessage = await this.client.GetAsync(@$"{AppSettings.ApiUrl}{ControllerName}\{id}");
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = await responseMessage.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<T>(responseData, this.jsonSerializerOptions);
            }

            return default;
        }

        public async Task<Result<T>> Create(T entity)
        {
            this.client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await this.SetBearerToken());

            this.ValidationErrors = new List<ValidationError>();
            var json = JsonSerializer.Serialize(entity);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var responseMessage = await this.client.PostAsync(@$"{AppSettings.ApiUrl}{ControllerName}", data);
            if (responseMessage.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                var responseData = await responseMessage.Content.ReadAsStringAsync();
                var response = JsonSerializer.Deserialize<Result>(responseData, this.jsonSerializerOptions);

                this.ValidationErrors = response.Errors.Select(x =>
                {
                    return (x as JObject).ToObject<ValidationError>();
                }).ToList();

                return Result<T>.Failure("ValidationError");
            }

            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = await responseMessage.Content.ReadAsStringAsync();
                var response = JsonSerializer.Deserialize<T>(responseData, this.jsonSerializerOptions);

                return Result<T>.Success(response);
            }

            return Result<T>.Failure("Error");
        }

        public async Task<Result<T>> Edit(T entity)
        {
            this.client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await this.SetBearerToken());

            this.ValidationErrors = new List<ValidationError>();
            var json = JsonSerializer.Serialize(entity);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var responseMessage = await this.client.PutAsync(@$"{AppSettings.ApiUrl}{ControllerName}", data);
            if (responseMessage.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                var responseData = await responseMessage.Content.ReadAsStringAsync();
                var response = JsonSerializer.Deserialize<Result>(responseData, this.jsonSerializerOptions);

                this.ValidationErrors = response.Errors.Select(x =>
                {
                    return (x as JObject).ToObject<ValidationError>();
                }).ToList();

                return Result<T>.Failure("ValidationError");
            }

            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = await responseMessage.Content.ReadAsStringAsync();
                var response = JsonSerializer.Deserialize<T>(responseData, this.jsonSerializerOptions);

                return Result<T>.Success(response);
            }

            return Result<T>.Failure("Error");
        }

        public async Task<bool> Delete(int id)
        {
            this.client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await this.SetBearerToken());

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
