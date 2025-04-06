
using Plot.Data.Models.Auth.Registration;
using Plot.Data.Models.Auth.ResetPassword;
using Plot.Data.Models.Fixtures;
using Plot.Data.Models.Stores;
using Plot.Data.Models.Users;

//NEED TO FINISH
public class SalesHttpClient
{
    private const string BASE_CONTROLLER_ADDRESS="/Sales" ;

    private readonly AuthHeaderHttpClient _authHeaderHttpClient;


    public SalesHttpClient(AuthHeaderHttpClient authHeaderHttpClient)
    {
        _authHeaderHttpClient = authHeaderHttpClient;
        _authHeaderHttpClient.AppendBaseAddress(BASE_CONTROLLER_ADDRESS);
    }


    public async Task<HttpResponseMessage> UploadSales(int floorsetId, IFormFile excelFile)
    {
        string endpoint = "";
        HttpMethod httpMethod = HttpMethod.Post;
        JsonContent jsonBody = JsonContent.Create(excelFile);

        var response = await _authHeaderHttpClient.SendAsyncWithAuth(endpoint, httpMethod, jsonBody);

        return response;
    }

    
    
}