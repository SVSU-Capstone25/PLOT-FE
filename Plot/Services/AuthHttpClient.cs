
using Plot.Data.Models.Auth.Registration;
using Plot.Data.Models.Auth.ResetPassword;

public class AuthHttpClient
{
    private const string BASE_CONTROLLER_ADDRESS="/auth" ;

    private readonly AuthHeaderHttpClient _authHeaderHttpClient;



    public AuthHttpClient(AuthHeaderHttpClient authHeaderHttpClient)
    {
        _authHeaderHttpClient = authHeaderHttpClient;
        _authHeaderHttpClient.AppendBaseAddress(BASE_CONTROLLER_ADDRESS);
    }

    
    // reset-password-request
    // 
    public async Task<HttpResponseMessage> ResetPasswordRequest(ResetPasswordRequest email)
    {
        string endpoint ="/reset-password-request";
        HttpMethod httpMethod = HttpMethod.Post;
        JsonContent jsonBody = JsonContent.Create(email);

        var response = await _authHeaderHttpClient.SendAsyncWithAuth(endpoint, httpMethod, jsonBody);
        
        return response;
    }
 
    
    // reset-password
    public async Task<HttpResponseMessage> ResetPassword(ResetPassword receivedResetPassword)
    {
        string endpoint ="/reset-password";
        HttpMethod httpMethod = HttpMethod.Post;
        JsonContent jsonBody = JsonContent.Create(receivedResetPassword);

        var response = await _authHeaderHttpClient.SendAsyncWithAuth(endpoint, httpMethod, jsonBody);
        
        return response;
    }
        

    // register
    public async Task<HttpResponseMessage> Register(UserRegistration user)
    {
        string endpoint ="/register";
        HttpMethod httpMethod = HttpMethod.Post;
        JsonContent jsonBody = JsonContent.Create(user);

        var response = await _authHeaderHttpClient.SendAsyncWithAuth(endpoint, httpMethod, jsonBody);
        
        return response;
    }
}