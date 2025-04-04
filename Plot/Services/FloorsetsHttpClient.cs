
using Plot.Data.Models.Auth.Registration;
using Plot.Data.Models.Auth.ResetPassword;
using Plot.Data.Models.Fixtures;
using Plot.Data.Models.Floorsets;

public class FloorsetsHttpClient
{
    private const string BASE_CONTROLLER_ADDRESS="/floorsets" ;

    private readonly AuthHeaderHttpClient _authHeaderHttpClient;



    public FloorsetsHttpClient(AuthHeaderHttpClient authHeaderHttpClient)
    {
        _authHeaderHttpClient = authHeaderHttpClient;
        _authHeaderHttpClient.AppendBaseAddress(BASE_CONTROLLER_ADDRESS);
    }

    
    //  [HttpGet("get-floorsets/{storeId:int}")]
    // 
    public async Task<List<Select_Floorset>?> GetFloorsetsByStore(int storeId)
    {
        string endpoint = $"/get-floorsets/{storeId}";
        HttpMethod httpMethod = HttpMethod.Get;
        JsonContent jsonBody = JsonContent.Create("");

        var response = await _authHeaderHttpClient.SendAsyncWithAuth(endpoint, httpMethod, jsonBody);

        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        return await response.Content.ReadFromJsonAsync<List<Select_Floorset>>();
    }

 
    //[HttpPost]THERE IS NO ENDPOINT ON THIS ONE---------------------------------------------------------------------------------
    public async Task<Select_Floorset?> CreateFloorset(Select_Floorset newFloorset)
    {
        string endpoint ="";
        HttpMethod httpMethod = HttpMethod.Post;
        JsonContent jsonBody = JsonContent.Create(newFloorset);

        

        var response = await _authHeaderHttpClient.SendAsyncWithAuth(endpoint, httpMethod, jsonBody);

        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

    
        return await response.Content.ReadFromJsonAsync<Select_Floorset>();
    }
        


    // [HttpPatch("public-info/{floorsetId:int}")]
    public async Task<Select_Floorset?> UpdatePublicInfo(int floorsetId, Select_Floorset floorset)
    {
        string endpoint =$"/public-info/{floorsetId}";
        HttpMethod httpMethod = HttpMethod.Patch;
        JsonContent jsonBody = JsonContent.Create(floorset);

        var response = await _authHeaderHttpClient.SendAsyncWithAuth(endpoint, httpMethod, jsonBody);

        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        
        return await response.Content.ReadFromJsonAsync<Select_Floorset>();
    }


   
    //[HttpDelete("{floorsetId:int}")]
    public async Task<HttpResponseMessage> DeleteFloorset(int floorsetId)
    {
        string endpoint =$"/{floorsetId}";
        HttpMethod httpMethod = HttpMethod.Delete;
        JsonContent jsonBody = JsonContent.Create("");

        var response = await _authHeaderHttpClient.SendAsyncWithAuth(endpoint, httpMethod, jsonBody);
        
        return response;
    }
}