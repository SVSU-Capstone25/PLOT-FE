/*
    Filename: AuthHttpClient.cs
    Part of Project: PLOT/PLOT-FE/Services

    File Purpose:
    This file contains a wrapper for httpClient, this allows us to send a 
    bearer token in every header for authentication for BE api endpoints.

    Class Purpose:
    The class is a wrapper for httpClient for authentication when talking with BE api endpoints.
    With the use of Blazor everytime you navigate to a new page the Authentication Bearer token
    gets reset. This class assures the Authentication Bearer token is set based on browser cookies
    for every HTTP request.

    Written by: Michael Polhill
*/
using System.Net.Http.Headers;
using Microsoft.JSInterop;

public class AuthHeaderHttpClient
{
    // VARIABLES -- VARIABLES -- VARIABLES -- VARIABLES -- VARIABLES ------

    //Dependency injection of httpClient, this is used to send HTTP requests
    private readonly HttpClient _httpClient;
    //Dependency injection of IHttpContextAccessor, this is used to get the JWT token from the cookie

    private readonly IHttpContextAccessor _httpContextAccessor;

    //Dependency injection to run JavaScript scripts
    private readonly IJSRuntime _jsRuntime;

    // // HTTP method to be used in the request
    // public HttpMethod httpMethod { get; set; }

    // // Relative endpoint url for the BE api
    // public string endPointStr { get; set; }

    // // Holds json body content for the request
    // public JsonContent jsonBody { get; set; }


    // Methods -- Methods -- Methods -- Methods -- Methods -- Methods -----


    /// <summary>
    /// Constructor for injecting dependencys
    /// </summary>
    /// <param name="httpClient">Dependency injection of httpclient</param>
    /// <param name="jsRuntime">Dependency injection of jsRuntime</param>
    public AuthHeaderHttpClient(HttpClient httpClient, IJSRuntime jsRuntime, IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor=httpContextAccessor;
        _httpClient = httpClient;
        _jsRuntime = jsRuntime;
        // httpMethod = HttpMethod.Post;
        // endPointStr = "";
        // jsonBody = JsonContent.Create("");
    }


    /// <summary>
    /// Send a custom HttpRequestMessage with a JWT Auth Header
    /// </summary>
    /// <param name="request">Http request message to be sent</param>
    /// <returns>Response from the HttpRequest</returns>
    public async Task<HttpResponseMessage> SendAsyncWithAuth(HttpRequestMessage request)
    {
        try
        {
            // Use Javascript function to get the Jwt Auth token from a cookie stored in the browser.
            //var token = await _jsRuntime.InvokeAsync<string>("getCookie", "Auth");
            var token = _httpContextAccessor.HttpContext?.Request.Cookies["Auth"];// Fuck JS contextaccessor is new best friend now

            // Test if the Auth token exists
            if (!string.IsNullOrEmpty(token))
            {
                //Set Authentication Header Bearer token with token from browser
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            //Return response
            return await _httpClient.SendAsync(request);
        }
        catch (Exception exception)
        {
            Console.WriteLine($"Exception Message: {exception.Message}");
            Console.WriteLine("----------------------------------------------------------------------");
            Console.WriteLine($"Exception StackTrace: {exception.StackTrace}");
            Console.WriteLine("----------------------------------------------------------------------");
            Console.WriteLine($"Exception StackTrace: {exception.Source}");
            var fallbackResponse = new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError)
            {
                Content = new StringContent("An error occurred while processing the request.")
            };
            return fallbackResponse;
        }
    }


    // /// <summary>
    // /// Constructs and sends an HttpRequestMessage based on class variables.
    // /// </summary>
    // /// <returns>Response from the HttpRequest</returns>
    // public async Task<HttpResponseMessage> SendAsyncWithAuth()
    // {
    //     //Set the full Uri of the targeted api endpoint
    //     var fullEndPointUri = new Uri(_httpClient.BaseAddress! + endPointStr);

    //     //Make a new http message with inputted http method
    //     var request = new HttpRequestMessage(httpMethod,
    //             fullEndPointUri);

    //     //Set the headers in the request to Accept json
    //     request.Headers.Add("Accept", "application/json");

    //     // Add jsonBody to the request
    //     request.Content = jsonBody;

    //     // Use Javascript function to get the Jwt Auth token from a cookie stored in the browser.
    //     //var token = await _jsRuntime.InvokeAsync<string>("getCookie", "Auth");
    //     var token = _httpContextAccessor.HttpContext?.Request.Cookies["Auth"];// Fuck JS contextaccessor is new best friend now


    //     // Test if the Auth token exists
    //     if (!string.IsNullOrEmpty(token))
    //     {
    //         //Set Authentication Header Bearer token with token from browser
    //         request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
    //     }

    //     var response = await _httpClient.SendAsync(request);

    //     clearRequest();

    //     //Return response
    //     return response;
    // }

    public async Task<HttpResponseMessage> SendAsyncWithAuth(string endpoint, HttpMethod httpMethod, JsonContent jsonBody )
    {
        //Set the full Uri of the targeted api endpoint
        var fullEndPointUri = new Uri(_httpClient.BaseAddress! + endpoint);

        //Make a new http message with inputted http method
        var request = new HttpRequestMessage(httpMethod,
                fullEndPointUri);

        //Set the headers in the request to Accept json
        request.Headers.Add("Accept", "application/json");

        // Add jsonBody to the request
        request.Content = jsonBody;

        // Use Javascript function to get the Jwt Auth token from a cookie stored in the browser.
        //var token = await _jsRuntime.InvokeAsync<string>("getCookie", "Auth");
        var token = _httpContextAccessor.HttpContext?.Request.Cookies["Auth"];// Fuck JS contextaccessor is new best friend now


        // Test if the Auth token exists
        if (!string.IsNullOrEmpty(token))
        {
            //Set Authentication Header Bearer token with token from browser
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        var response = await _httpClient.SendAsync(request);

        //clearRequest();

        //Return response
        return response;
    }


    /// <summary>
    /// Set httpClients BaseAddress
    /// </summary>
    /// <param name="baseAddress">Base uri for httpRequests</param>
    public void SetBaseAddress(Uri baseAddress)
    {
        _httpClient.BaseAddress = baseAddress;
    }


    /// <summary>
    /// Get httpClients BaseAddress
    /// </summary>
    /// <returns>Returns BaseAddress as a uri</returns>
    public Uri GetBaseAddress()
    {
        return _httpClient.BaseAddress!;
    }

    public void AppendBaseAddress(string controllerAddress)
    {
        var appendedBaseAddress = new Uri(_httpClient.BaseAddress! + controllerAddress);

        _httpClient.BaseAddress = appendedBaseAddress;
    }

    // private void clearRequest()
    // {
    //     httpMethod = HttpMethod.Post;
    //     endPointStr = "";
    //     jsonBody = JsonContent.Create("");
    // }
}
