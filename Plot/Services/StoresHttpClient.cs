using Plot.Data.Models.Auth.Registration;
using Plot.Data.Models.Auth.ResetPassword;
using Plot.Data.Models.Fixtures;
using Plot.Data.Models.Stores;

public class StoresHttpClient
{
    private const string BASE_CONTROLLER_ADDRESS="/stores" ;

    private readonly AuthHeaderHttpClient _authHeaderHttpClient;



    public StoresHttpClient(AuthHeaderHttpClient authHeaderHttpClient)
    {
        _authHeaderHttpClient = authHeaderHttpClient;
        _authHeaderHttpClient.AppendBaseAddress(BASE_CONTROLLER_ADDRESS);
    }

    
    // [HttpGet]
    // 
    public async Task<List<Store>?> GetListOfStores()//WORKING 
    {
        string endpoint = "get-all";
        HttpMethod httpMethod = HttpMethod.Get;
        JsonContent jsonBody = JsonContent.Create("");

        var response = await _authHeaderHttpClient.SendAsyncWithAuth(endpoint, httpMethod, jsonBody);

        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        return await response.Content.ReadFromJsonAsync<List<Store>>();
    }

 
 
    // [HttpGet("access/{userId:int}")] 
    public async Task<List<Store>?> GetStoreAccessByUserId(int userId)//WORKING just need to change API endpoint return
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
        


    // [HttpPost]
    public async Task<Store?> CreateStore(CreateStore store) //WORKING just need to change API endpoint return
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
    public async Task<Store?> UpdatePublicInfo(int storeId, UpdatePublicInfoStore store)//NOT tested but assumed working
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
    public async Task<Store?> UpdateStoreSize(int storeId, UpdateSizeStore store)//NOT tested but assumed working
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
    public async Task<HttpResponseMessage> DeleteStore(int storeId)//Working
    {
        string endpoint =$"/{storeId}";
        HttpMethod httpMethod = HttpMethod.Delete;
        JsonContent jsonBody = JsonContent.Create("");

        var response = await _authHeaderHttpClient.SendAsyncWithAuth(endpoint, httpMethod, jsonBody);
        
        return response;
    }
}