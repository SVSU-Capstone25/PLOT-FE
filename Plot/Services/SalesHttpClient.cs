using System.Net;
using Plot.Data.Models.Allocations;
using Plot.Services;

//NEED TO FINISH
public class SalesHttpClient : PlotHttpClient
{
    public SalesHttpClient(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor, "/sales")
    { }


    public async Task<HttpStatusCode> UploadSales(int floorsetId, IFormFile excelFile)
    {
        JsonContent body = JsonContent.Create(excelFile);

        return await SendPostAsync<HttpStatusCode>("", body);
    }

    public async Task<List<AllocationFulfillments>?> GetAllocationFulfillments(int floorsetId)
    {
        var response = await SendGetAsync<List<AllocationFulfillments>>($"/allocation-fulfillments/{floorsetId}");

        return response;
    }
}