
using Plot.Data.Models.Auth.Registration;
using Plot.Data.Models.Auth.ResetPassword;
using Plot.Data.Models.Fixtures;

public class FixturesHttpClient
{
    private const string BASE_CONTROLLER_ADDRESS="/Fixtures" ;

    private readonly AuthHeaderHttpClient _authHeaderHttpClient;



    public FixturesHttpClient(AuthHeaderHttpClient authHeaderHttpClient)
    {
        _authHeaderHttpClient = authHeaderHttpClient;
        _authHeaderHttpClient.AppendBaseAddress(BASE_CONTROLLER_ADDRESS);
    }

    
    // [HttpGet("get-fixtures/{floorsetId:int}")]
    // 
    public async Task<FloorsetFixtureInformation?> GetFloorsetFixtures(int floorsetId, int storeId)
    {
        string endpoint = $"/get-fixtures/{floorsetId}?storeId={storeId}";
        HttpMethod httpMethod = HttpMethod.Get;
        JsonContent jsonBody = JsonContent.Create("");

        var response = await _authHeaderHttpClient.SendAsyncWithAuth(endpoint, httpMethod, jsonBody);

        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        FloorsetFixtureInformation? floorsetFixtureInformation =
            await response.Content.ReadFromJsonAsync<FloorsetFixtureInformation>();

        return floorsetFixtureInformation;
    }

 
    //CONTROLLER NOT DONE NEED TO FINSISH-----------------------------------------
    // [HttpPatch("update-fixture/{floorsetId:int}")]
    public async Task<HttpResponseMessage> UpdateFixtureInformation(int floorsetId, int storeId, 
                                                                    UpdateFloorsetFixtureInformation floorsetFixtureInformation)
    {
        string endpoint ="";
        HttpMethod httpMethod = HttpMethod.Patch;
        JsonContent jsonBody = JsonContent.Create("");

        

        var response = await _authHeaderHttpClient.SendAsyncWithAuth(endpoint, httpMethod, jsonBody);

        
        
        return response;
    }
        


    // [HttpPatch("create-fixture/{storeId:int}")]
    public async Task<FixtureModel?> CreateFixtureModel(int storeId, CreateFixtureModel newFixtureModel)
    {
        string endpoint =$"/create-fixture/{storeId}";
        HttpMethod httpMethod = HttpMethod.Patch;
        JsonContent jsonBody = JsonContent.Create(newFixtureModel);

        var response = await _authHeaderHttpClient.SendAsyncWithAuth(endpoint, httpMethod, jsonBody);

        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        FixtureModel? fixtureModel = await response.Content.ReadFromJsonAsync<FixtureModel>();
        
        return fixtureModel;
    }


    //NOT SURE IF DON CONTROLLER NAME SEEMS WERID
    //[HttpPatch("delete-store/{storeId:int}")]
    public async Task<HttpResponseMessage> DeleteFixtureModel(int storeId, int modelId)
    {
        string endpoint =$"/delete-store/{storeId}?modelId={modelId}";
        HttpMethod httpMethod = HttpMethod.Patch;
        JsonContent jsonBody = JsonContent.Create("");

        var response = await _authHeaderHttpClient.SendAsyncWithAuth(endpoint, httpMethod, jsonBody);
        
        return response;
    }
}