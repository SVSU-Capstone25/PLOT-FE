using System.Threading.Tasks;
using Microsoft.JSInterop;

/*
    Filename: AuthService.cs
    Part of Project: PLOT/PLOT-FE/Services

    File Purpose:
    This file contains a login and logout service for the Blazor frontend.

    Class Purpose:
    This class is responsible for handling authentication requests to the backend API.
    It uses JavaScript interop to call the login and logout functions defined in the Blazor app.

    Written by: Michael Polhill
*/
public class AuthService
{
    // VARIABLES -- VARIABLES -- VARIABLES -- VARIABLES -- VARIABLES ------
    //Used for calling the JS functions for login and logout.
    private readonly IJSRuntime _jsRuntime;
    

    //Used for getting the cookies from the request.
    private readonly IHttpContextAccessor _httpContextAccessor;

    //The URL for the authentication API.
    private string _jsAuthUrl = "";


     // Methods -- Methods -- Methods -- Methods -- Methods -- Methods -----

    /// <summary>
    /// Inect the JS runtime and HTTP context accessor.
    /// </summary>
    /// <param name="jsRuntime"></param>
    /// <param name="httpContextAccessor"></param>
    public AuthService(IJSRuntime jsRuntime, IHttpContextAccessor httpContextAccessor)
    {
        _jsRuntime=jsRuntime;
        _httpContextAccessor=httpContextAccessor;
        _jsAuthUrl="http://localhost:8085/api/auth";
    }


    /// <summary>
    /// Method to login a user using the JS interop.
    /// This method will call the loginUser function in the JS file and pass the email and password to it.
    /// </summary>
    /// <param name="email"></param>
    /// <param name="password"></param>
    /// <returns> bool</returns> If the login was successful, it will return true, otherwise false.
    public async Task<bool> LoginAsync(string email, string password)
    {
        // The loginAPIUri is the URL for the login API.
        var loginAPIUri= new Uri(_jsAuthUrl +"/login");

        // Use Js Interop to call the loginUser function in the JS file.
        var result = await _jsRuntime.InvokeAsync<bool>("loginUser", loginAPIUri, email, password);

        // Return the result of the login.
        return result;
    }

    /// <summary>
    /// Method to logout a user using the JS interop.
    /// </summary>
    /// <returns>bool</returns> If the logout was successful, it will return true, otherwise false.
    public async Task<bool> LogOutAsync()
    {   // The logoutAPIUri is the URL for the logout API.
        var logoutAPIUri= new Uri(_jsAuthUrl +"/logout");

        //Get the auth token to use in the request
        var token =_httpContextAccessor.HttpContext?.Request.Cookies["Auth"];
        
        // Use Js Interop to call the logoutUser function in the JS file.
        var result = await _jsRuntime.InvokeAsync<bool>("logoutUser", logoutAPIUri,token);
        
        //Return the result.
        return result;
    }
}