using System.Net;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Plot.Data.Models.Allocations;
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

        try
        {
            return await base.SendAsync(httpRequestMessage);
        }
        catch (TimeoutException ex)
        {
            return new HttpResponseMessage(HttpStatusCode.RequestTimeout)
            {
                Content = new StringContent($"Request timed out: {ex.Message}")
            };
        }
    }

    private async Task<HttpResponseMessage> SendAsync(string endpoint, HttpMethod method)
    {
        //string fullUrl = $"{BaseAddress}{controller}{endpoint}";

        HttpRequestMessage httpRequestMessage = new HttpRequestMessage(method, $"{BaseAddress}{controller}{endpoint}");

        var token = _httpContextAccessor.HttpContext?.Request.Cookies["Auth"];
        httpRequestMessage.Headers.Add("Authorization", $"Bearer {token}");

        try
        {
            return await base.SendAsync(httpRequestMessage);

        }
        catch (Exception ex)
        {

            return new HttpResponseMessage(HttpStatusCode.InternalServerError)
            {
                Content = new StringContent("Error: " + ex.Message)
            };
        }

    }

    
    private async Task<HttpResponseMessage> SendAsync(string endpoint, HttpMethod method, MultipartFormDataContent content)
    {
        HttpRequestMessage httpRequestMessage = new HttpRequestMessage(method, $"{BaseAddress}{controller}{endpoint}");
        httpRequestMessage.Content = content;

        var token = _httpContextAccessor.HttpContext?.Request.Cookies["Auth"];
        httpRequestMessage.Headers.Add("Authorization", $"Bearer {token}");

        return await base.SendAsync(httpRequestMessage);
    }

    public async Task<T?> SendGetAsync<T>(string endpoint)
    {
        //Console.WriteLine("In SendGetAsync");
        var response = await SendAsync(endpoint, HttpMethod.Get);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<T>();
        }

        return default;
    }

    public async Task<(HttpStatusCode, T?)> SendPostAsync<T>(string endpoint, JsonContent body)
    {
        var response = await SendAsync(endpoint, HttpMethod.Post, body);

        if (response.IsSuccessStatusCode)
        {
            response.Content.Headers.TryGetValues("Content-Type", out var headers);
            var contentType = headers?.FirstOrDefault();

            switch (contentType)
            {
                case "application/json; charset=utf-8":
                    return (response.StatusCode, await response.Content.ReadFromJsonAsync<T>());
                default:
                    return (response.StatusCode, default);
            }
        }

        return (response.StatusCode, default);
    }

    public async Task<HttpStatusCode> SendPostContentAsync(string endpoint, UploadFile file)
    {
        
        if(file!=null)
        {
            var content = new MultipartFormDataContent();
            var fileContent = new StreamContent(file.Stream);
            
            fileContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);

            content.Add(fileContent, "file", file.FileName);
            
            var response = await SendAsync(endpoint, HttpMethod.Post,content);

            if(response!=null)
            {
                return response.StatusCode;
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