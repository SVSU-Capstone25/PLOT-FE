
using System.Net;
using Plot.Data.Models.Auth.Registration;
using Plot.Data.Models.Auth.ResetPassword;
using Plot.Data.Models.Fixtures;
using Plot.Data.Models.Floorsets;
using Plot.Services;

public class FloorsetsHttpClient : PlotHttpClient
{
    public FloorsetsHttpClient(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor, "/floorsets")
    { }

    public async Task<List<Floorset>?> GetFloorsetsByStore(int storeId)
    {
        return await SendGetAsync<List<Floorset>>($"/get-floorsets/{storeId}");
    }

    public async Task<Floorset?> GetFloorsetById(int floorsetId)
    {
        return await SendGetAsync<Floorset>($"/get-floorset/{floorsetId}");
    }

    // TODO: Update to return status code and tuid so that we can do validation
    public async Task<int> CreateFloorset(CreateFloorset newFloorset)
    {
        JsonContent body = JsonContent.Create(newFloorset);

        var (status, response) = await SendPostAsync<int>("/create-floorset", body);

        return response;
    }

    public async Task<HttpStatusCode> UpdatePublicInfo(int floorsetId, UpdatePublicInfoFloorset floorset)
    {
        JsonContent body = JsonContent.Create(floorset);

        return await SendPatchAsync($"/public-info/{floorsetId}", body);
    }

    public async Task<HttpStatusCode> DeleteFloorset(int floorsetId)
    {
        return await SendDeleteAsync($"/{floorsetId}");
    }

    public async Task<HttpStatusCode> CopyFloorset(FloorsetRef FloorsetRef)
    {   
        JsonContent body = JsonContent.Create(FloorsetRef);

        var (status, response) = await SendPostAsync<HttpStatusCode>($"/copy-floorset", body);

        return response;
    }
}