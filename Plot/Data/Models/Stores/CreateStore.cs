/*
    Filename: CreateStore.cs
    Part of Project: PLOT/PLOT-BE/Plot/Data/Models/Stores

    File Purpose:
    This file contains the store object model tied
    to creating a store.
    
    Class Purpose:
    This record is used as a model for frontend requests
    that create a store.
    
    Written by: Jordan Houlihan
*/

using System.ComponentModel.DataAnnotations;

namespace Plot.Data.Models.Stores;

public record CreateStore
{
    [Required]
    [StringLength(100, ErrorMessage = "Store name cannot exceed 100 characters.")]
    public required string NAME { get; set; }

    [Required]
    [StringLength(100, ErrorMessage = "Address cannot exceed 100 characters.")]
    public required string ADDRESS { get; set; }

    [Required]
    [StringLength(100, ErrorMessage = "City cannot exceed 100 characters.")]
    public required string CITY { get; set; }

    [Required]
    [StringLength(25, ErrorMessage = "State cannot exceed 25 characters.")]
    public required string STATE { get; set; }

    [Required]
    [StringLength(10, ErrorMessage = "Zip code cannot exceed 10 characters.")]
    public required string ZIP { get; set; }

    [Required]
    [Range(int.MinValue, int.MaxValue, ErrorMessage = "Width must be an integer.")]
    public required int WIDTH { get; set; }

    [Required]
    [Range(int.MinValue, int.MaxValue, ErrorMessage = "Length must be an integer.")]
    public required int LENGTH { get; set; }

    public required string USER_TUIDS { get; set; }
    public byte[]? BLUEPRINT_IMAGE { get; set; }
}