/*
    Filename: UploadFile.cs
    Part of Project: PLOT/PLOT-BE/Plot/Data/Models/Allocations

    File Purpose:
    This file contains the request input for sending
    an excel file over.
    
    Written by: Jordan Houlihan
*/

using Microsoft.AspNetCore.Components.Forms;

namespace Plot.Data.Models.Allocations;

public record UploadFile
{
    public MemoryStream? Stream { get; set; }
    public string? FileName { get; set; }
    public string? ContentType { get; set; }

}