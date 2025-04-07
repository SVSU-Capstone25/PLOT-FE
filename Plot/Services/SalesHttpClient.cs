using System.Net;
using Plot.Services;

//NEED TO FINISH
public class SalesHttpClient : PlotHttpClient
{
    public SalesHttpClient(HttpContextAccessor httpContextAccessor) : base(httpContextAccessor, "/sales")
    { }


    public async Task<HttpStatusCode> UploadSales(int floorsetId, MultipartFormDataContent excelFile)
    {

        //return await SendPostAsync<HttpStatusCode>($"/upload-sales/{floorsetId}", excelFile);
        
        JsonContent body = JsonContent.Create(excelFile);

        return await SendPostAsync<HttpStatusCode>("",body);
    }
}