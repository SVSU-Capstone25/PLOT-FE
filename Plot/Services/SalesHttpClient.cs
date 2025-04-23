using System.Net;
using Plot.Services;

//NEED TO FINISH
public class SalesHttpClient : PlotHttpClient
{
    public SalesHttpClient(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor, "/sales")
    { }


    public async Task<HttpStatusCode> UploadSales(int floorsetId, IFormFile excelFile)
    {
        JsonContent body = JsonContent.Create(excelFile);

        var (status, response) = await SendPostAsync<HttpStatusCode>("", body);

        return status;
    }

    public async Task<List<AllocationFulfillments>?> GetAllocationFulfillments(int floorsetId)
    {
        var response = await SendGetAsync<List<AllocationFulfillments>>($"/allocation-fulfillments/{floorsetId}");

        return response;
    }
}