
using System.Net;
using Plot.Data.Models.Stores;
using Plot.Data.Models.Users;
using Plot.Services;
using System.Text.Json;

//NEED TO FINISH
public class UsersHttpClient : PlotHttpClient
{
    public UsersHttpClient(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor, "/users")
    { }

    public async Task<List<UserDTO>?> GetAllUsers()
    {
        return await SendGetAsync<List<UserDTO>>("/get-all");
    }

    public async Task<UserDTO?> GetUserById(int userId)
    {
        return await SendGetAsync<UserDTO>($"/get-users-by-id/{userId}");
    }

    public async Task<HttpStatusCode> UpdateUserPublicInfo(int userId, UpdatePublicInfoUser user)
    {
        Console.WriteLine("UpdateUserPublicInfo function in UsersHttpClient.razor");

        // Serialize to string so you can see the actual JSON content
        string jsonString = JsonSerializer.Serialize(user, new JsonSerializerOptions { WriteIndented = true });
        Console.WriteLine("JSON body in UpdateUserPublicInfo in UsersHttpClient is:");
        Console.WriteLine("It is:\n" + jsonString);

        JsonContent body = JsonContent.Create(user);
        Console.WriteLine("JSON body in UpdateUserPublicInfo in UsersHttpClient is");
        Console.WriteLine(body);

        HttpStatusCode response = await SendPatchAsync($"/public-info/{userId}", body);
        Console.WriteLine($"Response: {response}");

        return response;


        //return await SendPatchAsync($"/public-info/{userId}", body);
    }

    public async Task<HttpStatusCode> DeleteUserById(int userId)
    {
        return await SendDeleteAsync($"/delete-user/{userId}");
    }

    public async Task<HttpStatusCode> DeleteFromStore(DeleteUserFromStoreRequest deleteUserFromStoreRequest)
    {
        JsonContent body = JsonContent.Create(deleteUserFromStoreRequest);

        return await SendPostAsync<HttpStatusCode>($"/delete-user-from-store", body);
    }

    public async Task<HttpStatusCode> AddUserToStore(AddUserToStoreRequest addUserToStoreRequest)
    {
        JsonContent body = JsonContent.Create(addUserToStoreRequest);

        return await SendPostAsync<HttpStatusCode>($"/add-user-to-store", body);
    }

    // TODO: Change to be a PUT request
    public async Task<HttpStatusCode> UpdateAccessList(UpdateAccessListRequest updateAccessListRequest)
    {
        Console.WriteLine("Starting UpdateAccessList(UpdateAccessListRequest updateAccessListRequest)");
        Console.WriteLine("updateAccessListRequest is");
        Console.WriteLine(updateAccessListRequest);
        JsonContent body = JsonContent.Create(updateAccessListRequest);

        return await SendPostAsync<HttpStatusCode>($"/update-access-list", body);
    }

    public async Task<List<Store>?> GetStoreOfUserById(int userId)
    {
        //Console.WriteLine("Attemping to get stores of user");
        var response = await SendGetAsync<List<Store>?>($"/stores/{userId}");
        //Console.WriteLine("RESPONSE is:\n" + response + "\n");
        return response;
    }

    public async Task<List<Store>?> GetStoresNotForUser(int userId)
    {
        //Console.WriteLine("Attemping to get stores not of user");
        return await SendGetAsync<List<Store>?>($"/stores-not/{userId}");
    }
}