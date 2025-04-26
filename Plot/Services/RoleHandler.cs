using Microsoft.AspNetCore.Authorization;
using Plot.Data.Models.Users;
using Plot.Services;

public class RoleHandler : AuthorizationHandler<RoleRequirement>, IAuthorizationHandler
{
    private readonly AuthHttpClient _authHttpClient;

    private readonly IHttpContextAccessor _httpContextAccessor;


    public RoleHandler(AuthHttpClient authHttpClient, ICookie cookie, IHttpContextAccessor httpContextAccessor)
    {
        _authHttpClient = authHttpClient;
        _httpContextAccessor = httpContextAccessor;
    }


    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, RoleRequirement requirement)
    {

        var currentUser = await _authHttpClient.GetCurrentUser();

        if(currentUser!=null)
        {
            if(requirement.AllowedRoles.Contains(currentUser.ROLE))
            {
                context.Succeed(requirement);
                return;
            }
        }
        else
        {
            var httpContext = _httpContextAccessor.HttpContext;

            if(httpContext!=null)
            {
                httpContext.Response.Cookies.Append("Auth", "", new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddDays(-1),
                });

                httpContext.Response.Redirect("/login");
            }
        }
        context.Fail();
        return;
    }
}
