using System.Net;
using Plot.Data.Models.Auth.Login;
using Plot.Data.Models.Auth.Registration;
using Plot.Data.Models.Auth.ResetPassword;
using Plot.Data.Models.Users;
using Plot.Services;
public class AuthHttpClient : PlotHttpClient
{
    public AuthHttpClient(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor, "/auth")
    { }

    public async Task<LoginToken?> LoginAsync(LoginRequest loginRequest)
    {
        JsonContent body = JsonContent.Create(loginRequest);

        var (status, response) = await SendPostAsync<LoginToken>("/login", body);

        return response;
    }

    public async Task<HttpStatusCode> ResetPasswordRequest(ResetPasswordRequest email)
    {
        JsonContent body = JsonContent.Create(email);

        var (status, response) = await SendPostAsync<HttpStatusCode>("/reset-password-request", body);

        return status;
    }

    public async Task<HttpStatusCode> ResetPassword(ResetPassword receivedResetPassword)
    {
        JsonContent body = JsonContent.Create(receivedResetPassword);

        var (status, response) = await SendPostAsync<HttpStatusCode>("/reset-password", body);

        return status;
    }

    public async Task<HttpStatusCode> Register(UserRegistration user)
    {
        JsonContent body = JsonContent.Create(user);

        var (status, response) = await SendPostAsync<HttpStatusCode>("/register", body);

        return status;
    }

    public async Task<UserDTO?> GetCurrentUser()
    {
        return await SendGetAsync<UserDTO>("/get-current-user");
    }
}