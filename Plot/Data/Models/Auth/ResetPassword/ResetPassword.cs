/*
    Filename: ResetPassword.cs
    Part of Project: PLOT/PLOT-FE/Plot/Data/Models/Auth/ResetPassword
    
    File Purpose:
    This file defines the ResetPassword model class, 
    a NewPassword and Token.

    Class Purpose:
    This record serves as a model to send to the api for a reset users password.
    The new password and token are used to reset a users password.

    Written by: Michael Polhill
*/

using System.ComponentModel.DataAnnotations;

namespace Plot.Data.Models.Auth.ResetPassword;

public record ResetPassword
{
    // VARIABLES -- VARIABLES -- VARIABLES -- VARIABLES -- VARIABLES ------

    // New password for the user.
    
    public string? NewPassword { get; set; }

    // Token used when the link was generated to validate the password reset.
    public string? Token { get; set; }
}