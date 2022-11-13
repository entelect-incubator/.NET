namespace Pezza.Portal.Helpers;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
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
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
            MaxDepth = 20
        };
    }

    public async Task<ListOutcome<T>> GetListAsync(string jsonData)
    {
        var data = new StringContent(jsonData, Encoding.UTF8, "application/json");
        var response = await this.client.PostAsync(@$"{AppSettings.ApiUrl}{this.ControllerName}\Search", data);

        var responseData = await response.Content.ReadAsStringAsync();

        var entities = System.Text.Json.JsonSerializer.Deserialize<ListOutcome<T>>(responseData, this.jsonSerializerOptions);
        return entities;
    }

    public async Task<T> GetAsync(int id)
    {
        var responseMessage = await this.client.GetAsync(@$"{AppSettings.ApiUrl}{this.ControllerName}\{id}");
        if (responseMessage.IsSuccessStatusCode)
        {
            var responseData = await responseMessage.Content.ReadAsStringAsync();
            return System.Text.Json.JsonSerializer.Deserialize<T>(responseData, this.jsonSerializerOptions);
        }

        return default;
    }

    public async Task<Result<T>> Create(T entity)
    {
        this.ValidationErrors = new List<ValidationError>();
        var json = JsonSerializer.Serialize(entity);
        var data = new StringContent(json, Encoding.UTF8, "application/json");

        var responseMessage = await this.client.PostAsync(@$"{AppSettings.ApiUrl}{this.ControllerName}", data);
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.BadRequest)
        {
            var responseData = await responseMessage.Content.ReadAsStringAsync();
            var response = JsonSerializer.Deserialize<Result>(responseData, this.jsonSerializerOptions);

            this.ValidationErrors = response.Errors.Select(x => (x as JObject).ToObject<ValidationError>()).ToList();

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
        this.ValidationErrors = new List<ValidationError>();
        var json = JsonSerializer.Serialize(entity);
        var data = new StringContent(json, Encoding.UTF8, "application/json");

        var responseMessage = await this.client.PutAsync(@$"{AppSettings.ApiUrl}{this.ControllerName}", data);
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.BadRequest)
        {
            var responseData = await responseMessage.Content.ReadAsStringAsync();
            var response = JsonSerializer.Deserialize<Result>(responseData, this.jsonSerializerOptions);

            this.ValidationErrors = response.Errors?.Select(x => (x as JObject).ToObject<ValidationError>()).ToList();

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
        var responseMessage = await this.client.DeleteAsync(@$"{AppSettings.ApiUrl}{this.ControllerName}\{id}");
        if (responseMessage.IsSuccessStatusCode)
        {
            var responseData = await responseMessage.Content.ReadAsStringAsync();
            return System.Text.Json.JsonSerializer.Deserialize<bool>(responseData, this.jsonSerializerOptions);
        }

        return false;
    }
}
