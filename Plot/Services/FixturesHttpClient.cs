
using System.Net;
using Plot.Data.Models.Fixtures;
using Plot.Services;

public class FixturesHttpClient : PlotHttpClient
{
    public FixturesHttpClient(IHttpContextAccessor httpContextAccessor) :
    base(httpContextAccessor, "/fixtures")
    { }

    /// <summary>
    /// This method gets all of a floorsets fixture information
    /// from the backend api.
    /// BE API:[HttpGet("get-fixtures/{floorsetId:int}")]
    /// </summary>
    /// <param name="floorsetId"></param>
    /// <returns>A floorsets fixture information or null if bad http response</returns>
    public async Task<List<FixtureInstance>?> GetFloorsetFixtureInformation(int floorsetId)
    {
        return await SendGetAsync<List<FixtureInstance>>($"/get-fixtures-instances/{floorsetId}");
    }

    public async Task<List<FixtureModel>?> GetFixtureModelsByStore(int storeId)
    {
        return await SendGetAsync<List<FixtureModel>>($"/get-fixture-models/{storeId}");
    }

    /// <summary>
    /// This sends updates to a floorsets fixture information to the back end api.
    /// BE API:[HttpPatch("update-fixture/{floorsetId:int}")]
    /// </summary>
    /// <param name="floorsetId"> Current floorset</param>
    /// <param name="fixtures">A floorsets entire fixture information</param>
    /// <returns>Http response</returns>
    public async Task<HttpStatusCode> UpdateFixtureInformation(int floorsetId, Fixtures_State fixtures)
    {
        JsonContent body = JsonContent.Create(fixtures);

        return await SendPatchAsync($"/update-fixture/{floorsetId}", body);
    }


    /// <summary>
    /// This method sends a new fixtures information to the backend api 
    /// for fixture creation.
    /// BE API:[HttpPatch("create-fixture/{storeId:int}")]
    /// </summary>
    /// <param name="storeId">Fixtures store</param>
    /// <param name="FixtureModel information"> </param>
    /// <returns>New fixture</returns>
    public async Task<HttpStatusCode> CreateFixtureModel(int storeId, CreateFixtureModel fixtureModel)
    {
        JsonContent body = JsonContent.Create(fixtureModel);

        return await SendPostAsync<HttpStatusCode>($"/create-fixture-model/", body);
    }

    /// <summary>
    /// This method sends a models Id to a backend api for deletion.
    /// BE API:[HttpPatch("delete-model/{storeId:int}")]
    /// </summary>
    /// <param name="storeId">Store Id the model is from</param>
    /// <param name="modelId">A models id number</param>
    /// <returns>Http response</returns>
    public async Task<HttpStatusCode> DeleteFixtureModel(int modelId)
    {
        return await SendDeleteAsync($"/delete-model/{modelId}");
    }


    /// <summary>
    /// This method sends 
    /// </summary>
    /// <param name="storeId"></param>
    /// <param name="update"></param>
    /// <returns></returns>
    public async Task<HttpStatusCode> UpdateFixtureModel(int storeId, Select_Fixtures update)
    {
        JsonContent body = JsonContent.Create(update);

        return await SendPatchAsync($"/update-fixture-model/", body);
    }
}