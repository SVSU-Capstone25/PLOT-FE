
using Plot.Data.Models.Auth.Registration;
using Plot.Data.Models.Auth.ResetPassword;
using Plot.Data.Models.Fixtures;
using Plot.Data.Models.Stores;
using Plot.Data.Models.Users;

//NEED TO FINISH
public class UsersHttpClient
{
    private const string BASE_CONTROLLER_ADDRESS="/users" ;

    private readonly AuthHeaderHttpClient _authHeaderHttpClient;



    public UsersHttpClient(AuthHeaderHttpClient authHeaderHttpClient)
    {
        _authHeaderHttpClient = authHeaderHttpClient;
        _authHeaderHttpClient.AppendBaseAddress(BASE_CONTROLLER_ADDRESS);
    }

    
    // [HttpGet("get-all")]
    // 
    public async Task<List<UserDTO>?> GetAllUsers()
    {
        string endpoint = "/get-all";
        HttpMethod httpMethod = HttpMethod.Get;
        JsonContent jsonBody = JsonContent.Create("");

        var response = await _authHeaderHttpClient.SendAsyncWithAuth(endpoint, httpMethod, jsonBody);

        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        return await response.Content.ReadFromJsonAsync<List<UserDTO>>();
    }

 
    
}