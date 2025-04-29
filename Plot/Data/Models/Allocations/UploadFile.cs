/*
    Filename: UploadFile.cs
    Part of Project: PLOT/PLOT-FE/Plot/Data/Models/Allocations

    File Purpose:
    This file contains the request input for sending
    an file over. Its used to upload the razor excel file.
    
    Written by: Michael Polhill
*/

using Microsoft.AspNetCore.Components.Forms;

namespace Plot.Data.Models.Allocations;

public class UploadFile
{
    public Stream Stream { get; set; } = default!;
    public string FileName { get; set; } = string.Empty;
    public string ContentType { get; set; } = "application/octet-stream";
}