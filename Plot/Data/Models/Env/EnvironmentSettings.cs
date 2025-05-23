/*
    Filename: EnvironmentSettings.cs
    Part of Project: PLOT/PLOT-BE/Plot/Data/Models/Environment

    File Purpose:
    This file defines the EnvironmentSettings record,
    defining the settings for the environment that will
    be used to configure the settings of the environment.

    Written by: Jordan Houlihan
    Updated by: Michael Polhill
*/

namespace Plot.Data.Models.Env;

public class EnvironmentSettings
{
    public readonly string audience;
    public readonly string issuer;
    public readonly string databaseConnection;
    public readonly string email_pass;
    public readonly string auth_secret_key;
    public readonly string auth_expiration_time;
    public readonly string password_reset_secret_key;
    public readonly string password_reset_expiration_time;

    /// <summary>
    /// Class constructor to get env variables from the .env file
    /// and set the classes variables to be used in the application.
    /// </summary>
    public EnvironmentSettings()
    {
        audience = Environment.GetEnvironmentVariable("AUDIENCE")!;
        issuer = Environment.GetEnvironmentVariable("ISSUER")!;
        databaseConnection = Environment.GetEnvironmentVariable("DB_CONNECTION")!;
        email_pass = Environment.GetEnvironmentVariable("EMAIL_PASS")!;
        auth_secret_key = Environment.GetEnvironmentVariable("AUTH_SECRET_KEY")!;
        auth_expiration_time = Environment.GetEnvironmentVariable("AUTH_EXPIRATION_TIME")!;
        password_reset_secret_key = Environment.GetEnvironmentVariable("PASSWORD_RESET_SECRET_KEY")!;
        password_reset_expiration_time = Environment.GetEnvironmentVariable("PASSWORD_RESET_EXPIRATION_TIME")!;
    }
}