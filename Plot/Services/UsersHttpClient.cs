
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


    public async Task<UserDTO?> GetUserById(int userId)
    {
        string endpoint = $"/get-users-by-id/{userId}";
        HttpMethod httpMethod = HttpMethod.Get;
        JsonContent jsonBody = JsonContent.Create("");

        var response = await _authHeaderHttpClient.SendAsyncWithAuth(endpoint, httpMethod, jsonBody);

        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        return await response.Content.ReadFromJsonAsync<UserDTO>();
    }


    public async Task<HttpResponseMessage> UpdateUserPublicInfo(int userId, UpdatePublicInfoUser user)
    {
        string endpoint = $"/public-info/{userId}";
        HttpMethod httpMethod = HttpMethod.Patch;
        JsonContent jsonBody = JsonContent.Create(user);

        var response = await _authHeaderHttpClient.SendAsyncWithAuth(endpoint, httpMethod, jsonBody);

        return response;
    }


    public async Task<HttpResponseMessage> DeleteUserById(int userId)
    {
        string endpoint = $"/delete-user/{userId}";
        HttpMethod httpMethod = HttpMethod.Delete;
        JsonContent jsonBody = JsonContent.Create("");

        var response = await _authHeaderHttpClient.SendAsyncWithAuth(endpoint, httpMethod, jsonBody);

        return response;
    }


    public async Task<HttpResponseMessage> DeleteFromStore(DeleteUserFromStoreRequest deleteUserFromStoreRequest)
    {
        string endpoint = $"/delete-user-from-store";
        HttpMethod httpMethod = HttpMethod.Delete;
        JsonContent jsonBody = JsonContent.Create("");

        var response = await _authHeaderHttpClient.SendAsyncWithAuth(endpoint, httpMethod, jsonBody);

        return response;
    }


    public async Task<Store?> GetStoreOfUserById(int userId)
    {
        string endpoint = $"/stores/{userId}";
        HttpMethod httpMethod = HttpMethod.Get;
        JsonContent jsonBody = JsonContent.Create("");

        var response = await _authHeaderHttpClient.SendAsyncWithAuth(endpoint, httpMethod, jsonBody);

        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        return await response.Content.ReadFromJsonAsync<Store>();
    }
}