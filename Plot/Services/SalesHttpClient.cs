using System.Net;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Components.Forms;
using Plot.Data.Models.Allocations;
using Plot.Services;

//NEED TO FINISH
public class SalesHttpClient : PlotHttpClient
{
    public SalesHttpClient(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor, "/sales")
    { }


    public async Task<HttpStatusCode> UploadSales(int floorsetId, UploadFile excelFile)
    {




        return await SendPostContentAsync<HttpStatusCode>($"/upload-sales/{floorsetId}", excelFile);
    }

    public async Task<List<AllocationFulfillments>?> GetAllocationFulfillments(int floorsetId)
    {
        var response = await SendGetAsync<List<AllocationFulfillments>>($"/allocation-fulfillments/{floorsetId}");

        return response;
    }
}