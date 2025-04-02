using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;

public class CustomAuthStateProvider : AuthenticationStateProvider
{


    public CustomAuthStateProvider()
    {
        AuthenticationStateChanged += OnAuthenticationStateChangedAsync;
    }

    private async void OnAuthenticationStateChangedAsync(Task<AuthenticationState> task)
    {
        var authenticationState = await task;

        if (authenticationState is not null)
        {
            Console.WriteLine("ayyyooo, the freakin authstate aint null");
        }
    }

    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var testClaim = new ClaimsIdentity(
            new[]
            {
                new Claim("Email", "Email@email.com"),
                new Claim("Role", "1"),
                new Claim("UserId", "1")
            },
            authenticationType: "FakeAuth" 
        );
        var principal = new ClaimsPrincipal(testClaim);
        
        return Task.FromResult(new AuthenticationState(principal));
    }

    public void LoginAsync()
    {
        var testClaim = new ClaimsIdentity(
            new[]
            {
                new Claim("Email", "Email@email.com"),
                new Claim("Role", "1"),
                new Claim("UserId", "1")
            },
            authenticationType: "FakeAuth" 
        );

        var principal = new ClaimsPrincipal(testClaim);

        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(principal)));
    }
}