using System.Net;
using Plot.Data.Models.Env;

namespace Plot.Services;

public class PlotHttpClient : HttpClient
{
    string controller;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public PlotHttpClient(IHttpContextAccessor httpContextAccessor, string controller) : base()
    {
        this.controller = controller;
        _httpContextAccessor = httpContextAccessor;
        BaseAddress = new Uri(Environment.GetEnvironmentVariable("BACKEND_URL") ?? "http://backend:8085/api");
    }

    private async Task<HttpResponseMessage> SendAsync(string endpoint, HttpMethod method, JsonContent body)
    {
        HttpRequestMessage httpRequestMessage = new HttpRequestMessage(method, $"{BaseAddress}{controller}{endpoint}");
        httpRequestMessage.Content = body;

        var token = _httpContextAccessor.HttpContext?.Request.Cookies["Auth"];
        httpRequestMessage.Headers.Add("Authorization", $"Bearer {token}");

        return await base.SendAsync(httpRequestMessage);
    }

    private async Task<HttpResponseMessage> SendAsync(string endpoint, HttpMethod method)
    {
        HttpRequestMessage httpRequestMessage = new HttpRequestMessage(method, $"{BaseAddress}{controller}{endpoint}");

        var token = _httpContextAccessor.HttpContext?.Request.Cookies["Auth"];
        httpRequestMessage.Headers.Add("Authorization", $"Bearer {token}");

        return await base.SendAsync(httpRequestMessage);
    }

    public async Task<T?> SendGetAsync<T>(string endpoint)
    {
        var response = await SendAsync(endpoint, HttpMethod.Get);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<T>();
        }

        return default;
    }

    public async Task<T?> SendPostAsync<T>(string endpoint, JsonContent body)
    {
        var response = await SendAsync(endpoint, HttpMethod.Post, body);

        Console.WriteLine(response);

        if (response.IsSuccessStatusCode)
        {
            response.Content.Headers.TryGetValues("Content-Type", out var headers);
            var contentType = headers?.FirstOrDefault();

            switch (contentType)
            {
                case "application/json; charset=utf-8":
                    return await response.Content.ReadFromJsonAsync<T>();
                default:
                    return default;
            }
        }


        if (response.StatusCode == HttpStatusCode.BadRequest)
        {
            response.Content.Headers.TryGetValues("Content-Type", out var headers);
            var contentType = headers?.FirstOrDefault();

            switch (contentType)
            {
                case "application/json; charset=utf-8":
                    return await response.Content.ReadFromJsonAsync<T>();
                default:
                    return default;
            }
        }

        return default;
    }

    public async Task<HttpStatusCode> SendPatchAsync(string endpoint, JsonContent body)
    {
        var response = await SendAsync(endpoint, HttpMethod.Patch, body);
        return response.StatusCode;
    }

    public async Task<HttpStatusCode> SendDeleteAsync(string endpoint)
    {
        var response = await SendAsync(endpoint, HttpMethod.Delete);

        return response.StatusCode;
    }
}