/*
    Filename: Store.cs
    Part of Project: PLOT/PLOT-BE/Plot/Data/Models/Stores

    File Purpose:
    This file contains the store object model.
    
    Class Purpose:
    This record is used as the main model
    for stores in the application. This will
    look the same as the schema in the database.

    Written by: Jordan Houlihan
*/

using System.ComponentModel.DataAnnotations;
namespace Plot.Data.Models.Stores;

public class Store
{
    [Range(1, int.MaxValue, ErrorMessage = "TUID must be an integer")]
    public required int TUID { get; set; }

    [Required]
    [StringLength(100, ErrorMessage = "Store name cannot exceed 100 characters.")]
    public string? NAME { get; set; }

    [Required]
    [StringLength(100, ErrorMessage = "Address cannot exceed 100 characters.")]
    public string? ADDRESS { get; set; }

    [Required]
    [StringLength(100, ErrorMessage = "City cannot exceed 100 characters.")]
    public string? CITY { get; set; }

    [Required]
    [StringLength(25, ErrorMessage = "State cannot exceed 25 characters.")]
    public string? STATE { get; set; }

    [Required]
    [StringLength(10, ErrorMessage = "Zip code cannot exceed 10 characters.")]
    public string? ZIP { get; set; }

    [Required]
    [Range(int.MinValue, int.MaxValue, ErrorMessage = "Width must be an integer.")]
    public required int WIDTH { get; set; }

    [Required]
    [Range(int.MinValue, int.MaxValue, ErrorMessage = "Length must be an integer.")]
    public required int LENGTH { get; set; }
    public string? USER_TUIDS { get; set; }
    public byte[]? BLUEPRINT_IMAGE { get; set; }
}