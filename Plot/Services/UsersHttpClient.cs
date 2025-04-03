
using Plot.Data.Models.Auth.Registration;
using Plot.Data.Models.Auth.ResetPassword;
using Plot.Data.Models.Fixtures;
using Plot.Data.Models.Stores;
using Plot.Data.Models.Users;

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

        return await response.Content.ReadFromJsonAsync<List<Store>>();
    }

 
    //THE CONTROLLER IS NOT COMPLETE FOR THIS ONE !!BE WARNED!!
    // [HttpGet("access/{userId:int}")]
    public async Task<List<Store>?> GetStoreAccessByUserId(int userId)
    {
        string endpoint = $"/access/{userId}";
        HttpMethod httpMethod = HttpMethod.Get;
        JsonContent jsonBody = JsonContent.Create("");

        var response = await _authHeaderHttpClient.SendAsyncWithAuth(endpoint, httpMethod, jsonBody);

        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        return await response.Content.ReadFromJsonAsync<List<Store>>();
    }
        


    // [HttpPost] I THINK THIS ONE NEEDS A NEW PATH
    public async Task<Store?> CreateStore(CreateStore store)
    {
        string endpoint ="";
        HttpMethod httpMethod = HttpMethod.Post;
        JsonContent jsonBody = JsonContent.Create(store);

        var response = await _authHeaderHttpClient.SendAsyncWithAuth(endpoint, httpMethod, jsonBody);

        if (!response.IsSuccessStatusCode)
        {
            return null;
        }
        
        return await response.Content.ReadFromJsonAsync<Store>();
    }


    //[HttpPatch("public-info/{storeId:int}")]
    public async Task<Store?> UpdatePublicInfo(int storeId, UpdatePublicInfoStore store)
    {
        string endpoint =$"/public-info/{storeId}";
        HttpMethod httpMethod = HttpMethod.Patch;
        JsonContent jsonBody = JsonContent.Create(store);

        var response = await _authHeaderHttpClient.SendAsyncWithAuth(endpoint, httpMethod, jsonBody);

        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        
        return await response.Content.ReadFromJsonAsync<Store>();
    }

    //[HttpPatch("size/{storeId:int}")]
    public async Task<Store?> UpdatePublicInfo(int storeId, UpdateSizeStore store)
    {
        string endpoint =$"/size/{storeId}";
        HttpMethod httpMethod = HttpMethod.Patch;
        JsonContent jsonBody = JsonContent.Create(store);

        var response = await _authHeaderHttpClient.SendAsyncWithAuth(endpoint, httpMethod, jsonBody);

        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        
        return await response.Content.ReadFromJsonAsync<Store>();
    }


   
    //[HttpDelete("{storeId:int}")]
    public async Task<HttpResponseMessage> DeleteStore(int storeId)
    {
        string endpoint =$"/{storeId}";
        HttpMethod httpMethod = HttpMethod.Delete;
        JsonContent jsonBody = JsonContent.Create("");

        var response = await _authHeaderHttpClient.SendAsyncWithAuth(endpoint, httpMethod, jsonBody);
        
        return response;
    }
}