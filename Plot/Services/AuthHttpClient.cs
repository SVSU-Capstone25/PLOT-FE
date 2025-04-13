using System.Net;
using Plot.Data.Models.Auth.Registration;
using Plot.Data.Models.Auth.ResetPassword;
using Plot.Services;
using System.Text.Json;

public class AuthHttpClient : PlotHttpClient
{
    public AuthHttpClient(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor, "/auth")
    { }

    public async Task<HttpStatusCode> ResetPasswordRequest(ResetPasswordRequest email)
    {
        JsonContent body = JsonContent.Create(email);

        return await SendPostAsync<HttpStatusCode>("/reset-password-request", body);
    }

    public async Task<HttpStatusCode> ResetPassword(ResetPassword receivedResetPassword)
    {
        JsonContent body = JsonContent.Create(receivedResetPassword);

        return await SendPostAsync<HttpStatusCode>("/reset-password", body);
    }

    public async Task<HttpStatusCode> Register(UserRegistration user)
    {
        // Log the object as JSON for debugging
        var json = JsonSerializer.Serialize(user, new JsonSerializerOptions { WriteIndented = true });
        Console.WriteLine("In AuthHttpClient and JSON body is:\n" + json);

        JsonContent body = JsonContent.Create(user);
        Console.WriteLine("body in Register in AuthHttpClient is " + body);
        /*
                var json = JsonSerializer.Serialize(body, new JsonSerializerOptions { WriteIndented = true });
                Console.WriteLine("In Auth Http Client and JSON body is " + json);
        */
        return await SendPostAsync<HttpStatusCode>("/register", body);
    }
}