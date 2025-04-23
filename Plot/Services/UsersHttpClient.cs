
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

    public async Task<UserDTO?> GetUserByEmail(string userEmail)
    {
        //Console.WriteLine("user email in client " + userEmail);
        var response = await SendGetAsync<UserDTO?>($"/get-user-by-email/{Uri.EscapeDataString(userEmail)}");
        //Console.WriteLine("The response in GetUserByEmail is " + response);
        return response;
    }

    public async Task<HttpStatusCode> UpdateUserPublicInfo(int userId, UpdatePublicInfoUser user)
    {
        JsonContent body = JsonContent.Create(user);

        return await SendPatchAsync($"/public-info/{userId}", body);
    }

    public async Task<HttpStatusCode> DeleteUserById(int userId)
    {
        return await SendDeleteAsync($"/delete-user/{userId}");
    }

    public async Task<HttpStatusCode> DeleteFromStore(DeleteUserFromStoreRequest deleteUserFromStoreRequest)
    {
        JsonContent body = JsonContent.Create(deleteUserFromStoreRequest);

        var (status, response) = await SendPostAsync<HttpStatusCode>($"/delete-user-from-store", body);

        return status;
    }

    public async Task<HttpStatusCode> AddUserToStore(AddUserToStoreRequest addUserToStoreRequest)
    {
        JsonContent body = JsonContent.Create(addUserToStoreRequest);

        var (status, response) = await SendPostAsync<HttpStatusCode>($"/add-user-to-store", body);

        return status;
    }

    // TODO: Change to be a PUT request
    public async Task<HttpStatusCode> UpdateAccessList(UpdateAccessListRequest updateAccessListRequest)
    {
        JsonContent body = JsonContent.Create(updateAccessListRequest);

        var (status, response) = await SendPostAsync<HttpStatusCode>($"/update-access-list", body);

        return status;
    }

    public async Task<IEnumerable<Store>?> GetStoreOfUserById(int userId)
    {
        return await SendGetAsync<IEnumerable<Store>?>($"/stores/{userId}");
    }

    public async Task<List<UserDTO>?> GetUsersByString(UsersByStringRequest usersByStringRequest)
    {
        JsonContent body = JsonContent.Create(usersByStringRequest);

        var (status, response) = await SendPostAsync<List<UserDTO>>("/get-users-by-string", body);

        return response;
    }

    public async Task<IEnumerable<Store>?> GetStoresNotForUser(int userId)
    {
        return await SendGetAsync<IEnumerable<Store>?>($"/stores-not/{userId}");
    }
}