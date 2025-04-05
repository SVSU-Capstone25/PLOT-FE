
using Plot.Data.Models.Auth.Registration;
using Plot.Data.Models.Auth.ResetPassword;
using Plot.Data.Models.Fixtures;

public class FixturesHttpClient
{
    private const string BASE_CONTROLLER_ADDRESS = "/Fixtures";

    private readonly AuthHeaderHttpClient _authHeaderHttpClient;



    public FixturesHttpClient(AuthHeaderHttpClient authHeaderHttpClient)
    {
        _authHeaderHttpClient = authHeaderHttpClient;
        _authHeaderHttpClient.AppendBaseAddress(BASE_CONTROLLER_ADDRESS);
    }


    /// <summary>
    /// This method gets all of a floorsets fixture information
    /// from the backend api.
    /// BE API:[HttpGet("get-fixtures/{floorsetId:int}")]
    /// </summary>
    /// <param name="floorsetId"></param>
    /// <returns>A floorsets fixture information or null if bad http response</returns>
    public async Task<List<FixtureInstance>?> GetFloorsetFixtureInformation(int floorsetId) //NOT SURE BACKEND API ONLY RETURNS ONE FixtureInstance
    {
        //End point for the Api
        string endpoint = $"/get-fixtures/{floorsetId}";
        //Set the http method corresponding with the endpoint
        HttpMethod httpMethod = HttpMethod.Get;
        //Dont need json content for this one just leave it blank. Prob change this later.
        JsonContent jsonBody = JsonContent.Create("");

        //Send in http request and wait for the information from the api
        var response = await _authHeaderHttpClient.SendAsyncWithAuth(endpoint, httpMethod, jsonBody);

        //If its a bad response return nothing
        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        //Return a floor sets fixtures information.
        return await response.Content.ReadFromJsonAsync<List<FixtureInstance>>();
    }


    /// <summary>
    /// This sends updates to a floorsets fixture information to the back end api.
    /// BE API:[HttpPatch("update-fixture/{floorsetId:int}")]
    /// </summary>
    /// <param name="floorsetId"> Current floorset</param>
    /// <param name="fixtures">A floorsets entire fixture information</param>
    /// <returns>Http response</returns>
    public async Task<HttpResponseMessage> UpdateFixtureInformation(int floorsetId, FixturesState fixtures)
    {
        //Set the api end point of the floorset you want to update
        string endpoint = $"/update-fixture/{floorsetId}";
        //Need to use http patch
        HttpMethod httpMethod = HttpMethod.Patch;
        //Put the fixtures information into json so the controller can receive the  object.
        JsonContent jsonBody = JsonContent.Create(fixtures);

        //wait for the response from the controller
        var response = await _authHeaderHttpClient.SendAsyncWithAuth(endpoint, httpMethod, jsonBody);

        //Send back the http response
        return response;
    }


    /// <summary>
    /// This method sends a new fixtures information to the backend api 
    /// for fixture creation.
    /// BE API:[HttpPatch("create-fixture/{storeId:int}")]
    /// </summary>
    /// <param name="storeId">Fixtures store</param>
    /// <param name="FixtureModel information"> </param>
    /// <returns>New fixture</returns>
    public async Task<FixtureModel?> CreateFixtureModel(int storeId, CreateFixtureModel fixtureModel)
    {
        //Set the api end point and the current store
        string endpoint = $"/create-fixture/{storeId}";
        //Http patch
        HttpMethod httpMethod = HttpMethod.Patch;
        //Create a json representation of the fixture model
        JsonContent jsonBody = JsonContent.Create(fixtureModel);

        //Send the http request and waith for response
        var response = await _authHeaderHttpClient.SendAsyncWithAuth(endpoint, httpMethod, jsonBody);

        //Return null if bad http response status
        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        //Return the new fixture.
        return await response.Content.ReadFromJsonAsync<FixtureModel>();
    }


    /// <summary>
    /// This method sends a models Id to a backend api for deletion.
    /// BE API:[HttpPatch("delete-model/{storeId:int}")]
    /// </summary>
    /// <param name="storeId">Store Id the model is from</param>
    /// <param name="modelId">A models id number</param>
    /// <returns>Http response</returns>
    public async Task<HttpResponseMessage> DeleteFixtureModel(int storeId, int modelId)
    {
        //Set api end point, include the store and model for deletion
        string endpoint = $"/delete-model/{storeId}?modelId={modelId}";
        //Set http to patch
        HttpMethod httpMethod = HttpMethod.Patch;
        //Dont need any Json data
        JsonContent jsonBody = JsonContent.Create("");

        //Wait for servers response
        var response = await _authHeaderHttpClient.SendAsyncWithAuth(endpoint, httpMethod, jsonBody);

        //Return the response
        return response;
    }


    /// <summary>
    /// This method sends 
    /// </summary>
    /// <param name="storeId"></param>
    /// <param name="update"></param>
    /// <returns></returns>
    public async Task<HttpResponseMessage> UpdateFixtureModel(int storeId, FixtureModel update)
    {
        string endpoint = $"/delete-store/{storeId}";
        HttpMethod httpMethod = HttpMethod.Patch;
        JsonContent jsonBody = JsonContent.Create(update);

        var response = await _authHeaderHttpClient.SendAsyncWithAuth(endpoint, httpMethod, jsonBody);

        return response;
    }
}