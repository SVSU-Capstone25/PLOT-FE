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

public class UploadFile
{
    public Stream Stream { get; set; } = default!;
    public string FileName { get; set; } = string.Empty;
    public string ContentType { get; set; } = "application/octet-stream";
}