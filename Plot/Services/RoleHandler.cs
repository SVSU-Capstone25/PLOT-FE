using Microsoft.AspNetCore.Authorization;
using Plot.Data.Models.Users;
using Plot.Services;

public class RoleHandler : AuthorizationHandler<RoleRequirement>, IAuthorizationHandler
{
    private readonly AuthHttpClient _authHttpClient;

    //private readonly Cookie _cookie;

    //private readonly JwtService _jwtService;

    //private readonly ClaimParserService _claimParserService;

    public RoleHandler(AuthHttpClient authHttpClient)
    {
        _authHttpClient = authHttpClient;
        //_cookie=cookie;
        //_jwtService=jwtService;
        //_claimParserService=claimParserService;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, RoleRequirement requirement)
    {

        UserDTO? currentUser = await _authHttpClient.GetCurrentUser();

        if(currentUser!=null && requirement.AllowedRoles.Contains(currentUser.ROLE))
        {
            Console.WriteLine("ROLE GOOD");
            context.Succeed(requirement);
        }else
        {
            Console.WriteLine("ROLE BAD");
            context.Fail();
        }


        //var token = await _cookie.GetValue("Auth");

        // if(String.IsNullOrEmpty(token))
        // {
        //     var userPrincipal = _jwtService.ValidateAuthToken(token);

        //     if(userPrincipal!=null)
        //     {
        //         var currentUserTUID = _claimParserService.GetUserId(userPrincipal);

        //         var userInDb = _usersHttpClient.GetUserById(currentUserTUID.Value);

        //         if(userInDb!=null && requirement.AllowedRoles.Contains(userInDb.ROLE))


        //     }

        // }

        // var email = context.User.Identity?.Name; // or ClaimTypes.Email

        // if (string.IsNullOrEmpty(email))
        //     return;

        // var user = await _usersHttpClient.GetUserByEmail(email); // Call DB every time

        // if (user != null && requirement.AllowedRoles.Contains(user.ROLE))
        // {
        //     context.Succeed(requirement);
        // }
    }
}
