using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Plot.Data.Models.Users;
using Plot.Data.Models.Env;

namespace Plot.Services;


public class JwtService
{
    // VARIABLES -- VARIABLES -- VARIABLES -- VARIABLES -- VARIABLES ------

    // Stores token issuer from settings.
    private readonly string _issuer;

    // Stores token audience from settings.
    private readonly string _audience;

    // Stores secret key for signing tokens
    private readonly string _authSecretKey;

    private readonly EnvironmentSettings _envSettings;

    public JwtService(EnvironmentSettings envSettings)
    {
        _envSettings = envSettings;
        _audience = _envSettings.audience;
        _issuer = _envSettings.issuer;
        _authSecretKey = _envSettings.auth_secret_key;
    }

    /// <summary>
    /// This method validates a JWT token and returns the user's email if
    /// valid. Used to validate the token in the password reset url.
    /// </summary>
    /// <param name="token">Jwt token to validate</param>
    /// <returns>Valid Token: Users email. Else: null</returns>
    public ClaimsPrincipal? GetValidClaims(string token)
    {
        // Token handler to validate the token.
        var tokenHandler = new JwtSecurityTokenHandler();

        // Convert applications secret key into a byte array
        // to create a new symmetric security key to match
        // against the incoming token.
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_authSecretKey));

        // Create validation parameters to match the
        // incoming token.
        var validationParameters = new TokenValidationParameters
        {
            // Ensure the incoming tokens signing key is valid.
            ValidateIssuerSigningKey = true,
            // Match the incoming tokens signing key to the applications
            // secret key.
            IssuerSigningKey = key,
            // Check the tokens issuer and audience.
            ValidateIssuer = true,
            ValidIssuer = _issuer,
            ValidateAudience = true,
            ValidAudience = _audience,
            // Make sure the token is not expired.
            ValidateLifetime = true
        };

        try
        {
            // Validate the token based on the validation parameters. out_ 
            // is used to ignore the security token as it is not needed.
            var principal = tokenHandler.ValidateToken(
                token, validationParameters, out _);

            return principal;
        }
        catch
        {
            // Return null if the token is invalid.
            return null;
        }
    }
}