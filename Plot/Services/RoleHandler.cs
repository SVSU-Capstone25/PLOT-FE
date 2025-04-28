using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Plot.Data.Models.Env;
using Plot.Data.Models.Users;
using Plot.Services;

public class RoleHandler : AuthorizationHandler<RoleRequirement>, IAuthorizationHandler
{
    private readonly AuthHttpClient _authHttpClient;

    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly JwtService _jwtService;
    private readonly ClaimParserService _claimParserService;

    private readonly EnvironmentSettings _envSettings;

    private readonly NavigationManager _navigationManager;

    


    public RoleHandler(AuthHttpClient authHttpClient, NavigationManager navigationManager, IHttpContextAccessor httpContextAccessor, JwtService jwtService,ClaimParserService claimParserService, EnvironmentSettings environmentSettings)
    {
        _authHttpClient = authHttpClient;
        _httpContextAccessor = httpContextAccessor;
        _jwtService = jwtService;
        _claimParserService=claimParserService;
        _envSettings = environmentSettings;
        _navigationManager =  navigationManager;
    }


    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, RoleRequirement requirement)
    {
        var httpContext = _httpContextAccessor.HttpContext;

        if(httpContext!=null)
        {
             var token = httpContext.Request.Cookies["Auth"];

             var userInDatabase = await _authHttpClient.GetCurrentUser();

             if(userInDatabase!=null && token !=null)
             {
                var userPrinciple = _jwtService.ValidateAuthToken(token);
                
                if(userPrinciple!=null)
                {
                    var userRole = _claimParserService.GetRole(userPrinciple);

                    if(userRole != userInDatabase.ROLE)
                    {
                        
                       //_navigationManager.NavigateTo("/login?roleChange=true",true);
                            
                        //httpContext.Response.Redirect("/login?roleChange=true");
                    }
                    
                    if(requirement.AllowedRoles.Contains(userInDatabase.ROLE))
                    {
                        Console.WriteLine("Good auth");
                        context.Succeed(requirement);
                        return;
                    }
                }
                
             }else
             {
                // Console.WriteLine("Bad user delete cookie");
                // httpContext.Response.Cookies.Append("Auth", "", new CookieOptions
                // {
                //     Expires = DateTimeOffset.UtcNow.AddDays(-1),
                // });

                // httpContext.Response.Redirect("/login");
             }
        }
        Console.WriteLine("Bad auth");
        context.Fail();
        return;
    }
}
