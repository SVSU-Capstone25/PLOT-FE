
using System.Net;
using Plot.Data.Models.Stores;
using Plot.Data.Models.Users;
using Plot.Services;

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

    public async Task<HttpResponseMessage?> UpdateUserPublicInfo(int userId, UpdatePublicInfoUser user)
    {
        JsonContent body = JsonContent.Create(user);

        return await SendPatchAsync<HttpResponseMessage>($"/public-info/{userId}", body);
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

    public async Task<Store?> GetStoreOfUserById(int userId)
    {
        return await SendGetAsync<Store>($"/stores/{userId}");
    }
}