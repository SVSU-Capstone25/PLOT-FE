/*
    Filename: ResetPasswordRequest.cs
    Part of Project: PLOT/PLOT-FE/Plot/Data/Models/Auth/ResetPassword

    File Purpose:
    This file defines the ResetPasswordRequest model class, 
    to send a Email address to the api to send an email for a password reset.

    Class Purpose:
    This record serves as a model for data to for password reset request.
    The email address is used to send a password reset email.

    Written by: Michael Polhill
*/

using System.ComponentModel.DataAnnotations;

namespace Plot.Data.Models.Auth.ResetPassword;

public record ResetPasswordRequest
{
    // VARIABLES -- VARIABLES -- VARIABLES -- VARIABLES -- VARIABLES ------

    // Email address of the user requesting a password reset.
    [Required]
    [EmailAddress]
    public string? EMAIL { get; set; }
}