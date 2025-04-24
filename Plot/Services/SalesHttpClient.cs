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

        var (status, response) = await SendPostAsync<HttpStatusCode>("", body);

        return status;
    }

    public async Task<List<AllocationFulfillments>?> GetAllocationFulfillments(int floorsetId)
    {
        var response = await SendGetAsync<List<AllocationFulfillments>>($"/allocation-fulfillments/{floorsetId}");

        return response;
    }

    public async Task<HttpStatusCode> InsertSales(CreateExcelFileModel ExcelFileModel){
        JsonContent body = JsonContent.Create(ExcelFileModel);
        var (status, response) = await SendPostAsync<HttpStatusCode>($"/insert-sales", body);

        return status;
    }

    public async Task<List<CreateExcelFileModel>?> GetSalesByFloorset(int floorsetId)
    {
        return await SendGetAsync<List<CreateExcelFileModel>?>($"/get-sales-by-floorset/{floorsetId}");
    }

}