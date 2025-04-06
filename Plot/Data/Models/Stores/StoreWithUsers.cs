/*
    Filename: Store.cs
    Part of Project: PLOT/PLOT-BE/Plot/Data/Models/Stores

    File Purpose:
    This file contains the store object model with users.
    
    Class Purpose:
    This record is used as the main model
    for stores in the application. This will
    look the same as the schema in the database, but with a list of added users in that store.

    Written by: Krzysztof Hejno
*/

using System.ComponentModel.DataAnnotations;
using Plot.Data.Models.Users;
namespace Plot.Data.Models.Stores;

public class StoreWithUsers
{
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "TUID must be an integer")]
    public int? TUID { get; set; }

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
    public int WIDTH { get; set; }

    [Required]
    [Range(int.MinValue, int.MaxValue, ErrorMessage = "Length must be an integer.")]
    public int LENGTH { get; set; }
    public byte[]? BLUEPRINT_IMAGE { get; set; }
    public List<UserDTO>? Users {get;set;}
}