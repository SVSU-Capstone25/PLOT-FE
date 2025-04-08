using System.Net;
using Plot.Data.Models.Stores;
using Plot.Services;

public class StoresHttpClient : PlotHttpClient
{
    public StoresHttpClient(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor, "/stores")
    { }

    public async Task<List<Store>?> GetListOfStores()
    {
        return await SendGetAsync<List<Store>>("/get-all");
    }

    public async Task<Store?> GetStore(int storeId)
    {
        return await SendGetAsync<Store>($"/get-store/{storeId}");
    }

    public async Task<List<Store>?> GetStoreAccessByUserId(int userId)
    {
        return await SendGetAsync<List<Store>>($"/access/{userId}");
    }

    public async Task<Store?> CreateStore(CreateStore store)
    {
        JsonContent body = JsonContent.Create(store);

        return await SendPostAsync<Store>($"/create-store", body);
    }


    public async Task<HttpStatusCode> UpdatePublicInfo(int storeId, UpdatePublicInfoStore store)
    {
        JsonContent body = JsonContent.Create(store);

        return await SendPatchAsync($"/public-info/{storeId}", body);
    }

    public async Task<HttpStatusCode> UpdateStoreSize(int storeId, UpdateSizeStore store)
    {
        JsonContent body = JsonContent.Create(store);

        return await SendPatchAsync($"/size/{storeId}", body);
    }

    public async Task<HttpStatusCode> DeleteStore(int storeId)
    {
        return await SendDeleteAsync($"/{storeId}");
    }
}