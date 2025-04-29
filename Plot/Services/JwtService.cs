using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Plot.Data.Models.Users;
using Plot.Data.Models.Env;

namespace Plot.Services;

/// <summary>
/// Filename: JwtService.cs
/// Part of Project: PLOT/PLOT-FE/Plot/Services
///
/// File Purpose:
/// This file defines the JwtService class, responsible 
/// validating JSON Web Tokens (JWT) for authorization.
///
/// Class Purpose:
/// This class validates a jwt and returns a users
/// claims. Used to validate and get claims from the jwt
/// in the auth cookie.
///
/// Written by: Michael Polhill
/// </summary>
public class JwtService
{
    // VARIABLES -- VARIABLES -- VARIABLES -- VARIABLES -- VARIABLES ------

    // Stores token issuer from env.
    private readonly string _issuer;

    // Stores token audience from env.
    private readonly string _audience;

    // Stores secret key for signing tokens
    private readonly string _authSecretKey;

    private readonly string _passwordResetSecretKey;

    // Injected class to get env settings
    private readonly EnvironmentSettings _envSettings;


    // METHODS -- METHODS -- METHODS -- METHODS -- METHODS ------

    /// <summary>
    // Constructor, local vars are set using env during
    // injection.
    /// </summary>
    /// <param name="envSettings">Service to get vars from env</param>
    public JwtService(EnvironmentSettings envSettings)
    {
        _envSettings = envSettings;
        _audience = _envSettings.audience;
        _issuer = _envSettings.issuer;
        _authSecretKey = _envSettings.auth_secret_key;
        _passwordResetSecretKey = _envSettings.password_reset_secret_key;
    }


    /// <summary>
    /// Method to validate a jwt auth token
    /// </summary>
    /// <param name="token">Auth jwt token as string</param>
    /// <returns>Validated claims principal</returns>
    public ClaimsPrincipal? ValidateAuthToken(string token)
    {
       return ValidateToken(token, _authSecretKey);
    }


    /// <summary>
    /// Method to validate a jwt password reset token
    /// </summary>
    /// <param name="token">Password reset token jwt token as string</param>
    /// <returns>Validated claims principal</returns>
    public ClaimsPrincipal? ValidatePasswordResetToken(string token)
    {
       return ValidateToken(token, _passwordResetSecretKey);
    }

    
    /// <summary>
    /// This method validates a JWT token and returns the user's Claims principal if
    /// valid.
    /// </summary>
    /// <param name="token">Jwt token to validate</param>
    /// <param name="secretKey">Key for validation</param>
    /// <returns>Claims principal or null if validation fails</returns>
    private ClaimsPrincipal? ValidateToken(string token, string secretKey)
    {
        // Token handler to validate the token.
        var tokenHandler = new JwtSecurityTokenHandler();

        // Convert applications secret key into a byte array
        // to create a new symmetric security key to match
        // against the incoming token.
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(secretKey));

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
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
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