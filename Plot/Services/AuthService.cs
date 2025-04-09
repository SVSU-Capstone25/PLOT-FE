using System.Threading.Tasks;
using Microsoft.JSInterop;
using Plot.Data.Models.Auth.Login;
using Plot.Services;

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
public class AuthService : PlotHttpClient
{
    // Methods -- Methods -- Methods -- Methods -- Methods -- Methods -----

    /// <summary>
    /// Inect the JS runtime and HTTP context accessor.
    /// </summary>
    /// <param name="jsRuntime"></param>
    /// <param name="httpContextAccessor"></param>
    public AuthService(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor, "/auth")
    { }


    /// <summary>
    /// Method to login a user using the JS interop.
    /// This method will call the loginUser function in the JS file and pass the email and password to it.
    /// </summary>
    /// <param name="email"></param>
    /// <param name="password"></param>
    /// <returns> bool</returns> If the login was successful, it will return true, otherwise false.
    public async Task<LoginToken?> LoginAsync(LoginRequest loginRequest)
    {
        JsonContent body = JsonContent.Create(loginRequest);

        return await SendPostAsync<LoginToken>("/login", body);
    }

    /// <summary>
    /// Method to logout a user using the JS interop.
    /// </summary>
    /// <returns>bool</returns> If the logout was successful, it will return true, otherwise false.
    public async Task<bool> LogOutAsync()
    {
        JsonContent body = JsonContent.Create("");

        return await SendPostAsync<bool>("/logout", body);
    }
}